-- =============================================================================
-- Taurus Bike Shop - Schema patch 9.1
-- Adds StorePaymentAccount table (store-side GCash / BPI account directory)
-- and snapshot columns on Payment so every order remembers which account
-- the customer was asked to pay to.
-- =============================================================================

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- -----------------------------------------------------------------------------
-- 1. StorePaymentAccount
-- -----------------------------------------------------------------------------
IF OBJECT_ID('dbo.StorePaymentAccount', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.StorePaymentAccount
    (
        StorePaymentAccountId INT           IDENTITY(1,1) NOT NULL,
        PaymentMethod         NVARCHAR(50)  NOT NULL,
        AccountName           NVARCHAR(150) NOT NULL,
        AccountNumber         NVARCHAR(50)  NOT NULL,
        BankName              NVARCHAR(100) NULL,
        QrImageUrl            NVARCHAR(1000) NULL,
        Instructions          NVARCHAR(500) NULL,
        IsActive              BIT           NOT NULL CONSTRAINT DF_StorePaymentAccount_IsActive      DEFAULT(1),
        DisplayOrder          INT           NOT NULL CONSTRAINT DF_StorePaymentAccount_DisplayOrder  DEFAULT(0),
        CreatedAt             DATETIME2(7)  NOT NULL CONSTRAINT DF_StorePaymentAccount_CreatedAt     DEFAULT(SYSUTCDATETIME()),
        UpdatedAt             DATETIME2(7)  NOT NULL CONSTRAINT DF_StorePaymentAccount_UpdatedAt     DEFAULT(SYSUTCDATETIME()),
        CONSTRAINT PK_StorePaymentAccount PRIMARY KEY CLUSTERED (StorePaymentAccountId ASC),
        CONSTRAINT CK_StorePaymentAccount_Method CHECK (PaymentMethod IN (N'GCash', N'BankTransfer')),
        -- GCash: 11-digit mobile number starting with 09 (e.g. 09123456789).
        -- BPI  : exactly 10 digits, numbers only.
        CONSTRAINT CK_StorePaymentAccount_AccountNumber CHECK
        (
            (PaymentMethod = N'GCash'
                AND LEN(AccountNumber) = 11
                AND AccountNumber LIKE N'09[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
            OR
            (PaymentMethod = N'BankTransfer'
                AND LEN(AccountNumber) = 10
                AND AccountNumber NOT LIKE N'%[^0-9]%')
        )
    );

    -- At most one active account per payment method at any time.
    CREATE UNIQUE INDEX UX_StorePaymentAccount_ActivePerMethod
        ON dbo.StorePaymentAccount (PaymentMethod)
        WHERE IsActive = 1;

    CREATE INDEX IX_StorePaymentAccount_Method
        ON dbo.StorePaymentAccount (PaymentMethod, DisplayOrder);
END;
GO

-- -----------------------------------------------------------------------------
-- 2. Payment snapshot columns (which store account the customer paid to)
-- -----------------------------------------------------------------------------
IF COL_LENGTH('dbo.Payment', 'PaidToAccountName') IS NULL
    ALTER TABLE dbo.Payment ADD PaidToAccountName   NVARCHAR(150) NULL;
GO
IF COL_LENGTH('dbo.Payment', 'PaidToAccountNumber') IS NULL
    ALTER TABLE dbo.Payment ADD PaidToAccountNumber NVARCHAR(50)  NULL;
GO
IF COL_LENGTH('dbo.Payment', 'PaidToBankName') IS NULL
    ALTER TABLE dbo.Payment ADD PaidToBankName      NVARCHAR(100) NULL;
GO

-- -----------------------------------------------------------------------------
-- 3. Seed one placeholder row per method so the customer page is never empty.
--    Admin is expected to edit these immediately after running this patch.
-- -----------------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.StorePaymentAccount WHERE PaymentMethod = N'GCash')
BEGIN
    INSERT INTO dbo.StorePaymentAccount
        (PaymentMethod, AccountName, AccountNumber, BankName, Instructions, IsActive, DisplayOrder)
    VALUES
        (N'GCash', N'TAURUS BIKE SHOP', N'09000000000', NULL,
         N'Send exact amount. Use your order number as the message. (Placeholder number — replace in Admin.)', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM dbo.StorePaymentAccount WHERE PaymentMethod = N'BankTransfer')
BEGIN
    INSERT INTO dbo.StorePaymentAccount
        (PaymentMethod, AccountName, AccountNumber, BankName, Instructions, IsActive, DisplayOrder)
    VALUES
        (N'BankTransfer', N'TAURUS BIKE SHOP', N'0000000000', N'BPI',
         N'Use your order number as the deposit note. Verification within 24 hours. (Placeholder number — replace in Admin.)', 1, 0);
END;
GO
