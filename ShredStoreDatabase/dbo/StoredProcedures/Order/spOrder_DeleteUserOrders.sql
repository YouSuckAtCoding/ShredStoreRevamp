CREATE PROCEDURE [dbo].[spOrder_DeleteUserOrders]
	@UserId int
AS
Begin


	Delete O from OrderItem O INNER JOIN [Order] On [Order].UserId = @UserId and O.OrderId = [Order].Id;

	Delete from [Order] where UserId = @UserId;

End


