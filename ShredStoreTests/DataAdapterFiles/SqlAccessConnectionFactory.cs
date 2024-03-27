using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShredStoreTests.DataAdapterFiles
{
    public class SqlAccessConnectionFactory : ISqlAccessConnectionFactory
    {
        private readonly string _connectionString;

        public SqlAccessConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<SqlConnection> CreateConnectionAsync(CancellationToken token = default)
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(token);
            return connection;

        }
    }
}
