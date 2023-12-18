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
        public Task InsertCarrinho(Carrinho carrinho) =>
            sqlDataAccess.SaveData("dbo.spCarrinho_Insert", new { carrinho.UsuarioId, carrinho.DataCriacao, carrinho.ProdutoId });

        public async Task<Carrinho?> GetCarrinho(int id)
        {
            var result = await sqlDataAccess.LoadData<Carrinho, dynamic>("dbo.spCarrinho_GetById", new { Id = id });

            return result.FirstOrDefault();

        }
        public Task UpdateCarrinho(Carrinho Carrinho) => sqlDataAccess.SaveData("dbo.spCarrinho_Update", new { Carrinho.UsuarioId, Carrinho.ProdutoId });

        public Task DeleteCarrinho(int id) => sqlDataAccess.SaveData("dbo.spCarrinho_Delete", new { Id = id });
    }
}
