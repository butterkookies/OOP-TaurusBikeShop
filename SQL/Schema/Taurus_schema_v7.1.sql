-- =============================================================================
-- Taurus Bike Shop  |  TaurusBikeShopDB
-- File    : Taurus_schema.sql
-- Purpose : Create all tables, indexes, views, and triggers
-- Platform: Google Cloud SQL — SQL Server (Cloud SQL for SQL Server)
--           Compatible with SQL Server 2019+ dialect
--
-- Version : 7.1  (fully 3NF-compliant + ReorderThreshold + SupportTask)
-- Changes from v6.0 → v7.1:
--
--   FIX 5 — Product.StockQuantity removed
--     MOD  : Product — StockQuantity column dropped
--            Stock is now exclusively owned by ProductVariant.
--            Products without real variants must have one
--            ProductVariant row (VariantName = 'Default').
--     MOD  : vw_InventoryStatus — CASE expression removed;
--            now a clean SUM(pv.StockQuantity) with no dual-source logic.
--     MOD  : vw_ActiveProducts  — StockQuantity column removed.
--
--   FIX 6 — Order.TotalAmount removed
--     MOD  : Order — TotalAmount column and CK_Order_Total dropped.
--            SubTotal, DiscountAmount, and ShippingFee remain stored.
--     MOD  : vw_OrderSummary — now computes
--            (SubTotal - DiscountAmount + ShippingFee) AS TotalAmount.
--
--   FIX 7 — OrderItem.Subtotal removed
--     MOD  : OrderItem — Subtotal column and CK_OrderItem_Subtotal dropped.
--            UnitPrice is locked at order time; no audit data lost.
--     NEW  : vw_OrderItemDetail — computes Quantity * UnitPrice AS Subtotal.
--
--   FIX 8 — PurchaseOrderItem.Subtotal removed
--     MOD  : PurchaseOrderItem — Subtotal and CK_POItem_Subtotal dropped.
--
--   FIX 9 — PurchaseOrder.TotalAmount removed
--     MOD  : PurchaseOrder — TotalAmount and CK_PurchaseOrder_Total dropped.
--     NEW  : vw_PurchaseOrderDetail — computes per-line Subtotal and
--            order-level TotalAmount via SUM(Quantity * UnitPrice).
--
--   All previous fixes from v5.0–v6.0 retained:
--     Fix 1 (v5.0): Address table (PostalCode→City transitive dep)
--     Fix 2 (v5.0): PickupOrder table (NULL-sparse pickup columns)
--     Fix 3 (v5.0): GCashPayment + BankTransferPayment subtypes
--     Fix 4 (v6.0): LalamoveDelivery + LBCDelivery subtypes
--
--   COD NOTE : Cash on Delivery is NOT offered by Taurus Bike Shop.
--   GCS NOTE : Images, proofs, and attachments are stored in Google Cloud Storage.
--              Only URLs + bucket/path metadata are stored in this database.
--
-- HOW TO RUN (Google Cloud SQL):
--   Option A — Cloud SQL Studio (Google Cloud Console):
--     1. Cloud SQL > Instance > Cloud SQL Studio
--     2. Authenticate, select database, open SQL editor, paste & Run
--   Option B — Cloud SQL Auth Proxy + SSMS:
--     1. cloud-sql-proxy YOUR_PROJECT:REGION:INSTANCE_NAME
--     2. Connect SSMS to 127.0.0.1,1433 and Execute (F5)
--   Option C — gcloud CLI:
--     gcloud sql connect INSTANCE_NAME --user=sqlserver
--     Then: :r Taurus_schema.sql
-- =============================================================================

SET NOCOUNT ON;
GO

-- =============================================================================
-- TABLE: Role
-- Created first — referenced by UserRole.
-- =============================================================================
CREATE TABLE [Role] (
    RoleId        INT           NOT NULL IDENTITY(1,1),
    RoleName      NVARCHAR(50)  NOT NULL UNIQUE,
    [Description] NVARCHAR(255) NULL,
    CreatedAt     DATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Role      PRIMARY KEY (RoleId),
    CONSTRAINT CK_Role_Name CHECK (RoleName IN ('Admin','Manager','Cashier','Staff','Customer'))
);
GO
CREATE INDEX IX_Role_RoleName ON [Role](RoleName);
GO

-- =============================================================================
-- TABLE: User
-- Covers: DB1 (User Database)
-- v5.0: Removed Address/City/PostalCode — now in Address table (3NF Fix 1).
--       DefaultAddressId FK is added via ALTER TABLE after Address is created
--       (SQL Server requires this to resolve the circular FK dependency).
-- =============================================================================
CREATE TABLE [User] (
    UserId           INT           NOT NULL IDENTITY(1,1),
    Email            NVARCHAR(255) NULL,
    PasswordHash     NVARCHAR(255) NULL,
    FirstName        NVARCHAR(100) NOT NULL,
    LastName         NVARCHAR(100) NOT NULL,
    PhoneNumber      NVARCHAR(20)  NULL,
    DefaultAddressId INT           NULL,   -- FK added below after Address table
    IsActive         BIT           NOT NULL DEFAULT 1,
    IsWalkIn         BIT           NOT NULL DEFAULT 0,
    CreatedAt        DATETIME      NOT NULL DEFAULT GETDATE(),
    LastLoginAt      DATETIME      NULL,
    CONSTRAINT PK_User PRIMARY KEY (UserId)
);
GO
CREATE UNIQUE INDEX UX_User_Email  ON [User](Email) WHERE Email IS NOT NULL;
CREATE INDEX IX_User_PhoneNumber   ON [User](PhoneNumber);
CREATE INDEX IX_User_IsActive      ON [User](IsActive);
CREATE INDEX IX_User_IsWalkIn      ON [User](IsWalkIn);
GO

-- =============================================================================
-- TABLE: UserRole
-- =============================================================================
CREATE TABLE UserRole (
    UserRoleId INT      NOT NULL IDENTITY(1,1),
    UserId     INT      NOT NULL,
    RoleId     INT      NOT NULL,
    AssignedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_UserRole      PRIMARY KEY (UserRoleId),
    CONSTRAINT FK_UserRole_User FOREIGN KEY (UserId) REFERENCES [User](UserId) ON DELETE CASCADE,
    CONSTRAINT FK_UserRole_Role FOREIGN KEY (RoleId) REFERENCES [Role](RoleId),
    CONSTRAINT UX_UserRole_Pair UNIQUE (UserId, RoleId)
);
GO
CREATE INDEX IX_UserRole_UserId ON UserRole(UserId);
CREATE INDEX IX_UserRole_RoleId ON UserRole(RoleId);
GO

-- =============================================================================
-- TABLE: Address  (NEW in v5.0 — 3NF Fix 1)
-- Eliminates: PostalCode → City transitive dependency from User and Order.
-- Each row is one address belonging to one User.
-- IsSnapshot = 1  → frozen copy created at checkout; linked from Order.
--                   Never modified — preserves historical shipping record.
-- IsSnapshot = 0  → live saved address; shown in user profile / checkout picker.
-- IsDefault  = 1  → the user's primary address (at most one per user, per index).
-- =============================================================================
CREATE TABLE [Address] (
    AddressId  INT           NOT NULL IDENTITY(1,1),
    UserId     INT           NOT NULL,
    Label      NVARCHAR(50)  NOT NULL DEFAULT 'Home',
    Street     NVARCHAR(500) NOT NULL,
    City       NVARCHAR(100) NOT NULL,
    PostalCode NVARCHAR(20)  NOT NULL,
    Province   NVARCHAR(100) NULL,
    Country    NVARCHAR(100) NOT NULL DEFAULT 'Philippines',
    IsDefault  BIT           NOT NULL DEFAULT 0,
    IsSnapshot BIT           NOT NULL DEFAULT 0,
    CreatedAt  DATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Address       PRIMARY KEY (AddressId),
    CONSTRAINT FK_Address_User  FOREIGN KEY (UserId) REFERENCES [User](UserId) ON DELETE CASCADE,
    CONSTRAINT CK_Address_Label CHECK (Label IN ('Home','Work','Other'))
);
GO
CREATE INDEX IX_Address_UserId     ON [Address](UserId);
CREATE INDEX IX_Address_IsDefault  ON [Address](UserId, IsDefault) WHERE IsDefault  = 1;
CREATE INDEX IX_Address_IsSnapshot ON [Address](IsSnapshot);
GO

-- Deferred FK: User.DefaultAddressId → Address (resolves circular dependency)
ALTER TABLE [User]
    ADD CONSTRAINT FK_User_DefaultAddress
    FOREIGN KEY (DefaultAddressId) REFERENCES [Address](AddressId);
GO
CREATE INDEX IX_User_DefaultAddressId ON [User](DefaultAddressId) WHERE DefaultAddressId IS NOT NULL;
GO

-- =============================================================================
-- TABLE: OTPVerification
-- Covers: Part 1 — 2-step registration OTP flow
-- =============================================================================
CREATE TABLE OTPVerification (
    OTPId     INT           NOT NULL IDENTITY(1,1),
    Email     NVARCHAR(255) NOT NULL,
    OTPCode   NVARCHAR(10)  NOT NULL,
    IsUsed    BIT           NOT NULL DEFAULT 0,
    ExpiresAt DATETIME      NOT NULL,
    CreatedAt DATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_OTPVerification PRIMARY KEY (OTPId)
);
GO
CREATE INDEX IX_OTP_Email     ON OTPVerification(Email);
CREATE INDEX IX_OTP_ExpiresAt ON OTPVerification(ExpiresAt);
CREATE INDEX IX_OTP_IsUsed    ON OTPVerification(IsUsed);
GO

-- =============================================================================
-- TABLE: GuestSession
-- Covers: Part 1 — "Continue as Guest" path
-- ConvertedToUserId is set if the guest later registers an account.
-- =============================================================================
CREATE TABLE GuestSession (
    GuestSessionId    INT           NOT NULL IDENTITY(1,1),
    SessionToken      NVARCHAR(100) NOT NULL,
    Email             NVARCHAR(255) NULL,
    PhoneNumber       NVARCHAR(20)  NULL,
    ConvertedToUserId INT           NULL,
    ExpiresAt         DATETIME      NOT NULL,
    CreatedAt         DATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_GuestSession        PRIMARY KEY (GuestSessionId),
    CONSTRAINT FK_GuestSession_User   FOREIGN KEY (ConvertedToUserId) REFERENCES [User](UserId),
    CONSTRAINT UX_GuestSession_Token  UNIQUE (SessionToken)
);
GO
CREATE INDEX IX_GuestSession_Token     ON GuestSession(SessionToken);
CREATE INDEX IX_GuestSession_ExpiresAt ON GuestSession(ExpiresAt);
GO

-- =============================================================================
-- TABLE: Category
-- CategoryCode matches product headers: UNIT, FRAME, FORK, HUB, UPGKIT,
-- STEM, HBAR, SADDLE, GRIP, PEDAL, RIM, TIRE, CHAIN
-- =============================================================================
CREATE TABLE Category (
    CategoryId       INT           NOT NULL IDENTITY(1,1),
    CategoryCode     VARCHAR(20)   NOT NULL,
    [Name]           NVARCHAR(100) NOT NULL,
    [Description]    NVARCHAR(500) NULL,
    ParentCategoryId INT           NULL,
    IsActive         BIT           NOT NULL DEFAULT 1,
    DisplayOrder     INT           NOT NULL DEFAULT 0,
    CONSTRAINT PK_Category        PRIMARY KEY (CategoryId),
    CONSTRAINT UQ_Category_Code   UNIQUE (CategoryCode),
    CONSTRAINT FK_Category_Parent FOREIGN KEY (ParentCategoryId) REFERENCES Category(CategoryId)
);
GO
CREATE INDEX IX_Category_ParentCategoryId ON Category(ParentCategoryId);
CREATE INDEX IX_Category_IsActive         ON Category(IsActive);
CREATE INDEX IX_Category_DisplayOrder     ON Category(DisplayOrder);
GO

-- =============================================================================
-- TABLE: Brand
-- =============================================================================
CREATE TABLE Brand (
    BrandId       INT           NOT NULL IDENTITY(1,1),
    BrandName     NVARCHAR(100) NOT NULL,
    Country       NVARCHAR(100) NULL,
    Website       NVARCHAR(255) NULL,
    [Description] NVARCHAR(500) NULL,
    IsActive      BIT           NOT NULL DEFAULT 1,
    CreatedAt     DATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Brand      PRIMARY KEY (BrandId),
    CONSTRAINT UQ_Brand_Name UNIQUE (BrandName)
);
GO
CREATE INDEX IX_Brand_IsActive ON Brand(IsActive);
GO

-- =============================================================================
-- TABLE: Product
-- Covers: DB2 (Product Database)
-- =============================================================================
CREATE TABLE Product (
    ProductId          INT            NOT NULL IDENTITY(1,1),
    CategoryId         INT            NOT NULL,
    BrandId            INT            NULL,
    SKU                VARCHAR(50)    NULL,
    [Name]             NVARCHAR(200)  NOT NULL,
    ShortDescription   NVARCHAR(300)  NULL,
    [Description]      NVARCHAR(MAX)  NULL,
    Price              DECIMAL(10,2)  NOT NULL,
    Currency           CHAR(3)        NOT NULL DEFAULT 'PHP',
    -- StockQuantity removed in v7.0 (3NF Fix 5): stock is solely owned by
    -- ProductVariant. Every product must have at least one variant row
    -- (use VariantName = 'Default' for products with no real variants).
    -- This eliminates the dual-source ambiguity vw_InventoryStatus worked
    -- around with a CASE expression.
    Material           NVARCHAR(100)  NULL,
    Color              NVARCHAR(100)  NULL,
    WheelSize          NVARCHAR(20)   NULL,
    SpeedCompatibility NVARCHAR(50)   NULL,
    BoostCompatible    BIT            NULL,
    TubelessReady      BIT            NULL,
    AxleStandard       NVARCHAR(50)   NULL,
    SuspensionTravel   NVARCHAR(50)   NULL,
    BrakeType          NVARCHAR(100)  NULL,
    AdditionalSpecs    NVARCHAR(1000) NULL,
    IsActive           BIT            NOT NULL DEFAULT 1,
    IsFeatured         BIT            NOT NULL DEFAULT 0,
    CreatedAt          DATETIME       NOT NULL DEFAULT GETDATE(),
    UpdatedAt          DATETIME       NULL,
    CONSTRAINT PK_Product          PRIMARY KEY (ProductId),
    CONSTRAINT FK_Product_Category FOREIGN KEY (CategoryId) REFERENCES Category(CategoryId),
    CONSTRAINT FK_Product_Brand    FOREIGN KEY (BrandId)    REFERENCES Brand(BrandId),
    CONSTRAINT CK_Product_Price    CHECK (Price >= 0),
    CONSTRAINT CK_Product_Currency CHECK (Currency IN ('PHP','USD','EUR'))
);
GO
CREATE INDEX IX_Product_CategoryId ON Product(CategoryId);
CREATE INDEX IX_Product_BrandId    ON Product(BrandId);
CREATE INDEX IX_Product_IsActive   ON Product(IsActive);
CREATE INDEX IX_Product_IsFeatured ON Product(IsFeatured);
CREATE INDEX IX_Product_Name       ON Product([Name]);
CREATE INDEX IX_Product_Price      ON Product(Price, IsActive);
CREATE UNIQUE INDEX UX_Product_SKU ON Product(SKU) WHERE SKU IS NOT NULL;
GO

-- =============================================================================
-- TABLE: PriceHistory
-- Auto-populated by trigger TR_Product_PriceAudit.
-- =============================================================================
CREATE TABLE PriceHistory (
    PriceHistoryId  INT           NOT NULL IDENTITY(1,1),
    ProductId       INT           NOT NULL,
    OldPrice        DECIMAL(10,2) NOT NULL,
    NewPrice        DECIMAL(10,2) NOT NULL,
    ChangedAt       DATETIME      NOT NULL DEFAULT GETDATE(),
    ChangedByUserId INT           NULL,
    Notes           NVARCHAR(500) NULL,
    CONSTRAINT PK_PriceHistory         PRIMARY KEY (PriceHistoryId),
    CONSTRAINT FK_PriceHistory_Product FOREIGN KEY (ProductId)       REFERENCES Product(ProductId),
    CONSTRAINT FK_PriceHistory_User    FOREIGN KEY (ChangedByUserId) REFERENCES [User](UserId)
);
GO
CREATE INDEX IX_PriceHistory_ProductId ON PriceHistory(ProductId, ChangedAt DESC);
CREATE INDEX IX_PriceHistory_ChangedAt ON PriceHistory(ChangedAt);
GO

IF OBJECT_ID('TR_Product_PriceAudit', 'TR') IS NOT NULL DROP TRIGGER TR_Product_PriceAudit;
GO
CREATE TRIGGER TR_Product_PriceAudit
ON Product AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO PriceHistory (ProductId, OldPrice, NewPrice, Notes)
    SELECT i.ProductId, d.Price, i.Price, 'Automatic price change audit'
    FROM inserted i INNER JOIN deleted d ON i.ProductId = d.ProductId
    WHERE i.Price <> d.Price;
END;
GO

-- =============================================================================
-- TABLE: ProductVariant
-- =============================================================================
CREATE TABLE ProductVariant (
    ProductVariantId INT           NOT NULL IDENTITY(1,1),
    ProductId        INT           NOT NULL,
    VariantName      NVARCHAR(100) NOT NULL,
    SKU              NVARCHAR(50)  NULL,
    AdditionalPrice  DECIMAL(10,2) NOT NULL DEFAULT 0,
    StockQuantity    INT           NOT NULL DEFAULT 0,
    IsActive         BIT           NOT NULL DEFAULT 1,
    CreatedAt        DATETIME      NOT NULL DEFAULT GETDATE(),
    UpdatedAt        DATETIME      NULL,
    CONSTRAINT PK_ProductVariant           PRIMARY KEY (ProductVariantId),
    CONSTRAINT FK_ProductVariant_Product   FOREIGN KEY (ProductId) REFERENCES Product(ProductId) ON DELETE CASCADE,
    CONSTRAINT CK_ProductVariant_StockQty  CHECK (StockQuantity >= 0),
    CONSTRAINT CK_ProductVariant_AddlPrice CHECK (AdditionalPrice >= 0)
);
GO
CREATE INDEX IX_ProductVariant_ProductId ON ProductVariant(ProductId);
CREATE INDEX IX_ProductVariant_IsActive  ON ProductVariant(IsActive);
CREATE INDEX IX_ProductVariant_SKU       ON ProductVariant(SKU);
GO

-- =============================================================================
-- TABLE: ProductImage
-- GCS storage pattern — only metadata and URL stored here.
-- Actual image binary lives in Google Cloud Storage.
--   gs://{StorageBucket}/{StoragePath}  → GCS object path
--   ImageUrl                            → CDN / public served URL
-- =============================================================================
CREATE TABLE ProductImage (
    ProductImageId   INT            NOT NULL IDENTITY(1,1),
    ProductId        INT            NOT NULL,
    StorageBucket    NVARCHAR(200)  NOT NULL,
    StoragePath      NVARCHAR(1000) NOT NULL,
    ImageUrl         NVARCHAR(1000) NOT NULL,
    ImageType        NVARCHAR(50)   NOT NULL,
    IsPrimary        BIT            NOT NULL DEFAULT 0,
    DisplayOrder     INT            NOT NULL DEFAULT 0,
    AltText          NVARCHAR(200)  NULL,
    FileSizeBytes    INT            NULL,
    MimeType         NVARCHAR(50)   NULL,
    Width            INT            NULL,
    Height           INT            NULL,
    UploadedByUserId INT            NULL,
    CreatedAt        DATETIME       NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_ProductImage           PRIMARY KEY (ProductImageId),
    CONSTRAINT FK_ProductImage_Product   FOREIGN KEY (ProductId)        REFERENCES Product(ProductId) ON DELETE CASCADE,
    CONSTRAINT FK_ProductImage_Uploader  FOREIGN KEY (UploadedByUserId) REFERENCES [User](UserId),
    CONSTRAINT CK_ProductImage_Type      CHECK (ImageType IN ('Full','Medium','Thumbnail')),
    CONSTRAINT CK_ProductImage_Order     CHECK (DisplayOrder >= 0)
);
GO
CREATE UNIQUE INDEX UX_ProductImage_Primary ON ProductImage(ProductId) WHERE IsPrimary = 1;
CREATE INDEX IX_ProductImage_ProductId      ON ProductImage(ProductId);
CREATE INDEX IX_ProductImage_ImageType      ON ProductImage(ImageType);
CREATE INDEX IX_ProductImage_IsPrimary      ON ProductImage(IsPrimary);
CREATE INDEX IX_ProductImage_DisplayOrder   ON ProductImage(DisplayOrder);
GO

-- =============================================================================
-- TABLE: Supplier
-- =============================================================================
CREATE TABLE Supplier (
    SupplierId    INT           NOT NULL IDENTITY(1,1),
    [Name]        NVARCHAR(200) NOT NULL,
    ContactPerson NVARCHAR(100) NULL,
    PhoneNumber   NVARCHAR(20)  NULL,
    Email         NVARCHAR(255) NULL,
    [Address]     NVARCHAR(500) NULL,
    IsActive      BIT           NOT NULL DEFAULT 1,
    CreatedAt     DATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Supplier PRIMARY KEY (SupplierId)
);
GO
CREATE INDEX IX_Supplier_IsActive     ON Supplier(IsActive);
CREATE UNIQUE INDEX UX_Supplier_Email ON Supplier(Email) WHERE Email IS NOT NULL;
GO

-- =============================================================================
-- TABLE: PurchaseOrder
-- Covers: Part 12 — admin inventory restocking
-- =============================================================================
CREATE TABLE PurchaseOrder (
    PurchaseOrderId      INT           NOT NULL IDENTITY(1,1),
    SupplierId           INT           NOT NULL,
    OrderDate            DATETIME      NOT NULL DEFAULT GETDATE(),
    ExpectedDeliveryDate DATETIME      NULL,
    [Status]             NVARCHAR(50)  NOT NULL DEFAULT 'Pending',
    -- TotalAmount removed in v7.0 (3NF Fix 8): derived from sum of
    -- PurchaseOrderItem (Quantity × UnitPrice). Computed in vw_PurchaseOrderDetail.
    CreatedByUserId      INT           NULL,
    CreatedAt            DATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_PurchaseOrder          PRIMARY KEY (PurchaseOrderId),
    CONSTRAINT FK_PurchaseOrder_Supplier FOREIGN KEY (SupplierId)      REFERENCES Supplier(SupplierId),
    CONSTRAINT FK_PurchaseOrder_User     FOREIGN KEY (CreatedByUserId) REFERENCES [User](UserId),
    CONSTRAINT CK_PurchaseOrder_Status   CHECK ([Status] IN ('Pending','Received','Cancelled'))
);
GO
CREATE INDEX IX_PurchaseOrder_SupplierId      ON PurchaseOrder(SupplierId);
CREATE INDEX IX_PurchaseOrder_Status          ON PurchaseOrder([Status]);
CREATE INDEX IX_PurchaseOrder_OrderDate       ON PurchaseOrder(OrderDate);
CREATE INDEX IX_PurchaseOrder_CreatedByUserId ON PurchaseOrder(CreatedByUserId);
GO

-- =============================================================================
-- TABLE: PurchaseOrderItem
-- Subtotal is intentionally stored — locked at PO issuance time.
-- =============================================================================
CREATE TABLE PurchaseOrderItem (
    PurchaseOrderItemId INT           NOT NULL IDENTITY(1,1),
    PurchaseOrderId     INT           NOT NULL,
    ProductId           INT           NOT NULL,
    ProductVariantId    INT           NULL,
    Quantity            INT           NOT NULL,
    UnitPrice           DECIMAL(10,2) NOT NULL,
    -- Subtotal removed in v7.0 (3NF Fix 8): derived from Quantity × UnitPrice.
    -- Computed in vw_PurchaseOrderDetail. UnitPrice is locked at PO time.
    CONSTRAINT PK_PurchaseOrderItem      PRIMARY KEY (PurchaseOrderItemId),
    CONSTRAINT FK_POItem_PO              FOREIGN KEY (PurchaseOrderId)  REFERENCES PurchaseOrder(PurchaseOrderId) ON DELETE CASCADE,
    CONSTRAINT FK_POItem_Product         FOREIGN KEY (ProductId)        REFERENCES Product(ProductId),
    CONSTRAINT FK_POItem_Variant         FOREIGN KEY (ProductVariantId) REFERENCES ProductVariant(ProductVariantId),
    CONSTRAINT CK_POItem_Quantity        CHECK (Quantity > 0),
    CONSTRAINT CK_POItem_UnitPrice       CHECK (UnitPrice >= 0)
);
GO
CREATE INDEX IX_POItem_PurchaseOrderId  ON PurchaseOrderItem(PurchaseOrderId);
CREATE INDEX IX_POItem_ProductId        ON PurchaseOrderItem(ProductId);
CREATE INDEX IX_POItem_ProductVariantId ON PurchaseOrderItem(ProductVariantId);
GO

-- =============================================================================
-- TABLE: Voucher
-- =============================================================================
CREATE TABLE Voucher (
    VoucherId          INT           NOT NULL IDENTITY(1,1),
    Code               NVARCHAR(50)  NOT NULL UNIQUE,
    [Description]      NVARCHAR(500) NULL,
    DiscountType       NVARCHAR(20)  NOT NULL,
    DiscountValue      DECIMAL(10,2) NOT NULL,
    MinimumOrderAmount DECIMAL(10,2) NULL,
    MaxUses            INT           NULL,
    MaxUsesPerUser     INT           NULL,
    StartDate          DATETIME      NOT NULL,
    EndDate            DATETIME      NULL,
    IsActive           BIT           NOT NULL DEFAULT 1,
    CreatedAt          DATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Voucher              PRIMARY KEY (VoucherId),
    CONSTRAINT CK_Voucher_DiscountType CHECK (DiscountType IN ('Percentage','Fixed')),
    CONSTRAINT CK_Voucher_DiscountVal  CHECK (DiscountValue > 0),
    CONSTRAINT CK_Voucher_MinOrderAmt  CHECK (MinimumOrderAmount IS NULL OR MinimumOrderAmount >= 0),
    CONSTRAINT CK_Voucher_MaxUses      CHECK (MaxUses IS NULL OR MaxUses > 0),
    CONSTRAINT CK_Voucher_MaxPerUser   CHECK (MaxUsesPerUser IS NULL OR MaxUsesPerUser > 0)
);
GO
CREATE INDEX IX_Voucher_Code      ON Voucher(Code);
CREATE INDEX IX_Voucher_IsActive  ON Voucher(IsActive);
CREATE INDEX IX_Voucher_StartDate ON Voucher(StartDate);
CREATE INDEX IX_Voucher_EndDate   ON Voucher(EndDate);
GO

-- =============================================================================
-- TABLE: UserVoucher
-- =============================================================================
CREATE TABLE UserVoucher (
    UserVoucherId INT      NOT NULL IDENTITY(1,1),
    UserId        INT      NOT NULL,
    VoucherId     INT      NOT NULL,
    AssignedAt    DATETIME NOT NULL DEFAULT GETDATE(),
    ExpiresAt     DATETIME NULL,
    CONSTRAINT PK_UserVoucher        PRIMARY KEY (UserVoucherId),
    CONSTRAINT FK_UserVoucher_User   FOREIGN KEY (UserId)    REFERENCES [User](UserId),
    CONSTRAINT FK_UserVoucher_Voucher FOREIGN KEY (VoucherId) REFERENCES Voucher(VoucherId),
    CONSTRAINT UX_UserVoucher_Pair   UNIQUE (UserId, VoucherId)
);
GO
CREATE INDEX IX_UserVoucher_UserId    ON UserVoucher(UserId);
CREATE INDEX IX_UserVoucher_VoucherId ON UserVoucher(VoucherId);
GO

-- =============================================================================
-- TABLE: Cart
-- Covers: DB5 (Cart Database)
-- UserId is nullable to support guest carts.
-- Exactly one of (UserId, GuestSessionId) must be populated — enforced by CHECK.
-- =============================================================================
CREATE TABLE Cart (
    CartId         INT      NOT NULL IDENTITY(1,1),
    UserId         INT      NULL,
    GuestSessionId INT      NULL,
    CreatedAt      DATETIME NOT NULL DEFAULT GETDATE(),
    LastUpdatedAt  DATETIME NULL,
    IsCheckedOut   BIT      NOT NULL DEFAULT 0,
    CONSTRAINT PK_Cart              PRIMARY KEY (CartId),
    CONSTRAINT FK_Cart_User         FOREIGN KEY (UserId)         REFERENCES [User](UserId),
    CONSTRAINT FK_Cart_GuestSession FOREIGN KEY (GuestSessionId) REFERENCES GuestSession(GuestSessionId),
    CONSTRAINT CK_Cart_Owner        CHECK (
        (UserId IS NOT NULL AND GuestSessionId IS NULL) OR
        (UserId IS NULL     AND GuestSessionId IS NOT NULL)
    )
);
GO
CREATE UNIQUE INDEX UX_Cart_User_Active  ON Cart(UserId)         WHERE IsCheckedOut = 0 AND UserId IS NOT NULL;
CREATE UNIQUE INDEX UX_Cart_Guest_Active ON Cart(GuestSessionId) WHERE IsCheckedOut = 0 AND GuestSessionId IS NOT NULL;
CREATE INDEX IX_Cart_UserId              ON Cart(UserId);
CREATE INDEX IX_Cart_GuestSessionId      ON Cart(GuestSessionId);
CREATE INDEX IX_Cart_IsCheckedOut        ON Cart(IsCheckedOut);
GO

-- =============================================================================
-- TABLE: CartItem
-- =============================================================================
CREATE TABLE CartItem (
    CartItemId       INT           NOT NULL IDENTITY(1,1),
    CartId           INT           NOT NULL,
    ProductId        INT           NOT NULL,
    ProductVariantId INT           NULL,
    Quantity         INT           NOT NULL,
    PriceAtAdd       DECIMAL(10,2) NOT NULL,
    AddedAt          DATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_CartItem            PRIMARY KEY (CartItemId),
    CONSTRAINT FK_CartItem_Cart       FOREIGN KEY (CartId)           REFERENCES Cart(CartId)                      ON DELETE CASCADE,
    CONSTRAINT FK_CartItem_Product    FOREIGN KEY (ProductId)        REFERENCES Product(ProductId),
    CONSTRAINT FK_CartItem_Variant    FOREIGN KEY (ProductVariantId) REFERENCES ProductVariant(ProductVariantId),
    CONSTRAINT CK_CartItem_Quantity   CHECK (Quantity > 0),
    CONSTRAINT CK_CartItem_PriceAtAdd CHECK (PriceAtAdd >= 0)
);
GO
CREATE INDEX IX_CartItem_CartId           ON CartItem(CartId);
CREATE INDEX IX_CartItem_ProductId        ON CartItem(ProductId);
CREATE INDEX IX_CartItem_ProductVariantId ON CartItem(ProductVariantId);
GO

-- =============================================================================
-- TABLE: Wishlist
-- Covers: DB5 — wishlist portion (out-of-stock save + restock notification)
-- =============================================================================
CREATE TABLE Wishlist (
    WishlistId INT      NOT NULL IDENTITY(1,1),
    UserId     INT      NOT NULL,
    ProductId  INT      NOT NULL,
    AddedAt    DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Wishlist             PRIMARY KEY (WishlistId),
    CONSTRAINT FK_Wishlist_User        FOREIGN KEY (UserId)    REFERENCES [User](UserId)    ON DELETE CASCADE,
    CONSTRAINT FK_Wishlist_Product     FOREIGN KEY (ProductId) REFERENCES Product(ProductId),
    CONSTRAINT UX_Wishlist_UserProduct UNIQUE (UserId, ProductId)
);
GO
CREATE INDEX IX_Wishlist_UserId    ON Wishlist(UserId);
CREATE INDEX IX_Wishlist_ProductId ON Wishlist(ProductId);
GO

-- =============================================================================
-- TABLE: Order
-- Covers: DB3 (Order Database)
-- v5.0 changes:
--   REMOVED ShippingAddress/ShippingCity/ShippingPostalCode
--           → replaced by ShippingAddressId FK to Address snapshot row
--   REMOVED PickupReadyAt/PickupExpiresAt/PickupConfirmedAt
--           → moved to PickupOrder table
--
-- TotalAmount is intentionally stored (audit snapshot).
-- CRITICAL: Must be created BEFORE InventoryLog.
-- =============================================================================
CREATE TABLE [Order] (
    OrderId              INT           NOT NULL IDENTITY(1,1),
    UserId               INT           NOT NULL,
    OrderNumber          NVARCHAR(50)  NOT NULL UNIQUE,
    OrderDate            DATETIME      NOT NULL DEFAULT GETDATE(),
    OrderStatus          NVARCHAR(50)  NOT NULL,
    SubTotal             DECIMAL(10,2) NOT NULL,
    DiscountAmount       DECIMAL(10,2) NOT NULL DEFAULT 0,
    ShippingFee          DECIMAL(10,2) NOT NULL DEFAULT 0,
    -- TotalAmount removed in v7.0 (3NF Fix 6): derived from
    -- SubTotal - DiscountAmount + ShippingFee. Computed in vw_OrderSummary.
    -- All three component columns are stored and immutable — no audit data lost.
    ShippingAddressId    INT           NULL,       -- FK → Address snapshot; NULL for POS
    ContactPhone         NVARCHAR(20)  NULL,
    DeliveryInstructions NVARCHAR(500) NULL,
    IsWalkIn             BIT           NOT NULL DEFAULT 0,
    CreatedAt            DATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Order             PRIMARY KEY (OrderId),
    CONSTRAINT FK_Order_User        FOREIGN KEY (UserId)            REFERENCES [User](UserId),
    CONSTRAINT FK_Order_ShipAddress FOREIGN KEY (ShippingAddressId) REFERENCES [Address](AddressId),
    CONSTRAINT CK_Order_Status      CHECK (OrderStatus IN (
        'Pending',
        'PendingVerification',   -- bank transfer awaiting admin approval
        'OnHold',                -- payment timeout or pickup expired
        'Processing',            -- payment verified, preparing order
        'ReadyForPickup',        -- store pickup: item ready at counter
        'PickedUp',              -- store pickup: customer collected
        'Shipped',               -- delivery: out for delivery
        'Delivered',             -- delivery: confirmed received
        'Cancelled'
    )),
    CONSTRAINT CK_Order_SubTotal  CHECK (SubTotal >= 0),
    CONSTRAINT CK_Order_Discount  CHECK (DiscountAmount >= 0),
    CONSTRAINT CK_Order_Shipping  CHECK (ShippingFee >= 0)
);
GO
CREATE INDEX IX_Order_UserId            ON [Order](UserId);
CREATE INDEX IX_Order_OrderNumber       ON [Order](OrderNumber);
CREATE INDEX IX_Order_OrderStatus       ON [Order](OrderStatus);
CREATE INDEX IX_Order_OrderDate         ON [Order](OrderDate);
CREATE INDEX IX_Order_IsWalkIn          ON [Order](IsWalkIn);
CREATE INDEX IX_Order_ShippingAddressId ON [Order](ShippingAddressId) WHERE ShippingAddressId IS NOT NULL;
GO

-- =============================================================================
-- TABLE: PickupOrder  (NEW in v5.0 — 3NF Fix 2)
-- Eliminates: NULL-sparse pickup columns that appeared on every delivery/POS row.
-- One row per store-pickup order. Delivery and POS orders have no row here.
-- One-to-one with Order enforced by UNIQUE constraint on OrderId.
-- =============================================================================
CREATE TABLE PickupOrder (
    PickupOrderId     INT      NOT NULL IDENTITY(1,1),
    OrderId           INT      NOT NULL,
    PickupReadyAt     DATETIME NULL,   -- when admin sets status to ReadyForPickup
    PickupExpiresAt   DATETIME NULL,   -- 3-day collection window (app: ReadyAt + 3d)
    PickupConfirmedAt DATETIME NULL,   -- when customer physically collected
    CONSTRAINT PK_PickupOrder       PRIMARY KEY (PickupOrderId),
    CONSTRAINT FK_PickupOrder_Order FOREIGN KEY (OrderId) REFERENCES [Order](OrderId) ON DELETE CASCADE,
    CONSTRAINT UX_PickupOrder_Order UNIQUE (OrderId)
);
GO
CREATE INDEX IX_PickupOrder_OrderId    ON PickupOrder(OrderId);
CREATE INDEX IX_PickupOrder_ExpiresAt  ON PickupOrder(PickupExpiresAt) WHERE PickupExpiresAt IS NOT NULL;
GO

-- =============================================================================
-- TABLE: OrderItem
-- Subtotal is intentionally stored — locked at order time (prices change).
-- CRITICAL: Must be created BEFORE InventoryLog.
-- =============================================================================
CREATE TABLE OrderItem (
    OrderItemId      INT           NOT NULL IDENTITY(1,1),
    OrderId          INT           NOT NULL,
    ProductId        INT           NOT NULL,
    ProductVariantId INT           NULL,
    Quantity         INT           NOT NULL,
    UnitPrice        DECIMAL(10,2) NOT NULL,
    -- Subtotal removed in v7.0 (3NF Fix 7): derived from Quantity × UnitPrice.
    -- Computed in vw_OrderItemDetail. UnitPrice is already locked at order
    -- time so all historical pricing is fully preserved.
    CONSTRAINT PK_OrderItem           PRIMARY KEY (OrderItemId),
    CONSTRAINT FK_OrderItem_Order     FOREIGN KEY (OrderId)           REFERENCES [Order](OrderId)                  ON DELETE CASCADE,
    CONSTRAINT FK_OrderItem_Product   FOREIGN KEY (ProductId)         REFERENCES Product(ProductId),
    CONSTRAINT FK_OrderItem_Variant   FOREIGN KEY (ProductVariantId)  REFERENCES ProductVariant(ProductVariantId),
    CONSTRAINT CK_OrderItem_Quantity  CHECK (Quantity > 0),
    CONSTRAINT CK_OrderItem_UnitPrice CHECK (UnitPrice >= 0)
);
GO
CREATE INDEX IX_OrderItem_OrderId           ON OrderItem(OrderId);
CREATE INDEX IX_OrderItem_ProductId         ON OrderItem(ProductId);
CREATE INDEX IX_OrderItem_ProductVariantId  ON OrderItem(ProductVariantId);
GO

-- =============================================================================
-- TABLE: VoucherUsage
-- CRITICAL: Must be created BEFORE InventoryLog.
-- =============================================================================
CREATE TABLE VoucherUsage (
    VoucherUsageId INT           NOT NULL IDENTITY(1,1),
    VoucherId      INT           NOT NULL,
    UserId         INT           NOT NULL,
    OrderId        INT           NOT NULL,
    DiscountAmount DECIMAL(10,2) NOT NULL,
    UsedAt         DATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_VoucherUsage         PRIMARY KEY (VoucherUsageId),
    CONSTRAINT FK_VoucherUsage_Voucher FOREIGN KEY (VoucherId) REFERENCES Voucher(VoucherId),
    CONSTRAINT FK_VoucherUsage_User    FOREIGN KEY (UserId)    REFERENCES [User](UserId),
    CONSTRAINT FK_VoucherUsage_Order   FOREIGN KEY (OrderId)   REFERENCES [Order](OrderId),
    CONSTRAINT CK_VoucherUsage_Disc    CHECK (DiscountAmount >= 0)
);
GO
CREATE INDEX IX_VoucherUsage_VoucherId ON VoucherUsage(VoucherId);
CREATE INDEX IX_VoucherUsage_UserId    ON VoucherUsage(UserId);
CREATE INDEX IX_VoucherUsage_OrderId   ON VoucherUsage(OrderId);
GO

-- =============================================================================
-- TABLE: InventoryLog
-- Covers: DB4 — stock movement history (Part 12)
-- ChangeType 'Lock'/'Unlock' map to Part 4 (lock on order) and
-- Part 6 (unlock on delivery confirmed or cancellation).
-- =============================================================================
CREATE TABLE InventoryLog (
    InventoryLogId   INT           NOT NULL IDENTITY(1,1),
    ProductId        INT           NOT NULL,
    ProductVariantId INT           NULL,
    ChangeQuantity   INT           NOT NULL,
    ChangeType       NVARCHAR(50)  NOT NULL,
    OrderId          INT           NULL,
    PurchaseOrderId  INT           NULL,
    ChangedByUserId  INT           NULL,
    Notes            NVARCHAR(500) NULL,
    CreatedAt        DATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_InventoryLog         PRIMARY KEY (InventoryLogId),
    CONSTRAINT FK_InvLog_Product       FOREIGN KEY (ProductId)        REFERENCES Product(ProductId),
    CONSTRAINT FK_InvLog_Variant       FOREIGN KEY (ProductVariantId) REFERENCES ProductVariant(ProductVariantId),
    CONSTRAINT FK_InvLog_Order         FOREIGN KEY (OrderId)          REFERENCES [Order](OrderId),
    CONSTRAINT FK_InvLog_PurchaseOrder FOREIGN KEY (PurchaseOrderId)  REFERENCES PurchaseOrder(PurchaseOrderId),
    CONSTRAINT FK_InvLog_User          FOREIGN KEY (ChangedByUserId)  REFERENCES [User](UserId),
    CONSTRAINT CK_InvLog_ChangeType    CHECK (ChangeType IN (
        'Purchase',    -- stock received from supplier
        'Sale',        -- stock deducted on confirmed order
        'Return',      -- stock returned by customer
        'Adjustment',  -- manual admin correction
        'Damage',      -- damaged stock written off
        'Loss',        -- lost/missing stock
        'Lock',        -- stock reserved pending payment confirmation
        'Unlock'       -- reserved stock released (cancelled or delivered)
    ))
);
GO
CREATE INDEX IX_InvLog_ProductId        ON InventoryLog(ProductId);
CREATE INDEX IX_InvLog_ProductVariantId ON InventoryLog(ProductVariantId);
CREATE INDEX IX_InvLog_ChangeType       ON InventoryLog(ChangeType);
CREATE INDEX IX_InvLog_CreatedAt        ON InventoryLog(CreatedAt);
CREATE INDEX IX_InvLog_OrderId          ON InventoryLog(OrderId);
CREATE INDEX IX_InvLog_PurchaseOrderId  ON InventoryLog(PurchaseOrderId);
GO

-- =============================================================================
-- TABLE: Payment
-- Covers: DB3 — payment records
-- v5.0 (3NF Fix 3): Stripped to method-agnostic fields only.
--   All method-specific columns moved to subtype tables below.
--
-- PaymentMethod values:
--   'GCash'        — online; subtype row in GCashPayment
--   'BankTransfer' — online; subtype row in BankTransferPayment
--   'Cash'         — walk-in POS ONLY; no subtype row needed
--   COD            — NOT offered by Taurus Bike Shop
-- =============================================================================
CREATE TABLE Payment (
    PaymentId     INT           NOT NULL IDENTITY(1,1),
    OrderId       INT           NOT NULL,
    PaymentMethod NVARCHAR(50)  NOT NULL,
    PaymentStage  NVARCHAR(20)  NOT NULL DEFAULT 'Upfront',
    PaymentStatus NVARCHAR(50)  NOT NULL,
    Amount        DECIMAL(10,2) NOT NULL,
    PaymentDate   DATETIME      NULL,
    CreatedAt     DATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Payment        PRIMARY KEY (PaymentId),
    CONSTRAINT FK_Payment_Order  FOREIGN KEY (OrderId) REFERENCES [Order](OrderId),
    CONSTRAINT CK_Payment_Method CHECK (PaymentMethod IN ('GCash','BankTransfer','Cash')),
    CONSTRAINT CK_Payment_Stage  CHECK (PaymentStage  IN ('Upfront','Confirmation')),
    CONSTRAINT CK_Payment_Status CHECK (PaymentStatus IN (
        'Pending',
        'VerificationPending',   -- bank transfer uploaded, awaiting admin review
        'VerificationRejected',  -- admin rejected the proof
        'Completed',
        'Failed'
    )),
    CONSTRAINT CK_Payment_Amount CHECK (Amount >= 0)
);
GO
CREATE INDEX IX_Payment_OrderId  ON Payment(OrderId);
CREATE INDEX IX_Payment_Method   ON Payment(PaymentMethod);
CREATE INDEX IX_Payment_Stage    ON Payment(PaymentStage);
CREATE INDEX IX_Payment_Status   ON Payment(PaymentStatus);
GO

-- =============================================================================
-- TABLE: GCashPayment  (NEW in v5.0 — 3NF Fix 3)
-- One-to-one with Payment. Created only when PaymentMethod = 'GCash'.
-- GcashTransactionId: reference number returned by the GCash gateway (EXT1).
-- NULL while PaymentStatus is Pending or Failed.
-- =============================================================================
CREATE TABLE GCashPayment (
    PaymentId          INT           NOT NULL,
    GcashTransactionId NVARCHAR(255) NULL,
    CONSTRAINT PK_GCashPayment     PRIMARY KEY (PaymentId),
    CONSTRAINT FK_GCashPayment_Pmt FOREIGN KEY (PaymentId) REFERENCES Payment(PaymentId) ON DELETE CASCADE
);
GO
CREATE INDEX IX_GCashPayment_TxnId ON GCashPayment(GcashTransactionId) WHERE GcashTransactionId IS NOT NULL;
GO

-- =============================================================================
-- TABLE: BankTransferPayment  (NEW in v5.0 — 3NF Fix 3)
-- One-to-one with Payment. Created only when PaymentMethod = 'BankTransfer'.
-- Holds all bank-transfer-specific data:
--   BpiReferenceNumber  — reference from customer's bank
--   Proof columns       — GCS URL/bucket/path of uploaded screenshot
--   Verification fields — admin who verified/rejected, notes, timestamps
--   VerificationDeadline — 24-hr auto-hold cutoff (polled by Part 14, Job 3)
-- =============================================================================
CREATE TABLE BankTransferPayment (
    PaymentId            INT            NOT NULL,
    BpiReferenceNumber   NVARCHAR(255)  NULL,
    ProofUrl             NVARCHAR(1000) NULL,
    ProofStorageBucket   NVARCHAR(200)  NULL,
    ProofStoragePath     NVARCHAR(1000) NULL,
    VerifiedByUserId     INT            NULL,
    VerificationNotes    NVARCHAR(500)  NULL,
    VerifiedAt           DATETIME       NULL,
    VerificationDeadline DATETIME       NULL,
    CONSTRAINT PK_BankTransferPayment        PRIMARY KEY (PaymentId),
    CONSTRAINT FK_BankTransferPayment_Pmt    FOREIGN KEY (PaymentId)        REFERENCES Payment(PaymentId)  ON DELETE CASCADE,
    CONSTRAINT FK_BankTransferPayment_Admin  FOREIGN KEY (VerifiedByUserId) REFERENCES [User](UserId)
);
GO
CREATE INDEX IX_BTP_BpiRef
    ON BankTransferPayment(BpiReferenceNumber) WHERE BpiReferenceNumber IS NOT NULL;
CREATE INDEX IX_BTP_VerificationDeadline
    ON BankTransferPayment(VerificationDeadline) WHERE VerificationDeadline IS NOT NULL;
GO

-- =============================================================================
-- TABLE: Delivery
-- Covers: DB3; EXT2 (Lalamove), EXT3 (LBC)
-- v6.0 (3NF Fix 4): Courier-specific columns extracted to subtype tables.
--   Base table holds only courier-agnostic fields.
--   LalamoveDelivery — BookingRef, DriverName, DriverPhone (Lalamove-only)
--   LBCDelivery      — TrackingNumber (LBC-only; no driver info from LBC API)
-- DeliveryStatus 'Failed' covers Part 5 & 8 failure paths.
-- IsDelayed / DelayedUntil cover Part 10 admin delay management.
-- =============================================================================
CREATE TABLE Delivery (
    DeliveryId            INT          NOT NULL IDENTITY(1,1),
    OrderId               INT          NOT NULL,
    Courier               NVARCHAR(20) NOT NULL,
    DeliveryStatus        NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    IsDelayed             BIT          NOT NULL DEFAULT 0,
    DelayedUntil          DATETIME     NULL,
    EstimatedDeliveryTime DATETIME     NULL,
    ActualDeliveryTime    DATETIME     NULL,
    CreatedAt             DATETIME     NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Delivery         PRIMARY KEY (DeliveryId),
    CONSTRAINT FK_Delivery_Order   FOREIGN KEY (OrderId) REFERENCES [Order](OrderId),
    CONSTRAINT CK_Delivery_Courier CHECK (Courier IN ('Lalamove','LBC')),
    CONSTRAINT CK_Delivery_Status  CHECK (DeliveryStatus IN (
        'Pending',    -- accepted, not yet picked up by courier
        'PickedUp',   -- courier collected the package
        'InTransit',  -- on the way to customer
        'Delivered',  -- customer confirmed receipt
        'Failed'      -- delivery attempt failed (Parts 5 & 8)
    ))
);
GO
CREATE INDEX IX_Delivery_OrderId   ON Delivery(OrderId);
CREATE INDEX IX_Delivery_Courier   ON Delivery(Courier);
CREATE INDEX IX_Delivery_Status    ON Delivery(DeliveryStatus);
CREATE INDEX IX_Delivery_IsDelayed ON Delivery(IsDelayed) WHERE IsDelayed = 1;
GO

-- =============================================================================
-- TABLE: LalamoveDelivery  (NEW in v6.0 — 3NF Fix 4)
-- One-to-one with Delivery. Created only when Courier = 'Lalamove'.
-- Holds all Lalamove-specific fields from EXT2 (Lalamove API):
--   BookingRef  — reference number returned by Lalamove booking API
--   DriverName  — driver assigned by Lalamove (from driver assignment webhook)
--   DriverPhone — driver contact number
-- NULL while DeliveryStatus = 'Pending' (driver not yet assigned).
-- =============================================================================
CREATE TABLE LalamoveDelivery (
    DeliveryId  INT           NOT NULL,
    BookingRef  NVARCHAR(255) NULL,   -- NULL until Lalamove confirms booking
    DriverName  NVARCHAR(100) NULL,   -- NULL until driver assigned
    DriverPhone NVARCHAR(20)  NULL,   -- NULL until driver assigned
    CONSTRAINT PK_LalamoveDelivery       PRIMARY KEY (DeliveryId),
    CONSTRAINT FK_LalamoveDelivery_Base  FOREIGN KEY (DeliveryId) REFERENCES Delivery(DeliveryId) ON DELETE CASCADE
);
GO
CREATE INDEX IX_LalamoveDelivery_BookingRef
    ON LalamoveDelivery(BookingRef) WHERE BookingRef IS NOT NULL;
GO

-- =============================================================================
-- TABLE: LBCDelivery  (NEW in v6.0 — 3NF Fix 4)
-- One-to-one with Delivery. Created only when Courier = 'LBC'.
-- Holds LBC-specific fields from EXT3 (LBC API):
--   TrackingNumber — LBC waybill/tracking number for customer self-tracking
-- LBC does not expose driver details via API — no driver columns here.
-- =============================================================================
CREATE TABLE LBCDelivery (
    DeliveryId     INT           NOT NULL,
    TrackingNumber NVARCHAR(255) NULL,   -- NULL until LBC waybill is generated
    CONSTRAINT PK_LBCDelivery       PRIMARY KEY (DeliveryId),
    CONSTRAINT FK_LBCDelivery_Base  FOREIGN KEY (DeliveryId) REFERENCES Delivery(DeliveryId) ON DELETE CASCADE
);
GO
CREATE INDEX IX_LBCDelivery_TrackingNumber
    ON LBCDelivery(TrackingNumber) WHERE TrackingNumber IS NOT NULL;
GO

-- =============================================================================
-- TABLE: Review
-- Covers: Part 6 — post-delivery review & rating
-- =============================================================================
CREATE TABLE Review (
    ReviewId           INT            NOT NULL IDENTITY(1,1),
    UserId             INT            NOT NULL,
    ProductId          INT            NOT NULL,
    OrderId            INT            NOT NULL,
    Rating             INT            NOT NULL,
    Comment            NVARCHAR(1000) NULL,
    IsVerifiedPurchase BIT            NOT NULL DEFAULT 0,
    CreatedAt          DATETIME       NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Review         PRIMARY KEY (ReviewId),
    CONSTRAINT FK_Review_User    FOREIGN KEY (UserId)    REFERENCES [User](UserId),
    CONSTRAINT FK_Review_Product FOREIGN KEY (ProductId) REFERENCES Product(ProductId),
    CONSTRAINT FK_Review_Order   FOREIGN KEY (OrderId)   REFERENCES [Order](OrderId),
    CONSTRAINT CK_Review_Rating  CHECK (Rating >= 1 AND Rating <= 5)
);
GO
CREATE INDEX IX_Review_ProductId ON Review(ProductId);
CREATE INDEX IX_Review_UserId    ON Review(UserId);
CREATE INDEX IX_Review_OrderId   ON Review(OrderId);
CREATE INDEX IX_Review_Rating    ON Review(Rating);
GO

-- =============================================================================
-- TABLE: POS_Session
-- Covers: Part 11 — walk-in POS terminal session tracking
-- =============================================================================
CREATE TABLE POS_Session (
    POSSessionId INT           NOT NULL IDENTITY(1,1),
    UserId       INT           NOT NULL,
    TerminalName NVARCHAR(50)  NOT NULL,
    ShiftStart   DATETIME      NOT NULL,
    ShiftEnd     DATETIME      NULL,
    TotalSales   DECIMAL(10,2) NOT NULL DEFAULT 0,
    CreatedAt    DATETIME      NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_POS_Session      PRIMARY KEY (POSSessionId),
    CONSTRAINT FK_POS_Session_User FOREIGN KEY (UserId) REFERENCES [User](UserId)
);
GO
CREATE INDEX IX_POS_Session_UserId     ON POS_Session(UserId);
CREATE INDEX IX_POS_Session_ShiftStart ON POS_Session(ShiftStart);
CREATE INDEX IX_POS_Session_ShiftEnd   ON POS_Session(ShiftEnd);
GO

-- =============================================================================
-- TABLE: SupportTicket
-- Covers: DB6 — Part 13, Part 6 (damage report), Part 2, Part 5
-- Attachments follow the same GCS pattern as ProductImage.
-- =============================================================================
CREATE TABLE SupportTicket (
    TicketId         INT            NOT NULL IDENTITY(1,1),
    UserId           INT            NOT NULL,
    OrderId          INT            NULL,
    TicketSource     NVARCHAR(50)   NOT NULL DEFAULT 'Customer',
    TicketCategory   NVARCHAR(100)  NOT NULL,
    Subject          NVARCHAR(200)  NOT NULL,
    [Description]    NVARCHAR(MAX)  NULL,
    AttachmentUrl    NVARCHAR(1000) NULL,
    AttachmentBucket NVARCHAR(200)  NULL,
    AttachmentPath   NVARCHAR(1000) NULL,
    TicketStatus     NVARCHAR(50)   NOT NULL DEFAULT 'Open',
    AssignedToUserId INT            NULL,
    ResolvedAt       DATETIME       NULL,
    CreatedAt        DATETIME       NOT NULL DEFAULT GETDATE(),
    UpdatedAt        DATETIME       NULL,
    CONSTRAINT PK_SupportTicket        PRIMARY KEY (TicketId),
    CONSTRAINT FK_Ticket_User          FOREIGN KEY (UserId)           REFERENCES [User](UserId),
    CONSTRAINT FK_Ticket_Order         FOREIGN KEY (OrderId)          REFERENCES [Order](OrderId),
    CONSTRAINT FK_Ticket_AssignedTo    FOREIGN KEY (AssignedToUserId) REFERENCES [User](UserId),
    CONSTRAINT CK_Ticket_Source        CHECK (TicketSource IN ('Customer','Admin','System')),
    CONSTRAINT CK_Ticket_Category      CHECK (TicketCategory IN (
        'DamagedItem','WrongItem','DeliveryIssue',
        'PaymentIssue','ProductInquiry','General'
    )),
    CONSTRAINT CK_Ticket_Status        CHECK (TicketStatus IN (
        'Open','InProgress','AwaitingResponse','Resolved','Closed'
    ))
);
GO
CREATE INDEX IX_Ticket_UserId     ON SupportTicket(UserId);
CREATE INDEX IX_Ticket_OrderId    ON SupportTicket(OrderId)          WHERE OrderId IS NOT NULL;
CREATE INDEX IX_Ticket_Status     ON SupportTicket(TicketStatus);
CREATE INDEX IX_Ticket_Category   ON SupportTicket(TicketCategory);
CREATE INDEX IX_Ticket_AssignedTo ON SupportTicket(AssignedToUserId) WHERE AssignedToUserId IS NOT NULL;
CREATE INDEX IX_Ticket_CreatedAt  ON SupportTicket(CreatedAt);
GO

-- =============================================================================
-- TABLE: SupportTicketReply
-- Covers: Part 13 — threaded replies per ticket
-- IsAdminReply = 1 → staff; 0 → customer
-- =============================================================================
CREATE TABLE SupportTicketReply (
    ReplyId          INT            NOT NULL IDENTITY(1,1),
    TicketId         INT            NOT NULL,
    UserId           INT            NOT NULL,
    IsAdminReply     BIT            NOT NULL DEFAULT 0,
    Message          NVARCHAR(MAX)  NOT NULL,
    AttachmentUrl    NVARCHAR(1000) NULL,
    AttachmentBucket NVARCHAR(200)  NULL,
    AttachmentPath   NVARCHAR(1000) NULL,
    CreatedAt        DATETIME       NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_SupportTicketReply PRIMARY KEY (ReplyId),
    CONSTRAINT FK_Reply_Ticket       FOREIGN KEY (TicketId) REFERENCES SupportTicket(TicketId) ON DELETE CASCADE,
    CONSTRAINT FK_Reply_User         FOREIGN KEY (UserId)   REFERENCES [User](UserId)
);
GO
CREATE INDEX IX_Reply_TicketId  ON SupportTicketReply(TicketId);
CREATE INDEX IX_Reply_UserId    ON SupportTicketReply(UserId);
CREATE INDEX IX_Reply_CreatedAt ON SupportTicketReply(CreatedAt);
GO

-- =============================================================================
-- TABLE: Notification
-- Covers: DB6 (Notification Queue) — referenced in 11 of 14 flowcharts
-- Every outbound Email/SMS is queued here before dispatch via EXT4.
-- Background Job (Part 14) polls vw_PendingNotifications and updates Status.
-- =============================================================================
CREATE TABLE Notification (
    NotificationId INT            NOT NULL IDENTITY(1,1),
    UserId         INT            NOT NULL,
    OrderId        INT            NULL,
    TicketId       INT            NULL,
    Channel        NVARCHAR(20)   NOT NULL,
    NotifType      NVARCHAR(100)  NOT NULL,
    Recipient      NVARCHAR(255)  NOT NULL,
    Subject        NVARCHAR(255)  NULL,
    Body           NVARCHAR(MAX)  NULL,
    [Status]       NVARCHAR(20)   NOT NULL DEFAULT 'Pending',
    RetryCount     INT            NOT NULL DEFAULT 0,
    SentAt         DATETIME       NULL,
    FailureReason  NVARCHAR(500)  NULL,
    CreatedAt      DATETIME       NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Notification      PRIMARY KEY (NotificationId),
    CONSTRAINT FK_Notif_User        FOREIGN KEY (UserId)   REFERENCES [User](UserId),
    CONSTRAINT FK_Notif_Order       FOREIGN KEY (OrderId)  REFERENCES [Order](OrderId),
    CONSTRAINT FK_Notif_Ticket      FOREIGN KEY (TicketId) REFERENCES SupportTicket(TicketId),
    CONSTRAINT CK_Notif_Channel     CHECK (Channel IN ('Email','SMS')),
    CONSTRAINT CK_Notif_Type        CHECK (NotifType IN (
        'OTPCode','WelcomeEmail',
        'OrderConfirmation','PaymentReceived','PaymentRejected','PaymentHeld',
        'TrackingUpdate','ReadyForPickup','PickupExpiry',
        'DeliveryDelay','DeliveryConfirmation',
        'WishlistRestock',
        'SupportTicketCreated','SupportTicketReply','SupportTicketResolved',
        'LowStockAlert','PendingOrderAlert'
    )),
    CONSTRAINT CK_Notif_Status CHECK ([Status] IN ('Pending','Sent','Failed'))
);
GO
CREATE INDEX IX_Notif_UserId    ON Notification(UserId);
CREATE INDEX IX_Notif_OrderId   ON Notification(OrderId)  WHERE OrderId  IS NOT NULL;
CREATE INDEX IX_Notif_TicketId  ON Notification(TicketId) WHERE TicketId IS NOT NULL;
CREATE INDEX IX_Notif_Status    ON Notification([Status]);
CREATE INDEX IX_Notif_Channel   ON Notification(Channel);
CREATE INDEX IX_Notif_CreatedAt ON Notification(CreatedAt);
CREATE INDEX IX_Notif_Pending   ON Notification([Status], CreatedAt) WHERE [Status] = 'Pending';
GO

-- =============================================================================
-- TABLE: SystemLog
-- Covers: audit trail for background jobs (Part 14) and admin actions
-- =============================================================================
CREATE TABLE SystemLog (
    SystemLogId      INT            NOT NULL IDENTITY(1,1),
    UserId           INT            NULL,
    EventType        NVARCHAR(100)  NOT NULL,
    EventDescription NVARCHAR(1000) NULL,
    CreatedAt        DATETIME       NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_SystemLog       PRIMARY KEY (SystemLogId),
    CONSTRAINT FK_SystemLog_User  FOREIGN KEY (UserId) REFERENCES [User](UserId),
    CONSTRAINT CK_SystemLog_Event CHECK (EventType IN (
        'Login','Logout','AccessDenied','UserCreated',
        'ProductUpdate','VoucherCreated',
        'OrderStatusChange','PaymentProcessed','PaymentVerified',
        'PaymentRejected','PaymentTimeout',
        'InventoryAdjustment','InventorySync','LowStockTriggered',
        'DeliveryStatusPoll','DeliveryDelayed','DeliveryFailed',
        'BackgroundJobStart','BackgroundJobComplete','BackgroundJobError',
        'SupportTicketCreated','SupportTicketResolved'
    ))
);
GO
CREATE INDEX IX_SystemLog_EventType ON SystemLog(EventType);
CREATE INDEX IX_SystemLog_CreatedAt ON SystemLog(CreatedAt);
CREATE INDEX IX_SystemLog_UserId    ON SystemLog(UserId);
GO


-- =============================================================================
-- VIEWS
-- =============================================================================

-- Voucher usage count per user
CREATE VIEW vw_UserVoucherUsageCount AS
SELECT UserId, VoucherId, COUNT(*) AS TimesUsed
FROM VoucherUsage
GROUP BY UserId, VoucherId;
GO

-- Active products with brand, category, primary image
CREATE VIEW vw_ActiveProducts AS
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

-- Product image gallery with GCS storage paths
CREATE VIEW vw_ProductImageGallery AS
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

-- Order summary — TotalAmount computed (not stored) in v7.0
CREATE VIEW vw_OrderSummary AS
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
    o.OrderId, o.OrderNumber, o.OrderDate, o.OrderStatus, o.SubTotal, o.DiscountAmount, o.ShippingFee, o.IsWalkIn,
    u.FirstName, u.LastName, u.Email, u.PhoneNumber,
    a.Street, a.City, a.PostalCode, a.Province,
    po.PickupReadyAt, po.PickupExpiresAt, po.PickupConfirmedAt;
GO

-- Product variant details with computed total price
CREATE VIEW vw_ProductVariantDetails AS
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

-- Inventory status — all stock lives in ProductVariant (v7.0)
-- Products with no real variants have one row with VariantName = 'Default'.
CREATE VIEW vw_InventoryStatus AS
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

-- Pending notifications for background job (Part 14) — max 3 retries
CREATE VIEW vw_PendingNotifications AS
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

-- Open support tickets — admin Part 13 dashboard
CREATE VIEW vw_OpenSupportTickets AS
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

-- Full payment detail — joins Payment base with both subtype tables
-- Subtype columns are NULL when not applicable to the payment method.
CREATE VIEW vw_PaymentDetail AS
SELECT
    p.PaymentId, p.OrderId, p.PaymentMethod,
    p.PaymentStage, p.PaymentStatus,
    p.Amount, p.PaymentDate, p.CreatedAt,
    -- GCash fields
    g.GcashTransactionId,
    -- BankTransfer fields
    bt.BpiReferenceNumber,
    bt.ProofUrl             AS PaymentProofUrl,
    bt.ProofStorageBucket,
    bt.ProofStoragePath,
    bt.VerifiedByUserId,
    bt.VerificationNotes,
    bt.VerifiedAt,
    bt.VerificationDeadline
FROM Payment p
LEFT JOIN GCashPayment          g  ON p.PaymentId = g.PaymentId
LEFT JOIN BankTransferPayment   bt ON p.PaymentId = bt.PaymentId;
GO


-- Full delivery detail — joins Delivery base with both subtype tables.
-- Subtype columns are NULL when not applicable to the courier.
CREATE VIEW vw_DeliveryDetail AS
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


-- Order item detail — Subtotal computed (not stored) in v7.0
CREATE VIEW vw_OrderItemDetail AS
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

-- Purchase order detail — item subtotals and order total computed in v7.0
CREATE VIEW vw_PurchaseOrderDetail AS
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


-- =============================================================================
-- v7.1 PATCH — merged inline (previously a separate patch file)
-- =============================================================================

-- =============================================================================
-- CHANGE 1: Add ReorderThreshold to ProductVariant
-- Enables Part 12 (stock update low-stock check) and
-- Part 14 Job 4 (stock level monitor) to compare StockQuantity
-- against a stored threshold rather than a hardcoded value.
-- DEFAULT 5 means low-stock alert triggers when fewer than 5 units remain.
-- =============================================================================
ALTER TABLE ProductVariant
    ADD ReorderThreshold INT NOT NULL DEFAULT 5;
GO

ALTER TABLE ProductVariant
    ADD CONSTRAINT CK_ProductVariant_ReorderThreshold
    CHECK (ReorderThreshold >= 0);
GO

CREATE INDEX IX_ProductVariant_ReorderThreshold
    ON ProductVariant(ReorderThreshold);
GO

-- =============================================================================
-- CHANGE 2: Create SupportTask table
-- Covers: Part 13 — "Create Follow-up Task" action branch
-- (e.g., "Ship replacement", "Arrange return pickup").
-- Distinct from SupportTicketReply — this is an actionable
-- work item with its own assignee, due date, and lifecycle.
-- TaskType  : ShipReplacement | ArrangeReturn | ContactSupplier | Other
-- TaskStatus: Pending → InProgress → Done | Cancelled
-- =============================================================================
CREATE TABLE SupportTask (
    TaskId           INT           NOT NULL IDENTITY(1,1),
    TicketId         INT           NOT NULL,
    AssignedToUserId INT           NULL,
    TaskType         NVARCHAR(50)  NOT NULL,
    TaskStatus       NVARCHAR(20)  NOT NULL DEFAULT 'Pending',
    DueDate          DATETIME      NULL,
    Notes            NVARCHAR(500) NULL,
    CreatedAt        DATETIME      NOT NULL DEFAULT GETDATE(),
    CompletedAt      DATETIME      NULL,
    CONSTRAINT PK_SupportTask            PRIMARY KEY (TaskId),
    CONSTRAINT FK_SupportTask_Ticket     FOREIGN KEY (TicketId)         REFERENCES SupportTicket(TicketId) ON DELETE CASCADE,
    CONSTRAINT FK_SupportTask_AssignedTo FOREIGN KEY (AssignedToUserId) REFERENCES [User](UserId),
    CONSTRAINT CK_SupportTask_Type       CHECK (TaskType IN (
        'ShipReplacement',
        'ArrangeReturn',
        'ContactSupplier',
        'Other'
    )),
    CONSTRAINT CK_SupportTask_Status     CHECK (TaskStatus IN (
        'Pending',
        'InProgress',
        'Done',
        'Cancelled'
    ))
);
GO

CREATE INDEX IX_SupportTask_TicketId         ON SupportTask(TicketId);
CREATE INDEX IX_SupportTask_AssignedToUserId ON SupportTask(AssignedToUserId) WHERE AssignedToUserId IS NOT NULL;
CREATE INDEX IX_SupportTask_TaskStatus       ON SupportTask(TaskStatus);
CREATE INDEX IX_SupportTask_DueDate          ON SupportTask(DueDate) WHERE DueDate IS NOT NULL;
GO


-- =============================================================================
-- SUMMARY
-- =============================================================================
PRINT '==============================================';
PRINT 'Taurus_schema.sql  —  Completed Successfully!';
PRINT '==============================================';
PRINT 'Version  : 7.1  (fully 3NF-compliant)';
PRINT 'Platform : Google Cloud SQL for SQL Server';
PRINT '----------------------------------------------';
PRINT 'Tables   : 38 created (37 base + SupportTask [v7.1])';
PRINT '  Auth   : Role, User, UserRole';
PRINT '  Address: Address                 [v5.0 - Fix 1]';
PRINT '  Auth2  : OTPVerification, GuestSession';
PRINT '  Catalog: Category, Brand, Product (no StockQty) [v7.0 - Fix 5]';
PRINT '           PriceHistory, ProductVariant, ProductImage';
PRINT '  Supply : Supplier, PurchaseOrder (no TotalAmt) [v7.0 - Fix 8+9]';
PRINT '           PurchaseOrderItem (no Subtotal)';
PRINT '  Voucher: Voucher, UserVoucher, VoucherUsage';
PRINT '  Cart   : Cart (guest-aware), CartItem, Wishlist';
PRINT '  Order  : Order (no TotalAmount)  [v7.0 - Fix 6]';
PRINT '           PickupOrder            [v5.0 - Fix 2]';
PRINT '           OrderItem (no Subtotal) [v7.0 - Fix 7]';
PRINT '  Inv    : InventoryLog';
PRINT '  Payment: Payment (base), GCashPayment,          [v5.0 - Fix 3]';
PRINT '           BankTransferPayment';
PRINT '  Deliver: Delivery (base), LalamoveDelivery,     [v6.0 - Fix 4]';
PRINT '           LBCDelivery';
PRINT '  Review : Review';
PRINT '  POS    : POS_Session';
PRINT '  Support: SupportTicket, SupportTicketReply, SupportTask [v7.1]';
PRINT '  Notif  : Notification';
PRINT '  System : SystemLog';
PRINT '----------------------------------------------';
PRINT 'Views    : 12 created';
PRINT '  vw_UserVoucherUsageCount';
PRINT '  vw_ActiveProducts';
PRINT '  vw_ProductImageGallery';
PRINT '  vw_OrderSummary        (computes TotalAmount)  [v7.0]';
PRINT '  vw_OrderItemDetail     (computes Subtotal)     [NEW v7.0]';
PRINT '  vw_ProductVariantDetails';
PRINT '  vw_InventoryStatus     (single-source stock)   [v7.0]';
PRINT '  vw_PurchaseOrderDetail (computes Subtotal+Total) [NEW v7.0]';
PRINT '  vw_PendingNotifications';
PRINT '  vw_OpenSupportTickets';
PRINT '  vw_PaymentDetail';
PRINT '  vw_DeliveryDetail';
PRINT '----------------------------------------------';
PRINT 'Triggers : 1 (TR_Product_PriceAudit)';
PRINT '----------------------------------------------';
PRINT '3NF Fixes — full history:';
PRINT '  Fix 1 (v5.0): Address table — PostalCode→City dep';
PRINT '  Fix 2 (v5.0): PickupOrder — sparse pickup cols on Order';
PRINT '  Fix 3 (v5.0): GCash + BankTransfer payment subtypes';
PRINT '  Fix 4 (v6.0): Lalamove + LBC delivery subtypes';
PRINT '  Fix 5 (v7.0): Product.StockQuantity removed — stock owned';
PRINT '                solely by ProductVariant';
PRINT '  Fix 6 (v7.0): Order.TotalAmount removed — computed in';
PRINT '                vw_OrderSummary';
PRINT '  Fix 7 (v7.0): OrderItem.Subtotal removed — computed in';
PRINT '                vw_OrderItemDetail';
PRINT '  Fix 8 (v7.0): PurchaseOrderItem.Subtotal removed — computed';
PRINT '                in vw_PurchaseOrderDetail';
PRINT '  Fix 9 (v7.0): PurchaseOrder.TotalAmount removed — computed';
PRINT '                in vw_PurchaseOrderDetail';
PRINT '----------------------------------------------';
PRINT 'Schema is now fully 3NF-compliant.';
PRINT 'Every non-key column depends on the PK, the whole PK,';
PRINT 'and nothing but the PK.';
PRINT '----------------------------------------------';
PRINT 'COD NOTE : Not offered. Methods: GCash|BankTransfer|Cash(POS)';
PRINT 'GCS NOTE : Images/proofs/attachments in Google Cloud Storage.';
PRINT '           Only URLs + bucket/path metadata stored in DB.';
PRINT '----------------------------------------------';
PRINT 'VARIANT NOTE: Products without real variants must have one';
PRINT '              ProductVariant row with VariantName = ''Default''.';
PRINT '              Stock is tracked exclusively via ProductVariant.';
PRINT '----------------------------------------------';
PRINT 'v7.1 additions: ProductVariant.ReorderThreshold, SupportTask table';
PRINT 'Run Taurus_seed.sql next to populate data.';
PRINT '==============================================';
GO