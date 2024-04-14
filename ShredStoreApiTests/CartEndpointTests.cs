using Application.Models;
using Application.Services.CartServices;
using Contracts.Request.CartRequests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ShredStore;
using ShredStoreApiTests.TestDoubles;
using ShredStoreTests.Fake;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ShredStoreApiTests
{
    public class CartEndpointTests
    {
        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            IgnoreNullValues = true,
            PropertyNameCaseInsensitive = true
        };

        private const int userId = 1;

        [Fact]
        public async Task Should_Return_200_From_Create_Endpoint()
        {
            using var client = CreateApiWithCartRepository<StubSuccessCartRepository>().CreateClient();
            CreateCartRequest request = FakeDataFactory.FakeCreateCartRequest();

            var jsonString = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, Utility.content_Type);

            var response = await client.PostAsync(ApiEndpoints.CartEndpoints.Create, httpContent);
            var responseInfo = response.Content.ReadAsStringAsync();

            response.Should().HaveStatusCode(HttpStatusCode.Created);
        }

    
        [Fact]
        public async Task Should_Return_200_From_Get_Endpoint()
        {
            using var client = CreateApiWithCartRepository<StubSuccessCartRepository>().CreateClient();
          
            var response = await client.GetAsync(Utility.SetGet_Or_DeleteUrl(userId, ApiEndpoints.CartEndpoints.Get));
          
            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }
      

        [Fact]
        public async Task Should_Return_200_From_Delete_Endpoint()
        {
            using var client = CreateApiWithCartRepository<StubSuccessCartRepository>().CreateClient();

            var response = await client.DeleteAsync(Utility.SetGet_Or_DeleteUrl(userId, ApiEndpoints.CartEndpoints.Delete));

            response.Should().HaveStatusCode(HttpStatusCode.OK);
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
