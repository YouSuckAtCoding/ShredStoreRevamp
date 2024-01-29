using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Bogus;
using Bogus.Extensions.Brazil;
using Contracts.Request;

namespace ShredStoreTests.Fake
{
    public class FakeDataFactory
    {
        public static User FakeUser()
        {
            var userFaker = new Faker<User>("pt_BR")
            .RuleFor(x => x.Name, f => f.Name.FullName(Bogus.DataSets.Name.Gender.Male))
            .RuleFor(x => x.Email, f => f.Internet.Email(f.Person.FirstName).ToLower())
            .RuleFor(x => x.Cpf, f => f.Person.Cpf(false))
            .RuleFor(x => x.Address, f => f.Address.StreetAddress())
            .RuleFor(x => x.Age, f => f.Random.Number(16,110))
            .RuleFor(x => x.Password, f => f.Random.AlphaNumeric(25));

            return userFaker;
        }
        public static CreateUserRequest FakeCreateUserRequest()
        {
            var userFaker = new Faker<CreateUserRequest>("pt_BR")
            .RuleFor(x => x.Name, f => f.Name.FullName(Bogus.DataSets.Name.Gender.Male))
            .RuleFor(x => x.Email, f => f.Internet.Email(f.Person.FirstName).ToLower())
            .RuleFor(x => x.Cpf, f => f.Person.Cpf(false))
            .RuleFor(x => x.Address, f => f.Address.StreetAddress())
            .RuleFor(x => x.Age, f => f.Random.Number(16, 110))
            .RuleFor(x => x.Password, f => f.Random.AlphaNumeric(25));

            return userFaker;
        }

        public static ResetPasswordUserRequest FakeResetPasswordUserRequest()
        {
            var userFaker = new Faker<ResetPasswordUserRequest>("pt_Br")
                .RuleFor(x => x.Email, "teste@gmail.com")
                .RuleFor(x => x.Password, f => f.Random.AlphaNumeric(25));
            return userFaker;            
        }

        public static IEnumerable<User> FakeUsers()
        {
            var userFaker = new Faker<User>("pt_BR")
            .RuleFor(x => x.Name, f => f.Name.FullName(Bogus.DataSets.Name.Gender.Male))
            .RuleFor(x => x.Email, f => f.Internet.Email(f.Person.FirstName).ToLower())
            .RuleFor(x => x.Cpf, f => f.Person.Cpf(false))
            .RuleFor(x => x.Address, f => f.Address.StreetAddress())
            .RuleFor(x => x.Age, f => f.Random.Number(25))
            .RuleFor(x => x.Password, f => f.Random.AlphaNumeric(25));

            return userFaker.Generate(10);
        }

        public static Product FakeProduct()
        {
            var ProductFaker = new Faker<Product>("pt_BR")
            .RuleFor(x => x.Name, f => f.Name.FirstName())
            .RuleFor(x => x.Price, f => f.Random.Number(100, 2000))
            .RuleFor(x => x.Type, f => f.Random.Word())
            .RuleFor(x => x.Category, f => f.Random.Word())
            .RuleFor(x => x.Description, f => f.Random.Words());

            return ProductFaker;
        }

        public static IEnumerable<Product> FakeProducts()
        {
            var ProductFaker = new Faker<Product>("pt_BR")
            .RuleFor(x => x.Name, f => f.Name.FirstName())
            .RuleFor(x => x.Price, f => f.Random.Number(100, 2000))
            .RuleFor(x => x.Type, f => f.Random.Word())
            .RuleFor(x => x.Category, f => f.Random.Word())
            .RuleFor(x => x.Description, f => f.Random.Words());

            return ProductFaker.Generate(10);
        }
        public static Cart FakeCart()
        {
            Cart Cart = new Cart
            {
                CreatedDate = DateTime.Now,
                UserId = 1,
            };

            return Cart;
            
        }

        public static Order FakeOrder()
        {
            var orderFaker = new Faker<Order>("pt_BR")
            .RuleFor(x => x.Date, DateTime.Now)
            .RuleFor(x => x.UserId, 1)
            .RuleFor(x => x.CartId, 1);

            return orderFaker;
        }
    }
}
