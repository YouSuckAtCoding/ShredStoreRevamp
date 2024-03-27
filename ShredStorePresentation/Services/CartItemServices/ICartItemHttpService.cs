using Contracts.Request.CartItemRequests;
using Contracts.Response.CartItemResponses;
using Contracts.Response.ProductsResponses;

namespace ShredStorePresentation.Services.CartItemServices
{
    public interface ICartItemHttpService
    {
        Task<IEnumerable<CartItemResponse>> GetCartItems(int cartId, CancellationToken token);
        Task<bool> InsertCartItems(CreateCartItemRequest request, CancellationToken token);
        Task RemoveItem(int productId, int userId);
        Task<bool> UpdateCartItem(UpdateCartItemRequest request, CancellationToken token);
    }
}