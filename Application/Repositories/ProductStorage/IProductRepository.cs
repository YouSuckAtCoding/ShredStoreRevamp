using Application.Models;
using Contracts.Response.ProductsResponses;

namespace Application.Repositories.ProductStorage
{
    public interface IProductRepository
    {
        Task DeleteProduct(int id, CancellationToken token);
        Task<IEnumerable<ProductCartItemResponse>> GetCartProducts(int cartId, CancellationToken token);
        Task<Product> GetProduct(int id, CancellationToken token);
        Task<IEnumerable<Product>> GetProducts(CancellationToken token);
        Task<IEnumerable<Product>> GetProductsByCategory(string Category, CancellationToken token);
        Task<IEnumerable<Product>> GetProductsByUserId(int Id, CancellationToken token);
        Task InsertProduct(Product Product, CancellationToken token);
        Task UpdateProduct(Product Product, CancellationToken token);
    }
}