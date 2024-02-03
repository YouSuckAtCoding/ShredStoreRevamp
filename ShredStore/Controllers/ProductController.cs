using Application.Models;
using Application.Services.ProductServices;
using Contracts.Request.ProductRequests;
using Contracts.Response.ProductsResponses;
using Contracts.Response.UserResponses;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var products = await _productService.GetProducts(token);
            return Ok(products);
        }

        [HttpGet(ApiEndpoints.ProductEndpoints.Get)]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HttpPost(ApiEndpoints.ProductEndpoints.Create)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken token)
        {
            var product = request.MapToProduct();
            bool res = await _productService.Create(product, token);
            return res ? Ok(res) : BadRequest();
        }

        [HttpPut(ApiEndpoints.ProductEndpoints.Update)]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateProductRequest request, CancellationToken token)
        {
            Product product = request.MapToProduct();

            Product? updated = await _productService.UpdateProduct(product, token);

            if(updated is not null)
                return Ok(updated.MapToProductResponse());

            return NotFound();
        }

        [HttpDelete(ApiEndpoints.ProductEndpoints.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken token)
        {
            var result = await _productService.DeleteProduct(id, token);
            return result ? Ok() : NotFound();
        }
    }
}
