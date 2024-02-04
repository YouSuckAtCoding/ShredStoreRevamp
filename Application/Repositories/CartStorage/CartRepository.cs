using Application.Models;
using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.CartStorage
{
    public class CartRepository : ICartRepository
    {
        private readonly ISqlDataAccess sqlDataAccess;

        public CartRepository(ISqlDataAccess _sqlDataAccess)
        {
            sqlDataAccess = _sqlDataAccess;
        }
        public Task InsertCart(Cart cart, CancellationToken token) =>
            sqlDataAccess.SaveData("dbo.spCart_Insert", new { cart.UserId, cart.CreatedDate });

        public async Task<Cart?> GetCart(int UserId, CancellationToken token)
        {
            var result = await sqlDataAccess.LoadData<Cart, dynamic>("dbo.spCart_GetById", new { UserId });

            return result.FirstOrDefault();

        }
        public Task DeleteCart(int UserId, CancellationToken token) => sqlDataAccess.SaveData("dbo.spCart_Delete", new { UserId });
    }
}
