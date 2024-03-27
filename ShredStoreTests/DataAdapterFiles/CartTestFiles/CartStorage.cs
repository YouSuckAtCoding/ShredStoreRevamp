using Application.Models;
using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShredStoreTests.DataAdapterFiles.CartTestFiles
{
    public class CartStorage : ICartStorage
    {

        private TestSqlDataAccess _dataAccess;

        public CartStorage(ISqlAccessConnectionFactory dbConnectionFactory)
        {
            ArgumentNullException.ThrowIfNull(dbConnectionFactory);
            _dataAccess = new TestSqlDataAccess(dbConnectionFactory);
        }

        public Task InsertCart(Cart Cart) =>
           _dataAccess.SaveData("dbo.spCart_Insert", new { Cart.UserId, Cart.CreatedDate });

        public async Task<Cart?> GetCart(int id)
        {
            var result = await _dataAccess.LoadData<Cart, dynamic>("dbo.spCart_GetById", new { UserId = id });
            return result.FirstOrDefault();

        }

        public Task DeleteCart(int id) => _dataAccess.SaveData("dbo.spCart_Delete", new { Id = id });
    }
}
