using Application.Models;
using Application.Services.CartServices;
using Contracts.Request.CartRequests;
using Contracts.Response.CartResponses;
using Contracts.Response.ProductsResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShredStore.Mapping;

namespace ShredStore.Controllers
{

    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost(ApiEndpoints.CartEndpoints.Create)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCartRequest request, CancellationToken token)
        {
            Cart cart = request.MapToCart();
            bool result = await _cartService.Create(cart, token);
            return result ? Created("shredstore.com", cart) : BadRequest();
        }

        [HttpGet(ApiEndpoints.CartEndpoints.Get)]
        [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken token)
        {
            Cart? cart = await _cartService.GetCart(id, token);
            if (cart is not null)
            {
                var result = cart.MapToCartResponse();
                return Ok(result);
            }
            return NotFound();
        }

        [HttpDelete(ApiEndpoints.CartEndpoints.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken token)
        {
            Cart? cart = await _cartService.GetCart(id, token);
            if (cart is not null)
            {
                await _cartService.DeleteCart(id, token);
                return Ok();
            }
            return NotFound();
        }

    }
}
