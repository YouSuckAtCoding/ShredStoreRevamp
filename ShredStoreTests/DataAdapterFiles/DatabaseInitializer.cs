using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using Dapper;

namespace ShredStoreTests.DataAdapterFiles
{
    public class DatabaseInitializer
    {
        private ISqlAccessConnectionFactory _connectionFactory;

        public DatabaseInitializer(ISqlAccessConnectionFactory sqlAccessConnectionFactory)
        {

            _connectionFactory = sqlAccessConnectionFactory;
        }

        public async Task InitializeAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync(default);
            string script = File.ReadAllText("D:\\Projetos\\ShredStore2.0\\ShredStore\\ShredStoreTests\\DataAdapterFiles\\Scripts\\ShredStoreScriptDatabase.sql");
            ExecuteScript(connection, script);
            
        }

        protected virtual void ExecuteScript(SqlConnection connection, string script)
        {
            try
            {
               
                string[] commandTextArray = System.Text.RegularExpressions.Regex.Split(script, "\r\n[\t ]*GO");

                SqlCommand _cmd = new SqlCommand(String.Empty, connection);

                foreach (string commandText in commandTextArray)
                {
                    if (commandText.Trim() == string.Empty) continue;
                    if ((commandText.Length >= 3) && (commandText.Substring(0, 3).ToUpper() == "USE"))
                    {
                        throw new Exception("Create-script contains USE-statement. Please provide non-database specific create-scripts!");
                    }

                    _cmd.CommandText = commandText;
                    _cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            

        }
    }
}
