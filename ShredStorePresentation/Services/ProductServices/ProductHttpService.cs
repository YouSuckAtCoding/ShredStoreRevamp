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

        public async Task<ProductResponse> Create(CreateProductRequest product, CancellationToken token)
        {
            var httpResponseMessage = await httpClient.PostAsJsonAsync(ApiEndpoints.ProductEndpoints.Create, product, token);

            var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var created = await JsonSerializer.DeserializeAsync<ProductResponse>(contentStream, jsonSerializerOptions);

            return created;

        }

        public async Task Delete(int id, CancellationToken token)
        {
            var httpResponseMessage = await httpClient.DeleteAsync(ApiEndpoints.UrlGenerator.SetUrlParameters(id, ApiEndpoints.ProductEndpoints.Delete), token);
            httpResponseMessage.EnsureSuccessStatusCode();

        }
        public async Task<ProductResponse> Edit(UpdateProductRequest product, CancellationToken token)
        {
            var httpResponseMessage = await httpClient.PutAsJsonAsync(ApiEndpoints.ProductEndpoints.Update, product, token);

            var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var edited = await JsonSerializer.DeserializeAsync<ProductResponse>(contentStream, jsonSerializerOptions);
            return edited;
        }
        public async Task<IEnumerable<ProductResponse>> GetAll(CancellationToken token)
        {
            var products = await httpClient.GetFromJsonAsync<IEnumerable<ProductResponse>>(ApiEndpoints.ProductEndpoints.GetAll, token);
            return products;

        }
        public async Task<IEnumerable<ProductResponse>> GetAllByCategory(string Category, CancellationToken token)
        {
            var products = await httpClient.GetFromJsonAsync<IEnumerable<ProductResponse>>(ApiEndpoints.UrlGenerator.SetUrlParameters(Category, ApiEndpoints.ProductEndpoints.GetByCategory), token);
            return products;

        }
        public async Task<IEnumerable<ProductResponse>> GetAllByUserId(int UserId, CancellationToken token)
        {
            var products = await httpClient.GetFromJsonAsync<IEnumerable<ProductResponse>>(ApiEndpoints.UrlGenerator.SetUrlParameters(UserId, ApiEndpoints.ProductEndpoints.GetByUserId), token);
            return products;

        }
        public async Task<IEnumerable<ProductCartItemResponse>> GetAllByCartId(int cartId, CancellationToken token)
        {
            var products = await httpClient.GetFromJsonAsync<IEnumerable<ProductCartItemResponse>>(ApiEndpoints.UrlGenerator.SetUrlParameters(cartId, ApiEndpoints.ProductEndpoints.GetByCartId), token);
            return products;

        }
        public async Task<ProductResponse> GetById(int id, CancellationToken token)
        {
            var Usuario = await httpClient.GetFromJsonAsync<ProductResponse>(ApiEndpoints.UrlGenerator.SetUrlParameters(id, ApiEndpoints.ProductEndpoints.Get), token);
            return Usuario;
        }
    }

}
