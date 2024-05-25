CREATE PROCEDURE [dbo].[spUser_Update]
	@Id int,
	@Name VARCHAR(20),
	@Address VARCHAR(50),
	@Role varchar(10)
AS
Begin

	Update dbo.[User]
	Set [Name] = ISNULL(@Name, [Name]), 
	[Address] = ISNULL(@Address, [Address]),
	[Role] = ISNULL(@Role, [Role])
	Where Id = @Id

End

