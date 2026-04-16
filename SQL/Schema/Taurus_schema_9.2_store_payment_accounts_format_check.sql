-- =============================================================================
-- Taurus Bike Shop - Schema patch 9.2
-- Enforces account-number format on dbo.StorePaymentAccount:
--   GCash        : 11 digits, must start with '09' (e.g. 09123456789)
--   BankTransfer : exactly 10 digits, numbers only (BPI savings/deposit)
--
-- Safe to run on top of 9.1. Idempotent: re-running this patch is a no-op.
-- =============================================================================

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- -----------------------------------------------------------------------------
-- 1. Normalize any legacy rows whose AccountNumber violates the new CHECK.
--    9.1 shipped placeholder values with dashes ('09XX-XXX-XXXX',
--    '0000-0000-0000') that fail the new regex-style pattern.
--    Non-conforming rows are reset to a safe placeholder; admin is expected
--    to edit the row via the Payment Accounts screen immediately after.
-- -----------------------------------------------------------------------------
UPDATE dbo.StorePaymentAccount
   SET AccountNumber = N'09000000000'
 WHERE PaymentMethod = N'GCash'
   AND (AccountNumber LIKE N'%[^0-9]%'
        OR LEN(AccountNumber) <> 11
        OR AccountNumber NOT LIKE N'09%');
GO

UPDATE dbo.StorePaymentAccount
   SET AccountNumber = N'0000000000'
 WHERE PaymentMethod = N'BankTransfer'
   AND (AccountNumber LIKE N'%[^0-9]%'
        OR LEN(AccountNumber) <> 10);
GO

-- -----------------------------------------------------------------------------
-- 2. Add CK_StorePaymentAccount_AccountNumber if not already present.
-- -----------------------------------------------------------------------------
IF NOT EXISTS
(
    SELECT 1
      FROM sys.check_constraints
     WHERE name = 'CK_StorePaymentAccount_AccountNumber'
       AND parent_object_id = OBJECT_ID('dbo.StorePaymentAccount')
)
BEGIN
    ALTER TABLE dbo.StorePaymentAccount
      ADD CONSTRAINT CK_StorePaymentAccount_AccountNumber CHECK
      (
          (PaymentMethod = N'GCash'
              AND LEN(AccountNumber) = 11
              AND AccountNumber LIKE N'09[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
          OR
          (PaymentMethod = N'BankTransfer'
              AND LEN(AccountNumber) = 10
              AND AccountNumber NOT LIKE N'%[^0-9]%')
      );
END;
GO
