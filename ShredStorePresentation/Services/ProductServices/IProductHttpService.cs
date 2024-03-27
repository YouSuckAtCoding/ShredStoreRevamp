using Contracts.Request.ProductRequests;
using Contracts.Response.ProductsResponses;
using ShredStorePresentation.Models;

namespace ShredStorePresentation.Services.ProductServices
{
    public interface IProductHttpService
    {
        Task<ProductResponse> Create(CreateProductRequest product, CancellationToken token);
        Task Delete(int id, CancellationToken token);
        Task<ProductResponse> Edit(UpdateProductRequest product, CancellationToken token);
        Task<IEnumerable<ProductResponse>> GetAll(CancellationToken token);
        Task<IEnumerable<ProductResponse>> GetAllByCategory(string Category, CancellationToken token);
        Task<IEnumerable<ProductResponse>> GetAllByUserId(int UserId, CancellationToken token);
        Task<IEnumerable<ProductCartItemResponse>> GetAllByCartId(int cartId, CancellationToken token);
        Task<ProductResponse> GetById(int id, CancellationToken token);
    }
}