using Application.Models;
using Contracts.Request;

namespace Application.Repositories.UserStorage
{
    public interface IUserRepository
    {
        Task DeleteUser(int id,CancellationToken token);
        Task<User?> GetUser(int id,CancellationToken token);
        Task<IEnumerable<User>> GetUsers(CancellationToken token);
        Task InsertUser(User User,CancellationToken token);
        Task<User?> Login(LoginUserRequest user,CancellationToken token);
        Task ResetPassword(ResetPasswordUserRequest request, CancellationToken token);
        Task UpdateUser(User user,CancellationToken token);
    }
}