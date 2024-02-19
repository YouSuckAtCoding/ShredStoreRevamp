using Application.Models;
using Application.Services.OrderItemServices;
using ShredStoreTests.Fake;

namespace ShredStoreApiTests.TestDoubles
{
    public class StubSuccessOrderItemRepository : IOrderItemService
    {
        public Task<bool> CreateOrderItem(OrderItem OrderItem, CancellationToken token)
        {
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAlltems(int OrderId, CancellationToken token)
        {
            return Task.FromResult(true);
        }

        public Task<bool> DeleteItem(int itemId, int OrderId, CancellationToken token)
        {
            return Task.FromResult(true);
        }

        public Task<OrderItem> GetOrderItem(int itemId, int OrderId, CancellationToken token)
        {
            return Task.FromResult(new OrderItem());
        }

        public Task<IEnumerable<OrderItem>> GetOrderItems(int OrderId, CancellationToken token)
        {
            var result = FakeDataFactory.FakeOrderItems();
            return Task.FromResult(result);
        }

        Task<OrderItem?> IOrderItemService.UpdateOrderItem(int itemId, int cartId, int quantity, CancellationToken token)
        {
            var result = FakeDataFactory.FakeOrderItems();
            return Task.FromResult(result.First())!;
        }
    }
}
