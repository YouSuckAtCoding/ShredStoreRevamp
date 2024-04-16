using Contracts.Request.JwtRequests;
using Contracts.Response.JwtResponses;
using Contracts.Response.UserResponses;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.Json;

namespace ShredStorePresentation.Services.JtwService
{
    public class JwtHttpService : IJwtHttpService
    {
        private readonly HttpClient httpClient;

        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            IgnoreNullValues = true,
            PropertyNameCaseInsensitive = true

        };

        public JwtHttpService(IConfiguration config)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(config.GetValue<string>("JwtUri")!);
        }

        public async Task<JwtGenerateResponse> GenerateJwtToken(JwtGenerateRequest request)
        {
            var httpResponseMessage = await httpClient.PostAsJsonAsync(ApiEndpoints.TokenEndpoints.GenerateToken, request);

            JwtGenerateResponse res = new JwtGenerateResponse
            {
                Token = await httpResponseMessage.Content.ReadAsStringAsync()
            };

            return res;

        }

        public string GetJtwData(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var jti = jwtToken.Claims.First(claim => claim.Type == "jti").Value;
            return jti;

        }
    }
}
