using Application.Models;

namespace Application.Services.OrderItemServices
{
    public interface IOrderItemService
    {
        Task<bool> CreateOrderItem(OrderItem OrderItem, CancellationToken token);
        Task<bool> DeleteAlltems(int cartId, CancellationToken token);
        Task<bool> DeleteItem(int itemId, int cartId, CancellationToken token);
        Task<OrderItem> GetOrderItem(int itemId, int cartId, CancellationToken token);
        Task<IEnumerable<OrderItem>> GetOrderItems(int cartId, CancellationToken token);
        Task UpdateOrderItem(int itemId, int cartId, int quantity, CancellationToken token);
    }
}