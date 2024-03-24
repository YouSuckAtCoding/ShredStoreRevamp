using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MsSql;

namespace ShredStoreTests.DataAdapterFiles
{
    public class SqlAccessFixture : IAsyncLifetime
    {
        private readonly MsSqlContainer _msSqlContainer
            = new MsSqlBuilder().Build();

        public ISqlAccessConnectionFactory ConnectionFactory;

        public async Task InitializeAsync()
        {
            await _msSqlContainer.StartAsync();

            ConnectionFactory = new SqlAccessConnectionFactory(_msSqlContainer.GetConnectionString());
            await new DatabaseInitializer(ConnectionFactory).InitializeAsync();
        }
        public Task DisposeAsync()
        {
            return _msSqlContainer.DisposeAsync().AsTask();
        }
    }
}
