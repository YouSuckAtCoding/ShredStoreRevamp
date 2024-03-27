CREATE PROCEDURE [dbo].[spCart_Insert]
	@UserId int,
	@CreatedDate datetime
AS
Begin

	INSERT INTO dbo.Cart (UserId, CreatedDate)
	VALUES (@UserId,  @CreatedDate)

End

