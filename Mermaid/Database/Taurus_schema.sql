-- =============================================================================
-- Taurus Bike Shop  |  TaurusBikeShopDB
-- File    : Taurus_schema.sql
-- Purpose : Create all tables, indexes, views, and triggers
-- Platform: Google Cloud SQL — SQL Server (Cloud SQL for SQL Server)
--           Compatible with SQL Server 2019+ dialect
--
-- Version : 4.0
-- Changes from v3.1:
--   Platform   — Migrated from FreeASPHosting (SQL Server shared) to
--                Google Cloud SQL for SQL Server. No dialect changes needed;
--                standard T-SQL works on Cloud SQL for SQL Server.
--                Connection is managed via Cloud SQL Auth Proxy or
--                direct IP in Google Cloud Console > SQL > Connections.
--
--   New Tables —
--     + OTPVerification      : Stores registration OTP codes with expiry
--     + GuestSession         : Tracks guest checkout sessions (no account)
--     + SupportTicket        : Full support ticket lifecycle (Part 13)
--     + SupportTicketReply   : Thread of replies per ticket
--     + Notification         : Outbound email/SMS queue (DB6 from flowcharts)
--
--   Modified Tables —
--     [User]         — UserId now also linked from GuestSession
--     Cart           — UserId made nullable; GuestSessionId added for guests
--     [Order]        — Added OrderStatus values: ReadyForPickup, PickedUp,
--                      PendingVerification, OnHold
--                    — Added pickup expiry columns: PickupReadyAt,
--                      PickupExpiresAt, PickupConfirmedAt
--     Payment        — Added PaymentProofUrl (bank transfer screenshot)
--                    — Added VerifiedByUserId, VerificationNotes, VerifiedAt
--                    — Removed COD (not offered by Taurus for delivery)
--                    — Cash retained exclusively for walk-in POS
--     Delivery       — Added DeliveryStatus: 'Failed'
--                    — Added IsDelayed BIT and DelayedUntil DATETIME
--     InventoryLog   — Added ChangeType values: 'Lock', 'Unlock'
--     SystemLog      — Expanded EventType list for background job events
--     ProductImage   — Clarified: stores URL only (actual file on GCS)
--                    — Added StorageBucket, StoragePath for GCS reference
--
--   COD NOTE: Cash on Delivery is NOT offered by Taurus Bike Shop.
--             PaymentMethod CHECK allows 'GCash' and 'BankTransfer' for
--             online/delivery orders. 'Cash' is walk-in POS only.
--             COD has been deliberately excluded from all constraints.
--
-- HOW TO RUN (Google Cloud SQL):
--   Option A — Cloud SQL Studio (in Google Cloud Console):
--     1. Go to Cloud SQL > Your Instance > Cloud SQL Studio
--     2. Authenticate and select your database
--     3. Open a new SQL editor tab, paste this script, and Run
--
--   Option B — Cloud SQL Auth Proxy + SSMS or sqlcmd:
--     1. Start Cloud SQL Auth Proxy:
--        cloud-sql-proxy YOUR_PROJECT:REGION:INSTANCE_NAME
--     2. Connect SSMS to 127.0.0.1,1433
--     3. Open this file and Execute (F5)
--
--   Option C — gcloud CLI:
--     gcloud sql connect INSTANCE_NAME --user=sqlserver
--     Then run: :r Taurus_schema.sql
-- =============================================================================

SET NOCOUNT ON;
GO

-- =============================================================================
-- TABLE: User
-- Covers: DB1 (User Database) from flowcharts
-- Notes:  Email/PasswordHash are NULL-able to support walk-in placeholder
--         records (IsWalkIn = 1) that have no credentials.
-- =============================================================================
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

CREATE UNIQUE INDEX UX_User_Email  ON [User](Email)       WHERE Email IS NOT NULL;
CREATE INDEX IX_User_PhoneNumber   ON [User](PhoneNumber);
CREATE INDEX IX_User_IsActive      ON [User](IsActive);
CREATE INDEX IX_User_IsWalkIn      ON [User](IsWalkIn);
GO

-- =============================================================================
-- TABLE: Role
-- =============================================================================
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

-- =============================================================================
-- TABLE: UserRole
-- =============================================================================
CREATE TABLE UserRole (
    UserRoleId  INT      NOT NULL IDENTITY(1,1),
    UserId      INT      NOT NULL,
    RoleId      INT      NOT NULL,
    AssignedAt  DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_UserRole           PRIMARY KEY (UserRoleId),
    CONSTRAINT FK_UserRole_User      FOREIGN KEY (UserId) REFERENCES [User](UserId) ON DELETE CASCADE,
    CONSTRAINT FK_UserRole_Role      FOREIGN KEY (RoleId) REFERENCES [Role](RoleId),
    CONSTRAINT UX_UserRole_UserRole  UNIQUE (UserId, RoleId)
);
GO

CREATE INDEX IX_UserRole_UserId ON UserRole(UserId);
CREATE INDEX IX_UserRole_RoleId ON UserRole(RoleId);
GO

-- =============================================================================
-- TABLE: OTPVerification  (NEW in v4.0)
-- Covers: Part 1 — 2-step registration OTP flow
-- Notes:  One OTP record is inserted per registration attempt.
--         IsUsed is flipped to 1 on successful verification.
--         Expired/used records can be purged by the background job.
-- =============================================================================
CREATE TABLE OTPVerification (
    OTPId       INT             NOT NULL IDENTITY(1,1),
    Email       NVARCHAR(255)   NOT NULL,
    OTPCode     NVARCHAR(10)    NOT NULL,
    IsUsed      BIT             NOT NULL DEFAULT 0,
    ExpiresAt   DATETIME        NOT NULL,
    CreatedAt   DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_OTPVerification PRIMARY KEY (OTPId)
);
GO

CREATE INDEX IX_OTP_Email      ON OTPVerification(Email);
CREATE INDEX IX_OTP_ExpiresAt  ON OTPVerification(ExpiresAt);
CREATE INDEX IX_OTP_IsUsed     ON OTPVerification(IsUsed);
GO

-- =============================================================================
-- TABLE: GuestSession  (NEW in v4.0)
-- Covers: Part 1 — "Continue as Guest" path
-- Notes:  A session token is generated client-side (UUID) and stored here.
--         Linked to Cart so guest cart items persist across page reloads.
--         ConvertedToUserId is populated if guest later registers.
-- =============================================================================
CREATE TABLE GuestSession (
    GuestSessionId  INT             NOT NULL IDENTITY(1,1),
    SessionToken    NVARCHAR(100)   NOT NULL,   -- UUID generated client-side
    Email           NVARCHAR(255)   NULL,        -- optionally collected at checkout
    PhoneNumber     NVARCHAR(20)    NULL,
    ConvertedToUserId INT           NULL,        -- set if guest registers after shopping
    ExpiresAt       DATETIME        NOT NULL,    -- e.g. GETDATE() + 7 days
    CreatedAt       DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_GuestSession              PRIMARY KEY (GuestSessionId),
    CONSTRAINT FK_GuestSession_User         FOREIGN KEY (ConvertedToUserId) REFERENCES [User](UserId),
    CONSTRAINT UX_GuestSession_Token        UNIQUE (SessionToken)
);
GO

CREATE INDEX IX_GuestSession_Token     ON GuestSession(SessionToken);
CREATE INDEX IX_GuestSession_ExpiresAt ON GuestSession(ExpiresAt);
GO

-- =============================================================================
-- TABLE: Category
-- CategoryCode matches product category headers:
--   UNIT, FRAME, FORK, HUB, UPGKIT, STEM,
--   HBAR, SADDLE, GRIP, PEDAL, RIM, TIRE, CHAIN
-- =============================================================================
CREATE TABLE Category (
    CategoryId       INT            NOT NULL IDENTITY(1,1),
    CategoryCode     VARCHAR(20)    NOT NULL,
    [Name]           NVARCHAR(100)  NOT NULL,
    [Description]    NVARCHAR(500)  NULL,
    ParentCategoryId INT            NULL,
    IsActive         BIT            NOT NULL DEFAULT 1,
    DisplayOrder     INT            NOT NULL DEFAULT 0,
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
    BrandId       INT            NOT NULL IDENTITY(1,1),
    BrandName     NVARCHAR(100)  NOT NULL,
    Country       NVARCHAR(100)  NULL,
    Website       NVARCHAR(255)  NULL,
    [Description] NVARCHAR(500)  NULL,
    IsActive      BIT            NOT NULL DEFAULT 1,
    CreatedAt     DATETIME       NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Brand       PRIMARY KEY (BrandId),
    CONSTRAINT UQ_Brand_Name  UNIQUE (BrandName)
);
GO

CREATE INDEX IX_Brand_IsActive ON Brand(IsActive);
GO

-- =============================================================================
-- TABLE: Product
-- Covers: DB2 (Product Database) from flowcharts
-- =============================================================================
CREATE TABLE Product (
    ProductId           INT             NOT NULL IDENTITY(1,1),
    CategoryId          INT             NOT NULL,
    BrandId             INT             NULL,
    SKU                 VARCHAR(50)     NULL,
    [Name]              NVARCHAR(200)   NOT NULL,
    ShortDescription    NVARCHAR(300)   NULL,
    [Description]       NVARCHAR(MAX)   NULL,
    -- Pricing
    Price               DECIMAL(10,2)   NOT NULL,
    Currency            CHAR(3)         NOT NULL DEFAULT 'PHP',
    -- Inventory (base stock; variants tracked separately in ProductVariant)
    StockQuantity       INT             NOT NULL DEFAULT 0,
    -- Bicycle-specific attributes
    Material            NVARCHAR(100)   NULL,
    Color               NVARCHAR(100)   NULL,
    WheelSize           NVARCHAR(20)    NULL,
    SpeedCompatibility  NVARCHAR(50)    NULL,
    BoostCompatible     BIT             NULL,
    TubelessReady       BIT             NULL,
    AxleStandard        NVARCHAR(50)    NULL,
    SuspensionTravel    NVARCHAR(50)    NULL,
    BrakeType           NVARCHAR(100)   NULL,
    AdditionalSpecs     NVARCHAR(1000)  NULL,
    -- Status flags
    IsActive            BIT             NOT NULL DEFAULT 1,
    IsFeatured          BIT             NOT NULL DEFAULT 0,
    CreatedAt           DATETIME        NOT NULL DEFAULT GETDATE(),
    UpdatedAt           DATETIME        NULL,
    CONSTRAINT PK_Product           PRIMARY KEY (ProductId),
    CONSTRAINT FK_Product_Category  FOREIGN KEY (CategoryId) REFERENCES Category(CategoryId),
    CONSTRAINT FK_Product_Brand     FOREIGN KEY (BrandId)    REFERENCES Brand(BrandId),
    CONSTRAINT CK_Product_Price     CHECK (Price >= 0),
    CONSTRAINT CK_Product_StockQty  CHECK (StockQuantity >= 0),
    CONSTRAINT CK_Product_Currency  CHECK (Currency IN ('PHP','USD','EUR'))
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
-- Auto-populated by trigger TR_Product_PriceAudit below.
-- =============================================================================
CREATE TABLE PriceHistory (
    PriceHistoryId  INT             NOT NULL IDENTITY(1,1),
    ProductId       INT             NOT NULL,
    OldPrice        DECIMAL(10,2)   NOT NULL,
    NewPrice        DECIMAL(10,2)   NOT NULL,
    ChangedAt       DATETIME        NOT NULL DEFAULT GETDATE(),
    ChangedByUserId INT             NULL,
    Notes           NVARCHAR(500)   NULL,
    CONSTRAINT PK_PriceHistory          PRIMARY KEY (PriceHistoryId),
    CONSTRAINT FK_PriceHistory_Product  FOREIGN KEY (ProductId)       REFERENCES Product(ProductId),
    CONSTRAINT FK_PriceHistory_User     FOREIGN KEY (ChangedByUserId) REFERENCES [User](UserId)
);
GO

CREATE INDEX IX_PriceHistory_ProductId ON PriceHistory(ProductId, ChangedAt DESC);
CREATE INDEX IX_PriceHistory_ChangedAt ON PriceHistory(ChangedAt);
GO

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

-- =============================================================================
-- TABLE: ProductVariant
-- =============================================================================
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
-- Covers: Image storage pattern for Google Cloud Storage (GCS)
-- DESIGN: Only metadata and the public/signed URL are stored here.
--         The actual image binary lives in a GCS bucket.
--         StorageBucket + StoragePath together form the GCS object path:
--           gs://{StorageBucket}/{StoragePath}
--         ImageUrl is the publicly accessible (or CDN) URL used by the app.
-- =============================================================================
CREATE TABLE ProductImage (
    ProductImageId  INT              NOT NULL IDENTITY(1,1),
    ProductId       INT              NOT NULL,
    -- GCS storage reference
    StorageBucket   NVARCHAR(200)    NOT NULL,   -- e.g. 'taurus-product-images'
    StoragePath     NVARCHAR(1000)   NOT NULL,   -- e.g. 'products/unit/bike-001-full.webp'
    -- Served URL (CDN or GCS public URL)
    ImageUrl        NVARCHAR(1000)   NOT NULL,   -- e.g. 'https://storage.googleapis.com/...'
    -- Display metadata
    ImageType       NVARCHAR(50)     NOT NULL,   -- 'Full' | 'Medium' | 'Thumbnail'
    IsPrimary       BIT              NOT NULL DEFAULT 0,
    DisplayOrder    INT              NOT NULL DEFAULT 0,
    AltText         NVARCHAR(200)    NULL,
    -- File metadata (optional but useful for cache control / admin UI)
    FileSizeBytes   INT              NULL,        -- original file size
    MimeType        NVARCHAR(50)     NULL,        -- e.g. 'image/webp', 'image/jpeg'
    Width           INT              NULL,        -- pixels
    Height          INT              NULL,        -- pixels
    UploadedByUserId INT             NULL,        -- admin who uploaded
    CreatedAt       DATETIME         NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_ProductImage              PRIMARY KEY (ProductImageId),
    CONSTRAINT FK_ProductImage_Product      FOREIGN KEY (ProductId)         REFERENCES Product(ProductId)  ON DELETE CASCADE,
    CONSTRAINT FK_ProductImage_UploadedBy   FOREIGN KEY (UploadedByUserId)  REFERENCES [User](UserId),
    CONSTRAINT CK_ProductImage_Type         CHECK (ImageType IN ('Full','Medium','Thumbnail')),
    CONSTRAINT CK_ProductImage_Order        CHECK (DisplayOrder >= 0)
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

CREATE INDEX IX_Supplier_IsActive     ON Supplier(IsActive);
CREATE UNIQUE INDEX UX_Supplier_Email ON Supplier(Email) WHERE Email IS NOT NULL;
GO

-- =============================================================================
-- TABLE: PurchaseOrder
-- Covers: Part 12 — Admin inventory restocking flow
-- =============================================================================
CREATE TABLE PurchaseOrder (
    PurchaseOrderId         INT             NOT NULL IDENTITY(1,1),
    SupplierId              INT             NOT NULL,
    OrderDate               DATETIME        NOT NULL DEFAULT GETDATE(),
    ExpectedDeliveryDate    DATETIME        NULL,
    [Status]                NVARCHAR(50)    NOT NULL DEFAULT 'Pending',
    TotalAmount             DECIMAL(10,2)   NOT NULL DEFAULT 0,
    CreatedByUserId         INT             NULL,
    CreatedAt               DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_PurchaseOrder          PRIMARY KEY (PurchaseOrderId),
    CONSTRAINT FK_PurchaseOrder_Supplier FOREIGN KEY (SupplierId)      REFERENCES Supplier(SupplierId),
    CONSTRAINT FK_PurchaseOrder_User     FOREIGN KEY (CreatedByUserId) REFERENCES [User](UserId),
    CONSTRAINT CK_PurchaseOrder_Status   CHECK ([Status] IN ('Pending','Received','Cancelled')),
    CONSTRAINT CK_PurchaseOrder_Total    CHECK (TotalAmount >= 0)
);
GO

CREATE INDEX IX_PurchaseOrder_SupplierId      ON PurchaseOrder(SupplierId);
CREATE INDEX IX_PurchaseOrder_Status          ON PurchaseOrder([Status]);
CREATE INDEX IX_PurchaseOrder_OrderDate       ON PurchaseOrder(OrderDate);
CREATE INDEX IX_PurchaseOrder_CreatedByUserId ON PurchaseOrder(CreatedByUserId);
GO

-- =============================================================================
-- TABLE: PurchaseOrderItem
-- =============================================================================
CREATE TABLE PurchaseOrderItem (
    PurchaseOrderItemId INT             NOT NULL IDENTITY(1,1),
    PurchaseOrderId     INT             NOT NULL,
    ProductId           INT             NOT NULL,
    ProductVariantId    INT             NULL,
    Quantity            INT             NOT NULL,
    UnitPrice           DECIMAL(10,2)   NOT NULL,
    Subtotal            DECIMAL(10,2)   NOT NULL,
    CONSTRAINT PK_PurchaseOrderItem          PRIMARY KEY (PurchaseOrderItemId),
    CONSTRAINT FK_POItem_PO                  FOREIGN KEY (PurchaseOrderId)  REFERENCES PurchaseOrder(PurchaseOrderId) ON DELETE CASCADE,
    CONSTRAINT FK_POItem_Product             FOREIGN KEY (ProductId)        REFERENCES Product(ProductId),
    CONSTRAINT FK_POItem_Variant             FOREIGN KEY (ProductVariantId) REFERENCES ProductVariant(ProductVariantId),
    CONSTRAINT CK_POItem_Quantity            CHECK (Quantity > 0),
    CONSTRAINT CK_POItem_UnitPrice           CHECK (UnitPrice >= 0),
    CONSTRAINT CK_POItem_Subtotal            CHECK (Subtotal >= 0)
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
    UserVoucherId   INT      NOT NULL IDENTITY(1,1),
    UserId          INT      NOT NULL,
    VoucherId       INT      NOT NULL,
    AssignedAt      DATETIME NOT NULL DEFAULT GETDATE(),
    ExpiresAt       DATETIME NULL,
    CONSTRAINT PK_UserVoucher              PRIMARY KEY (UserVoucherId),
    CONSTRAINT FK_UserVoucher_User         FOREIGN KEY (UserId)    REFERENCES [User](UserId),
    CONSTRAINT FK_UserVoucher_Voucher      FOREIGN KEY (VoucherId) REFERENCES Voucher(VoucherId),
    CONSTRAINT UX_UserVoucher_UserVoucher  UNIQUE (UserId, VoucherId)
);
GO

CREATE INDEX IX_UserVoucher_UserId    ON UserVoucher(UserId);
CREATE INDEX IX_UserVoucher_VoucherId ON UserVoucher(VoucherId);
GO

-- =============================================================================
-- TABLE: Cart
-- Covers: DB5 (Cart Database) from flowcharts
-- v4.0: UserId is now NULLABLE to support guest carts.
--       Either UserId OR GuestSessionId must be populated (enforced by CHECK).
--       A registered user has one active cart (UX_Cart_User_Active).
--       A guest session has one active cart (UX_Cart_Guest_Active).
-- =============================================================================
CREATE TABLE Cart (
    CartId          INT      NOT NULL IDENTITY(1,1),
    UserId          INT      NULL,               -- NULL for guest carts
    GuestSessionId  INT      NULL,               -- NULL for registered user carts
    CreatedAt       DATETIME NOT NULL DEFAULT GETDATE(),
    LastUpdatedAt   DATETIME NULL,
    IsCheckedOut    BIT      NOT NULL DEFAULT 0,
    CONSTRAINT PK_Cart              PRIMARY KEY (CartId),
    CONSTRAINT FK_Cart_User         FOREIGN KEY (UserId)         REFERENCES [User](UserId),
    CONSTRAINT FK_Cart_GuestSession FOREIGN KEY (GuestSessionId) REFERENCES GuestSession(GuestSessionId),
    -- Exactly one owner: either a registered user or a guest session
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
    CartItemId          INT             NOT NULL IDENTITY(1,1),
    CartId              INT             NOT NULL,
    ProductId           INT             NOT NULL,
    ProductVariantId    INT             NULL,
    Quantity            INT             NOT NULL,
    PriceAtAdd          DECIMAL(10,2)   NOT NULL,
    AddedAt             DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_CartItem            PRIMARY KEY (CartItemId),
    CONSTRAINT FK_CartItem_Cart       FOREIGN KEY (CartId)            REFERENCES Cart(CartId)                        ON DELETE CASCADE,
    CONSTRAINT FK_CartItem_Product    FOREIGN KEY (ProductId)         REFERENCES Product(ProductId),
    CONSTRAINT FK_CartItem_Variant    FOREIGN KEY (ProductVariantId)  REFERENCES ProductVariant(ProductVariantId),
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
-- Covers: DB5 (Cart Database) — wishlist portion
-- Notes:  Part 2 saves out-of-stock items here; Part 12 triggers notifications
--         back to these users when stock is restored.
-- =============================================================================
CREATE TABLE Wishlist (
    WishlistId  INT      NOT NULL IDENTITY(1,1),
    UserId      INT      NOT NULL,
    ProductId   INT      NOT NULL,
    AddedAt     DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Wishlist              PRIMARY KEY (WishlistId),
    CONSTRAINT FK_Wishlist_User         FOREIGN KEY (UserId)    REFERENCES [User](UserId)    ON DELETE CASCADE,
    CONSTRAINT FK_Wishlist_Product      FOREIGN KEY (ProductId) REFERENCES Product(ProductId),
    CONSTRAINT UX_Wishlist_UserProduct  UNIQUE (UserId, ProductId)
);
GO

CREATE INDEX IX_Wishlist_UserId    ON Wishlist(UserId);
CREATE INDEX IX_Wishlist_ProductId ON Wishlist(ProductId);
GO

-- =============================================================================
-- TABLE: Order
-- Covers: DB3 (Order Database) from flowcharts
-- v4.0 changes:
--   OrderStatus — Added: 'ReadyForPickup', 'PickedUp',
--                        'PendingVerification', 'OnHold'
--     Full lifecycle:
--       Online delivery:   Pending → PendingVerification (bank transfer)
--                                  → Processing → Shipped → Delivered
--                          or:      Pending → OnHold (timeout) → Cancelled
--       Pickup order:      Pending → Processing → ReadyForPickup → PickedUp
--                          or:      ReadyForPickup → OnHold → Cancelled
--       Walk-in POS:       Processing → Delivered  (instant)
--   Pickup expiry columns — Part 8 enforces 3-day pickup window:
--       PickupReadyAt      — timestamp when status set to ReadyForPickup
--       PickupExpiresAt    — PickupReadyAt + 3 days (app layer sets this)
--       PickupConfirmedAt  — timestamp when customer actually collects
--
-- CRITICAL: Must be created BEFORE InventoryLog (FK dependency)
-- =============================================================================
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
    -- Pickup-specific columns (NULL for delivery orders)
    PickupReadyAt           DATETIME        NULL,   -- when marked ReadyForPickup
    PickupExpiresAt         DATETIME        NULL,   -- 3-day expiry deadline
    PickupConfirmedAt       DATETIME        NULL,   -- when customer collected
    CreatedAt               DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Order        PRIMARY KEY (OrderId),
    CONSTRAINT FK_Order_User   FOREIGN KEY (UserId) REFERENCES [User](UserId),
    CONSTRAINT CK_Order_Status CHECK (OrderStatus IN (
        'Pending',
        'PendingVerification',  -- bank transfer awaiting admin approval
        'OnHold',               -- payment timeout or pickup expired
        'Processing',           -- payment verified, preparing order
        'ReadyForPickup',       -- store pickup: item ready at counter
        'PickedUp',             -- store pickup: customer collected
        'Shipped',              -- delivery: out for delivery
        'Delivered',            -- delivery: confirmed received
        'Cancelled'
    )),
    CONSTRAINT CK_Order_SubTotal   CHECK (SubTotal >= 0),
    CONSTRAINT CK_Order_Discount   CHECK (DiscountAmount >= 0),
    CONSTRAINT CK_Order_Shipping   CHECK (ShippingFee >= 0),
    CONSTRAINT CK_Order_Total      CHECK (TotalAmount >= 0)
);
GO

CREATE INDEX IX_Order_UserId      ON [Order](UserId);
CREATE INDEX IX_Order_OrderNumber ON [Order](OrderNumber);
CREATE INDEX IX_Order_OrderStatus ON [Order](OrderStatus);
CREATE INDEX IX_Order_OrderDate   ON [Order](OrderDate);
CREATE INDEX IX_Order_IsWalkIn    ON [Order](IsWalkIn);
-- Index for background job: pickup expiry monitor
CREATE INDEX IX_Order_PickupExpiresAt ON [Order](PickupExpiresAt) WHERE PickupExpiresAt IS NOT NULL;
GO

-- =============================================================================
-- TABLE: OrderItem
-- CRITICAL: Must be created BEFORE InventoryLog
-- =============================================================================
CREATE TABLE OrderItem (
    OrderItemId         INT             NOT NULL IDENTITY(1,1),
    OrderId             INT             NOT NULL,
    ProductId           INT             NOT NULL,
    ProductVariantId    INT             NULL,
    Quantity            INT             NOT NULL,
    UnitPrice           DECIMAL(10,2)   NOT NULL,
    Subtotal            DECIMAL(10,2)   NOT NULL,
    CONSTRAINT PK_OrderItem           PRIMARY KEY (OrderItemId),
    CONSTRAINT FK_OrderItem_Order     FOREIGN KEY (OrderId)           REFERENCES [Order](OrderId)                   ON DELETE CASCADE,
    CONSTRAINT FK_OrderItem_Product   FOREIGN KEY (ProductId)         REFERENCES Product(ProductId),
    CONSTRAINT FK_OrderItem_Variant   FOREIGN KEY (ProductVariantId)  REFERENCES ProductVariant(ProductVariantId),
    CONSTRAINT CK_OrderItem_Quantity  CHECK (Quantity > 0),
    CONSTRAINT CK_OrderItem_UnitPrice CHECK (UnitPrice >= 0),
    CONSTRAINT CK_OrderItem_Subtotal  CHECK (Subtotal >= 0)
);
GO

CREATE INDEX IX_OrderItem_OrderId           ON OrderItem(OrderId);
CREATE INDEX IX_OrderItem_ProductId         ON OrderItem(ProductId);
CREATE INDEX IX_OrderItem_ProductVariantId  ON OrderItem(ProductVariantId);
GO

-- =============================================================================
-- TABLE: VoucherUsage
-- CRITICAL: Must be created BEFORE InventoryLog
-- =============================================================================
CREATE TABLE VoucherUsage (
    VoucherUsageId  INT             NOT NULL IDENTITY(1,1),
    VoucherId       INT             NOT NULL,
    UserId          INT             NOT NULL,
    OrderId         INT             NOT NULL,
    DiscountAmount  DECIMAL(10,2)   NOT NULL,
    UsedAt          DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_VoucherUsage          PRIMARY KEY (VoucherUsageId),
    CONSTRAINT FK_VoucherUsage_Voucher  FOREIGN KEY (VoucherId) REFERENCES Voucher(VoucherId),
    CONSTRAINT FK_VoucherUsage_User     FOREIGN KEY (UserId)    REFERENCES [User](UserId),
    CONSTRAINT FK_VoucherUsage_Order    FOREIGN KEY (OrderId)   REFERENCES [Order](OrderId),
    CONSTRAINT CK_VoucherUsage_Discount CHECK (DiscountAmount >= 0)
);
GO

CREATE INDEX IX_VoucherUsage_VoucherId ON VoucherUsage(VoucherId);
CREATE INDEX IX_VoucherUsage_UserId    ON VoucherUsage(UserId);
CREATE INDEX IX_VoucherUsage_OrderId   ON VoucherUsage(OrderId);
GO

-- =============================================================================
-- TABLE: InventoryLog
-- Covers: DB4 (Inventory Database) — movement history shown in Part 12
-- v4.0: Added 'Lock' and 'Unlock' to ChangeType
--   Lock   — stock reserved when an order is placed (Part 4: LockInventoryStock)
--   Unlock — reserved stock released on delivery/pickup confirmed (Part 6)
--            or on order cancellation
-- =============================================================================
CREATE TABLE InventoryLog (
    InventoryLogId      INT             NOT NULL IDENTITY(1,1),
    ProductId           INT             NOT NULL,
    ProductVariantId    INT             NULL,
    ChangeQuantity      INT             NOT NULL,   -- positive = added, negative = removed
    ChangeType          NVARCHAR(50)    NOT NULL,
    OrderId             INT             NULL,
    PurchaseOrderId     INT             NULL,
    ChangedByUserId     INT             NULL,
    Notes               NVARCHAR(500)   NULL,
    CreatedAt           DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_InventoryLog          PRIMARY KEY (InventoryLogId),
    CONSTRAINT FK_InvLog_Product        FOREIGN KEY (ProductId)        REFERENCES Product(ProductId),
    CONSTRAINT FK_InvLog_Variant        FOREIGN KEY (ProductVariantId) REFERENCES ProductVariant(ProductVariantId),
    CONSTRAINT FK_InvLog_Order          FOREIGN KEY (OrderId)          REFERENCES [Order](OrderId),
    CONSTRAINT FK_InvLog_PurchaseOrder  FOREIGN KEY (PurchaseOrderId)  REFERENCES PurchaseOrder(PurchaseOrderId),
    CONSTRAINT FK_InvLog_User           FOREIGN KEY (ChangedByUserId)  REFERENCES [User](UserId),
    CONSTRAINT CK_InvLog_ChangeType     CHECK (ChangeType IN (
        'Purchase',     -- stock received from supplier
        'Sale',         -- stock deducted on confirmed order
        'Return',       -- stock returned by customer
        'Adjustment',   -- manual admin correction
        'Damage',       -- damaged stock written off
        'Loss',         -- lost/missing stock
        'Lock',         -- stock reserved pending payment confirmation
        'Unlock'        -- reserved stock released (cancelled or delivered)
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
-- Covers: DB3 (Order Database) — payment records portion
-- v4.0 changes:
--   PaymentMethod — 'GCash' | 'BankTransfer' | 'Cash'
--     GCash        : online orders (EXT1 — GCash gateway)
--     BankTransfer : online orders (EXT5 — bank verification + admin approval)
--     Cash         : walk-in POS ONLY (IsWalkIn = 1 on the linked Order)
--     COD          : NOT offered by Taurus Bike Shop — excluded entirely
--
--   PaymentStatus — Added 'VerificationPending' | 'VerificationRejected'
--     These map directly to the admin verification workflow in Part 9.
--
--   New proof/verification columns (Part 4 & Part 9):
--     PaymentProofUrl      — GCS URL of the uploaded bank transfer screenshot
--     ProofStorageBucket   — GCS bucket name for the proof file
--     ProofStoragePath     — GCS object path for the proof file
--     VerifiedByUserId     — admin UserId who approved or rejected
--     VerificationNotes    — admin's reason (especially for rejections)
--     VerifiedAt           — timestamp of the approval/rejection action
--     VerificationDeadline — 24-hour auto-hold deadline (set on upload)
-- =============================================================================
CREATE TABLE Payment (
    PaymentId               INT             NOT NULL IDENTITY(1,1),
    OrderId                 INT             NOT NULL,
    PaymentMethod           NVARCHAR(50)    NOT NULL,
    PaymentStage            NVARCHAR(20)    NOT NULL DEFAULT 'Upfront',
    PaymentStatus           NVARCHAR(50)    NOT NULL,
    Amount                  DECIMAL(10,2)   NOT NULL,
    -- GCash-specific
    GcashTransactionId      NVARCHAR(255)   NULL,
    -- Bank Transfer-specific
    BpiReferenceNumber      NVARCHAR(255)   NULL,
    -- Proof of payment (bank transfer screenshot stored in GCS)
    PaymentProofUrl         NVARCHAR(1000)  NULL,   -- served URL (CDN/GCS public)
    ProofStorageBucket      NVARCHAR(200)   NULL,   -- e.g. 'taurus-payment-proofs'
    ProofStoragePath        NVARCHAR(1000)  NULL,   -- e.g. 'proofs/2025/order-1234.jpg'
    -- Admin verification fields (Part 9)
    VerifiedByUserId        INT             NULL,
    VerificationNotes       NVARCHAR(500)   NULL,
    VerifiedAt              DATETIME        NULL,
    VerificationDeadline    DATETIME        NULL,   -- 24-hr auto-hold cut-off
    -- Timestamps
    PaymentDate             DATETIME        NULL,
    CreatedAt               DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Payment               PRIMARY KEY (PaymentId),
    CONSTRAINT FK_Payment_Order         FOREIGN KEY (OrderId)          REFERENCES [Order](OrderId),
    CONSTRAINT FK_Payment_VerifiedBy    FOREIGN KEY (VerifiedByUserId) REFERENCES [User](UserId),
    CONSTRAINT CK_Payment_Method        CHECK (PaymentMethod IN ('GCash','BankTransfer','Cash')),
    CONSTRAINT CK_Payment_Stage         CHECK (PaymentStage  IN ('Upfront','Confirmation')),
    CONSTRAINT CK_Payment_Status        CHECK (PaymentStatus IN (
        'Pending',              -- initial state
        'VerificationPending',  -- bank transfer uploaded, awaiting admin review
        'VerificationRejected', -- admin rejected the proof
        'Completed',            -- payment confirmed
        'Failed',               -- GCash/card payment failed
        'Refunded'              -- refund processed
    )),
    CONSTRAINT CK_Payment_Amount        CHECK (Amount >= 0),
    -- GCash must have a transaction ID once completed
    CONSTRAINT CK_Payment_GcashRef      CHECK (
        PaymentMethod <> 'GCash'        OR GcashTransactionId  IS NOT NULL OR PaymentStatus IN ('Pending','Failed')
    ),
    -- BankTransfer must have a BPI reference number once past Pending
    CONSTRAINT CK_Payment_BpiRef        CHECK (
        PaymentMethod <> 'BankTransfer' OR BpiReferenceNumber   IS NOT NULL OR PaymentStatus = 'Pending'
    )
);
GO

CREATE INDEX IX_Payment_OrderId               ON Payment(OrderId);
CREATE INDEX IX_Payment_PaymentMethod         ON Payment(PaymentMethod);
CREATE INDEX IX_Payment_PaymentStage          ON Payment(PaymentStage);
CREATE INDEX IX_Payment_PaymentStatus         ON Payment(PaymentStatus);
CREATE INDEX IX_Payment_GcashTransactionId    ON Payment(GcashTransactionId) WHERE GcashTransactionId IS NOT NULL;
CREATE INDEX IX_Payment_BpiReferenceNumber    ON Payment(BpiReferenceNumber)  WHERE BpiReferenceNumber  IS NOT NULL;
-- Index for background job: payment timeout monitor (Part 14, Job 3)
CREATE INDEX IX_Payment_VerificationDeadline  ON Payment(VerificationDeadline) WHERE VerificationDeadline IS NOT NULL;
GO

-- =============================================================================
-- TABLE: Delivery
-- Covers: DB3 — delivery records; EXT2 (Lalamove), EXT3 (LBC)
-- v4.0 changes:
--   DeliveryStatus — Added 'Failed' for delivery failure path (Parts 5, 8)
--   IsDelayed      — BIT flag set when admin marks delivery as delayed (Part 10)
--   DelayedUntil   — Updated ETA after delay notification
-- =============================================================================
CREATE TABLE Delivery (
    DeliveryId              INT             NOT NULL IDENTITY(1,1),
    OrderId                 INT             NOT NULL,
    Courier                 NVARCHAR(20)    NOT NULL,
    LalamoveBookingRef      NVARCHAR(255)   NULL,
    LbcTrackingNumber       NVARCHAR(255)   NULL,
    DeliveryStatus          NVARCHAR(50)    NOT NULL DEFAULT 'Pending',
    IsDelayed               BIT             NOT NULL DEFAULT 0,  -- Part 10: admin marks delayed
    DelayedUntil            DATETIME        NULL,                -- updated ETA when delayed
    DriverName              NVARCHAR(100)   NULL,
    DriverPhone             NVARCHAR(20)    NULL,
    EstimatedDeliveryTime   DATETIME        NULL,
    ActualDeliveryTime      DATETIME        NULL,
    CreatedAt               DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Delivery             PRIMARY KEY (DeliveryId),
    CONSTRAINT FK_Delivery_Order       FOREIGN KEY (OrderId) REFERENCES [Order](OrderId),
    CONSTRAINT CK_Delivery_Courier     CHECK (Courier        IN ('Lalamove','LBC')),
    CONSTRAINT CK_Delivery_Status      CHECK (DeliveryStatus IN (
        'Pending',      -- accepted, not yet picked up by courier
        'PickedUp',     -- courier collected the package
        'InTransit',    -- on the way to customer
        'Delivered',    -- customer confirmed receipt
        'Failed'        -- delivery attempt failed (Part 5 & 8 failure path)
    )),
    CONSTRAINT CK_Delivery_LalamoveRef CHECK (
        Courier <> 'Lalamove' OR LalamoveBookingRef IS NOT NULL OR DeliveryStatus = 'Pending'
    ),
    CONSTRAINT CK_Delivery_LbcTracking CHECK (
        Courier <> 'LBC' OR LbcTrackingNumber IS NOT NULL OR DeliveryStatus = 'Pending'
    ),
    CONSTRAINT CK_Delivery_LbcNoDriver CHECK (
        Courier <> 'LBC' OR (DriverName IS NULL AND DriverPhone IS NULL)
    )
);
GO

CREATE INDEX IX_Delivery_OrderId           ON Delivery(OrderId);
CREATE INDEX IX_Delivery_Courier           ON Delivery(Courier);
CREATE INDEX IX_Delivery_DeliveryStatus    ON Delivery(DeliveryStatus);
CREATE INDEX IX_Delivery_IsDelayed         ON Delivery(IsDelayed) WHERE IsDelayed = 1;
CREATE INDEX IX_Delivery_LalamoveBookingRef ON Delivery(LalamoveBookingRef) WHERE LalamoveBookingRef IS NOT NULL;
CREATE INDEX IX_Delivery_LbcTrackingNumber  ON Delivery(LbcTrackingNumber)  WHERE LbcTrackingNumber  IS NOT NULL;
GO

-- =============================================================================
-- TABLE: Review
-- Covers: Part 6 — post-delivery review & rating
-- =============================================================================
CREATE TABLE Review (
    ReviewId            INT             NOT NULL IDENTITY(1,1),
    UserId              INT             NOT NULL,
    ProductId           INT             NOT NULL,
    OrderId             INT             NOT NULL,
    Rating              INT             NOT NULL,
    Comment             NVARCHAR(1000)  NULL,
    IsVerifiedPurchase  BIT             NOT NULL DEFAULT 0,
    CreatedAt           DATETIME        NOT NULL DEFAULT GETDATE(),
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
-- Covers: Part 11 — Walk-in POS terminal session tracking
-- =============================================================================
CREATE TABLE POS_Session (
    POSSessionId    INT             NOT NULL IDENTITY(1,1),
    UserId          INT             NOT NULL,   -- cashier/staff running the session
    TerminalName    NVARCHAR(50)    NOT NULL,
    ShiftStart      DATETIME        NOT NULL,
    ShiftEnd        DATETIME        NULL,
    TotalSales      DECIMAL(10,2)   NOT NULL DEFAULT 0,
    CreatedAt       DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_POS_Session      PRIMARY KEY (POSSessionId),
    CONSTRAINT FK_POS_Session_User FOREIGN KEY (UserId) REFERENCES [User](UserId)
);
GO

CREATE INDEX IX_POS_Session_UserId     ON POS_Session(UserId);
CREATE INDEX IX_POS_Session_ShiftStart ON POS_Session(ShiftStart);
CREATE INDEX IX_POS_Session_ShiftEnd   ON POS_Session(ShiftEnd);
GO

-- =============================================================================
-- TABLE: SupportTicket  (NEW in v4.0)
-- Covers: DB6 — Part 13 (entire support module), Part 6 (damage report),
--               Part 2 (contact support), Part 5 (delivery issue)
-- Notes:  TicketSource distinguishes who created the ticket.
--         OrderId is nullable — some tickets may not relate to a specific order.
--         AssignedToUserId is the admin/staff handling the ticket.
-- =============================================================================
CREATE TABLE SupportTicket (
    TicketId            INT             NOT NULL IDENTITY(1,1),
    UserId              INT             NOT NULL,   -- customer who raised the ticket
    OrderId             INT             NULL,       -- related order (if any)
    TicketSource        NVARCHAR(50)    NOT NULL DEFAULT 'Customer',
    TicketCategory      NVARCHAR(100)   NOT NULL,
    Subject             NVARCHAR(200)   NOT NULL,
    [Description]       NVARCHAR(MAX)   NULL,
    -- Attachment stored in GCS (same pattern as ProductImage / PaymentProof)
    AttachmentUrl       NVARCHAR(1000)  NULL,
    AttachmentBucket    NVARCHAR(200)   NULL,       -- e.g. 'taurus-support-attachments'
    AttachmentPath      NVARCHAR(1000)  NULL,
    TicketStatus        NVARCHAR(50)    NOT NULL DEFAULT 'Open',
    AssignedToUserId    INT             NULL,       -- admin/staff assigned
    ResolvedAt          DATETIME        NULL,
    CreatedAt           DATETIME        NOT NULL DEFAULT GETDATE(),
    UpdatedAt           DATETIME        NULL,
    CONSTRAINT PK_SupportTicket             PRIMARY KEY (TicketId),
    CONSTRAINT FK_Ticket_User               FOREIGN KEY (UserId)            REFERENCES [User](UserId),
    CONSTRAINT FK_Ticket_Order              FOREIGN KEY (OrderId)           REFERENCES [Order](OrderId),
    CONSTRAINT FK_Ticket_AssignedTo         FOREIGN KEY (AssignedToUserId)  REFERENCES [User](UserId),
    CONSTRAINT CK_Ticket_Source             CHECK (TicketSource IN (
        'Customer',     -- raised by customer via app
        'Admin',        -- raised internally by admin
        'System'        -- auto-created by background job
    )),
    CONSTRAINT CK_Ticket_Category          CHECK (TicketCategory IN (
        'DamagedItem',
        'WrongItem',
        'DeliveryIssue',
        'PaymentIssue',
        'ReturnRefund',
        'ProductInquiry',
        'General'
    )),
    CONSTRAINT CK_Ticket_Status            CHECK (TicketStatus IN (
        'Open',
        'InProgress',
        'AwaitingResponse',  -- admin replied, waiting for customer
        'Resolved',
        'Closed'
    ))
);
GO

CREATE INDEX IX_Ticket_UserId         ON SupportTicket(UserId);
CREATE INDEX IX_Ticket_OrderId        ON SupportTicket(OrderId) WHERE OrderId IS NOT NULL;
CREATE INDEX IX_Ticket_Status         ON SupportTicket(TicketStatus);
CREATE INDEX IX_Ticket_Category       ON SupportTicket(TicketCategory);
CREATE INDEX IX_Ticket_AssignedTo     ON SupportTicket(AssignedToUserId) WHERE AssignedToUserId IS NOT NULL;
CREATE INDEX IX_Ticket_CreatedAt      ON SupportTicket(CreatedAt);
GO

-- =============================================================================
-- TABLE: SupportTicketReply  (NEW in v4.0)
-- Covers: Part 13 — reply thread per ticket (customer & admin messages)
-- Notes:  IsAdminReply = 1 means the message was sent by staff.
--         IsAdminReply = 0 means the customer replied.
-- =============================================================================
CREATE TABLE SupportTicketReply (
    ReplyId         INT             NOT NULL IDENTITY(1,1),
    TicketId        INT             NOT NULL,
    UserId          INT             NOT NULL,   -- who wrote this reply
    IsAdminReply    BIT             NOT NULL DEFAULT 0,
    Message         NVARCHAR(MAX)   NOT NULL,
    -- Optional attachment (same GCS pattern)
    AttachmentUrl   NVARCHAR(1000)  NULL,
    AttachmentBucket NVARCHAR(200)  NULL,
    AttachmentPath  NVARCHAR(1000)  NULL,
    CreatedAt       DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_SupportTicketReply        PRIMARY KEY (ReplyId),
    CONSTRAINT FK_Reply_Ticket              FOREIGN KEY (TicketId) REFERENCES SupportTicket(TicketId) ON DELETE CASCADE,
    CONSTRAINT FK_Reply_User                FOREIGN KEY (UserId)   REFERENCES [User](UserId)
);
GO

CREATE INDEX IX_Reply_TicketId  ON SupportTicketReply(TicketId);
CREATE INDEX IX_Reply_UserId    ON SupportTicketReply(UserId);
CREATE INDEX IX_Reply_CreatedAt ON SupportTicketReply(CreatedAt);
GO

-- =============================================================================
-- TABLE: Notification  (NEW in v4.0)
-- Covers: DB6 (Notification Queue) from flowcharts
--         Referenced in 11 of 14 flowchart modules.
-- Notes:  Every outbound email/SMS is queued here before dispatch via EXT4.
--         The background job (Part 14) processes Pending records and updates
--         Status to Sent or Failed.
--         OrderId and TicketId are optional contextual links.
-- =============================================================================
CREATE TABLE Notification (
    NotificationId      INT             NOT NULL IDENTITY(1,1),
    UserId              INT             NOT NULL,
    OrderId             INT             NULL,
    TicketId            INT             NULL,
    Channel             NVARCHAR(20)    NOT NULL,   -- 'Email' or 'SMS'
    NotifType           NVARCHAR(100)   NOT NULL,
    Recipient           NVARCHAR(255)   NOT NULL,   -- email address or phone number
    Subject             NVARCHAR(255)   NULL,       -- email subject (NULL for SMS)
    Body                NVARCHAR(MAX)   NULL,
    [Status]            NVARCHAR(20)    NOT NULL DEFAULT 'Pending',
    RetryCount          INT             NOT NULL DEFAULT 0,
    SentAt              DATETIME        NULL,
    FailureReason       NVARCHAR(500)   NULL,
    CreatedAt           DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Notification          PRIMARY KEY (NotificationId),
    CONSTRAINT FK_Notif_User            FOREIGN KEY (UserId)    REFERENCES [User](UserId),
    CONSTRAINT FK_Notif_Order           FOREIGN KEY (OrderId)   REFERENCES [Order](OrderId),
    CONSTRAINT FK_Notif_Ticket          FOREIGN KEY (TicketId)  REFERENCES SupportTicket(TicketId),
    CONSTRAINT CK_Notif_Channel         CHECK (Channel IN ('Email','SMS')),
    CONSTRAINT CK_Notif_Type            CHECK (NotifType IN (
        'OTPCode',              -- Part 1: registration OTP
        'WelcomeEmail',         -- Part 1: account created
        'OrderConfirmation',    -- Part 5: order placed
        'PaymentReceived',      -- Part 4/9: payment acknowledged
        'PaymentRejected',      -- Part 9: bank transfer proof rejected
        'PaymentHeld',          -- Part 4/9: 24-hr auto-hold triggered
        'TrackingUpdate',       -- Part 5: tracking link sent
        'ReadyForPickup',       -- Part 8: pickup order ready at counter
        'PickupExpiry',         -- Part 8: pickup expiry warning
        'DeliveryDelay',        -- Part 10: delivery marked delayed
        'DeliveryConfirmation', -- Part 6: order delivered
        'WishlistRestock',      -- Part 12: wishlist item back in stock
        'SupportTicketCreated', -- Part 13: ticket opened
        'SupportTicketReply',   -- Part 13: admin replied
        'SupportTicketResolved',-- Part 13: ticket closed
        'LowStockAlert',        -- Part 14: admin low stock alert
        'PendingOrderAlert'     -- Part 14: admin unprocessed order alert
    )),
    CONSTRAINT CK_Notif_Status CHECK ([Status] IN ('Pending','Sent','Failed'))
);
GO

CREATE INDEX IX_Notif_UserId    ON Notification(UserId);
CREATE INDEX IX_Notif_OrderId   ON Notification(OrderId)   WHERE OrderId  IS NOT NULL;
CREATE INDEX IX_Notif_TicketId  ON Notification(TicketId)  WHERE TicketId IS NOT NULL;
CREATE INDEX IX_Notif_Status    ON Notification([Status]);
CREATE INDEX IX_Notif_Channel   ON Notification(Channel);
CREATE INDEX IX_Notif_CreatedAt ON Notification(CreatedAt);
-- Index for background job polling: find unsent notifications quickly
CREATE INDEX IX_Notif_Pending   ON Notification([Status], CreatedAt) WHERE [Status] = 'Pending';
GO

-- =============================================================================
-- TABLE: SystemLog
-- Covers: Background job audit trail (Part 14) and admin action logging
-- v4.0: Expanded EventType to cover all background job events from Part 14
-- =============================================================================
CREATE TABLE SystemLog (
    SystemLogId         INT             NOT NULL IDENTITY(1,1),
    UserId              INT             NULL,   -- NULL for automated/system events
    EventType           NVARCHAR(100)   NOT NULL,
    EventDescription    NVARCHAR(1000)  NULL,
    CreatedAt           DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_SystemLog      PRIMARY KEY (SystemLogId),
    CONSTRAINT FK_SystemLog_User FOREIGN KEY (UserId) REFERENCES [User](UserId),
    CONSTRAINT CK_SystemLog_Event CHECK (EventType IN (
        -- Auth events
        'Login',
        'Logout',
        'AccessDenied',
        'UserCreated',
        -- Product/catalog events
        'ProductUpdate',
        'VoucherCreated',
        -- Order/payment events
        'OrderStatusChange',
        'PaymentProcessed',
        'PaymentVerified',
        'PaymentRejected',
        'PaymentTimeout',       -- Part 14, Job 3: auto-hold triggered
        -- Inventory events
        'InventoryAdjustment',
        'InventorySync',        -- Part 14, Job 1: POS/online sync
        'LowStockTriggered',    -- Part 14, Job 4: low stock detected
        -- Delivery events
        'DeliveryStatusPoll',   -- Part 14, Job 5: courier API polled
        'DeliveryDelayed',
        'DeliveryFailed',
        -- Background job lifecycle
        'BackgroundJobStart',
        'BackgroundJobComplete',
        'BackgroundJobError',
        -- Support events
        'SupportTicketCreated',
        'SupportTicketResolved'
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

-- View: Voucher usage count per user
CREATE VIEW vw_UserVoucherUsageCount AS
SELECT UserId, VoucherId, COUNT(*) AS TimesUsed
FROM VoucherUsage
GROUP BY UserId, VoucherId;
GO

-- View: Active products with brand, category, and primary image
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
    pi.StorageBucket    AS PrimaryImageBucket,
    pi.StoragePath      AS PrimaryImagePath,
    CASE WHEN EXISTS (
        SELECT 1 FROM ProductVariant pv
        WHERE pv.ProductId = p.ProductId AND pv.IsActive = 1
    ) THEN 1 ELSE 0 END AS HasVariants
FROM Product p
INNER JOIN Category c      ON p.CategoryId  = c.CategoryId
LEFT  JOIN Brand b         ON p.BrandId     = b.BrandId
LEFT  JOIN ProductImage pi ON p.ProductId   = pi.ProductId AND pi.IsPrimary = 1
WHERE p.IsActive = 1 AND c.IsActive = 1;
GO

-- View: Full product image gallery with GCS storage paths
CREATE VIEW vw_ProductImageGallery AS
SELECT
    pi.ProductImageId,
    pi.ProductId,
    p.[Name]        AS ProductName,
    pi.StorageBucket,
    pi.StoragePath,
    pi.ImageUrl,
    pi.ImageType,
    pi.IsPrimary,
    pi.DisplayOrder,
    pi.AltText,
    pi.FileSizeBytes,
    pi.MimeType,
    pi.Width,
    pi.Height,
    pi.CreatedAt
FROM ProductImage pi
INNER JOIN Product p ON pi.ProductId = p.ProductId
WHERE p.IsActive = 1;
GO

-- View: Order summary with customer info and item counts
CREATE VIEW vw_OrderSummary AS
SELECT
    o.OrderId,
    o.OrderNumber,
    o.OrderDate,
    o.OrderStatus,
    o.TotalAmount,
    o.IsWalkIn,
    o.PickupReadyAt,
    o.PickupExpiresAt,
    o.PickupConfirmedAt,
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
    o.TotalAmount, o.IsWalkIn,
    o.PickupReadyAt, o.PickupExpiresAt, o.PickupConfirmedAt,
    u.FirstName, u.LastName, u.Email, u.PhoneNumber;
GO

-- View: Product variant details with computed total price
CREATE VIEW vw_ProductVariantDetails AS
SELECT
    pv.ProductVariantId,
    pv.ProductId,
    p.[Name]                        AS ProductName,
    pv.VariantName,
    pv.SKU,
    p.Price                         AS BasePrice,
    pv.AdditionalPrice,
    (p.Price + pv.AdditionalPrice)  AS TotalPrice,
    pv.StockQuantity,
    pv.IsActive
FROM ProductVariant pv
INNER JOIN Product p ON pv.ProductId = p.ProductId
WHERE pv.IsActive = 1 AND p.IsActive = 1;
GO

-- View: Inventory status — available vs variant stock
CREATE VIEW vw_InventoryStatus AS
SELECT
    p.ProductId,
    p.[Name]                        AS ProductName,
    p.StockQuantity                 AS BaseStockQuantity,
    ISNULL(SUM(pv.StockQuantity), 0) AS VariantTotalStock,
    CASE
        WHEN EXISTS (
            SELECT 1 FROM ProductVariant
            WHERE ProductId = p.ProductId AND IsActive = 1
        )
        THEN ISNULL(SUM(pv.StockQuantity), 0)
        ELSE p.StockQuantity
    END                             AS AvailableStock,
    c.[Name]    AS CategoryName,
    b.BrandName
FROM Product p
INNER JOIN Category c ON p.CategoryId = c.CategoryId
LEFT  JOIN Brand b    ON p.BrandId    = b.BrandId
LEFT  JOIN ProductVariant pv ON p.ProductId = pv.ProductId AND pv.IsActive = 1
WHERE p.IsActive = 1
GROUP BY p.ProductId, p.[Name], p.StockQuantity, c.[Name], b.BrandName;
GO

-- View: Pending notifications for background job processing (Part 14)
CREATE VIEW vw_PendingNotifications AS
SELECT
    n.NotificationId,
    n.Channel,
    n.NotifType,
    n.Recipient,
    n.Subject,
    n.Body,
    n.RetryCount,
    n.CreatedAt,
    u.Email     AS UserEmail,
    u.PhoneNumber AS UserPhone
FROM Notification n
INNER JOIN [User] u ON n.UserId = u.UserId
WHERE n.[Status] = 'Pending'
  AND n.RetryCount < 3;   -- stop retrying after 3 failed attempts
GO

-- View: Open support tickets with customer and order info (Part 13)
CREATE VIEW vw_OpenSupportTickets AS
SELECT
    t.TicketId,
    t.TicketSource,
    t.TicketCategory,
    t.Subject,
    t.TicketStatus,
    t.CreatedAt,
    u.FirstName + ' ' + u.LastName  AS CustomerName,
    u.Email                         AS CustomerEmail,
    u.PhoneNumber                   AS CustomerPhone,
    o.OrderNumber,
    a.FirstName + ' ' + a.LastName  AS AssignedTo
FROM SupportTicket t
INNER JOIN [User] u         ON t.UserId           = u.UserId
LEFT  JOIN [Order] o        ON t.OrderId          = o.OrderId
LEFT  JOIN [User] a         ON t.AssignedToUserId = a.UserId
WHERE t.TicketStatus NOT IN ('Resolved','Closed');
GO


-- =============================================================================
-- SUMMARY
-- =============================================================================
PRINT '==============================================';
PRINT 'Taurus_schema.sql  —  Completed Successfully!';
PRINT '==============================================';
PRINT 'Version  : 4.0';
PRINT 'Platform : Google Cloud SQL for SQL Server';
PRINT '----------------------------------------------';
PRINT 'Tables   : 31 created';
PRINT '  Core   : User, Role, UserRole';
PRINT '  Auth   : OTPVerification, GuestSession        [NEW]';
PRINT '  Catalog: Category, Brand, Product, PriceHistory,';
PRINT '           ProductVariant, ProductImage';
PRINT '           (ProductImage stores GCS URLs only)';
PRINT '  Supply : Supplier, PurchaseOrder, PurchaseOrderItem';
PRINT '  Voucher: Voucher, UserVoucher, VoucherUsage';
PRINT '  Cart   : Cart (guest-aware), CartItem, Wishlist';
PRINT '  Order  : Order (+ pickup columns), OrderItem';
PRINT '  Inv    : InventoryLog (+ Lock/Unlock types)';
PRINT '  Payment: Payment (+ proof/verification columns)';
PRINT '  Deliver: Delivery (+ Failed status, IsDelayed)';
PRINT '  Review : Review';
PRINT '  POS    : POS_Session';
PRINT '  Support: SupportTicket, SupportTicketReply     [NEW]';
PRINT '  Notif  : Notification                          [NEW]';
PRINT '  System : SystemLog (expanded event types)';
PRINT '----------------------------------------------';
PRINT 'Views    : 8 created';
PRINT '  vw_UserVoucherUsageCount';
PRINT '  vw_ActiveProducts          (+ GCS image fields)';
PRINT '  vw_ProductImageGallery     (+ GCS storage fields)';
PRINT '  vw_OrderSummary            (+ pickup columns)';
PRINT '  vw_ProductVariantDetails';
PRINT '  vw_InventoryStatus';
PRINT '  vw_PendingNotifications    [NEW]';
PRINT '  vw_OpenSupportTickets      [NEW]';
PRINT '----------------------------------------------';
PRINT 'Triggers : 1 (TR_Product_PriceAudit)';
PRINT '----------------------------------------------';
PRINT 'COD NOTE : Cash on Delivery is NOT offered.';
PRINT '           PaymentMethod: GCash | BankTransfer | Cash (POS only)';
PRINT '----------------------------------------------';
PRINT 'GCS NOTE : Images/proofs/attachments stored in';
PRINT '           Google Cloud Storage. Only URLs and';
PRINT '           bucket/path metadata stored in DB.';
PRINT '----------------------------------------------';
PRINT 'Run Taurus_seed.sql next to populate data.';
PRINT '==============================================';
GO