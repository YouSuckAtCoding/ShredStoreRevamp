CREATE PROCEDURE [dbo].[spPedido_Insert]
	@Data date,
	@CarrinhoId int,
	@Valor money,
	@UsuarioId int

AS
Begin

	INSERT INTO dbo.Pedido (Data, CarrinhoId, Valor, UsuarioId)
	Values (@Data, @CarrinhoId, @Valor, @UsuarioId)

End
