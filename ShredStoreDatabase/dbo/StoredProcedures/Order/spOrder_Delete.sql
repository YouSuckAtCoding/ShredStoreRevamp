CREATE PROCEDURE [dbo].[spOrder_Delete]
	@Id int
AS
Begin
	
	Delete from dbo.[Order] where Id = @Id

End

