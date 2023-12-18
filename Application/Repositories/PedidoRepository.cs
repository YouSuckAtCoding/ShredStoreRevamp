using Application.Models;
using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly ISqlDataAccess sqlDataAccess;

        public PedidoRepository(ISqlDataAccess _sqlDataAccess)
        {
            sqlDataAccess = _sqlDataAccess;
        }
        public Task InsertPedido(Pedido pedido) =>
            sqlDataAccess.SaveData("dbo.spPedido_Insert", new { DateTime.Now, pedido.CarrinhoId, pedido.Valor, pedido.UsuarioId });

        public Task<IEnumerable<Pedido>> GetPedidos() => sqlDataAccess.LoadData<Pedido, dynamic>("dbo.spPedido_GetAll", new { });

        public async Task<Pedido?> GetPedido(int id)
        {
            var result = await sqlDataAccess.LoadData<Pedido, dynamic>("dbo.spPedido_GetById", new { Id = id });

            return result.FirstOrDefault();

        }
        public Task UpdatePedido(Pedido Pedido) => sqlDataAccess.SaveData("dbo.spPedido_Update", new { Pedido.Id, Pedido.Data, Pedido.Valor });

        public Task DeletePedido(int id) => sqlDataAccess.SaveData("dbo.spPedido_Delete", new { Id = id });
    }
}
