﻿using Application.Models;
using Application.Repositories;
using Dapper;
using FluentAssertions;
using ShredStoreTests.DataAdapterFiles;
using ShredStoreTests.DataAdapterFiles.CartTestFiles;
using ShredStoreTests.DataAdapterFiles.ProductTestFiles;
using ShredStoreTests.DataAdapterFiles.UserTestFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShredStoreTests
{
    public class CartDatabaseTests : IClassFixture<SqlAccessFixture>
    {
        private readonly ISqlAccessConnectionFactory _dbConnectionFactory;
        public CartDatabaseTests(SqlAccessFixture fixture)
        {
            _dbConnectionFactory = fixture.ConnectionFactory;
        }

        [Fact]
        public void Should_Throw_ArgumentNullException_If_Missing_Connection_String()
        {
            var create = () => new CartStorage(null!);
            create.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [InlineData("spCart_Insert")]
        [InlineData("spCart_GetById")]
        [InlineData("spCart_Delete")]
        public async Task Should_Be_True_If_Cart_Stored_Procedure_Exists(string Sp)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(default);
            string spName = Sp;
            dynamic res = await connection.QueryAsync(Utility.CreateQueryForStoredProcedureCheck(spName));
            string name = res[0].name;
            name.Should().Be(spName);
        }

        [Fact]
        public async Task Should_Be_Null_If_Cart_Not_Exists()
        {
            ICartStorage CartStorage = new CartStorage(_dbConnectionFactory);
            int cartd = 2;

            var check = await CartStorage.GetCart(cartd);

            check.Should().BeSameAs(null);
        }
        [Fact]
        public async Task Should_Insert_Cart_If_Doesnt_Exist()
        {

            int userId = await GenerateUserId(_dbConnectionFactory);

            ICartStorage CartStorage = new CartStorage(_dbConnectionFactory);
            Cart cart = Fake.FakeDataFactory.FakeCart();
            cart.UserId = userId;

            await CartStorage.InsertCart(cart);
            var check = await CartStorage.GetCart(userId);

            check.UserId.Should().Be(userId);

            await Utility.CleanUpCarts(_dbConnectionFactory);

        }
        [Fact]
        public async Task Should_Delete_Cart_If_Exists()
        {

            int userId = await GenerateUserId(_dbConnectionFactory);

            ICartStorage CartStorage = new CartStorage(_dbConnectionFactory);
            Cart cart = Fake.FakeDataFactory.FakeCart();
            cart.UserId = userId;

            await CartStorage.InsertCart(cart);

            Cart res = await CartStorage.GetCart(userId);

            await CartStorage.DeleteCart(cart.UserId);

            res = await CartStorage.GetCart(userId);
            res.Should().BeSameAs(null);

            await Utility.CleanUpCarts(_dbConnectionFactory);

        }

        private async Task<int> GenerateUserId(ISqlAccessConnectionFactory _dbConnectionFactory)
        {
            User user = Fake.FakeDataFactory.FakeUser();
            IUserStorage UserStorage = new UserStorage(_dbConnectionFactory);

            await UserStorage.InsertUser(user);

            var res = await UserStorage.GetUsers();
            User returnedUser = res.FirstOrDefault();

            return returnedUser.Id;

        }
      

    }
}
