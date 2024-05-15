using Contracts.Request.CartRequests;
using Contracts.Response.CartResponses;
using Contracts.Response.UserResponses;
using System.Text.Json;

namespace ShredStorePresentation.Services.CartServices
{
    public class CartHttpService : ICartHttpService
    {
        private readonly HttpClient httpClient;

        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            IgnoreNullValues = true,
            PropertyNameCaseInsensitive = true

        };

        public CartHttpService(IConfiguration config)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(config.GetValue<string>(Constants.ConfigApiUri)!);
        }

        public async Task<CreateCartRequest> Create(CreateCartRequest cart, string token)
        {
            var httpResponseMessage = await httpClient.PostAsJsonAsync(ApiEndpoints.CartEndpoints.Create, cart);

            var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var created = await JsonSerializer.DeserializeAsync<CreateCartRequest>(contentStream, jsonSerializerOptions);

            return created;

        }

        public async Task<CartResponse> GetById(int id, string token)
        {   
            var response = await httpClient.GetAsync(ApiEndpoints.UrlGenerator.SetUrlParameters(id, ApiEndpoints.CartEndpoints.Get));

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return new CartResponse();
            var result = await response.Content.ReadAsStringAsync();

            CartResponse cart = JsonSerializer.Deserialize<CartResponse>(result, jsonSerializerOptions)!;

            return cart;
        }


    }
}
