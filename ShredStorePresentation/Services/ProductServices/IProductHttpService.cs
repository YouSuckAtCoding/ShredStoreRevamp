using Contracts.Request.ProductRequests;
using Contracts.Response.ProductsResponses;
using ShredStorePresentation.Models;

namespace ShredStorePresentation.Services.ProductServices
{
    public interface IProductHttpService
    {
        Task<ProductResponse> Create(CreateProductRequest product, string token);
        Task Delete(int id, string token);
        Task<ProductResponse> Edit(UpdateProductRequest product, string token);
        Task<IEnumerable<ProductResponse>> GetAll();
        Task<IEnumerable<ProductResponse>> GetAllByCategory(string Category);
        Task<IEnumerable<ProductResponse>> GetAllByUserId(int UserId, string token);
        Task<IEnumerable<ProductCartItemResponse>> GetAllByCartId(int cartId, string token);
        Task<ProductResponse> GetById(int id);
    }
}