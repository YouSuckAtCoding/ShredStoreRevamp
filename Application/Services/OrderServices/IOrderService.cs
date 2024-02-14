using Application.Models;
using Contracts.Response.OrderResponses;

namespace Application.Services.OrderServices
{
    public interface IOrderService
    {
        Task<bool> DeleteOrder(int id, CancellationToken token);
        Task<Order?> GetOrder(int id, CancellationToken token);
        Task<IEnumerable<Order>> GetOrders(int userId, CancellationToken token);
        Task<bool> InsertOrder(Order order, CancellationToken token);
        Task<Order?> UpdateOrder(Order order, CancellationToken token);
    }
}