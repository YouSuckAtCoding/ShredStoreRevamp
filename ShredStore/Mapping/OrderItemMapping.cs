using Application.Models;
using Contracts.Request.OrderItemRequests;
using Contracts.Response.OrdertemResponses;

namespace ShredStore.Mapping
{
    public static class OrderItemMapping
    {
        public static OrderItem MapToOrderItem(this CreateOrderItemRequest request)
        {
            return new OrderItem
            {
                OrderId = request.OrderId,
                Quantity = request.Quantity,
                ProductId = request.ProductId
            };
        }

        public static OrderItemResponse MapToOrderItemResponse(this OrderItem request)
        {
            return new OrderItemResponse
            {
                Id = request.Id,
                OrderId = request.OrderId,
                Quantity = request.Quantity,
                ProductId = request.ProductId,
                Price = request.Price
            };
        }
        public static OrderItem MapToOrderItem(this UpdateOrderItemRequest request)
        {
            return new OrderItem
            {
                Id = request.Id,
                OrderId = request.OrderId,
                Quantity = request.Quantity,
                ProductId = request.ProductId
            };
        }

    }
}
