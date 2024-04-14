using Application.Models;
using Dapper;
using FluentAssertions;
using ShredStoreTests.DataAdapterFiles;
using ShredStoreTests.DataAdapterFiles.UserTestFiles;

namespace ShredStoreTests
{
    public class UserDatabaseTests : IClassFixture<SqlAccessFixture>
    {

        private readonly ISqlAccessConnectionFactory _dbConnectionFactory;
        public UserDatabaseTests(SqlAccessFixture fixture)
        {
            _dbConnectionFactory = fixture.ConnectionFactory;
        }
        
        [Fact]
        public void Should_Throw_ArgumentNullException_If_Missing_Connection_String()
        {
            var create = () => new UserStorage(null!);
            create.Should().ThrowExactly<ArgumentNullException>();
        }
        [Theory]
        [InlineData("spUser_Insert")]
        [InlineData("spUser_Login")]
        [InlineData("spUser_GetAll")]
        [InlineData("spUser_GetById")]
        [InlineData("spUser_Delete")]
        [InlineData("spUser_Update")]
        [InlineData("spUser_ResetPasswordByEmail")]
        public async Task Should_Be_True_If_Stored_Procedure_Exists(string Sp)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(default);
            string spName = Sp;

            dynamic res = await connection.QueryAsync(Utility.CreateQueryForStoredProcedureCheck(spName));
            string name = res[0].name;

            name.Should().Be(spName);
        }

        [Fact]
        public async Task Should_Insert_User_If_Not_Exists()
        {
            User user = Fake.FakeDataFactory.FakeUser();
            IUserStorage storage = new UserStorage(_dbConnectionFactory);
            int expected = 0;

            await storage.InsertUser(user);

            var res = await storage.GetUsers();
            res.Should().HaveCountGreaterThan(expected);
            await Utility.CleanUpUsers(_dbConnectionFactory);
        }
        [Fact]
        public async Task Should_Be_Default_If_User_Does_Not_Exists()
        {
            IUserStorage storage = new UserStorage(_dbConnectionFactory);
            int userId = int.MaxValue;

            var res = await storage.GetUser(userId);

            res.Should().BeSameAs(default);

            await Utility.CleanUpUsers(_dbConnectionFactory);

        }

        [Fact]
        public async Task Should_Delete_User_If_Exists()
        {
            User user = Fake.FakeDataFactory.FakeUser();
            IUserStorage storage = new UserStorage(_dbConnectionFactory);
            
            await storage.InsertUser(user);
            var res = await storage.GetUsers();

            User returned = res.Where(x => x.Cpf == user.Cpf).FirstOrDefault();

            await storage.DeleteUser(returned.Id);

            res = await storage.GetUsers();
            res.Should().NotContain(x => x.Cpf == user.Cpf);

            await Utility.CleanUpUsers(_dbConnectionFactory);
        }
        [Fact]
        public async Task Should_Be_Null_If_Login_Fails()
        {
            User user = Fake.FakeDataFactory.FakeUser();
            IUserStorage storage = new UserStorage(_dbConnectionFactory);

            var res = await storage.Login(user.Email, user.Password);

            res.Should().Be(null);

            await Utility.CleanUpUsers(_dbConnectionFactory);
        }
        [Fact]
        public async Task Should_Return_User_If_Login_Succeeds()
        {
            User user = Fake.FakeDataFactory.FakeUser();
            IUserStorage storage = new UserStorage(_dbConnectionFactory);

            await storage.InsertUser(user);
            var res = await storage.Login(user.Email, user.Password);

            res.Cpf.Should().BeEquivalentTo(user.Cpf);

            await Utility.CleanUpUsers(_dbConnectionFactory);
        }
        [Fact]
        public async Task Should_Update_User_If_Exists()
        {
            User user = Fake.FakeDataFactory.FakeUser();
            IUserStorage storage = new UserStorage(_dbConnectionFactory);

            await storage.InsertUser(user);

            User user2 = Fake.FakeDataFactory.FakeUser();
            var res = await storage.GetUsers();

            
            user2.Id = res.First().Id;

            await storage.UpdateUser(user2);

            res = await storage.GetUsers();

            res.Should().Contain(x => x.Cpf == user2.Cpf);

            await Utility.CleanUpUsers(_dbConnectionFactory);
        }




    }
}
