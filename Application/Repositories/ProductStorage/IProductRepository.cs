using Application.Models;

namespace Application.Repositories.ProductStorage
{
    public interface IProductRepository
    {
        Task DeleteProduct(int id, CancellationToken token);
        Task<Product?> GetProduct(int id, CancellationToken token);
        Task<IEnumerable<Product>> GetProducts(CancellationToken token);
        Task InsertProduct(Product Product, CancellationToken token);
        Task UpdateProduct(Product Product, CancellationToken token);
        Task<IEnumerable<Product>> GetProductsByCategory(string category, CancellationToken token);
        Task<IEnumerable<Product>> GetProductsByUserId(int id, CancellationToken token);
    }
}