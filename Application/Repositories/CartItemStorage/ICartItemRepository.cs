using Application.Models;

namespace Application.Repositories.CartItemStorage
{
    public interface ICartItemRepository
    {
        Task DeleteAllCartItem(int cartId, CancellationToken token);
        Task DeleteCartItem(int productId, int cartId, CancellationToken token);
        Task<CartItem?> GetCartItem(int itemId, int cartId, CancellationToken token);
        Task<IEnumerable<CartItem>> GetCartItems(int id, CancellationToken token);
        Task InsertCartItem(CartItem cartItem, CancellationToken token);
        Task UpdateCartItem(int productId, int quantity, int cartId, CancellationToken token);
    }
}