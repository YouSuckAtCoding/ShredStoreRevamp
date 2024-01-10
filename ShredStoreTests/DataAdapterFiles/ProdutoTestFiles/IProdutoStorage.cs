using Application.Models;

namespace ShredStoreTests.DataAdapterFiles.ProdutoTestFiles
{
    public interface IProdutoStorage
    {
        Task DeleteProduto(int id);
        Task<Produto?> GetProduto(int id);
        Task<IEnumerable<Produto>> GetProdutos();
        Task InsertProduto(Produto produto);
        Task UpdateProduto(Produto produto);
    }
}