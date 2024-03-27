using Application.Models;
using Dapper;
using Org.BouncyCastle.Asn1.Mozilla;
using ShredStoreTests.DataAdapterFiles;
using ShredStoreTests.DataAdapterFiles.UserTestFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShredStoreTests
{
    public static class Utility
    {
        public static string CreateQueryForStoredProcedureCheck(string spName)
        {
            string str = @"SELECT name
                     FROM sys.objects
                     WHERE type = 'P' AND name = '" + spName + "';";
            return str;
        }

        public static async Task CleanUpUsers(ISqlAccessConnectionFactory _dbConnectionFactory)
        {
            string str = @"Delete from dbo.[User]";
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(default);
            await connection.QueryAsync(str);
        }
        public static async Task CleanUpProducts(ISqlAccessConnectionFactory _dbConnectionFactory)
        {
            string str = @"Delete from dbo.Product";
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(default);
            await connection.QueryAsync(str);
        }
        public static async Task ClearCartItems(ISqlAccessConnectionFactory _dbConnectionFactory)
        {
            string str = @"Delete from dbo.CartItem";
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(default);
            await connection.QueryAsync(str);
        }
        public static async Task CleanUpCarts(ISqlAccessConnectionFactory _dbConnectionFactory)
        {
            string str = @"Delete from dbo.Cart";
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(default);
            await connection.QueryAsync(str);
        }
        public static async Task CleanUpOrders(ISqlAccessConnectionFactory _dbConnectionFactory)
        {
            string str = @"Delete from dbo.[Order]";
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(default);
            await connection.QueryAsync(str);
        }

        public static async Task<int> GenerateUserId(ISqlAccessConnectionFactory _dbConnectionFactory)
        {
            User user = Fake.FakeDataFactory.FakeUser();
            IUserStorage userStorage = new UserStorage(_dbConnectionFactory);

            await userStorage.InsertUser(user);
            var userRes = await userStorage.Login(user.Email, user.Password);
            return userRes!.Id;
        }
    }
}
