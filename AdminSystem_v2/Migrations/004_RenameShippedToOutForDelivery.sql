-- Migration 004: Rename 'Shipped' status to 'OutForDelivery'
-- Reflects that courier (Lalamove/LBC) has picked up the order and it is out for delivery.

-- Step 1: Drop the existing check constraint
ALTER TABLE [dbo].[Order] DROP CONSTRAINT [CK_Order_Status];
GO

-- Step 2: Update existing rows from 'Shipped' to 'OutForDelivery'
UPDATE [dbo].[Order]
SET    OrderStatus = 'OutForDelivery'
WHERE  OrderStatus = 'Shipped';
GO

-- Step 3: Re-create the check constraint with 'OutForDelivery' replacing 'Shipped'
ALTER TABLE [dbo].[Order] WITH CHECK ADD CONSTRAINT [CK_Order_Status] CHECK (
    [OrderStatus] IN (
        'Pending',
        'PendingVerification',
        'OnHold',
        'Processing',
        'ReadyForPickup',
        'PickedUp',
        'OutForDelivery',
        'Delivered',
        'Cancelled'
    )
);
GO

ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [CK_Order_Status];
GO
