using Application.Models;

namespace Application.Repositories.UserStorage
{
    public interface IUserRepository
    {
        Task DeleteUser(int id);
        Task<User?> GetUser(int id);
        Task<IEnumerable<User>> GetUsers();
        Task InsertUser(User User);
        Task<User?> Login(string Name, string Password);
        Task UpdateUser(User user);
    }
}