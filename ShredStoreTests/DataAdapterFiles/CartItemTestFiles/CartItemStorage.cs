using Application.Models;
using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShredStoreTests.DataAdapterFiles.CartItemTestFiles
{
    public class CartItemStorage : ICartItemStorage
    {
        private TestSqlDataAccess _dataAccess;

        public CartItemStorage(ISqlAccessConnectionFactory dbConnectionFactory)
        {
            ArgumentNullException.ThrowIfNull(dbConnectionFactory);
            _dataAccess = new TestSqlDataAccess(dbConnectionFactory);
        }

        public Task<IEnumerable<CartItem>> GetCartItems(int id)
        {
            var result = _dataAccess.LoadData<CartItem, dynamic>("dbo.spCartItem_GetAll", new { CartId = id });
            return result;
        }
        public Task InsertCartItem(CartItem cartItem) =>
            _dataAccess.SaveData("dbo.spCartItem_Insert", new { cartItem.CartId, cartItem.ProductId, cartItem.Quantity});

        public Task UpdateCartItem(int productId, int quantity, int cartId) =>
            _dataAccess.SaveData("dbo.spCartItem_Update", new { ProductId = productId, Quantity = quantity, CartId = cartId });

        public Task DeleteCartItem(int productId, int cartId) => _dataAccess.SaveData("dbo.spCartItem_Delete", new { ProductId = productId, CartId = cartId });
        public Task DeleteAllCartItem(int cartId) => _dataAccess.SaveData("dbo.spCartItem_DeleteAll", new { CartId = cartId });


    }
}
