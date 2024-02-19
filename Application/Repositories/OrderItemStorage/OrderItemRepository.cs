using Application.Models;
using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.OrderItemStorage
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ISqlDataAccess sqlDataAccess;

        public OrderItemRepository(ISqlDataAccess sqlDataAccess)
        {
            this.sqlDataAccess = sqlDataAccess;
        }

        public Task<IEnumerable<OrderItem>> GetOrderItems(int id, CancellationToken token)
        {
            var result = sqlDataAccess.LoadData<OrderItem, dynamic>("dbo.spOrderItem_GetAll", new { OrderId = id }, token: token);
            return result;
        }
        public Task InsertOrderItem(OrderItem OrderItem, CancellationToken token) =>
            sqlDataAccess.SaveData("dbo.spOrderItem_Insert", new { OrderItem.OrderId, OrderItem.ProductId, OrderItem.Quantity }, token: token);
        public Task UpdateOrderItem(int ProductId, int Quantity, int OrderId, CancellationToken token) =>
         sqlDataAccess.SaveData("dbo.spOrderItem_Update", new { ProductId, Quantity, OrderId }, token: token);

        public Task DeleteOrderItem(int ProductId, int OrderId, CancellationToken token) => sqlDataAccess.SaveData("dbo.spOrderItem_Delete", new { ProductId, OrderId }, token: token);
        public Task DeleteAllOrderItem(int OrderId, CancellationToken token) => sqlDataAccess.SaveData("dbo.spOrderItem_DeleteAll", new { OrderId }, token: token);

        public async Task<OrderItem?> GetOrderItem(int ProductId, int OrderId, CancellationToken token)
        {
            var result = await sqlDataAccess.LoadData<OrderItem, dynamic>("dbo.spOrderItem_GetById", new { ProductId, OrderId }, token: token);
            return result.FirstOrDefault();
        }


    }
}
