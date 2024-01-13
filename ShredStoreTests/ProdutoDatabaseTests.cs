﻿using Application.Models;
using Dapper;
using FluentAssertions;
using ShredStoreTests.DataAdapterFiles;
using ShredStoreTests.DataAdapterFiles.ProductTestFiles;
using ShredStoreTests.DataAdapterFiles.UserTestFiles;

namespace ShredStoreTests
{
    public class ProductDatabaseTests : IClassFixture<SqlAccessFixture>
    {
        private readonly ISqlAccessConnectionFactory _dbConnectionFactory;
        public ProductDatabaseTests(SqlAccessFixture fixture)
        {
            _dbConnectionFactory = fixture.ConnectionFactory;
        }

        [Fact]
        public void Should_Throw_ArgumentNullException_If_Missing_Connection_String()
        {
            var create = () => new ProductStorage(null!);
            create.Should().ThrowExactly<ArgumentNullException>();
        }
        [Theory]
        [InlineData("spProduct_Insert")]
        [InlineData("spProduct_GetAll")]
        [InlineData("spProduct_GetById")]
        [InlineData("spProduct_Delete")]
        [InlineData("spProduct_Update")]
        public async Task Should_Be_True_If_Product_Stored_Procedure_Exists(string Sp)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(default);
            string spName = Sp;
            Utility utility = new Utility();
            dynamic res = await connection.QueryAsync(utility.CreateQueryForStoredProcedureCheck(spName));
            string name = res[0].name;
            name.Should().Be(spName);
        }
        [Fact]
        public async Task Should_Insert_Product_If_Doesnt_Exist()
        {
            Product prod = Fake.FakeDataFactory.FakeProduct();
            IProductStorage storage = new ProductStorage(_dbConnectionFactory);
            await storage.InsertProduct(prod);
            var res = await storage.GetProducts();
            res.Should().HaveCountGreaterThan(0);
            await CleanUpProducts(_dbConnectionFactory);

        }
        [Fact]
        public async Task Should_Be_Empty_If_No_Product_Exists()
        {
            IProductStorage storage = new ProductStorage(_dbConnectionFactory);
            var res = await storage.GetProducts();
            res.Should().BeEmpty();
            await CleanUpProducts(_dbConnectionFactory);
        }
        [Fact]
        public async Task Should_Be_Default_If_Product_Doesnt_Exists()
        {
            IProductStorage storage = new ProductStorage(_dbConnectionFactory);
            var res = await storage.GetProduct(2);
            res.Should().BeSameAs(default);
            await CleanUpProducts(_dbConnectionFactory);
        }
        [Fact]
        public async Task Should_Update_Product_If_Exists()
        {
            Product prod = Fake.FakeDataFactory.FakeProduct();
            IProductStorage storage = new ProductStorage(_dbConnectionFactory);
            await storage.InsertProduct(prod);
            Product prod2 = Fake.FakeDataFactory.FakeProduct();
            var res = await storage.GetProducts();
            prod2.Id = res.ElementAt(0).Id;
            await storage.UpdateProduct(prod2);
            res = await storage.GetProducts();
            res.Should().Contain(x => x.Description == prod2.Description);
            await CleanUpProducts(_dbConnectionFactory);
        }

        [Fact]
        public async Task Should_Delete_Product_If_Exists()
        {
            Product prod = Fake.FakeDataFactory.FakeProduct();
            IProductStorage storage = new ProductStorage(_dbConnectionFactory);

            await storage.InsertProduct(prod);
            var res = await storage.GetProducts();
            Product returned = res.Where(x => x.Description == prod.Description).FirstOrDefault();

            await storage.DeleteProduct(returned.Id);
            res = await storage.GetProducts();
            res.Should().NotContain(x => x.Description == prod.Description);

            await CleanUpProducts(_dbConnectionFactory);

        }


        private async Task CleanUpProducts(ISqlAccessConnectionFactory _dbConnectionFactory)
        {
            string str = @"Delete from dbo.Product";
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(default);
            await connection.QueryAsync(str);


        }
    }
}
