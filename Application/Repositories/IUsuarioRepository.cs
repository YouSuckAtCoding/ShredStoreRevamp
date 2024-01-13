using Application.Models;

namespace Application.Repositories
{
    public interface IUsuarioRepository
    {
        Task DeleteUsuario(int id);
        Task<User?> GetUsuario(int id);
        Task<IEnumerable<User>> GetUsuarios();
        Task InsertUser(User usuario);
        Task<User?> Login(string Name, string Password);
        Task UpdateUsuario(User user);
    }
}