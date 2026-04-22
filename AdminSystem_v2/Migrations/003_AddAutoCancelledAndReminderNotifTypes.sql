-- Migration 003: Add 'OrderAutoCancelled' and 'PendingOrderReminder' to CK_Notif_Type.
-- Fixes a SqlException thrown by PendingOrderMonitorJob when it tries to queue
-- an OrderAutoCancelled or PendingOrderReminder notification — both types were
-- present in the application's NotifTypes constants but missing from the DB constraint.
--
-- This migration builds on migration 002 (which added VoucherAssigned / InApp).
-- It preserves all 18 existing types and adds the 2 missing ones.

ALTER TABLE [dbo].[Notification] DROP CONSTRAINT [CK_Notif_Type];
GO

ALTER TABLE [dbo].[Notification] ADD CONSTRAINT [CK_Notif_Type]
    CHECK ([NotifType] IN (
        'WelcomeEmail',
        'OrderConfirmation',
        'PaymentReceived',
        'PaymentRejected',
        'PaymentHeld',
        'TrackingUpdate',
        'ReadyForPickup',
        'PickupExpiry',
        'DeliveryDelay',
        'DeliveryConfirmation',
        'WishlistRestock',
        'SupportTicketCreated',
        'SupportTicketReply',
        'SupportTicketResolved',
        'LowStockAlert',
        'PendingOrderAlert',
        'OTPCode',
        'VoucherAssigned',
        'PendingOrderReminder',
        'OrderAutoCancelled'
    ));
GO
