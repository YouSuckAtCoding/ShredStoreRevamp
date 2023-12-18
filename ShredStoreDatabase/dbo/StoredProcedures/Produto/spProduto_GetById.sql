CREATE PROCEDURE [dbo].[spProduto_GetById]
	@Id int
AS
Begin

	Select Id, Nome, Descricao, Valor, Tipo, Categoria from dbo.Produto
	Where Id = @Id

End
