using Contracts.Response.UserResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Response.ProductsResponses
{
    public class ProductsResponse
    {
        public required IEnumerable<ProductResponse> Items { get; init; } = Enumerable.Empty<ProductResponse>();
    }
}
