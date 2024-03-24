using Application.Models;
using Application.Repositories;
using Dapper;
using DatabaseAccess;

namespace ShredStoreTests.DataAdapterFiles.UserTestFiles
{
    internal class UserStorage : IUserStorage
    {
        private TestSqlDataAccess _dataAccess;

        public UserStorage(ISqlAccessConnectionFactory dbConnectionFactory)
        {
            ArgumentNullException.ThrowIfNull(dbConnectionFactory);
            _dataAccess = new TestSqlDataAccess(dbConnectionFactory);
        }

        public Task DeleteUser(int id) => _dataAccess.SaveData("dbo.spUser_Delete", new { Id = id });

        public async Task<User?> GetUser(int id)
        {
            var result = await _dataAccess.LoadData<User, dynamic>("dbo.spUser_GetById", new { Id = id });

            return result.FirstOrDefault();
        }

        public Task<IEnumerable<User>> GetUsers() => _dataAccess.LoadData<User,dynamic>("dbo.spUser_GetAll", new { });

        public Task InsertUser(User User) =>
            _dataAccess.SaveData("dbo.spUser_Insert", new { User.Name, User.Age, User.Email, User.Cpf, User.Address, User.Password, User.Role });

        public async Task<User?> Login(string Email, string Password)
        {
            var p = new DynamicParameters();
            p.Add("@Email", Email);
            p.Add("@Password", Password);
            p.Add("@ResponseMessage", "", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output);

            var user = await _dataAccess.LoadData<User, dynamic>("dbo.spUser_Login", p);

            if (user.Any())
            {
                return user.FirstOrDefault();
            }
            else
                return null;

        }
        public Task UpdateUser(User user) => _dataAccess.SaveData("dbo.spUser_Update", new { user.Id, user.Name, user.Age, user.Email, user.Cpf, user.Address});


    }
}
