CREATE PROCEDURE [dbo].[spProduct_GetAll]
	
AS
Begin

	Select Id, [Name], [Description], Price, [Type], Category, Brand, ImageName from dbo.Product

End

