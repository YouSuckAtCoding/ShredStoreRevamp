using Application.Models;

namespace ShredStoreTests.DataAdapterFiles.ProductTestFiles
{
    public interface IProductStorage
    {
        Task DeleteProduct(int id);
        Task<Product?> GetProduct(int id);
        Task<IEnumerable<Product>> GetProducts();
        Task InsertProduct(Product Product);
        Task UpdateProduct(Product Product);
    }
}