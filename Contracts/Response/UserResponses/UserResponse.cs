using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Response.UserResponses
{

    public static class Role
    {
        public const string Admin = "Adm";
        public const string Shop = "Shop";
        public const string Customer = "Customer";
    }

    public static class Claims
    {
        public const string Admin = "admin";
        public const string Shop = "shop_member";
        public const string Customer = "customer_member";
    }
    public class UserResponse
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public int Age { get; init; }
        public string Email { get; init; }
        public string Cpf { get; init; }
        public string Address { get; init; }
        public string Role { get; init; }
    }
}
