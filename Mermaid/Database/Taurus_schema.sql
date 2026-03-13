-- =============================================================================
-- Taurus Bike Shop  |  TaurusBikeShopDB
-- File   : Taurus_schema.sql
-- Purpose: Create all tables, indexes, views, and triggers
-- MSSQL  : SQL Server 2016+ (FreeASPHosting.net compatible)
--
-- HOW TO RUN:
--   1. Open SSMS (or your hosting control panel query editor).
--   2. In the database dropdown select your database
--      (currently named: butterk00kies_SampleDB).
--   3. Execute this script — it will create all objects inside
--      whichever database you are connected to.
--
-- DATABASE RENAME NOTE:
--   FreeASPHosting.net assigns a default database name
--   (e.g. butterk00kies_SampleDB) and shared hosting accounts
--   do NOT have the ALTER ANY DATABASE permission needed to
--   rename it via SQL. To get your database renamed to
--   "TaurusBikeShopDB", contact FreeASPHosting.net support
--   directly and ask them to rename it for you.
--   The SQL command that a DBA with sysadmin would run is:
--     ALTER DATABASE [butterk00kies_SampleDB]
--         MODIFY NAME = [TaurusBikeShopDB];
-- =============================================================================
SET NOCOUNT ON;
GO

-- =============================================
-- Taurus Bike Shop Online Ordering System
-- Database Creation Script (SQL Server)
-- Version: 3.1 - Delivery & Payment aligned to business rules
-- Changes from v3.0:
--   Payment table:
--     + PaymentMethod: removed 'Cash'/'Card'; now 'GCash','BankTransfer','Cash'
--       (Cash is retained for walk-in POS only; deliveries require GCash or BankTransfer)
--     + PaymentStage: new column — 'Upfront' (paid before dispatch) or
--       'Confirmation' (confirmed on delivery arrival); enforces payment-first policy
--     + BpiReferenceNumber: new column for BPI bank transfer reference numbers
--       (distinct from GCash TransactionId)
--   Delivery table:
--     + Courier: new column — 'Lalamove' or 'LBC'
--       (Lalamove = within Bulacan & Manila; LBC = all other areas)
--     + LalamoveBookingRef: renamed from LalamoveReference, nullable
--     + LbcTrackingNumber: new column for LBC shipment tracking, nullable
--     + DriverName / DriverPhone: now Lalamove-specific; NULL for LBC shipments
-- =============================================


-- =============================================
-- TABLE: User
-- =============================================
CREATE TABLE [User] (
    UserId          INT             NOT NULL IDENTITY(1,1),
    Email           NVARCHAR(255)   NULL,
    PasswordHash    NVARCHAR(255)   NULL,
    FirstName       NVARCHAR(100)   NOT NULL,
    LastName        NVARCHAR(100)   NOT NULL,
    PhoneNumber     NVARCHAR(20)    NULL,
    [Address]       NVARCHAR(500)   NULL,
    City            NVARCHAR(100)   NULL,
    PostalCode      NVARCHAR(20)    NULL,
    IsActive        BIT             NOT NULL DEFAULT 1,
    IsWalkIn        BIT             NOT NULL DEFAULT 0,
    CreatedAt       DATETIME        NOT NULL DEFAULT GETDATE(),
    LastLoginAt     DATETIME        NULL,
    CONSTRAINT PK_User PRIMARY KEY (UserId)
);
GO

CREATE UNIQUE INDEX UX_User_Email       ON [User](Email)        WHERE Email IS NOT NULL;
CREATE INDEX IX_User_PhoneNumber        ON [User](PhoneNumber);
CREATE INDEX IX_User_IsActive           ON [User](IsActive);
CREATE INDEX IX_User_IsWalkIn           ON [User](IsWalkIn);
GO

-- =============================================
-- TABLE: Role
-- =============================================
CREATE TABLE [Role] (
    RoleId          INT             NOT NULL IDENTITY(1,1),
    RoleName        NVARCHAR(50)    NOT NULL UNIQUE,
    [Description]   NVARCHAR(255)   NULL,
    CreatedAt       DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Role      PRIMARY KEY (RoleId),
    CONSTRAINT CK_Role_Name CHECK (RoleName IN
        ('Admin','Manager','Cashier','Staff','Customer'))
);
GO

CREATE INDEX IX_Role_RoleName ON [Role](RoleName);
GO

-- =============================================
-- TABLE: UserRole
-- =============================================
CREATE TABLE UserRole (
    UserRoleId  INT      NOT NULL IDENTITY(1,1),
    UserId      INT      NOT NULL,
    RoleId      INT      NOT NULL,
    AssignedAt  DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_UserRole              PRIMARY KEY (UserRoleId),
    CONSTRAINT FK_UserRole_User         FOREIGN KEY (UserId) REFERENCES [User](UserId)   ON DELETE CASCADE,
    CONSTRAINT FK_UserRole_Role         FOREIGN KEY (RoleId) REFERENCES [Role](RoleId),
    CONSTRAINT UX_UserRole_User_Role    UNIQUE (UserId, RoleId)
);
GO

CREATE INDEX IX_UserRole_UserId ON UserRole(UserId);
CREATE INDEX IX_UserRole_RoleId ON UserRole(RoleId);
GO

-- =============================================
-- TABLE: Category
-- Extended: added CategoryCode column (max 20 chars).
-- CategoryCode matches the Excel category headers:
--   UNIT, FRAME, FORK, HUB, UPGKIT, STEM,
--   HBAR, SADDLE, GRIP, PEDAL, RIM, TIRE, CHAIN
-- =============================================
CREATE TABLE Category (
    CategoryId       INT            NOT NULL IDENTITY(1,1),
    CategoryCode     VARCHAR(20)    NOT NULL,   -- << NEW: short code, e.g. 'UNIT', 'FRAME'
    [Name]           NVARCHAR(100)  NOT NULL,
    [Description]    NVARCHAR(500)  NULL,
    ParentCategoryId INT            NULL,
    IsActive         BIT            NOT NULL DEFAULT 1,
    DisplayOrder     INT            NOT NULL DEFAULT 0,
    CONSTRAINT PK_Category          PRIMARY KEY (CategoryId),
    CONSTRAINT UQ_Category_Code     UNIQUE (CategoryCode),
    CONSTRAINT FK_Category_Parent   FOREIGN KEY (ParentCategoryId) REFERENCES Category(CategoryId)
);
GO

CREATE INDEX IX_Category_ParentCategoryId ON Category(ParentCategoryId);
CREATE INDEX IX_Category_IsActive         ON Category(IsActive);
CREATE INDEX IX_Category_DisplayOrder     ON Category(DisplayOrder);
GO

-- =============================================
-- TABLE: Brand  (NEW in v3.0)
-- Captures the manufacturer/brand for every product
-- derived from product names in PARTS-PRICES.xlsx.
-- =============================================
CREATE TABLE Brand (
    BrandId     INT            NOT NULL IDENTITY(1,1),
    BrandName   NVARCHAR(100)  NOT NULL,
    Country     NVARCHAR(100)  NULL,
    Website     NVARCHAR(255)  NULL,
    [Description] NVARCHAR(500) NULL,
    IsActive    BIT            NOT NULL DEFAULT 1,
    CreatedAt   DATETIME       NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Brand         PRIMARY KEY (BrandId),
    CONSTRAINT UQ_Brand_Name    UNIQUE (BrandName)
);
GO

CREATE INDEX IX_Brand_IsActive ON Brand(IsActive);
GO

-- =============================================
-- TABLE: Product
-- Extended with 12 bicycle-specific attributes
-- sourced from PARTS-PRICES.xlsx and web research.
-- =============================================
CREATE TABLE Product (
    ProductId           INT             NOT NULL IDENTITY(1,1),
    CategoryId          INT             NOT NULL,
    BrandId             INT             NULL,       -- << NEW: FK to Brand
    SKU                 VARCHAR(50)     NULL,       -- << NEW: e.g. 'UNIT-PIN-001'
    [Name]              NVARCHAR(200)   NOT NULL,
    ShortDescription    NVARCHAR(300)   NULL,       -- << NEW: one-liner for cards/lists
    [Description]       NVARCHAR(MAX)   NULL,
    -- Pricing
    Price               DECIMAL(10,2)   NOT NULL,
    Currency            CHAR(3)         NOT NULL DEFAULT 'PHP',  -- << NEW
    -- Inventory
    StockQuantity       INT             NOT NULL DEFAULT 0,
    -- Bicycle-specific attributes (all NEW in v3.0)
    Material            NVARCHAR(100)   NULL,   -- e.g. 'Carbon Fiber T800'
    Color               NVARCHAR(100)   NULL,
    WheelSize           NVARCHAR(20)    NULL,   -- e.g. '27.5"', '29"', '700c'
    SpeedCompatibility  NVARCHAR(50)    NULL,   -- e.g. '12-speed', 'Universal'
    BoostCompatible     BIT             NULL,   -- NULL = not applicable
    TubelessReady       BIT             NULL,
    AxleStandard        NVARCHAR(50)    NULL,   -- e.g. 'QR 9x135mm', 'TA 12x148mm'
    SuspensionTravel    NVARCHAR(50)    NULL,   -- e.g. '120mm' (forks only)
    BrakeType           NVARCHAR(100)   NULL,   -- e.g. 'Hydraulic Disc'
    AdditionalSpecs     NVARCHAR(1000)  NULL,   -- free-text spec dump
    -- Status flags
    IsActive            BIT             NOT NULL DEFAULT 1,
    IsFeatured          BIT             NOT NULL DEFAULT 0,
    CreatedAt           DATETIME        NOT NULL DEFAULT GETDATE(),
    UpdatedAt           DATETIME        NULL,
    CONSTRAINT PK_Product               PRIMARY KEY (ProductId),
    CONSTRAINT FK_Product_Category      FOREIGN KEY (CategoryId)  REFERENCES Category(CategoryId),
    CONSTRAINT FK_Product_Brand         FOREIGN KEY (BrandId)     REFERENCES Brand(BrandId),
    CONSTRAINT CK_Product_Price         CHECK (Price >= 0),
    CONSTRAINT CK_Product_StockQty     CHECK (StockQuantity >= 0),
    CONSTRAINT CK_Product_Currency      CHECK (Currency IN ('PHP','USD','EUR'))
);
GO

CREATE INDEX IX_Product_CategoryId ON Product(CategoryId);
CREATE INDEX IX_Product_BrandId    ON Product(BrandId);
CREATE INDEX IX_Product_IsActive   ON Product(IsActive);
CREATE INDEX IX_Product_IsFeatured ON Product(IsFeatured);
CREATE INDEX IX_Product_Name       ON Product([Name]);
CREATE INDEX IX_Product_Price      ON Product(Price, IsActive);
-- SKU is nullable: a standard UNIQUE constraint only permits one NULL row.
-- A filtered unique index skips NULLs entirely, allowing unlimited un-SKU'd products.
CREATE UNIQUE INDEX UX_Product_SKU ON Product(SKU) WHERE SKU IS NOT NULL;
GO

-- =============================================
-- TABLE: PriceHistory  (NEW in v3.0)
-- Audit trail of every price change on a product.
-- =============================================
CREATE TABLE PriceHistory (
    PriceHistoryId  INT             NOT NULL IDENTITY(1,1),
    ProductId       INT             NOT NULL,
    OldPrice        DECIMAL(10,2)   NOT NULL,
    NewPrice        DECIMAL(10,2)   NOT NULL,
    ChangedAt       DATETIME        NOT NULL DEFAULT GETDATE(),
    ChangedByUserId INT             NULL,
    Notes           NVARCHAR(500)   NULL,
    CONSTRAINT PK_PriceHistory              PRIMARY KEY (PriceHistoryId),
    CONSTRAINT FK_PriceHistory_Product      FOREIGN KEY (ProductId)       REFERENCES Product(ProductId),
    CONSTRAINT FK_PriceHistory_User         FOREIGN KEY (ChangedByUserId) REFERENCES [User](UserId)
);
GO

CREATE INDEX IX_PriceHistory_ProductId  ON PriceHistory(ProductId, ChangedAt DESC);
CREATE INDEX IX_PriceHistory_ChangedAt  ON PriceHistory(ChangedAt);
GO

-- Auto-populate PriceHistory whenever Product.Price changes
IF OBJECT_ID('TR_Product_PriceAudit', 'TR') IS NOT NULL
    DROP TRIGGER TR_Product_PriceAudit;
GO

CREATE TRIGGER TR_Product_PriceAudit
ON Product
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO PriceHistory (ProductId, OldPrice, NewPrice, Notes)
    SELECT i.ProductId, d.Price, i.Price, 'Automatic price change audit'
    FROM inserted i
    INNER JOIN deleted d ON i.ProductId = d.ProductId
    WHERE i.Price <> d.Price;
END;
GO

-- =============================================
-- TABLE: ProductVariant
-- =============================================
CREATE TABLE ProductVariant (
    ProductVariantId    INT             NOT NULL IDENTITY(1,1),
    ProductId           INT             NOT NULL,
    VariantName         NVARCHAR(100)   NOT NULL,
    SKU                 NVARCHAR(50)    NULL,
    AdditionalPrice     DECIMAL(10,2)   NOT NULL DEFAULT 0,
    StockQuantity       INT             NOT NULL DEFAULT 0,
    IsActive            BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME        NOT NULL DEFAULT GETDATE(),
    UpdatedAt           DATETIME        NULL,
    CONSTRAINT PK_ProductVariant            PRIMARY KEY (ProductVariantId),
    CONSTRAINT FK_ProductVariant_Product    FOREIGN KEY (ProductId) REFERENCES Product(ProductId) ON DELETE CASCADE,
    CONSTRAINT CK_ProductVariant_StockQty  CHECK (StockQuantity >= 0),
    CONSTRAINT CK_ProductVariant_AddlPrice CHECK (AdditionalPrice >= 0)
);
GO

CREATE INDEX IX_ProductVariant_ProductId ON ProductVariant(ProductId);
CREATE INDEX IX_ProductVariant_IsActive  ON ProductVariant(IsActive);
CREATE INDEX IX_ProductVariant_SKU       ON ProductVariant(SKU);
GO

-- =============================================
-- TABLE: ProductImage
-- =============================================
CREATE TABLE ProductImage (
    ProductImageId  INT              NOT NULL IDENTITY(1,1),
    ProductId       INT              NOT NULL,
    ImageUrl        NVARCHAR(1000)   NOT NULL,
    ImageType       NVARCHAR(50)     NOT NULL,
    IsPrimary       BIT              NOT NULL DEFAULT 0,
    DisplayOrder    INT              NOT NULL DEFAULT 0,
    AltText         NVARCHAR(200)    NULL,
    CreatedAt       DATETIME         NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_ProductImage          PRIMARY KEY (ProductImageId),
    CONSTRAINT FK_ProductImage_Product  FOREIGN KEY (ProductId) REFERENCES Product(ProductId) ON DELETE CASCADE,
    CONSTRAINT CK_ProductImage_Type     CHECK (ImageType IN ('Full','Medium','Thumbnail')),
    CONSTRAINT CK_ProductImage_Order    CHECK (DisplayOrder >= 0)
);
GO

CREATE UNIQUE INDEX UX_ProductImage_Primary ON ProductImage(ProductId) WHERE IsPrimary = 1;
CREATE INDEX IX_ProductImage_ProductId      ON ProductImage(ProductId);
CREATE INDEX IX_ProductImage_ImageType      ON ProductImage(ImageType);
CREATE INDEX IX_ProductImage_IsPrimary      ON ProductImage(IsPrimary);
CREATE INDEX IX_ProductImage_DisplayOrder   ON ProductImage(DisplayOrder);
GO

-- =============================================
-- TABLE: Supplier
-- =============================================
CREATE TABLE Supplier (
    SupplierId      INT             NOT NULL IDENTITY(1,1),
    [Name]          NVARCHAR(200)   NOT NULL,
    ContactPerson   NVARCHAR(100)   NULL,
    PhoneNumber     NVARCHAR(20)    NULL,
    Email           NVARCHAR(255)   NULL,
    [Address]       NVARCHAR(500)   NULL,
    IsActive        BIT             NOT NULL DEFAULT 1,
    CreatedAt       DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Supplier PRIMARY KEY (SupplierId)
);
GO

CREATE INDEX IX_Supplier_IsActive       ON Supplier(IsActive);
CREATE UNIQUE INDEX UX_Supplier_Email   ON Supplier(Email) WHERE Email IS NOT NULL;
GO

-- =============================================
-- TABLE: PurchaseOrder
-- =============================================
CREATE TABLE PurchaseOrder (
    PurchaseOrderId         INT             NOT NULL IDENTITY(1,1),
    SupplierId              INT             NOT NULL,
    OrderDate               DATETIME        NOT NULL DEFAULT GETDATE(),
    ExpectedDeliveryDate    DATETIME        NULL,
    [Status]                NVARCHAR(50)    NOT NULL DEFAULT 'Pending',
    TotalAmount             DECIMAL(10,2)   NOT NULL DEFAULT 0,
    CreatedByUserId         INT             NULL,
    CreatedAt               DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_PurchaseOrder             PRIMARY KEY (PurchaseOrderId),
    CONSTRAINT FK_PurchaseOrder_Supplier    FOREIGN KEY (SupplierId)        REFERENCES Supplier(SupplierId),
    CONSTRAINT FK_PurchaseOrder_User        FOREIGN KEY (CreatedByUserId)   REFERENCES [User](UserId),
    CONSTRAINT CK_PurchaseOrder_Status      CHECK ([Status] IN ('Pending','Received','Cancelled')),
    CONSTRAINT CK_PurchaseOrder_Total       CHECK (TotalAmount >= 0)
);
GO

CREATE INDEX IX_PurchaseOrder_SupplierId        ON PurchaseOrder(SupplierId);
CREATE INDEX IX_PurchaseOrder_Status            ON PurchaseOrder([Status]);
CREATE INDEX IX_PurchaseOrder_OrderDate         ON PurchaseOrder(OrderDate);
CREATE INDEX IX_PurchaseOrder_CreatedByUserId   ON PurchaseOrder(CreatedByUserId);
GO

-- =============================================
-- TABLE: PurchaseOrderItem
-- =============================================
CREATE TABLE PurchaseOrderItem (
    PurchaseOrderItemId INT             NOT NULL IDENTITY(1,1),
    PurchaseOrderId     INT             NOT NULL,
    ProductId           INT             NOT NULL,
    ProductVariantId    INT             NULL,
    Quantity            INT             NOT NULL,
    UnitPrice           DECIMAL(10,2)   NOT NULL,
    Subtotal            DECIMAL(10,2)   NOT NULL,
    CONSTRAINT PK_PurchaseOrderItem             PRIMARY KEY (PurchaseOrderItemId),
    CONSTRAINT FK_POItem_PO                     FOREIGN KEY (PurchaseOrderId)   REFERENCES PurchaseOrder(PurchaseOrderId) ON DELETE CASCADE,
    CONSTRAINT FK_POItem_Product                FOREIGN KEY (ProductId)         REFERENCES Product(ProductId),
    CONSTRAINT FK_POItem_Variant                FOREIGN KEY (ProductVariantId)  REFERENCES ProductVariant(ProductVariantId),
    CONSTRAINT CK_POItem_Quantity               CHECK (Quantity > 0),
    CONSTRAINT CK_POItem_UnitPrice              CHECK (UnitPrice >= 0),
    CONSTRAINT CK_POItem_Subtotal               CHECK (Subtotal >= 0)
);
GO

CREATE INDEX IX_POItem_PurchaseOrderId      ON PurchaseOrderItem(PurchaseOrderId);
CREATE INDEX IX_POItem_ProductId            ON PurchaseOrderItem(ProductId);
CREATE INDEX IX_POItem_ProductVariantId     ON PurchaseOrderItem(ProductVariantId);
GO

-- =============================================
-- TABLE: Voucher
-- =============================================
CREATE TABLE Voucher (
    VoucherId           INT             NOT NULL IDENTITY(1,1),
    Code                NVARCHAR(50)    NOT NULL UNIQUE,
    [Description]       NVARCHAR(500)   NULL,
    DiscountType        NVARCHAR(20)    NOT NULL,
    DiscountValue       DECIMAL(10,2)   NOT NULL,
    MinimumOrderAmount  DECIMAL(10,2)   NULL,
    MaxUses             INT             NULL,
    MaxUsesPerUser      INT             NULL,
    StartDate           DATETIME        NOT NULL,
    EndDate             DATETIME        NULL,
    IsActive            BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Voucher                   PRIMARY KEY (VoucherId),
    CONSTRAINT CK_Voucher_DiscountType      CHECK (DiscountType IN ('Percentage','Fixed')),
    CONSTRAINT CK_Voucher_DiscountValue     CHECK (DiscountValue > 0),
    CONSTRAINT CK_Voucher_MinOrderAmt       CHECK (MinimumOrderAmount >= 0 OR MinimumOrderAmount IS NULL),
    CONSTRAINT CK_Voucher_MaxUses           CHECK (MaxUses > 0 OR MaxUses IS NULL),
    CONSTRAINT CK_Voucher_MaxPerUser        CHECK (MaxUsesPerUser > 0 OR MaxUsesPerUser IS NULL)
);
GO

CREATE INDEX IX_Voucher_Code        ON Voucher(Code);
CREATE INDEX IX_Voucher_IsActive    ON Voucher(IsActive);
CREATE INDEX IX_Voucher_StartDate   ON Voucher(StartDate);
CREATE INDEX IX_Voucher_EndDate     ON Voucher(EndDate);
GO

-- =============================================
-- TABLE: UserVoucher
-- =============================================
CREATE TABLE UserVoucher (
    UserVoucherId   INT      NOT NULL IDENTITY(1,1),
    UserId          INT      NOT NULL,
    VoucherId       INT      NOT NULL,
    AssignedAt      DATETIME NOT NULL DEFAULT GETDATE(),
    ExpiresAt       DATETIME NULL,
    CONSTRAINT PK_UserVoucher               PRIMARY KEY (UserVoucherId),
    CONSTRAINT FK_UserVoucher_User          FOREIGN KEY (UserId)     REFERENCES [User](UserId),
    CONSTRAINT FK_UserVoucher_Voucher       FOREIGN KEY (VoucherId)  REFERENCES Voucher(VoucherId),
    CONSTRAINT UX_UserVoucher_User_Voucher  UNIQUE (UserId, VoucherId)
);
GO

CREATE INDEX IX_UserVoucher_UserId      ON UserVoucher(UserId);
CREATE INDEX IX_UserVoucher_VoucherId   ON UserVoucher(VoucherId);
GO

-- =============================================
-- TABLE: Cart
-- =============================================
CREATE TABLE Cart (
    CartId          INT      NOT NULL IDENTITY(1,1),
    UserId          INT      NOT NULL,
    CreatedAt       DATETIME NOT NULL DEFAULT GETDATE(),
    LastUpdatedAt   DATETIME NULL,
    IsCheckedOut    BIT      NOT NULL DEFAULT 0,
    CONSTRAINT PK_Cart          PRIMARY KEY (CartId),
    CONSTRAINT FK_Cart_User     FOREIGN KEY (UserId) REFERENCES [User](UserId)
);
GO

CREATE UNIQUE INDEX UX_Cart_User_Active ON Cart(UserId) WHERE IsCheckedOut = 0;
CREATE INDEX IX_Cart_UserId             ON Cart(UserId);
CREATE INDEX IX_Cart_IsCheckedOut       ON Cart(IsCheckedOut);
GO

-- =============================================
-- TABLE: CartItem
-- =============================================
CREATE TABLE CartItem (
    CartItemId          INT             NOT NULL IDENTITY(1,1),
    CartId              INT             NOT NULL,
    ProductId           INT             NOT NULL,
    ProductVariantId    INT             NULL,
    Quantity            INT             NOT NULL,
    PriceAtAdd          DECIMAL(10,2)   NOT NULL,
    AddedAt             DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_CartItem              PRIMARY KEY (CartItemId),
    CONSTRAINT FK_CartItem_Cart         FOREIGN KEY (CartId)            REFERENCES Cart(CartId)                         ON DELETE CASCADE,
    CONSTRAINT FK_CartItem_Product      FOREIGN KEY (ProductId)         REFERENCES Product(ProductId),
    CONSTRAINT FK_CartItem_Variant      FOREIGN KEY (ProductVariantId)  REFERENCES ProductVariant(ProductVariantId),
    CONSTRAINT CK_CartItem_Quantity     CHECK (Quantity > 0),
    CONSTRAINT CK_CartItem_PriceAtAdd   CHECK (PriceAtAdd >= 0)
);
GO

CREATE INDEX IX_CartItem_CartId             ON CartItem(CartId);
CREATE INDEX IX_CartItem_ProductId          ON CartItem(ProductId);
CREATE INDEX IX_CartItem_ProductVariantId   ON CartItem(ProductVariantId);
GO

-- =============================================
-- TABLE: Wishlist
-- =============================================
CREATE TABLE Wishlist (
    WishlistId  INT      NOT NULL IDENTITY(1,1),
    UserId      INT      NOT NULL,
    ProductId   INT      NOT NULL,
    AddedAt     DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Wishlist                  PRIMARY KEY (WishlistId),
    CONSTRAINT FK_Wishlist_User             FOREIGN KEY (UserId)    REFERENCES [User](UserId)       ON DELETE CASCADE,
    CONSTRAINT FK_Wishlist_Product          FOREIGN KEY (ProductId) REFERENCES Product(ProductId),
    CONSTRAINT UX_Wishlist_User_Product     UNIQUE (UserId, ProductId)
);
GO

CREATE INDEX IX_Wishlist_UserId     ON Wishlist(UserId);
CREATE INDEX IX_Wishlist_ProductId  ON Wishlist(ProductId);
GO

-- =============================================
-- TABLE: Order
-- CRITICAL: Must be created BEFORE InventoryLog
-- =============================================
CREATE TABLE [Order] (
    OrderId                 INT             NOT NULL IDENTITY(1,1),
    UserId                  INT             NOT NULL,
    OrderNumber             NVARCHAR(50)    NOT NULL UNIQUE,
    OrderDate               DATETIME        NOT NULL DEFAULT GETDATE(),
    OrderStatus             NVARCHAR(50)    NOT NULL,
    SubTotal                DECIMAL(10,2)   NOT NULL,
    DiscountAmount          DECIMAL(10,2)   NOT NULL DEFAULT 0,
    ShippingFee             DECIMAL(10,2)   NOT NULL DEFAULT 0,
    TotalAmount             DECIMAL(10,2)   NOT NULL,
    ShippingAddress         NVARCHAR(500)   NULL,
    ShippingCity            NVARCHAR(100)   NULL,
    ShippingPostalCode      NVARCHAR(20)    NULL,
    ContactPhone            NVARCHAR(20)    NULL,
    DeliveryInstructions    NVARCHAR(500)   NULL,
    IsWalkIn                BIT             NOT NULL DEFAULT 0,
    CreatedAt               DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Order             PRIMARY KEY (OrderId),
    CONSTRAINT FK_Order_User        FOREIGN KEY (UserId) REFERENCES [User](UserId),
    CONSTRAINT CK_Order_Status      CHECK (OrderStatus IN ('Pending','Processing','Shipped','Delivered','Cancelled')),
    CONSTRAINT CK_Order_SubTotal    CHECK (SubTotal >= 0),
    CONSTRAINT CK_Order_Discount    CHECK (DiscountAmount >= 0),
    CONSTRAINT CK_Order_Shipping    CHECK (ShippingFee >= 0),
    CONSTRAINT CK_Order_Total       CHECK (TotalAmount >= 0)
);
GO

CREATE INDEX IX_Order_UserId        ON [Order](UserId);
CREATE INDEX IX_Order_OrderNumber   ON [Order](OrderNumber);
CREATE INDEX IX_Order_OrderStatus   ON [Order](OrderStatus);
CREATE INDEX IX_Order_OrderDate     ON [Order](OrderDate);
CREATE INDEX IX_Order_IsWalkIn      ON [Order](IsWalkIn);
GO

-- =============================================
-- TABLE: OrderItem
-- CRITICAL: Must be created BEFORE InventoryLog
-- =============================================
CREATE TABLE OrderItem (
    OrderItemId         INT             NOT NULL IDENTITY(1,1),
    OrderId             INT             NOT NULL,
    ProductId           INT             NOT NULL,
    ProductVariantId    INT             NULL,
    Quantity            INT             NOT NULL,
    UnitPrice           DECIMAL(10,2)   NOT NULL,
    Subtotal            DECIMAL(10,2)   NOT NULL,
    CONSTRAINT PK_OrderItem             PRIMARY KEY (OrderItemId),
    CONSTRAINT FK_OrderItem_Order       FOREIGN KEY (OrderId)           REFERENCES [Order](OrderId)                     ON DELETE CASCADE,
    CONSTRAINT FK_OrderItem_Product     FOREIGN KEY (ProductId)         REFERENCES Product(ProductId),
    CONSTRAINT FK_OrderItem_Variant     FOREIGN KEY (ProductVariantId)  REFERENCES ProductVariant(ProductVariantId),
    CONSTRAINT CK_OrderItem_Quantity    CHECK (Quantity > 0),
    CONSTRAINT CK_OrderItem_UnitPrice   CHECK (UnitPrice >= 0),
    CONSTRAINT CK_OrderItem_Subtotal    CHECK (Subtotal >= 0)
);
GO

CREATE INDEX IX_OrderItem_OrderId           ON OrderItem(OrderId);
CREATE INDEX IX_OrderItem_ProductId         ON OrderItem(ProductId);
CREATE INDEX IX_OrderItem_ProductVariantId  ON OrderItem(ProductVariantId);
GO

-- =============================================
-- TABLE: VoucherUsage
-- CRITICAL: Must be created BEFORE InventoryLog
-- =============================================
CREATE TABLE VoucherUsage (
    VoucherUsageId  INT             NOT NULL IDENTITY(1,1),
    VoucherId       INT             NOT NULL,
    UserId          INT             NOT NULL,
    OrderId         INT             NOT NULL,
    DiscountAmount  DECIMAL(10,2)   NOT NULL,
    UsedAt          DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_VoucherUsage              PRIMARY KEY (VoucherUsageId),
    CONSTRAINT FK_VoucherUsage_Voucher      FOREIGN KEY (VoucherId) REFERENCES Voucher(VoucherId),
    CONSTRAINT FK_VoucherUsage_User         FOREIGN KEY (UserId)    REFERENCES [User](UserId),
    CONSTRAINT FK_VoucherUsage_Order        FOREIGN KEY (OrderId)   REFERENCES [Order](OrderId),
    CONSTRAINT CK_VoucherUsage_Discount     CHECK (DiscountAmount >= 0)
);
GO

CREATE INDEX IX_VoucherUsage_VoucherId  ON VoucherUsage(VoucherId);
CREATE INDEX IX_VoucherUsage_UserId     ON VoucherUsage(UserId);
CREATE INDEX IX_VoucherUsage_OrderId    ON VoucherUsage(OrderId);
GO

-- =============================================
-- TABLE: InventoryLog
-- Now Order table exists — FK resolves correctly
-- =============================================
CREATE TABLE InventoryLog (
    InventoryLogId      INT          NOT NULL IDENTITY(1,1),
    ProductId           INT          NOT NULL,
    ProductVariantId    INT          NULL,
    ChangeQuantity      INT          NOT NULL,
    ChangeType          NVARCHAR(50) NOT NULL,
    OrderId             INT          NULL,
    PurchaseOrderId     INT          NULL,
    ChangedByUserId     INT          NULL,
    Notes               NVARCHAR(500) NULL,
    CreatedAt           DATETIME     NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_InventoryLog              PRIMARY KEY (InventoryLogId),
    CONSTRAINT FK_InvLog_Product            FOREIGN KEY (ProductId)         REFERENCES Product(ProductId),
    CONSTRAINT FK_InvLog_Variant            FOREIGN KEY (ProductVariantId)  REFERENCES ProductVariant(ProductVariantId),
    CONSTRAINT FK_InvLog_Order              FOREIGN KEY (OrderId)           REFERENCES [Order](OrderId),
    CONSTRAINT FK_InvLog_PurchaseOrder      FOREIGN KEY (PurchaseOrderId)   REFERENCES PurchaseOrder(PurchaseOrderId),
    CONSTRAINT FK_InvLog_User               FOREIGN KEY (ChangedByUserId)   REFERENCES [User](UserId),
    CONSTRAINT CK_InvLog_ChangeType         CHECK (ChangeType IN ('Purchase','Sale','Return','Adjustment','Damage','Loss'))
);
GO

CREATE INDEX IX_InvLog_ProductId        ON InventoryLog(ProductId);
CREATE INDEX IX_InvLog_ProductVariantId ON InventoryLog(ProductVariantId);
CREATE INDEX IX_InvLog_ChangeType       ON InventoryLog(ChangeType);
CREATE INDEX IX_InvLog_CreatedAt        ON InventoryLog(CreatedAt);
CREATE INDEX IX_InvLog_OrderId          ON InventoryLog(OrderId);
CREATE INDEX IX_InvLog_PurchaseOrderId  ON InventoryLog(PurchaseOrderId);
GO

-- =============================================
-- TABLE: Payment
-- v3.1 changes:
--   PaymentMethod : 'GCash' | 'BankTransfer' | 'Cash'
--     - Deliveries accept GCash and BankTransfer (BPI) only.
--     - Cash is reserved exclusively for walk-in POS transactions.
--       The app layer must enforce this; the DB accepts all three
--       to support future payment modes without a schema change.
--   PaymentStage  : 'Upfront' | 'Confirmation'
--     - Upfront    — customer pays before the shop dispatches the order.
--     - Confirmation — optional second record when customer confirms
--                      receipt on delivery (e.g. for split / escrow flows).
--     - For standard delivery: one 'Upfront' record is created at checkout.
--   GcashTransactionId  — GCash reference number (NULL for BankTransfer/Cash)
--   BpiReferenceNumber  — BPI bank transfer reference (NULL for GCash/Cash)
-- =============================================
CREATE TABLE Payment (
    PaymentId           INT             NOT NULL IDENTITY(1,1),
    OrderId             INT             NOT NULL,
    PaymentMethod       NVARCHAR(50)    NOT NULL,
    PaymentStage        NVARCHAR(20)    NOT NULL DEFAULT 'Upfront',
    PaymentStatus       NVARCHAR(50)    NOT NULL,
    Amount              DECIMAL(10,2)   NOT NULL,
    GcashTransactionId  NVARCHAR(255)   NULL,   -- populated for GCash payments
    BpiReferenceNumber  NVARCHAR(255)   NULL,   -- populated for BankTransfer payments
    PaymentDate         DATETIME        NULL,
    CreatedAt           DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Payment               PRIMARY KEY (PaymentId),
    CONSTRAINT FK_Payment_Order         FOREIGN KEY (OrderId) REFERENCES [Order](OrderId),
    CONSTRAINT CK_Payment_Method        CHECK (PaymentMethod  IN ('GCash','BankTransfer','Cash')),
    CONSTRAINT CK_Payment_Stage         CHECK (PaymentStage   IN ('Upfront','Confirmation')),
    CONSTRAINT CK_Payment_Status        CHECK (PaymentStatus  IN ('Pending','Completed','Failed','Refunded')),
    CONSTRAINT CK_Payment_Amount        CHECK (Amount >= 0),
    -- Ensure the correct reference field is used per method
    CONSTRAINT CK_Payment_GcashRef      CHECK (
        PaymentMethod <> 'GCash'         OR GcashTransactionId IS NOT NULL OR PaymentStatus = 'Pending'
    ),
    CONSTRAINT CK_Payment_BpiRef        CHECK (
        PaymentMethod <> 'BankTransfer'  OR BpiReferenceNumber  IS NOT NULL OR PaymentStatus = 'Pending'
    )
);
GO

CREATE INDEX IX_Payment_OrderId            ON Payment(OrderId);
CREATE INDEX IX_Payment_PaymentMethod      ON Payment(PaymentMethod);
CREATE INDEX IX_Payment_PaymentStage       ON Payment(PaymentStage);
CREATE INDEX IX_Payment_PaymentStatus      ON Payment(PaymentStatus);
CREATE INDEX IX_Payment_GcashTransactionId ON Payment(GcashTransactionId) WHERE GcashTransactionId IS NOT NULL;
CREATE INDEX IX_Payment_BpiReferenceNumber ON Payment(BpiReferenceNumber)  WHERE BpiReferenceNumber  IS NOT NULL;
GO

-- =============================================
-- TABLE: Delivery
-- v3.1 changes:
--   Courier             : 'Lalamove' | 'LBC'
--     - Lalamove  — used when ShippingCity is within Metro Manila or Bulacan.
--                   (Routing logic enforced at the application layer.)
--     - LBC       — used for all other provinces and regions.
--   LalamoveBookingRef  : Lalamove booking/order reference; NULL for LBC.
--   LbcTrackingNumber   : LBC shipment tracking number; NULL for Lalamove.
--   DriverName/Phone    : Lalamove driver details; typically NULL for LBC.
--   DeliveryStatus values:
--     Pending   → order accepted, not yet picked up
--     PickedUp  → courier has collected the package
--     InTransit → package is on the way (Lalamove live / LBC in network)
--     Delivered → customer confirmed receipt
-- =============================================
CREATE TABLE Delivery (
    DeliveryId              INT             NOT NULL IDENTITY(1,1),
    OrderId                 INT             NOT NULL,
    Courier                 NVARCHAR(20)    NOT NULL,   -- 'Lalamove' or 'LBC'
    LalamoveBookingRef      NVARCHAR(255)   NULL,       -- Lalamove booking reference
    LbcTrackingNumber       NVARCHAR(255)   NULL,       -- LBC tracking number
    DeliveryStatus          NVARCHAR(50)    NOT NULL    DEFAULT 'Pending',
    DriverName              NVARCHAR(100)   NULL,       -- Lalamove driver (NULL for LBC)
    DriverPhone             NVARCHAR(20)    NULL,       -- Lalamove driver phone (NULL for LBC)
    EstimatedDeliveryTime   DATETIME        NULL,
    ActualDeliveryTime      DATETIME        NULL,
    CreatedAt               DATETIME        NOT NULL    DEFAULT GETDATE(),
    CONSTRAINT PK_Delivery              PRIMARY KEY (DeliveryId),
    CONSTRAINT FK_Delivery_Order        FOREIGN KEY (OrderId) REFERENCES [Order](OrderId),
    CONSTRAINT CK_Delivery_Courier      CHECK (Courier         IN ('Lalamove','LBC')),
    CONSTRAINT CK_Delivery_Status       CHECK (DeliveryStatus  IN ('Pending','PickedUp','InTransit','Delivered')),
    -- Lalamove bookings must have a booking ref once past Pending
    CONSTRAINT CK_Delivery_LalamoveRef  CHECK (
        Courier <> 'Lalamove' OR LalamoveBookingRef IS NOT NULL OR DeliveryStatus = 'Pending'
    ),
    -- LBC shipments must have a tracking number once past Pending
    CONSTRAINT CK_Delivery_LbcTracking  CHECK (
        Courier <> 'LBC' OR LbcTrackingNumber IS NOT NULL OR DeliveryStatus = 'Pending'
    ),
    -- LBC shipments should not carry Lalamove-specific driver details
    CONSTRAINT CK_Delivery_LbcNoDriver  CHECK (
        Courier <> 'LBC' OR (DriverName IS NULL AND DriverPhone IS NULL)
    )
);
GO

CREATE INDEX IX_Delivery_OrderId            ON Delivery(OrderId);
CREATE INDEX IX_Delivery_Courier            ON Delivery(Courier);
CREATE INDEX IX_Delivery_DeliveryStatus     ON Delivery(DeliveryStatus);
CREATE INDEX IX_Delivery_LalamoveBookingRef ON Delivery(LalamoveBookingRef) WHERE LalamoveBookingRef IS NOT NULL;
CREATE INDEX IX_Delivery_LbcTrackingNumber  ON Delivery(LbcTrackingNumber)  WHERE LbcTrackingNumber  IS NOT NULL;
GO

-- =============================================
-- TABLE: Review
-- =============================================
CREATE TABLE Review (
    ReviewId            INT             NOT NULL IDENTITY(1,1),
    UserId              INT             NOT NULL,
    ProductId           INT             NOT NULL,
    OrderId             INT             NOT NULL,
    Rating              INT             NOT NULL,
    Comment             NVARCHAR(1000)  NULL,
    IsVerifiedPurchase  BIT             NOT NULL DEFAULT 0,
    CreatedAt           DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Review            PRIMARY KEY (ReviewId),
    CONSTRAINT FK_Review_User       FOREIGN KEY (UserId)    REFERENCES [User](UserId),
    CONSTRAINT FK_Review_Product    FOREIGN KEY (ProductId) REFERENCES Product(ProductId),
    CONSTRAINT FK_Review_Order      FOREIGN KEY (OrderId)   REFERENCES [Order](OrderId),
    CONSTRAINT CK_Review_Rating     CHECK (Rating >= 1 AND Rating <= 5)
);
GO

CREATE INDEX IX_Review_ProductId    ON Review(ProductId);
CREATE INDEX IX_Review_UserId       ON Review(UserId);
CREATE INDEX IX_Review_OrderId      ON Review(OrderId);
CREATE INDEX IX_Review_Rating       ON Review(Rating);
GO

-- =============================================
-- TABLE: POS_Session
-- =============================================
CREATE TABLE POS_Session (
    POSSessionId    INT             NOT NULL IDENTITY(1,1),
    UserId          INT             NOT NULL,
    TerminalName    NVARCHAR(50)    NOT NULL,
    ShiftStart      DATETIME        NOT NULL,
    ShiftEnd        DATETIME        NULL,
    TotalSales      DECIMAL(10,2)   NOT NULL DEFAULT 0,
    CreatedAt       DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_POS_Session       PRIMARY KEY (POSSessionId),
    CONSTRAINT FK_POS_Session_User  FOREIGN KEY (UserId) REFERENCES [User](UserId)
);
GO

CREATE INDEX IX_POS_Session_UserId      ON POS_Session(UserId);
CREATE INDEX IX_POS_Session_ShiftStart  ON POS_Session(ShiftStart);
CREATE INDEX IX_POS_Session_ShiftEnd    ON POS_Session(ShiftEnd);
GO

-- =============================================
-- TABLE: SystemLog
-- =============================================
CREATE TABLE SystemLog (
    SystemLogId         INT             NOT NULL IDENTITY(1,1),
    UserId              INT             NULL,
    EventType           NVARCHAR(50)    NOT NULL,
    EventDescription    NVARCHAR(1000)  NULL,
    CreatedAt           DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_SystemLog         PRIMARY KEY (SystemLogId),
    CONSTRAINT FK_SystemLog_User    FOREIGN KEY (UserId) REFERENCES [User](UserId),
    CONSTRAINT CK_SystemLog_Event   CHECK (EventType IN (
        'Login','Logout','ProductUpdate','OrderStatusChange',
        'InventoryAdjustment','UserCreated','VoucherCreated',
        'PaymentProcessed','AccessDenied'))
);
GO

CREATE INDEX IX_SystemLog_EventType ON SystemLog(EventType);
CREATE INDEX IX_SystemLog_CreatedAt ON SystemLog(CreatedAt);
CREATE INDEX IX_SystemLog_UserId    ON SystemLog(UserId);
GO


-- =============================================
-- VIEWS  (all preserved from v2.0)
-- =============================================

CREATE VIEW vw_UserVoucherUsageCount AS
SELECT UserId, VoucherId, COUNT(*) AS TimesUsed
FROM VoucherUsage
GROUP BY UserId, VoucherId;
GO

CREATE VIEW vw_ActiveProducts AS
SELECT
    p.ProductId,
    p.[Name]            AS ProductName,
    p.ShortDescription,
    p.[Description],
    p.SKU,
    p.Price,
    p.Currency,
    p.StockQuantity,
    p.IsFeatured,
    p.WheelSize,
    p.SpeedCompatibility,
    p.BoostCompatible,
    p.TubelessReady,
    p.Material,
    p.BrakeType,
    c.CategoryId,
    c.[Name]            AS CategoryName,
    c.CategoryCode,
    b.BrandId,
    b.BrandName,
    b.Country           AS BrandCountry,
    pi.ImageUrl         AS PrimaryImageUrl,
    CASE WHEN EXISTS (
        SELECT 1 FROM ProductVariant pv
        WHERE pv.ProductId = p.ProductId AND pv.IsActive = 1
    ) THEN 1 ELSE 0 END AS HasVariants
FROM Product p
INNER JOIN Category c   ON p.CategoryId = c.CategoryId
LEFT  JOIN Brand b      ON p.BrandId    = b.BrandId
LEFT  JOIN ProductImage pi ON p.ProductId = pi.ProductId AND pi.IsPrimary = 1
WHERE p.IsActive = 1 AND c.IsActive = 1;
GO

CREATE VIEW vw_ProductImageGallery AS
SELECT
    pi.ProductImageId,
    pi.ProductId,
    p.[Name]    AS ProductName,
    pi.ImageUrl,
    pi.ImageType,
    pi.IsPrimary,
    pi.DisplayOrder,
    pi.AltText,
    pi.CreatedAt
FROM ProductImage pi
INNER JOIN Product p ON pi.ProductId = p.ProductId
WHERE p.IsActive = 1;
GO

CREATE VIEW vw_OrderSummary AS
SELECT
    o.OrderId,
    o.OrderNumber,
    o.OrderDate,
    o.OrderStatus,
    o.TotalAmount,
    o.IsWalkIn,
    u.FirstName + ' ' + u.LastName  AS CustomerName,
    u.Email                         AS CustomerEmail,
    u.PhoneNumber                   AS CustomerPhone,
    COUNT(oi.OrderItemId)           AS ItemCount,
    SUM(oi.Quantity)                AS TotalQuantity
FROM [Order] o
INNER JOIN [User] u     ON o.UserId  = u.UserId
LEFT  JOIN OrderItem oi ON o.OrderId = oi.OrderId
GROUP BY
    o.OrderId, o.OrderNumber, o.OrderDate, o.OrderStatus,
    o.TotalAmount, o.IsWalkIn, u.FirstName, u.LastName,
    u.Email, u.PhoneNumber;
GO

CREATE VIEW vw_ProductVariantDetails AS
SELECT
    pv.ProductVariantId,
    pv.ProductId,
    p.[Name]                            AS ProductName,
    pv.VariantName,
    pv.SKU,
    p.Price                             AS BasePrice,
    pv.AdditionalPrice,
    (p.Price + pv.AdditionalPrice)      AS TotalPrice,
    pv.StockQuantity,
    pv.IsActive
FROM ProductVariant pv
INNER JOIN Product p ON pv.ProductId = p.ProductId
WHERE pv.IsActive = 1 AND p.IsActive = 1;
GO

CREATE VIEW vw_InventoryStatus AS
SELECT
    p.ProductId,
    p.[Name]                AS ProductName,
    p.StockQuantity         AS BaseStockQuantity,
    ISNULL(SUM(pv.StockQuantity), 0) AS VariantTotalStock,
    CASE
        WHEN EXISTS (SELECT 1 FROM ProductVariant WHERE ProductId = p.ProductId AND IsActive = 1)
        THEN ISNULL(SUM(pv.StockQuantity), 0)
        ELSE p.StockQuantity
    END                     AS AvailableStock,
    c.[Name]                AS CategoryName,
    b.BrandName
FROM Product p
INNER JOIN Category c ON p.CategoryId = c.CategoryId
LEFT  JOIN Brand b    ON p.BrandId    = b.BrandId
LEFT  JOIN ProductVariant pv ON p.ProductId = pv.ProductId AND pv.IsActive = 1
WHERE p.IsActive = 1
GROUP BY p.ProductId, p.[Name], p.StockQuantity, c.[Name], b.BrandName;
GO

PRINT '==============================================';
PRINT 'Taurus_schema.sql  —  Completed Successfully!';
PRINT '==============================================';
PRINT 'Version : 3.1';
PRINT 'Tables  : 26 created';
PRINT 'Views   :  6 created';
PRINT 'Trigger :  1 created (TR_Product_PriceAudit)';
PRINT 'Indexes : 97+ created';
PRINT '----------------------------------------------';
PRINT 'v3.1 changes:';
PRINT '  Payment  — method: GCash | BankTransfer | Cash';
PRINT '           — stage:  Upfront | Confirmation';
PRINT '           — fields: GcashTransactionId, BpiReferenceNumber';
PRINT '  Delivery — courier: Lalamove (NCR/Bulacan) | LBC (others)';
PRINT '           — fields: LalamoveBookingRef, LbcTrackingNumber';
PRINT '----------------------------------------------';
PRINT 'Run Taurus_seed.sql next to populate data.';
PRINT '==============================================';
GO