using Application.Models;

namespace ShredStoreTests.DataAdapterFiles.UsuarioTestFiles
{
    internal interface IUsuarioStorage
    {
        Task DeleteUsuario(int id);
        Task<Usuario?> GetUsuario(int id);
        Task<IEnumerable<Usuario>> GetUsuarios();
        Task InsertUser(Usuario usuario);
        Task<Usuario?> Login(string Nome, string Password);
        Task UpdateUsuario(Usuario user);
    }
}