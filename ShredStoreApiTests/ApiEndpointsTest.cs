﻿namespace ShredStore
{
    public static class ApiEndpointsTest
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
    }
}