CREATE PROCEDURE [dbo].[spOrder_Insert]
	@Date date,
	@CartId int,
	@TotalAmount money,
	@UserId int

AS
Begin

	INSERT INTO dbo.[Order] ([Date], CartId, TotalAmount, UserId)
	Values (@Date, @CartId, @TotalAmount, @UserId)

End
