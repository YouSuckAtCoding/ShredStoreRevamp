using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Request.OrderRequests
{
    public class CreateOrderRequest
    {
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }

        public int PaymentId { get; set; }

    }
}
