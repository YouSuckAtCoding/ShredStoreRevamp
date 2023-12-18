CREATE PROCEDURE [dbo].[spCarrinho_Update]
	@UsuarioId int,
	@ProdutoId int
AS
Begin

	Update dbo.Carrinho 
	Set ProdutoId = @ProdutoId
	Where UsuarioId = @UsuarioId

End
