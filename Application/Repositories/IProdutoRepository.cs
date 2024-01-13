using Application.Models;

namespace Application.Repositories
{
    public interface IProdutoRepository
    {
        Task DeleteProduto(int id);
        Task<Product?> GetProduto(int id);
        Task<IEnumerable<Product>> GetProdutos();
        Task InsertProduto(Product produto);
        Task UpdateProduto(Product produto);
    }
}