using Contracts.Request.CartItemRequests;
using Contracts.Response.CartItemResponses;
using System.Text.Json;

namespace ShredStorePresentation.Services.CartItemServices
{
    public class CartItemHttpService : ICartItemHttpService
    {
        private readonly HttpClient httpClient;

        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            IgnoreNullValues = true,
            PropertyNameCaseInsensitive = true

        };

        public CartItemHttpService(IConfiguration config)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(config.GetValue<string>("ApiUri")!);
        }

        public async Task<bool> InsertCartItems(CreateCartItemRequest request, string token)
        {
            var httpResponseMessage = await httpClient.PostAsJsonAsync(ApiEndpoints.CartItemEndpoints.Create, request);
            if (httpResponseMessage.StatusCode != System.Net.HttpStatusCode.OK)
                return false;

            return true;
        }

        public async Task<IEnumerable<CartItemResponse>> GetCartItems(int cartId, string token)
        {
            var httpResponseMessage = await httpClient.GetAsync(ApiEndpoints.UrlGenerator.
                SetUrlParameters(cartId, ApiEndpoints.CartItemEndpoints.GetAll));

            var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var products = await JsonSerializer.DeserializeAsync<IEnumerable<CartItemResponse>>(contentStream, jsonSerializerOptions);

            if (products is null)
                return new List<CartItemResponse>();

            return products;
        }

        public async Task<bool> UpdateCartItem(UpdateCartItemRequest request, string token)
        {
            var httpResponseMessage = await httpClient.PutAsJsonAsync(ApiEndpoints.CartItemEndpoints.Update, request);

            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            return false;
        }

        public async Task RemoveItem(int productId, int userId, string token)
        {
            await httpClient.DeleteAsync(ApiEndpoints.UrlGenerator.SetCartItem_GetUrl(productId, userId, ApiEndpoints.CartItemEndpoints.Delete));

        }
    }
}
