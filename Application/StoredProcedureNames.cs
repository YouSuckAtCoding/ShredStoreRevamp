namespace Application
{
    internal static class StoredProcedureNames
    {
        private const string Base = "dbo.";
        internal static class User
        {
            private const string UserBase = Base + "spUser_";

            internal static string Delete = UserBase + "Delete";
            internal static string GetById = UserBase + "GetById";
            internal static string GetAll = UserBase + "GetAll";
            internal static string Insert = UserBase + "Insert";
            internal static string Login = UserBase + "Login";
            internal static string ResetPassword = UserBase + "ResetPasswordByEmail";
            internal static string Update = UserBase + "Update";

        }
        internal static class Product
        {
            private const string ProductBase = Base + "spProduct_";
            
            internal static string Delete = ProductBase + "Delete";
            internal static string GetById = ProductBase + "GetById";
            internal static string GetByUser = ProductBase + "GetByUserId";
            internal static string GetByCategory = ProductBase + "GetByCategory";
            internal static string GetByCart = ProductBase + "GetByCartId";
            internal static string GetAll = ProductBase + "GetAll";
            internal static string Insert = ProductBase + "Insert";
            internal static string Login = ProductBase + "Login";
            internal static string ResetPassword = ProductBase + "ResetPasswordByEmail";
            internal static string Update = ProductBase + "Update";

        }
    }
}
