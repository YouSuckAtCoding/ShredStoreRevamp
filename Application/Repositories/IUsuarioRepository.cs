using Application.Models;

namespace Application.Repositories
{
    public interface IUsuarioRepository
    {
        Task DeleteUsuario(int id);
        Task<Usuario?> GetUsuario(int id);
        Task<IEnumerable<Usuario>> GetUsuarios();
        Task InsertUser(Usuario usuario);
        Task<Usuario?> Login(string Nome, string Password);
        Task UpdateUsuario(Usuario user);
    }
}