using Application.Models;
using Application.Repositories.CartStorage;

namespace Application.Services.CartServices
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<bool> Create(Cart cart, CancellationToken token)
        {
            await _cartRepository.InsertCart(cart, token);
            return true;
        }

        public async Task DeleteCart(int id, CancellationToken token)
        {
            await _cartRepository.DeleteCart(id, token);
        }

        public async Task<Cart?> GetCart(int id, CancellationToken token)
        {
            return await _cartRepository.GetCart(id, token);
        }
    }
}
