using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Response.JwtResponses
{
    public record JwtDecodedResponse
    {
        public required string KeyId { get; init; }
        public required string Issuer { get; init; }
        public required IEnumerable<string> Audiences { get; init; }
        public required IEnumerable<Claim> Claims { get; init; }
        public required DateTime ValidTo { get; init; }
        public required string SignatureAlgorithm { get; init; }
        public required string RawData { get; init; }
        public required string Subject { get; init; }
        public required DateTime ValidFrom { get; init; }
        public required string EncodedHeader { get; init; }
        public required string EncodedPayload { get; init; }
    }
}
