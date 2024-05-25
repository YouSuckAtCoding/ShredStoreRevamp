using Application.Extensions;
using Contracts.Response.JwtResponses;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Services.JwtServices
{
    public class JwtService : IJwtService
    {
        private const string AuthScheme = "Bearer ";
        public JwtDecodedResponse DecodeJwtToken(string token)
        {
            token = token.Replace(AuthScheme, "");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            JwtDecodedResponse response = jwtToken.MapToDecodedResponse();

            return response;

        }




    }
}
