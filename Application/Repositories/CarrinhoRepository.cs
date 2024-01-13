using Application.Models;
using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public class CarrinhoRepository : ICarrinhoRepository
    {
        private readonly ISqlDataAccess sqlDataAccess;

        public CarrinhoRepository(ISqlDataAccess _sqlDataAccess)
        {
            sqlDataAccess = _sqlDataAccess;
        }
        public Task InsertCarrinho(Cart carrinho) =>
            sqlDataAccess.SaveData("dbo.spCarrinho_Insert", new { carrinho.UserId, carrinho.CreatedDate});

        public async Task<Cart?> GetCarrinho(int id)
        {
            var result = await sqlDataAccess.LoadData<Cart, dynamic>("dbo.spCarrinho_GetById", new { Id = id });

            return result.FirstOrDefault();

        }

        public Task DeleteCarrinho(int id) => sqlDataAccess.SaveData("dbo.spCarrinho_Delete", new { Id = id });
    }
}
