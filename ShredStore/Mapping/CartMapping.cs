using Application.Models;
using Contracts.Request.CartRequests;
using Contracts.Response.CartResponses;

namespace ShredStore.Mapping
{
    public static class CartMapping
    {
        public static Cart MapToCart(this CreateCartRequest request)
        {
            return new Cart
            {
                UserId = request.UserId,
                CreatedDate = request.CreatedDate
            };
        }

        public static CartResponse MapToCartResponse(this Cart request)
        {
            return new CartResponse
            {
                UserId = request.UserId,
                CreatedDate = request.CreatedDate
            };
        }
    }
}
