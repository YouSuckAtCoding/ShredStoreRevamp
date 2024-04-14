using Application.Models;
using Contracts.Request.OrderRequests;
using Microsoft.SqlServer.Management.Smo;
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
        public const string content_Type = "application/json";
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

    }
}
