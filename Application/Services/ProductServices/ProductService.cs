using Application.Models;
using Application.Repositories.ProductStorage;
using Contracts.Response.ProductsResponses;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IValidator<Product> _productValidator;

        public ProductService(IProductRepository productRepository, IValidator<Product> productValidator)
        {
            _productRepository = productRepository;
            _productValidator = productValidator;
        }

        public async Task<bool> Create(Product product, CancellationToken token)
        {
            await _productValidator.ValidateAndThrowAsync(product, token);
            await _productRepository.InsertProduct(product, token);
            return true;
        }

        public async Task<bool> DeleteProduct(int id, CancellationToken token)
        {
            await _productRepository.DeleteProduct(id, token);
            return true;
        }

        public async Task<Product?> GetProduct(int id, CancellationToken token)
        {
            var result = await _productRepository.GetProduct(id, token);
            return result is not null ? result : null;
        }

        public async Task<IEnumerable<Product>> GetProducts(CancellationToken token)
        {
            IEnumerable<Product> products = await _productRepository.GetProducts(token);
            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(string category, CancellationToken token)
        {
            IEnumerable<Product> products = await _productRepository.GetProductsByCategory(category, token);
            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsByUser(int id, CancellationToken token)
        {
            IEnumerable<Product> products = await _productRepository.GetProductsByUserId(id, token);
            return products;
        }

        public async Task<Product?> UpdateProduct(Product product, CancellationToken token)
        {
            await _productValidator.ValidateAndThrowAsync(product, token);
            await _productRepository.UpdateProduct(product, token);
            var result = await _productRepository.GetProduct(product.Id, token);
            return result is not null ? result : null;
        }

        public async Task<IEnumerable<ProductCartItemResponse>> GetCartProducts(int cartId, CancellationToken token)
        {
            var result = await _productRepository.GetCartProducts(cartId, token);
            if (result is null)
                return null;
            return result;
        }
    }
}
