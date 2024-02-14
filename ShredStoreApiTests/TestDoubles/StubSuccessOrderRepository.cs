using Application.Models;
using Application.Services.OrderServices;
using ShredStoreTests.Fake;

namespace ShredStoreApiTests.TestDoubles
{
    internal class StubSuccessOrderRepository : IOrderService
    {
        public Task<bool> DeleteOrder(int id, CancellationToken token)
        {
            return Task.FromResult(true);
        }

        public Task<Order?> GetOrder(int id, CancellationToken token)
        {
            var fake = FakeDataFactory.FakeOrder();
            return Task.FromResult(fake)!;
        }

        public Task<IEnumerable<Order>> GetOrders(int userId, CancellationToken token)
        {
            var fake = FakeDataFactory.FakeOrders();
            return Task.FromResult(fake);
        }

        public Task<bool> InsertOrder(Order order, CancellationToken token)
        {
            return Task.FromResult(true);
        }

        public Task<Order> UpdateOrder(Order order, CancellationToken token)
        {
            var fake = FakeDataFactory.FakeOrder();
            return Task.FromResult(fake)!;
        }
    }
}
