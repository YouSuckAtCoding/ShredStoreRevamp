CREATE PROCEDURE [dbo].[spCartItem_Insert]
	@CartId int, 
	@ProductId int,
	@Quantity int,
	@Price money
AS
Begin

	Insert into dbo.[CartItem] (CartId, ProductId, Quantity, Price)
	Values (@CartId, @ProductId, @Quantity, @Price)

End