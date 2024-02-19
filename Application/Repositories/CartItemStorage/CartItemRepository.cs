using Application.Models;
using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.CartItemStorage
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly ISqlDataAccess sqlDataAccess;

        public CartItemRepository(ISqlDataAccess _sqlDataAccess)
        {
            sqlDataAccess = _sqlDataAccess;
        }

        public Task<IEnumerable<CartItem>> GetCartItems(int id, CancellationToken token)
        {
            var result = sqlDataAccess.LoadData<CartItem, dynamic>("dbo.spCartItem_GetAll", new { CartId = id }, token:token);
            return result;
        }
        public Task InsertCartItem(CartItem cartItem, CancellationToken token) =>
            sqlDataAccess.SaveData("dbo.spCartItem_Insert", new { cartItem.CartId, cartItem.ProductId, cartItem.Quantity }, token: token);
        public Task UpdateCartItem(int productId, int quantity, int cartId, CancellationToken token) =>
         sqlDataAccess.SaveData("dbo.spCartItem_Update", new { ProductId = productId, Quantity = quantity, CartId = cartId }, token: token);

        public Task DeleteCartItem(int productId, int cartId, CancellationToken token) => sqlDataAccess.SaveData("dbo.spCartItem_Delete", new { ProductId = productId, CartId = cartId }, token: token);
        public Task DeleteAllCartItem(int cartId, CancellationToken token) => sqlDataAccess.SaveData("dbo.spCartItem_DeleteAll", new { CartId = cartId }, token: token);

        public async Task<CartItem?> GetCartItem(int itemId, int cartId, CancellationToken token)
        {
            var result = await sqlDataAccess.LoadData<CartItem, dynamic>("dbo.spCartItem_GetById", new { ProductId = itemId, CartId = cartId }, token: token);
            return result.FirstOrDefault();
        }
    }
}
