using System.Data;
using System.Data.SqlClient;

namespace ShredStoreTests.DataAdapterFiles
{
    public interface ISqlAccessConnectionFactory
    {
        Task<SqlConnection> CreateConnectionAsync(CancellationToken token);
    }
}