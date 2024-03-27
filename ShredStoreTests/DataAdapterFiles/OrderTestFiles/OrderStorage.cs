using Application.Models;
using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShredStoreTests.DataAdapterFiles.OrderTestFiles
{
    public class OrderStorage : IOrderStorage
    {
        private TestSqlDataAccess _dataAccess;

        public OrderStorage(ISqlAccessConnectionFactory dbConnectionFactory)
        {
            ArgumentNullException.ThrowIfNull(dbConnectionFactory);
            _dataAccess = new TestSqlDataAccess(dbConnectionFactory);
        }
        public Task InsertOrder(Order Order) =>
        _dataAccess.SaveData("dbo.spOrder_Insert", new { Order.CreatedDate, Order.UserId, Order.TotalAmount, Order.PaymentId});

        public Task<IEnumerable<Order>> GetOrders(int UserId) => _dataAccess.LoadData<Order, dynamic>("dbo.spOrder_GetAllUserOrders", new { UserId });

        public async Task<Order?> GetOrder(int id)
        {
            var result = await _dataAccess.LoadData<Order, dynamic>("dbo.spOrder_GetById", new { Id = id });
            return result.FirstOrDefault();
        }
        public Task UpdateOrder(Order Order) => _dataAccess.SaveData("dbo.spOrder_Update", new { Order.Id, Order.CreatedDate, Order.TotalAmount});

        public Task DeleteOrder(int id) => _dataAccess.SaveData("dbo.spOrder_Delete", new { Id = id });
    }
}
