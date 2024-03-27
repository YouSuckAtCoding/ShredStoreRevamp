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
        [Fact]
        public async Task Should_Return_200_From_GetAll_Endpoint()
        {
            using var client = CreateApiWithProductRepository<StubSuccessProductRepository>().CreateClient();

            var response = await client.GetAsync(ApiEndpointsTest.ProductEndpoints.GetAll);

            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }
        [Fact]
        public async Task Should_Return_All_Products_Real_GetAll_Endpoint()
        {

            using var client = CreateApi.CreateOfficialApi().CreateClient();
            var response = await client.GetAsync(ApiEndpointsTest.ProductEndpoints.GetAll);
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var products= JsonSerializer.Deserialize<IEnumerable<Product>>(result, jsonSerializerOptions);

            products.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Should_Return_200_From_Create_Endpoint()
        {
            using var client = CreateApiWithProductRepository<StubSuccessProductRepository>().CreateClient();
            var request = FakeDataFactory.FakeCreateProductRequest();

            var jsonString = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(ApiEndpointsTest.ProductEndpoints.Create, httpContent);
            var responseInfo = response.Content.ReadAsStringAsync();

            response.Should().HaveStatusCode(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Should_Insert_Product_Create_Endpoint()
        {
            using var client = CreateApi.CreateOfficialApi().CreateClient();
            var request = FakeDataFactory.FakeCreateProductRequest();

            var jsonString = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(ApiEndpointsTest.ProductEndpoints.Create, httpContent);

            response.Should().HaveStatusCode(HttpStatusCode.Created);

        }
        [Fact]
        public async Task Should_Return_200_From_Get_Endpoint()
        {
            using var client = CreateApiWithProductRepository<StubSuccessProductRepository>().CreateClient();

            var response = await client.GetAsync(Utility.SetGet_Or_DeleteUrl(2, ApiEndpoints.ProductEndpoints.Get));

            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Return_Product_From_Real_Get_Endpoint()
        {
            using var client = CreateApi.CreateOfficialApi().CreateClient();

            IEnumerable<Product>? products = await ReturnAllProducts(client).ConfigureAwait(false);

            var response = await client.GetAsync(Utility.SetGet_Or_DeleteUrl(products!.First().Id, ApiEndpoints.ProductEndpoints.Get));
            var responseInfo = await response.Content.ReadAsStringAsync();

            var product = JsonSerializer.Deserialize<ProductResponse>(responseInfo, jsonSerializerOptions);

            product.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Should_Return_200_From_Update_Endpoint()
        {
            using var client = CreateApiWithProductRepository<StubSuccessProductRepository>().CreateClient();

            string url = Utility.SetGet_Or_DeleteUrl(2, ApiEndpoints.ProductEndpoints.Get);
            
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

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(ApiEndpointsTest.ProductEndpoints.Update, httpContent);

            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Update_Product_Real_Update_Endpoint()
        {
            using var client = CreateApi.CreateOfficialApi().CreateClient();

            IEnumerable<Product>? products = await ReturnAllProducts(client).ConfigureAwait(false);

            int id = products.Last().Id;

            var getResponse = await client.GetAsync(Utility.SetGet_Or_DeleteUrl(id, ApiEndpoints.ProductEndpoints.Get));

            var getResult = await getResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            var returned = JsonSerializer.Deserialize<Product>(getResult, jsonSerializerOptions)!;

            UpdateProductRequest request = new UpdateProductRequest
            {
                Id = returned.Id,
                Name = "Teste novo 123",
                Description = returned.Description,
                Price = returned.Price,
                Type = returned.Type,
                Category = returned.Category,
                Brand = "Ibanez",
                ImageName = returned.ImageName
            };

            var jsonString = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(ApiEndpointsTest.ProductEndpoints.Update, httpContent);

            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var product = JsonSerializer.Deserialize<Product>(result, jsonSerializerOptions);
            product!.Name.Should().Be("Teste novo 123");
            product!.Brand.Should().Be("Ibanez");

        }

        [Fact]
        public async Task Should_Return_200_From_Delete_Endpoint()
        {
            using var client = CreateApiWithProductRepository<StubSuccessProductRepository>().CreateClient();

            var response = await client.DeleteAsync(Utility.SetGet_Or_DeleteUrl(2, ApiEndpoints.ProductEndpoints.Delete));

            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Delete_Product_Real_Delete_Endpoint()
        {
            using var client = CreateApi.CreateOfficialApi().CreateClient();

            IEnumerable<Product>? products = await ReturnAllProducts(client).ConfigureAwait(false);

            int id = products.Last().Id;

            await client.DeleteAsync(Utility.SetGet_Or_DeleteUrl(id, ApiEndpoints.ProductEndpoints.Delete));

            string url = Utility.SetGet_Or_DeleteUrl(id, ApiEndpoints.ProductEndpoints.Delete);

            var getResponse = await client.GetAsync(url);

            var getResult = await getResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            var returned = JsonSerializer.Deserialize<Product>(getResult, jsonSerializerOptions)!;

            returned.Id.Should().Be(0);

        }

        private static async Task<IEnumerable<Product>?> ReturnAllProducts(HttpClient client)
        {
            var response = await client.GetAsync(ApiEndpointsTest.ProductEndpoints.GetAll);

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
