using Application.Models;
using Contracts.Response.ProductsResponses;
using DatabaseAccess;

namespace Application.Repositories.ProductStorage
{
    public class ProductRepository : IProductRepository
    {
        private readonly ISqlDataAccess sqlDataAccess;

        public ProductRepository(ISqlDataAccess _sqlDataAccess)
        {
            sqlDataAccess = _sqlDataAccess;
        }
        public Task InsertProduct(Product Product, CancellationToken token) =>
           sqlDataAccess.SaveData(StoredProcedureNames.Product.Insert, new { Product.Name, Product.Description, Product.Price, Product.Type, Product.Category, Product.Brand, Product.ImageName, Product.UserId }, token: token);

        public Task<IEnumerable<Product>> GetProducts(CancellationToken token) => sqlDataAccess.LoadData<Product, dynamic>(StoredProcedureNames.Product.GetAll, new { }, token: token);

        public Task<IEnumerable<Product>> GetProductsByCategory(string Category, CancellationToken token) => sqlDataAccess.LoadData<Product, dynamic>(StoredProcedureNames.Product.GetByCategory, new { Category }, token: token);
        public Task<IEnumerable<Product>> GetProductsByUserId(int Id, CancellationToken token) => sqlDataAccess.LoadData<Product, dynamic>(StoredProcedureNames.Product.GetByUser, new { UserId = Id }, token: token);

        public async Task<Product> GetProduct(int id, CancellationToken token)
        {
            var result = await sqlDataAccess.LoadData<Product, dynamic>(StoredProcedureNames.Product.GetById, new { Id = id }, token: token);

            return result is null ? new Product() : result.First();
        }
        public Task UpdateProduct(Product Product, CancellationToken token) => sqlDataAccess.SaveData(StoredProcedureNames.Product.Update, new { Product.Id, Product.Name, Product.Description, Product.Price, Product.Type, Product.Category, Product.Brand, Product.ImageName }, token: token);

        public Task DeleteProduct(int id, CancellationToken token) => sqlDataAccess.SaveData(StoredProcedureNames.Product.Delete, new { Id = id }, token: token);

        public Task<IEnumerable<ProductCartItemResponse>> GetCartProducts(int cartId, CancellationToken token) => sqlDataAccess.LoadData<ProductCartItemResponse, dynamic>(StoredProcedureNames.Product.GetByCart, new { CartId = cartId }, token: token);

    }
}
