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

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var result = await client.PostAsync(ApiEndpointsTest.CartItemEndpoints.Create, httpContent);
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        [Fact]
        public async Task Should_Create_Item_From_Real_Create_Endpoint()
        {
            var client = CreateApi.CreateOfficialApi().CreateClient();

            IEnumerable<Product>? products = await Utility.ReturnAllProducts(client).ConfigureAwait(false);

            var response = await client.GetAsync(Utility.SetGet_Or_DeleteUrl(products!.First().Id, ApiEndpoints.ProductEndpoints.Get));
            var responseInfo = await response.Content.ReadAsStringAsync();

            var product = JsonSerializer.Deserialize<ProductResponse>(responseInfo, jsonSerializerOptions);

            CreateCartItemRequest request = new CreateCartItemRequest
            {
                CartId = 1,
                ProductId = 30,
                Quantity = 4

            };
            var jsonString = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var result = await client.PostAsync(ApiEndpointsTest.CartItemEndpoints.Create, httpContent);
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        }

        [Fact]
        public async Task Should_Return_200_From_GetAll_Endpoint()
        {
            var client = CreateApiWithCartItemRepository<StubSuccessCartItemRepository>().CreateClient();
            var response = await client.GetAsync(Utility.SetCartItem_GetUrl(30,1, ApiEndpoints.CartItemEndpoints.GetAll));
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Return_All_Items_From_Real_GetAll_Endpoint()
        {
            var client = CreateApi.CreateOfficialApi().CreateClient();
            var response = await client.GetAsync(Utility.SetCartItem_GetUrl(30,1,ApiEndpoints.CartItemEndpoints.GetAll));
            var responseInfo = await response.Content.ReadAsStringAsync();

            var items = JsonSerializer.Deserialize<IEnumerable<CartItem>>(responseInfo, jsonSerializerOptions);
            items.Should().NotBeEmpty();
        }
        [Fact]
        public async Task Should_Return_200_From_Get_Endpoint()
        {
            var client = CreateApiWithCartItemRepository<StubSuccessCartItemRepository>().CreateClient();
            var response = await client.GetAsync(Utility.SetCartItem_GetUrl(30, 1, ApiEndpoints.CartItemEndpoints.Get));
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Return_Item_From_Real_Get_Endpoint()
        {
            var client = CreateApi.CreateOfficialApi().CreateClient();
            var response = await client.GetAsync(Utility.SetCartItem_GetUrl(30, 1, ApiEndpoints.CartItemEndpoints.Get));
            var responseInfo = await response.Content.ReadAsStringAsync();

            var item = JsonSerializer.Deserialize<CartItem>(responseInfo, jsonSerializerOptions);
            item.CartId.Should().Be(1);
        }
        [Fact]
        public async Task Should_Return_200_From_Update_Endpoint()
        {
            var client = CreateApiWithCartItemRepository<StubSuccessCartItemRepository>().CreateClient();
            UpdateCartItemRequest request = new UpdateCartItemRequest
            {
                CartId = 1,
                ProductId = 30,
                Quantity = 4

            };
            var jsonString = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(ApiEndpointsTest.CartItemEndpoints.Update, httpContent);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        [Fact]
        public async Task Should_Update_Cart_Item_From_Real_Update_Endpoint()
        {
            var client = CreateApi.CreateOfficialApi().CreateClient();
            UpdateCartItemRequest request = new UpdateCartItemRequest
            {
                CartId = 1,
                ProductId = 30,
                Quantity = 8

            };
            var jsonString = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            await client.PutAsync(ApiEndpointsTest.CartItemEndpoints.Update, httpContent);

            var response = await client.GetAsync(Utility.SetCartItem_GetUrl(request.ProductId, request.CartId, ApiEndpoints.CartItemEndpoints.Get));
            var responseInfo = await response.Content.ReadAsStringAsync();

            var item = JsonSerializer.Deserialize<CartItem>(responseInfo, jsonSerializerOptions);
            item.Quantity.Should().Be(8);
        }

        [Fact]
        public async Task Should_Return_200_From_Delete_Endpoint()
        {
            var client = CreateApiWithCartItemRepository<StubSuccessCartItemRepository>().CreateClient();
            var response = await client.DeleteAsync(Utility.SetCartItem_GetUrl(30,1, ApiEndpointsTest.CartItemEndpoints.Delete));
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Delete_Item_From_Real_Delete_Endpoint()
        {
            var client = CreateApi.CreateOfficialApi().CreateClient();

            await CreateUser(client);

            var users = await Utility.GetAllUsers(client);

            IEnumerable<Product>? products = await Utility.ReturnAllProducts(client).ConfigureAwait(false);

            CreateCartRequest CartRequest = new CreateCartRequest
            {
                UserId = users!.Last().Id,
                CreatedDate = DateTime.UtcNow
            };

            var jsonCartString = JsonSerializer.Serialize(CartRequest);

           var httpContent = new StringContent(jsonCartString, Encoding.UTF8, "application/json");

            await client.PostAsync(ApiEndpointsTest.CartEndpoints.Create, httpContent);

            CreateCartItemRequest request = new CreateCartItemRequest
            {
                CartId = users!.Last().Id,
                ProductId = products!.Last().Id,
                Quantity = 4

            };
            var jsonString = JsonSerializer.Serialize(request);

            httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            await client.PostAsync(ApiEndpointsTest.CartItemEndpoints.Create, httpContent);

            await client.DeleteAsync(Utility.SetCartItem_GetUrl(request.ProductId, request.CartId, ApiEndpointsTest.CartItemEndpoints.Delete));

            var response = await client.GetAsync(Utility.SetCartItem_GetUrl(request.ProductId, request.CartId, ApiEndpoints.CartItemEndpoints.Get));
            var responseInfo = await response.Content.ReadAsStringAsync();

            var item = JsonSerializer.Deserialize<CartItem>(responseInfo, jsonSerializerOptions);
            item.CartId.Should().Be(0);

        }

    
        [Fact]
        public async Task Should_Delete_All_Item_From_Real_Delete_All_Endpoint()
        {
            var client = CreateApi.CreateOfficialApi().CreateClient();

            await CreateUser(client);

            var users = await Utility.GetAllUsers(client);

            IEnumerable<Product>? products = await Utility.ReturnAllProducts(client).ConfigureAwait(false);

            CreateCartRequest CartRequest = new CreateCartRequest
            {
                UserId = users!.Last().Id,
                CreatedDate = DateTime.UtcNow
            };

            var jsonCartString = JsonSerializer.Serialize(CartRequest);

            var httpContent = new StringContent(jsonCartString, Encoding.UTF8, "application/json");

            await client.PostAsync(ApiEndpointsTest.CartEndpoints.Create, httpContent);

            int cartId = users!.Last().Id;

            for (int i = 0; i < 10; i++)
            {
                CreateCartItemRequest request = new CreateCartItemRequest
                {
                    CartId = cartId,
                    ProductId = products!.ElementAt(i).Id,
                    Quantity = 4

                };
                var jsonString = JsonSerializer.Serialize(request);

                httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                await client.PostAsync(ApiEndpointsTest.CartItemEndpoints.Create, httpContent);
            }
            

            await client.DeleteAsync(Utility.SetCartItem_GetUrl(endpoint:ApiEndpointsTest.CartItemEndpoints.DeleteAll, cartId: cartId));

            var response = await client.GetAsync(Utility.SetCartItem_GetUrl(endpoint:ApiEndpoints.CartItemEndpoints.GetAll, cartId:cartId));
            var responseInfo = await response.Content.ReadAsStringAsync();

            var items = JsonSerializer.Deserialize<IEnumerable<CartItem>>(responseInfo, jsonSerializerOptions);
            items.Should().BeEmpty();

        }
        private static async Task CreateUser(HttpClient client)
        {
            var userRequest = FakeDataFactory.FakeCreateUserRequest();

            var jsonUserString = JsonSerializer.Serialize(userRequest);

            var httpContent = new StringContent(jsonUserString, Encoding.UTF8, "application/json");
            await client.PostAsync(ApiEndpointsTest.UserEndpoints.Create, httpContent);

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
