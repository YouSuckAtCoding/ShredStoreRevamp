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
        public Task InsertCarrinho(Cart carrinho) =>
            sqlDataAccess.SaveData("dbo.spCarrinho_Insert", new { carrinho.UserId, carrinho.CreatedDate });

        public async Task<Cart?> GetCarrinho(int UserId)
        {
            var result = await sqlDataAccess.LoadData<Cart, dynamic>("dbo.spCarrinho_GetById", new { UserId });

            return result.FirstOrDefault();

        }

        public Task DeleteCarrinho(int UserId) => sqlDataAccess.SaveData("dbo.spCarrinho_Delete", new { UserId });
    }
}
