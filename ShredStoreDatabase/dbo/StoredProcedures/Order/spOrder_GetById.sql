CREATE PROCEDURE [dbo].[spOrder_GetById]
	@Id int
AS
Begin

	Select Id, [Date], CartId, TotalAmount, UserId from dbo.[Order]
	Where Id = @Id

End
