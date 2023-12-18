using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Bogus;
using Bogus.Extensions.Brazil;

namespace ShredStoreTests.Fake
{
    public class FakeDataFactory
    {
        public static Usuario FakeUsuarios()
        {
            var userFaker = new Faker<Usuario>("pt_BR")
            .RuleFor(x => x.Nome, f => f.Name.FullName(Bogus.DataSets.Name.Gender.Male))
            .RuleFor(x => x.Email, f => f.Internet.Email(f.Person.FirstName).ToLower())
            .RuleFor(x => x.Cpf, f => f.Person.Cpf())
            .RuleFor(x => x.Endereco, f => f.Address.StreetAddress())
            .RuleFor(x => x.Idade, f => f.Random.Number(25))
            .RuleFor(x => x.Password, f => f.Random.AlphaNumeric(25));

            return userFaker;
        }

        public static Produto FakeProdutos()
        {
            var produtoFaker = new Faker<Produto>("pt_BR")
            .RuleFor(x => x.Nome, f => f.Random.String())
            .RuleFor(x => x.Valor, f => f.Random.Number(100, 2000))
            .RuleFor(x => x.Tipo, f => f.Random.String())
            .RuleFor(x => x.Categoria, f => f.Random.String());

            return produtoFaker;
        }

        public static Pedido FakePedido() 
        {
            Pedido pedido = new Pedido
            {
                Data = DateTime.Now,
                UsuarioId = 1,
                CarrinhoId = 1,
                Valor = 2000
            };

            return pedido;
            
        }

        public static Carrinho FakeCarrinho()
        {
            Carrinho carrinho = new Carrinho
            {
                DataCriacao = DateTime.Now,
                UsuarioId = 1,
                ProdutoId = 1,
                ValorTotal = 2000
            };

            return carrinho;
            
        }
    }
}
