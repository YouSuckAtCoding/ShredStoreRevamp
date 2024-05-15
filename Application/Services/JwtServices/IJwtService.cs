using Contracts.Response.JwtResponses;

namespace Application.Services.JwtServices
{
    public interface IJwtService
    {
        JwtDecodedResponse DecodeJwtToken(string token);
    }
}