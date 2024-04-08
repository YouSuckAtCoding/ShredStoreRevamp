using Application.Models;
using Contracts.Request;
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
        public Task DeleteUser(int id, CancellationToken token) => sqlDataAccess.SaveData("dbo.spUser_Delete", new { Id = id }, token:token);

        public async Task<User?> GetUser(int id, CancellationToken token)
        {
            var result = await sqlDataAccess.LoadData<User, dynamic>("dbo.spUser_GetById", new { Id = id }, token: token);
            return result.FirstOrDefault();
        }

        public Task<IEnumerable<User>> GetUsers(CancellationToken token) => sqlDataAccess.LoadData<User, dynamic>("dbo.spUser_GetAll", new { }, token: token);

        public Task InsertUser(User User, CancellationToken token) =>
            sqlDataAccess.SaveData("dbo.spUser_Insert", new { User.Name, User.Age, User.Email, User.Cpf, User.Address, User.Password , User.Role}, token: token);
        public async Task<User?> Login(LoginUserRequest request, CancellationToken token)
        {
            var p = new DynamicParameters();
            p.Add("@Email", request.Email);
            p.Add("@Password", request.Password);
            p.Add("@ResponseMessage", "", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output);

            var user = await sqlDataAccess.LoadData<User, dynamic>("dbo.spUser_Login", p, token: token);

            if (user.Any())
            {
                return user.FirstOrDefault();
            }
            else
                return null;

        }

        public Task ResetPassword(ResetPasswordUserRequest request, CancellationToken token)
            => sqlDataAccess.SaveData("dbo.spUser_ResetPasswordByEmail", new { request.Email, request.NewPassword }, token: token);

        public Task UpdateUser(User user, CancellationToken token) => sqlDataAccess.SaveData("dbo.spUser_Update", new { user.Id, user.Name, user.Address, user.Role}, token: token);

    };

}

