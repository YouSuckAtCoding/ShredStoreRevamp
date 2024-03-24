using Contracts.Request.ProductRequests;
using Contracts.Response.ProductsResponses;
using ShredStorePresentation.Models;

namespace ShredStorePresentation.Services.ProductServices
{
    public interface IProductHttpService
    {
        Task<ProductResponse> Create(CreateProductRequest product);
        Task Delete(int id);
        Task<ProductResponse> Edit(UpdateProductRequest product);
        Task<IEnumerable<ProductResponse>> GetAll();
        Task<IEnumerable<ProductResponse>> GetAllByCategory(string Category);
        Task<IEnumerable<ProductResponse>> GetAllByUserId(int UserId);
        Task<ProductResponse> GetById(int id);
    }
}