using Application.Models;

namespace Application.Repositories
{
    public interface IOrderRepository
    {
        Task DeleteOrder(int id);
        Task<Order?> GetOrder(int id);
        Task<IEnumerable<Order>> GetOrders(int UserId);
        Task InsertOrder(Order Order);
        Task UpdateOrder(Order Order);
    }
}