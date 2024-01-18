CREATE PROCEDURE [dbo].[spCart_GetById]
	@UserId int
AS
Begin
	
	Select UserId, CreatedDate from Cart
	Where UserId = @UserId
	
End
