using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Bogus;
using Bogus.Extensions.Brazil;
using Contracts.Request;
using Contracts.Request.CartRequests;
using Contracts.Request.OrderRequests;
using Contracts.Request.ProductRequests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

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
            .RuleFor(x => x.Role, "Customer")
            .RuleFor(x => x.Password, f => f.Random.AlphaNumeric(15));

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
            .RuleFor(x => x.Role, "Customer")
            .RuleFor(x => x.Password, f => f.Random.AlphaNumeric(15));

            return userFaker;
        }

        public static ResetPasswordUserRequest FakeResetPasswordUserRequest()
        {
            var userFaker = new Faker<ResetPasswordUserRequest>("pt_Br")
                .RuleFor(x => x.Email, "teste@gmail.com")
                .RuleFor(x => x.Password, f => f.Random.AlphaNumeric(15));
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
            .RuleFor(x => x.Role, "Customer")
            .RuleFor(x => x.Password, f => f.Random.AlphaNumeric(15));

            return userFaker.Generate(10);
        }

        public static Product FakeProduct()
        {
            var ProductFaker = new Faker<Product>("pt_BR")
            .RuleFor(x => x.Name, f => f.Name.FirstName())
            .RuleFor(x => x.Price, f => f.Random.Number(100, 2000))
            .RuleFor(x => x.Type, f => f.Random.Word())
            .RuleFor(x => x.Category, f => f.Random.Word())
            .RuleFor(x => x.Brand, f => f.Random.Word())
            .RuleFor(x => x.ImageName, f => f.Random.Word())
            .RuleFor(x => x.Description, f => f.Random.Words());

            return ProductFaker;
        }
        public static CreateProductRequest FakeCreateProductRequest()
        {
            IFormFile file = GenerateFakeFile(".png", "test;test");

            var productFaker = new Faker<CreateProductRequest>("pt_BR")
            .RuleFor(x => x.Name, f => f.Name.FirstName())
            .RuleFor(x => x.Price, f => f.Random.Number(100, 2000))
            .RuleFor(x => x.Type, f => f.Random.Word())
            .RuleFor(x => x.Category, f => f.Random.Word())
            .RuleFor(x => x.Brand, f => f.Random.Word())
            .RuleFor(x => x.ImageName, f => f.Random.Word())
            .RuleFor(x => x.Description, f => f.Random.Words(15));

            return productFaker;
        }

       

        public static IEnumerable<Product> FakeProducts()
        {
            

            var ProductFaker = new Faker<Product>("pt_BR")
            .RuleFor(x => x.Name, f => f.Name.FirstName())
            .RuleFor(x => x.Price, f => f.Random.Number(100, 2000))
            .RuleFor(x => x.Type, f => f.Random.Word())
            .RuleFor(x => x.Category, f => f.Random.Word())
            .RuleFor(x => x.Brand, f => f.Random.Word())
            .RuleFor(x => x.ImageName, f => f.Random.Word())
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
            .RuleFor(x => x.CreatedDate, DateTime.Now)
            .RuleFor(x => x.UserId, 1)
            .RuleFor(x => x.TotalAmount, 1000)
            .RuleFor(x => x.PaymentId, 1);

            return orderFaker;
        }

        public static IEnumerable<Order> FakeOrders()
        {
            var orderFaker = new Faker<Order>("pt_BR")
            .RuleFor(x => x.CreatedDate, DateTime.Now)
            .RuleFor(x => x.UserId, 1)
            .RuleFor(x => x.TotalAmount, 1000)
            .RuleFor(x => x.PaymentId, 1);

            return orderFaker.Generate(10);
        }
        
        public static CreateOrderRequest FakeCreateOrderRequest()
        {
            var orderFaker = new Faker<CreateOrderRequest>("pt_BR")
            .RuleFor(x => x.CreatedDate, DateTime.Now)
            .RuleFor(x => x.UserId, 1)
            .RuleFor(x => x.TotalAmount, 1000)
            .RuleFor(x => x.PaymentId, 1);

            return orderFaker;
        }

        public static CreateCartRequest FakeCreateCartRequest()
        {
            var CartRequestFaker = new Faker<CreateCartRequest>("pt_BR")
                .RuleFor(x => x.UserId, 1)
                .RuleFor(x => x.CreatedDate, DateTime.UtcNow);
            return CartRequestFaker;
        }

        public static IEnumerable<OrderItem> FakeOrderItems()
        {
            var fakeItem = new Faker<OrderItem>("pt_BR")
                .RuleFor(x => x.OrderId, 1)
                .RuleFor(x => x.ProductId, 1)
                .RuleFor(x => x.Id, 1)
                .RuleFor(x => x.Price, 1000)
                .RuleFor(x => x.Quantity, 1000);
            return fakeItem.Generate(10);
        }

        public static Payment FakePayment()
        {
            var fakePayment = new Faker<Payment>("pt_BR")
                .RuleFor(x => x.Amount, 2000)
                .RuleFor(x => x.Date, DateTime.Now)
                .RuleFor(x => x.PaymentType, 1)
                .RuleFor(x => x.Payed, false);
            return fakePayment;


        }

        public static IFormFile GenerateFakeFile(string contentType, string content)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            try
            {
                var file = new FormFile(
                baseStream: new MemoryStream(bytes),
                baseStreamOffset: 0,
                length: bytes.Length,
                name: "Data",
                fileName: "dummy.csv"
            )
                {
                    Headers = new HeaderDictionary(),
                    ContentType = contentType
                };

                return file;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            

            
        }

    }
}
