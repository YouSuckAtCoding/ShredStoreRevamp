using Application.Models;

namespace Application.Services.CartItemServices
{
    public interface ICartItemService
    {
        Task<bool> CreateCartItem(CartItem cartItem, CancellationToken token);
        Task<bool> DeleteAlltems(int cartId, CancellationToken token);
        Task<bool> DeleteItem(int itemId, int cartId, CancellationToken token);
        Task<CartItem> GetCartItem(int itemId, int cartId, CancellationToken token);
        Task<IEnumerable<CartItem>> GetCartItems(int cartId, CancellationToken token);
        Task UpdateCartItem(int itemId, int cartId, int quantity, CancellationToken token);
    }
}