using Application.Models;
using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.ProductStorage
{
    public class ProductRepository : IProductRepository
    {
        private readonly ISqlDataAccess sqlDataAccess;

        public ProductRepository(ISqlDataAccess _sqlDataAccess)
        {
            sqlDataAccess = _sqlDataAccess;
        }
        public Task InsertProduct(Product Product) =>
           sqlDataAccess.SaveData("dbo.spProduct_Insert", new { Product.Name, Product.Description, Product.Price, Product.Type, Product.Category, Product.Brand, Product.ImageName });

        public Task<IEnumerable<Product>> GetProducts() => sqlDataAccess.LoadData<Product, dynamic>("dbo.spProduct_GetAll", new { });

        public async Task<Product?> GetProduct(int id)
        {
            var result = await sqlDataAccess.LoadData<Product, dynamic>("dbo.spProduct_GetById", new { Id = id });

            return result.FirstOrDefault();

        }
        public Task UpdateProduct(Product Product) => sqlDataAccess.SaveData("dbo.spProduct_Update", new { Product.Id, Product.Name, Product.Description, Product.Price, Product.Type, Product.Category, Product.Brand, Product.ImageName });

        public Task DeleteProduct(int id) => sqlDataAccess.SaveData("dbo.spProduct_Delete", new { Id = id });

    }
}
