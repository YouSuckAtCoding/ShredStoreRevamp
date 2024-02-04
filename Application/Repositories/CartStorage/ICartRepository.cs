using Application.Models;

namespace Application.Repositories.CartStorage
{
    public interface ICartRepository
    {
        Task DeleteCart(int UserId, CancellationToken token);
        Task<Cart?> GetCart(int UserId, CancellationToken token);
        Task InsertCart(Cart cart, CancellationToken token);

    }
}