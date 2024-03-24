using Contracts.Request.ProductRequests;
using Contracts.Response.ProductsResponses;
using ShredStorePresentation.Models;
using System.Text.Json;

namespace ShredStorePresentation.Services.ProductServices
{
    public class ProductHttpService : IProductHttpService
    {
        private readonly HttpClient httpClient;

        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            IgnoreNullValues = true

        };

        public ProductHttpService(IConfiguration config)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(config.GetValue<string>("ApiUri")!);
        }

        public async Task<ProductResponse> Create(CreateProductRequest product)
        {
            var httpResponseMessage = await httpClient.PostAsJsonAsync(ApiEndpoints.ProductEndpoints.Create, product);

            var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var created = await JsonSerializer.DeserializeAsync<ProductResponse>(contentStream, jsonSerializerOptions);

            return created;

        }

        public async Task Delete(int id)
        {
            var httpResponseMessage = await httpClient.DeleteAsync(ApiEndpoints.UrlGenerator.SetUrlParameters(id, ApiEndpoints.ProductEndpoints.Delete));
            httpResponseMessage.EnsureSuccessStatusCode();

        }
        public async Task<ProductResponse> Edit(UpdateProductRequest product)
        {
            var httpResponseMessage = await httpClient.PutAsJsonAsync(ApiEndpoints.ProductEndpoints.Update, product);

            var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var edited = await JsonSerializer.DeserializeAsync<ProductResponse>(contentStream, jsonSerializerOptions);
            return edited;
        }
        public async Task<IEnumerable<ProductResponse>> GetAll()
        {
            var products = await httpClient.GetFromJsonAsync<IEnumerable<ProductResponse>>(ApiEndpoints.ProductEndpoints.GetAll, cancellationToken: default);
            return products;

        }
        public async Task<IEnumerable<ProductResponse>> GetAllByCategory(string Category)
        {
            var products = await httpClient.GetFromJsonAsync<IEnumerable<ProductResponse>>(ApiEndpoints.UrlGenerator.SetUrlParameters(Category, ApiEndpoints.ProductEndpoints.GetByCategory));
            return products;

        }
        public async Task<IEnumerable<ProductResponse>> GetAllByUserId(int UserId)
        {
            var products = await httpClient.GetFromJsonAsync<IEnumerable<ProductResponse>>(ApiEndpoints.UrlGenerator.SetUrlParameters(UserId, ApiEndpoints.ProductEndpoints.GetByUserId));
            return products;

        }
        public async Task<ProductResponse> GetById(int id)
        {
            var Usuario = await httpClient.GetFromJsonAsync<ProductResponse>(ApiEndpoints.UrlGenerator.SetUrlParameters(id, ApiEndpoints.ProductEndpoints.Get));
            return Usuario;
        }
    }

}
