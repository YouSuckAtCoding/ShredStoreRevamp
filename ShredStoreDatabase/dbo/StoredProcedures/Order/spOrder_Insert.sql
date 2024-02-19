CREATE PROCEDURE [dbo].[spOrder_Insert]
	@CreatedDate datetime,
	@UserId int, 
	@TotalAmount money,
	@PaymentId int

AS
Begin

	INSERT INTO dbo.[Order] ([CreatedDate], UserId, TotalAmount, PaymentId)
	Values (@CreatedDate, @UserId, @TotalAmount, @PaymentId)

End
