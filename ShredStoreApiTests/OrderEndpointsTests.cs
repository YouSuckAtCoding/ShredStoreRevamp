using Application.Models;
using Application.Services.OrderServices;
using Application.Services.UserServices;
using Contracts.Request.OrderRequests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ShredStore;
using ShredStoreApiTests.TestDoubles;
using ShredStoreTests.Fake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShredStoreApiTests
{
    public class OrderEndpointsTests
    {
        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            IgnoreNullValues = true,
            PropertyNameCaseInsensitive = true

        };
        [Fact]
        public async Task Should_Return_200_From_Create_Endpoint()
        {
            using var client = CreateApiWithOrderRepository<StubSuccessOrderRepository>().CreateClient();
            var request = FakeDataFactory.FakeCreateOrderRequest();

            var jsonString = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(ApiEndpointsTest.OrderEndpoints.Create, httpContent);
            response.Should().HaveStatusCode(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Should_Insert_Order_From_Real_Create_Endpoint()
        {
            using var client = CreateApi.CreateOfficialApi().CreateClient();
            var request = FakeDataFactory.FakeCreateOrderRequest();

            var jsonString = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(ApiEndpointsTest.OrderEndpoints.Create, httpContent);
            response.Should().HaveStatusCode(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Should_Return_200_From_GetAll_Endpoint()
        {
            using var client = CreateApiWithOrderRepository<StubSuccessOrderRepository>().CreateClient();
            var response = await client.GetAsync(Utility.SetGet_Or_DeleteUrl(1,ApiEndpointsTest.OrderEndpoints.GetAll));
            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_All_User_Orders_From_Real_GetAll_Endpoint()
        {
            using var client = CreateApi.CreateOfficialApi().CreateClient();

            int id = await SeuUpOrders(client);

           IEnumerable < Order > orders = await GetAllUserOrders(client, id).ConfigureAwait(false);
           orders.Should().HaveCount(5);
        }

        [Fact]
        public async Task Should_Return_200_From_Get_Endpoint()
        {
            using var client = CreateApiWithOrderRepository<StubSuccessOrderRepository>().CreateClient();
            var response = await client.GetAsync(Utility.SetGet_Or_DeleteUrl(1, ApiEndpointsTest.OrderEndpoints.Get));
            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Return_Order_From_Real_Get_Endpoint()
        {
            using var client = CreateApi.CreateOfficialApi().CreateClient();

            int id = await SeuUpOrders(client);

            IEnumerable<Order> orders = await GetAllUserOrders(client, id).ConfigureAwait(false);

            int orderId = orders.First().Id;

            var response = await client.GetAsync(Utility.SetGet_Or_DeleteUrl(orderId, ApiEndpointsTest.OrderEndpoints.Get));
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var order = JsonSerializer.Deserialize<Order>(result, jsonSerializerOptions);
            order.Id.Should().Be(orderId);
        }

        [Fact]
        public async Task Should_Return_200_From_Update_Endpoint()
        {
            using var client = CreateApiWithOrderRepository<StubSuccessOrderRepository>().CreateClient();

            UpdateOrderRequest request = new UpdateOrderRequest
            {
                Id = 1,
                CreatedDate = DateTime.Now,
                UserId = 1,
                PaymentId = 1,
                TotalAmount = 1000

            };

            var jsonString = JsonSerializer.Serialize(request);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(ApiEndpointsTest.OrderEndpoints.Update, httpContent);

            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Update_Order_From_Real_Update_Endpoint()
        {
            using var client = CreateApi.CreateOfficialApi().CreateClient();

            int id = await SeuUpOrders(client);

            IEnumerable<Order> orders = await GetAllUserOrders(client, id).ConfigureAwait(false);

            var selected = orders.First();
            int newAmount = 1000000;

            UpdateOrderRequest request = new UpdateOrderRequest
            {
                Id = selected.Id,
                CreatedDate = DateTime.Now,
                UserId = selected.UserId,
                PaymentId = selected.PaymentId,
                TotalAmount = newAmount

            };

            var jsonString = JsonSerializer.Serialize(request);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(ApiEndpointsTest.OrderEndpoints.Update, httpContent);

            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var order = JsonSerializer.Deserialize<Order>(result, jsonSerializerOptions);

            order.TotalAmount.Should().Be(newAmount);
        }

        [Fact]
        public async Task Should_Return_200_From_Delete_Endpoint()
        {
            using var client = CreateApiWithOrderRepository<StubSuccessOrderRepository>().CreateClient();
            var response = await client.GetAsync(Utility.SetGet_Or_DeleteUrl(1, ApiEndpointsTest.OrderEndpoints.Delete));
            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Delete_Order_From_Real_Delete_Endpoint()
        {
            using var client = CreateApi.CreateOfficialApi().CreateClient();
            
            int id = await SeuUpOrders(client);
            var orders = await GetAllUserOrders(client, id);

            int orderId = orders.Last().Id;
            await client.DeleteAsync(Utility.SetGet_Or_DeleteUrl(orderId, ApiEndpointsTest.OrderEndpoints.Delete));

            orders = await GetAllUserOrders(client, id);
            orders.Should().NotContain(x => x.Id == orderId);
        }


        private static async Task<IEnumerable<Order>> GetAllUserOrders(HttpClient client, int id)
        {
            var response = await client.GetAsync(Utility.SetGet_Or_DeleteUrl(id, ApiEndpointsTest.OrderEndpoints.GetAll));
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var orders = JsonSerializer.Deserialize<IEnumerable<Order>>(result, jsonSerializerOptions);
            return orders;
        }

        private ApiFactory CreateApiWithOrderRepository<T>()
        where T : class, IOrderService
        {
            var api = new ApiFactory(services =>
            {
                services.RemoveAll(typeof(IOrderService));

                services.TryAddScoped<IOrderService, T>();
                
            });
            return api;
        }

        private static async Task<int> SeuUpOrders(HttpClient client)
        {
            var users = await Utility.GetAllUsers(client);
            int id = users.Last().Id;
            for (int i = 0; i < 5; i++)
            {
                CreateOrderRequest request = new CreateOrderRequest
                {
                    CreatedDate = DateTime.Now,
                    UserId = id,
                    TotalAmount = 1000,
                    PaymentId = 3
                };
                var jsonString = JsonSerializer.Serialize(request);

                var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                await client.PostAsync(ApiEndpointsTest.OrderEndpoints.Create, httpContent);
            }

            return id;
        }

    }
}
