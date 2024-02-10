﻿CREATE PROCEDURE [dbo].[spProduct_Insert]
	@Name VARCHAR(25),
	@Description varchar(300),
	@Price MONEY,
	@Type VARCHAR(30),
	@Category VARCHAR(30),
	@Brand VARCHAR(50),
	@ImageName VARCHAR(30)
	
AS
Begin
	
	INSERT INTO dbo.Product ([Name], [Description], Price, [Type], Category, Brand, ImageName)
	VALUES (@Name, @Description, @Price, @Type, @Category, @Brand, @ImageName)

End
