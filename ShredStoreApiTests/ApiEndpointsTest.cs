namespace ShredStore
{
    public static class ApiEndpointsTest
    {
        private const string ApiBase = "api";

        public static class User
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
    }
}
