using Contracts.Response.JwtResponses;
using Contracts.Response.UserResponses;

namespace ShredStorePresentation.Services.JtwServices
{
    public interface IJwtService
    {
        Dictionary<string, object> GenerateClaims(string role);
        Task<JwtGenerateResponse> GenerateToken(UserResponse loggedUser);
    }
}