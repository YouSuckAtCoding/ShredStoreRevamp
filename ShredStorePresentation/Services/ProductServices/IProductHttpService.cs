using ShredStorePresentation.Models;

namespace ShredStorePresentation.Services.ProductServices
{
    public interface IProductHttpService
    {
        Task<ProductViewResponse> Create(ProductViewResponse product);
        Task Delete(int id);
        Task<ProductViewResponse> Edit(ProductViewResponse product);
        Task<IEnumerable<ProductViewResponse>> GetAll();
        Task<IEnumerable<ProductViewResponse>> GetAllByCategory(string Category);
        Task<IEnumerable<ProductViewResponse>> GetAllByUserId(int UserId);
        Task<ProductViewResponse> GetById(int id);
    }
}