CREATE TABLE [dbo].[Cart]
(
	[UserId] INT NOT NULL FOREIGN KEY REFERENCES [User](Id) PRIMARY KEY,
	[TotalAmount] MONEY,
	[CreatedDate] DATE NOT NULL

)
