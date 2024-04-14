using Application.Models;
using Application.Services.ProductServices;
using Application.Services.UserServices;
using Contracts.Request;
using Contracts.Request.ProductRequests;
using Contracts.Response.ProductsResponses;
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
    public class ProductEndpointTests
    {
        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            IgnoreNullValues = true,
            PropertyNameCaseInsensitive = true

        };

        
        private const int Id = 2;

        [Fact]
        public async Task Should_Return_200_From_GetAll_Endpoint()
        {
            using var client = CreateApiWithProductRepository<StubSuccessProductRepository>().CreateClient();

            var response = await client.GetAsync(ApiEndpoints.ProductEndpoints.GetAll);

            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }


        [Fact]
        public async Task Should_Return_200_From_Get_Endpoint()
        {
            using var client = CreateApiWithProductRepository<StubSuccessProductRepository>().CreateClient();

            var response = await client.GetAsync(Utility.SetGet_Or_DeleteUrl(Id, ApiEndpoints.ProductEndpoints.Get));

            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Return_200_From_Update_Endpoint()
        {
            using var client = CreateApiWithProductRepository<StubSuccessProductRepository>().CreateClient();

            string url = Utility.SetGet_Or_DeleteUrl(Id, ApiEndpoints.ProductEndpoints.Get);

            var getResponse = await client.GetAsync(url);

            var getResult = await getResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            var returned = JsonSerializer.Deserialize<Product>(getResult, jsonSerializerOptions)!;

            UpdateProductRequest request = new UpdateProductRequest
            {
                Id = returned.Id,
                Name = returned.Name,
                Description = returned.Description,
                Price = returned.Price,
                Type = returned.Type,
                Category = returned.Category,
                Brand = returned.Brand,
                ImageName = returned.ImageName
            };

            var jsonString = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, Utility.content_Type);

            var response = await client.PutAsync(ApiEndpoints.ProductEndpoints.Update, httpContent);

            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }


        [Fact]
        public async Task Should_Return_200_From_Delete_Endpoint()
        {
            using var client = CreateApiWithProductRepository<StubSuccessProductRepository>().CreateClient();

            var response = await client.DeleteAsync(Utility.SetGet_Or_DeleteUrl(Id, ApiEndpoints.ProductEndpoints.Delete));

            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }

        private static async Task<IEnumerable<Product>?> ReturnAllProducts(HttpClient client)
        {
            var response = await client.GetAsync(ApiEndpoints.ProductEndpoints.GetAll);

            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var products = JsonSerializer.Deserialize<IEnumerable<Product>>(result, jsonSerializerOptions);
            return products;
        }

        private ApiFactory CreateApiWithProductRepository<T>()
  where T : class, IProductService
        {
            var api = new ApiFactory(services =>
            {
                services.RemoveAll(typeof(IProductService));

                services.TryAddScoped<IProductService, T>();

            });
            return api;
        }
    }
}
