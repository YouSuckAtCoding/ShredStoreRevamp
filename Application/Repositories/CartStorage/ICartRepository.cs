using Application.Models;

namespace Application.Repositories.CartStorage
{
    public interface ICartRepository
    {
        Task DeleteCarrinho(int UserId);
        Task<Cart?> GetCarrinho(int UserId);
        Task InsertCarrinho(Cart carrinho);

    }
}