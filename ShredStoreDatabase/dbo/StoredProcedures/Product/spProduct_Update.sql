CREATE PROCEDURE [dbo].[spProduct_Update]
	@Id int,
	@Name VARCHAR(60),
	@Description varchar(750),
	@Price MONEY,
	@Type VARCHAR(30),
	@Category VARCHAR(30),
	@Brand VARCHAR(50),
	@ImageName VARCHAR(30)
AS
Begin

	Update dbo.Product
	Set [Name] = @Name,
	[Description] = @Description,
	Price = @Price,
	[Type] = @Type,
	Category = @Category,
	Brand = @Brand,
	ImageName = @ImageName
	Where Id = @Id

End
