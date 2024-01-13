CREATE PROCEDURE [dbo].[spCartItem_GetAll]
	@CartId int
AS
Begin

	Select CartId, ProductId, prod.[Name], prod.Description, prod.Price from dbo.[CartItem]
	Join dbo.Product prod on Prod.Id = ProductId
	where CartId = @CartId

End

