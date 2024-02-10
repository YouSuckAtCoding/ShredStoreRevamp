﻿CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Name] VARCHAR(20) NOT NULL,
	[Age] INT NOT NULL,
	[Email] varchar(50) NOT NULL,
	[Cpf] VARCHAR(11) NOT NULL,
	[Address] VARCHAR(50) NOT NULL,
	[Role] VARCHAR(50) NOT NULL,
	[Password] BINARY(64) NOT NULL, 
    [Salt] UNIQUEIDENTIFIER NOT NULL
)