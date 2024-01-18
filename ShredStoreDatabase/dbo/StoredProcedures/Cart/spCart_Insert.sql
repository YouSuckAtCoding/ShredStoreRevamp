CREATE PROCEDURE [dbo].[spCart_Insert]
	@UserId int
AS
Begin

	INSERT INTO dbo.Cart (UserId, CreatedDate)
	VALUES (@UserId,  GETDATE())

End

