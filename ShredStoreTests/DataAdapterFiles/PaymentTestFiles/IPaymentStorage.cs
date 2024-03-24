using Application.Models;

namespace ShredStoreTests.DataAdapterFiles.PaymentTestFiles
{
    public interface IPaymentStorage
    {
        Task InsertPayment(Payment Payment);

        Task<IEnumerable<Payment>> GetPayments();
    }
}