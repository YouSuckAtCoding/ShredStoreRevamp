CREATE PROCEDURE [dbo].[spCartItem_GetAll]
	@CartId int
AS
Begin

	Select CartId, ProductId, Quantity, Price from dbo.[CartItem]
	where CartId = @CartId

End

