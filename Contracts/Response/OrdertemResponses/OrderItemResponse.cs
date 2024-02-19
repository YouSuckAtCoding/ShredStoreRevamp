using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Response.OrdertemResponses
{
    public class OrderItemResponse
    {
        public int Id { get; init; }
        public int OrderId { get; init; }
        public decimal Price { get; init; }
        public int Quantity { get; init; }
        public int ProductId { get; init; }
    }
}
