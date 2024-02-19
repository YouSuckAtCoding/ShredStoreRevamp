CREATE PROCEDURE [dbo].[spUser_Insert]
	@Name VARCHAR(20),
	@Age INT,
	@Email VARCHAR(50),
	@Cpf VARCHAR(11),
	@Address VARCHAR(50),
	@Password nvarchar(50)

AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Salt UNIQUEIDENTIFIER=NEWID()

	INSERT INTO dbo.[User] ([Name], Age, Email, Cpf, Address, [Password], Salt, [Role])
	VALUES (@Name, @Age, @Email, @Cpf, @Address,HASHBYTES('SHA2_512', @Password+CAST(@Salt as NVARCHAR(36))), @Salt, 'User')
END
