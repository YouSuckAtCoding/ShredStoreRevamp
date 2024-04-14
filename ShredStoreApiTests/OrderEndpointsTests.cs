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

        
        private const int id = 1;
        [Fact]
        public async Task Should_Return_200_From_Create_Endpoint()
        {
            using var client = CreateApiWithOrderRepository<StubSuccessOrderRepository>().CreateClient();
            var request = FakeDataFactory.FakeCreateOrderRequest();

            var jsonString = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, Utility.content_Type);
            var response = await client.PostAsync(ApiEndpoints.OrderEndpoints.Create, httpContent);
            response.Should().HaveStatusCode(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Should_Return_200_From_GetAll_Endpoint()
        {
            using var client = CreateApiWithOrderRepository<StubSuccessOrderRepository>().CreateClient();
            var response = await client.GetAsync(Utility.SetGet_Or_DeleteUrl(id,ApiEndpoints.OrderEndpoints.GetAll));
            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }

    
        [Fact]
        public async Task Should_Return_200_From_Get_Endpoint()
        {
            using var client = CreateApiWithOrderRepository<StubSuccessOrderRepository>().CreateClient();
            var response = await client.GetAsync(Utility.SetGet_Or_DeleteUrl(id, ApiEndpoints.OrderEndpoints.Get));
            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Return_200_From_Update_Endpoint()
        {
            using var client = CreateApiWithOrderRepository<StubSuccessOrderRepository>().CreateClient();

            UpdateOrderRequest request = new UpdateOrderRequest
            {
                Id = id,
                CreatedDate = DateTime.Now,
                UserId = 1,
                PaymentId = 1,
                TotalAmount = 1000

            };

            var jsonString = JsonSerializer.Serialize(request);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, Utility.content_Type);
            var response = await client.PutAsync(ApiEndpoints.OrderEndpoints.Update, httpContent);

            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Return_200_From_Delete_Endpoint()
        {
            using var client = CreateApiWithOrderRepository<StubSuccessOrderRepository>().CreateClient();
            var response = await client.GetAsync(Utility.SetGet_Or_DeleteUrl(id, ApiEndpoints.OrderEndpoints.Delete));
            response.Should().HaveStatusCode(HttpStatusCode.OK);
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

    }
}
