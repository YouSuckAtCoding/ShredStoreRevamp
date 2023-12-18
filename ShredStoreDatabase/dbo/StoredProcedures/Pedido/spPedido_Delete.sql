CREATE PROCEDURE [dbo].[spPedido_Delete]
	@Id int
AS
Begin
	
	Delete from dbo.Pedido where Id = @Id

End

