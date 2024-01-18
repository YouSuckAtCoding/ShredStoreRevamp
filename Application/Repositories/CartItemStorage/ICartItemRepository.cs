using Application.Models;

namespace Application.Repositories.CartItemStorage
{
    public interface ICartItemRepository
    {
        Task DeleteAllCartItem(int cartId);
        Task DeleteCartItem(int productId, int cartId);
        Task<IEnumerable<CartItem>> GetCartItems(int id);
        Task InsertCartItem(CartItem cartItem);
        Task UpdateCartItem(int productId, int quantity, int cartId);
    }
}