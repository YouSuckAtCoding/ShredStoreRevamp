CREATE PROCEDURE [dbo].[spOrderItem_GetById]
	@OrderId int,
	@ProductId int
AS
Begin

	Select Id, OrderId, ProductId, Quantity, Price from dbo.[OrderItem]
	where OrderId = @OrderId and ProductId = @ProductId

End