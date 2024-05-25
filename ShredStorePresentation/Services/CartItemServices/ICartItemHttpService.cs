using Contracts.Request.CartItemRequests;
using Contracts.Response.CartItemResponses;
using Contracts.Response.ProductsResponses;

namespace ShredStorePresentation.Services.CartItemServices
{
    public interface ICartItemHttpService
    {
        Task<IEnumerable<CartItemResponse>> GetCartItems(int cartId, string token);
        Task<bool> InsertCartItems(CreateCartItemRequest request, string token);
        Task RemoveItem(int productId, int userId, string token);
        Task<bool> UpdateCartItem(UpdateCartItemRequest request, string token);
    }
}