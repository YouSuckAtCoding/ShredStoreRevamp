using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShredStoreTests.DataAdapterFiles
{
    public class DatabaseInitializer
    {
        private ISqlAccessConnectionFactory _connectionFactory;

        public DatabaseInitializer(ISqlAccessConnectionFactory sqlAccessConnectionFactory)
        {

            _connectionFactory = sqlAccessConnectionFactory;
        }

        public async Task InitializeAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync(default);
            
                await connection.ExecuteAsync(
                @"CREATE TABLE [User] (
                    Id INT NOT NULL PRIMARY KEY IDENTITY,
	                Name VARCHAR(20) NOT NULL,
	                Age INT NOT NULL,
	                Email varchar(50) NOT NULL,
	                Cpf VARCHAR(11) NOT NULL,
	                Address VARCHAR(50) NOT NULL,
	                Password BINARY(64) NOT NULL, 
                    Salt UNIQUEIDENTIFIER NOT NULL
                );"
                );
            await connection.ExecuteAsync(
                @"CREATE TABLE [dbo].[Product]
                (
                	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
                	[Name] VARCHAR(25) NOT NULL,
                	[Description] VARCHAR(300) NOT NULL,
                	[Price] MONEY NOT NULL,
                	[Type] VARCHAR(30) NOT NULL,
                	[Category] VARCHAR(30) NOT NULL
                )
                ");

            await connection.ExecuteAsync(
               @"CREATE TABLE [dbo].[Cart]
                (
                	[UserId] INT NOT NULL FOREIGN KEY REFERENCES [User](Id) PRIMARY KEY,
                	[TotalAmount] MONEY,
                	[CreatedDate] DATE NOT NULL
                
                )"
                );
            await connection.ExecuteAsync(@"
                CREATE TABLE [dbo].[ItemCart]
                (
                	
                    [CartId] INT NOT NULL FOREIGN KEY REFERENCES Cart(UserId),
                    [ProductId] INT NOT NULL FOREIGN KEY REFERENCES Product(Id)
                
                )"
                );

                await CreateUserStoredProcedures(connection);
                await CreateProductStoredProcedures(connection);
                await CreateCartStoredProcedures(connection);



        }

        private async Task CreateUserStoredProcedures(IDbConnection connection)
        {
            await connection.ExecuteAsync(
                @"CREATE PROCEDURE [dbo].[spUser_Insert]
            	@Name VARCHAR(20),
            	@Age INT,
            	@EMAIL VARCHAR(50),
            	@CPF VARCHAR(11),
            	@Address VARCHAR(50),
            	@Password nvarchar(50)
            
                AS
                BEGIN
            	SET NOCOUNT ON
            	DECLARE @Salt UNIQUEIDENTIFIER=NEWID()
            
            	INSERT INTO dbo.[User] (Name, Age, Email, Cpf, Address, [Password], Salt)
            	VALUES (@Name, @Age, @EMAIL, @CPF, @Address,HASHBYTES('SHA2_512', @Password+CAST(@Salt as NVARCHAR(36))), @Salt)
                END"
                );

            await connection.ExecuteAsync(
                @"CREATE PROCEDURE [dbo].[spUser_GetAll]
                AS
                Begin
                    Select Id, Name, Age, Email, Cpf, Address from dbo.[User]
                End
                ");

            await connection.ExecuteAsync(
                @"CREATE PROCEDURE [dbo].[spUser_GetById]
                @Id int
                AS
                Begin
                	Select Id, Name, Age, Email, Cpf, Address from dbo.[User]
                	Where Id = @Id
                End");

            await connection.ExecuteAsync(
                @"CREATE PROCEDURE [dbo].[spUser_Delete]
                @Id int
                AS
                Begin
                	Delete from dbo.[User] where Id = @Id
                End	");

            await connection.ExecuteAsync(
                @"CREATE PROCEDURE [dbo].[spUser_Update]
            	@Id int,
            	@Name VARCHAR(20),
            	@Age INT,
            	@Email VARCHAR(50),
            	@Cpf VARCHAR(11),
            	@Address VARCHAR(50)
                AS
                Begin
            
            	Update dbo.[User]
            	Set Name = @Name, 
            	Age = @Age,
            	Email = @Email,
            	Cpf = @Cpf,
            	Address = @Address
            	Where Id = @Id
            
                End");
            await connection.ExecuteAsync(
                @"CREATE PROCEDURE [dbo].[spUser_Login]
            	@Name nvarchar(50),
            	@Password nvarchar(50),
            	@ResponseMessage nvarchar(250)='' Output
            
                AS
                Begin
            	
            	SET NOCOUNT ON
            
            	Declare @SelectedId int
            
            	IF Exists (Select Top 1 Id From dbo.[User] Where Name = @Name)
            	Begin
            		Set @SelectedId=(Select Id from dbo.[User] Where Name = @Name And Password=HASHBYTES('SHA2_512',@Password+Cast(Salt As nvarchar(36))))
            
            		If(@SelectedId Is Null)
            			Set @ResponseMessage='Incorret Password'
            		Else
            			Set @ResponseMessage='Login Successfull'
            			exec dbo.spUser_GetById @SelectedId
            			
            	End
            	Else
            		Set @ResponseMessage='Invalid Login Attempt'
            
                End");
        }

        private async Task CreateProductStoredProcedures(IDbConnection connection)
        {
            await connection.ExecuteAsync(
                @"CREATE PROCEDURE [dbo].[spProduct_Insert]
            	@Name VARCHAR(25),
            	@Description varchar(300),
            	@Price MONEY,
            	@Type VARCHAR(30),
            	@Category VARCHAR(30)
            	
                AS
                Begin
            	
            	INSERT INTO dbo.Product (Name, Description, Price, Type, Category)
            	VALUES (@Name, @Description, @Price, @Type, @Category)
            
                End
                "
                );

            await connection.ExecuteAsync(
                @"CREATE PROCEDURE [dbo].[spProduct_GetById]
                @Id int
                AS
                Begin
                
                	Select Id, Name, Description, Price, Type, Category from dbo.Product
                	Where Id = @Id
                
                End");

            await connection.ExecuteAsync(
                @"CREATE PROCEDURE [dbo].[spProduct_GetAll]
	
                AS
                Begin
                
                	Select Id, Name, Description, Price, Type, Category from dbo.Product
                
                End"
                );
            await connection.ExecuteAsync(
                @"CREATE PROCEDURE [dbo].[spProduct_Update]
            	@Id int,
            	@Name VARCHAR(25),
            	@Description varchar(300),
            	@Price MONEY,
            	@Type VARCHAR(30),
            	@Category VARCHAR(30)
                AS
                Begin
            
            	Update dbo.Product
            	Set Name = @Name,
            	Description = @Description,
            	Price = @Price,
            	Type = @Type,
            	Category = @Category
            
                End
                ");
            await connection.ExecuteAsync(
                @"CREATE PROCEDURE [dbo].[spProduct_Delete]
                @Id int
                AS
                Begin
                
                	Delete from dbo.Product where Id = @Id
                
                End
                ");
        }
        private async Task CreateCartStoredProcedures(IDbConnection connection)
        {
            await connection.ExecuteAsync(@"
             CREATE PROCEDURE [dbo].[spCart_Insert]
             	@UserId int,
             	@CreatedDate Date
             AS
             Begin
             
             	INSERT INTO dbo.Cart (UserId, CreatedDate)
             	VALUES (@UserId,  @CreatedDate)
             
             End
             
             
            ");

            await connection.ExecuteAsync(@"
            CREATE PROCEDURE [dbo].[spCart_Delete]
            	@UserId int
            AS
            Begin
            
            	Delete from dbo.Cart where UserId = @UserId
            
            End
            ");
            await connection.ExecuteAsync(@"
            CREATE PROCEDURE [dbo].[spCart_GetById]
            	@UserId int
            AS
            Begin
            	
            	Select UserId, SUM(prod.Price) as TotalAmount, CreatedDate from Cart
            	JOIN dbo.ItemCart p On UserId = p.CartId
            	JOIN dbo.Product prod On prod.Id = p.ProductId
            	Where UserId = @UserId
            	Group By UserId, CreatedDate
            	
            End
            ");
        }
    }
}
