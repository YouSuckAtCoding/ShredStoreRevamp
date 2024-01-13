CREATE PROCEDURE [dbo].[spCart_Insert]
	@UserId int,
	@CreatedDate Date
AS
Begin

	INSERT INTO dbo.Cart (UserId, CreatedDate)
	VALUES (@UserId,  @CreatedDate)

End

