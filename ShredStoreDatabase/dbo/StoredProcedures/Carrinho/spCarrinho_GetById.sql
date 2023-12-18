CREATE PROCEDURE [dbo].[spCarrinho_GetById]
	@UsuarioId int
AS
Begin
	
	Select UsuarioId, ProdutoId, SUM(p.Valor) as Total, DataCriacao from Carrinho
	JOIN dbo.Produto p On ProdutoId = p.Id
	Where UsuarioId = @UsuarioId
	Group By UsuarioId, ProdutoId, DataCriacao
	
End
