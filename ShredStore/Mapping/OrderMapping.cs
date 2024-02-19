using Application.Models;
using Contracts.Request.OrderRequests;
using Contracts.Response.OrderResponses;

namespace ShredStore.Mapping
{
    public static class OrderMapping
    {
        public static Order MapToOrder(this CreateOrderRequest request)
        {
            return new Order()
            {
                CreatedDate = request.CreatedDate,
                UserId = request.UserId,
                TotalAmount = request.TotalAmount,
                PaymentId = request.PaymentId
            };
        }
        public static Order MapToOrder(this UpdateOrderRequest request)
        {
            return new Order()
            {
                Id = request.Id,
                CreatedDate = request.CreatedDate,
                UserId = request.UserId,
                TotalAmount = request.TotalAmount,
                PaymentId = request.PaymentId
            };
        }

        public static OrderResponse MapToOrderResponse(this Order request)
        {
            return new OrderResponse
            {
                Id = request.Id,
                CreatedDate = request.CreatedDate,
                UserId = request.UserId,
                TotalAmount = request.TotalAmount,
                PaymentId = request.PaymentId
            };
        }
    }
}
