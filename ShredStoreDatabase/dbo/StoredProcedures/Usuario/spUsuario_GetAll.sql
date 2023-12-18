CREATE PROCEDURE [dbo].[spUsuario_GetAll]
	
AS
Begin
	Select Id, Nome, Idade, Email, Cpf, Endereco from dbo.Usuario
End
