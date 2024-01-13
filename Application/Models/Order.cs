using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int CartId { get; set; }
        public decimal TotalAmount { get; set; }
        public int UserId { get; set; }
    }
}
