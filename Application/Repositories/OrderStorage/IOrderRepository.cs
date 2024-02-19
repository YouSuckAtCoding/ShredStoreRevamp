using Application.Models;

namespace Application.Repositories
{
    public interface IOrderRepository
    {
        Task DeleteOrder(int id, CancellationToken token);
        Task<IEnumerable<Order>> GetAllOrders(CancellationToken token);
        Task<Order?> GetOrder(int id, CancellationToken token);
        Task<IEnumerable<Order>> GetOrders(int UserId, CancellationToken token);
        Task InsertOrder(Order Order, CancellationToken token);
        Task UpdateOrder(Order Order, CancellationToken token);
    }
}