using Application.Models;

namespace ShredStoreTests.DataAdapterFiles.CartTestFiles
{
    public interface ICartStorage
    {
        Task DeleteCart(int id);
        Task<Cart?> GetCart(int id);
        Task InsertCart(Cart Cart);
    }
}