-- =============================================================================
-- Taurus_schema_8.0_patch_payments.sql
-- Adds missing columns to GCashPayment and BankTransferPayment tables,
-- and updates vw_PaymentDetail to include the new GCash screenshot fields.
--
-- Run against the TaurusBikeShop database AFTER Taurus_schema_8.0.sql
-- =============================================================================

USE [TaurusBikeShop];
GO

-- ─────────────────────────────────────────────────────────────────────────────
-- 1. GCashPayment — add ScreenshotUrl, StorageBucket, StoragePath, SubmittedAt
-- ─────────────────────────────────────────────────────────────────────────────
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'GCashPayment' AND COLUMN_NAME = 'ScreenshotUrl'
)
BEGIN
    ALTER TABLE [dbo].[GCashPayment]
        ADD [ScreenshotUrl] [nvarchar](1000) NULL;
END
GO

IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'GCashPayment' AND COLUMN_NAME = 'StorageBucket'
)
BEGIN
    ALTER TABLE [dbo].[GCashPayment]
        ADD [StorageBucket] [nvarchar](200) NULL;
END
GO

IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'GCashPayment' AND COLUMN_NAME = 'StoragePath'
)
BEGIN
    ALTER TABLE [dbo].[GCashPayment]
        ADD [StoragePath] [nvarchar](1000) NULL;
END
GO

IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'GCashPayment' AND COLUMN_NAME = 'SubmittedAt'
)
BEGIN
    ALTER TABLE [dbo].[GCashPayment]
        ADD [SubmittedAt] [datetime] NULL;
END
GO

-- ─────────────────────────────────────────────────────────────────────────────
-- 2. BankTransferPayment — add SubmittedAt
-- ─────────────────────────────────────────────────────────────────────────────
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'BankTransferPayment' AND COLUMN_NAME = 'SubmittedAt'
)
BEGIN
    ALTER TABLE [dbo].[BankTransferPayment]
        ADD [SubmittedAt] [datetime] NULL;
END
GO

-- ─────────────────────────────────────────────────────────────────────────────
-- 3. Recreate vw_PaymentDetail to include the new GCash columns
-- ─────────────────────────────────────────────────────────────────────────────
IF OBJECT_ID('dbo.vw_PaymentDetail', 'V') IS NOT NULL
    DROP VIEW [dbo].[vw_PaymentDetail];
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

PRINT 'Patch applied: GCashPayment + BankTransferPayment columns added, vw_PaymentDetail updated.';
GO
