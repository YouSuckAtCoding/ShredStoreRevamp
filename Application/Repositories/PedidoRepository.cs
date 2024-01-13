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
        public Task InsertPedido(Order pedido) =>
            sqlDataAccess.SaveData("dbo.spPedido_Insert", new { DateTime.Now, pedido.CartId, pedido.TotalAmount, pedido.UserId });

        public Task<IEnumerable<Order>> GetPedidos() => sqlDataAccess.LoadData<Order, dynamic>("dbo.spPedido_GetAll", new { });

        public async Task<Order?> GetPedido(int id)
        {
            var result = await sqlDataAccess.LoadData<Order, dynamic>("dbo.spPedido_GetById", new { Id = id });

            return result.FirstOrDefault();

        }
        public Task UpdatePedido(Order Pedido) => sqlDataAccess.SaveData("dbo.spPedido_Update", new { Pedido.Id, Pedido.Date, Pedido.TotalAmount });

        public Task DeletePedido(int id) => sqlDataAccess.SaveData("dbo.spPedido_Delete", new { Id = id });
    }
}
