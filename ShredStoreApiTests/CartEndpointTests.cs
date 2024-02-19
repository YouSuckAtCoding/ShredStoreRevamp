using Application.Models;
using Application.Services.CartServices;
using Application.Services.ProductServices;
using Contracts.Request.CartRequests;
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
using static ShredStore.ApiEndpointsTest;

namespace ShredStoreApiTests
{
    public class CartEndpointTests
    {
        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            IgnoreNullValues = true,
            PropertyNameCaseInsensitive = true
        };

        [Fact]
        public async Task Should_Return_200_From_Create_Endpoint()
        {
            using var client = CreateApiWithCartRepository<StubSuccessCartRepository>().CreateClient();
            CreateCartRequest request = FakeDataFactory.FakeCreateCartRequest();

            var jsonString = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(ApiEndpointsTest.CartEndpoints.Create, httpContent);
            var responseInfo = response.Content.ReadAsStringAsync();

            response.Should().HaveStatusCode(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Should_Create_Cart_From_Real_Create_Endpoint()
        {
            using var client = CreateApi.CreateOfficialApi().CreateClient();

            var users = await Utility.GetAllUsers(client);

            CreateCartRequest request = new CreateCartRequest
            {
                UserId = users!.Last().Id,
                CreatedDate = DateTime.UtcNow
            };

            var jsonString = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(ApiEndpointsTest.CartEndpoints.Create, httpContent);
            var responseInfo = response.Content.ReadAsStringAsync();

            response.Should().HaveStatusCode(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Should_Return_200_From_Get_Endpoint()
        {
            using var client = CreateApiWithCartRepository<StubSuccessCartRepository>().CreateClient();
          
            var response = await client.GetAsync(Utility.SetGet_Or_DeleteUrl(1, ApiEndpoints.CartEndpoints.Get));
          
            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }
        [Fact]
        public async Task Should_Return_Cart_From_Real_Get_Endpoint()
        {
            using var client = CreateApi.CreateOfficialApi().CreateClient();

            var response = await client.GetAsync(Utility.SetGet_Or_DeleteUrl(1, ApiEndpoints.CartEndpoints.Get));
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var cart = JsonSerializer.Deserialize<Cart>(result, jsonSerializerOptions);
            cart.UserId.Should().Be(1);
        }

        [Fact]
        public async Task Should_Return_200_From_Delete_Endpoint()
        {
            using var client = CreateApiWithCartRepository<StubSuccessCartRepository>().CreateClient();

            var response = await client.DeleteAsync(Utility.SetGet_Or_DeleteUrl(1, ApiEndpoints.CartEndpoints.Delete));

            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Delete_Cart_From_Real_Delete_Endpoint()
        {
            using var client = CreateApi.CreateOfficialApi().CreateClient();

            CreateCartRequest request = new CreateCartRequest
            {
                UserId = 2,
                CreatedDate = DateTime.UtcNow
            };

            var jsonString = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            await client.PostAsync(ApiEndpointsTest.CartEndpoints.Create, httpContent);

            var response = await client.DeleteAsync(Utility.SetGet_Or_DeleteUrl(request.UserId, ApiEndpoints.CartEndpoints.Delete));

            response = await client.GetAsync(Utility.SetGet_Or_DeleteUrl(request.UserId, ApiEndpoints.CartEndpoints.Get));

            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var cart = JsonSerializer.Deserialize<Cart>(result, jsonSerializerOptions);

            cart.UserId.Should().Be(0);
        }

        private ApiFactory CreateApiWithCartRepository<T>()
   where T : class, ICartService
        {
            var api = new ApiFactory(services =>
            {
                services.RemoveAll(typeof(ICartService));

                services.TryAddScoped<ICartService, T>();

            });
            return api;
        }
       
    }
}
