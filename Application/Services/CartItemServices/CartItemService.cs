using Application.Models;
using Application.Repositories.CartItemStorage;
using Application.Repositories.CartStorage;
using FluentValidation;

namespace Application.Services.CartItemServices
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IValidator<CartItem> _cartItemValidator;

        public CartItemService(ICartItemRepository cartItemRepository, IValidator<CartItem> cartItemValidator, ICartRepository cartRepository)
        {
            _cartItemRepository = cartItemRepository;
            _cartItemValidator = cartItemValidator;
            _cartRepository = cartRepository;
        }

        public async Task<bool> CreateCartItem(CartItem cartItem, CancellationToken token)
        {
            await _cartItemValidator.ValidateAndThrowAsync(cartItem, token);
            await _cartItemRepository.InsertCartItem(cartItem, token);
            return true;
        }

        public async Task<bool> DeleteAlltems(int cartId, CancellationToken token)
        {
            var result = await _cartRepository.GetCart(cartId, token);
            
            if (result is null)
                return false;

            await _cartItemRepository.DeleteAllCartItem(cartId, token);
            return true;
        }

        public async Task<bool> DeleteItem(int itemId, int cartId, CancellationToken token)
        {
            var result = await _cartItemRepository.GetCartItem(itemId, cartId, token);
            
            if (result is null)
                return false;

            await _cartItemRepository.DeleteCartItem(itemId, cartId, token);
            return true;
        }

        public async Task<CartItem> GetCartItem(int itemId, int cartId, CancellationToken token)
        {
            var result = await _cartItemRepository.GetCartItem(itemId, cartId, token);
            return result;
        }

        public async Task<IEnumerable<CartItem>> GetCartItems(int cartId, CancellationToken token)
        {
            IEnumerable<CartItem> result = await _cartItemRepository.GetCartItems(cartId, token);
            return result;
        }

        public async Task UpdateCartItem(int itemId, int cartId, int quantity, CancellationToken token)
        {
           await _cartItemRepository.UpdateCartItem(itemId, quantity, cartId, token);

        }
    }
}
