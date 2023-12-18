CREATE PROCEDURE [dbo].[spUsuario_Update]
	@Id int,
	@Nome VARCHAR(20),
	@Idade INT,
	@Email VARCHAR(50),
	@Cpf VARCHAR(11),
	@Endereco VARCHAR(50)
AS
Begin

	Update dbo.Usuario 
	Set Nome = @Nome, 
	Idade = @Idade,
	Email = @Email,
	Cpf = @Cpf,
	Endereco = @Endereco
	Where Id = @Id

End

