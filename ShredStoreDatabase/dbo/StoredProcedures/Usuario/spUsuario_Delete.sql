CREATE PROCEDURE [dbo].[spUsuario_Delete]
	@Id int
AS
Begin
	
	Delete from dbo.Usuario where Id = @Id
	
End	
