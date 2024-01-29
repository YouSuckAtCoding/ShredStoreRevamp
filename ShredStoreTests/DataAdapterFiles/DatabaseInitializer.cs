using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Dapper;
using Moq;
using Renci.SshNet.Security;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

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
            
                 connection.Execute(
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
             connection.Execute(
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

             connection.Execute(
               @"CREATE TABLE [dbo].[Cart]
                (
                	[UserId] INT NOT NULL FOREIGN KEY REFERENCES [User](Id) PRIMARY KEY,
                	[TotalAmount] MONEY,
                	[CreatedDate] DATETIME NOT NULL
                
                )"
            );
            connection.Execute(
            @"CREATE TABLE[dbo].[CartItem]
            (
              [CartId] INT NOT NULL FOREIGN KEY REFERENCES Cart(UserId),
              [ProductId] INT NOT NULL FOREIGN KEY REFERENCES Product(Id),
              [Quantity] INT NOT NULL,
              [Price] money NOT NULL
            )");
             connection.Execute(@"
                CREATE TABLE [dbo].[Order]
                (
                	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
                	[Date] DATETIME NOT NULL,
                	[CartId] INT NOT NULL FOREIGN KEY REFERENCES Cart(UserId),
                	[TotalAmount] MONEY NOT NULL,
                	[UserId] INT NOT NULL FOREIGN KEY REFERENCES [User](Id)
                
                )");

                CreateUserStoredProcedures(connection);
                CreateProductStoredProcedures(connection);
                CreateCartStoredProcedures(connection);
                CreateCarttemStoredProcedures(connection);
                CreateOrderStorageProcedures(connection);



        }

        private void CreateUserStoredProcedures(IDbConnection connection)
        {
             connection.Execute(
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

             connection.Execute(
                @"CREATE PROCEDURE [dbo].[spUser_GetAll]
                AS
                Begin
                    Select Id, Name, Age, Email, Cpf, Address from dbo.[User]
                End
                ");

             connection.Execute(
                @"CREATE PROCEDURE [dbo].[spUser_GetById]
                @Id int
                AS
                Begin
                	Select Id, Name, Age, Email, Cpf, Address from dbo.[User]
                	Where Id = @Id
                End");

             connection.Execute(
                @"CREATE PROCEDURE [dbo].[spUser_Delete]
                @Id int
                AS
                Begin
                	Delete from dbo.[User] where Id = @Id
                End	");

             connection.Execute(
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
             connection.Execute(
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
            connection.Execute(
                @"CREATE PROCEDURE [dbo].[spUser_ResetPasswordByEmail]
            	@Email nvarchar(200),
            	@NewPassword nvarchar(50)
            AS
            Begin
            	
            	DECLARE @Salt UNIQUEIDENTIFIER=NEWID()
            	
            	Update dbo.[User] Set Password = HASHBYTES('SHA2_512', @NewPassword+CAST(@Salt as NVARCHAR(36))),
            	Salt = @Salt
            	Where Email = @Email;
            
            End");
        }
        private void CreateProductStoredProcedures(IDbConnection connection)
        {
             connection.Execute(
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

             connection.Execute(
                @"CREATE PROCEDURE [dbo].[spProduct_GetById]
                @Id int
                AS
                Begin
                
                	Select Id, Name, Description, Price, Type, Category from dbo.Product
                	Where Id = @Id
                
                End");

             connection.Execute(
                @"CREATE PROCEDURE [dbo].[spProduct_GetAll]
	
                AS
                Begin
                
                	Select Id, Name, Description, Price, Type, Category from dbo.Product
                
                End"
                );
             connection.Execute(
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
             connection.Execute(
                @"CREATE PROCEDURE [dbo].[spProduct_Delete]
                @Id int
                AS
                Begin
                
                	Delete from dbo.Product where Id = @Id
                
                End
                ");
        }
        private void CreateCartStoredProcedures(IDbConnection connection)
        {
             connection.Execute(@"
             CREATE PROCEDURE [dbo].[spCart_Insert]
             	@UserId int,
             	@CreatedDate Datetime
             AS
             Begin
             
             	INSERT INTO dbo.Cart (UserId, CreatedDate)
             	VALUES (@UserId,  @CreatedDate)
             
             End
             
             
            ");

             connection.Execute(@"
            CREATE PROCEDURE [dbo].[spCart_Delete]
            	@UserId int
            AS
            Begin
            
            	Delete from dbo.Cart where UserId = @UserId
            
            End
            ");
             connection.Execute(@"
                CREATE PROCEDURE [dbo].[spCart_GetById]
                	@UserId int
                AS
                Begin
                	
                	Select UserId, CreatedDate from Cart
                	Where UserId = @UserId
                	
                End");
        }
        private void CreateCarttemStoredProcedures(IDbConnection connection)
        {
             connection.Execute(
                @"CREATE PROCEDURE [dbo].[spCartItem_Insert]
            	@CartId int, 
            	@ProductId int,
            	@Quantity int,
            	@Price money
            AS
            Begin
            
            	Insert into dbo.[CartItem] (CartId, ProductId, Quantity, Price)
            	Values (@CartId, @ProductId, @Quantity, @Price)
            
            End");

             connection.Execute(
                @"CREATE PROCEDURE [dbo].[spCartItem_GetAll]
                	@CartId int
                AS
                Begin
                
                	Select CartId, ProductId, Quantity from dbo.[CartItem]  
                    Where CartId = @CartId
                
                End
                
                ");
             connection.Execute(
                @"CREATE PROCEDURE [dbo].[spCartItem_DeleteAll]
                	@CartId int
                AS
                	
                Begin
                	
                	Delete from dbo.[CartItem] 
                	Where CartId = @CartId;
                
                End");
             connection.Execute(
                @"
                CREATE PROCEDURE [dbo].[spCartItem_Delete]
                	@ProductId int,
                	@CartId int
                AS
                	
                Begin
                
                		Delete from dbo.[CartItem] 
                		Where ProductId = @ProductId and CartId = @CartId;
                End");

             connection.Execute(
                @"
                CREATE PROCEDURE [dbo].[spCartItem_Update]
                		@ProductId int,
                	@Quantity int,
                	@CartId int
                AS
                	
                Begin
                		Update dbo.CartItem
                		Set Quantity = @Quantity
                		Where ProductId = @ProductId 
                		and CartId = @CartId;
                End");
        }
        private void CreateOrderStorageProcedures(IDbConnection connection)
        {
             connection.Execute(@"
                CREATE PROCEDURE [dbo].[spOrder_Insert]
            	@Date datetime,
            	@CartId int,
            	@UserId int
            
            AS
            Begin
            
            	Declare @TotalAmount money
            	SET @TotalAmount = (Select SUM(Price) from dbo.CartItem where CartId = @CartId);
            	
            	INSERT INTO dbo.[Order] ([Date], CartId, TotalAmount, UserId)
            	Values (@Date, @CartId, @TotalAmount, @UserId)
            
            End");
             connection.Execute(@"
                CREATE PROCEDURE [dbo].[spOrder_Update]
                	@Id int,
                	@Date datetime
                AS
                Begin
                	
                	Update dbo.[Order]
                	Set [Date] = @Date
                	Where Id = @Id
                
                End");
             connection.Execute(@"
                CREATE PROCEDURE [dbo].[spOrder_Delete]
                	@Id int
                AS
                Begin
                	
                	Delete from dbo.[Order] where Id = @Id
                
                End");
             connection.Execute(@"
                CREATE PROCEDURE [dbo].[spOrder_GetById]
                	@Id int
                AS
                Begin
                
                	Select Id, [Date], CartId, TotalAmount, UserId from dbo.[Order]
                	Where Id = @Id
                
                End");
            connection.Execute(@"
                CREATE PROCEDURE [dbo].[spOrder_GetAllUserOrders]
                	@UserId int
                AS
                Begin
                
                	Select Id, [Date], CartId, TotalAmount, UserId from dbo.[Order]
                	Where UserId = @UserId
                
                End");
            
        }
    }
}
