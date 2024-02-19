using Application.Models;
using Dapper;
using FluentAssertions;
using ShredStoreTests.DataAdapterFiles;
using ShredStoreTests.DataAdapterFiles.CartItemTestFiles;
using ShredStoreTests.DataAdapterFiles.CartTestFiles;
using ShredStoreTests.DataAdapterFiles.ProductTestFiles;
using ShredStoreTests.DataAdapterFiles.UserTestFiles;
using ShredStoreTests.Fake;
using System.Data.SqlClient;


namespace ShredStoreTests
{
    public class CartItemDatabaseTests : IClassFixture<SqlAccessFixture>
    {
        private readonly ISqlAccessConnectionFactory _dbConnectionFactory;
        public CartItemDatabaseTests(SqlAccessFixture fixture)
        {
            _dbConnectionFactory = fixture.ConnectionFactory;
        }

        [Fact]
        public void Should_Throw_ArgumentNullException_If_Missing_Connection_String()
        {
            var create = () => new CartItemStorage(null!);
            create.Should().ThrowExactly<ArgumentNullException>();
        }
        [Theory]
        [InlineData("spCartItem_Insert")]
        [InlineData("spCartItem_GetAll")]
        [InlineData("spCartItem_Update")]
        [InlineData("spCartItem_Delete")]
        [InlineData("spCartItem_DeleteAll")]
        public async Task Should_Be_True_If_Stored_Procedure_Exists(string Sp)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(default);
            string spName = Sp;
            dynamic res = await connection.QueryAsync(Utility.CreateQueryForStoredProcedureCheck(spName));
            string name = res[0].name;
            name.Should().Be(spName);
        }

        [Fact]
        public async Task Should_Throw_SqlExecption_If_Product_Doenst_Exists()
        {
            await SetUpCartItem();
            CartItem cartItem = new CartItem
            {
                ProductId = 50,
                CartId = 1,
                Quantity = 5

            };

            ICartItemStorage cartItemStorage = new CartItemStorage(_dbConnectionFactory);
            var res = () => cartItemStorage.InsertCartItem(cartItem);
            await res.Should().ThrowExactlyAsync<SqlException>();
        }

        [Fact]
        public async Task Should_Throw_SqlExecption_If_Cart_Doenst_Exists()
        {

            CartItem cartItem = new CartItem
            {
                ProductId = 1,
                CartId = 2,
                Quantity = 1
            };

            ICartItemStorage cartItemStorage = new CartItemStorage(_dbConnectionFactory);
            var res = () => cartItemStorage.InsertCartItem(cartItem);
            await res.Should().ThrowExactlyAsync<SqlException>();
        }

        [Fact]
        public async Task Should_Insert_Item_If_Doenst_Exists()
        {

            Product prod = FakeDataFactory.FakeProduct();
            IProductStorage productStorage = new ProductStorage(_dbConnectionFactory);

            await productStorage.InsertProduct(prod);

            CartItem cartItem = new CartItem
            {
                ProductId = 1,
                CartId = 1,
                Quantity = 1
            };
            ICartItemStorage cartItemStorage = new CartItemStorage(_dbConnectionFactory);
            await cartItemStorage.InsertCartItem(cartItem);
            var res = await cartItemStorage.GetCartItems(1);

            res.Should().HaveCount(1);
            await Utility.ClearCartItems(_dbConnectionFactory);
        }

        [Fact]
        public async Task Should_Insert_Multiple_Cart_Items()
        {

            IEnumerable<Product> prods = FakeDataFactory.FakeProducts();
            IProductStorage productStorage = new ProductStorage(_dbConnectionFactory);

            foreach(Product product in prods)
            {
                await productStorage.InsertProduct(product);
            }

            IEnumerable<Product> products = await productStorage.GetProducts();

            ICartItemStorage cartItemStorage = new CartItemStorage(_dbConnectionFactory);

            foreach(Product product in products)
            {
                CartItem cartItem = new CartItem
                {
                    ProductId = product.Id,
                    CartId = 1,
                    Quantity = 1
                    
                };
                await cartItemStorage.InsertCartItem(cartItem);
            }
            
            var res = await cartItemStorage.GetCartItems(1);
            res.Should().HaveCountGreaterThanOrEqualTo(10);

            await Utility.ClearCartItems(_dbConnectionFactory);
        }


        [Fact]
        public async Task Should_Delete_CartItem()
        {
            

            Product prod = FakeDataFactory.FakeProduct();
            IProductStorage productStorage = new ProductStorage(_dbConnectionFactory);

            await productStorage.InsertProduct(prod);

            var returned = await productStorage.GetProducts();

            CartItem cartItem = new CartItem
            {
                ProductId = returned.First().Id,
                CartId = 1,
                Quantity = 1
            };
            ICartItemStorage cartItemStorage = new CartItemStorage(_dbConnectionFactory);
            await cartItemStorage.InsertCartItem(cartItem);

            await cartItemStorage.DeleteCartItem(returned.First().Id,1);

            var res = await cartItemStorage.GetCartItems(1);

            res.Should().HaveCount(0);
            await Utility.ClearCartItems(_dbConnectionFactory);
        }
        [Fact]
        public async Task Should_Be_Empty_After_Delete_All()
        {
            ICartItemStorage cartItemStorage = new CartItemStorage(_dbConnectionFactory);
            await cartItemStorage.DeleteAllCartItem(1);

            IEnumerable<CartItem> CartItems = await cartItemStorage.GetCartItems(1);
            CartItems.Should().BeEmpty();
        }

        [Fact]
        public async Task Should_Change_Item_Quantity()
        {

            Product prod = FakeDataFactory.FakeProduct();
            IProductStorage productStorage = new ProductStorage(_dbConnectionFactory);

            await productStorage.InsertProduct(prod);

            CartItem cartItem = new CartItem
            {
                ProductId = 2,
                CartId = 1,
                Quantity = 5

            };

            ICartItemStorage cartItemStorage = new CartItemStorage(_dbConnectionFactory);
            await cartItemStorage.InsertCartItem(cartItem);

            await cartItemStorage.UpdateCartItem(2, 1, 1);

            IEnumerable<CartItem> items = await cartItemStorage.GetCartItems(1);

            var res = items.First();
            res.Quantity.Should().Be(1);

            await Utility.ClearCartItems(_dbConnectionFactory);
        }

        private async Task SetUpCartItem()
        {
            Cart cart = FakeDataFactory.FakeCart();
            User user = FakeDataFactory.FakeUser();

            IUserStorage userStorage = new UserStorage(_dbConnectionFactory);
            await userStorage.InsertUser(user);

            ICartStorage cartStorage = new CartStorage(_dbConnectionFactory);
            await cartStorage.InsertCart(cart);
        }
    }
}
