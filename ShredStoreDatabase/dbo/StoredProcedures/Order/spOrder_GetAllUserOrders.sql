CREATE PROCEDURE [dbo].[spOrder_GetAllUserOrders]
	@UserId int
AS
Begin

	Select Id, [CreatedDate], TotalAmount, UserId from dbo.[Order]
	Where UserId = @UserId

End
