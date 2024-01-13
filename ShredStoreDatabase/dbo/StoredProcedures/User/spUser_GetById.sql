CREATE PROCEDURE [dbo].[spUser_GetById]
	@Id int
AS
Begin
	Select Id, [Name], Age, Email, Cpf, Address from dbo.[User]
	Where Id = @Id
End
