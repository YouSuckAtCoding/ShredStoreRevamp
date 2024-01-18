CREATE PROCEDURE [dbo].[spOrder_Insert]
	@Date datetime,
	@CartId int,
	@UserId int

AS
Begin

	Declare @TotalAmount money
	SET @TotalAmount = (Select SUM(Price) from dbo.CartItem where CartId = @CartId);
	
	INSERT INTO dbo.[Order] ([Date], CartId, TotalAmount, UserId)
	Values (@Date, @CartId, @TotalAmount, @UserId)

End
