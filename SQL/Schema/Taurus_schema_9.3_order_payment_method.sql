-- =============================================================================
-- Taurus Bike Shop - Schema patch 9.3
-- Adds dbo.[Order].PaymentMethod so the customer's checkout choice is
-- persisted with the order itself. The payment upload page reads this column
-- to render the correct instructions and resolve the correct
-- StorePaymentAccount BEFORE any Payment row exists.
--
-- Previously the chosen method lived only in CheckoutViewModel (form-only) and
-- was lost at order creation, causing the payment page to blindly default to
-- Bank Transfer and fail the store-account lookup.
--
-- Idempotent. Safe to re-run.
-- =============================================================================

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- -----------------------------------------------------------------------------
-- 1. Add the column (nullable first so the ALTER succeeds on tables with rows).
-- -----------------------------------------------------------------------------
IF COL_LENGTH('dbo.[Order]', 'PaymentMethod') IS NULL
BEGIN
    ALTER TABLE dbo.[Order] ADD PaymentMethod NVARCHAR(50) NULL;
END;
GO

-- -----------------------------------------------------------------------------
-- 2. Backfill existing rows.
--    Priority: most recent non-failed Payment's method → Cash for walk-in →
--    GCash as a safe default for pre-v9.3 online orders.
-- -----------------------------------------------------------------------------
UPDATE o
   SET PaymentMethod = COALESCE
       (
           (
               SELECT TOP 1 p.PaymentMethod
               FROM   dbo.Payment p
               WHERE  p.OrderId = o.OrderId
                 AND  p.PaymentStatus <> N'Failed'
               ORDER  BY p.PaymentDate DESC, p.PaymentId DESC
           ),
           CASE WHEN o.IsWalkIn = 1 THEN N'Cash' ELSE N'GCash' END
       )
  FROM dbo.[Order] o
 WHERE o.PaymentMethod IS NULL OR o.PaymentMethod = N'';
GO

-- -----------------------------------------------------------------------------
-- 3. Lock the column down: NOT NULL + CHECK constraint.
-- -----------------------------------------------------------------------------
IF EXISTS
(
    SELECT 1 FROM sys.columns
    WHERE  object_id = OBJECT_ID('dbo.[Order]')
      AND  name      = 'PaymentMethod'
      AND  is_nullable = 1
)
BEGIN
    ALTER TABLE dbo.[Order]
      ALTER COLUMN PaymentMethod NVARCHAR(50) NOT NULL;
END;
GO

IF NOT EXISTS
(
    SELECT 1
      FROM sys.check_constraints
     WHERE name = 'CK_Order_PaymentMethod'
       AND parent_object_id = OBJECT_ID('dbo.[Order]')
)
BEGIN
    ALTER TABLE dbo.[Order]
      ADD CONSTRAINT CK_Order_PaymentMethod CHECK
          (PaymentMethod IN (N'GCash', N'BankTransfer', N'Cash'));
END;
GO
