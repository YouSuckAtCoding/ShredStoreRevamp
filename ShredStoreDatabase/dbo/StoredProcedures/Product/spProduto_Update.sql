CREATE PROCEDURE [dbo].[spProduct_Update]
	@Id int,
	@Name VARCHAR(25),
	@Description varchar(300),
	@Price MONEY,
	@Type VARCHAR(30),
	@Category VARCHAR(30)
AS
Begin

	Update dbo.Product
	Set [Name] = @Name,
	[Description] = @Description,
	Price = @Price,
	[Type] = @Type,
	Category = @Category

End
