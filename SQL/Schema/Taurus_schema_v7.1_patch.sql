-- =============================================================================
-- Taurus Bike Shop  |  TaurusBikeShopDB
-- File    : Taurus_schema_v71_patch.sql
-- Purpose : Upgrade schema from v7.0 → v7.1
-- Platform: Google Cloud SQL — SQL Server (Cloud SQL for SQL Server)
--
-- Changes:
--   1. ALTER TABLE ProductVariant
--        ADD ReorderThreshold INT NOT NULL DEFAULT 5
--      Enables Part 12 (stock update low-stock check) and
--      Part 14 Job 4 (stock level monitor) to compare StockQuantity
--      against a stored threshold rather than a hardcoded value.
--
--   2. CREATE TABLE SupportTask
--      Covers Part 13 "Create Follow-up Task" branch
--      (e.g., "Ship replacement", "Arrange return pickup").
--      Distinct from SupportTicketReply — this is an actionable
--      work item with its own assignee and lifecycle.
-- =============================================================================

SET NOCOUNT ON;
GO

-- =============================================================================
-- CHANGE 1: Add ReorderThreshold to ProductVariant
-- =============================================================================
ALTER TABLE ProductVariant
    ADD ReorderThreshold INT NOT NULL DEFAULT 5;
GO

ALTER TABLE ProductVariant
    ADD CONSTRAINT CK_ProductVariant_ReorderThreshold
    CHECK (ReorderThreshold >= 0);
GO

-- =============================================================================
-- CHANGE 2: Create SupportTask table
-- Covers: Part 13 — "Create Follow-up Task" action branch
-- TaskType values:
--   ShipReplacement   — ship a replacement unit to the customer
--   ArrangeReturn     — coordinate a product return pickup
--   IssueRefund       — process a manual refund
--   ContactSupplier   — escalate to supplier
--   Other             — any other operational task
-- TaskStatus lifecycle: Pending → InProgress → Done | Cancelled
-- =============================================================================
CREATE TABLE SupportTask (
    TaskId           INT           NOT NULL IDENTITY(1,1),
    TicketId         INT           NOT NULL,
    AssignedToUserId INT           NULL,
    TaskType         NVARCHAR(50)  NOT NULL,
    TaskStatus       NVARCHAR(20)  NOT NULL DEFAULT 'Pending',
    DueDate          DATETIME      NULL,
    Notes            NVARCHAR(500) NULL,
    CreatedAt        DATETIME      NOT NULL DEFAULT GETDATE(),
    CompletedAt      DATETIME      NULL,
    CONSTRAINT PK_SupportTask            PRIMARY KEY (TaskId),
    CONSTRAINT FK_SupportTask_Ticket     FOREIGN KEY (TicketId)         REFERENCES SupportTicket(TicketId) ON DELETE CASCADE,
    CONSTRAINT FK_SupportTask_AssignedTo FOREIGN KEY (AssignedToUserId) REFERENCES [User](UserId),
    CONSTRAINT CK_SupportTask_Type       CHECK (TaskType IN (
        'ShipReplacement',
        'ArrangeReturn',
        'IssueRefund',
        'ContactSupplier',
        'Other'
    )),
    CONSTRAINT CK_SupportTask_Status     CHECK (TaskStatus IN (
        'Pending',
        'InProgress',
        'Done',
        'Cancelled'
    ))
);
GO

CREATE INDEX IX_SupportTask_TicketId         ON SupportTask(TicketId);
CREATE INDEX IX_SupportTask_AssignedToUserId ON SupportTask(AssignedToUserId) WHERE AssignedToUserId IS NOT NULL;
CREATE INDEX IX_SupportTask_TaskStatus       ON SupportTask(TaskStatus);
CREATE INDEX IX_SupportTask_DueDate          ON SupportTask(DueDate) WHERE DueDate IS NOT NULL;
GO

-- =============================================================================
PRINT '==============================================';
PRINT 'Taurus_schema_v71_patch.sql — Applied OK';
PRINT '==============================================';
PRINT 'v7.0 → v7.1 changes:';
PRINT '  1. ProductVariant.ReorderThreshold INT DEFAULT 5 added';
PRINT '     + CK_ProductVariant_ReorderThreshold CHECK >= 0';
PRINT '  2. SupportTask table created (38 total tables)';
PRINT '     TaskType : ShipReplacement | ArrangeReturn |';
PRINT '                IssueRefund | ContactSupplier | Other';
PRINT '     TaskStatus: Pending | InProgress | Done | Cancelled';
PRINT '==============================================';
GO
