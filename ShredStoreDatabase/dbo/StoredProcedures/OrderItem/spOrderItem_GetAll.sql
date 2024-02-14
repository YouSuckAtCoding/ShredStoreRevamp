CREATE PROCEDURE [dbo].[spOrderItem_GetAll]
	@OrderId int
AS

Begin
	
	Select * from dbo.OrderItem Where OrderId = @OrderId;
End
