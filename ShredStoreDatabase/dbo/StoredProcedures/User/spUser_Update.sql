CREATE PROCEDURE [dbo].[spUser_Update]
	@Id int,
	@Name VARCHAR(20),
	@Age INT,
	@Email VARCHAR(50),
	@Cpf VARCHAR(11),
	@Address VARCHAR(50)
AS
Begin

	Update dbo.[User]
	Set [Name] = @Name, 
	Age = @Age,
	Email = @Email,
	Cpf = @Cpf,
	Address = @Address
	Where Id = @Id

End

