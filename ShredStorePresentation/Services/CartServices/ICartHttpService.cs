using Contracts.Request.CartRequests;
using Contracts.Response.CartResponses;

namespace ShredStorePresentation.Services.CartServices
{
    public interface ICartHttpService
    {
        Task<CreateCartRequest> Create(CreateCartRequest cart,string token);
        Task<CartResponse> GetById(int id,string token);
    }
}