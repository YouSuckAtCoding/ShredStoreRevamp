using Application.Models;
using Contracts.Request.ProductRequests;
using Contracts.Response.ProductsResponses;

namespace ShredStore.Mapping
{
    public static class ProductMapping
    {

        public static Product MapToProduct(this CreateProductRequest request)
        {
            return new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Type = request.Type,
                Category = request.Category,
                Brand = request.Brand,
                ImageName = request.ImageName
            };
        }
        public static Product MapToProduct(this UpdateProductRequest request)
        {
            return new Product
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Type = request.Type,
                Category = request.Category,
                Brand = request.Brand,
                ImageName = request.ImageName
            };
        }
        public static ProductResponse MapToProductResponse(this Product request)
        {
            return new ProductResponse
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Type = request.Type,
                Category = request.Category,
                Brand = request.Brand,
                ImageName = request.ImageName
            };
        }
    }
}
