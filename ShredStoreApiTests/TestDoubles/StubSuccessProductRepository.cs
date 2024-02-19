using Application.Models;
using Application.Services.ProductServices;
using ShredStoreTests.Fake;

namespace ShredStoreApiTests.TestDoubles
{
    public class StubSuccessProductRepository : IProductService
    {
        public Task<bool> Create(Product product, CancellationToken token)
        {
            return Task.FromResult(true);
        }

        public Task<bool> DeleteProduct(int id, CancellationToken token)
        {
            return Task.FromResult(true);
        }

        public Task<Product?> GetProduct(int id, CancellationToken token)
        {
            var fake = FakeDataFactory.FakeProduct();
            return Task.FromResult(fake)!;
        }

        public Task<IEnumerable<Product>> GetProducts(CancellationToken token)
        {
            var fake = FakeDataFactory.FakeProducts();
            return Task.FromResult(fake);
        }

        public Task<Product> UpdateProduct(Product product, CancellationToken token)
        {
            var fake = FakeDataFactory.FakeProduct();
            return Task.FromResult(fake);
        }
    }
}
