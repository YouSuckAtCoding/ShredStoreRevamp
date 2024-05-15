using Contracts.Response.JwtResponses;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Extensions
{
    public static class JwtExtensions
    {
        public static JwtDecodedResponse MapToDecodedResponse(this JwtSecurityToken token)
        {
            return new JwtDecodedResponse
            {
                KeyId = token.Id,
                Issuer = token.Issuer,
                Audiences = token.Audiences,
                Claims = token.Claims,
                ValidTo = token.ValidTo,
                SignatureAlgorithm = token.SignatureAlgorithm,
                RawData = token.RawData,
                Subject = token.Subject,
                ValidFrom = token.ValidFrom,
                EncodedHeader = token.EncodedHeader,
                EncodedPayload = token.EncodedPayload
            };

        }
    }
}
