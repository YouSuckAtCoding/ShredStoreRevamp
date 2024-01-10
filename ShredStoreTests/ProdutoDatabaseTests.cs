using Application.Models;
using Dapper;
using FluentAssertions;
using ShredStoreTests.DataAdapterFiles;
using ShredStoreTests.DataAdapterFiles.ProdutoTestFiles;
using ShredStoreTests.DataAdapterFiles.UsuarioTestFiles;

namespace ShredStoreTests
{
    public class ProdutoDatabaseTests : IClassFixture<SqlAccessFixture>
    {
        private readonly ISqlAccessConnectionFactory _dbConnectionFactory;
        public ProdutoDatabaseTests(SqlAccessFixture fixture)
        {
            _dbConnectionFactory = fixture.ConnectionFactory;
        }

        [Fact]
        public void Should_Throw_ArgumentNullException_If_Missing_Connection_String()
        {
            var create = () => new ProdutoStorage(null!);
            create.Should().ThrowExactly<ArgumentNullException>();
        }
        [Theory]
        [InlineData("spProduto_Insert")]
        [InlineData("spProduto_GetAll")]
        [InlineData("spProduto_GetById")]
        [InlineData("spProduto_Delete")]
        [InlineData("spProduto_Update")]
        public async Task Should_Be_True_If_Produto_Stored_Procedure_Exists(string Sp)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            string spName = Sp;
            Utility utility = new Utility();
            dynamic res = await connection.QueryAsync(utility.CreateQueryForStoredProcedureCheck(spName));
            string name = res[0].name;
            name.Should().Be(spName);
        }
        [Fact]
        public async Task Should_Insert_Produto_If_Doesnt_Exist()
        {
            Produto prod = Fake.FakeDataFactory.FakeProdutos();
            IProdutoStorage storage = new ProdutoStorage(_dbConnectionFactory);
            await storage.InsertProduto(prod);
            var res = await storage.GetProdutos();
            res.Should().HaveCountGreaterThan(0);
            await CleanUpProdutos(_dbConnectionFactory);

        }
        [Fact]
        public async Task Should_Be_Empty_If_No_Produto_Exists()
        {
            IProdutoStorage storage = new ProdutoStorage(_dbConnectionFactory);
            var res = await storage.GetProdutos();
            res.Should().BeEmpty();
            await CleanUpProdutos(_dbConnectionFactory);
        }
        [Fact]
        public async Task Should_Be_Default_If_Produto_Doesnt_Exists()
        {
            IProdutoStorage storage = new ProdutoStorage(_dbConnectionFactory);
            var res = await storage.GetProduto(2);
            res.Should().BeSameAs(default);
            await CleanUpProdutos(_dbConnectionFactory);
        }
        [Fact]
        public async Task Should_Update_Produto_If_Exists()
        {
            Produto prod = Fake.FakeDataFactory.FakeProdutos();
            IProdutoStorage storage = new ProdutoStorage(_dbConnectionFactory);
            await storage.InsertProduto(prod);
            Produto prod2 = Fake.FakeDataFactory.FakeProdutos();
            var res = await storage.GetProdutos();
            prod2.Id = res.ElementAt(0).Id;
            await storage.UpdateProduto(prod2);
            res = await storage.GetProdutos();
            res.Should().Contain(x => x.Descricao == prod2.Descricao);
            await CleanUpProdutos(_dbConnectionFactory);
        }

        [Fact]
        public async Task Should_Delete_Produto_If_Exists()
        {
            Produto prod = Fake.FakeDataFactory.FakeProdutos();
            IProdutoStorage storage = new ProdutoStorage(_dbConnectionFactory);

            await storage.InsertProduto(prod);
            var res = await storage.GetProdutos();
            Produto returned = res.Where(x => x.Descricao == prod.Descricao).FirstOrDefault();

            await storage.DeleteProduto(returned.Id);
            res = await storage.GetProdutos();
            res.Should().NotContain(x => x.Descricao == prod.Descricao);

            await CleanUpProdutos(_dbConnectionFactory);

        }


        private async Task CleanUpProdutos(ISqlAccessConnectionFactory _dbConnectionFactory)
        {
            string str = @"Delete from dbo.Produto";
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            await connection.QueryAsync(str);


        }
    }
}
