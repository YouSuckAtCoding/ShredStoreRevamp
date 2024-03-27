CREATE PROCEDURE [dbo].[spProduct_Insert]
	@Name VARCHAR(60),
	@Description varchar(750),
	@Price MONEY,
	@Type VARCHAR(30),
	@Category VARCHAR(30),
	@Brand VARCHAR(50),
	@ImageName VARCHAR(30),
	@UserId int
	
AS
Begin
	
	INSERT INTO dbo.Product ([Name], [Description], Price, [Type], Category, Brand, ImageName, UserId)
	VALUES (@Name, @Description, @Price, @Type, @Category, @Brand, @ImageName, @UserId)

End

