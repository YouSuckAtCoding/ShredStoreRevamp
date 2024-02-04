using Application.Models;

namespace Application.Services.CartServices
{
    public interface ICartService
    {
        Task<bool> Create(Cart cart, CancellationToken token);
        Task DeleteCart(int id, CancellationToken token);
        Task<Cart?> GetCart(int id, CancellationToken token);
    }
}