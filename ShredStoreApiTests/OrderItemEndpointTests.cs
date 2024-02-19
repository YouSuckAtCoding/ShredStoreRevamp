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

        [Fact]
        public async Task Should_Return_200_From_Create_Endpoint()
        {
            var client = CreateApiWithOrderItemRepository<StubSuccessOrderItemRepository>().CreateClient();

            CreateOrderItemRequest request = new CreateOrderItemRequest
            {
                OrderId = 1,
                ProductId = 30,
                Quantity = 4

            };

            var jsonString = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var result = await client.PostAsync(ApiEndpointsTest.OrderItemEndpoints.Create, httpContent);
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        [Fact]
        public async Task Should_Create_Item_From_Real_Create_Endpoint()
        {
            var client = CreateApi.CreateOfficialApi().CreateClient();

            StringContent httpContent = await SetUpOrderItem(client).ConfigureAwait(false);

            var result = await client.PostAsync(ApiEndpointsTest.OrderItemEndpoints.Create, httpContent);
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        }
        [Fact]
        public async Task Should_Return_200_From_GetAll_Endpoint()
        {
            var client = CreateApiWithOrderItemRepository<StubSuccessOrderItemRepository>().CreateClient();
            var orders = await Utility.GetAllOrders(client);
            var result = await client.GetAsync(Utility.SetGet_Or_DeleteUrl(orders.Last().Id, ApiEndpointsTest.OrderItemEndpoints.GetAll));
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Fact]
        public async Task Should_Return_OrderItems_From_Real_GetAll_Endpoint()
        {
            var client = CreateApi.CreateOfficialApi().CreateClient();
            await SetUpOrderItem(client);
            
            StringContent httpContent = await SetUpOrderItem(client).ConfigureAwait(false);
            await client.PostAsync(ApiEndpointsTest.OrderItemEndpoints.Create, httpContent);

            var orders = await Utility.GetAllOrders(client);
            var result = await client.GetAsync(Utility.SetGet_Or_DeleteUrl(orders.Last().Id, ApiEndpointsTest.OrderItemEndpoints.GetAll));
            var responseInfo = await result.Content.ReadAsStringAsync();
            var items = JsonSerializer.Deserialize<IEnumerable<OrderItem>>(responseInfo, jsonSerializerOptions);
            items.Should().HaveCountGreaterThan(0);
        }
        [Fact]
        public async Task Should_Return_200_From_Get_Endpoint()
        {
            var client = CreateApiWithOrderItemRepository<StubSuccessOrderItemRepository>().CreateClient();
            var orders = await Utility.GetAllOrders(client);
            var products = await Utility.ReturnAllProducts(client);
            var result = await client.GetAsync(Utility.SetOrderItem_GetUrl(products.First().Id,orders.Last().Id, ApiEndpointsTest.OrderItemEndpoints.Get));
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Fact]
        public async Task Should_Return_Item_From_Real_Get_Endpoint()
        {
            var client = CreateApi.CreateOfficialApi().CreateClient();
            await SetUpOrderItem(client);

            StringContent httpContent = await SetUpOrderItem(client).ConfigureAwait(false);
            await client.PostAsync(ApiEndpointsTest.OrderItemEndpoints.Create, httpContent);

            var orders = await Utility.GetAllOrders(client);
            var products = await Utility.ReturnAllProducts(client);
            var result = await client.GetAsync(Utility.SetOrderItem_GetUrl(products.First().Id, orders.Last().Id, ApiEndpointsTest.OrderItemEndpoints.Get));
            var responseInfo = await result.Content.ReadAsStringAsync();
            var items = JsonSerializer.Deserialize<OrderItem>(responseInfo, jsonSerializerOptions);
            items.ProductId.Should().Be(products.First().Id);
        }

        [Fact]
        public async Task Should_Return_200_From_Update_Endpoint()
        {
            var client = CreateApiWithOrderItemRepository<StubSuccessOrderItemRepository>().CreateClient();
            var orders = await Utility.GetAllOrders(client);
            var products = await Utility.ReturnAllProducts(client);
            UpdateOrderItemRequest request = new UpdateOrderItemRequest
            {
                Id = 1,
                OrderId = orders.Last().Id,
                ProductId = products.First()!.Id,
                Quantity = 12

            };
            var jsonString = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var result = await client.PutAsync(ApiEndpointsTest.OrderItemEndpoints.Update, httpContent);
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Update_Item_From_Real_Update_Endpoint()
        {
            var client = CreateApi.CreateOfficialApi().CreateClient();
            
            var orders = await Utility.GetAllOrders(client);
            var products = await Utility.ReturnAllProducts(client);
            
            var res = await client.GetAsync(Utility.SetGet_Or_DeleteUrl(orders.Last().Id, ApiEndpointsTest.OrderItemEndpoints.GetAll));
            var responseInfo = await res.Content.ReadAsStringAsync();
            var items = JsonSerializer.Deserialize<IEnumerable<OrderItem>>(responseInfo, jsonSerializerOptions);

            var selected = items.Last();


            UpdateOrderItemRequest request = new UpdateOrderItemRequest
            {
                Id = selected.Id,
                OrderId = orders.Last().Id,
                ProductId = products.First()!.Id,
                Quantity = selected.Quantity + 10

            };
            var jsonString = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var result = await client.PutAsync(ApiEndpointsTest.OrderItemEndpoints.Update, httpContent);
            var resultContent = await result.Content.ReadAsStringAsync();
            var updated = JsonSerializer.Deserialize<OrderItem>(resultContent, jsonSerializerOptions);

            updated.Quantity.Should().Be(selected.Quantity + 10);


        }

        [Fact]
        public async Task Should_Return_200_From_Delete_Endpoint()
        {
            var client = CreateApiWithOrderItemRepository<StubSuccessOrderItemRepository>().CreateClient();
            var result = await client.DeleteAsync(Utility.SetOrderItem_GetUrl(1,1, ApiEndpointsTest.OrderItemEndpoints.Delete));
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Delete_Item_From_Real_Delete_Endpoint()
        {
            var client = CreateApi.CreateOfficialApi().CreateClient();
            await SetUpOrderItem(client);

            StringContent httpContent = await SetUpOrderItem(client).ConfigureAwait(false);
            await client.PostAsync(ApiEndpointsTest.OrderItemEndpoints.Create, httpContent);

            var orders = await Utility.GetAllOrders(client);
            var products = await Utility.ReturnAllProducts(client);
            
            var result = await client.DeleteAsync(Utility.SetOrderItem_GetUrl(products.First().Id, orders.Last().Id, ApiEndpointsTest.OrderItemEndpoints.Delete));
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Return_200_From_Delete_All_Endpoint()
        {
            var client = CreateApiWithOrderItemRepository<StubSuccessOrderItemRepository>().CreateClient();
            var result = await client.DeleteAsync(Utility.SetOrderItem_GetUrl(orderId:1, endpoint:ApiEndpointsTest.OrderItemEndpoints.DeleteAll));
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Delete_All_Orders_From_Real_DeleteAll_Endpoint()
        {
            var client = CreateApi.CreateOfficialApi().CreateClient();

            await SetUpOrderItem(client);

            StringContent httpContent = await SetUpOrderItem(client).ConfigureAwait(false);
            await client.PostAsync(ApiEndpointsTest.OrderItemEndpoints.Create, httpContent);

            var orders = await Utility.GetAllOrders(client);
            var order = orders.Last();

            await client.DeleteAsync(Utility.SetOrderItem_GetUrl(orderId: order.Id, endpoint: ApiEndpointsTest.OrderItemEndpoints.DeleteAll));

            var result = await client.GetAsync(Utility.SetGet_Or_DeleteUrl(order.Id, ApiEndpointsTest.OrderItemEndpoints.GetAll));
            var responseInfo = await result.Content.ReadAsStringAsync();
            var items = JsonSerializer.Deserialize<IEnumerable<OrderItem>>(responseInfo, jsonSerializerOptions);
            items.Should().BeEmpty();
        }

        private static async Task<StringContent> SetUpOrderItem(HttpClient client)
        {
            IEnumerable<Product>? products = await Utility.ReturnAllProducts(client).ConfigureAwait(false);

            var response = await client.GetAsync(Utility.SetGet_Or_DeleteUrl(products!.First().Id, ApiEndpoints.ProductEndpoints.Get));
            var responseInfo = await response.Content.ReadAsStringAsync();

            var product = JsonSerializer.Deserialize<ProductResponse>(responseInfo, jsonSerializerOptions);

            var orders = await Utility.GetAllOrders(client);

            CreateOrderItemRequest request = new CreateOrderItemRequest
            {
                OrderId = orders.Last().Id,
                ProductId = product!.Id,
                Quantity = 4
            };

            var jsonString = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            return httpContent;
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
