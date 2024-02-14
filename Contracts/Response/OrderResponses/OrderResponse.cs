using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Response.OrderResponses
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public int PaymentId { get; set; }
    }
}
