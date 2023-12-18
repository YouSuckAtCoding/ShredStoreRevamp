CREATE PROCEDURE [dbo].[spPedido_GetById]
	@Id int
AS
Begin

	Select Id, [Data], CarrinhoId, Valor, UsuarioId from dbo.Pedido
	Where Id = @Id

End
