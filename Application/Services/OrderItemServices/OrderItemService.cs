using Application.Models;
using Application.Repositories;
using Application.Repositories.OrderItemStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.OrderItemServices
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderItemService(IOrderItemRepository orderItemRepository, IOrderRepository orderRepository)
        {
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
        }

        public async Task<bool> CreateOrderItem(OrderItem OrderItem, CancellationToken token)
        {

            await _orderItemRepository.InsertOrderItem(OrderItem, token);
            return true;
        }

        public async Task<bool> DeleteAlltems(int orderId, CancellationToken token)
        {
            var result = await _orderRepository.GetOrder(orderId, token);

            if (result is null)
                return false;

            await _orderItemRepository.DeleteAllOrderItem(orderId, token);
            return true;
        }

        public async Task<bool> DeleteItem(int itemId, int orderId, CancellationToken token)
        {
            var result = await _orderItemRepository.GetOrderItem(itemId, orderId, token);

            if (result is null)
                return false;

            await _orderItemRepository.DeleteOrderItem(itemId, orderId, token);
            return true;
        }

        public async Task<OrderItem> GetOrderItem(int itemId, int orderId, CancellationToken token)
        {
            var result = await _orderItemRepository.GetOrderItem(itemId, orderId, token);
            return result;
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItems(int orderId, CancellationToken token)
        {
            IEnumerable<OrderItem> result = await _orderItemRepository.GetOrderItems(orderId, token);
            return result;
        }

        public async Task<OrderItem?> UpdateOrderItem(int itemId, int orderId, int quantity, CancellationToken token)
        {
            await _orderItemRepository.UpdateOrderItem(itemId, quantity, orderId, token);
            return await _orderItemRepository.GetOrderItem(itemId, orderId, token);

        }
    }
}
