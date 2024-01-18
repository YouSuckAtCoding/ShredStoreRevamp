using Application.Models;
using Application.Repositories;
using Dapper;
using FluentAssertions;
using ShredStoreTests.DataAdapterFiles;
using ShredStoreTests.DataAdapterFiles.CartItemTestFiles;
using ShredStoreTests.DataAdapterFiles.CartTestFiles;
using ShredStoreTests.DataAdapterFiles.OrderTestFiles;
using ShredStoreTests.DataAdapterFiles.ProductTestFiles;
using ShredStoreTests.DataAdapterFiles.UserTestFiles;
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
            await SetUpMultipleCartItems();
            IOrderStorage orderStorage = new OrderStorage(_dbConnectionFactory);
            Order order = FakeDataFactory.FakeOrder();
            await orderStorage.InsertOrder(order);

            var res = await orderStorage.GetOrders(order.UserId);
            res.First().CartId.Should().Be(order.CartId);

            await Utility.CleanUpOrders(_dbConnectionFactory);
            await Utility.ClearCartItems(_dbConnectionFactory);
            await Utility.CleanUpCarts(_dbConnectionFactory);

        }
        [Fact]
        public async Task TotalAmount_Should_Be_Equal_To_CartItem_Price_Multiplied_Product_Quantity()
        {
            decimal expected = await SetUpCartItemPriceTests();

            IOrderStorage orderStorage = new OrderStorage(_dbConnectionFactory);
            Order order = FakeDataFactory.FakeOrder();
            await orderStorage.InsertOrder(order);

            var res = await orderStorage.GetOrders(order.UserId);
            res.First().TotalAmount.Should().Be(expected);

            await Utility.CleanUpOrders(_dbConnectionFactory);
            await Utility.ClearCartItems(_dbConnectionFactory);
            await Utility.CleanUpCarts(_dbConnectionFactory);

        }
        [Fact]
        public async Task Should_Delete_Order()
        {
            await SetUpMultipleCartItems();
            IOrderStorage orderStorage = new OrderStorage(_dbConnectionFactory);
            Order order = FakeDataFactory.FakeOrder();
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
            await SetUpMultipleCartItems();
            IOrderStorage orderStorage = new OrderStorage(_dbConnectionFactory);
            Order order = FakeDataFactory.FakeOrder();
            await orderStorage.InsertOrder(order);

            var res = await orderStorage.GetOrders(order.UserId);

            order.Id = res.First().Id;
            DateTime newDate = new DateTime(2019, 05, 09, 09, 15, 00);
            order.Date = newDate;
            await orderStorage.UpdateOrder(order);

            res = await orderStorage.GetOrders(order.UserId);

            res.First().Date.Should().Be(newDate);

            await Utility.CleanUpOrders(_dbConnectionFactory);
            await Utility.ClearCartItems(_dbConnectionFactory);
            await Utility.CleanUpCarts(_dbConnectionFactory);
        }
        private async Task<decimal> SetUpCartItemPriceTests()
        {

            User user = FakeDataFactory.FakeUser();
            IUserStorage userStorage = new UserStorage(_dbConnectionFactory);
            await userStorage.InsertUser(user);

            IProductStorage productStorage = new ProductStorage(_dbConnectionFactory);
            Product product = FakeDataFactory.FakeProduct();
            await productStorage.InsertProduct(product);

            Cart cart = FakeDataFactory.FakeCart();
            ICartStorage cartStorage = new CartStorage(_dbConnectionFactory);
            await cartStorage.InsertCart(cart);

            ICartItemStorage cartItemStorage = new CartItemStorage(_dbConnectionFactory);

            CartItem cartItem = new CartItem
            {
                ProductId = 1,
                CartId = 1,
                Quantity = 4
            };
            cartItem.Price = product.Price * cartItem.Quantity;
            await cartItemStorage.InsertCartItem(cartItem);

            return product.Price * cartItem.Quantity;

        }
        private async Task SetUpMultipleCartItems()
        {
            Cart cart = FakeDataFactory.FakeCart();
            User user = FakeDataFactory.FakeUser();
            IEnumerable<Product> products = FakeDataFactory.FakeProducts();

            IUserStorage userStorage = new UserStorage(_dbConnectionFactory);
            await userStorage.InsertUser(user);

            ICartStorage cartStorage = new CartStorage(_dbConnectionFactory);
            await cartStorage.InsertCart(cart);

            IProductStorage productStorage = new ProductStorage(_dbConnectionFactory);

            foreach (Product product in products)
            {
                await productStorage.InsertProduct(product);
            }

            IEnumerable<Product> returnedProducts = await productStorage.GetProducts();

            ICartItemStorage cartItemStorage = new CartItemStorage(_dbConnectionFactory);

            foreach (Product product in returnedProducts)
            {
                CartItem cartItem = new CartItem
                {
                    ProductId = product.Id,
                    CartId = 1,
                    Quantity = 4
                };
                cartItem.Price = product.Price * cartItem.Quantity;
                await cartItemStorage.InsertCartItem(cartItem);
            }
        }

    }
}
