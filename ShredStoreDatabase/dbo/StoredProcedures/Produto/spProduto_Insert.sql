CREATE PROCEDURE [dbo].[sp_Produto_Insert]
	@Nome VARCHAR(25),
	@Descricao varchar(300),
	@Valor MONEY,
	@Tipo VARCHAR(30),
	@Categoria VARCHAR(30)
	
AS
Begin
	
	INSERT INTO dbo.Produto (Nome, Descricao, Valor, Tipo, Categoria)
	VALUES (@Nome, @Descricao, @Valor, @Tipo, @Categoria)

End

