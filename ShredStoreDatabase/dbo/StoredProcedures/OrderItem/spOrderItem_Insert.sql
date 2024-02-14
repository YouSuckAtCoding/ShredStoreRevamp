CREATE PROCEDURE [dbo].[spOrderItem_Insert]
	@OrderId int, 
	@ProductId int,
	@Quantity int	
AS
	
	If(( Select count(*) from [OrderItems] where ProductId = @ProductId) = 1)

	Begin
		Exec spOrderItem_Update @ProductId,@Quantity, @OrderId
	End;
	Else
		Begin
		Declare @ProductPrice as money 
			Set @ProductPrice = (Select price from Product Where Id = @ProductId);


		Insert into dbo.[OrderItem] (OrderId, ProductId, Quantity, Price)
		Values (@OrderId, @ProductId, @Quantity, @Quantity * @ProductPrice);
		End
	