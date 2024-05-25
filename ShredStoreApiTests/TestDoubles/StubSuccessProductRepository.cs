using Application.Models;
using Application.Services.ProductServices;
using Contracts.Response.ProductsResponses;
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

        public Task<IEnumerable<ProductCartItemResponse>> GetCartProducts(int cartId, CancellationToken token)
        {
            return Task.FromResult(Enumerable.Empty<ProductCartItemResponse>());
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

        public Task<IEnumerable<Product>> GetProductsByCategory(string category, CancellationToken token)
        {
            var fake = FakeDataFactory.FakeProducts();
            return Task.FromResult(fake);
        }

        public Task<IEnumerable<Product>> GetProductsByUser(int id, CancellationToken token)
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
