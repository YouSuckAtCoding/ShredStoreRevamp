using Application.Models;
using Application.Services.CartServices;
using ShredStoreTests.Fake;

namespace ShredStoreApiTests.TestDoubles
{
    public class StubSuccessCartRepository : ICartService
    {
        public Task<bool> Create(Cart cart, CancellationToken token)
        {
            return Task.FromResult(true);
        }

        public Task DeleteCart(int id, CancellationToken token)
        {
            return Task.FromResult(true);
        }

        public Task<Cart?> GetCart(int id, CancellationToken token)
        {
            var res = FakeDataFactory.FakeCart();
            return Task.FromResult(res)!;
        }
    }
}
