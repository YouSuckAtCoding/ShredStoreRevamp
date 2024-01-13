CREATE PROCEDURE [dbo].[spCartItem_Delete]
	@CartId int, 
	@ProductId int
AS
Begin

	Delete from dbo.[CartItem] where CartId = @CartId and ProductId = @ProductId

End
