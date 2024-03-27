using Contracts.Request.CartRequests;
using Contracts.Response.CartResponses;

namespace ShredStorePresentation.Services.CartServices
{
    public interface ICartHttpService
    {
        Task<CreateCartRequest> Create(CreateCartRequest cart, CancellationToken token);
        Task<CartResponse> GetById(int id, CancellationToken token);
    }
}