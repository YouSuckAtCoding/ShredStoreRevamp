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
        public Task InsertOrder(Order Order) =>
       sqlDataAccess.SaveData("dbo.spOrder_Insert", new { Date = DateTime.Now, Order.CartId, Order.UserId });

        public Task<IEnumerable<Order>> GetOrders(int UserId) => sqlDataAccess.LoadData<Order, dynamic>("dbo.spOrder_GetAllUserOrders", new { UserId = UserId });

        public async Task<Order?> GetOrder(int id)
        {
            var result = await sqlDataAccess.LoadData<Order, dynamic>("dbo.spOrder_GetById", new { Id = id });
            return result.FirstOrDefault();
        }
        public Task UpdateOrder(Order Order) => sqlDataAccess.SaveData("dbo.spOrder_Update", new { Order.Id, Order.Date });

        public Task DeleteOrder(int id) => sqlDataAccess.SaveData("dbo.spOrder_Delete", new { Id = id });
    }
}
