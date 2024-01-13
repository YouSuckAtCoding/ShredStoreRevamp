CREATE TABLE [dbo].[CartItem]
(
	
    [CartId] INT NOT NULL FOREIGN KEY REFERENCES Cart(UserId),
    [ProductId] INT NOT NULL FOREIGN KEY REFERENCES Product(Id)

)
