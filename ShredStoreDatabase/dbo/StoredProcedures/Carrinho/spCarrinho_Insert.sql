CREATE PROCEDURE [dbo].[spCarrinho_Insert]
	@UsuarioId int,
	@ProdutoId int,
	@DataCriacao Date
AS
Begin

	INSERT INTO dbo.Carrinho (UsuarioId, ProdutoId, DataCriacao)
	VALUES (@UsuarioId, @ProdutoId, @DataCriacao)

End
