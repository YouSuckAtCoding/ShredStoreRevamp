CREATE PROCEDURE [dbo].[spOrder_GetAllUserOrders]
	@UserId int
AS
Begin

	Select Id, [Date], CartId, TotalAmount, UserId from dbo.[Order]
	Where UserId = @UserId

End
