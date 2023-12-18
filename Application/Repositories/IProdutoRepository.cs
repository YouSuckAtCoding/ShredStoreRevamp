using Application.Models;

namespace Application.Repositories
{
    public interface IProdutoRepository
    {
        Task DeleteProduto(int id);
        Task<Produto?> GetProduto(int id);
        Task<IEnumerable<Produto>> GetProdutos();
        Task InsertProduto(Produto produto);
        Task UpdateProduto(Produto produto);
    }
}