CREATE PROCEDURE [dbo].[spOrderItem_DeleteAll]
	@OrderId int
AS
	
Begin
	
	Delete from dbo.[OrderItem] 
	Where OrderId = @OrderId;

End
