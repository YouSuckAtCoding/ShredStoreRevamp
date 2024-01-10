using Application.Models;
using Application.Repositories;
using Dapper;
using DatabaseAccess;

namespace ShredStoreTests.DataAdapterFiles.UsuarioTestFiles
{
    internal class UsuarioStorage : IUsuarioStorage
    {
        private TestSqlDataAccess _dataAccess;

        public UsuarioStorage(ISqlAccessConnectionFactory dbConnectionFactory)
        {
            ArgumentNullException.ThrowIfNull(dbConnectionFactory);
            _dataAccess = new TestSqlDataAccess(dbConnectionFactory);
        }

        public Task DeleteUsuario(int id) => _dataAccess.SaveData("dbo.spUsuario_Delete", new { Id = id });

        public async Task<Usuario?> GetUsuario(int id)
        {
            var result = await _dataAccess.LoadData<Usuario, dynamic>("dbo.spUsuario_GetById", new { Id = id });

            return result.FirstOrDefault();

        }

        public Task<IEnumerable<Usuario>> GetUsuarios() => _dataAccess.LoadData<Usuario,dynamic>("dbo.spUsuario_GetAll", new { });

        public Task InsertUser(Usuario usuario) =>
            _dataAccess.SaveData("dbo.spUsuario_Insert", new { usuario.Nome, usuario.Idade, usuario.Email, usuario.Cpf, usuario.Endereco, usuario.Password });

        public async Task<Usuario?> Login(string Nome, string Password)
        {
            var p = new DynamicParameters();
            p.Add("@Nome", Nome);
            p.Add("@Password", Password);
            p.Add("@ResponseMessage", "", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output);

            var user = await _dataAccess.LoadData<Usuario, dynamic>("dbo.spUsuario_Login", p);

            if (user.Any())
            {
                return user.FirstOrDefault();
            }
            else
                return null;

        }
        public Task UpdateUsuario(Usuario user) => _dataAccess.SaveData("dbo.spUsuario_Update", new { user.Id, user.Nome, user.Idade, user.Email, user.Cpf, user.Endereco });


    }
}
