using Application.Models;

namespace Application.Repositories
{
    public interface ICarrinhoRepository
    {
        Task DeleteCarrinho(int id);
        Task<Cart?> GetCarrinho(int id);
        Task InsertCarrinho(Cart carrinho);

    }
}