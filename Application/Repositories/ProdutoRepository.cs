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
        public Task InsertProduto(Produto produto) =>
            sqlDataAccess.SaveData("dbo.spProduto_Insert", new { produto.Nome, produto.Descricao, produto.Valor, produto.Tipo, produto.Categoria });

        public Task<IEnumerable<Produto>> GetProdutos() => sqlDataAccess.LoadData<Produto, dynamic>("dbo.spProduto_GetAll", new { });

        public async Task<Produto?> GetProduto(int id)
        {
            var result = await sqlDataAccess.LoadData<Produto, dynamic>("dbo.spProduto_GetById", new { Id = id });

            return result.FirstOrDefault();

        }
        public Task UpdateProduto(Produto produto) => sqlDataAccess.SaveData("dbo.spProduto_Update", new { produto.Id, produto.Nome, produto.Descricao, produto.Valor, produto.Tipo, produto.Categoria });

        public Task DeleteProduto(int id) => sqlDataAccess.SaveData("dbo.spProduto_Delete", new { Id = id });
    }
}
