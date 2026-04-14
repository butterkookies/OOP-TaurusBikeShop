-- Migration: Allow 'InApp' channel and 'VoucherAssigned' NotifType in Notification table.
-- The original CHECK constraints only permit Channel IN ('SMS','Email')
-- and NotifType must include all 16 existing values plus VoucherAssigned.
--
-- IMPORTANT: The previous version of this file incorrectly referenced the v8.2.0
-- schema which only had 3 generic NotifType values ('Push','SMS','Email').
-- The production schema (v8.0 / v8.1) has 16 real notification types.
-- This corrected version preserves ALL existing types and adds VoucherAssigned.

-- Drop and recreate Channel constraint to include 'InApp'
ALTER TABLE [dbo].[Notification] DROP CONSTRAINT [CK_Notif_Channel];
GO
ALTER TABLE [dbo].[Notification] ADD CONSTRAINT [CK_Notif_Channel]
    CHECK ([Channel] IN ('SMS', 'Email', 'InApp'));
GO

-- Drop and recreate NotifType constraint to include 'VoucherAssigned'
-- Preserves all 16 existing types from schema v8.0 + adds VoucherAssigned
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
        'VoucherAssigned'
    ));
GO
