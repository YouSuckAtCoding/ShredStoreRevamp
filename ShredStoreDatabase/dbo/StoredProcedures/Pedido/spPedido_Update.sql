CREATE PROCEDURE [dbo].[spPedido_Update]
	@Id int,
	@Data date,
	@Valor money	
AS
Begin
	
	Update dbo.Pedido
	Set Data = @Data,
	Valor = @Valor
	Where Id = @Id

End
