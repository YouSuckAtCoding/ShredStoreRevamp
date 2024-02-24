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

        public async Task<ProductViewResponse> Create(ProductViewResponse product)
        {
            var httpResponseMessage = await httpClient.PostAsJsonAsync(ApiEndpoints.ProductEndpoints.Create, product);

            var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var created = await JsonSerializer.DeserializeAsync<ProductViewResponse>(contentStream, jsonSerializerOptions);

            return created;

        }

        public async Task Delete(int id)
        {
            var httpResponseMessage = await httpClient.DeleteAsync(ApiEndpoints.UrlGenerator.SetUrlParameters(id, ApiEndpoints.ProductEndpoints.Delete));
            httpResponseMessage.EnsureSuccessStatusCode();

        }
        public async Task<ProductViewResponse> Edit(ProductViewResponse product)
        {
            var httpResponseMessage = await httpClient.PutAsJsonAsync(ApiEndpoints.ProductEndpoints.Update, product);

            var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var edited = await JsonSerializer.DeserializeAsync<ProductViewResponse>(contentStream, jsonSerializerOptions);
            return edited;
        }
        public async Task<IEnumerable<ProductViewResponse>> GetAll()
        {
            var products = await httpClient.GetFromJsonAsync<IEnumerable<ProductViewResponse>>(ApiEndpoints.ProductEndpoints.GetAll, cancellationToken:default);
            return products;

        }
        public async Task<IEnumerable<ProductViewResponse>> GetAllByCategory(string Category)
        {
            var products = await httpClient.GetFromJsonAsync<IEnumerable<ProductViewResponse>>($"api/v1/Product/GetAll/{Category}");
            return products;

        }
        public async Task<IEnumerable<ProductViewResponse>> GetAllByUserId(int UserId)
        {
            var products = await httpClient.GetFromJsonAsync<IEnumerable<ProductViewResponse>>($"api/v1/Product/GetAllByUser/{UserId}");
            return products;

        }
        public async Task<ProductViewResponse> GetById(int id)
        {
            var Usuario = await httpClient.GetFromJsonAsync<ProductViewResponse>(ApiEndpoints.UrlGenerator.SetUrlParameters(id, ApiEndpoints.ProductEndpoints.Get));
            return Usuario;
        }
    }

}
