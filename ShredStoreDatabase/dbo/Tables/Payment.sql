CREATE TABLE [dbo].[Payment]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY,
	[Amount] money NOT NULL,
	[Date] datetime NOT NULL,
	[PaymentType] tinyint NOT NULL,
	[Payed] BIT NOT NULL
)
