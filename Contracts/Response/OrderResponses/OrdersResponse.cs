using Contracts.Response.UserResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Response.OrderResponses
{
    public class OrdersResponse
    {
        public required IEnumerable<OrderResponse> Items { get; init; } = Enumerable.Empty<OrderResponse>();
    }
}
