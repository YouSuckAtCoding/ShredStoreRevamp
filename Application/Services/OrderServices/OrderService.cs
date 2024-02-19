using Application.Models;
using Application.Repositories;
using Contracts.Response.OrderResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<bool> DeleteOrder(int id, CancellationToken token)
        {
            Order? result = await _orderRepository.GetOrder(id, token);
            if (result is null)
                return false;
            await _orderRepository.DeleteOrder(id, token);
            return true;
                   
        }

        public async Task<IEnumerable<Order>> GetAllOrders(CancellationToken token)
        {
            return await _orderRepository.GetAllOrders(token);
        }

        public async Task<Order?> GetOrder(int id, CancellationToken token)
        {
            return await _orderRepository.GetOrder(id, token);
        }

        public async Task<IEnumerable<Order>> GetOrders(int userId, CancellationToken token)
        {
            var result = await _orderRepository.GetOrders(userId, token);
            return result;               
        }

        public async Task<bool> InsertOrder(Order order, CancellationToken token)
        {
            await _orderRepository.InsertOrder(order, token);
            return true;
        }

        public async Task<Order?> UpdateOrder(Order order, CancellationToken token)
        {
            await _orderRepository.UpdateOrder(order, token);
            Order? result = await _orderRepository.GetOrder(order.Id, token);
            return result;

        }
    }
}
