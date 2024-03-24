using Application.Models;

namespace Application.Services.ProductServices
{
    public interface IProductService
    {
        Task<bool> Create(Product product, CancellationToken token);
        Task<bool> DeleteProduct(int id, CancellationToken token);
        Task<Product?> GetProduct(int id, CancellationToken token);
        Task<IEnumerable<Product>> GetProducts(CancellationToken token);
        Task<IEnumerable<Product>> GetProductsByCategory(string category, CancellationToken token);
        Task<IEnumerable<Product>> GetProductsByUser(int id, CancellationToken token);
        Task<Product?> UpdateProduct(Product product, CancellationToken token);
    }
}