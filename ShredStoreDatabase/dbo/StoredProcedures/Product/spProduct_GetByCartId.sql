CREATE PROCEDURE [dbo].[spProduct_GetByCartId]
	@CartId int
AS
Begin

	Select Product.Id, Product.[Name], Product.Price, Product.ImageName, CartItem.Quantity, CartItem.Price
	from Product Join CartItem on Product.Id = CartItem.ProductId and CartItem.CartId = @CartId

End

