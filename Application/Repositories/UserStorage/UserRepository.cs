using Application.Models;
using Dapper;
using DatabaseAccess;

namespace Application.Repositories.UserStorage
{

    public class UserRepository : IUserRepository
    {
        private readonly ISqlDataAccess sqlDataAccess;

        public UserRepository(ISqlDataAccess _sqlDataAccess)
        {
            sqlDataAccess = _sqlDataAccess;
        }
        public Task DeleteUser(int id) => sqlDataAccess.SaveData("dbo.spUser_Delete", new { Id = id });

        public async Task<User?> GetUser(int id)
        {
            var result = await sqlDataAccess.LoadData<User, dynamic>("dbo.spUser_GetById", new { Id = id });
            return result.FirstOrDefault();
        }

        public Task<IEnumerable<User>> GetUsers() => sqlDataAccess.LoadData<User, dynamic>("dbo.spUser_GetAll", new { });

        public Task InsertUser(User User) =>
            sqlDataAccess.SaveData("dbo.spUser_Insert", new { User.Name, User.Age, User.Email, User.Cpf, User.Address, User.Password });

        public async Task<User?> Login(string Name, string Password)
        {
            var p = new DynamicParameters();
            p.Add("@Name", Name);
            p.Add("@Password", Password);
            p.Add("@ResponseMessage", "", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output);

            var user = await sqlDataAccess.LoadData<User, dynamic>("dbo.spUser_Login", p);

            if (user.Any())
            {
                return user.FirstOrDefault();
            }
            else
                return null;

        }
        public Task UpdateUser(User user) => sqlDataAccess.SaveData("dbo.spUser_Update", new { user.Id, user.Name, user.Age, user.Email, user.Cpf, user.Address });

    };

}

