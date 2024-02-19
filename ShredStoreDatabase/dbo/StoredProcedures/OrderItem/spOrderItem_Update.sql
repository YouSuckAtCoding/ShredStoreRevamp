CREATE PROCEDURE [dbo].[spOrderItem_Update]
	@ProductId int,
	@Quantity int,
	@OrderId int
AS
	
Begin

		Declare @ProductPrice as money 
		Set @ProductPrice = (Select price from Product Where Id = @ProductId);

		Update dbo.OrderItem
		Set Quantity = @Quantity,
		Price = @Quantity * @ProductPrice
		Where ProductId = @ProductId 
		and OrderId = @OrderId;
End
