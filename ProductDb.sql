USE [master]
GO
/****** deop database ProductsDB if exists ******/
IF EXISTS(SELECT database_id FROM sys.databases WHERE name = 'ProductsDB')
	DROP DATABASE ProductsDB
-- sql server 2016
-- DROP DATABASE IF EXISTS ProductsDB
GO
/****** Object:  Database [ProductsDB]    Script Date: 9/11/2019 9:49:04 PM ******/
CREATE DATABASE [ProductsDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Products', FILENAME = N'\Products.mdf' , SIZE = 2097152KB , MAXSIZE = 8388608KB , FILEGROWTH = 1048576KB )
 LOG ON 
( NAME = N'Products_log', FILENAME = N'\Products_log.ldf' , SIZE = 1048576KB , MAXSIZE = 2097152KB , FILEGROWTH = 10%)
GO
ALTER DATABASE [ProductsDB] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ProductsDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ProductsDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ProductsDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ProductsDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ProductsDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ProductsDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [ProductsDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ProductsDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ProductsDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ProductsDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ProductsDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ProductsDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ProductsDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ProductsDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ProductsDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ProductsDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ProductsDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ProductsDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ProductsDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ProductsDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ProductsDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ProductsDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ProductsDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ProductsDB] SET RECOVERY FULL 
GO
ALTER DATABASE [ProductsDB] SET  MULTI_USER 
GO
ALTER DATABASE [ProductsDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ProductsDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ProductsDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ProductsDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ProductsDB] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'ProductsDB', N'ON'
GO
ALTER DATABASE [ProductsDB] SET QUERY_STORE = OFF
GO
USE [ProductsDB]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 9/11/2019 9:49:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Price] [float] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedOn] [datetime] NULL
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Products_Name]    Script Date: 9/11/2019 9:49:04 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Products_Name] ON [dbo].[Products]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[AddProduct]    Script Date: 9/11/2019 9:49:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddProduct]
	@Name varchar(100),
	@Price float

AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ProductID int = null;

	SELECT @Name = CASE Trim(@Name) WHEN '' THEN NULL ELSE Trim(@Name) END;

	Insert Into Products
	(
		Name,
		Price,
		CreatedOn
	) Values (
		@Name,
		@Price,
		GETDATE()
	)

	SELECT @ProductID = SCOPE_IDENTITY()

	SELECT        ID, Name, Price, CreatedOn
	FROM            Products
	WHERE        (ID = @ProductID)

END
GO
/****** Object:  StoredProcedure [dbo].[DeleteProduct]    Script Date: 9/11/2019 9:49:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteProduct]
	@ProductId int

AS
BEGIN
	SET NOCOUNT ON;

	Delete Products
	WHERE ID = @ProductId

END
GO
/****** Object:  StoredProcedure [dbo].[GetAllProducts]    Script Date: 9/11/2019 9:49:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllProducts]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ID, Name, Price, CreatedOn, ModifiedOn
	FROM Products

END
GO
/****** Object:  StoredProcedure [dbo].[GetProduct]    Script Date: 9/11/2019 9:49:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetProduct]
	@ProductId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ID, Name, Price, CreatedOn, ModifiedOn
	FROM Products
	WHERE ID = @ProductId

END
GO
/****** Object:  StoredProcedure [dbo].[SearchProducts]    Script Date: 9/11/2019 9:49:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SearchProducts]
	@Search varchar(100) = null
AS
BEGIN
	SET NOCOUNT ON;

	SELECT @Search = CASE TRIM(@Search) WHEN '' THEN NULL ELSE TRIM(@Search) END;

	SELECT        ID, Name, Price, CreatedOn, ModifiedOn
	FROM            Products
	WHERE        (@Search IS NULL) OR
							 (Name like '%' + @Search + '%')

END
GO
/****** Object:  StoredProcedure [dbo].[UpdateProduct]    Script Date: 9/11/2019 9:49:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateProduct]
	@ProductId int,
	@Name varchar(100),
	@Price float

AS
BEGIN
	SET NOCOUNT ON;

	SELECT @Name = CASE Trim(@Name) WHEN '' THEN NULL ELSE Trim(@Name) END;

	Update Products set
		Name = @Name,
		Price = @Price,
		ModifiedOn = GETDATE()
	WHERE ID = @ProductId

	SELECT        ID, Name, Price, CreatedOn, ModifiedOn
	FROM            Products
	WHERE        (ID = @ProductID)

END
GO
USE [master]
GO
ALTER DATABASE [ProductsDB] SET  READ_WRITE 
GO
/****** Seeding ******/
USE [ProductsDB]
GO
INSERT INTO Products (Name, Price, CreatedOn)
SELECT 'Product1', 100, GETDATE()
UNION ALL
SELECT 'Product2', 150, GETDATE()
UNION ALL
SELECT 'Product3', 118, GETDATE()
UNION ALL
SELECT 'Product4', 400, GETDATE()
UNION ALL
SELECT 'Product5', 178, GETDATE()
UNION ALL
SELECT 'Product6', 524, GETDATE()
UNION ALL
SELECT 'Product7', 258, GETDATE()
UNION ALL
SELECT 'Product8', 18, GETDATE()
UNION ALL
SELECT 'Product9', 822, GETDATE()
UNION ALL
SELECT 'Product10', 178, GETDATE()
UNION ALL
SELECT 'Product11', 96, GETDATE()