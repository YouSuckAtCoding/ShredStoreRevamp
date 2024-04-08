using Contracts.Request;
using Contracts.Request.ProductRequests;
using Contracts.Response.ProductsResponses;
using Contracts.Response.UserResponses;
using ShredStorePresentation.Models;

namespace ShredStorePresentation.Extensions
{
    public static class Mapper
    {

        public static CreateProductRequest MapToProductRequest(this CreateProductViewRequest request)
        {
            return new CreateProductRequest
            {
                UserId = request.UserId,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Type = request.Type,
                Category = request.Category,
                Brand = request.Brand,
                ImageName = request.ImageName
            };

        }
        public static UpdateProductViewRequest MapToUpdateProductRequest(this ProductResponse request)
        {
            return new UpdateProductViewRequest
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

        public static UpdateProductRequest MapToUpdateProductRequest(this UpdateProductViewRequest request)
        {
            return new UpdateProductRequest
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

        public static UpdateUserRequest MapToUpdateUserRequest(this UserResponse request)
        {
            return new UpdateUserRequest
            {
                Id = request.Id,
                Name = request.Name,
                Address = request.Address,
                Role = request.Role
            };
        }

    }
}
