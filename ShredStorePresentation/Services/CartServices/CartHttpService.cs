using System.Text.Json;

namespace ShredStorePresentation.Services.CartServices
{
    public class CartHttpService
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
            httpClient.BaseAddress = new Uri(config.GetValue<string>("ApiUri")!);
        }



    }
}
