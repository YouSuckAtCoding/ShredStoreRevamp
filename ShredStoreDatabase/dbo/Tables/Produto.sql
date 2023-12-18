﻿CREATE TABLE [dbo].[Produto]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Nome] VARCHAR(25) NOT NULL,
	[Descricao] VARCHAR(300) NOT NULL,
	[Valor] MONEY NOT NULL,
	[Tipo] VARCHAR(30) NOT NULL,
	[Categoria] VARCHAR(30) NOT NULL
)
