using Contracts.Request.JwtRequests;
using Contracts.Response.JwtResponses;
using Contracts.Response.UserResponses;
using ShredStorePresentation.Services.JtwService;

namespace ShredStorePresentation.Services.JtwServices
{
    public class JwtGenerationService : IJwtGenerationService
    {
        private readonly IJwtHttpService _jwtHttpService;

        public JwtGenerationService(IJwtHttpService jwtHttpService)
        {
            _jwtHttpService = jwtHttpService;
        }

        public Dictionary<string, object> GenerateClaims(string role)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            string allowed = "true";
            switch (role)
            {
                case Role.Admin:
                    dict.Add(Claims.Admin, allowed);
                    break;
                case Role.Shop:
                    dict.Add(Claims.Shop, allowed);
                    break;
                case Role.Customer:
                    dict.Add(Claims.Customer, allowed);
                    break;
            }
            return dict;
        }
        public async Task<JwtGenerateResponse> GenerateToken(UserResponse loggedUser)
        {
            JwtGenerateRequest request = new JwtGenerateRequest
            {
                Email = loggedUser.Email,
                UserId = loggedUser.Id,
                Claims = GenerateClaims(loggedUser.Role)
            };

            JwtGenerateResponse token = await _jwtHttpService.GenerateJwtToken(request);
            return token;
        }
    }
}
