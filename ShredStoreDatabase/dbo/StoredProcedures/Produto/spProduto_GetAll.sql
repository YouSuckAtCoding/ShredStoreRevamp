CREATE PROCEDURE [dbo].[spProduto_GetAll]
	
AS
Begin

	Select Id, Nome, Descricao, Valor, Tipo, Categoria from dbo.Produto

End

