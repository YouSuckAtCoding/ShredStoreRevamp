using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Request.PaymentRequests
{
    public enum PaymentType
    {
        Credito = 1,
        Debito = 2,
        Pix = 3
    }
    public class CreatePaymentRequest
    {


        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public PaymentType PaymentType { get; set; }
        public bool Payed { get; set; }

    }
}
