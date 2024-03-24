using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShredStoreTests.DataAdapterFiles.PaymentTestFiles
{
    public class PaymentStorage : IPaymentStorage
    {
        private TestSqlDataAccess _dataAccess;

        public PaymentStorage(ISqlAccessConnectionFactory dbConnectionFactory)
        {
            ArgumentNullException.ThrowIfNull(dbConnectionFactory);
            _dataAccess = new TestSqlDataAccess(dbConnectionFactory);
        }

        public Task InsertPayment(Payment Payment) =>
           _dataAccess.SaveData("dbo.spPayment_Insert", new { Payment.Amount, Payment.Date, Payment.PaymentType, Payment.Payed });

        public Task<IEnumerable<Payment>> GetPayments() => _dataAccess.LoadData<Payment, dynamic>("dbo.spPayment_GetAll", new { });
    }
}
