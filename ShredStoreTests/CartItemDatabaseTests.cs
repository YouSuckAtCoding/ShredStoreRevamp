﻿using Application.Models;
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
            int productId = int.MaxValue;
            int quantity = 5;
            CartItem cartItem = new CartItem
            {
                ProductId = productId,
                CartId = await GenerateCartId(),
                Quantity = quantity

            };

            ICartItemStorage cartItemStorage = new CartItemStorage(_dbConnectionFactory);
            var res = () => cartItemStorage.InsertCartItem(cartItem);
            await res.Should().ThrowExactlyAsync<SqlException>();
        }

        [Fact]
        public async Task Should_Throw_SqlExecption_If_Cart_Doenst_Exists()
        {

            int cartId = 2;
            int productId = 1;
            int quantity = 1;

            CartItem cartItem = new CartItem
            {
                ProductId = productId,
                CartId = cartId,
                Quantity = quantity
            };

            ICartItemStorage cartItemStorage = new CartItemStorage(_dbConnectionFactory);
            var res = () => cartItemStorage.InsertCartItem(cartItem);
            await res.Should().ThrowExactlyAsync<SqlException>();
        }

        [Fact]
        public async Task Should_Insert_Item_If_Doenst_Exists()
        {

            int productId = await GenerateProductId();
            int quantity = 1;
            CartItem cartItem = new CartItem
            {
                ProductId = productId,
                CartId = await GenerateCartId(),
                Quantity = quantity
            };
            ICartItemStorage cartItemStorage = new CartItemStorage(_dbConnectionFactory);

            await cartItemStorage.InsertCartItem(cartItem);
            var res = await cartItemStorage.GetCartItems(cartItem.CartId);

            res.Should().HaveCount(quantity);
            await Utility.ClearCartItems(_dbConnectionFactory);
        }

        [Fact]
        public async Task Should_Insert_Multiple_Cart_Items()
        {
            IEnumerable<Product> products = await InsertMultipleProducts();
            ICartItemStorage cartItemStorage = new CartItemStorage(_dbConnectionFactory);
            int cartId = await GenerateCartId();
            int quantity = 1;

            foreach (Product product in products)
            {
                CartItem cartItem = new CartItem
                {
                    ProductId = product.Id,
                    CartId = cartId,
                    Quantity = quantity

                };
                await cartItemStorage.InsertCartItem(cartItem);
            }
            var res = await cartItemStorage.GetCartItems(cartId);

            res.Should().HaveCountGreaterThanOrEqualTo(10);

            await Utility.ClearCartItems(_dbConnectionFactory);
        }

       

        [Fact]
        public async Task Should_Delete_CartItem()
        {
            int productId = await GenerateProductId();
            int quantity = 1;
            int expected = 0;
            CartItem cartItem = new CartItem
            {
                ProductId = productId,
                CartId = await GenerateCartId(),
                Quantity = quantity
            };

            ICartItemStorage cartItemStorage = new CartItemStorage(_dbConnectionFactory);
            await cartItemStorage.InsertCartItem(cartItem);
            await cartItemStorage.DeleteCartItem(productId, cartItem.CartId);
            var res = await cartItemStorage.GetCartItems(cartItem.CartId);

            res.Should().HaveCount(expected);
            await Utility.ClearCartItems(_dbConnectionFactory);
        }

     
        [Fact]
        public async Task Should_Be_Empty_After_Delete_All()
        {
            int cardId = 1;
            ICartItemStorage cartItemStorage = new CartItemStorage(_dbConnectionFactory);
            await cartItemStorage.DeleteAllCartItem(cardId);

            IEnumerable<CartItem> CartItems = await cartItemStorage.GetCartItems(cardId);
            CartItems.Should().BeEmpty();
        }

        [Fact]
        public async Task Should_Change_Item_Quantity()
        {

            int productId = await GenerateProductId();
            int quantity = 5;
            int expected = 1;
            CartItem cartItem = new CartItem
            {
                ProductId = productId,
                CartId = await GenerateCartId(),
                Quantity = quantity

            };

            ICartItemStorage cartItemStorage = new CartItemStorage(_dbConnectionFactory);
            await cartItemStorage.InsertCartItem(cartItem);
            await cartItemStorage.UpdateCartItem(productId, expected, cartItem.CartId);
            IEnumerable<CartItem> items = await cartItemStorage.GetCartItems(cartItem.CartId);

            var res = items.First();
            res.Quantity.Should().Be(expected);

            await Utility.ClearCartItems(_dbConnectionFactory);
        }

        [Fact]
        public async Task Should_Return_Products_From_Cart_Items()
        {
            IProductStorage storage = new ProductStorage(_dbConnectionFactory);
            IEnumerable<Product> products = await InsertMultipleProducts();
            ICartItemStorage cartItemStorage = new CartItemStorage(_dbConnectionFactory);
            int cartId = await GenerateCartId();
            int quantity = 1;
            int expected = 0;

            foreach (Product product in products)
            {
                CartItem cartItem = new CartItem
                {
                    ProductId = product.Id,
                    CartId = cartId,
                    Quantity = quantity

                };
                await cartItemStorage.InsertCartItem(cartItem);
            }

            var result = await storage.GetCartProducts(cartId, default);
            result.Count().Should().BeGreaterThan(expected);


        }

        private async Task<int> GenerateCartId()
        {
            Cart cart = FakeDataFactory.FakeCart();
            cart.UserId = await Utility.GenerateUserId(_dbConnectionFactory);

            ICartStorage cartStorage = new CartStorage(_dbConnectionFactory);
            await cartStorage.InsertCart(cart);
            return cart.UserId;

        }

        private async Task<int> GenerateProductId()
        {
            int userId = await Utility.GenerateUserId(_dbConnectionFactory);
            Product prod = FakeDataFactory.FakeProduct();
            IProductStorage productStorage = new ProductStorage(_dbConnectionFactory);
            prod.UserId = userId;

            await productStorage.InsertProduct(prod);

            var returned = await productStorage.GetProducts();
            return returned.First().Id;
        }
        private async Task<IEnumerable<Product>> InsertMultipleProducts()
        {
            int userId = await Utility.GenerateUserId(_dbConnectionFactory);
            IEnumerable<Product> prods = FakeDataFactory.FakeProducts();
            IProductStorage productStorage = new ProductStorage(_dbConnectionFactory);
            foreach (Product product in prods)
            {
                product.UserId = userId;
                await productStorage.InsertProduct(product);
            }
            IEnumerable<Product> products = await productStorage.GetProducts();
            return products;
        }
    }
}
