using Application.Models;
using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ISqlDataAccess sqlDataAccess;

        public OrderRepository(ISqlDataAccess _sqlDataAccess)
        {
            sqlDataAccess = _sqlDataAccess;
        }

        public Task InsertOrder(Order Order, CancellationToken token) =>
       sqlDataAccess.SaveData("dbo.spOrder_Insert", new { Order.CreatedDate, Order.UserId, Order.TotalAmount, Order.PaymentId }, token: token);

        public Task<IEnumerable<Order>> GetOrders(int UserId, CancellationToken token) => sqlDataAccess.LoadData<Order, dynamic>("dbo.spOrder_GetAllUserOrders", new { UserId }, token: token);

        public async Task<Order?> GetOrder(int id, CancellationToken token)
        {
            var result = await sqlDataAccess.LoadData<Order, dynamic>("dbo.spOrder_GetById", new { Id = id }, token: token);
            return result.FirstOrDefault();
        }
        public Task UpdateOrder(Order Order, CancellationToken token) => sqlDataAccess.SaveData("dbo.spOrder_Update", new { Order.Id, Order.CreatedDate, Order.TotalAmount}, token: token);

        public Task DeleteOrder(int id, CancellationToken token) => sqlDataAccess.SaveData("dbo.spOrder_Delete", new { Id = id }, token: token);
    }
}
