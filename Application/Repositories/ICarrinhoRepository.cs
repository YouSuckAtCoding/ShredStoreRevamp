using Application.Models;

namespace Application.Repositories
{
    public interface ICarrinhoRepository
    {
        Task DeleteCarrinho(int id);
        Task<Carrinho?> GetCarrinho(int id);
        Task InsertCarrinho(Carrinho carrinho);
        Task UpdateCarrinho(Carrinho Carrinho);
    }
}