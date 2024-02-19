using Application.Models;
using Application.Services.CartItemServices;
using ShredStoreTests.Fake;

namespace ShredStoreApiTests.TestDoubles
{
    public class StubSuccessCartItemRepository : ICartItemService
    {
        public Task<bool> CreateCartItem(CartItem cartItem, CancellationToken token)
        {
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAlltems(int cartId, CancellationToken token)
        {
            return Task.FromResult(true);
        }

        public Task<bool> DeleteItem(int productId, int cartId, CancellationToken token)
        {
            return Task.FromResult(true);
        }

        public Task<CartItem> GetCartItem(int itemId, int cartId, CancellationToken token)
        {
            var res = new CartItem();
            return Task.FromResult(res);
        }

        public Task<IEnumerable<CartItem>> GetCartItems(int cartId, CancellationToken token)
        {
            IEnumerable<CartItem> res = Array.Empty<CartItem>();
            return Task.FromResult(res);
        }

        public Task UpdateCartItem(int productId, int cartId, int quantity, CancellationToken token)
        {
            return Task.FromResult(true);
        }
    }
}
