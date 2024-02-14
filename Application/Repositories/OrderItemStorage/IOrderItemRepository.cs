using Application.Models;

namespace Application.Repositories.OrderItemStorage
{
    public interface IOrderItemRepository
    {
        Task DeleteAllOrderItem(int OrderId, CancellationToken token);
        Task DeleteOrderItem(int ProductId, int OrderId, CancellationToken token);
        Task<OrderItem?> GetOrderItem(int ProductId, int OrderId, CancellationToken token);
        Task<IEnumerable<OrderItem>> GetOrderItems(int id, CancellationToken token);
        Task InsertOrderItem(OrderItem OrderItem, CancellationToken token);
        Task UpdateOrderItem(int ProductId, int Quantity, int OrderId, CancellationToken token);
    }
}