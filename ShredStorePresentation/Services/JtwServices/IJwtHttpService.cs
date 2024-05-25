using Contracts.Request.JwtRequests;
using Contracts.Response.JwtResponses;

namespace ShredStorePresentation.Services.JtwService
{
    public interface IJwtHttpService
    {
        Task<JwtGenerateResponse> GenerateJwtToken(JwtGenerateRequest request);
    }
}