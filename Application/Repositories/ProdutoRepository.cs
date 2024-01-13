using Application.Models;
using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ISqlDataAccess sqlDataAccess;

        public ProdutoRepository(ISqlDataAccess _sqlDataAccess)
        {
            sqlDataAccess = _sqlDataAccess;
        }
        public Task InsertProduto(Product produto) =>
            sqlDataAccess.SaveData("dbo.spProduto_Insert", new { produto.Name, produto.Description, produto.Price, produto.Type, produto.Category });

        public Task<IEnumerable<Product>> GetProdutos() => sqlDataAccess.LoadData<Product, dynamic>("dbo.spProduto_GetAll", new { });

        public async Task<Product?> GetProduto(int id)
        {
            var result = await sqlDataAccess.LoadData<Product, dynamic>("dbo.spProduto_GetById", new { Id = id });

            return result.FirstOrDefault();

        }
        public Task UpdateProduto(Product produto) => sqlDataAccess.SaveData("dbo.spProduto_Update", new { produto.Id, produto.Name, produto.Description, produto.Price, produto.Type, produto.Category });

        public Task DeleteProduto(int id) => sqlDataAccess.SaveData("dbo.spProduto_Delete", new { Id = id });
    }
}
