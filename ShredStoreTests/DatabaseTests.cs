using Application.Models;
using Bogus.DataSets;
using Dapper;
using DatabaseAccess;
using FluentAssertions;
using ShredStoreTests.DataAdapterFiles;
using ShredStoreTests.DataAdapterFiles.UsuarioTestFiles;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShredStoreTests
{
    public class DatabaseTests : IClassFixture<SqlAccessFixture>
    {

        private readonly ISqlAccessConnectionFactory _dbConnectionFactory;
        public DatabaseTests(SqlAccessFixture fixture)
        {
            _dbConnectionFactory = fixture.ConnectionFactory;
        }
        [Fact]
        public void Should_Throw_ArgumentNullException_If_Missing_Connection_String()
        {
            var create = () => new UsuarioStorage(null!);
            create.Should().ThrowExactly<ArgumentNullException>();
        }
        [Fact]
        public async Task Should_Be_True_If_Stored_Procedure_Exists()
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            string spName = "spUsuario_Insert";
            dynamic res = await connection.QueryAsync(CreateQueryForStoredProcedureCheck(spName));
            string name = res[0].name;
            name.Should().Be(spName);
        }

        [Fact]
        public async Task Should_Insert_Usuario_If_Not_Exists()
        {
            Usuario user = Fake.FakeDataFactory.FakeUsuarios();
            IUsuarioStorage storage = new UsuarioStorage(_dbConnectionFactory);
            await storage.InsertUser(user);
            var res = await storage.GetUsuarios();
            res.Should().HaveCountGreaterThan(0);
            await CleanUpUsuarios(_dbConnectionFactory);
        }
        [Fact]
        public async Task Should_Be_Default_If_Usuario_Does_Not_Exists()
        {
            Usuario user = Fake.FakeDataFactory.FakeUsuarios();
            IUsuarioStorage storage = new UsuarioStorage(_dbConnectionFactory);
            var res = await storage.GetUsuario(50);
            res.Should().BeSameAs(default);
            await CleanUpUsuarios(_dbConnectionFactory);

        }

        [Fact]
        public async Task Should_Delete_User_If_Exists()
        {
            Usuario user = Fake.FakeDataFactory.FakeUsuarios();
            IUsuarioStorage storage = new UsuarioStorage(_dbConnectionFactory);
            
            await storage.InsertUser(user);
            var res = await storage.GetUsuarios();
            Usuario returned = res.Where(x => x.Cpf == user.Cpf).FirstOrDefault();
            await storage.DeleteUsuario(returned.Id);

            res = await storage.GetUsuarios();
            res.Should().NotContain(x => x.Cpf == user.Cpf);

            await CleanUpUsuarios(_dbConnectionFactory);
        }
        [Fact]
        public async Task Should_Be_Null_If_Login_Fails()
        {
            Usuario user = Fake.FakeDataFactory.FakeUsuarios();
            IUsuarioStorage storage = new UsuarioStorage(_dbConnectionFactory);
            var res = await storage.Login(user.Nome, user.Password);
            res.Should().Be(null);
            await CleanUpUsuarios(_dbConnectionFactory);
        }
        [Fact]
        public async Task Should_Return_User_If_Login_Succeeds()
        {
            Usuario user = Fake.FakeDataFactory.FakeUsuarios();
            IUsuarioStorage storage = new UsuarioStorage(_dbConnectionFactory);
            await storage.InsertUser(user);
            var res = await storage.Login(user.Nome, user.Password);
            res.Cpf.Should().BeEquivalentTo(user.Cpf);
            await CleanUpUsuarios(_dbConnectionFactory);
        }
        [Fact]
        public async Task Should_Update_Usuario_If_Exists()
        {
            Usuario user = Fake.FakeDataFactory.FakeUsuarios();
            IUsuarioStorage storage = new UsuarioStorage(_dbConnectionFactory);
            await storage.InsertUser(user);
            Usuario user2 = Fake.FakeDataFactory.FakeUsuarios();
            user2.Id = 1;
            await storage.UpdateUsuario(user2);
            var res = await storage.GetUsuario(1);
            res.Cpf.Should().BeEquivalentTo(user2.Cpf);
            await CleanUpUsuarios(_dbConnectionFactory);
        }
        private string CreateQueryForStoredProcedureCheck(string spName)
        {
            string str = @"SELECT name
                     FROM sys.objects
                     WHERE type = 'P' AND name = '" + spName + "';";
            return str;
        }
        private async Task CleanUpUsuarios(ISqlAccessConnectionFactory _dbConnectionFactory)
        {
            string str = @"Delete from dbo.Usuario";
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            dynamic res = await connection.QueryAsync(str);
            
            
        }

    }
}
