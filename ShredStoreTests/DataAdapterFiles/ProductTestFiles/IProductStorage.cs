using Application.Models;
using Contracts.Response.ProductsResponses;

namespace ShredStoreTests.DataAdapterFiles.ProductTestFiles
{
    public interface IProductStorage
    {
        Task DeleteProduct(int id);
        Task<IEnumerable<ProductCartItemResponse>> GetCartProducts(int cartId, CancellationToken token);
        Task<Product?> GetProduct(int id);
        Task<IEnumerable<Product>> GetProducts();
        Task<IEnumerable<Product>> GetProductsByCategory(string Category, CancellationToken token);
        Task<IEnumerable<Product>> GetProductsByUserId(int Id, CancellationToken token);
        Task InsertProduct(Product Product);
        Task UpdateProduct(Product Product);
    }
}