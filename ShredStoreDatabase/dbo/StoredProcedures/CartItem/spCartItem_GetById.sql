CREATE PROCEDURE [dbo].[spCartItem_GetById]
	@CartId int,
	@ProductId int
AS
Begin

	Select CartId, ProductId, Quantity, Price from dbo.[CartItem]
	where CartId = @CartId and ProductId = @ProductId

End