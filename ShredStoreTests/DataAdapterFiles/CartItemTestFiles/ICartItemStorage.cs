using Application.Models;

namespace ShredStoreTests.DataAdapterFiles.CartItemTestFiles
{
    public interface ICartItemStorage
    {
        Task DeleteAllCartItem(int cartId);
        Task DeleteCartItem(int productId, int cartId);
        Task<IEnumerable<CartItem>> GetCartItems(int id);
        Task InsertCartItem(CartItem cartItem);
        Task UpdateCartItem(int productId, int quantity, int cartId);
    }
}