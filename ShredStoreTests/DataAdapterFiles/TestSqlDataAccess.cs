using Dapper;
using System.Data;

namespace ShredStoreTests.DataAdapterFiles
{
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
