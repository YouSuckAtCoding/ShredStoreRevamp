CREATE PROCEDURE [dbo].[spCartItem_Update]
	@ProductId int,
	@Quantity int,
	@CartId int
AS
	
Begin

		Declare @ProductPrice as money 
		Set @ProductPrice = (Select price from Product Where Id = @ProductId);

		Update dbo.CartItem
		Set Quantity = @Quantity,
		Price = @Quantity * @ProductPrice
		Where ProductId = @ProductId 
		and CartId = @CartId;
End