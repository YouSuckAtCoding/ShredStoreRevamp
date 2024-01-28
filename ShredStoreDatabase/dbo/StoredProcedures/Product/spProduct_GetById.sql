CREATE PROCEDURE [dbo].[spProduct_GetById]
	@Id int
AS
Begin

	Select Id, [Name], [Description], Price, [Type], Category, Brand, ImageName from dbo.Product
	Where Id = @Id

End
