using Application.Models;
using Contracts.Request.CartItemRequests;
using Contracts.Response.CartItemResponses;

namespace ShredStore.Mapping
{
    public static class CartItemMapping
    {
        public static CartItem MapToCartItem(this CreateCartItemRequest request)
        {
            return new CartItem
            {
                CartId = request.CartId,
                Quantity = request.Quantity,
                ProductId = request.ProductId
            };
        }

        public static CartItemResponse MapToCartItemResponse(this CartItem request)
        {
            return new CartItemResponse
            {
                CartId = request.CartId,
                Quantity = request.Quantity,
                ProductId = request.ProductId,
                Price = request.Price
            };
        }
        public static CartItem MapToCartItem(this UpdateCartItemRequest request)
        {
            return new CartItem
            {
                CartId = request.CartId,
                Quantity = request.Quantity,
                ProductId = request.ProductId
            };
        }


    }
}
