-- Taurus Bike Shop — Schema Patch: Add IsRead tracking to Notification table
-- Apply after Taurus-schema-v8.2.0.sql
-- Idempotent — safe to run multiple times.

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID(N'Notification') AND name = N'IsRead'
)
BEGIN
    ALTER TABLE [Notification] ADD [IsRead] BIT NOT NULL CONSTRAINT [DF_Notification_IsRead] DEFAULT 0;
    PRINT 'Added Notification.IsRead column.';
END
ELSE
    PRINT 'Notification.IsRead column already exists — skipped.';

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID(N'Notification') AND name = N'ReadAt'
)
BEGIN
    ALTER TABLE [Notification] ADD [ReadAt] DATETIME2 NULL;
    PRINT 'Added Notification.ReadAt column.';
END
ELSE
    PRINT 'Notification.ReadAt column already exists — skipped.';

GO
