CREATE PROCEDURE [dbo].[spCartItem_Insert]
	@CartId int, 
	@ProductId int,
	@Quantity int	
AS

	SET IDENTITY_INSERT dbo.[CartItem] ON
	
	If(( Select count(*) from CartItem where ProductId = @ProductId) = 1)

	Begin
		Exec spCartItem_Update @ProductId,@Quantity, @CartId
	End;
	Else
		Begin
		Declare @ProductPrice as money 
			Set @ProductPrice = (Select price from Product Where Id = @ProductId);


		Insert into dbo.[CartItem] (CartId, ProductId, Quantity, Price)
		Values (@CartId, @ProductId, @Quantity, @Quantity * @ProductPrice);
		End
	SET IDENTITY_INSERT dbo.[CartItem] OFF
	
		
