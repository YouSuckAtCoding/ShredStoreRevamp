using Application.Models;
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

        public async Task<Product?> GetProduct(int id)
        {
            var result = await _dataAccess.LoadData<Product, dynamic>("dbo.spProduct_GetById", new { Id = id });

            return result.FirstOrDefault();

        }
        public Task UpdateProduct(Product Product) => _dataAccess.SaveData("dbo.spProduct_Update", new { Product.Id, Product.Name, Product.Description, Product.Price, Product.Type, Product.Category, Product.Brand, Product.ImageName});

        public Task DeleteProduct(int id) => _dataAccess.SaveData("dbo.spProduct_Delete", new { Id = id });


    }
}
