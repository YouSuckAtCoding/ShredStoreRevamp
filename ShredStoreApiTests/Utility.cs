using Application.Models;
using Contracts.Request.OrderRequests;
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
        public static string SetOrderItem_GetUrl(int itemId = 0, int orderId = 0, string endpoint = "")
        {
            string url = endpoint;
            url = url.Replace("{orderId}", $"{orderId}");
            url = url.Replace("{itemId}", $"{itemId}");
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

        public static async Task<int> SeuUpOrders(HttpClient client)
        {
            var users = await Utility.GetAllUsers(client);
            int id = users.Last().Id;
            for (int i = 0; i < 5; i++)
            {
                CreateOrderRequest request = new CreateOrderRequest
                {
                    CreatedDate = DateTime.Now,
                    UserId = id,
                    TotalAmount = 1000,
                    PaymentId = 3
                };
                var jsonString = JsonSerializer.Serialize(request);

                var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                await client.PostAsync(ApiEndpointsTest.OrderEndpoints.Create, httpContent);
            }

            return id;
        }
        public static async Task<IEnumerable<Order>> GetAllUserOrders(HttpClient client, int id)
        {
            var response = await client.GetAsync(Utility.SetGet_Or_DeleteUrl(id, ApiEndpointsTest.OrderEndpoints.GetAll));
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var orders = JsonSerializer.Deserialize<IEnumerable<Order>>(result, jsonSerializerOptions);
            return orders;
        }

        public static async Task<IEnumerable<Order>> GetAllOrders(HttpClient client)
        {
            var response = await client.GetAsync(ApiEndpointsTest.OrderEndpoints.GetAllOrders);
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var orders = JsonSerializer.Deserialize<IEnumerable<Order>>(result, jsonSerializerOptions);
            return orders;
        }



    }
}
