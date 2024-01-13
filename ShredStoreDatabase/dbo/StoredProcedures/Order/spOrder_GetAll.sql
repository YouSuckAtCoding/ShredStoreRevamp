CREATE PROCEDURE [dbo].[spOrder_GetAll]
	
AS
Begin

	Select Id, [Date], CartId, TotalAmount, UserId from dbo.[Order]

End
