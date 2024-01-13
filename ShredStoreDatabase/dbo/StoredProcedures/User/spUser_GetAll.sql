CREATE PROCEDURE [dbo].[spUser_GetAll]
	
AS
Begin
	Select Id, [Name], Age, Email, Cpf, Address from dbo.[User]
End
