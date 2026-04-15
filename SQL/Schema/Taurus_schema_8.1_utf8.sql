USE [master]
GO
/****** Object:  Database [Taurus-bike-shop-sqlserver-2026]    Script Date: 04/15/2026 8:17:17 PM ******/
CREATE DATABASE [Taurus-bike-shop-sqlserver-2026]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TaurusBikeShopDB', FILENAME = N'/var/opt/mssql/data/fd997ada-09e6-46a1-aae0-6030405a2e5c_1_48037077-cb01-_e1f3d319-3179-4551-b19f-bb90ae79c0af.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'TaurusBikeShopDB_log', FILENAME = N'/var/opt/mssql/data/fd997ada-09e6-46a1-aae0-6030405a2e5c_2_48037077-cb01-_38116672-4b20-49ca-976b-6e55181ef403.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Taurus-bike-shop-sqlserver-2026].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET ARITHABORT OFF 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET RECOVERY FULL 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET  MULTI_USER 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET QUERY_STORE = ON
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Taurus-bike-shop-sqlserver-2026]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[PaymentId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[PaymentMethod] [nvarchar](50) NOT NULL,
	[PaymentStage] [nvarchar](20) NOT NULL,
	[PaymentStatus] [nvarchar](50) NOT NULL,
	[Amount] [decimal](10, 2) NOT NULL,
	[PaymentDate] [datetime] NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GCashPayment]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GCashPayment](
	[PaymentId] [int] NOT NULL,
	[GcashTransactionId] [nvarchar](255) NULL,
	[ScreenshotUrl] [nvarchar](1000) NULL,
	[StorageBucket] [nvarchar](200) NULL,
	[StoragePath] [nvarchar](1000) NULL,
	[SubmittedAt] [datetime] NULL,
 CONSTRAINT [PK_GCashPayment] PRIMARY KEY CLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BankTransferPayment]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BankTransferPayment](
	[PaymentId] [int] NOT NULL,
	[BpiReferenceNumber] [nvarchar](255) NULL,
	[ProofUrl] [nvarchar](1000) NULL,
	[ProofStorageBucket] [nvarchar](200) NULL,
	[ProofStoragePath] [nvarchar](1000) NULL,
	[VerifiedByUserId] [int] NULL,
	[VerificationNotes] [nvarchar](500) NULL,
	[VerifiedAt] [datetime] NULL,
	[VerificationDeadline] [datetime] NULL,
	[SubmittedAt] [datetime] NULL,
 CONSTRAINT [PK_BankTransferPayment] PRIMARY KEY CLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_PaymentDetail]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_PaymentDetail] AS
SELECT
        p.PaymentId, p.OrderId, p.PaymentMethod,
        p.PaymentStage, p.PaymentStatus,
        p.Amount, p.PaymentDate, p.CreatedAt,
        -- GCash fields
        g.GcashTransactionId,
        g.ScreenshotUrl     AS GCashScreenshotUrl,
        g.StorageBucket     AS GCashStorageBucket,
        g.StoragePath       AS GCashStoragePath,
        g.SubmittedAt       AS GCashSubmittedAt,
        -- BankTransfer fields
        bt.BpiReferenceNumber,
        bt.ProofUrl                     AS PaymentProofUrl,
        bt.ProofStorageBucket,
        bt.ProofStoragePath,
        bt.VerifiedByUserId,
        bt.VerificationNotes,
        bt.VerifiedAt,
        bt.VerificationDeadline,
        bt.SubmittedAt                  AS BankTransferSubmittedAt
FROM Payment p
LEFT JOIN GCashPayment          g  ON p.PaymentId = g.PaymentId
LEFT JOIN BankTransferPayment   bt ON p.PaymentId = bt.PaymentId;
GO
/****** Object:  Table [dbo].[VoucherUsage]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VoucherUsage](
	[VoucherUsageId] [int] IDENTITY(1,1) NOT NULL,
	[VoucherId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[OrderId] [int] NOT NULL,
	[DiscountAmount] [decimal](10, 2) NOT NULL,
	[UsedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_VoucherUsage] PRIMARY KEY CLUSTERED 
(
	[VoucherUsageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_UserVoucherUsageCount]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================================================
-- VIEWS
-- =============================================================================

-- Voucher usage count per user
CREATE VIEW [dbo].[vw_UserVoucherUsageCount] AS
SELECT UserId, VoucherId, COUNT(*) AS TimesUsed
FROM VoucherUsage
GROUP BY UserId, VoucherId;
GO
/****** Object:  Table [dbo].[ProductImage]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductImage](
	[ProductImageId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[StorageBucket] [nvarchar](200) NOT NULL,
	[StoragePath] [nvarchar](1000) NOT NULL,
	[ImageUrl] [nvarchar](1000) NOT NULL,
	[ImageType] [nvarchar](50) NOT NULL,
	[IsPrimary] [bit] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[AltText] [nvarchar](200) NULL,
	[FileSizeBytes] [int] NULL,
	[MimeType] [nvarchar](50) NULL,
	[Width] [int] NULL,
	[Height] [int] NULL,
	[UploadedByUserId] [int] NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_ProductImage] PRIMARY KEY CLUSTERED 
(
	[ProductImageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryCode] [varchar](20) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[ParentCategoryId] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Category_Code] UNIQUE NONCLUSTERED 
(
	[CategoryCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Brand]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Brand](
	[BrandId] [int] IDENTITY(1,1) NOT NULL,
	[BrandName] [nvarchar](100) NOT NULL,
	[Country] [nvarchar](100) NULL,
	[Website] [nvarchar](255) NULL,
	[Description] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Brand] PRIMARY KEY CLUSTERED 
(
	[BrandId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Brand_Name] UNIQUE NONCLUSTERED 
(
	[BrandName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[BrandId] [int] NULL,
	[SKU] [varchar](50) NULL,
	[Name] [nvarchar](200) NOT NULL,
	[ShortDescription] [nvarchar](300) NULL,
	[Description] [nvarchar](max) NULL,
	[Price] [decimal](10, 2) NOT NULL,
	[Currency] [char](3) NOT NULL,
	[Material] [nvarchar](100) NULL,
	[Color] [nvarchar](100) NULL,
	[WheelSize] [nvarchar](20) NULL,
	[SpeedCompatibility] [nvarchar](50) NULL,
	[BoostCompatible] [bit] NULL,
	[TubelessReady] [bit] NULL,
	[AxleStandard] [nvarchar](50) NULL,
	[SuspensionTravel] [nvarchar](50) NULL,
	[BrakeType] [nvarchar](100) NULL,
	[AdditionalSpecs] [nvarchar](1000) NULL,
	[IsActive] [bit] NOT NULL,
	[IsFeatured] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductVariant]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductVariant](
	[ProductVariantId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[VariantName] [nvarchar](100) NOT NULL,
	[SKU] [nvarchar](50) NULL,
	[AdditionalPrice] [decimal](10, 2) NOT NULL,
	[StockQuantity] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[ReorderThreshold] [int] NOT NULL,
 CONSTRAINT [PK_ProductVariant] PRIMARY KEY CLUSTERED 
(
	[ProductVariantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_ActiveProducts]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Active products with brand, category, primary image
CREATE VIEW [dbo].[vw_ActiveProducts] AS
SELECT
    p.ProductId,
    p.[Name]             AS ProductName,
    p.ShortDescription,
    p.[Description],
    p.SKU,
    p.Price,
    p.Currency,
    p.IsFeatured,
    p.WheelSize,
    p.SpeedCompatibility,
    p.BoostCompatible,
    p.TubelessReady,
    p.Material,
    p.BrakeType,
    c.CategoryId,
    c.[Name]             AS CategoryName,
    c.CategoryCode,
    b.BrandId,
    b.BrandName,
    b.Country            AS BrandCountry,
    pi.ImageUrl          AS PrimaryImageUrl,
    pi.StorageBucket     AS PrimaryImageBucket,
    pi.StoragePath       AS PrimaryImagePath,
    CASE WHEN EXISTS (
        SELECT 1 FROM ProductVariant pv
        WHERE pv.ProductId = p.ProductId AND pv.IsActive = 1
    ) THEN 1 ELSE 0 END  AS HasVariants
FROM Product p
INNER JOIN Category c      ON p.CategoryId = c.CategoryId
LEFT  JOIN Brand b         ON p.BrandId    = b.BrandId
LEFT  JOIN ProductImage pi ON p.ProductId  = pi.ProductId AND pi.IsPrimary = 1
WHERE p.IsActive = 1 AND c.IsActive = 1;
GO
/****** Object:  View [dbo].[vw_ProductImageGallery]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Product image gallery with GCS storage paths
CREATE VIEW [dbo].[vw_ProductImageGallery] AS
SELECT
    pi.ProductImageId, pi.ProductId,
    p.[Name]      AS ProductName,
    pi.StorageBucket, pi.StoragePath, pi.ImageUrl,
    pi.ImageType, pi.IsPrimary, pi.DisplayOrder,
    pi.AltText, pi.FileSizeBytes, pi.MimeType,
    pi.Width, pi.Height, pi.CreatedAt
FROM ProductImage pi
INNER JOIN Product p ON pi.ProductId = p.ProductId
WHERE p.IsActive = 1;
GO
/****** Object:  View [dbo].[vw_ProductVariantDetails]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Product variant details with computed total price
CREATE VIEW [dbo].[vw_ProductVariantDetails] AS
SELECT
    pv.ProductVariantId, pv.ProductId,
    p.[Name]                       AS ProductName,
    pv.VariantName, pv.SKU,
    p.Price                        AS BasePrice,
    pv.AdditionalPrice,
    (p.Price + pv.AdditionalPrice) AS TotalPrice,
    pv.StockQuantity, pv.IsActive
FROM ProductVariant pv
INNER JOIN Product p ON pv.ProductId = p.ProductId
WHERE pv.IsActive = 1 AND p.IsActive = 1;
GO
/****** Object:  View [dbo].[vw_InventoryStatus]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Inventory status — all stock lives in ProductVariant (v7.0)
-- Products with no real variants have one row with VariantName = 'Default'.
CREATE VIEW [dbo].[vw_InventoryStatus] AS
SELECT
    p.ProductId,
    p.[Name]                         AS ProductName,
    ISNULL(SUM(pv.StockQuantity), 0) AS TotalStock,
    c.[Name]                         AS CategoryName,
    b.BrandName
FROM Product p
INNER JOIN Category c ON p.CategoryId = c.CategoryId
LEFT  JOIN Brand b    ON p.BrandId    = b.BrandId
LEFT  JOIN ProductVariant pv ON p.ProductId = pv.ProductId AND pv.IsActive = 1
WHERE p.IsActive = 1
GROUP BY p.ProductId, p.[Name], c.[Name], b.BrandName;
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[NotificationId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[OrderId] [int] NULL,
	[TicketId] [int] NULL,
	[Channel] [nvarchar](20) NOT NULL,
	[NotifType] [nvarchar](100) NOT NULL,
	[Recipient] [nvarchar](255) NOT NULL,
	[Subject] [nvarchar](255) NULL,
	[Body] [nvarchar](max) NULL,
	[Status] [nvarchar](20) NOT NULL,
	[RetryCount] [int] NOT NULL,
	[SentAt] [datetime] NULL,
	[FailureReason] [nvarchar](500) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[IsRead] [bit] NOT NULL,
	[ReadAt] [datetime2](7) NULL,
 CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED 
(
	[NotificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](255) NULL,
	[PasswordHash] [nvarchar](255) NULL,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[DefaultAddressId] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[IsWalkIn] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastLoginAt] [datetime] NULL,
	[FailedLoginAttempts] [int] NOT NULL,
	[LockoutUntil] [datetime] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_PendingNotifications]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Pending notifications for background job (Part 14) — max 3 retries
CREATE VIEW [dbo].[vw_PendingNotifications] AS
SELECT
    n.NotificationId, n.Channel, n.NotifType,
    n.Recipient, n.Subject, n.Body,
    n.RetryCount, n.CreatedAt,
    u.Email       AS UserEmail,
    u.PhoneNumber AS UserPhone
FROM Notification n
INNER JOIN [User] u ON n.UserId = u.UserId
WHERE n.[Status] = 'Pending' AND n.RetryCount < 3;
GO
/****** Object:  Table [dbo].[SupportTicket]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SupportTicket](
	[TicketId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[OrderId] [int] NULL,
	[TicketSource] [nvarchar](50) NOT NULL,
	[TicketCategory] [nvarchar](100) NOT NULL,
	[Subject] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[AttachmentUrl] [nvarchar](1000) NULL,
	[AttachmentBucket] [nvarchar](200) NULL,
	[AttachmentPath] [nvarchar](1000) NULL,
	[TicketStatus] [nvarchar](50) NOT NULL,
	[AssignedToUserId] [int] NULL,
	[ResolvedAt] [datetime] NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_SupportTicket] PRIMARY KEY CLUSTERED 
(
	[TicketId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[OrderNumber] [nvarchar](50) NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[OrderStatus] [nvarchar](50) NOT NULL,
	[SubTotal] [decimal](10, 2) NOT NULL,
	[DiscountAmount] [decimal](10, 2) NOT NULL,
	[ShippingFee] [decimal](10, 2) NOT NULL,
	[ShippingAddressId] [int] NULL,
	[ContactPhone] [nvarchar](20) NULL,
	[DeliveryInstructions] [nvarchar](500) NULL,
	[IsWalkIn] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[OrderNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_OpenSupportTickets]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Open support tickets — admin Part 13 dashboard
CREATE VIEW [dbo].[vw_OpenSupportTickets] AS
SELECT
    t.TicketId, t.TicketSource, t.TicketCategory,
    t.Subject, t.TicketStatus, t.CreatedAt,
    u.FirstName + ' ' + u.LastName  AS CustomerName,
    u.Email                          AS CustomerEmail,
    u.PhoneNumber                    AS CustomerPhone,
    o.OrderNumber,
    a.FirstName + ' ' + a.LastName   AS AssignedTo
FROM SupportTicket t
INNER JOIN [User] u  ON t.UserId           = u.UserId
LEFT  JOIN [Order] o ON t.OrderId          = o.OrderId
LEFT  JOIN [User] a  ON t.AssignedToUserId = a.UserId
WHERE t.TicketStatus NOT IN ('Resolved','Closed');
GO
/****** Object:  Table [dbo].[LBCDelivery]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LBCDelivery](
	[DeliveryId] [int] NOT NULL,
	[TrackingNumber] [nvarchar](255) NULL,
 CONSTRAINT [PK_LBCDelivery] PRIMARY KEY CLUSTERED 
(
	[DeliveryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Delivery]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Delivery](
	[DeliveryId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[Courier] [nvarchar](20) NOT NULL,
	[DeliveryStatus] [nvarchar](50) NOT NULL,
	[IsDelayed] [bit] NOT NULL,
	[DelayedUntil] [datetime] NULL,
	[EstimatedDeliveryTime] [datetime] NULL,
	[ActualDeliveryTime] [datetime] NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Delivery] PRIMARY KEY CLUSTERED 
(
	[DeliveryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LalamoveDelivery]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LalamoveDelivery](
	[DeliveryId] [int] NOT NULL,
	[BookingRef] [nvarchar](255) NULL,
	[DriverName] [nvarchar](100) NULL,
	[DriverPhone] [nvarchar](20) NULL,
 CONSTRAINT [PK_LalamoveDelivery] PRIMARY KEY CLUSTERED 
(
	[DeliveryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_DeliveryDetail]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Full delivery detail — joins Delivery base with both subtype tables.
-- Subtype columns are NULL when not applicable to the courier.
CREATE VIEW [dbo].[vw_DeliveryDetail] AS
SELECT
    d.DeliveryId,
    d.OrderId,
    d.Courier,
    d.DeliveryStatus,
    d.IsDelayed,
    d.DelayedUntil,
    d.EstimatedDeliveryTime,
    d.ActualDeliveryTime,
    d.CreatedAt,
    -- Lalamove-specific fields (NULL for LBC orders)
    lm.BookingRef       AS LalamoveBookingRef,
    lm.DriverName,
    lm.DriverPhone,
    -- LBC-specific fields (NULL for Lalamove orders)
    lbc.TrackingNumber  AS LbcTrackingNumber
FROM Delivery d
LEFT JOIN LalamoveDelivery lm  ON d.DeliveryId = lm.DeliveryId
LEFT JOIN LBCDelivery      lbc ON d.DeliveryId = lbc.DeliveryId;
GO
/****** Object:  Table [dbo].[OrderItem]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItem](
	[OrderItemId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[ProductVariantId] [int] NULL,
	[Quantity] [int] NOT NULL,
	[UnitPrice] [decimal](10, 2) NOT NULL,
 CONSTRAINT [PK_OrderItem] PRIMARY KEY CLUSTERED 
(
	[OrderItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_OrderItemDetail]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Order item detail — Subtotal computed (not stored) in v7.0
CREATE VIEW [dbo].[vw_OrderItemDetail] AS
SELECT
    oi.OrderItemId,
    oi.OrderId,
    oi.ProductId,
    oi.ProductVariantId,
    p.[Name]                        AS ProductName,
    pv.VariantName,
    oi.Quantity,
    oi.UnitPrice,
    (oi.Quantity * oi.UnitPrice)    AS Subtotal
FROM OrderItem oi
INNER JOIN Product p ON oi.ProductId = p.ProductId
LEFT  JOIN ProductVariant pv ON oi.ProductVariantId = pv.ProductVariantId;
GO
/****** Object:  Table [dbo].[Supplier]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Supplier](
	[SupplierId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[ContactPerson] [nvarchar](100) NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[Email] [nvarchar](255) NULL,
	[Address] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED 
(
	[SupplierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchaseOrder]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseOrder](
	[PurchaseOrderId] [int] IDENTITY(1,1) NOT NULL,
	[SupplierId] [int] NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[ExpectedDeliveryDate] [datetime] NULL,
	[Status] [nvarchar](50) NOT NULL,
	[CreatedByUserId] [int] NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_PurchaseOrder] PRIMARY KEY CLUSTERED 
(
	[PurchaseOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchaseOrderItem]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseOrderItem](
	[PurchaseOrderItemId] [int] IDENTITY(1,1) NOT NULL,
	[PurchaseOrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[ProductVariantId] [int] NULL,
	[Quantity] [int] NOT NULL,
	[UnitPrice] [decimal](10, 2) NOT NULL,
 CONSTRAINT [PK_PurchaseOrderItem] PRIMARY KEY CLUSTERED 
(
	[PurchaseOrderItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_PurchaseOrderDetail]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Purchase order detail — item subtotals and order total computed in v7.0
CREATE VIEW [dbo].[vw_PurchaseOrderDetail] AS
SELECT
    po.PurchaseOrderId,
    po.SupplierId,
    s.[Name]                              AS SupplierName,
    po.OrderDate,
    po.ExpectedDeliveryDate,
    po.[Status],
    po.CreatedByUserId,
    po.CreatedAt,
    SUM(poi.Quantity * poi.UnitPrice)     AS TotalAmount,
    COUNT(poi.PurchaseOrderItemId)        AS LineItemCount
FROM PurchaseOrder po
INNER JOIN Supplier s ON po.SupplierId = s.SupplierId
LEFT  JOIN PurchaseOrderItem poi ON po.PurchaseOrderId = poi.PurchaseOrderId
GROUP BY
    po.PurchaseOrderId, po.SupplierId, s.[Name],
    po.OrderDate, po.ExpectedDeliveryDate, po.[Status],
    po.CreatedByUserId, po.CreatedAt;
GO
/****** Object:  Table [dbo].[PickupOrder]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PickupOrder](
	[PickupOrderId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[PickupReadyAt] [datetime] NULL,
	[PickupExpiresAt] [datetime] NULL,
	[PickupConfirmedAt] [datetime] NULL,
 CONSTRAINT [PK_PickupOrder] PRIMARY KEY CLUSTERED 
(
	[PickupOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UX_PickupOrder_Order] UNIQUE NONCLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Address]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Address](
	[AddressId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Label] [nvarchar](50) NOT NULL,
	[Street] [nvarchar](500) NOT NULL,
	[City] [nvarchar](100) NOT NULL,
	[PostalCode] [nvarchar](20) NOT NULL,
	[Province] [nvarchar](100) NULL,
	[Country] [nvarchar](100) NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[IsSnapshot] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED 
(
	[AddressId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_OrderSummary]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_OrderSummary] AS
SELECT
    o.OrderId,
    o.OrderNumber,
    o.OrderDate,
    o.OrderStatus,
    (o.SubTotal - o.DiscountAmount + o.ShippingFee) AS TotalAmount,
    o.SubTotal,
    o.DiscountAmount,
    o.ShippingFee,
    o.IsWalkIn,
    u.FirstName + ' ' + u.LastName AS CustomerName,
    u.Email                         AS CustomerEmail,
    u.PhoneNumber                   AS CustomerPhone,
    a.Street                        AS ShippingStreet,
    a.City                          AS ShippingCity,
    a.PostalCode                    AS ShippingPostalCode,
    a.Province                      AS ShippingProvince,
    po.PickupReadyAt,
    po.PickupExpiresAt,
    po.PickupConfirmedAt,
    COUNT(oi.OrderItemId)           AS ItemCount,
    SUM(oi.Quantity)                AS TotalQuantity
FROM [Order] o
INNER JOIN [User] u        ON o.UserId            = u.UserId
LEFT  JOIN [Address] a     ON o.ShippingAddressId = a.AddressId
LEFT  JOIN PickupOrder po  ON o.OrderId           = po.OrderId
LEFT  JOIN OrderItem oi    ON o.OrderId           = oi.OrderId
GROUP BY
    o.OrderId, o.OrderNumber, o.OrderDate, o.OrderStatus,
    o.SubTotal, o.DiscountAmount, o.ShippingFee, o.IsWalkIn,
    u.FirstName, u.LastName, u.Email, u.PhoneNumber,
    a.Street, a.City, a.PostalCode, a.Province,
    po.PickupReadyAt, po.PickupExpiresAt, po.PickupConfirmedAt;
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cart]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart](
	[CartId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[GuestSessionId] [int] NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastUpdatedAt] [datetime] NULL,
	[IsCheckedOut] [bit] NOT NULL,
 CONSTRAINT [PK_Cart] PRIMARY KEY CLUSTERED 
(
	[CartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CartItem]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CartItem](
	[CartItemId] [int] IDENTITY(1,1) NOT NULL,
	[CartId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[ProductVariantId] [int] NULL,
	[Quantity] [int] NOT NULL,
	[PriceAtAdd] [decimal](10, 2) NOT NULL,
	[AddedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_CartItem] PRIMARY KEY CLUSTERED 
(
	[CartItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GuestSession]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GuestSession](
	[GuestSessionId] [int] IDENTITY(1,1) NOT NULL,
	[SessionToken] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](255) NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[ConvertedToUserId] [int] NULL,
	[ExpiresAt] [datetime] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_GuestSession] PRIMARY KEY CLUSTERED 
(
	[GuestSessionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UX_GuestSession_Token] UNIQUE NONCLUSTERED 
(
	[SessionToken] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InventoryLog]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InventoryLog](
	[InventoryLogId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[ProductVariantId] [int] NULL,
	[ChangeQuantity] [int] NOT NULL,
	[ChangeType] [nvarchar](50) NOT NULL,
	[OrderId] [int] NULL,
	[PurchaseOrderId] [int] NULL,
	[ChangedByUserId] [int] NULL,
	[Notes] [nvarchar](500) NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_InventoryLog] PRIMARY KEY CLUSTERED 
(
	[InventoryLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderStatusAudit]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderStatusAudit](
	[AuditId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[FromStatus] [nvarchar](50) NOT NULL,
	[ToStatus] [nvarchar](50) NOT NULL,
	[Success] [bit] NOT NULL,
	[Reason] [nvarchar](500) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AuditId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OTPVerification]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OTPVerification](
	[OTPId] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[OTPCode] [nvarchar](10) NOT NULL,
	[IsUsed] [bit] NOT NULL,
	[ExpiresAt] [datetime] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_OTPVerification] PRIMARY KEY CLUSTERED 
(
	[OTPId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[POS_Session]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[POS_Session](
	[POSSessionId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[TerminalName] [nvarchar](50) NOT NULL,
	[ShiftStart] [datetime] NOT NULL,
	[ShiftEnd] [datetime] NULL,
	[TotalSales] [decimal](10, 2) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_POS_Session] PRIMARY KEY CLUSTERED 
(
	[POSSessionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PriceHistory]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PriceHistory](
	[PriceHistoryId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[OldPrice] [decimal](10, 2) NOT NULL,
	[NewPrice] [decimal](10, 2) NOT NULL,
	[ChangedAt] [datetime] NOT NULL,
	[ChangedByUserId] [int] NULL,
	[Notes] [nvarchar](500) NULL,
 CONSTRAINT [PK_PriceHistory] PRIMARY KEY CLUSTERED 
(
	[PriceHistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Review]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Review](
	[ReviewId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[OrderId] [int] NOT NULL,
	[Rating] [int] NOT NULL,
	[Comment] [nvarchar](1000) NULL,
	[IsVerifiedPurchase] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Review] PRIMARY KEY CLUSTERED 
(
	[ReviewId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SupportTask]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SupportTask](
	[TaskId] [int] IDENTITY(1,1) NOT NULL,
	[TicketId] [int] NOT NULL,
	[AssignedToUserId] [int] NULL,
	[TaskType] [nvarchar](50) NOT NULL,
	[TaskStatus] [nvarchar](20) NOT NULL,
	[DueDate] [datetime] NULL,
	[Notes] [nvarchar](500) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[CompletedAt] [datetime] NULL,
 CONSTRAINT [PK_SupportTask] PRIMARY KEY CLUSTERED 
(
	[TaskId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SupportTicketReply]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SupportTicketReply](
	[ReplyId] [int] IDENTITY(1,1) NOT NULL,
	[TicketId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[IsAdminReply] [bit] NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[AttachmentUrl] [nvarchar](1000) NULL,
	[AttachmentBucket] [nvarchar](200) NULL,
	[AttachmentPath] [nvarchar](1000) NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_SupportTicketReply] PRIMARY KEY CLUSTERED 
(
	[ReplyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemLog]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemLog](
	[SystemLogId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[EventType] [nvarchar](100) NOT NULL,
	[EventDescription] [nvarchar](1000) NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_SystemLog] PRIMARY KEY CLUSTERED 
(
	[SystemLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
	[UserRoleId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[AssignedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[UserRoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UX_UserRole_Pair] UNIQUE NONCLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserVoucher]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserVoucher](
	[UserVoucherId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[VoucherId] [int] NOT NULL,
	[AssignedAt] [datetime] NOT NULL,
	[ExpiresAt] [datetime] NULL,
 CONSTRAINT [PK_UserVoucher] PRIMARY KEY CLUSTERED 
(
	[UserVoucherId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UX_UserVoucher_Pair] UNIQUE NONCLUSTERED 
(
	[UserId] ASC,
	[VoucherId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Voucher]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Voucher](
	[VoucherId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DiscountType] [nvarchar](20) NOT NULL,
	[DiscountValue] [decimal](10, 2) NOT NULL,
	[MinimumOrderAmount] [decimal](10, 2) NULL,
	[MaxUses] [int] NULL,
	[MaxUsesPerUser] [int] NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Voucher] PRIMARY KEY CLUSTERED 
(
	[VoucherId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wishlist]    Script Date: 04/15/2026 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wishlist](
	[WishlistId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[AddedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Wishlist] PRIMARY KEY CLUSTERED 
(
	[WishlistId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UX_Wishlist_UserProduct] UNIQUE NONCLUSTERED 
(
	[UserId] ASC,
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_Address_IsDefault]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Address_IsDefault] ON [dbo].[Address]
(
	[UserId] ASC,
	[IsDefault] ASC
)
WHERE ([IsDefault]=(1))
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Address_IsSnapshot]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Address_IsSnapshot] ON [dbo].[Address]
(
	[IsSnapshot] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Address_UserId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Address_UserId] ON [dbo].[Address]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_BTP_BpiRef]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_BTP_BpiRef] ON [dbo].[BankTransferPayment]
(
	[BpiReferenceNumber] ASC
)
WHERE ([BpiReferenceNumber] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_BTP_VerificationDeadline]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_BTP_VerificationDeadline] ON [dbo].[BankTransferPayment]
(
	[VerificationDeadline] ASC
)
WHERE ([VerificationDeadline] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Brand_IsActive]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Brand_IsActive] ON [dbo].[Brand]
(
	[IsActive] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Cart_GuestSessionId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Cart_GuestSessionId] ON [dbo].[Cart]
(
	[GuestSessionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Cart_IsCheckedOut]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Cart_IsCheckedOut] ON [dbo].[Cart]
(
	[IsCheckedOut] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Cart_UserId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Cart_UserId] ON [dbo].[Cart]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UX_Cart_Guest_Active]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_Cart_Guest_Active] ON [dbo].[Cart]
(
	[GuestSessionId] ASC
)
WHERE ([IsCheckedOut]=(0) AND [GuestSessionId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UX_Cart_User_Active]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_Cart_User_Active] ON [dbo].[Cart]
(
	[UserId] ASC
)
WHERE ([IsCheckedOut]=(0) AND [UserId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CartItem_CartId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_CartItem_CartId] ON [dbo].[CartItem]
(
	[CartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CartItem_ProductId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_CartItem_ProductId] ON [dbo].[CartItem]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CartItem_ProductVariantId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_CartItem_ProductVariantId] ON [dbo].[CartItem]
(
	[ProductVariantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Category_DisplayOrder]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Category_DisplayOrder] ON [dbo].[Category]
(
	[DisplayOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Category_IsActive]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Category_IsActive] ON [dbo].[Category]
(
	[IsActive] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Category_ParentCategoryId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Category_ParentCategoryId] ON [dbo].[Category]
(
	[ParentCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Delivery_Courier]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Delivery_Courier] ON [dbo].[Delivery]
(
	[Courier] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Delivery_IsDelayed]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Delivery_IsDelayed] ON [dbo].[Delivery]
(
	[IsDelayed] ASC
)
WHERE ([IsDelayed]=(1))
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Delivery_OrderId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Delivery_OrderId] ON [dbo].[Delivery]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Delivery_Status]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Delivery_Status] ON [dbo].[Delivery]
(
	[DeliveryStatus] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_GCashPayment_TxnId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_GCashPayment_TxnId] ON [dbo].[GCashPayment]
(
	[GcashTransactionId] ASC
)
WHERE ([GcashTransactionId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_GuestSession_ExpiresAt]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_GuestSession_ExpiresAt] ON [dbo].[GuestSession]
(
	[ExpiresAt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_GuestSession_Token]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_GuestSession_Token] ON [dbo].[GuestSession]
(
	[SessionToken] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_InvLog_ChangeType]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_InvLog_ChangeType] ON [dbo].[InventoryLog]
(
	[ChangeType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_InvLog_CreatedAt]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_InvLog_CreatedAt] ON [dbo].[InventoryLog]
(
	[CreatedAt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_InvLog_OrderId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_InvLog_OrderId] ON [dbo].[InventoryLog]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_InvLog_ProductId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_InvLog_ProductId] ON [dbo].[InventoryLog]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_InvLog_ProductVariantId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_InvLog_ProductVariantId] ON [dbo].[InventoryLog]
(
	[ProductVariantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_InvLog_PurchaseOrderId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_InvLog_PurchaseOrderId] ON [dbo].[InventoryLog]
(
	[PurchaseOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_LalamoveDelivery_BookingRef]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_LalamoveDelivery_BookingRef] ON [dbo].[LalamoveDelivery]
(
	[BookingRef] ASC
)
WHERE ([BookingRef] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_LBCDelivery_TrackingNumber]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_LBCDelivery_TrackingNumber] ON [dbo].[LBCDelivery]
(
	[TrackingNumber] ASC
)
WHERE ([TrackingNumber] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Notif_Channel]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Notif_Channel] ON [dbo].[Notification]
(
	[Channel] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Notif_CreatedAt]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Notif_CreatedAt] ON [dbo].[Notification]
(
	[CreatedAt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Notif_OrderId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Notif_OrderId] ON [dbo].[Notification]
(
	[OrderId] ASC
)
WHERE ([OrderId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Notif_Pending]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Notif_Pending] ON [dbo].[Notification]
(
	[Status] ASC,
	[CreatedAt] ASC
)
WHERE ([Status]='Pending')
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Notif_Status]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Notif_Status] ON [dbo].[Notification]
(
	[Status] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Notif_TicketId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Notif_TicketId] ON [dbo].[Notification]
(
	[TicketId] ASC
)
WHERE ([TicketId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Notif_UserId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Notif_UserId] ON [dbo].[Notification]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Order_IsWalkIn]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Order_IsWalkIn] ON [dbo].[Order]
(
	[IsWalkIn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Order_OrderDate]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Order_OrderDate] ON [dbo].[Order]
(
	[OrderDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Order_OrderNumber]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Order_OrderNumber] ON [dbo].[Order]
(
	[OrderNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Order_OrderStatus]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Order_OrderStatus] ON [dbo].[Order]
(
	[OrderStatus] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Order_ShippingAddressId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Order_ShippingAddressId] ON [dbo].[Order]
(
	[ShippingAddressId] ASC
)
WHERE ([ShippingAddressId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Order_UserId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Order_UserId] ON [dbo].[Order]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderItem_OrderId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_OrderItem_OrderId] ON [dbo].[OrderItem]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderItem_ProductId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_OrderItem_ProductId] ON [dbo].[OrderItem]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderItem_ProductVariantId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_OrderItem_ProductVariantId] ON [dbo].[OrderItem]
(
	[ProductVariantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderStatusAudit_OrderId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_OrderStatusAudit_OrderId] ON [dbo].[OrderStatusAudit]
(
	[OrderId] ASC,
	[CreatedAt] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_OTP_Email]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_OTP_Email] ON [dbo].[OTPVerification]
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OTP_ExpiresAt]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_OTP_ExpiresAt] ON [dbo].[OTPVerification]
(
	[ExpiresAt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OTP_IsUsed]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_OTP_IsUsed] ON [dbo].[OTPVerification]
(
	[IsUsed] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Payment_Method]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Payment_Method] ON [dbo].[Payment]
(
	[PaymentMethod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Payment_OrderId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Payment_OrderId] ON [dbo].[Payment]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Payment_Stage]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Payment_Stage] ON [dbo].[Payment]
(
	[PaymentStage] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Payment_Status]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Payment_Status] ON [dbo].[Payment]
(
	[PaymentStatus] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PickupOrder_ExpiresAt]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_PickupOrder_ExpiresAt] ON [dbo].[PickupOrder]
(
	[PickupExpiresAt] ASC
)
WHERE ([PickupExpiresAt] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PickupOrder_OrderId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_PickupOrder_OrderId] ON [dbo].[PickupOrder]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_POS_Session_ShiftEnd]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_POS_Session_ShiftEnd] ON [dbo].[POS_Session]
(
	[ShiftEnd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_POS_Session_ShiftStart]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_POS_Session_ShiftStart] ON [dbo].[POS_Session]
(
	[ShiftStart] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_POS_Session_UserId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_POS_Session_UserId] ON [dbo].[POS_Session]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PriceHistory_ChangedAt]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_PriceHistory_ChangedAt] ON [dbo].[PriceHistory]
(
	[ChangedAt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PriceHistory_ProductId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_PriceHistory_ProductId] ON [dbo].[PriceHistory]
(
	[ProductId] ASC,
	[ChangedAt] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Product_BrandId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Product_BrandId] ON [dbo].[Product]
(
	[BrandId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Product_CategoryId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Product_CategoryId] ON [dbo].[Product]
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Product_IsActive]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Product_IsActive] ON [dbo].[Product]
(
	[IsActive] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Product_IsFeatured]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Product_IsFeatured] ON [dbo].[Product]
(
	[IsFeatured] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Product_Name]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Product_Name] ON [dbo].[Product]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Product_Price]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Product_Price] ON [dbo].[Product]
(
	[Price] ASC,
	[IsActive] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_Product_SKU]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_Product_SKU] ON [dbo].[Product]
(
	[SKU] ASC
)
WHERE ([SKU] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductImage_DisplayOrder]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProductImage_DisplayOrder] ON [dbo].[ProductImage]
(
	[DisplayOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ProductImage_ImageType]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProductImage_ImageType] ON [dbo].[ProductImage]
(
	[ImageType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductImage_IsPrimary]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProductImage_IsPrimary] ON [dbo].[ProductImage]
(
	[IsPrimary] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductImage_ProductId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProductImage_ProductId] ON [dbo].[ProductImage]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UX_ProductImage_Primary]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_ProductImage_Primary] ON [dbo].[ProductImage]
(
	[ProductId] ASC
)
WHERE ([IsPrimary]=(1))
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductVariant_IsActive]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProductVariant_IsActive] ON [dbo].[ProductVariant]
(
	[IsActive] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductVariant_ProductId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProductVariant_ProductId] ON [dbo].[ProductVariant]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductVariant_ReorderThreshold]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProductVariant_ReorderThreshold] ON [dbo].[ProductVariant]
(
	[ReorderThreshold] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ProductVariant_SKU]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProductVariant_SKU] ON [dbo].[ProductVariant]
(
	[SKU] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PurchaseOrder_CreatedByUserId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_PurchaseOrder_CreatedByUserId] ON [dbo].[PurchaseOrder]
(
	[CreatedByUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PurchaseOrder_OrderDate]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_PurchaseOrder_OrderDate] ON [dbo].[PurchaseOrder]
(
	[OrderDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_PurchaseOrder_Status]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_PurchaseOrder_Status] ON [dbo].[PurchaseOrder]
(
	[Status] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PurchaseOrder_SupplierId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_PurchaseOrder_SupplierId] ON [dbo].[PurchaseOrder]
(
	[SupplierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_POItem_ProductId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_POItem_ProductId] ON [dbo].[PurchaseOrderItem]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_POItem_ProductVariantId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_POItem_ProductVariantId] ON [dbo].[PurchaseOrderItem]
(
	[ProductVariantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_POItem_PurchaseOrderId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_POItem_PurchaseOrderId] ON [dbo].[PurchaseOrderItem]
(
	[PurchaseOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Review_OrderId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Review_OrderId] ON [dbo].[Review]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Review_ProductId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Review_ProductId] ON [dbo].[Review]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Review_Rating]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Review_Rating] ON [dbo].[Review]
(
	[Rating] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Review_UserId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Review_UserId] ON [dbo].[Review]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Role_RoleName]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Role_RoleName] ON [dbo].[Role]
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Supplier_IsActive]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Supplier_IsActive] ON [dbo].[Supplier]
(
	[IsActive] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_Supplier_Email]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_Supplier_Email] ON [dbo].[Supplier]
(
	[Email] ASC
)
WHERE ([Email] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_SupportTask_AssignedToUserId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_SupportTask_AssignedToUserId] ON [dbo].[SupportTask]
(
	[AssignedToUserId] ASC
)
WHERE ([AssignedToUserId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_SupportTask_DueDate]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_SupportTask_DueDate] ON [dbo].[SupportTask]
(
	[DueDate] ASC
)
WHERE ([DueDate] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_SupportTask_TaskStatus]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_SupportTask_TaskStatus] ON [dbo].[SupportTask]
(
	[TaskStatus] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_SupportTask_TicketId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_SupportTask_TicketId] ON [dbo].[SupportTask]
(
	[TicketId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Ticket_AssignedTo]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Ticket_AssignedTo] ON [dbo].[SupportTicket]
(
	[AssignedToUserId] ASC
)
WHERE ([AssignedToUserId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Ticket_Category]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Ticket_Category] ON [dbo].[SupportTicket]
(
	[TicketCategory] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Ticket_CreatedAt]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Ticket_CreatedAt] ON [dbo].[SupportTicket]
(
	[CreatedAt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Ticket_OrderId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Ticket_OrderId] ON [dbo].[SupportTicket]
(
	[OrderId] ASC
)
WHERE ([OrderId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Ticket_Status]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Ticket_Status] ON [dbo].[SupportTicket]
(
	[TicketStatus] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Ticket_UserId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Ticket_UserId] ON [dbo].[SupportTicket]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Reply_CreatedAt]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Reply_CreatedAt] ON [dbo].[SupportTicketReply]
(
	[CreatedAt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Reply_TicketId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Reply_TicketId] ON [dbo].[SupportTicketReply]
(
	[TicketId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Reply_UserId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Reply_UserId] ON [dbo].[SupportTicketReply]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_SystemLog_CreatedAt]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_SystemLog_CreatedAt] ON [dbo].[SystemLog]
(
	[CreatedAt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_SystemLog_EventType]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_SystemLog_EventType] ON [dbo].[SystemLog]
(
	[EventType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_SystemLog_UserId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_SystemLog_UserId] ON [dbo].[SystemLog]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_User_DefaultAddressId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_User_DefaultAddressId] ON [dbo].[User]
(
	[DefaultAddressId] ASC
)
WHERE ([DefaultAddressId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_User_IsActive]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_User_IsActive] ON [dbo].[User]
(
	[IsActive] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_User_IsWalkIn]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_User_IsWalkIn] ON [dbo].[User]
(
	[IsWalkIn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_User_PhoneNumber]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_User_PhoneNumber] ON [dbo].[User]
(
	[PhoneNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_User_Email]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_User_Email] ON [dbo].[User]
(
	[Email] ASC
)
WHERE ([Email] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserRole_RoleId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserRole_RoleId] ON [dbo].[UserRole]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserRole_UserId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserRole_UserId] ON [dbo].[UserRole]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserVoucher_UserId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserVoucher_UserId] ON [dbo].[UserVoucher]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserVoucher_VoucherId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserVoucher_VoucherId] ON [dbo].[UserVoucher]
(
	[VoucherId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Voucher_Code]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Voucher_Code] ON [dbo].[Voucher]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Voucher_EndDate]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Voucher_EndDate] ON [dbo].[Voucher]
(
	[EndDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Voucher_IsActive]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Voucher_IsActive] ON [dbo].[Voucher]
(
	[IsActive] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Voucher_StartDate]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Voucher_StartDate] ON [dbo].[Voucher]
(
	[StartDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_VoucherUsage_OrderId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_VoucherUsage_OrderId] ON [dbo].[VoucherUsage]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_VoucherUsage_UserId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_VoucherUsage_UserId] ON [dbo].[VoucherUsage]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_VoucherUsage_VoucherId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_VoucherUsage_VoucherId] ON [dbo].[VoucherUsage]
(
	[VoucherId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Wishlist_ProductId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Wishlist_ProductId] ON [dbo].[Wishlist]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Wishlist_UserId]    Script Date: 04/15/2026 8:17:18 PM ******/
CREATE NONCLUSTERED INDEX [IX_Wishlist_UserId] ON [dbo].[Wishlist]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Address] ADD  DEFAULT ('Home') FOR [Label]
GO
ALTER TABLE [dbo].[Address] ADD  DEFAULT ('Philippines') FOR [Country]
GO
ALTER TABLE [dbo].[Address] ADD  DEFAULT ((0)) FOR [IsDefault]
GO
ALTER TABLE [dbo].[Address] ADD  DEFAULT ((0)) FOR [IsSnapshot]
GO
ALTER TABLE [dbo].[Address] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Brand] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Brand] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Cart] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Cart] ADD  DEFAULT ((0)) FOR [IsCheckedOut]
GO
ALTER TABLE [dbo].[CartItem] ADD  DEFAULT (getdate()) FOR [AddedAt]
GO
ALTER TABLE [dbo].[Category] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Category] ADD  DEFAULT ((0)) FOR [DisplayOrder]
GO
ALTER TABLE [dbo].[Delivery] ADD  DEFAULT ('Pending') FOR [DeliveryStatus]
GO
ALTER TABLE [dbo].[Delivery] ADD  DEFAULT ((0)) FOR [IsDelayed]
GO
ALTER TABLE [dbo].[Delivery] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[GuestSession] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[InventoryLog] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Notification] ADD  DEFAULT ('Pending') FOR [Status]
GO
ALTER TABLE [dbo].[Notification] ADD  DEFAULT ((0)) FOR [RetryCount]
GO
ALTER TABLE [dbo].[Notification] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Notification] ADD  CONSTRAINT [DF_Notification_IsRead]  DEFAULT ((0)) FOR [IsRead]
GO
ALTER TABLE [dbo].[Order] ADD  DEFAULT (getdate()) FOR [OrderDate]
GO
ALTER TABLE [dbo].[Order] ADD  DEFAULT ((0)) FOR [DiscountAmount]
GO
ALTER TABLE [dbo].[Order] ADD  DEFAULT ((0)) FOR [ShippingFee]
GO
ALTER TABLE [dbo].[Order] ADD  DEFAULT ((0)) FOR [IsWalkIn]
GO
ALTER TABLE [dbo].[Order] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[OrderStatusAudit] ADD  DEFAULT ((1)) FOR [Success]
GO
ALTER TABLE [dbo].[OrderStatusAudit] ADD  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[OTPVerification] ADD  DEFAULT ((0)) FOR [IsUsed]
GO
ALTER TABLE [dbo].[OTPVerification] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Payment] ADD  DEFAULT ('Upfront') FOR [PaymentStage]
GO
ALTER TABLE [dbo].[Payment] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[POS_Session] ADD  DEFAULT ((0)) FOR [TotalSales]
GO
ALTER TABLE [dbo].[POS_Session] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[PriceHistory] ADD  DEFAULT (getdate()) FOR [ChangedAt]
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT ('PHP') FOR [Currency]
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT ((0)) FOR [IsFeatured]
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[ProductImage] ADD  DEFAULT ((0)) FOR [IsPrimary]
GO
ALTER TABLE [dbo].[ProductImage] ADD  DEFAULT ((0)) FOR [DisplayOrder]
GO
ALTER TABLE [dbo].[ProductImage] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[ProductVariant] ADD  DEFAULT ((0)) FOR [AdditionalPrice]
GO
ALTER TABLE [dbo].[ProductVariant] ADD  DEFAULT ((0)) FOR [StockQuantity]
GO
ALTER TABLE [dbo].[ProductVariant] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[ProductVariant] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[ProductVariant] ADD  DEFAULT ((5)) FOR [ReorderThreshold]
GO
ALTER TABLE [dbo].[PurchaseOrder] ADD  DEFAULT (getdate()) FOR [OrderDate]
GO
ALTER TABLE [dbo].[PurchaseOrder] ADD  DEFAULT ('Pending') FOR [Status]
GO
ALTER TABLE [dbo].[PurchaseOrder] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Review] ADD  DEFAULT ((0)) FOR [IsVerifiedPurchase]
GO
ALTER TABLE [dbo].[Review] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Role] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Supplier] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Supplier] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[SupportTask] ADD  DEFAULT ('Pending') FOR [TaskStatus]
GO
ALTER TABLE [dbo].[SupportTask] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[SupportTicket] ADD  DEFAULT ('Customer') FOR [TicketSource]
GO
ALTER TABLE [dbo].[SupportTicket] ADD  DEFAULT ('Open') FOR [TicketStatus]
GO
ALTER TABLE [dbo].[SupportTicket] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[SupportTicketReply] ADD  DEFAULT ((0)) FOR [IsAdminReply]
GO
ALTER TABLE [dbo].[SupportTicketReply] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[SystemLog] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[User] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[User] ADD  DEFAULT ((0)) FOR [IsWalkIn]
GO
ALTER TABLE [dbo].[User] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_FailedLoginAttempts]  DEFAULT ((0)) FOR [FailedLoginAttempts]
GO
ALTER TABLE [dbo].[UserRole] ADD  DEFAULT (getdate()) FOR [AssignedAt]
GO
ALTER TABLE [dbo].[UserVoucher] ADD  DEFAULT (getdate()) FOR [AssignedAt]
GO
ALTER TABLE [dbo].[Voucher] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Voucher] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[VoucherUsage] ADD  DEFAULT (getdate()) FOR [UsedAt]
GO
ALTER TABLE [dbo].[Wishlist] ADD  DEFAULT (getdate()) FOR [AddedAt]
GO
ALTER TABLE [dbo].[Address]  WITH CHECK ADD  CONSTRAINT [FK_Address_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Address] CHECK CONSTRAINT [FK_Address_User]
GO
ALTER TABLE [dbo].[BankTransferPayment]  WITH CHECK ADD  CONSTRAINT [FK_BankTransferPayment_Admin] FOREIGN KEY([VerifiedByUserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[BankTransferPayment] CHECK CONSTRAINT [FK_BankTransferPayment_Admin]
GO
ALTER TABLE [dbo].[BankTransferPayment]  WITH CHECK ADD  CONSTRAINT [FK_BankTransferPayment_Pmt] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Payment] ([PaymentId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BankTransferPayment] CHECK CONSTRAINT [FK_BankTransferPayment_Pmt]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD  CONSTRAINT [FK_Cart_GuestSession] FOREIGN KEY([GuestSessionId])
REFERENCES [dbo].[GuestSession] ([GuestSessionId])
GO
ALTER TABLE [dbo].[Cart] CHECK CONSTRAINT [FK_Cart_GuestSession]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD  CONSTRAINT [FK_Cart_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Cart] CHECK CONSTRAINT [FK_Cart_User]
GO
ALTER TABLE [dbo].[CartItem]  WITH CHECK ADD  CONSTRAINT [FK_CartItem_Cart] FOREIGN KEY([CartId])
REFERENCES [dbo].[Cart] ([CartId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CartItem] CHECK CONSTRAINT [FK_CartItem_Cart]
GO
ALTER TABLE [dbo].[CartItem]  WITH CHECK ADD  CONSTRAINT [FK_CartItem_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
GO
ALTER TABLE [dbo].[CartItem] CHECK CONSTRAINT [FK_CartItem_Product]
GO
ALTER TABLE [dbo].[CartItem]  WITH CHECK ADD  CONSTRAINT [FK_CartItem_Variant] FOREIGN KEY([ProductVariantId])
REFERENCES [dbo].[ProductVariant] ([ProductVariantId])
GO
ALTER TABLE [dbo].[CartItem] CHECK CONSTRAINT [FK_CartItem_Variant]
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [FK_Category_Parent] FOREIGN KEY([ParentCategoryId])
REFERENCES [dbo].[Category] ([CategoryId])
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [FK_Category_Parent]
GO
ALTER TABLE [dbo].[Delivery]  WITH CHECK ADD  CONSTRAINT [FK_Delivery_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
GO
ALTER TABLE [dbo].[Delivery] CHECK CONSTRAINT [FK_Delivery_Order]
GO
ALTER TABLE [dbo].[GCashPayment]  WITH CHECK ADD  CONSTRAINT [FK_GCashPayment_Pmt] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Payment] ([PaymentId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GCashPayment] CHECK CONSTRAINT [FK_GCashPayment_Pmt]
GO
ALTER TABLE [dbo].[GuestSession]  WITH CHECK ADD  CONSTRAINT [FK_GuestSession_User] FOREIGN KEY([ConvertedToUserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[GuestSession] CHECK CONSTRAINT [FK_GuestSession_User]
GO
ALTER TABLE [dbo].[InventoryLog]  WITH CHECK ADD  CONSTRAINT [FK_InvLog_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
GO
ALTER TABLE [dbo].[InventoryLog] CHECK CONSTRAINT [FK_InvLog_Order]
GO
ALTER TABLE [dbo].[InventoryLog]  WITH CHECK ADD  CONSTRAINT [FK_InvLog_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
GO
ALTER TABLE [dbo].[InventoryLog] CHECK CONSTRAINT [FK_InvLog_Product]
GO
ALTER TABLE [dbo].[InventoryLog]  WITH CHECK ADD  CONSTRAINT [FK_InvLog_PurchaseOrder] FOREIGN KEY([PurchaseOrderId])
REFERENCES [dbo].[PurchaseOrder] ([PurchaseOrderId])
GO
ALTER TABLE [dbo].[InventoryLog] CHECK CONSTRAINT [FK_InvLog_PurchaseOrder]
GO
ALTER TABLE [dbo].[InventoryLog]  WITH CHECK ADD  CONSTRAINT [FK_InvLog_User] FOREIGN KEY([ChangedByUserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[InventoryLog] CHECK CONSTRAINT [FK_InvLog_User]
GO
ALTER TABLE [dbo].[InventoryLog]  WITH CHECK ADD  CONSTRAINT [FK_InvLog_Variant] FOREIGN KEY([ProductVariantId])
REFERENCES [dbo].[ProductVariant] ([ProductVariantId])
GO
ALTER TABLE [dbo].[InventoryLog] CHECK CONSTRAINT [FK_InvLog_Variant]
GO
ALTER TABLE [dbo].[LalamoveDelivery]  WITH CHECK ADD  CONSTRAINT [FK_LalamoveDelivery_Base] FOREIGN KEY([DeliveryId])
REFERENCES [dbo].[Delivery] ([DeliveryId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LalamoveDelivery] CHECK CONSTRAINT [FK_LalamoveDelivery_Base]
GO
ALTER TABLE [dbo].[LBCDelivery]  WITH CHECK ADD  CONSTRAINT [FK_LBCDelivery_Base] FOREIGN KEY([DeliveryId])
REFERENCES [dbo].[Delivery] ([DeliveryId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LBCDelivery] CHECK CONSTRAINT [FK_LBCDelivery_Base]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notif_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_Notif_Order]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notif_Ticket] FOREIGN KEY([TicketId])
REFERENCES [dbo].[SupportTicket] ([TicketId])
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_Notif_Ticket]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notif_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_Notif_User]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_ShipAddress] FOREIGN KEY([ShippingAddressId])
REFERENCES [dbo].[Address] ([AddressId])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_ShipAddress]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_User]
GO
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_Order]
GO
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
GO
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_Product]
GO
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Variant] FOREIGN KEY([ProductVariantId])
REFERENCES [dbo].[ProductVariant] ([ProductVariantId])
GO
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_Variant]
GO
ALTER TABLE [dbo].[OrderStatusAudit]  WITH CHECK ADD  CONSTRAINT [FK_OrderStatusAudit_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
GO
ALTER TABLE [dbo].[OrderStatusAudit] CHECK CONSTRAINT [FK_OrderStatusAudit_Order]
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_Payment_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_Payment_Order]
GO
ALTER TABLE [dbo].[PickupOrder]  WITH CHECK ADD  CONSTRAINT [FK_PickupOrder_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PickupOrder] CHECK CONSTRAINT [FK_PickupOrder_Order]
GO
ALTER TABLE [dbo].[POS_Session]  WITH CHECK ADD  CONSTRAINT [FK_POS_Session_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[POS_Session] CHECK CONSTRAINT [FK_POS_Session_User]
GO
ALTER TABLE [dbo].[PriceHistory]  WITH CHECK ADD  CONSTRAINT [FK_PriceHistory_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
GO
ALTER TABLE [dbo].[PriceHistory] CHECK CONSTRAINT [FK_PriceHistory_Product]
GO
ALTER TABLE [dbo].[PriceHistory]  WITH CHECK ADD  CONSTRAINT [FK_PriceHistory_User] FOREIGN KEY([ChangedByUserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[PriceHistory] CHECK CONSTRAINT [FK_PriceHistory_User]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Brand] FOREIGN KEY([BrandId])
REFERENCES [dbo].[Brand] ([BrandId])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Brand]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([CategoryId])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Category]
GO
ALTER TABLE [dbo].[ProductImage]  WITH CHECK ADD  CONSTRAINT [FK_ProductImage_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductImage] CHECK CONSTRAINT [FK_ProductImage_Product]
GO
ALTER TABLE [dbo].[ProductImage]  WITH CHECK ADD  CONSTRAINT [FK_ProductImage_Uploader] FOREIGN KEY([UploadedByUserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[ProductImage] CHECK CONSTRAINT [FK_ProductImage_Uploader]
GO
ALTER TABLE [dbo].[ProductVariant]  WITH CHECK ADD  CONSTRAINT [FK_ProductVariant_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductVariant] CHECK CONSTRAINT [FK_ProductVariant_Product]
GO
ALTER TABLE [dbo].[PurchaseOrder]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseOrder_Supplier] FOREIGN KEY([SupplierId])
REFERENCES [dbo].[Supplier] ([SupplierId])
GO
ALTER TABLE [dbo].[PurchaseOrder] CHECK CONSTRAINT [FK_PurchaseOrder_Supplier]
GO
ALTER TABLE [dbo].[PurchaseOrder]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseOrder_User] FOREIGN KEY([CreatedByUserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[PurchaseOrder] CHECK CONSTRAINT [FK_PurchaseOrder_User]
GO
ALTER TABLE [dbo].[PurchaseOrderItem]  WITH CHECK ADD  CONSTRAINT [FK_POItem_PO] FOREIGN KEY([PurchaseOrderId])
REFERENCES [dbo].[PurchaseOrder] ([PurchaseOrderId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PurchaseOrderItem] CHECK CONSTRAINT [FK_POItem_PO]
GO
ALTER TABLE [dbo].[PurchaseOrderItem]  WITH CHECK ADD  CONSTRAINT [FK_POItem_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
GO
ALTER TABLE [dbo].[PurchaseOrderItem] CHECK CONSTRAINT [FK_POItem_Product]
GO
ALTER TABLE [dbo].[PurchaseOrderItem]  WITH CHECK ADD  CONSTRAINT [FK_POItem_Variant] FOREIGN KEY([ProductVariantId])
REFERENCES [dbo].[ProductVariant] ([ProductVariantId])
GO
ALTER TABLE [dbo].[PurchaseOrderItem] CHECK CONSTRAINT [FK_POItem_Variant]
GO
ALTER TABLE [dbo].[Review]  WITH CHECK ADD  CONSTRAINT [FK_Review_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
GO
ALTER TABLE [dbo].[Review] CHECK CONSTRAINT [FK_Review_Order]
GO
ALTER TABLE [dbo].[Review]  WITH CHECK ADD  CONSTRAINT [FK_Review_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
GO
ALTER TABLE [dbo].[Review] CHECK CONSTRAINT [FK_Review_Product]
GO
ALTER TABLE [dbo].[Review]  WITH CHECK ADD  CONSTRAINT [FK_Review_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Review] CHECK CONSTRAINT [FK_Review_User]
GO
ALTER TABLE [dbo].[SupportTask]  WITH CHECK ADD  CONSTRAINT [FK_SupportTask_AssignedTo] FOREIGN KEY([AssignedToUserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[SupportTask] CHECK CONSTRAINT [FK_SupportTask_AssignedTo]
GO
ALTER TABLE [dbo].[SupportTask]  WITH CHECK ADD  CONSTRAINT [FK_SupportTask_Ticket] FOREIGN KEY([TicketId])
REFERENCES [dbo].[SupportTicket] ([TicketId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SupportTask] CHECK CONSTRAINT [FK_SupportTask_Ticket]
GO
ALTER TABLE [dbo].[SupportTicket]  WITH CHECK ADD  CONSTRAINT [FK_Ticket_AssignedTo] FOREIGN KEY([AssignedToUserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[SupportTicket] CHECK CONSTRAINT [FK_Ticket_AssignedTo]
GO
ALTER TABLE [dbo].[SupportTicket]  WITH CHECK ADD  CONSTRAINT [FK_Ticket_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
GO
ALTER TABLE [dbo].[SupportTicket] CHECK CONSTRAINT [FK_Ticket_Order]
GO
ALTER TABLE [dbo].[SupportTicket]  WITH CHECK ADD  CONSTRAINT [FK_Ticket_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[SupportTicket] CHECK CONSTRAINT [FK_Ticket_User]
GO
ALTER TABLE [dbo].[SupportTicketReply]  WITH CHECK ADD  CONSTRAINT [FK_Reply_Ticket] FOREIGN KEY([TicketId])
REFERENCES [dbo].[SupportTicket] ([TicketId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SupportTicketReply] CHECK CONSTRAINT [FK_Reply_Ticket]
GO
ALTER TABLE [dbo].[SupportTicketReply]  WITH CHECK ADD  CONSTRAINT [FK_Reply_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[SupportTicketReply] CHECK CONSTRAINT [FK_Reply_User]
GO
ALTER TABLE [dbo].[SystemLog]  WITH CHECK ADD  CONSTRAINT [FK_SystemLog_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[SystemLog] CHECK CONSTRAINT [FK_SystemLog_User]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_DefaultAddress] FOREIGN KEY([DefaultAddressId])
REFERENCES [dbo].[Address] ([AddressId])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_DefaultAddress]
GO
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([RoleId])
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_UserRole_Role]
GO
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_UserRole_User]
GO
ALTER TABLE [dbo].[UserVoucher]  WITH CHECK ADD  CONSTRAINT [FK_UserVoucher_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[UserVoucher] CHECK CONSTRAINT [FK_UserVoucher_User]
GO
ALTER TABLE [dbo].[UserVoucher]  WITH CHECK ADD  CONSTRAINT [FK_UserVoucher_Voucher] FOREIGN KEY([VoucherId])
REFERENCES [dbo].[Voucher] ([VoucherId])
GO
ALTER TABLE [dbo].[UserVoucher] CHECK CONSTRAINT [FK_UserVoucher_Voucher]
GO
ALTER TABLE [dbo].[VoucherUsage]  WITH CHECK ADD  CONSTRAINT [FK_VoucherUsage_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
GO
ALTER TABLE [dbo].[VoucherUsage] CHECK CONSTRAINT [FK_VoucherUsage_Order]
GO
ALTER TABLE [dbo].[VoucherUsage]  WITH CHECK ADD  CONSTRAINT [FK_VoucherUsage_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[VoucherUsage] CHECK CONSTRAINT [FK_VoucherUsage_User]
GO
ALTER TABLE [dbo].[VoucherUsage]  WITH CHECK ADD  CONSTRAINT [FK_VoucherUsage_Voucher] FOREIGN KEY([VoucherId])
REFERENCES [dbo].[Voucher] ([VoucherId])
GO
ALTER TABLE [dbo].[VoucherUsage] CHECK CONSTRAINT [FK_VoucherUsage_Voucher]
GO
ALTER TABLE [dbo].[Wishlist]  WITH CHECK ADD  CONSTRAINT [FK_Wishlist_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
GO
ALTER TABLE [dbo].[Wishlist] CHECK CONSTRAINT [FK_Wishlist_Product]
GO
ALTER TABLE [dbo].[Wishlist]  WITH CHECK ADD  CONSTRAINT [FK_Wishlist_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Wishlist] CHECK CONSTRAINT [FK_Wishlist_User]
GO
ALTER TABLE [dbo].[Address]  WITH CHECK ADD  CONSTRAINT [CK_Address_Label] CHECK  (([Label]='Other' OR [Label]='Work' OR [Label]='Home'))
GO
ALTER TABLE [dbo].[Address] CHECK CONSTRAINT [CK_Address_Label]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD  CONSTRAINT [CK_Cart_Owner] CHECK  (([UserId] IS NOT NULL AND [GuestSessionId] IS NULL OR [UserId] IS NULL AND [GuestSessionId] IS NOT NULL))
GO
ALTER TABLE [dbo].[Cart] CHECK CONSTRAINT [CK_Cart_Owner]
GO
ALTER TABLE [dbo].[CartItem]  WITH CHECK ADD  CONSTRAINT [CK_CartItem_PriceAtAdd] CHECK  (([PriceAtAdd]>=(0)))
GO
ALTER TABLE [dbo].[CartItem] CHECK CONSTRAINT [CK_CartItem_PriceAtAdd]
GO
ALTER TABLE [dbo].[CartItem]  WITH CHECK ADD  CONSTRAINT [CK_CartItem_Quantity] CHECK  (([Quantity]>(0)))
GO
ALTER TABLE [dbo].[CartItem] CHECK CONSTRAINT [CK_CartItem_Quantity]
GO
ALTER TABLE [dbo].[Delivery]  WITH CHECK ADD  CONSTRAINT [CK_Delivery_Courier] CHECK  (([Courier]='LBC' OR [Courier]='Lalamove'))
GO
ALTER TABLE [dbo].[Delivery] CHECK CONSTRAINT [CK_Delivery_Courier]
GO
ALTER TABLE [dbo].[Delivery]  WITH CHECK ADD  CONSTRAINT [CK_Delivery_Status] CHECK  (([DeliveryStatus]='Failed' OR [DeliveryStatus]='Delivered' OR [DeliveryStatus]='InTransit' OR [DeliveryStatus]='PickedUp' OR [DeliveryStatus]='Pending'))
GO
ALTER TABLE [dbo].[Delivery] CHECK CONSTRAINT [CK_Delivery_Status]
GO
ALTER TABLE [dbo].[InventoryLog]  WITH CHECK ADD  CONSTRAINT [CK_InvLog_ChangeType] CHECK  (([ChangeType]='Unlock' OR [ChangeType]='Lock' OR [ChangeType]='Loss' OR [ChangeType]='Damage' OR [ChangeType]='Adjustment' OR [ChangeType]='Return' OR [ChangeType]='Sale' OR [ChangeType]='Purchase'))
GO
ALTER TABLE [dbo].[InventoryLog] CHECK CONSTRAINT [CK_InvLog_ChangeType]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [CK_Notif_Channel] CHECK  (([Channel]='InApp' OR [Channel]='Email' OR [Channel]='SMS'))
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [CK_Notif_Channel]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [CK_Notif_Status] CHECK  (([Status]='Failed' OR [Status]='Sent' OR [Status]='Pending'))
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [CK_Notif_Status]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [CK_Notif_Type] CHECK  (([NotifType]='VoucherAssigned' OR [NotifType]='OTPCode' OR [NotifType]='PendingOrderAlert' OR [NotifType]='LowStockAlert' OR [NotifType]='SupportTicketResolved' OR [NotifType]='SupportTicketReply' OR [NotifType]='SupportTicketCreated' OR [NotifType]='WishlistRestock' OR [NotifType]='DeliveryConfirmation' OR [NotifType]='DeliveryDelay' OR [NotifType]='PickupExpiry' OR [NotifType]='ReadyForPickup' OR [NotifType]='TrackingUpdate' OR [NotifType]='PaymentHeld' OR [NotifType]='PaymentRejected' OR [NotifType]='PaymentReceived' OR [NotifType]='OrderConfirmation' OR [NotifType]='WelcomeEmail'))
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [CK_Notif_Type]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [CK_Order_Discount] CHECK  (([DiscountAmount]>=(0)))
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [CK_Order_Discount]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [CK_Order_Shipping] CHECK  (([ShippingFee]>=(0)))
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [CK_Order_Shipping]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [CK_Order_Status] CHECK  (([OrderStatus]='Cancelled' OR [OrderStatus]='Delivered' OR [OrderStatus]='OutForDelivery' OR [OrderStatus]='PickedUp' OR [OrderStatus]='ReadyForPickup' OR [OrderStatus]='Processing' OR [OrderStatus]='OnHold' OR [OrderStatus]='PendingVerification' OR [OrderStatus]='Pending'))
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [CK_Order_Status]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [CK_Order_SubTotal] CHECK  (([SubTotal]>=(0)))
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [CK_Order_SubTotal]
GO
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [CK_OrderItem_Quantity] CHECK  (([Quantity]>(0)))
GO
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [CK_OrderItem_Quantity]
GO
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [CK_OrderItem_UnitPrice] CHECK  (([UnitPrice]>=(0)))
GO
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [CK_OrderItem_UnitPrice]
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [CK_Payment_Amount] CHECK  (([Amount]>=(0)))
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [CK_Payment_Amount]
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [CK_Payment_Method] CHECK  (([PaymentMethod]='Cash' OR [PaymentMethod]='BankTransfer' OR [PaymentMethod]='GCash'))
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [CK_Payment_Method]
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [CK_Payment_Stage] CHECK  (([PaymentStage]='Confirmation' OR [PaymentStage]='Upfront'))
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [CK_Payment_Stage]
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [CK_Payment_Status] CHECK  (([PaymentStatus]='Failed' OR [PaymentStatus]='Completed' OR [PaymentStatus]='VerificationRejected' OR [PaymentStatus]='VerificationPending' OR [PaymentStatus]='Pending'))
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [CK_Payment_Status]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [CK_Product_Currency] CHECK  (([Currency]='EUR' OR [Currency]='USD' OR [Currency]='PHP'))
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [CK_Product_Currency]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [CK_Product_Price] CHECK  (([Price]>=(0)))
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [CK_Product_Price]
GO
ALTER TABLE [dbo].[ProductImage]  WITH CHECK ADD  CONSTRAINT [CK_ProductImage_Order] CHECK  (([DisplayOrder]>=(0)))
GO
ALTER TABLE [dbo].[ProductImage] CHECK CONSTRAINT [CK_ProductImage_Order]
GO
ALTER TABLE [dbo].[ProductImage]  WITH CHECK ADD  CONSTRAINT [CK_ProductImage_Type] CHECK  (([ImageType]='Thumbnail' OR [ImageType]='Medium' OR [ImageType]='Full'))
GO
ALTER TABLE [dbo].[ProductImage] CHECK CONSTRAINT [CK_ProductImage_Type]
GO
ALTER TABLE [dbo].[ProductVariant]  WITH CHECK ADD  CONSTRAINT [CK_ProductVariant_AddlPrice] CHECK  (([AdditionalPrice]>=(0)))
GO
ALTER TABLE [dbo].[ProductVariant] CHECK CONSTRAINT [CK_ProductVariant_AddlPrice]
GO
ALTER TABLE [dbo].[ProductVariant]  WITH CHECK ADD  CONSTRAINT [CK_ProductVariant_ReorderThreshold] CHECK  (([ReorderThreshold]>=(0)))
GO
ALTER TABLE [dbo].[ProductVariant] CHECK CONSTRAINT [CK_ProductVariant_ReorderThreshold]
GO
ALTER TABLE [dbo].[ProductVariant]  WITH CHECK ADD  CONSTRAINT [CK_ProductVariant_StockQty] CHECK  (([StockQuantity]>=(0)))
GO
ALTER TABLE [dbo].[ProductVariant] CHECK CONSTRAINT [CK_ProductVariant_StockQty]
GO
ALTER TABLE [dbo].[PurchaseOrder]  WITH CHECK ADD  CONSTRAINT [CK_PurchaseOrder_Status] CHECK  (([Status]='Cancelled' OR [Status]='Received' OR [Status]='Pending'))
GO
ALTER TABLE [dbo].[PurchaseOrder] CHECK CONSTRAINT [CK_PurchaseOrder_Status]
GO
ALTER TABLE [dbo].[PurchaseOrderItem]  WITH CHECK ADD  CONSTRAINT [CK_POItem_Quantity] CHECK  (([Quantity]>(0)))
GO
ALTER TABLE [dbo].[PurchaseOrderItem] CHECK CONSTRAINT [CK_POItem_Quantity]
GO
ALTER TABLE [dbo].[PurchaseOrderItem]  WITH CHECK ADD  CONSTRAINT [CK_POItem_UnitPrice] CHECK  (([UnitPrice]>=(0)))
GO
ALTER TABLE [dbo].[PurchaseOrderItem] CHECK CONSTRAINT [CK_POItem_UnitPrice]
GO
ALTER TABLE [dbo].[Review]  WITH CHECK ADD  CONSTRAINT [CK_Review_Rating] CHECK  (([Rating]>=(1) AND [Rating]<=(5)))
GO
ALTER TABLE [dbo].[Review] CHECK CONSTRAINT [CK_Review_Rating]
GO
ALTER TABLE [dbo].[Role]  WITH CHECK ADD  CONSTRAINT [CK_Role_Name] CHECK  (([RoleName]='Customer' OR [RoleName]='Staff' OR [RoleName]='Cashier' OR [RoleName]='Manager' OR [RoleName]='Admin'))
GO
ALTER TABLE [dbo].[Role] CHECK CONSTRAINT [CK_Role_Name]
GO
ALTER TABLE [dbo].[SupportTask]  WITH CHECK ADD  CONSTRAINT [CK_SupportTask_Status] CHECK  (([TaskStatus]='Cancelled' OR [TaskStatus]='Done' OR [TaskStatus]='InProgress' OR [TaskStatus]='Pending'))
GO
ALTER TABLE [dbo].[SupportTask] CHECK CONSTRAINT [CK_SupportTask_Status]
GO
ALTER TABLE [dbo].[SupportTask]  WITH CHECK ADD  CONSTRAINT [CK_SupportTask_Type] CHECK  (([TaskType]='Other' OR [TaskType]='ContactSupplier' OR [TaskType]='ArrangeReturn' OR [TaskType]='ShipReplacement'))
GO
ALTER TABLE [dbo].[SupportTask] CHECK CONSTRAINT [CK_SupportTask_Type]
GO
ALTER TABLE [dbo].[SupportTicket]  WITH CHECK ADD  CONSTRAINT [CK_Ticket_Category] CHECK  (([TicketCategory]='General' OR [TicketCategory]='ProductInquiry' OR [TicketCategory]='ReturnRefund' OR [TicketCategory]='PaymentIssue' OR [TicketCategory]='DeliveryIssue' OR [TicketCategory]='WrongItem' OR [TicketCategory]='DamagedItem'))
GO
ALTER TABLE [dbo].[SupportTicket] CHECK CONSTRAINT [CK_Ticket_Category]
GO
ALTER TABLE [dbo].[SupportTicket]  WITH CHECK ADD  CONSTRAINT [CK_Ticket_Source] CHECK  (([TicketSource]='System' OR [TicketSource]='Admin' OR [TicketSource]='Customer'))
GO
ALTER TABLE [dbo].[SupportTicket] CHECK CONSTRAINT [CK_Ticket_Source]
GO
ALTER TABLE [dbo].[SupportTicket]  WITH CHECK ADD  CONSTRAINT [CK_Ticket_Status] CHECK  (([TicketStatus]='Closed' OR [TicketStatus]='Resolved' OR [TicketStatus]='AwaitingResponse' OR [TicketStatus]='InProgress' OR [TicketStatus]='Open'))
GO
ALTER TABLE [dbo].[SupportTicket] CHECK CONSTRAINT [CK_Ticket_Status]
GO
ALTER TABLE [dbo].[SystemLog]  WITH CHECK ADD  CONSTRAINT [CK_SystemLog_Event] CHECK  (([EventType]='SupportTicketResolved' OR [EventType]='SupportTicketCreated' OR [EventType]='BackgroundJobError' OR [EventType]='BackgroundJobComplete' OR [EventType]='BackgroundJobStart' OR [EventType]='DeliveryFailed' OR [EventType]='DeliveryDelayed' OR [EventType]='DeliveryStatusPoll' OR [EventType]='LowStockTriggered' OR [EventType]='InventorySync' OR [EventType]='InventoryAdjustment' OR [EventType]='PaymentTimeout' OR [EventType]='PaymentRejected' OR [EventType]='PaymentVerified' OR [EventType]='PaymentProcessed' OR [EventType]='OrderStatusChange' OR [EventType]='VoucherCreated' OR [EventType]='ProductUpdate' OR [EventType]='UserCreated' OR [EventType]='AccessDenied' OR [EventType]='Logout' OR [EventType]='Login'))
GO
ALTER TABLE [dbo].[SystemLog] CHECK CONSTRAINT [CK_SystemLog_Event]
GO
ALTER TABLE [dbo].[Voucher]  WITH CHECK ADD  CONSTRAINT [CK_Voucher_DiscountType] CHECK  (([DiscountType]='Fixed' OR [DiscountType]='Percentage'))
GO
ALTER TABLE [dbo].[Voucher] CHECK CONSTRAINT [CK_Voucher_DiscountType]
GO
ALTER TABLE [dbo].[Voucher]  WITH CHECK ADD  CONSTRAINT [CK_Voucher_DiscountVal] CHECK  (([DiscountValue]>(0)))
GO
ALTER TABLE [dbo].[Voucher] CHECK CONSTRAINT [CK_Voucher_DiscountVal]
GO
ALTER TABLE [dbo].[Voucher]  WITH CHECK ADD  CONSTRAINT [CK_Voucher_MaxPerUser] CHECK  (([MaxUsesPerUser] IS NULL OR [MaxUsesPerUser]>(0)))
GO
ALTER TABLE [dbo].[Voucher] CHECK CONSTRAINT [CK_Voucher_MaxPerUser]
GO
ALTER TABLE [dbo].[Voucher]  WITH CHECK ADD  CONSTRAINT [CK_Voucher_MaxUses] CHECK  (([MaxUses] IS NULL OR [MaxUses]>(0)))
GO
ALTER TABLE [dbo].[Voucher] CHECK CONSTRAINT [CK_Voucher_MaxUses]
GO
ALTER TABLE [dbo].[Voucher]  WITH CHECK ADD  CONSTRAINT [CK_Voucher_MinOrderAmt] CHECK  (([MinimumOrderAmount] IS NULL OR [MinimumOrderAmount]>=(0)))
GO
ALTER TABLE [dbo].[Voucher] CHECK CONSTRAINT [CK_Voucher_MinOrderAmt]
GO
ALTER TABLE [dbo].[VoucherUsage]  WITH CHECK ADD  CONSTRAINT [CK_VoucherUsage_Disc] CHECK  (([DiscountAmount]>=(0)))
GO
ALTER TABLE [dbo].[VoucherUsage] CHECK CONSTRAINT [CK_VoucherUsage_Disc]
GO
USE [master]
GO
ALTER DATABASE [Taurus-bike-shop-sqlserver-2026] SET  READ_WRITE 
GO
