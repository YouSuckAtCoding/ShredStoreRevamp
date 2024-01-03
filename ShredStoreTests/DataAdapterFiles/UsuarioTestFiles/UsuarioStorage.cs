using Application.Models;
using Application.Repositories;
using Dapper;
using DatabaseAccess;
using ShredStoreTests.DataAdapterFiles;
using System.Data;

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

    public class TestSqlDataAccess 
    {

        private ISqlAccessConnectionFactory _dbConnectionFactory;

        public TestSqlDataAccess(ISqlAccessConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }
        /// <summary>
        /// Método para buscar dados do banco
        /// </summary>
        /// <typeparam name="T">Parametro genérico</typeparam>
        /// <typeparam name="U">Parametro genérico</typeparam>
        /// <param name="StoredProcedure">Stored Procedure feita na base de dados</param>
        /// <param name="parameters"></param>
        /// <param name="connectionId">Id da connection string</param>
        /// <returns>Dados</returns>
        public async Task<IEnumerable<T>> LoadData<T, U>(string StoredProcedure,
            U parameters)
        {
            using IDbConnection connection = await _dbConnectionFactory.CreateConnectionAsync();

            return await connection.QueryAsync<T>(StoredProcedure, parameters, commandType: CommandType.StoredProcedure);

        }
        /// <summary>
        /// Método para salvar no banco de dados
        /// </summary>
        /// <typeparam name="T">Parametro Genérico</typeparam>
        /// <param name="StoredProcedure">Stored Procedure feita na base de dados</param>
        /// <param name="parameters"></param>
        /// <param name="connectionId">Id da connection string</param>
        /// <returns>Nada.</returns>
        public async Task SaveData<T>(string StoredProcedure,
            T parameters)
        {
            using IDbConnection connection = await _dbConnectionFactory.CreateConnectionAsync();

            await connection.ExecuteAsync(StoredProcedure, parameters, commandType: CommandType.StoredProcedure);

        }
    }
}
