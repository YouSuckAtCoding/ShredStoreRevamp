using Application.Models;
using Dapper;
using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{

    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ISqlDataAccess sqlDataAccess;

        public UsuarioRepository(ISqlDataAccess _sqlDataAccess)
        {
            sqlDataAccess = _sqlDataAccess;
        }
        public Task InsertUser(Usuario usuario) =>
            sqlDataAccess.SaveData("dbo.spUsuario_Insert", new { usuario.Nome, usuario.Idade, usuario.Email, usuario.Cpf, usuario.Endereco, usuario.Password });

        public Task<IEnumerable<Usuario>> GetUsuarios() => sqlDataAccess.LoadData<Usuario, dynamic>("dbo.spUsuario_GetAll", new { });

        public async Task<Usuario?> GetUsuario(int id)
        {
            var result = await sqlDataAccess.LoadData<Usuario, dynamic>("dbo.spUsuario_GetById", new { Id = id });

            return result.FirstOrDefault();

        }
        public async Task<Usuario?> Login(string Nome, string Password)
        {
            var p = new DynamicParameters();
            p.Add("@Nome", Nome);
            p.Add("@Password", Password);
            p.Add("@ResponseMessage", "", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output);

            var user = await sqlDataAccess.LoadData<Usuario, dynamic>("dbo.spUsuario_Login", p);

            if (user.Any())
            {
                return user.FirstOrDefault();
            }
            else
                return null;

        }
        public Task UpdateUsuario(Usuario user) => sqlDataAccess.SaveData("dbo.spUsuario_Update", new { user.Id, user.Nome, user.Idade, user.Email, user.Cpf, user.Endereco });

        public Task DeleteUsuario(int id) => sqlDataAccess.SaveData("dbo.spUsuario_Delete", new { Id = id });

    };

}

