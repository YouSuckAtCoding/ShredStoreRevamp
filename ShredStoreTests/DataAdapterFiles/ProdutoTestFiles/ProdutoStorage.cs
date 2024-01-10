using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShredStoreTests.DataAdapterFiles.ProdutoTestFiles
{
    public class ProdutoStorage : IProdutoStorage
    {
        private TestSqlDataAccess _dataAccess;

        public ProdutoStorage(ISqlAccessConnectionFactory dbConnectionFactory)
        {
            ArgumentNullException.ThrowIfNull(dbConnectionFactory);
            _dataAccess = new TestSqlDataAccess(dbConnectionFactory);
        }

        public Task InsertProduto(Produto produto) =>
           _dataAccess.SaveData("dbo.spProduto_Insert", new { produto.Nome, produto.Descricao, produto.Valor, produto.Tipo, produto.Categoria });

        public Task<IEnumerable<Produto>> GetProdutos() => _dataAccess.LoadData<Produto, dynamic>("dbo.spProduto_GetAll", new { });

        public async Task<Produto?> GetProduto(int id)
        {
            var result = await _dataAccess.LoadData<Produto, dynamic>("dbo.spProduto_GetById", new { Id = id });

            return result.FirstOrDefault();

        }
        public Task UpdateProduto(Produto produto) => _dataAccess.SaveData("dbo.spProduto_Update", new { produto.Id, produto.Nome, produto.Descricao, produto.Valor, produto.Tipo, produto.Categoria });

        public Task DeleteProduto(int id) => _dataAccess.SaveData("dbo.spProduto_Delete", new { Id = id });


    }
}
