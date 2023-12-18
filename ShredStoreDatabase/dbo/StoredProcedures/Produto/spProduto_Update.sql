CREATE PROCEDURE [dbo].[spProduto_Update]
	@Id int,
	@Nome VARCHAR(25),
	@Descricao varchar(300),
	@Valor MONEY,
	@Tipo VARCHAR(30),
	@Categoria VARCHAR(30)
AS
Begin

	Update dbo.Produto
	Set Nome = @Nome,
	Descricao = @Descricao,
	Valor = @Valor,
	Tipo = @Tipo,
	Categoria = @Categoria

End
