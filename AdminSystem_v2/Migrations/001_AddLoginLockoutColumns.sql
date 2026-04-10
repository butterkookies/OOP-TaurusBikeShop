-- Migration: Add brute-force protection columns to [User] table (VULN-003)
-- Run this against your Taurus Bike Shop database.

IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'User' AND COLUMN_NAME = 'FailedLoginAttempts'
)
BEGIN
    ALTER TABLE [dbo].[User]
        ADD [FailedLoginAttempts] INT NOT NULL CONSTRAINT DF_User_FailedLoginAttempts DEFAULT 0;
END
GO

IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'User' AND COLUMN_NAME = 'LockoutUntil'
)
BEGIN
    ALTER TABLE [dbo].[User]
        ADD [LockoutUntil] DATETIME NULL;
END
GO
