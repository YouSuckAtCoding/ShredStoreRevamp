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
        public Task InsertUser(User usuario) =>
            sqlDataAccess.SaveData("dbo.spUsuario_Insert", new { usuario.Name, usuario.Age, usuario.Email, usuario.Cpf, usuario.Address, usuario.Password });

        public Task<IEnumerable<User>> GetUsuarios() => sqlDataAccess.LoadData<User, dynamic>("dbo.spUsuario_GetAll", new { });

        public async Task<User?> GetUsuario(int id)
        {
            var result = await sqlDataAccess.LoadData<User, dynamic>("dbo.spUsuario_GetById", new { Id = id });

            return result.FirstOrDefault();

        }
        public async Task<User?> Login(string Name, string Password)
        {
            var p = new DynamicParameters();
            p.Add("@Name", Name);
            p.Add("@Password", Password);
            p.Add("@ResponseMessage", "", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output);

            var user = await sqlDataAccess.LoadData<User, dynamic>("dbo.spUsuario_Login", p);

            if (user.Any())
            {
                return user.FirstOrDefault();
            }
            else
                return null;

        }
        public Task UpdateUsuario(User user) => sqlDataAccess.SaveData("dbo.spUsuario_Update", new { user.Id, user.Name, user.Age, user.Email, user.Cpf, user.Address });

        public Task DeleteUsuario(int id) => sqlDataAccess.SaveData("dbo.spUsuario_Delete", new { Id = id });

    };

}

