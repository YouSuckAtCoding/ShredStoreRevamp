using Application.Models;
using ShredStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShredStoreApiTests
{
    public static class Utility
    {
        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            IgnoreNullValues = true,
            PropertyNameCaseInsensitive = true
        };
        public static string SetGet_Or_DeleteUrl(int id, string endpoint)
        {
            string url = endpoint;
            url = url.Replace( "{id}", $"{id}");
            url = url.Replace("{id:int}", $"{id}");
            url = url.Replace("{userId}", $"{id}");
            url = url.Replace("{orderId}", $"{id}");
            return url;
        }

        public static string SetCartItem_GetUrl(int itemId = 0, int cartId = 0, string endpoint = "")
        {
            string url = endpoint;
            url = url.Replace("{cartId}", $"{cartId}");
            url = url.Replace("{itemId}", $"{itemId}");
            return url;
        }

        public static async Task<IEnumerable<User>?> GetAllUsers(HttpClient client)
        {
            var response = await client.GetAsync(ApiEndpointsTest.UserEndpoints.GetAll);
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var users = JsonSerializer.Deserialize<IEnumerable<User>>(result, jsonSerializerOptions);
            return users;
        }

        public static async Task<IEnumerable<Product>?> ReturnAllProducts(HttpClient client)
        {
            var response = await client.GetAsync(ApiEndpointsTest.ProductEndpoints.GetAll);

            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var products = JsonSerializer.Deserialize<IEnumerable<Product>>(result, jsonSerializerOptions);
            return products;
        }



    }
}
