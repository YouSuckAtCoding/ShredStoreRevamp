CREATE PROCEDURE [dbo].[spCarrinho_Delete]
	@Id int
AS
Begin

	Delete from dbo.Carrinho where UsuarioId = @Id

End
