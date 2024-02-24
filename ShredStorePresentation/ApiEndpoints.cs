﻿namespace ShredStorePresentation
{
    public static class ApiEndpoints
    {
        
        private const string ApiBase = "api";

        public static class UserEndpoints
        {
            private const string Base = $"{ApiBase}/users";

            public const string Create = Base;
            public const string Get = $"{Base}/{{id}}";
            public const string GetAll = Base;
            public const string Update = $"{Base}/update";
            public const string Delete = $"{Base}/{{id:int}}";
            public const string Login = $"{Base}/login";
            public const string ResetPassword = $"{Base}/reset";
        }

        public static class ProductEndpoints
        {
            private const string Base = $"{ApiBase}/products";

            public const string Create = Base;
            public const string Get = $"{Base}/{{id}}";
            public const string GetAll = Base;
            public const string Update = $"{Base}/update";
            public const string Delete = $"{Base}/{{id:int}}";
        }

        public static class CartEndpoints
        {
            private const string Base = $"{ApiBase}/cart";

            public const string Create = Base;
            public const string Get = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id:int}}";
        }

        public static class CartItemEndpoints
        {
            private const string Base = $"{ApiBase}/item";

            public const string Create = Base;
            public const string Get = $"{Base}/{{itemId}}/{{cartId}}";
            public const string GetAll = $"{Base}/{{cartId}}";
            public const string Update = $"{Base}/update";
            public const string Delete = $"{Base}/{{itemId}}/{{cartId}}";
            public const string DeleteAll = $"{Base}/{{cartId}}";
        }

        public static class OrderEndpoints
        {
            private const string Base = $"{ApiBase}/order";

            public const string Create = Base;
            public const string Get = $"{Base}/Id={{orderId}}";
            public const string GetAll = $"{Base}/{{userId}}";
            public const string GetAllOrders = $"{Base}";
            public const string Update = $"{Base}/update";
            public const string Delete = $"{Base}/{{orderId}}";
        }

        public static class OrderItemEndpoints
        {
            private const string Base = $"{ApiBase}/orderItem";

            public const string Create = Base;
            public const string Get = $"{Base}/{{itemId}}/{{orderId}}";
            public const string GetAll = $"{Base}/{{orderId}}";
            public const string Update = $"{Base}/update";
            public const string Delete = $"{Base}/{{itemId}}/{{orderId}}";
            public const string DeleteAll = $"{Base}/{{orderId}}";
        }

        public static class UrlGenerator
        {
            public static string SetUrlParameters(int id, string endpoint)
            {
                string url = endpoint;
                url = url.Replace("{id}", $"{id}");
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
}
