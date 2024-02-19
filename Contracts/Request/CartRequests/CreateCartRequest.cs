using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Request.CartRequests
{
    public class CreateCartRequest
    {
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
