using Application.Models;
using Application.Services.OrderServices;
using Contracts.Request;
using Contracts.Request.OrderRequests;
using Contracts.Response.OrderResponses;
using Contracts.Response.UserResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShredStore.Mapping;

namespace ShredStore.Controllers
{
    
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet(ApiEndpoints.OrderEndpoints.GetAll)]
        [ProducesResponseType(typeof(OrdersResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromRoute] int userId, CancellationToken token)
        {
            var result = await _orderService.GetOrders(userId, token);
            return Ok(result);
        }

        [HttpPost(ApiEndpoints.OrderEndpoints.Create)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken token)
        {
            Order Order = request.MapToOrder();

            bool result = await _orderService.InsertOrder(Order, token);
            if (result)
                return Created("shredstore.com", Order);

            return BadRequest();
        }

        [HttpGet(ApiEndpoints.OrderEndpoints.Get)]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] int orderId, CancellationToken token)
        {
            Order? order = await _orderService.GetOrder(orderId, token);
            if (order is not null)
            {
                var result = order.MapToOrderResponse();
                return Ok(result);
            }
            return NotFound();

        }

        [HttpPut(ApiEndpoints.OrderEndpoints.Update)]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(UpdateOrderRequest request, CancellationToken token)
        {
            Order order = request.MapToOrder();

            Order? updated = await _orderService.UpdateOrder(order, token);

            return Ok(updated.MapToOrderResponse());
        }

        [HttpDelete(ApiEndpoints.OrderEndpoints.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] int orderId, CancellationToken token)
        {

            var result = await _orderService.DeleteOrder(orderId, token);
            return result ? Ok() : NotFound();
        }
    }
}
