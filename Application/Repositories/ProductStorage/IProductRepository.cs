using Application.Models;

namespace Application.Repositories.ProductStorage
{
    public interface IProductRepository
    {
        Task DeleteProduct(int id);
        Task<Product?> GetProduct(int id);
        Task<IEnumerable<Product>> GetProducts();
        Task InsertProduct(Product Product);
        Task UpdateProduct(Product Product);
    }
}