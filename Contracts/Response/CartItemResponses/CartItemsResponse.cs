using Contracts.Response.UserResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Response.CartItemResponses
{
    public class CartItemsResponse
    {
        public required IEnumerable<CartItemResponse> Items { get; init; } = Enumerable.Empty<CartItemResponse>();
    }
}
