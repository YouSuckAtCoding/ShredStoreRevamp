CREATE PROCEDURE [dbo].[spCartItem_Delete]
	@ProductId int,
	@CartId int
AS
	
Begin

		Delete from dbo.[CartItem] 
		Where ProductId = @ProductId and CartId = @CartId;
End