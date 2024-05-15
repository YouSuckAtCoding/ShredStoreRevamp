using Application.Models;
using Application.Services.OrderItemServices;
using Contracts.Request.OrderItemRequests;
using Contracts.Response.OrdertemResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShredStore.Jwt;
using ShredStore.Mapping;

namespace ShredStore.Controllers
{

    [ApiController]
    [Authorize(AuthConstants.CustomerPolicyName)]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(IOrderItemService OrderItemService)
        {
            _orderItemService = OrderItemService;
        }

        [HttpPost(ApiEndpoints.OrderItemEndpoints.Create)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateOrderItemRequest request, CancellationToken token)
        {
            OrderItem orderItem = request.MapToOrderItem();
            bool result = await _orderItemService.CreateOrderItem(orderItem, token);
            return result ? Created("shredstore.com", orderItem) : BadRequest();

        }


        [HttpGet(ApiEndpoints.OrderItemEndpoints.GetAll)]
        [ProducesResponseType(typeof(OrderItemsResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromRoute] int orderId, CancellationToken token)
        {
            var result = await _orderItemService.GetOrderItems(orderId, token);
            return Ok(result);
        }

        [HttpGet(ApiEndpoints.OrderItemEndpoints.Get)]
        [ProducesResponseType(typeof(OrderItemResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] int itemId, [FromRoute] int orderId, CancellationToken token)
        {
            OrderItem item = await _orderItemService.GetOrderItem(itemId, orderId, token);
            return item is not null ? Ok(item.MapToOrderItemResponse()) : NotFound();
        }

        [HttpPut(ApiEndpoints.OrderItemEndpoints.Update)]
        [ProducesResponseType(typeof(OrderItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] UpdateOrderItemRequest request, CancellationToken token)
        {
            OrderItem item = request.MapToOrderItem();

            OrderItem? result = await _orderItemService.UpdateOrderItem(item.ProductId, item.OrderId, item.Quantity, token);

            return Ok(result.MapToOrderItemResponse());
        }

        [HttpDelete(ApiEndpoints.OrderItemEndpoints.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int itemId, [FromRoute] int orderId, CancellationToken token)
        {
            var result = await _orderItemService.DeleteItem(itemId, orderId, token);
            return result ? Ok() : NotFound();
        }

        [HttpDelete(ApiEndpoints.OrderItemEndpoints.DeleteAll)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int orderId, CancellationToken token)
        {
            var result = await _orderItemService.DeleteAlltems(orderId, token);
            return result ? Ok() : NotFound();
        }
    }
}
