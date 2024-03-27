using Application.Models;
using Contracts.Response.ProductsResponses;
using DatabaseAccess;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShredStoreTests.DataAdapterFiles.ProductTestFiles
{
    public class ProductStorage : IProductStorage
    {
        private TestSqlDataAccess _dataAccess;

        public ProductStorage(ISqlAccessConnectionFactory dbConnectionFactory)
        {
            ArgumentNullException.ThrowIfNull(dbConnectionFactory);
            _dataAccess = new TestSqlDataAccess(dbConnectionFactory);
        }

        public Task InsertProduct(Product Product) =>
           _dataAccess.SaveData("dbo.spProduct_Insert", new { Product.Name, Product.Description, Product.Price, Product.Type, Product.Category, Product.Brand, Product.ImageName, Product.UserId });

        public Task<IEnumerable<Product>> GetProducts() => _dataAccess.LoadData<Product, dynamic>("dbo.spProduct_GetAll", new { });
        public Task<IEnumerable<Product>> GetProductsByCategory(string Category, CancellationToken token) => _dataAccess.LoadData<Product, dynamic>("dbo.spProduct_GetByCategory", new { Category }, token: token);
        public Task<IEnumerable<Product>> GetProductsByUserId(int Id, CancellationToken token) => _dataAccess.LoadData<Product, dynamic>("dbo.spProduct_GetByUserId", new { UserId = Id }, token: token);
        public async Task<Product?> GetProduct(int id)
        {
            var result = await _dataAccess.LoadData<Product, dynamic>("dbo.spProduct_GetById", new { Id = id });

            return result.FirstOrDefault();

        }
        public Task<IEnumerable<ProductCartItemResponse>> GetCartProducts(int cartId, CancellationToken token) => _dataAccess.LoadData<ProductCartItemResponse, dynamic>("dbo.spProduct_GetByCartId", new { CartId = cartId }, token: token);
        public Task UpdateProduct(Product Product) => _dataAccess.SaveData("dbo.spProduct_Update", new { Product.Id, Product.Name, Product.Description, Product.Price, Product.Type, Product.Category, Product.Brand, Product.ImageName });

        public Task DeleteProduct(int id) => _dataAccess.SaveData("dbo.spProduct_Delete", new { Id = id });


    }
}
