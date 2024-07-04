using Contracts.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public string Email { get; set; } = "";
        public string Cpf { get; set; } = "";
        public string Address { get; set; } = "";
        public string Password { get; set; } = "";
        public string Role { get; set; } = "";

    }
}
