using Application.Models;
using Application.Services.CartItemServices;
using Contracts.Request.CartItemRequests;
using Contracts.Response.CartItemResponses;
using Microsoft.AspNetCore.Mvc;
using ShredStore.Mapping;

namespace ShredStore.Controllers
{

    [ApiController]
    public class CartItemController : ControllerBase
    {

        private readonly ICartItemService _cartItemService;

        public CartItemController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        [HttpPost(ApiEndpoints.CartItemEndpoints.Create)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCartItemRequest request, CancellationToken token)
        {
            CartItem cart = request.MapToCartItem();
            bool result = await _cartItemService.CreateCartItem(cart, token);
            return result ? Created("shredstore.com", cart) : BadRequest();
            
        }

        [HttpGet(ApiEndpoints.CartItemEndpoints.GetAll)]
        [ProducesResponseType(typeof(CartItemsResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromRoute] int cartId, CancellationToken token)
        {
            var result = await _cartItemService.GetCartItems(cartId, token);
            return Ok(result);
        }

        [HttpGet(ApiEndpoints.CartItemEndpoints.Get)]
        [ProducesResponseType(typeof(CartItemResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] int itemId, [FromRoute] int cartId, CancellationToken token)
        {
            CartItem item = await _cartItemService.GetCartItem(itemId, cartId, token);
            return item is not null ? Ok(item.MapToCartItemResponse()) : NotFound();
        }

        [HttpPut(ApiEndpoints.CartItemEndpoints.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] UpdateCartItemRequest request, CancellationToken token)
        {
            CartItem item = request.MapToCartItem();

            await _cartItemService.UpdateCartItem(item.ProductId, item.CartId, item.Quantity, token);

            return Ok();
        }

        [HttpDelete(ApiEndpoints.CartItemEndpoints.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int itemId, [FromRoute] int cartId, CancellationToken token)
        { 
            var result = await _cartItemService.DeleteItem(itemId, cartId, token);
            return result ? Ok() : NotFound();
        }

        [HttpDelete(ApiEndpoints.CartItemEndpoints.DeleteAll)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int cartId, CancellationToken token)
        {
            var result = await _cartItemService.DeleteAlltems(cartId, token);
            return result ? Ok() : NotFound();
        }
    }
}
