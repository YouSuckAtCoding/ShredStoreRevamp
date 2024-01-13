CREATE PROCEDURE [dbo].[spCart_GetById]
	@UserId int
AS
Begin
	
	Select UserId, SUM(prod.Price) as TotalAmount, CreatedDate from Cart
	JOIN dbo.CartItem p On UserId = p.CartId
	JOIN dbo.Product prod On prod.Id = p.ProductId
	Where UserId = @UserId
	Group By UserId, CreatedDate
	
End
