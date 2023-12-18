using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Endereco { get; set; }
        public string Password { get; set; }


    }
}
