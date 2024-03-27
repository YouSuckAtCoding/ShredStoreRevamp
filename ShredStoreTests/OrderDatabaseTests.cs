using Application.Models;
using Dapper;
using FluentAssertions;
using ShredStoreTests.DataAdapterFiles;
using ShredStoreTests.DataAdapterFiles.OrderTestFiles;
using ShredStoreTests.DataAdapterFiles.PaymentTestFiles;
using ShredStoreTests.Fake;

namespace ShredStoreTests
{
    public class OrderDatabaseTests : IClassFixture<SqlAccessFixture>
    {
        private readonly ISqlAccessConnectionFactory _dbConnectionFactory;
        public OrderDatabaseTests(SqlAccessFixture fixture)
        {
            _dbConnectionFactory = fixture.ConnectionFactory;
        }

        [Fact]
        public void Should_Throw_ArgumentNullException_If_Missing_Connection_String()
        {
            var create = () => new OrderStorage(null!);
            create.Should().ThrowExactly<ArgumentNullException>();
        }
        [Theory]
        [InlineData("spOrder_Insert")]
        [InlineData("spOrder_GetById")]
        [InlineData("spOrder_GetAllUserOrders")]
        [InlineData("spOrder_Delete")]
        [InlineData("spOrder_Update")]
        public async Task Should_Be_True_If_Stored_Procedure_Exists(string Sp)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(default);
            string spName = Sp;
            dynamic res = await connection.QueryAsync(Utility.CreateQueryForStoredProcedureCheck(spName));
            string name = res[0].name;
            name.Should().Be(spName);
        }
        [Fact]
        public async Task Should_Insert_Order_If_Doenst_Exists()
        {
            IOrderStorage orderStorage = new OrderStorage(_dbConnectionFactory);
            Order order = FakeDataFactory.FakeOrder();
            order.UserId = await Utility.GenerateUserId(_dbConnectionFactory);
            int paymentId = await GeneratePaymentId();
            order.PaymentId = paymentId;

            await orderStorage.InsertOrder(order);
            var res = await orderStorage.GetOrders(order.UserId);

            res.First().UserId.Should().Be(order.UserId);

            await Utility.CleanUpOrders(_dbConnectionFactory);
            await Utility.ClearCartItems(_dbConnectionFactory);
            await Utility.CleanUpCarts(_dbConnectionFactory);

        }

        [Fact]
        public async Task TotalAmount_Should_Be_Equal_To_CartItem_Price_Multiplied_Product_Quantity()
        {
            IOrderStorage orderStorage = new OrderStorage(_dbConnectionFactory);
            Order order = FakeDataFactory.FakeOrder();
            order.PaymentId = await GeneratePaymentId();
            order.TotalAmount = 1000;
            order.UserId = await Utility.GenerateUserId(_dbConnectionFactory);

            await orderStorage.InsertOrder(order);
            var res = await orderStorage.GetOrders(order.UserId);
            
            res.First().TotalAmount.Should().Be(1000);

            await Utility.CleanUpOrders(_dbConnectionFactory);
            await Utility.ClearCartItems(_dbConnectionFactory);
            await Utility.CleanUpCarts(_dbConnectionFactory);

        }
        [Fact]
        public async Task Should_Delete_Order()
        {
            IOrderStorage orderStorage = new OrderStorage(_dbConnectionFactory);
            Order order = FakeDataFactory.FakeOrder();
            order.PaymentId = await GeneratePaymentId();
            order.UserId = await Utility.GenerateUserId(_dbConnectionFactory);

            await orderStorage.InsertOrder(order);
            var res = await orderStorage.GetOrders(order.UserId);
            await orderStorage.DeleteOrder(res.First().Id);
            res = await orderStorage.GetOrders(order.UserId);

            res.Should().BeEmpty();

            await Utility.CleanUpOrders(_dbConnectionFactory);
            await Utility.ClearCartItems(_dbConnectionFactory);
            await Utility.CleanUpCarts(_dbConnectionFactory);
        }
        [Fact]
        public async Task Should_Update_Order_Date()
        {

            IOrderStorage orderStorage = new OrderStorage(_dbConnectionFactory);
            Order order = FakeDataFactory.FakeOrder();
            order.PaymentId = await GeneratePaymentId();
            order.UserId = await Utility.GenerateUserId(_dbConnectionFactory);
            DateTime newDate = new DateTime(2019, 05, 09, 09, 15, 00);

            await orderStorage.InsertOrder(order);
            var res = await orderStorage.GetOrders(order.UserId);
            order.Id = res.First().Id;
            order.CreatedDate = newDate;
            await orderStorage.UpdateOrder(order);
            res = await orderStorage.GetOrders(order.UserId);

            res.First().CreatedDate.Should().Be(newDate);

            await Utility.CleanUpOrders(_dbConnectionFactory);
            await Utility.ClearCartItems(_dbConnectionFactory);
            await Utility.CleanUpCarts(_dbConnectionFactory);
        }
        private async Task<int> GeneratePaymentId()
        {
            IPaymentStorage paymentStorage = new PaymentStorage(_dbConnectionFactory);
            await paymentStorage.InsertPayment(FakeDataFactory.FakePayment());
            IEnumerable<Payment> payments = await paymentStorage.GetPayments();
            int paymentId = payments.First().Id;
            return paymentId;
        }

    }
}
