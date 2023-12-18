using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Carrinho
    {
        public int UsuarioId { get; set; }
        public int ProdutoId { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
