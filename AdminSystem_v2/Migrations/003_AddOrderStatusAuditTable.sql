-- Migration 003: Add OrderStatusAudit table
-- Tracks every status transition attempt (successful or rejected) for accountability.

IF OBJECT_ID('OrderStatusAudit', 'U') IS NULL
BEGIN
    CREATE TABLE OrderStatusAudit (
        AuditId      INT            IDENTITY(1,1) PRIMARY KEY,
        OrderId      INT            NOT NULL,
        FromStatus   NVARCHAR(50)   NOT NULL,
        ToStatus     NVARCHAR(50)   NOT NULL,
        Success      BIT            NOT NULL DEFAULT 1,
        Reason       NVARCHAR(500)  NULL,
        CreatedAt    DATETIME2      NOT NULL DEFAULT GETUTCDATE(),

        CONSTRAINT FK_OrderStatusAudit_Order
            FOREIGN KEY (OrderId) REFERENCES [Order](OrderId)
    );

    CREATE NONCLUSTERED INDEX IX_OrderStatusAudit_OrderId
        ON OrderStatusAudit (OrderId, CreatedAt DESC);
END
GO
