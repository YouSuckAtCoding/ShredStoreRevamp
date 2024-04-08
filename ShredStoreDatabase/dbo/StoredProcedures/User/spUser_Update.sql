CREATE PROCEDURE [dbo].[spUser_Update]
	@Id int,
	@Name VARCHAR(20),
	@Address VARCHAR(50),
	@Role varchar(10)
AS
Begin

	Update dbo.[User]
	Set [Name] = @Name, 
	[Address] = @Address,
	Role = @Role
	Where Id = @Id

End

