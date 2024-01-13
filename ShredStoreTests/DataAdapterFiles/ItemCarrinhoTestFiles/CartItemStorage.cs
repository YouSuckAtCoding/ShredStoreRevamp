using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShredStoreTests.DataAdapterFiles.ItemCartTestFiles
{
    public class ItemCartStorage
    {
        private TestSqlDataAccess _dataAccess;

        public ItemCartStorage(ISqlAccessConnectionFactory dbConnectionFactory)
        {
            ArgumentNullException.ThrowIfNull(dbConnectionFactory);
            _dataAccess = new TestSqlDataAccess(dbConnectionFactory);
        }

        public Task InsertProduct(Product Product) =>
         _dataAccess.SaveData("dbo.spProduct_Insert", new { Product.Name, Product.Description, Product.Price, Product.Type, Product.Category });

        public Task<IEnumerable<Product>> GetProducts() => _dataAccess.LoadData<Product, dynamic>("dbo.spProduct_GetAll", new { });
        public Task DeleteProduct(int id) => _dataAccess.SaveData("dbo.spProduct_Delete", new { Id = id });


    }
}
