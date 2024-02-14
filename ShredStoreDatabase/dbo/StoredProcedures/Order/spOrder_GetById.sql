CREATE PROCEDURE [dbo].[spOrder_GetById]
	@Id int
AS
Begin

	Select Id, [CreatedDate], UserId, TotalAmount from dbo.[Order]
	Where Id = @Id

End
