using Application.Models;
using Application.Services.ProductServices;
using Contracts.Request.ProductRequests;
using Contracts.Response.ProductsResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShredStore.Jwt;
using ShredStore.Mapping;

namespace ShredStore.Controllers
{

    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        
        [HttpGet(ApiEndpoints.ProductEndpoints.GetAll)]
        [ProducesResponseType(typeof(ProductsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var products = await _productService.GetProducts(token);
            return Ok(products);
        }

        
        [HttpGet(ApiEndpoints.ProductEndpoints.GetByCategory)]
        [ProducesResponseType(typeof(ProductsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetByCategory([FromRoute] string category, CancellationToken token)
        {
            var products = await _productService.GetProductsByCategory(category, token);
            return Ok(products);
        }

        [Authorize(AuthConstants.ShopPolicyName)]
        [HttpGet(ApiEndpoints.ProductEndpoints.GetByUserId)]
        [ProducesResponseType(typeof(ProductsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetByUserId([FromRoute] int userId, CancellationToken token)
        {
            var products = await _productService.GetProductsByUser(userId, token);
            return Ok(products);
        }

        [Authorize(AuthConstants.CustomerPolicyName)]
        [HttpGet(ApiEndpoints.ProductEndpoints.GetByCartId)]
        [ProducesResponseType(typeof(ProductCartItemResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetByCartId([FromRoute] int cartId, CancellationToken token)
        {
            var products = await _productService.GetCartProducts(cartId, token);
            return Ok(products);
        }

        [HttpGet(ApiEndpoints.ProductEndpoints.Get)]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken token)
        {
            Product? product = await _productService.GetProduct(id, token);
            if (product is not null)
            {
                var result = product.MapToProductResponse();
                return Ok(result);
            }
            return NotFound();
        }

        [Authorize(AuthConstants.ShopPolicyName)]
        [HttpPost(ApiEndpoints.ProductEndpoints.Create)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken token)
        {
            var product = request.MapToProduct();
            bool res = await _productService.Create(product, token);
            return res ? Created("shredstore.com", product) : BadRequest();
        }

        [Authorize(AuthConstants.ShopPolicyName)]
        [HttpPut(ApiEndpoints.ProductEndpoints.Update)]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update([FromBody] UpdateProductRequest request, CancellationToken token)
        {
            Product product = request.MapToProduct();

            Product? updated = await _productService.UpdateProduct(product, token);

            if(updated is not null)
                return Ok(updated.MapToProductResponse());

            return NotFound();
        }

        [Authorize(AuthConstants.ShopPolicyName)]
        [HttpDelete(ApiEndpoints.ProductEndpoints.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken token)
        {
            var result = await _productService.DeleteProduct(id, token);
            return result ? Ok() : NotFound();
        }
    }
}
