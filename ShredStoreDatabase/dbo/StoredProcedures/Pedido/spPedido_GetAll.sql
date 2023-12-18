CREATE PROCEDURE [dbo].[spPedido_GetAll]
	
AS
Begin

	Select Id, [Data], CarrinhoId, Valor, UsuarioId from dbo.Pedido

End
