using System.Data;

namespace ShredStoreTests.DataAdapterFiles
{
    public interface ISqlAccessConnectionFactory
    {
        Task<IDbConnection> CreateConnectionAsync(CancellationToken token);
    }
}