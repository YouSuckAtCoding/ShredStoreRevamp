CREATE PROCEDURE [dbo].[spOrderItem_Delete]
	@ProductId int,
	@OrderId int
AS
	
Begin

		Delete from dbo.[OrderItem] 
		Where ProductId = @ProductId and OrderId = @OrderId;
End