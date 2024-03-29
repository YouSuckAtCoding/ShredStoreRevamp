/****** Object:  Database [ShredStoreOfficialDatabase]    Script Date: 25/03/2024 20:05:30 ******/
CREATE DATABASE [ShredStoreOfficialDatabase]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ShredStoreOfficialDatabase', FILENAME = N'/var/opt/mssql/data/ShredStoreOfficialDatabase_Primary.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ShredStoreOfficialDatabase_log', FILENAME = N'/var/opt/mssql/data/ShredStoreOfficialDatabase_Primary.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ShredStoreOfficialDatabase].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET ANSI_NULL_DEFAULT ON 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET ANSI_NULLS ON 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET ANSI_PADDING ON 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET ANSI_WARNINGS ON 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET ARITHABORT ON 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET CURSOR_DEFAULT  LOCAL 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET CONCAT_NULL_YIELDS_NULL ON 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET QUOTED_IDENTIFIER ON 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET RECOVERY FULL 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET  MULTI_USER 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET PAGE_VERIFY NONE  
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'ShredStoreOfficialDatabase', N'ON'
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET QUERY_STORE = OFF
GO
/****** Object:  Table [dbo].[Cart]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart](
	[UserId] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CartItem]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CartItem](
	[CartId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [money] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[TotalAmount] [money] NOT NULL,
	[UserId] [int] NOT NULL,
	[PaymentId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderItem]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [money] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Amount] [money] NOT NULL,
	[Date] [datetime] NOT NULL,
	[PaymentType] [tinyint] NOT NULL,
	[Payed] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](25) NOT NULL,
	[Description] [varchar](300) NOT NULL,
	[Price] [money] NOT NULL,
	[Type] [varchar](30) NOT NULL,
	[Category] [varchar](30) NOT NULL,
	[Brand] [nvarchar](50) NOT NULL,
	[ImageName] [nvarchar](50) NOT NULL,
	[UserId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](20) NOT NULL,
	[Age] [int] NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Cpf] [varchar](11) NOT NULL,
	[Address] [varchar](50) NOT NULL,
	[Role] [varchar](50) NOT NULL,
	[Password] [binary](64) NOT NULL,
	[Salt] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[CartItem]  WITH CHECK ADD FOREIGN KEY([CartId])
REFERENCES [dbo].[Cart] ([UserId])
GO
ALTER TABLE [dbo].[CartItem]  WITH CHECK ADD FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Payment] ([Id])
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
GO
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
/****** Object:  StoredProcedure [dbo].[spCart_Delete]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spCart_Delete]
	@Id int
AS
Begin

	Delete from dbo.Cart where UserId = @Id

End
GO
/****** Object:  StoredProcedure [dbo].[spCart_GetById]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spCart_GetById]
	@UserId int
AS
Begin
	
	Select UserId, CreatedDate from Cart
	Where UserId = @UserId
	
End
GO
/****** Object:  StoredProcedure [dbo].[spCart_Insert]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spCart_Insert]
	@UserId int,
	@CreatedDate datetime
AS
Begin

	INSERT INTO dbo.Cart (UserId, CreatedDate)
	VALUES (@UserId,  @CreatedDate)

End
GO
/****** Object:  StoredProcedure [dbo].[spCartItem_Delete]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spCartItem_Delete]
	@ProductId int,
	@CartId int
AS
	
Begin

		Delete from dbo.[CartItem] 
		Where ProductId = @ProductId and CartId = @CartId;
End
GO
/****** Object:  StoredProcedure [dbo].[spCartItem_DeleteAll]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spCartItem_DeleteAll]
	@CartId int
AS
	
Begin
	
	Delete from dbo.[CartItem] 
	Where CartId = @CartId;

End
GO
/****** Object:  StoredProcedure [dbo].[spCartItem_GetAll]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spCartItem_GetAll]
	@CartId int
AS
Begin

	Select CartId, ProductId, Quantity, Price from dbo.[CartItem]
	where CartId = @CartId

End
GO
/****** Object:  StoredProcedure [dbo].[spCartItem_GetById]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spCartItem_GetById]
	@CartId int,
	@ProductId int
AS
Begin

	Select CartId, ProductId, Quantity, Price from dbo.[CartItem]
	where CartId = @CartId and ProductId = @ProductId

End
GO
/****** Object:  StoredProcedure [dbo].[spCartItem_Insert]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spCartItem_Insert]
	@CartId int, 
	@ProductId int,
	@Quantity int	
AS

	SET IDENTITY_INSERT dbo.[CartItem] ON
	
	If(( Select count(*) from CartItem where ProductId = @ProductId and CartId = @CartId) = 1)

	Begin
		Exec spCartItem_Update @ProductId,@Quantity, @CartId
	End;
	Else
		Begin
		Declare @ProductPrice as money 
			Set @ProductPrice = (Select price from Product Where Id = @ProductId);


		Insert into dbo.[CartItem] (CartId, ProductId, Quantity, Price)
		Values (@CartId, @ProductId, @Quantity, @Quantity * @ProductPrice);
		End
	SET IDENTITY_INSERT dbo.[CartItem] OFF
GO
/****** Object:  StoredProcedure [dbo].[spCartItem_Update]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spCartItem_Update]
	@ProductId int,
	@Quantity int,
	@CartId int
AS
	
Begin

		Declare @ProductPrice as money 
		Set @ProductPrice = (Select price from Product Where Id = @ProductId);

		Update dbo.CartItem
		Set Quantity = @Quantity,
		Price = @Quantity * @ProductPrice
		Where ProductId = @ProductId 
		and CartId = @CartId;
End
GO
/****** Object:  StoredProcedure [dbo].[spOrder_Delete]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spOrder_Delete]
	@Id int
AS
Begin
	
	Delete from dbo.[Order] where Id = @Id

End
GO
/****** Object:  StoredProcedure [dbo].[spOrder_GetAll]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spOrder_GetAll]
	
AS

Begin
	
	Select * from dbo.[Order];
	
End
GO
/****** Object:  StoredProcedure [dbo].[spOrder_GetAllUserOrders]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spOrder_GetAllUserOrders]
	@UserId int
AS
Begin

	Select Id, [CreatedDate], TotalAmount, UserId from dbo.[Order]
	Where UserId = @UserId

End
GO
/****** Object:  StoredProcedure [dbo].[spOrder_GetById]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spOrder_GetById]
	@Id int
AS
Begin

	Select Id, [CreatedDate], UserId, TotalAmount from dbo.[Order]
	Where Id = @Id

End
GO
/****** Object:  StoredProcedure [dbo].[spOrder_Insert]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spOrder_Insert]
	@CreatedDate datetime,
	@UserId int, 
	@TotalAmount money,
	@PaymentId int

AS
Begin

	INSERT INTO dbo.[Order] ([CreatedDate], UserId, TotalAmount, PaymentId)
	Values (@CreatedDate, @UserId, @TotalAmount, @PaymentId)

End
GO
/****** Object:  StoredProcedure [dbo].[spOrder_Update]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spOrder_Update]
	@Id int,
	@CreatedDate datetime,
	@TotalAmount money
AS
Begin
	
	Update dbo.[Order]
	Set [CreatedDate] = @CreatedDate,
	TotalAmount = @TotalAmount
	Where Id = @Id

End
GO
/****** Object:  StoredProcedure [dbo].[spOrderItem_Delete]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spOrderItem_Delete]
	@ProductId int,
	@OrderId int
AS
	
Begin

		Delete from dbo.[OrderItem] 
		Where ProductId = @ProductId and OrderId = @OrderId;
End
GO
/****** Object:  StoredProcedure [dbo].[spOrderItem_DeleteAll]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spOrderItem_DeleteAll]
	@OrderId int
AS
	
Begin
	
	Delete from dbo.[OrderItem] 
	Where OrderId = @OrderId;

End
GO
/****** Object:  StoredProcedure [dbo].[spOrderItem_GetAll]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spOrderItem_GetAll]
	@OrderId int
AS

Begin
	
	Select * from dbo.OrderItem Where OrderId = @OrderId;
End
GO
/****** Object:  StoredProcedure [dbo].[spOrderItem_GetById]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spOrderItem_GetById]
	@OrderId int,
	@ProductId int
AS
Begin

	Select Id, OrderId, ProductId, Quantity, Price from dbo.[OrderItem]
	where OrderId = @OrderId and ProductId = @ProductId

End
GO
/****** Object:  StoredProcedure [dbo].[spOrderItem_Insert]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spOrderItem_Insert]
	@OrderId int, 
	@ProductId int,
	@Quantity int	
AS
	
	If(( Select count(*) from [OrderItem] where ProductId = @ProductId and OrderId = @OrderId) = 1)

	Begin
		Exec spOrderItem_Update @ProductId,@Quantity, @OrderId
	End;
	Else
		Begin
		Declare @ProductPrice as money 
			Set @ProductPrice = (Select price from Product Where Id = @ProductId);


		Insert into dbo.[OrderItem] (OrderId, ProductId, Quantity, Price)
		Values (@OrderId, @ProductId, @Quantity, @Quantity * @ProductPrice);
		End
GO
/****** Object:  StoredProcedure [dbo].[spOrderItem_Update]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spOrderItem_Update]
	@ProductId int,
	@Quantity int,
	@OrderId int
AS
	
Begin

		Declare @ProductPrice as money 
		Set @ProductPrice = (Select price from Product Where Id = @ProductId);

		Update dbo.OrderItem
		Set Quantity = @Quantity,
		Price = @Quantity * @ProductPrice
		Where ProductId = @ProductId 
		and OrderId = @OrderId;
End
GO
/****** Object:  StoredProcedure [dbo].[spPayment_GetAll]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spPayment_GetAll]

As
Begin
	Select * from dbo.Payment;
End
GO
/****** Object:  StoredProcedure [dbo].[spPayment_Insert]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spPayment_Insert]
	@Amount money,
	@Date datetime,
	@PaymentType tinyint,
	@Payed bit
As
Begin
	
	Insert into dbo.Payment (Amount, [Date], PaymentType, Payed)
	Values (@Amount, @Date, @PaymentType, @Payed);

End
GO
/****** Object:  StoredProcedure [dbo].[spProduct_Delete]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spProduct_Delete]
	@Id int
AS
Begin

	Delete from dbo.Product where Id = @Id

End
GO
/****** Object:  StoredProcedure [dbo].[spProduct_GetAll]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spProduct_GetAll]
	
AS
Begin

	Select Id, [Name], [Description], Price, [Type], Category, Brand, ImageName, UserId from dbo.Product

End
GO
/****** Object:  StoredProcedure [dbo].[spProduct_GetByCartId]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spProduct_GetByCartId]
	@CartId int
AS
Begin

	Select Product.Id, Product.[Name], Product.Price, Product.ImageName, CartItem.Quantity, CartItem.Price
	from Product Join CartItem on Product.Id = CartItem.ProductId and CartItem.CartId = @CartId

End
GO
/****** Object:  StoredProcedure [dbo].[spProduct_GetByCategory]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spProduct_GetByCategory]
		@Category nvarchar(100)
AS
Begin

	Select Id, Name, UserId, Brand, Price, [Description], Category, ImageName from dbo.[Product]
	Where Category = @Category;

End
GO
/****** Object:  StoredProcedure [dbo].[spProduct_GetById]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spProduct_GetById]
	@Id int
AS
Begin

	Select Id, [Name], [Description], Price, [Type], Category, Brand, ImageName from dbo.Product
	Where Id = @Id

End
GO
/****** Object:  StoredProcedure [dbo].[spProduct_GetByUserId]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spProduct_GetByUserId]
		@UserId int
AS
Begin
	
	Select Id, Name, UserId, Brand, Price, [Description], category, ImageName from dbo.[Product]
	Where UserId = @UserId;
	
End
GO
/****** Object:  StoredProcedure [dbo].[spProduct_Insert]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spProduct_Insert]
	@Name VARCHAR(25),
	@Description varchar(300),
	@Price MONEY,
	@Type VARCHAR(30),
	@Category VARCHAR(30),
	@Brand VARCHAR(50),
	@ImageName VARCHAR(30),
	@UserId int
	
AS
Begin
	
	INSERT INTO dbo.Product ([Name], [Description], Price, [Type], Category, Brand, ImageName, UserId)
	VALUES (@Name, @Description, @Price, @Type, @Category, @Brand, @ImageName, @UserId)

End
GO
/****** Object:  StoredProcedure [dbo].[spProduct_Update]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spProduct_Update]
	@Id int,
	@Name VARCHAR(25),
	@Description varchar(300),
	@Price MONEY,
	@Type VARCHAR(30),
	@Category VARCHAR(30),
	@Brand VARCHAR(50),
	@ImageName VARCHAR(30)
AS
Begin

	Update dbo.Product
	Set [Name] = @Name,
	[Description] = @Description,
	Price = @Price,
	[Type] = @Type,
	Category = @Category,
	Brand = @Brand,
	ImageName = @ImageName
	Where Id = @Id

End
GO
/****** Object:  StoredProcedure [dbo].[spUser_Delete]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spUser_Delete]
	@Id int
AS
Begin
	
	Delete from dbo.[User] where Id = @Id
	
End
GO
/****** Object:  StoredProcedure [dbo].[spUser_GetAll]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spUser_GetAll]
	
AS
Begin
	Select Id, [Name], Age, Email, Cpf, [Address], [Role] from dbo.[User]
End
GO
/****** Object:  StoredProcedure [dbo].[spUser_GetById]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spUser_GetById]
	@Id int
AS
Begin
	Select Id, [Name], Age, Email, Cpf, [Address], [Role] from dbo.[User]
	Where Id = @Id
End
GO
/****** Object:  StoredProcedure [dbo].[spUser_Insert]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spUser_Insert]
	@Name VARCHAR(20),
	@Age INT,
	@Email VARCHAR(50),
	@Cpf VARCHAR(11),
	@Address VARCHAR(50),
	@Password nvarchar(50),
	@Role varchar(10)

AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Salt UNIQUEIDENTIFIER=NEWID()

	INSERT INTO dbo.[User] ([Name], Age, Email, Cpf, Address, [Password], Salt, [Role])
	VALUES (@Name, @Age, @Email, @Cpf, @Address,HASHBYTES('SHA2_512', @Password+CAST(@Salt as NVARCHAR(36))), @Salt, @Role)
END
GO
/****** Object:  StoredProcedure [dbo].[spUser_Login]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spUser_Login]
	@Email nvarchar(50),
	@Password nvarchar(50),
	@ResponseMessage nvarchar(250)='' Output

AS
Begin
	
	SET NOCOUNT ON

	Declare @SelectedId int

	IF Exists (Select Top 1 Id From dbo.[User] Where [Email] = @Email)
	Begin
		Set @SelectedId=(Select Id from dbo.[User] Where [Email] = @Email And Password=HASHBYTES('SHA2_512',@Password+Cast(Salt As nvarchar(36))))

		If(@SelectedId Is Null)
			Set @ResponseMessage='Incorret Password'
		Else
			Set @ResponseMessage='Login Successfull'
			exec dbo.spUser_GetById @SelectedId
			
	End
	Else
		Set @ResponseMessage='Invalid Login Attempt'

End
GO
/****** Object:  StoredProcedure [dbo].[spUser_ResetPasswordByEmail]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spUser_ResetPasswordByEmail]
	@Email nvarchar(200),
	@NewPassword nvarchar(50)
AS
Begin
	
	DECLARE @Salt UNIQUEIDENTIFIER=NEWID()
	
	Update dbo.[User] Set Password = HASHBYTES('SHA2_512', @NewPassword+CAST(@Salt as NVARCHAR(36))),
	Salt = @Salt
	Where Email = @Email;

End
GO
/****** Object:  StoredProcedure [dbo].[spUser_Update]    Script Date: 25/03/2024 20:05:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spUser_Update]
	@Id int,
	@Name VARCHAR(20),
	@Age INT,
	@Email VARCHAR(50),
	@Cpf VARCHAR(11),
	@Address VARCHAR(50)
AS
Begin

	Update dbo.[User]
	Set [Name] = @Name, 
	Age = @Age,
	Email = @Email,
	Cpf = @Cpf,
	Address = @Address
	Where Id = @Id

End
GO
ALTER DATABASE [ShredStoreOfficialDatabase] SET  READ_WRITE 
GO
