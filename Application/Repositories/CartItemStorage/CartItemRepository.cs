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

        public Task<IEnumerable<CartItem>> GetCartItems(int id)
        {
            var result = sqlDataAccess.LoadData<CartItem, dynamic>("dbo.spCartItem_GetAll", new { CartId = id });
            return result;
        }
        public Task InsertCartItem(CartItem cartItem) =>
            sqlDataAccess.SaveData("dbo.spCartItem_Insert", new { cartItem.CartId, cartItem.ProductId, cartItem.Quantity, cartItem.Price });
        public Task UpdateCartItem(int productId, int quantity, int cartId) =>
         sqlDataAccess.SaveData("dbo.spCartItem_Update", new { ProductId = productId, Quantity = quantity, CartId = cartId });

        public Task DeleteCartItem(int productId, int cartId) => sqlDataAccess.SaveData("dbo.spCartItem_Delete", new { ProductId = productId, CartId = cartId });
        public Task DeleteAllCartItem(int cartId) => sqlDataAccess.SaveData("dbo.spCartItem_DeleteAll", new { CartId = cartId });


    }
}
