-- Migration 005: Add ActiveSession table for server-side session tracking
-- Supports forceful logout and refresh-token revocation.
-- Run against: Taurus-bike-shop-sqlserver-2026

IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'ActiveSession'
)
BEGIN
    CREATE TABLE [dbo].[ActiveSession] (
        [SessionId]    INT            NOT NULL IDENTITY(1,1),
        [UserId]       INT            NOT NULL,
        [RefreshToken] NVARCHAR(500)  NOT NULL,
        [DeviceInfo]   NVARCHAR(500)  NULL,
        [IpAddress]    NVARCHAR(50)   NULL,
        [IsRevoked]    BIT            NOT NULL CONSTRAINT DF_ActiveSession_IsRevoked DEFAULT 0,
        [ExpiresAt]    DATETIME2      NOT NULL,
        [CreatedAt]    DATETIME2      NOT NULL CONSTRAINT DF_ActiveSession_CreatedAt DEFAULT GETUTCDATE(),
        [RevokedAt]    DATETIME2      NULL,

        CONSTRAINT PK_ActiveSession PRIMARY KEY ([SessionId]),

        CONSTRAINT FK_ActiveSession_User FOREIGN KEY ([UserId])
            REFERENCES [dbo].[User] ([UserId])
            ON DELETE NO ACTION
    );
END
GO

-- Unique index on RefreshToken — one row per token
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID('dbo.ActiveSession') AND name = 'UX_ActiveSession_Token'
)
BEGIN
    CREATE UNIQUE INDEX [UX_ActiveSession_Token]
        ON [dbo].[ActiveSession] ([RefreshToken]);
END
GO

-- Composite index for fast "get active sessions for user" queries (filtered: not revoked)
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID('dbo.ActiveSession') AND name = 'IX_ActiveSession_Active'
)
BEGIN
    CREATE INDEX [IX_ActiveSession_Active]
        ON [dbo].[ActiveSession] ([UserId], [ExpiresAt])
        WHERE [IsRevoked] = 0;
END
GO

-- Index to support FK lookups from UserId
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID('dbo.ActiveSession') AND name = 'IX_ActiveSession_UserId'
)
BEGIN
    CREATE INDEX [IX_ActiveSession_UserId]
        ON [dbo].[ActiveSession] ([UserId]);
END
GO
