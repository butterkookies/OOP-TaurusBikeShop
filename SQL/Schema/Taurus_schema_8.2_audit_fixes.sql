/*
 * ============================================================================
 *  Taurus Bike Shop — Schema 8.2 Audit Fix Migration
 * ============================================================================
 *  Addresses all findings from schema_audit_report.md (Critical, Major,
 *  Minor, Security, Performance, Scalability) against Taurus_schema_8.1.sql.
 *
 *  Target DB  : Taurus-bike-shop-sqlserver-2026
 *  Date       : 2026-04-15
 *
 *  Run order  : Execute batches top-to-bottom. Each GO-delimited batch is
 *               independent so partial re-runs are safe.
 *
 *  IMPORTANT  : Back up the database before running this migration.
 * ============================================================================
 */

USE [Taurus-bike-shop-sqlserver-2026]
GO

-- ============================================================================
--  SETUP: Temp helper to drop unnamed DEFAULT constraints
-- ============================================================================
CREATE PROCEDURE #DropDefault @tbl NVARCHAR(128), @col NVARCHAR(128)
AS
BEGIN
    DECLARE @sql NVARCHAR(MAX);
    SELECT @sql = 'ALTER TABLE [dbo].[' + @tbl + '] DROP CONSTRAINT [' + d.name + ']'
    FROM sys.default_constraints d
    INNER JOIN sys.columns c
        ON d.parent_object_id = c.object_id AND d.parent_column_id = c.column_id
    WHERE OBJECT_NAME(d.parent_object_id) = @tbl AND c.name = @col;
    IF @sql IS NOT NULL EXEC sp_executesql @sql;
END;
GO

-- ============================================================================
--  PHASE 1 — CRITICAL: Soft-delete columns  (Audit §4.3)
-- ============================================================================
--  Replace physical CASCADE deletes with logical deletion on financial tables.

ALTER TABLE [dbo].[User] ADD [IsDeleted] BIT NOT NULL CONSTRAINT [DF_User_IsDeleted] DEFAULT (0);
GO
ALTER TABLE [dbo].[Order] ADD [IsDeleted] BIT NOT NULL CONSTRAINT [DF_Order_IsDeleted] DEFAULT (0);
GO
ALTER TABLE [dbo].[Payment] ADD [IsDeleted] BIT NOT NULL CONSTRAINT [DF_Payment_IsDeleted] DEFAULT (0);
GO

-- Indexes for soft-delete filtering
CREATE NONCLUSTERED INDEX [IX_User_IsDeleted] ON [dbo].[User]([IsDeleted] ASC)
    WHERE ([IsDeleted] = 0);
GO
CREATE NONCLUSTERED INDEX [IX_Order_IsDeleted] ON [dbo].[Order]([IsDeleted] ASC)
    WHERE ([IsDeleted] = 0);
GO
CREATE NONCLUSTERED INDEX [IX_Payment_IsDeleted] ON [dbo].[Payment]([IsDeleted] ASC)
    WHERE ([IsDeleted] = 0);
GO


-- ============================================================================
--  PHASE 2 — CRITICAL: New columns on [Order]  (Audit §1.2, §2.3, §7.2, §7.3)
-- ============================================================================

-- §1.2 / §7.3  FulfillmentType — exclusive arc for Delivery vs Pickup vs WalkIn
ALTER TABLE [dbo].[Order] ADD [FulfillmentType] NVARCHAR(20) NOT NULL
    CONSTRAINT [DF_Order_FulfillmentType] DEFAULT ('Delivery');
GO

-- Backfill existing rows
UPDATE [dbo].[Order] SET [FulfillmentType] = 'WalkIn'  WHERE [IsWalkIn] = 1;
UPDATE [dbo].[Order] SET [FulfillmentType] = 'Pickup'
    WHERE [OrderId] IN (SELECT [OrderId] FROM [dbo].[PickupOrder])
      AND [IsWalkIn] = 0;
GO

ALTER TABLE [dbo].[Order] ADD CONSTRAINT [CK_Order_FulfillmentType]
    CHECK ([FulfillmentType] IN ('Delivery', 'Pickup', 'WalkIn'));
GO

CREATE NONCLUSTERED INDEX [IX_Order_FulfillmentType] ON [dbo].[Order]([FulfillmentType] ASC);
GO

-- §1.1  GuestSessionId — allow guest checkout without shadow users
ALTER TABLE [dbo].[Order] ADD [GuestSessionId] INT NULL;
GO
ALTER TABLE [dbo].[Order] ADD CONSTRAINT [FK_Order_GuestSession]
    FOREIGN KEY ([GuestSessionId]) REFERENCES [dbo].[GuestSession]([GuestSessionId]);
GO

-- Relax UserId to allow guest orders (one of UserId/GuestSessionId must be set)
-- First, we need to make UserId nullable
-- Note: FK_Order_User already exists; we keep it but allow NULL
EXEC #DropDefault 'Order', 'UserId';
GO

-- Drop FK, alter column, re-add FK
ALTER TABLE [dbo].[Order] DROP CONSTRAINT [FK_Order_User];
GO
ALTER TABLE [dbo].[Order] ALTER COLUMN [UserId] INT NULL;
GO
ALTER TABLE [dbo].[Order] ADD CONSTRAINT [FK_Order_User]
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([UserId]);
GO
ALTER TABLE [dbo].[Order] ADD CONSTRAINT [CK_Order_Owner]
    CHECK ([UserId] IS NOT NULL OR [GuestSessionId] IS NOT NULL);
GO

-- §2.3  CartId — cart-to-order lineage
ALTER TABLE [dbo].[Order] ADD [CartId] INT NULL;
GO
ALTER TABLE [dbo].[Order] ADD CONSTRAINT [FK_Order_Cart]
    FOREIGN KEY ([CartId]) REFERENCES [dbo].[Cart]([CartId]);
GO
CREATE NONCLUSTERED INDEX [IX_Order_CartId] ON [dbo].[Order]([CartId] ASC)
    WHERE ([CartId] IS NOT NULL);
GO

-- §7.2  POSSessionId — link walk-in orders to POS shifts
ALTER TABLE [dbo].[Order] ADD [POSSessionId] INT NULL;
GO
ALTER TABLE [dbo].[Order] ADD CONSTRAINT [FK_Order_POSSession]
    FOREIGN KEY ([POSSessionId]) REFERENCES [dbo].[POS_Session]([POSSessionId]);
GO
CREATE NONCLUSTERED INDEX [IX_Order_POSSessionId] ON [dbo].[Order]([POSSessionId] ASC)
    WHERE ([POSSessionId] IS NOT NULL);
GO


-- ============================================================================
--  PHASE 3 — CRITICAL: Computed column  (Audit §2.4)
-- ============================================================================

ALTER TABLE [dbo].[Order] ADD [TotalAmount] AS ([SubTotal] - [DiscountAmount] + [ShippingFee]) PERSISTED;
GO


-- ============================================================================
--  PHASE 4 — CRITICAL: CHECK constraints  (Audit §2.1, §1.4)
-- ============================================================================

-- §2.1  Enforce credentials on non-walk-in users
ALTER TABLE [dbo].[User] ADD CONSTRAINT [CK_User_Credentials]
    CHECK ([IsWalkIn] = 1 OR ([Email] IS NOT NULL AND [PasswordHash] IS NOT NULL));
GO

-- §1.4  Enforce minimum token entropy on guest sessions
ALTER TABLE [dbo].[GuestSession] WITH NOCHECK ADD CONSTRAINT [CK_GuestSession_TokenLength]
    CHECK (LEN([SessionToken]) >= 32);
GO
ALTER TABLE [dbo].[GuestSession] CHECK CONSTRAINT [CK_GuestSession_TokenLength];
GO


-- ============================================================================
--  PHASE 5 — CRITICAL: Unique index fixes  (Audit §1.3, §4.4, §3.6, §3.7)
-- ============================================================================

-- §1.3  GCash transaction ID must be unique (double-spend prevention)
DROP INDEX [IX_GCashPayment_TxnId] ON [dbo].[GCashPayment];
GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_GCashPayment_TxnId] ON [dbo].[GCashPayment]
(
    [GcashTransactionId] ASC
)
WHERE ([GcashTransactionId] IS NOT NULL);
GO

-- §4.4  BPI bank reference must be unique
DROP INDEX [IX_BTP_BpiRef] ON [dbo].[BankTransferPayment];
GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_BTP_BpiRef] ON [dbo].[BankTransferPayment]
(
    [BpiReferenceNumber] ASC
)
WHERE ([BpiReferenceNumber] IS NOT NULL);
GO

-- §3.6  User email must be unique for non-walk-in users
CREATE UNIQUE NONCLUSTERED INDEX [UX_User_Email] ON [dbo].[User]
(
    [Email] ASC
)
WHERE ([Email] IS NOT NULL AND [IsWalkIn] = 0);
GO

-- §3.7  ProductVariant SKU must be unique
DROP INDEX [IX_ProductVariant_SKU] ON [dbo].[ProductVariant];
GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_ProductVariant_SKU] ON [dbo].[ProductVariant]
(
    [SKU] ASC
)
WHERE ([SKU] IS NOT NULL);
GO

-- §1.2  Delivery.OrderId must be unique (one delivery per order)
CREATE UNIQUE NONCLUSTERED INDEX [UX_Delivery_Order] ON [dbo].[Delivery]
(
    [OrderId] ASC
);
GO


-- ============================================================================
--  PHASE 6 — PERFORMANCE: Remove useless indexes  (Audit §5.3, §5.4)
-- ============================================================================

-- §5.3  Boolean indexes with only 2 distinct values — useless for optimizer
DROP INDEX [IX_Brand_IsActive]          ON [dbo].[Brand];
GO
DROP INDEX [IX_Cart_IsCheckedOut]       ON [dbo].[Cart];
GO
DROP INDEX [IX_Category_IsActive]       ON [dbo].[Category];
GO
DROP INDEX [IX_Product_IsActive]        ON [dbo].[Product];
GO
DROP INDEX [IX_Product_IsFeatured]      ON [dbo].[Product];
GO
DROP INDEX [IX_ProductVariant_IsActive] ON [dbo].[ProductVariant];
GO
DROP INDEX [IX_OTP_IsUsed]             ON [dbo].[OTPVerification];
GO

-- §5.4  Redundant index — UX_PickupOrder_Order already covers OrderId
DROP INDEX [IX_PickupOrder_OrderId] ON [dbo].[PickupOrder];
GO

-- Redundant: IX_Delivery_OrderId is now covered by UX_Delivery_Order
DROP INDEX [IX_Delivery_OrderId] ON [dbo].[Delivery];
GO


-- ============================================================================
--  PHASE 7 — PERFORMANCE: Add composite indexes  (Audit §5.5)
-- ============================================================================

-- Orders by user + status (common dashboard query)
CREATE NONCLUSTERED INDEX [IX_Order_UserId_Status] ON [dbo].[Order]
(
    [UserId] ASC,
    [OrderStatus] ASC
)
INCLUDE ([OrderDate], [TotalAmount], [FulfillmentType])
WHERE ([IsDeleted] = 0);
GO

-- Unread notifications per user
CREATE NONCLUSTERED INDEX [IX_Notif_UserId_Unread] ON [dbo].[Notification]
(
    [UserId] ASC,
    [IsRead] ASC
)
INCLUDE ([NotifType], [Subject], [CreatedAt])
WHERE ([IsRead] = 0);
GO

-- Active cart items for a user
CREATE NONCLUSTERED INDEX [IX_CartItem_CartId_Product] ON [dbo].[CartItem]
(
    [CartId] ASC,
    [ProductId] ASC,
    [ProductVariantId] ASC
);
GO


-- ============================================================================
--  PHASE 8 — SECURITY: OTP and Product spec improvements  (Audit §3.2, §3.1)
-- ============================================================================

-- §3.2  Expand OTPCode to fit SHA-256 hashes (64 hex chars)
ALTER TABLE [dbo].[OTPVerification] ALTER COLUMN [OTPCode] NVARCHAR(128) NOT NULL;
GO

-- §3.1  Validate AdditionalSpecs is proper JSON
ALTER TABLE [dbo].[Product] ADD CONSTRAINT [CK_Product_AdditionalSpecs]
    CHECK ([AdditionalSpecs] IS NULL OR ISJSON([AdditionalSpecs]) = 1);
GO


-- ============================================================================
--  PHASE 9 — MAJOR: New tables  (Audit §7.5, §4.2)
-- ============================================================================

-- §7.5  Refund table — financial return/refund tracking
CREATE TABLE [dbo].[Refund](
    [RefundId]           [int] IDENTITY(1,1) NOT NULL,
    [OrderId]            [int] NOT NULL,
    [PaymentId]          [int] NULL,
    [RefundAmount]       [decimal](10, 2) NOT NULL,
    [RefundReason]       [nvarchar](500) NOT NULL,
    [RefundStatus]       [nvarchar](50) NOT NULL,
    [RefundMethod]       [nvarchar](50) NULL,
    [RequestedByUserId]  [int] NULL,
    [ApprovedByUserId]   [int] NULL,
    [TicketId]           [int] NULL,
    [Notes]              [nvarchar](500) NULL,
    [CreatedAt]          [datetime2](7) NOT NULL,
    [ProcessedAt]        [datetime2](7) NULL,
 CONSTRAINT [PK_Refund] PRIMARY KEY CLUSTERED ([RefundId] ASC),
 CONSTRAINT [FK_Refund_Order]       FOREIGN KEY ([OrderId])           REFERENCES [dbo].[Order]([OrderId]),
 CONSTRAINT [FK_Refund_Payment]     FOREIGN KEY ([PaymentId])         REFERENCES [dbo].[Payment]([PaymentId]),
 CONSTRAINT [FK_Refund_RequestedBy] FOREIGN KEY ([RequestedByUserId]) REFERENCES [dbo].[User]([UserId]),
 CONSTRAINT [FK_Refund_ApprovedBy]  FOREIGN KEY ([ApprovedByUserId])  REFERENCES [dbo].[User]([UserId]),
 CONSTRAINT [FK_Refund_Ticket]      FOREIGN KEY ([TicketId])          REFERENCES [dbo].[SupportTicket]([TicketId]),
 CONSTRAINT [CK_Refund_Amount]      CHECK ([RefundAmount] > 0),
 CONSTRAINT [CK_Refund_Status]      CHECK ([RefundStatus] IN ('Pending','Approved','Rejected','Completed')),
 CONSTRAINT [CK_Refund_Method]      CHECK ([RefundMethod] IS NULL OR [RefundMethod] IN ('OriginalPayment','StoreCredit','Cash'))
) ON [PRIMARY];
GO

ALTER TABLE [dbo].[Refund] ADD CONSTRAINT [DF_Refund_Status]    DEFAULT ('Pending')      FOR [RefundStatus];
GO
ALTER TABLE [dbo].[Refund] ADD CONSTRAINT [DF_Refund_CreatedAt] DEFAULT (SYSDATETIME())  FOR [CreatedAt];
GO

CREATE NONCLUSTERED INDEX [IX_Refund_OrderId]  ON [dbo].[Refund]([OrderId] ASC);
GO
CREATE NONCLUSTERED INDEX [IX_Refund_Status]   ON [dbo].[Refund]([RefundStatus] ASC)
    WHERE ([RefundStatus] IN ('Pending','Approved'));
GO
CREATE NONCLUSTERED INDEX [IX_Refund_TicketId] ON [dbo].[Refund]([TicketId] ASC)
    WHERE ([TicketId] IS NOT NULL);
GO


-- §4.2  ActiveSession — JWT/refresh token management for WPF + Web
CREATE TABLE [dbo].[ActiveSession](
    [SessionId]    [int] IDENTITY(1,1) NOT NULL,
    [UserId]       [int] NOT NULL,
    [RefreshToken]  [nvarchar](500) NOT NULL,
    [DeviceInfo]   [nvarchar](500) NULL,
    [IpAddress]    [nvarchar](50) NULL,
    [IsRevoked]    [bit] NOT NULL,
    [ExpiresAt]    [datetime2](7) NOT NULL,
    [CreatedAt]    [datetime2](7) NOT NULL,
    [RevokedAt]    [datetime2](7) NULL,
 CONSTRAINT [PK_ActiveSession] PRIMARY KEY CLUSTERED ([SessionId] ASC),
 CONSTRAINT [FK_ActiveSession_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([UserId])
) ON [PRIMARY];
GO

ALTER TABLE [dbo].[ActiveSession] ADD CONSTRAINT [DF_ActiveSession_IsRevoked]  DEFAULT (0)              FOR [IsRevoked];
GO
ALTER TABLE [dbo].[ActiveSession] ADD CONSTRAINT [DF_ActiveSession_CreatedAt]  DEFAULT (SYSDATETIME())  FOR [CreatedAt];
GO

CREATE UNIQUE NONCLUSTERED INDEX [UX_ActiveSession_Token] ON [dbo].[ActiveSession]([RefreshToken] ASC);
GO
CREATE NONCLUSTERED INDEX [IX_ActiveSession_UserId]  ON [dbo].[ActiveSession]([UserId] ASC);
GO
CREATE NONCLUSTERED INDEX [IX_ActiveSession_Active]  ON [dbo].[ActiveSession]([UserId] ASC, [ExpiresAt] ASC)
    WHERE ([IsRevoked] = 0);
GO


-- ============================================================================
--  PHASE 10 — SECURITY: Remove dangerous ON DELETE CASCADE  (Audit §4.3)
-- ============================================================================
--  Keep CASCADE on TPT subtypes (GCash/BankTransfer->Payment,
--  Lalamove/LBC->Delivery) and ephemeral data (CartItem->Cart).
--  Remove CASCADE from financially/audit-sensitive relationships.

-- Address -> User: removing user shouldn't destroy address history
ALTER TABLE [dbo].[Address] DROP CONSTRAINT [FK_Address_User];
GO
ALTER TABLE [dbo].[Address] ADD CONSTRAINT [FK_Address_User]
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([UserId]);
GO

-- OrderItem -> Order: removing order shouldn't destroy line items
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK_OrderItem_Order];
GO
ALTER TABLE [dbo].[OrderItem] ADD CONSTRAINT [FK_OrderItem_Order]
    FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Order]([OrderId]);
GO

-- Notification -> User: audit trail
ALTER TABLE [dbo].[Notification] DROP CONSTRAINT [FK_Notif_User];
GO
ALTER TABLE [dbo].[Notification] ADD CONSTRAINT [FK_Notif_User]
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([UserId]);
GO

-- UserRole -> User: role history
ALTER TABLE [dbo].[UserRole] DROP CONSTRAINT [FK_UserRole_User];
GO
ALTER TABLE [dbo].[UserRole] ADD CONSTRAINT [FK_UserRole_User]
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([UserId]);
GO

-- Wishlist -> User
ALTER TABLE [dbo].[Wishlist] DROP CONSTRAINT [FK_Wishlist_User];
GO
ALTER TABLE [dbo].[Wishlist] ADD CONSTRAINT [FK_Wishlist_User]
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([UserId]);
GO

-- ProductImage -> Product: keep image history
ALTER TABLE [dbo].[ProductImage] DROP CONSTRAINT [FK_ProductImage_Product];
GO
ALTER TABLE [dbo].[ProductImage] ADD CONSTRAINT [FK_ProductImage_Product]
    FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product]([ProductId]);
GO

-- ProductVariant -> Product: inventory history
ALTER TABLE [dbo].[ProductVariant] DROP CONSTRAINT [FK_ProductVariant_Product];
GO
ALTER TABLE [dbo].[ProductVariant] ADD CONSTRAINT [FK_ProductVariant_Product]
    FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product]([ProductId]);
GO


-- ============================================================================
--  PHASE 11 — SCALABILITY: Upgrade audit table PKs to BIGINT  (Audit §6.1)
-- ============================================================================

-- SystemLog
ALTER TABLE [dbo].[SystemLog] DROP CONSTRAINT [PK_SystemLog];
GO
ALTER TABLE [dbo].[SystemLog] ALTER COLUMN [SystemLogId] BIGINT NOT NULL;
GO
ALTER TABLE [dbo].[SystemLog] ADD CONSTRAINT [PK_SystemLog] PRIMARY KEY CLUSTERED ([SystemLogId] ASC);
GO

-- InventoryLog
ALTER TABLE [dbo].[InventoryLog] DROP CONSTRAINT [PK_InventoryLog];
GO
ALTER TABLE [dbo].[InventoryLog] ALTER COLUMN [InventoryLogId] BIGINT NOT NULL;
GO
ALTER TABLE [dbo].[InventoryLog] ADD CONSTRAINT [PK_InventoryLog] PRIMARY KEY CLUSTERED ([InventoryLogId] ASC);
GO

-- Notification
ALTER TABLE [dbo].[Notification] DROP CONSTRAINT [PK_Notification];
GO
ALTER TABLE [dbo].[Notification] ALTER COLUMN [NotificationId] BIGINT NOT NULL;
GO
ALTER TABLE [dbo].[Notification] ADD CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED ([NotificationId] ASC);
GO

-- SupportTicketReply
ALTER TABLE [dbo].[SupportTicketReply] DROP CONSTRAINT [PK_SupportTicketReply];
GO
ALTER TABLE [dbo].[SupportTicketReply] ALTER COLUMN [ReplyId] BIGINT NOT NULL;
GO
ALTER TABLE [dbo].[SupportTicketReply] ADD CONSTRAINT [PK_SupportTicketReply] PRIMARY KEY CLUSTERED ([ReplyId] ASC);
GO

-- OrderStatusAudit — PK is unnamed, find and drop dynamically
DECLARE @osa_pk NVARCHAR(128);
SELECT @osa_pk = kc.name
FROM sys.key_constraints kc
WHERE kc.parent_object_id = OBJECT_ID('[dbo].[OrderStatusAudit]') AND kc.[type] = 'PK';

IF @osa_pk IS NOT NULL
BEGIN
    EXEC('ALTER TABLE [dbo].[OrderStatusAudit] DROP CONSTRAINT [' + @osa_pk + ']');
END
GO

ALTER TABLE [dbo].[OrderStatusAudit] ALTER COLUMN [AuditId] BIGINT NOT NULL;
GO
ALTER TABLE [dbo].[OrderStatusAudit] ADD CONSTRAINT [PK_OrderStatusAudit] PRIMARY KEY CLUSTERED ([AuditId] ASC);
GO


-- ============================================================================
--  PHASE 12 — CONSISTENCY: Standardize datetime -> datetime2(7)  (Audit §3.4)
-- ============================================================================
--  All timestamp columns standardized for microsecond precision.
--  Columns with indexes are handled first (drop index, alter, recreate).

/*
 *  12a — Drop indexes that include datetime columns
 */
DROP INDEX [IX_BTP_VerificationDeadline]  ON [dbo].[BankTransferPayment];
GO
DROP INDEX [IX_GuestSession_ExpiresAt]    ON [dbo].[GuestSession];
GO
DROP INDEX [IX_InvLog_CreatedAt]          ON [dbo].[InventoryLog];
GO
DROP INDEX [IX_Notif_CreatedAt]           ON [dbo].[Notification];
GO
DROP INDEX [IX_Notif_Pending]             ON [dbo].[Notification];
GO
DROP INDEX [IX_Order_OrderDate]           ON [dbo].[Order];
GO
DROP INDEX [IX_OTP_ExpiresAt]             ON [dbo].[OTPVerification];
GO
DROP INDEX [IX_POS_Session_ShiftEnd]      ON [dbo].[POS_Session];
GO
DROP INDEX [IX_POS_Session_ShiftStart]    ON [dbo].[POS_Session];
GO
DROP INDEX [IX_PriceHistory_ChangedAt]    ON [dbo].[PriceHistory];
GO
DROP INDEX [IX_PriceHistory_ProductId]    ON [dbo].[PriceHistory];
GO
DROP INDEX [IX_PickupOrder_ExpiresAt]     ON [dbo].[PickupOrder];
GO

/*
 *  12b — Drop default constraints on all datetime columns, alter to datetime2(7),
 *         then re-add defaults using SYSDATETIME().
 *         Nullable columns without defaults are altered directly.
 */

-- ── Address ──
EXEC #DropDefault 'Address', 'CreatedAt';
GO
ALTER TABLE [dbo].[Address] ALTER COLUMN [CreatedAt] datetime2(7) NOT NULL;
GO
ALTER TABLE [dbo].[Address] ADD DEFAULT (SYSDATETIME()) FOR [CreatedAt];
GO

-- ── Brand ──
EXEC #DropDefault 'Brand', 'CreatedAt';
GO
ALTER TABLE [dbo].[Brand] ALTER COLUMN [CreatedAt] datetime2(7) NOT NULL;
GO
ALTER TABLE [dbo].[Brand] ADD DEFAULT (SYSDATETIME()) FOR [CreatedAt];
GO

-- ── Cart ──
EXEC #DropDefault 'Cart', 'CreatedAt';
GO
ALTER TABLE [dbo].[Cart] ALTER COLUMN [CreatedAt] datetime2(7) NOT NULL;
ALTER TABLE [dbo].[Cart] ALTER COLUMN [LastUpdatedAt] datetime2(7) NULL;
GO
ALTER TABLE [dbo].[Cart] ADD DEFAULT (SYSDATETIME()) FOR [CreatedAt];
GO

-- ── CartItem ──
EXEC #DropDefault 'CartItem', 'AddedAt';
GO
ALTER TABLE [dbo].[CartItem] ALTER COLUMN [AddedAt] datetime2(7) NOT NULL;
GO
ALTER TABLE [dbo].[CartItem] ADD DEFAULT (SYSDATETIME()) FOR [AddedAt];
GO

-- ── Delivery ──
EXEC #DropDefault 'Delivery', 'CreatedAt';
GO
ALTER TABLE [dbo].[Delivery] ALTER COLUMN [CreatedAt] datetime2(7) NOT NULL;
ALTER TABLE [dbo].[Delivery] ALTER COLUMN [DelayedUntil] datetime2(7) NULL;
ALTER TABLE [dbo].[Delivery] ALTER COLUMN [EstimatedDeliveryTime] datetime2(7) NULL;
ALTER TABLE [dbo].[Delivery] ALTER COLUMN [ActualDeliveryTime] datetime2(7) NULL;
GO
ALTER TABLE [dbo].[Delivery] ADD DEFAULT (SYSDATETIME()) FOR [CreatedAt];
GO

-- ── GCashPayment ──
ALTER TABLE [dbo].[GCashPayment] ALTER COLUMN [SubmittedAt] datetime2(7) NULL;
GO

-- ── BankTransferPayment ──
ALTER TABLE [dbo].[BankTransferPayment] ALTER COLUMN [VerifiedAt] datetime2(7) NULL;
ALTER TABLE [dbo].[BankTransferPayment] ALTER COLUMN [VerificationDeadline] datetime2(7) NULL;
ALTER TABLE [dbo].[BankTransferPayment] ALTER COLUMN [SubmittedAt] datetime2(7) NULL;
GO

-- ── GuestSession ──
EXEC #DropDefault 'GuestSession', 'CreatedAt';
GO
ALTER TABLE [dbo].[GuestSession] ALTER COLUMN [ExpiresAt] datetime2(7) NOT NULL;
ALTER TABLE [dbo].[GuestSession] ALTER COLUMN [CreatedAt] datetime2(7) NOT NULL;
GO
ALTER TABLE [dbo].[GuestSession] ADD DEFAULT (SYSDATETIME()) FOR [CreatedAt];
GO

-- ── InventoryLog ──
EXEC #DropDefault 'InventoryLog', 'CreatedAt';
GO
ALTER TABLE [dbo].[InventoryLog] ALTER COLUMN [CreatedAt] datetime2(7) NOT NULL;
GO
ALTER TABLE [dbo].[InventoryLog] ADD DEFAULT (SYSDATETIME()) FOR [CreatedAt];
GO

-- ── Notification ──
EXEC #DropDefault 'Notification', 'CreatedAt';
GO
ALTER TABLE [dbo].[Notification] ALTER COLUMN [CreatedAt] datetime2(7) NOT NULL;
ALTER TABLE [dbo].[Notification] ALTER COLUMN [SentAt] datetime2(7) NULL;
GO
ALTER TABLE [dbo].[Notification] ADD DEFAULT (SYSDATETIME()) FOR [CreatedAt];
GO

-- ── Order ──
EXEC #DropDefault 'Order', 'OrderDate';
EXEC #DropDefault 'Order', 'CreatedAt';
GO
ALTER TABLE [dbo].[Order] ALTER COLUMN [OrderDate] datetime2(7) NOT NULL;
ALTER TABLE [dbo].[Order] ALTER COLUMN [CreatedAt] datetime2(7) NOT NULL;
ALTER TABLE [dbo].[Order] ALTER COLUMN [UpdatedAt] datetime2(7) NULL;
GO
ALTER TABLE [dbo].[Order] ADD DEFAULT (SYSDATETIME()) FOR [OrderDate];
ALTER TABLE [dbo].[Order] ADD DEFAULT (SYSDATETIME()) FOR [CreatedAt];
GO

-- ── OTPVerification ──
EXEC #DropDefault 'OTPVerification', 'CreatedAt';
GO
ALTER TABLE [dbo].[OTPVerification] ALTER COLUMN [ExpiresAt] datetime2(7) NOT NULL;
ALTER TABLE [dbo].[OTPVerification] ALTER COLUMN [CreatedAt] datetime2(7) NOT NULL;
GO
ALTER TABLE [dbo].[OTPVerification] ADD DEFAULT (SYSDATETIME()) FOR [CreatedAt];
GO

-- ── Payment ──
EXEC #DropDefault 'Payment', 'CreatedAt';
GO
ALTER TABLE [dbo].[Payment] ALTER COLUMN [PaymentDate] datetime2(7) NULL;
ALTER TABLE [dbo].[Payment] ALTER COLUMN [CreatedAt] datetime2(7) NOT NULL;
GO
ALTER TABLE [dbo].[Payment] ADD DEFAULT (SYSDATETIME()) FOR [CreatedAt];
GO

-- ── PickupOrder ──
ALTER TABLE [dbo].[PickupOrder] ALTER COLUMN [PickupReadyAt] datetime2(7) NULL;
ALTER TABLE [dbo].[PickupOrder] ALTER COLUMN [PickupExpiresAt] datetime2(7) NULL;
ALTER TABLE [dbo].[PickupOrder] ALTER COLUMN [PickupConfirmedAt] datetime2(7) NULL;
GO

-- ── POS_Session ──
EXEC #DropDefault 'POS_Session', 'CreatedAt';
GO
ALTER TABLE [dbo].[POS_Session] ALTER COLUMN [ShiftStart] datetime2(7) NOT NULL;
ALTER TABLE [dbo].[POS_Session] ALTER COLUMN [ShiftEnd] datetime2(7) NULL;
ALTER TABLE [dbo].[POS_Session] ALTER COLUMN [CreatedAt] datetime2(7) NOT NULL;
GO
ALTER TABLE [dbo].[POS_Session] ADD DEFAULT (SYSDATETIME()) FOR [CreatedAt];
GO

-- ── PriceHistory ──
EXEC #DropDefault 'PriceHistory', 'ChangedAt';
GO
ALTER TABLE [dbo].[PriceHistory] ALTER COLUMN [ChangedAt] datetime2(7) NOT NULL;
GO
ALTER TABLE [dbo].[PriceHistory] ADD DEFAULT (SYSDATETIME()) FOR [ChangedAt];
GO

-- ── Product ──
EXEC #DropDefault 'Product', 'CreatedAt';
GO
ALTER TABLE [dbo].[Product] ALTER COLUMN [CreatedAt] datetime2(7) NOT NULL;
ALTER TABLE [dbo].[Product] ALTER COLUMN [UpdatedAt] datetime2(7) NULL;
GO
ALTER TABLE [dbo].[Product] ADD DEFAULT (SYSDATETIME()) FOR [CreatedAt];
GO

-- ── ProductImage ──
EXEC #DropDefault 'ProductImage', 'CreatedAt';
GO
ALTER TABLE [dbo].[ProductImage] ALTER COLUMN [CreatedAt] datetime2(7) NOT NULL;
GO
ALTER TABLE [dbo].[ProductImage] ADD DEFAULT (SYSDATETIME()) FOR [CreatedAt];
GO

-- ── ProductVariant ──
EXEC #DropDefault 'ProductVariant', 'CreatedAt';
GO
ALTER TABLE [dbo].[ProductVariant] ALTER COLUMN [CreatedAt] datetime2(7) NOT NULL;
ALTER TABLE [dbo].[ProductVariant] ALTER COLUMN [UpdatedAt] datetime2(7) NULL;
GO
ALTER TABLE [dbo].[ProductVariant] ADD DEFAULT (SYSDATETIME()) FOR [CreatedAt];
GO

-- ── PurchaseOrder ──
EXEC #DropDefault 'PurchaseOrder', 'OrderDate';
EXEC #DropDefault 'PurchaseOrder', 'CreatedAt';
GO
ALTER TABLE [dbo].[PurchaseOrder] ALTER COLUMN [OrderDate] datetime2(7) NOT NULL;
ALTER TABLE [dbo].[PurchaseOrder] ALTER COLUMN [ExpectedDeliveryDate] datetime2(7) NULL;
ALTER TABLE [dbo].[PurchaseOrder] ALTER COLUMN [CreatedAt] datetime2(7) NOT NULL;
GO
ALTER TABLE [dbo].[PurchaseOrder] ADD DEFAULT (SYSDATETIME()) FOR [OrderDate];
ALTER TABLE [dbo].[PurchaseOrder] ADD DEFAULT (SYSDATETIME()) FOR [CreatedAt];
GO

-- ── Review ──
EXEC #DropDefault 'Review', 'CreatedAt';
GO
ALTER TABLE [dbo].[Review] ALTER COLUMN [CreatedAt] datetime2(7) NOT NULL;
GO
ALTER TABLE [dbo].[Review] ADD DEFAULT (SYSDATETIME()) FOR [CreatedAt];
GO

-- ── Role ──
EXEC #DropDefault 'Role', 'CreatedAt';
GO
ALTER TABLE [dbo].[Role] ALTER COLUMN [CreatedAt] datetime2(7) NOT NULL;
GO
ALTER TABLE [dbo].[Role] ADD DEFAULT (SYSDATETIME()) FOR [CreatedAt];
GO

-- ── Supplier ──
EXEC #DropDefault 'Supplier', 'CreatedAt';
GO
ALTER TABLE [dbo].[Supplier] ALTER COLUMN [CreatedAt] datetime2(7) NOT NULL;
GO
ALTER TABLE [dbo].[Supplier] ADD DEFAULT (SYSDATETIME()) FOR [CreatedAt];
GO

-- ── SupportTask ──
EXEC #DropDefault 'SupportTask', 'CreatedAt';
GO
ALTER TABLE [dbo].[SupportTask] ALTER COLUMN [DueDate] datetime2(7) NULL;
ALTER TABLE [dbo].[SupportTask] ALTER COLUMN [CreatedAt] datetime2(7) NOT NULL;
ALTER TABLE [dbo].[SupportTask] ALTER COLUMN [CompletedAt] datetime2(7) NULL;
GO
ALTER TABLE [dbo].[SupportTask] ADD DEFAULT (SYSDATETIME()) FOR [CreatedAt];
GO

-- ── SupportTicket ──
EXEC #DropDefault 'SupportTicket', 'CreatedAt';
GO
ALTER TABLE [dbo].[SupportTicket] ALTER COLUMN [ResolvedAt] datetime2(7) NULL;
ALTER TABLE [dbo].[SupportTicket] ALTER COLUMN [CreatedAt] datetime2(7) NOT NULL;
ALTER TABLE [dbo].[SupportTicket] ALTER COLUMN [UpdatedAt] datetime2(7) NULL;
GO
ALTER TABLE [dbo].[SupportTicket] ADD DEFAULT (SYSDATETIME()) FOR [CreatedAt];
GO

-- ── SupportTicketReply ──
EXEC #DropDefault 'SupportTicketReply', 'CreatedAt';
GO
ALTER TABLE [dbo].[SupportTicketReply] ALTER COLUMN [CreatedAt] datetime2(7) NOT NULL;
GO
ALTER TABLE [dbo].[SupportTicketReply] ADD DEFAULT (SYSDATETIME()) FOR [CreatedAt];
GO

-- ── SystemLog ──
EXEC #DropDefault 'SystemLog', 'CreatedAt';
GO
ALTER TABLE [dbo].[SystemLog] ALTER COLUMN [CreatedAt] datetime2(7) NOT NULL;
GO
ALTER TABLE [dbo].[SystemLog] ADD DEFAULT (SYSDATETIME()) FOR [CreatedAt];
GO

-- ── User ──
EXEC #DropDefault 'User', 'CreatedAt';
GO
ALTER TABLE [dbo].[User] ALTER COLUMN [CreatedAt] datetime2(7) NOT NULL;
ALTER TABLE [dbo].[User] ALTER COLUMN [LastLoginAt] datetime2(7) NULL;
ALTER TABLE [dbo].[User] ALTER COLUMN [LockoutUntil] datetime2(7) NULL;
GO
ALTER TABLE [dbo].[User] ADD DEFAULT (SYSDATETIME()) FOR [CreatedAt];
GO

-- ── UserRole ──
EXEC #DropDefault 'UserRole', 'AssignedAt';
GO
ALTER TABLE [dbo].[UserRole] ALTER COLUMN [AssignedAt] datetime2(7) NOT NULL;
GO
ALTER TABLE [dbo].[UserRole] ADD DEFAULT (SYSDATETIME()) FOR [AssignedAt];
GO

-- ── UserVoucher ──
EXEC #DropDefault 'UserVoucher', 'AssignedAt';
GO
ALTER TABLE [dbo].[UserVoucher] ALTER COLUMN [AssignedAt] datetime2(7) NOT NULL;
ALTER TABLE [dbo].[UserVoucher] ALTER COLUMN [ExpiresAt] datetime2(7) NULL;
GO
ALTER TABLE [dbo].[UserVoucher] ADD DEFAULT (SYSDATETIME()) FOR [AssignedAt];
GO

-- ── Voucher ──
EXEC #DropDefault 'Voucher', 'CreatedAt';
GO
ALTER TABLE [dbo].[Voucher] ALTER COLUMN [StartDate] datetime2(7) NOT NULL;
ALTER TABLE [dbo].[Voucher] ALTER COLUMN [EndDate] datetime2(7) NULL;
ALTER TABLE [dbo].[Voucher] ALTER COLUMN [CreatedAt] datetime2(7) NOT NULL;
GO
ALTER TABLE [dbo].[Voucher] ADD DEFAULT (SYSDATETIME()) FOR [CreatedAt];
GO

-- ── VoucherUsage ──
EXEC #DropDefault 'VoucherUsage', 'UsedAt';
GO
ALTER TABLE [dbo].[VoucherUsage] ALTER COLUMN [UsedAt] datetime2(7) NOT NULL;
GO
ALTER TABLE [dbo].[VoucherUsage] ADD DEFAULT (SYSDATETIME()) FOR [UsedAt];
GO

-- ── Wishlist ──
EXEC #DropDefault 'Wishlist', 'AddedAt';
GO
ALTER TABLE [dbo].[Wishlist] ALTER COLUMN [AddedAt] datetime2(7) NOT NULL;
GO
ALTER TABLE [dbo].[Wishlist] ADD DEFAULT (SYSDATETIME()) FOR [AddedAt];
GO


/*
 *  12c — Recreate indexes that were dropped in 12a (now on datetime2 columns)
 */

CREATE NONCLUSTERED INDEX [IX_BTP_VerificationDeadline] ON [dbo].[BankTransferPayment]
    ([VerificationDeadline] ASC) WHERE ([VerificationDeadline] IS NOT NULL);
GO
CREATE NONCLUSTERED INDEX [IX_GuestSession_ExpiresAt] ON [dbo].[GuestSession]
    ([ExpiresAt] ASC);
GO
CREATE NONCLUSTERED INDEX [IX_InvLog_CreatedAt] ON [dbo].[InventoryLog]
    ([CreatedAt] ASC);
GO
CREATE NONCLUSTERED INDEX [IX_Notif_CreatedAt] ON [dbo].[Notification]
    ([CreatedAt] ASC);
GO
CREATE NONCLUSTERED INDEX [IX_Notif_Pending] ON [dbo].[Notification]
    ([Status] ASC, [CreatedAt] ASC) WHERE ([Status] = 'Pending');
GO
CREATE NONCLUSTERED INDEX [IX_Order_OrderDate] ON [dbo].[Order]
    ([OrderDate] ASC);
GO
CREATE NONCLUSTERED INDEX [IX_OTP_ExpiresAt] ON [dbo].[OTPVerification]
    ([ExpiresAt] ASC);
GO
CREATE NONCLUSTERED INDEX [IX_POS_Session_ShiftEnd] ON [dbo].[POS_Session]
    ([ShiftEnd] ASC);
GO
CREATE NONCLUSTERED INDEX [IX_POS_Session_ShiftStart] ON [dbo].[POS_Session]
    ([ShiftStart] ASC);
GO
CREATE NONCLUSTERED INDEX [IX_PriceHistory_ChangedAt] ON [dbo].[PriceHistory]
    ([ChangedAt] ASC);
GO
CREATE NONCLUSTERED INDEX [IX_PriceHistory_ProductId] ON [dbo].[PriceHistory]
    ([ProductId] ASC, [ChangedAt] DESC);
GO
CREATE NONCLUSTERED INDEX [IX_PickupOrder_ExpiresAt] ON [dbo].[PickupOrder]
    ([PickupExpiresAt] ASC) WHERE ([PickupExpiresAt] IS NOT NULL);
GO


-- ============================================================================
--  PHASE 13 — INTEGRITY: Trigger to prevent Delivery on walk-in orders  (Audit §7.1)
-- ============================================================================

CREATE OR ALTER TRIGGER [dbo].[TR_Delivery_NoWalkIn] ON [dbo].[Delivery]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (
        SELECT 1
        FROM inserted i
        INNER JOIN [dbo].[Order] o ON i.OrderId = o.OrderId
        WHERE o.IsWalkIn = 1
    )
    BEGIN
        RAISERROR('Cannot create a delivery record for a walk-in order. Walk-in orders are fulfilled immediately at the counter.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END;
GO


-- ============================================================================
--  PHASE 14 — VIEWS: Update views to reflect schema changes  (Audit §4.3)
-- ============================================================================

-- vw_OrderSummary — add TotalAmount, FulfillmentType, filter soft-deleted
ALTER VIEW [dbo].[vw_OrderSummary] AS
SELECT
    o.OrderId,
    o.OrderNumber,
    o.OrderDate,
    o.OrderStatus,
    o.FulfillmentType,
    o.TotalAmount,
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
WHERE o.IsDeleted = 0
GROUP BY
    o.OrderId, o.OrderNumber, o.OrderDate, o.OrderStatus,
    o.FulfillmentType, o.TotalAmount,
    o.SubTotal, o.DiscountAmount, o.ShippingFee, o.IsWalkIn,
    u.FirstName, u.LastName, u.Email, u.PhoneNumber,
    a.Street, a.City, a.PostalCode, a.Province,
    po.PickupReadyAt, po.PickupExpiresAt, po.PickupConfirmedAt;
GO

-- vw_PendingNotifications — filter soft-deleted users
ALTER VIEW [dbo].[vw_PendingNotifications] AS
SELECT
    n.NotificationId, n.Channel, n.NotifType,
    n.Recipient, n.Subject, n.Body,
    n.RetryCount, n.CreatedAt,
    u.Email       AS UserEmail,
    u.PhoneNumber AS UserPhone
FROM Notification n
INNER JOIN [User] u ON n.UserId = u.UserId
WHERE n.[Status] = 'Pending' AND n.RetryCount < 3 AND u.IsDeleted = 0;
GO

-- vw_OpenSupportTickets — filter soft-deleted users
ALTER VIEW [dbo].[vw_OpenSupportTickets] AS
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
WHERE t.TicketStatus NOT IN ('Resolved','Closed')
  AND u.IsDeleted = 0;
GO


-- ============================================================================
--  PHASE 15 — CLEANUP
-- ============================================================================

DROP PROCEDURE #DropDefault;
GO

PRINT '========================================';
PRINT '  Migration 8.2 complete.';
PRINT '  All audit findings addressed.';
PRINT '========================================';
GO
