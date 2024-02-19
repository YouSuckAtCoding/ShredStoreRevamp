using Application.Models;

namespace ShredStoreTests.DataAdapterFiles.OrderTestFiles
{
    public interface IOrderStorage
    {
        Task DeleteOrder(int id);
        Task<Order?> GetOrder(int id);
        Task<IEnumerable<Order>> GetOrders(int UserId);
        Task InsertOrder(Order Order);
        Task UpdateOrder(Order Order);
    }
}