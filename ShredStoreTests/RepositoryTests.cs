using Application.Repositories;
using DatabaseAccess;
using Moq;
using ShredStoreTests.Fake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit; 

namespace ShredStoreTests
{
    public class RepositoryTests
    {
        
        [Fact]
        public void InsertUsuario_ShouldWork()
        {
            var sqlMock = new Mock<ISqlDataAccess>();
            var repo = new UsuarioRepository(sqlMock.Object);
            var usuario = FakeDataFactory.FakeUsuarios();
            var result = repo.InsertUser(usuario);
            Assert.True(result.IsCompletedSuccessfully);

        }
        [Fact]
        public void GetUsuarios_ShouldWork()
        {
            var sqlMock = new Mock<ISqlDataAccess>();
            var repo = new UsuarioRepository(sqlMock.Object);
            var result = repo.GetUsuarios();
            Assert.True(result.IsCompletedSuccessfully);

        }
        [Fact]
        public void GetUsuario_ShouldWork()
        {
            var sqlMock = new Mock<ISqlDataAccess>();
            var repo = new UsuarioRepository(sqlMock.Object);
            var result = repo.GetUsuario(1);
            Assert.True(result.IsCompletedSuccessfully);

        }
        [Fact]
        public void UpdateUsuario_ShouldWork()
        {
            var sqlMock = new Mock<ISqlDataAccess>();
            var repo = new UsuarioRepository(sqlMock.Object);
            var usuario = FakeDataFactory.FakeUsuarios();
            usuario.Id = 1;
            var result = repo.UpdateUsuario(usuario);
            Assert.True(result.IsCompletedSuccessfully);

        }
        [Fact]
        public void DeleteUsuario_ShouldWork()
        {
            var sqlMock = new Mock<ISqlDataAccess>();
            var repo = new UsuarioRepository(sqlMock.Object);
            var result = repo.DeleteUsuario(1);
            Assert.True(result.IsCompletedSuccessfully);

        }

        [Fact]
        public void InsertPedido_ShouldWork()
        {
            var sqlMock = new Mock<ISqlDataAccess>();
            var repo = new PedidoRepository(sqlMock.Object);
            var pedido = FakeDataFactory.FakePedido();
            var result = repo.InsertPedido(pedido);
            Assert.True(result.IsCompletedSuccessfully);

        }
        [Fact]
        public void GetPedidos_ShouldWork()
        {
            var sqlMock = new Mock<ISqlDataAccess>();
            var repo = new PedidoRepository(sqlMock.Object);
            var result = repo.GetPedidos();
            Assert.True(result.IsCompletedSuccessfully);

        }
        [Fact]
        public void GetPedido_ShouldWork()
        {
            var sqlMock = new Mock<ISqlDataAccess>();
            var repo = new PedidoRepository(sqlMock.Object);
            var result = repo.GetPedido(1);
            Assert.True(result.IsCompletedSuccessfully);

        }
        [Fact]
        public void UpdatePedido_ShouldWork()
        {
            var sqlMock = new Mock<ISqlDataAccess>();
            var repo = new PedidoRepository(sqlMock.Object);
            var Pedido = FakeDataFactory.FakePedido();
            Pedido.Id = 1;
            var result = repo.UpdatePedido(Pedido);
            Assert.True(result.IsCompletedSuccessfully);

        }
        [Fact]
        public void DeletePedido_ShouldWork()
        {
            var sqlMock = new Mock<ISqlDataAccess>();
            var repo = new PedidoRepository(sqlMock.Object);
            var result = repo.DeletePedido(1);
            Assert.True(result.IsCompletedSuccessfully);

        }
        [Fact]
        public void InsertProduto_ShouldWork()
        {
            var sqlMock = new Mock<ISqlDataAccess>();
            var repo = new ProdutoRepository(sqlMock.Object);
            var Produto = FakeDataFactory.FakeProdutos();
            var result = repo.InsertProduto(Produto);
            Assert.True(result.IsCompletedSuccessfully);

        }
        [Fact]
        public void GetProdutos_ShouldWork()
        {
            var sqlMock = new Mock<ISqlDataAccess>();
            var repo = new ProdutoRepository(sqlMock.Object);
            var result = repo.GetProdutos();
            Assert.True(result.IsCompletedSuccessfully);

        }
        [Fact]
        public void GetProduto_ShouldWork()
        {
            var sqlMock = new Mock<ISqlDataAccess>();
            var repo = new ProdutoRepository(sqlMock.Object);
            var result = repo.GetProduto(1);
            Assert.True(result.IsCompletedSuccessfully);

        }
        [Fact]
        public void UpdateProduto_ShouldWork()
        {
            var sqlMock = new Mock<ISqlDataAccess>();
            var repo = new ProdutoRepository(sqlMock.Object);
            var Produto = FakeDataFactory.FakeProdutos();
            Produto.Id = 1;
            var result = repo.UpdateProduto(Produto);
            Assert.True(result.IsCompletedSuccessfully);

        }
        [Fact]
        public void DeleteProduto_ShouldWork()
        {
            var sqlMock = new Mock<ISqlDataAccess>();
            var repo = new ProdutoRepository(sqlMock.Object);
            var result = repo.DeleteProduto(1);
            Assert.True(result.IsCompletedSuccessfully);

        }

        [Fact]
        public void InsertCarrinho_ShouldWork()
        {
            var sqlMock = new Mock<ISqlDataAccess>();
            var repo = new CarrinhoRepository(sqlMock.Object);
            var Carrinho = FakeDataFactory.FakeCarrinho();
            var result = repo.InsertCarrinho(Carrinho);
            Assert.True(result.IsCompletedSuccessfully);

        }

        [Fact]
        public void GetCarrinho_ShouldWork()
        {
            var sqlMock = new Mock<ISqlDataAccess>();
            var repo = new CarrinhoRepository(sqlMock.Object);
            var result = repo.GetCarrinho(1);
            Assert.True(result.IsCompletedSuccessfully);

        }
        [Fact]
        public void UpdateCarrinho_ShouldWork()
        {
            var sqlMock = new Mock<ISqlDataAccess>();
            var repo = new CarrinhoRepository(sqlMock.Object);
            var Carrinho = FakeDataFactory.FakeCarrinho();
            Carrinho.UsuarioId = 1;
            var result = repo.UpdateCarrinho(Carrinho);
            Assert.True(result.IsCompletedSuccessfully);

        }
        [Fact]
        public void DeleteCarrinho_ShouldWork()
        {
            var sqlMock = new Mock<ISqlDataAccess>();
            var repo = new CarrinhoRepository(sqlMock.Object);
            var result = repo.DeleteCarrinho(1);
            Assert.True(result.IsCompletedSuccessfully);

        }

    }
}
