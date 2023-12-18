CREATE PROCEDURE [dbo].[spUsuario_GetById]
	@Id int
AS
Begin
	Select Id, Nome, Idade, Email, Cpf, Endereco from dbo.Usuario
	Where Id = @Id
End
