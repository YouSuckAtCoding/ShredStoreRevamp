using Application.Models;
using Application.Services.CartItemServices;
using Contracts.Request.CartItemRequests;
using Contracts.Request.CartRequests;
using Contracts.Response.CartItemResponses;
using Contracts.Response.ProductsResponses;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ShredStore;
using ShredStoreApiTests.TestDoubles;
using ShredStoreTests.Fake;
using System.Text;
using System.Text.Json;

namespace ShredStoreApiTests
{
    public class CartItemEndpointTests
    {
        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            IgnoreNullValues = true,
            PropertyNameCaseInsensitive = true

        };

        private const int cartId = 1;
        private const int itemId = 30;


        [Fact]
        public async Task Should_Return_200_From_Create_Endpoint()
        {
            var client = CreateApiWithCartItemRepository<StubSuccessCartItemRepository>().CreateClient();

            CreateCartItemRequest request = new CreateCartItemRequest
            {
                CartId = 1,
                ProductId = 30,
                Quantity = 4

            };

            var jsonString = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, Utility.content_Type);

            var result = await client.PostAsync(ApiEndpoints.CartItemEndpoints.Create, httpContent);
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

       
        [Fact]
        public async Task Should_Return_200_From_GetAll_Endpoint()
        {
            var client = CreateApiWithCartItemRepository<StubSuccessCartItemRepository>().CreateClient();
            var response = await client.GetAsync(Utility.SetCartItem_GetUrl(itemId,cartId, ApiEndpoints.CartItemEndpoints.GetAll));
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

       
        [Fact]
        public async Task Should_Return_200_From_Get_Endpoint()
        {
            var client = CreateApiWithCartItemRepository<StubSuccessCartItemRepository>().CreateClient();
            var response = await client.GetAsync(Utility.SetCartItem_GetUrl(itemId, cartId, ApiEndpoints.CartItemEndpoints.Get));
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Return_200_From_Update_Endpoint()
        {
            var client = CreateApiWithCartItemRepository<StubSuccessCartItemRepository>().CreateClient();            
            UpdateCartItemRequest request = new UpdateCartItemRequest
            {
                CartId = cartId,
                ProductId = itemId,
                Quantity = 4

            };
            var jsonString = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, Utility.content_Type);
            var response = await client.PutAsync(ApiEndpoints.CartItemEndpoints.Update, httpContent);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
      
        [Fact]
        public async Task Should_Return_200_From_Delete_Endpoint()
        {
            var client = CreateApiWithCartItemRepository<StubSuccessCartItemRepository>().CreateClient();
            var response = await client.DeleteAsync(Utility.SetCartItem_GetUrl(itemId, cartId, ApiEndpoints.CartItemEndpoints.Delete));
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
     
        private ApiFactory CreateApiWithCartItemRepository<T>()
            where T : class, ICartItemService
        {
            var api = new ApiFactory(services =>
            {
                services.RemoveAll(typeof(ICartItemService));

                services.TryAddScoped<ICartItemService, T>();

            });
            return api;
        }
     
    }
}
