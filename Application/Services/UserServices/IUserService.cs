using Application.Models;
using Contracts.Request;
using Contracts.Response;

namespace Application.Services.UserServices
{
    public interface IUserService
    {
        Task<bool> DeleteUser(int id, CancellationToken token);
        Task<User?> GetUser(int id, CancellationToken token);
        Task<IEnumerable<User>> GetUsers(CancellationToken token);
        Task<bool> InsertUser(User user, CancellationToken token);
        Task<User?> Login(LoginUserRequest user, CancellationToken token);
        Task<User> UpdateUser(User request, CancellationToken token);
    }
}