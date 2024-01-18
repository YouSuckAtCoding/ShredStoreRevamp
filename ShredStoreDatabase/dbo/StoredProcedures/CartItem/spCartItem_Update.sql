CREATE PROCEDURE [dbo].[spCartItem_Update]
	@ProductId int,
	@Quantity int,
	@CartId int
AS
	
Begin
		Update dbo.CartItem
		Set Quantity = @Quantity
		Where ProductId = @ProductId 
		and CartId = @CartId;
End