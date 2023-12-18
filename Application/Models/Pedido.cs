using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public int CarrinhoId { get; set; }
        public decimal Valor { get; set; }
        public int UsuarioId { get; set; }
    }
}
