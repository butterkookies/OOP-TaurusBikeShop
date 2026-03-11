-- =============================================================================
-- TAURUS BIKE SHOP - DATABASE SCHEMA
-- Compatible with: Microsoft SQL Server
-- Description: Normalized product catalog schema for bicycle parts & complete units
-- =============================================================================

USE TaurusBikeShop;
GO

-- =============================================================================
-- TABLE: Categories
-- Stores bicycle part/product categories
-- =============================================================================
CREATE TABLE Categories (
    CategoryID      INT             NOT NULL IDENTITY(1,1),
    CategoryCode    VARCHAR(20)     NOT NULL,
    CategoryName    NVARCHAR(100)   NOT NULL,
    Description     NVARCHAR(500)   NULL,
    DisplayOrder    INT             NOT NULL DEFAULT 0,
    IsActive        BIT             NOT NULL DEFAULT 1,
    CreatedAt       DATETIME2       NOT NULL DEFAULT GETDATE(),
    UpdatedAt       DATETIME2       NOT NULL DEFAULT GETDATE(),

    CONSTRAINT PK_Categories           PRIMARY KEY (CategoryID),
    CONSTRAINT UQ_Categories_Code      UNIQUE (CategoryCode),
    CONSTRAINT UQ_Categories_Name      UNIQUE (CategoryName)
);
GO

-- =============================================================================
-- TABLE: Brands
-- Stores product brands / manufacturers
-- =============================================================================
CREATE TABLE Brands (
    BrandID         INT             NOT NULL IDENTITY(1,1),
    BrandName       NVARCHAR(100)   NOT NULL,
    Country         NVARCHAR(100)   NULL,
    Website         NVARCHAR(255)   NULL,
    Description     NVARCHAR(500)   NULL,
    IsActive        BIT             NOT NULL DEFAULT 1,
    CreatedAt       DATETIME2       NOT NULL DEFAULT GETDATE(),
    UpdatedAt       DATETIME2       NOT NULL DEFAULT GETDATE(),

    CONSTRAINT PK_Brands              PRIMARY KEY (BrandID),
    CONSTRAINT UQ_Brands_Name         UNIQUE (BrandName)
);
GO

-- =============================================================================
-- TABLE: Products
-- Core product catalog enriched with attributes sourced from research
-- =============================================================================
CREATE TABLE Products (
    ProductID               INT             NOT NULL IDENTITY(1,1),
    CategoryID              INT             NOT NULL,
    BrandID                 INT             NOT NULL,
    SKU                     VARCHAR(50)     NOT NULL,
    ProductName             NVARCHAR(200)   NOT NULL,
    ShortDescription        NVARCHAR(300)   NULL,
    FullDescription         NVARCHAR(2000)  NULL,

    -- Physical attributes
    Material                NVARCHAR(100)   NULL,
    Color                   NVARCHAR(100)   NULL,
    WeightGrams             DECIMAL(8,2)    NULL,

    -- Bicycle-specific attributes
    WheelSize               NVARCHAR(20)    NULL,   -- e.g. '27.5"', '29"', '700c'
    SpeedCompatibility      NVARCHAR(50)    NULL,   -- e.g. '9-speed', '11-speed', 'Universal'
    BoostCompatible         BIT             NULL,   -- NULL = N/A, 1 = Yes, 0 = No
    TubelessReady           BIT             NULL,
    AxleStandard            NVARCHAR(50)    NULL,   -- e.g. 'QR', 'Thru-Axle 12x142mm'
    FrameMaterial           NVARCHAR(100)   NULL,   -- Relevant for complete bikes & frames
    SuspensionTravel        NVARCHAR(50)    NULL,   -- e.g. '100mm', '120mm' (for forks)
    BrakeType               NVARCHAR(100)   NULL,   -- e.g. 'Hydraulic Disc', 'Rim Brake'
    AdditionalSpecs         NVARCHAR(1000)  NULL,   -- Free-text for extra spec details

    -- Pricing & status
    Price                   DECIMAL(10,2)   NOT NULL,
    Currency                CHAR(3)         NOT NULL DEFAULT 'PHP',
    IsActive                BIT             NOT NULL DEFAULT 1,
    StockQuantity           INT             NOT NULL DEFAULT 0,

    CreatedAt               DATETIME2       NOT NULL DEFAULT GETDATE(),
    UpdatedAt               DATETIME2       NOT NULL DEFAULT GETDATE(),

    CONSTRAINT PK_Products              PRIMARY KEY (ProductID),
    CONSTRAINT UQ_Products_SKU          UNIQUE (SKU),
    CONSTRAINT FK_Products_Categories   FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID),
    CONSTRAINT FK_Products_Brands       FOREIGN KEY (BrandID)    REFERENCES Brands(BrandID),
    CONSTRAINT CK_Products_Price        CHECK (Price >= 0),
    CONSTRAINT CK_Products_Stock        CHECK (StockQuantity >= 0),
    CONSTRAINT CK_Products_Currency     CHECK (Currency IN ('PHP','USD','EUR'))
);
GO

-- =============================================================================
-- TABLE: PriceHistory
-- Audit trail of price changes per product
-- =============================================================================
CREATE TABLE PriceHistory (
    PriceHistoryID  INT             NOT NULL IDENTITY(1,1),
    ProductID       INT             NOT NULL,
    OldPrice        DECIMAL(10,2)   NOT NULL,
    NewPrice        DECIMAL(10,2)   NOT NULL,
    ChangedAt       DATETIME2       NOT NULL DEFAULT GETDATE(),
    ChangedBy       NVARCHAR(100)   NULL,
    Notes           NVARCHAR(500)   NULL,

    CONSTRAINT PK_PriceHistory              PRIMARY KEY (PriceHistoryID),
    CONSTRAINT FK_PriceHistory_Products     FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);
GO

-- =============================================================================
-- TABLE: ProductCompatibility
-- Cross-references which products are compatible with each other
-- e.g., an upgrade kit compatible with specific frames
-- =============================================================================
CREATE TABLE ProductCompatibility (
    CompatibilityID     INT             NOT NULL IDENTITY(1,1),
    ProductID           INT             NOT NULL,
    CompatibleProductID INT             NOT NULL,
    Notes               NVARCHAR(300)   NULL,

    CONSTRAINT PK_ProductCompatibility              PRIMARY KEY (CompatibilityID),
    CONSTRAINT FK_ProductCompat_Product             FOREIGN KEY (ProductID)            REFERENCES Products(ProductID),
    CONSTRAINT FK_ProductCompat_CompatProduct       FOREIGN KEY (CompatibleProductID)  REFERENCES Products(ProductID),
    CONSTRAINT UQ_ProductCompatibility              UNIQUE (ProductID, CompatibleProductID),
    CONSTRAINT CK_ProductCompat_NoSelfRef           CHECK (ProductID <> CompatibleProductID)
);
GO

-- =============================================================================
-- INDEXES
-- =============================================================================

-- Products: filter by category and active status
CREATE NONCLUSTERED INDEX IX_Products_CategoryID
    ON Products (CategoryID, IsActive)
    INCLUDE (ProductName, Price, SKU);
GO

-- Products: filter by brand
CREATE NONCLUSTERED INDEX IX_Products_BrandID
    ON Products (BrandID, IsActive)
    INCLUDE (ProductName, Price);
GO

-- Products: price range queries
CREATE NONCLUSTERED INDEX IX_Products_Price
    ON Products (Price, IsActive);
GO

-- PriceHistory: lookup history by product
CREATE NONCLUSTERED INDEX IX_PriceHistory_ProductID
    ON PriceHistory (ProductID, ChangedAt DESC);
GO

-- =============================================================================
-- TRIGGER: Auto-update UpdatedAt on Products
-- =============================================================================
CREATE OR ALTER TRIGGER TR_Products_UpdatedAt
ON Products
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE p
    SET    p.UpdatedAt = GETDATE()
    FROM   Products p
    INNER JOIN inserted i ON p.ProductID = i.ProductID;
END;
GO

-- =============================================================================
-- TRIGGER: Auto-log price changes into PriceHistory
-- =============================================================================
CREATE OR ALTER TRIGGER TR_Products_PriceAudit
ON Products
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO PriceHistory (ProductID, OldPrice, NewPrice, ChangedAt, Notes)
    SELECT
        i.ProductID,
        d.Price,
        i.Price,
        GETDATE(),
        'Automatic price change audit'
    FROM inserted  i
    INNER JOIN deleted d ON i.ProductID = d.ProductID
    WHERE i.Price <> d.Price;
END;
GO
