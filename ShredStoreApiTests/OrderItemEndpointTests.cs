using Application.Models;
using Application.Services.OrderItemServices;
using Contracts.Request.OrderItemRequests;
using Contracts.Request.OrderItemRequests;
using Contracts.Response.ProductsResponses;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ShredStore;
using ShredStoreApiTests.TestDoubles;
using System.Text;
using System.Text.Json;

namespace ShredStoreApiTests
{
    public class OrderItemEndpointTests
    {
        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            IgnoreNullValues = true,
            PropertyNameCaseInsensitive = true

        };
        private readonly int id = 1;
        private readonly int orderId = 10;
        private readonly int productId = 5;
        private readonly int quantity = 5;
        

        [Fact]
        public async Task Should_Return_200_From_Create_Endpoint()
        {
            var client = CreateApiWithOrderItemRepository<StubSuccessOrderItemRepository>().CreateClient();

            CreateOrderItemRequest request = new CreateOrderItemRequest
            {
                OrderId = orderId,
                ProductId = productId,
                Quantity = quantity

            };

            var jsonString = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, Utility.content_Type);

            var result = await client.PostAsync(ApiEndpoints.OrderItemEndpoints.Create, httpContent);
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        [Fact]
        public async Task Should_Return_200_From_GetAll_Endpoint()
        {
            var client = CreateApiWithOrderItemRepository<StubSuccessOrderItemRepository>().CreateClient();
            
            var result = await client.GetAsync(Utility.SetGet_Or_DeleteUrl(orderId, ApiEndpoints.OrderItemEndpoints.GetAll));
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Fact]
        public async Task Should_Return_200_From_Get_Endpoint()
        {
            var client = CreateApiWithOrderItemRepository<StubSuccessOrderItemRepository>().CreateClient();
            var result = await client.GetAsync(Utility.SetOrderItem_GetUrl(productId,orderId, ApiEndpoints.OrderItemEndpoints.Get));
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
     
        [Fact]
        public async Task Should_Return_200_From_Update_Endpoint()
        {
            var client = CreateApiWithOrderItemRepository<StubSuccessOrderItemRepository>().CreateClient();
            UpdateOrderItemRequest request = new UpdateOrderItemRequest
            {
                Id = id,
                OrderId = orderId,
                ProductId = productId,
                Quantity = quantity

            };
            var jsonString = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, Utility.content_Type);
            var result = await client.PutAsync(ApiEndpoints.OrderItemEndpoints.Update, httpContent);
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Return_200_From_Delete_Endpoint()
        {
            var client = CreateApiWithOrderItemRepository<StubSuccessOrderItemRepository>().CreateClient();
            var result = await client.DeleteAsync(Utility.SetOrderItem_GetUrl(productId,orderId, ApiEndpoints.OrderItemEndpoints.Delete));
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Return_200_From_Delete_All_Endpoint()
        {
            var client = CreateApiWithOrderItemRepository<StubSuccessOrderItemRepository>().CreateClient();
            var result = await client.DeleteAsync(Utility.SetOrderItem_GetUrl(orderId, endpoint:ApiEndpoints.OrderItemEndpoints.DeleteAll));
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }


        private ApiFactory CreateApiWithOrderItemRepository<T>()
        where T : class, IOrderItemService
        {
            var api = new ApiFactory(services =>
            {
                services.RemoveAll(typeof(IOrderItemService));

                services.TryAddScoped<IOrderItemService, T>();

            });
            return api;
        }
    }
}
