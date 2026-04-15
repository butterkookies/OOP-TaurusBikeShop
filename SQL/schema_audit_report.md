# Taurus Bike Shop Database Schema 8.1 - Audit Report

**Prepared By:** Senior Database Architect & Software Auditor  
**Target Schema:** [SQL\Schema\Taurus_schema_8.1.sql](file:///c:/Andrei.dev/Projects/OOP-TaurusBikeShop/SQL/Schema/Taurus_schema_8.1.sql)  
**Database Environment:** SQL Server 2022 (Compatibility Level 160) / .NET 8 WPF + Web  
**Audit Date:** 2026-04-15

---

## Schema Inventory

**Tables (32):** `User`, `Role`, `UserRole`, `Address`, `Order`, `OrderItem`, `OrderStatusAudit`, `Payment`, `GCashPayment`, `BankTransferPayment`, `Delivery`, `LalamoveDelivery`, `LBCDelivery`, `PickupOrder`, `Cart`, `CartItem`, `GuestSession`, `Product`, `ProductVariant`, `ProductImage`, `Category`, `Brand`, `Supplier`, `PurchaseOrder`, `PurchaseOrderItem`, `InventoryLog`, `PriceHistory`, `Notification`, `OTPVerification`, `SupportTicket`, `SupportTask`, `SupportTicketReply`, `SystemLog`, `Voucher`, `VoucherUsage`, `UserVoucher`, `Wishlist`, `Review`, `POS_Session`, `__EFMigrationsHistory`

**Views (10):** `vw_PaymentDetail`, `vw_UserVoucherUsageCount`, `vw_ActiveProducts`, `vw_ProductImageGallery`, `vw_ProductVariantDetails`, `vw_InventoryStatus`, `vw_PendingNotifications`, `vw_OpenSupportTickets`, `vw_DeliveryDetail`, `vw_OrderItemDetail`, `vw_PurchaseOrderDetail`, `vw_OrderSummary`

---

## 1. Critical Issues (Must Fix)

### 1.1 Guest Checkout is Structurally Broken

`[Order].UserId` is defined as `[int] NOT NULL` (Line 549). The `GuestSession` table exists to handle unauthenticated shopping carts, but the moment a guest converts their cart to an order, the schema **forces** a registered `UserId` to be attached. Unless the application silently creates "shadow" `User` records for every guest, a true guest checkout (without requiring account creation) will crash with a NOT NULL / FK constraint violation on INSERT.

**Evidence:**
- `Order.UserId INT NOT NULL` with `FK_Order_User` referencing `User(UserId)`
- `Cart` correctly allows `UserId NULL` via `CK_Cart_Owner` check constraint
- No bridge between `GuestSession` and `Order` exists

**Fix:** Either add `GuestSessionId INT NULL` to `Order` with an exclusive-arc CHECK (`UserId IS NOT NULL OR GuestSessionId IS NOT NULL`), or explicitly document that guest checkout always creates a shadow `User` with `IsWalkIn = 1`.

---

### 1.2 Order Can Be Both Delivery AND Pickup Simultaneously

There is **no structural enforcement** preventing an order from having both a `Delivery` record and a `PickupOrder` record. The `Delivery` table has no unique constraint on `OrderId` either, meaning an order could even have *multiple* deliveries attached.

**Evidence:**
- `Delivery.OrderId INT NOT NULL` with `FK_Delivery_Order` -- no UNIQUE constraint
- `PickupOrder.OrderId INT NOT NULL` with `UX_PickupOrder_Order` UNIQUE -- good, but only prevents duplicate pickups
- Nothing prevents `INSERT INTO Delivery` AND `INSERT INTO PickupOrder` for the same `OrderId`

**Fix:** Add a `FulfillmentType NVARCHAR(20) NOT NULL` column on `Order` with `CHECK (FulfillmentType IN ('Delivery', 'Pickup', 'WalkIn'))`. Then either use triggers or application-level enforcement to validate that only the correct child table is populated. At minimum, add a `UNIQUE` constraint on `Delivery.OrderId`.

---

### 1.3 Missing UNIQUE Constraint on GCash Transaction ID

`GCashPayment.GcashTransactionId` has a non-clustered index (`IX_GCashPayment_TxnId`) but it is **not unique**. This means duplicate GCash transaction IDs can be inserted, creating a **double-spend vulnerability** if the application fails to check under concurrent load.

**Evidence:**
- `IX_GCashPayment_TxnId` is `CREATE NONCLUSTERED INDEX` (not `CREATE UNIQUE NONCLUSTERED INDEX`)
- Compare with `Product.SKU` which correctly uses `CREATE UNIQUE NONCLUSTERED INDEX [UX_Product_SKU]`

**Fix:** Change `IX_GCashPayment_TxnId` to `CREATE UNIQUE NONCLUSTERED INDEX` with the same `WHERE ([GcashTransactionId] IS NOT NULL)` filter.

---

### 1.4 Unsafe Guest Session Token Type

`GuestSession.SessionToken` is `NVARCHAR(100)`. If tokens are generated as sequential strings or short alphanumeric codes, session hijacking becomes trivial via enumeration. The database cannot enforce randomness, but using `UNIQUEIDENTIFIER` would guarantee 128-bit entropy at the type level.

**Evidence:**
- `SessionToken NVARCHAR(100) NOT NULL` with `UX_GuestSession_Token UNIQUE`
- No CHECK constraint on minimum length or format

**Fix:** Change column type to `UNIQUEIDENTIFIER` with `DEFAULT NEWID()`, or add a `CHECK (LEN(SessionToken) >= 32)` constraint to enforce minimum entropy.

---

## 2. Major Design Flaws

### 2.1 User Table Over-Unification (Admin / Customer / Walk-In)

The `User` table unifies Customers, Staff, Admins, and Walk-In POS ghosts into a single entity. Because Walk-In users don't have emails or passwords, both `Email` and `PasswordHash` are nullable.

**Flaw:** Without a CHECK constraint enforcing `IsWalkIn = 1 OR (Email IS NOT NULL AND PasswordHash IS NOT NULL)`, an application bug can easily save a standard online customer without login credentials, silently breaking their account forever.

**Evidence:**
- `Email NVARCHAR(255) NULL`, `PasswordHash NVARCHAR(255) NULL`
- `IsWalkIn BIT NOT NULL`
- No CHECK constraint correlating these fields

**Fix:** Add:
```sql
ALTER TABLE [User] ADD CONSTRAINT CK_User_Credentials
CHECK (IsWalkIn = 1 OR (Email IS NOT NULL AND PasswordHash IS NOT NULL));
```

---

### 2.2 Denormalized Order Totals Without Sync Enforcement

`Order.SubTotal` is physically stored but is technically derivable from `SUM(OrderItem.Quantity * OrderItem.UnitPrice)`. There is no trigger, computed column, or database-level mechanism to keep `Order.SubTotal` synchronized with `OrderItem` changes.

**Evidence:**
- `Order.SubTotal DECIMAL(10,2) NOT NULL` with `CK_Order_SubTotal CHECK (SubTotal >= 0)`
- `OrderItem` has no trigger that recalculates the parent `Order.SubTotal`
- `vw_OrderItemDetail` computes `Quantity * UnitPrice` as `Subtotal` per line -- confirming the data is derivable

**Risk:** An application bug that modifies `OrderItem` without updating `Order.SubTotal` will create silent financial discrepancies. In a POS system handling real money, this is dangerous.

**Fix:** Either add an `AFTER INSERT, UPDATE, DELETE` trigger on `OrderItem` that recalculates `Order.SubTotal`, or remove the stored `SubTotal` entirely and use `vw_OrderSummary` for reads.

---

### 2.3 Loss of Cart-to-Order Lineage

The `Order` table has no `CartId` FK. Once a cart is checked out (`Cart.IsCheckedOut = 1`), there is no auditable FK tying `Order 1234` back to `Cart 5678`.

**Impact:** Destroys abandoned cart conversion analytics, makes checkout debugging impossible, and breaks any future A/B testing on cart flows.

**Fix:** Add `CartId INT NULL` to `Order` with `FK_Order_Cart REFERENCES Cart(CartId)`.

---

### 2.4 No `TotalAmount` or Computed Column on Order

The `Order` table stores `SubTotal`, `DiscountAmount`, and `ShippingFee` separately but has no `TotalAmount` column or computed column. The view `vw_OrderSummary` computes `(SubTotal - DiscountAmount + ShippingFee) AS TotalAmount`, but every direct query on `Order` must manually re-derive this.

**Fix:** Add a persisted computed column:
```sql
ALTER TABLE [Order] ADD TotalAmount AS (SubTotal - DiscountAmount + ShippingFee) PERSISTED;
```

---

## 3. Minor Issues / Improvements

### 3.1 Rigid Hardware Spec Columns on Product

The `Product` table has fixed columns for `WheelSize`, `SpeedCompatibility`, `BrakeType`, `BoostCompatible`, `TubelessReady`, `AxleStandard`, `SuspensionTravel`, `Material`, `Color`. If Taurus begins selling helmets, gloves, or accessories, these columns become dead weight.

**Recommendation:** Migrate bike-specific specs to a JSON column (`AdditionalSpecs NVARCHAR(MAX)` already exists and could be validated with `ISJSON`) or a normalized EAV table `ProductAttribute(ProductId, AttributeKey, AttributeValue)`. Keep the existing `AdditionalSpecs` column but validate it:
```sql
ALTER TABLE Product ADD CONSTRAINT CK_Product_Specs CHECK (AdditionalSpecs IS NULL OR ISJSON(AdditionalSpecs) = 1);
```

### 3.2 OTP Code Stored in Plaintext

`OTPVerification.OTPCode NVARCHAR(10)` stores OTPs in plaintext. A SQL injection attack, rogue admin query, or database backup exposure immediately compromises all active OTP codes.

**Fix:** Hash OTP codes with SHA-256 before storage. Compare by hashing user input and matching against the stored hash.

### 3.3 Address Snapshot Versioning is Cumbersome

Orders reference `ShippingAddressId` which links to `Address`. The `IsSnapshot = 1` flag creates immutable copies, but historical order queries must navigate through the address versioning model. There's no direct denormalization of address fields on the `Order` row.

**Recommendation:** Consider snapshotting key address fields (`Street`, `City`, `PostalCode`, `Province`) directly on `Order` or `Delivery` for historical reliability.

### 3.4 Inconsistent DateTime Types

Most tables use `[datetime]` for timestamps, but `OrderStatusAudit.CreatedAt` and `Notification.ReadAt` use `[datetime2](7)`. This inconsistency will cause subtle precision mismatches in time comparisons across joins.

**Fix:** Standardize all timestamp columns to `datetime2(7)` for microsecond precision and no timezone ambiguity.

### 3.5 `POS_Session.TotalSales` Will Drift

`POS_Session.TotalSales DECIMAL(10,2)` is physically stored but has no mechanism to stay in sync with actual orders processed during that session. There's no FK from `Order` to `POS_Session`, so recalculation isn't even structurally possible.

**Fix:** Add `POSSessionId INT NULL` to `Order` for walk-in orders, then either use a trigger to maintain `TotalSales` or make it a computed column via a scalar function.

### 3.6 Missing `User.Email` Unique Constraint

`User.Email` is nullable (for walk-in users) but has no unique constraint. Two registered customers could theoretically share the same email address.

**Fix:**
```sql
CREATE UNIQUE NONCLUSTERED INDEX UX_User_Email ON [User](Email) WHERE (Email IS NOT NULL AND IsWalkIn = 0);
```

### 3.7 No `ProductVariant.SKU` Unique Constraint

`ProductVariant.SKU` has a non-clustered index (`IX_ProductVariant_SKU`) but it is not unique. Duplicate variant SKUs can exist.

**Fix:** Change to `CREATE UNIQUE NONCLUSTERED INDEX` with `WHERE (SKU IS NOT NULL)`.

---

## 4. Security Risks

### 4.1 Plaintext OTP Codes
As noted in 3.2, `OTPVerification.OTPCode` is stored in cleartext. Easily compromised via SQL injection, rogue admin reads, or backup theft.

### 4.2 No Refresh Token / Session Management Table
For a dual WPF + Web architecture, JWT revocation typically requires a `RefreshToken` or `ActiveSession` table. This is notably absent. Without it, there's no way to forcibly log out a user or detect compromised sessions at the database level.

### 4.3 Cascading Physical DELETEs Destroy Audit Trails
Extensive use of `ON DELETE CASCADE` across:
- `Address` -> cascades from `User` delete
- `CartItem` -> cascades from `Cart` delete
- `OrderItem` -> cascades from `Order` delete
- `Notification` -> cascades from `User` delete
- `GCashPayment/BankTransferPayment` -> cascade from `Payment` delete
- `LalamoveDelivery/LBCDelivery` -> cascade from `Delivery` delete
- `UserRole`, `Wishlist` -> cascade from `User` delete

Deleting a `User` physically destroys their addresses, notifications, roles, and wishlist. Deleting an `Order` destroys all line items. For a financial system, this annihilates audit trails.

**Fix:** Replace physical deletes with soft-delete (`IsDeleted BIT NOT NULL DEFAULT 0`) on `User`, `Order`, `Payment`, and all financially sensitive tables. Remove `ON DELETE CASCADE` from these relationships.

### 4.4 `BankTransferPayment.BpiReferenceNumber` Not Unique
Similar to the GCash issue, `IX_BTP_BpiRef` is a regular non-clustered index, not unique. Duplicate bank reference numbers can be submitted.

---

## 5. Performance Concerns

### 5.1 TPT (Table-Per-Type) Join Overhead

Payment has two subtypes (`GCashPayment`, `BankTransferPayment`). Delivery has two subtypes (`LalamoveDelivery`, `LBCDelivery`). Entity Framework's TPT pattern mandates `LEFT JOIN` chains for every read, as modeled in `vw_PaymentDetail` and `vw_DeliveryDetail`. Under millions of rows, these multi-join queries become I/O bottlenecks on transaction grids and reporting dashboards.

**Mitigation:** Consider materialized/indexed views, or switch to TPH (Table-Per-Hierarchy) with a discriminator column if subtype fields are few.

### 5.2 Massive Non-Clustered Index Count

The schema defines **70+ non-clustered indexes** across all tables. While beneficial for reads, every INSERT/UPDATE/DELETE on heavily-indexed tables like `InventoryLog` (6 indexes), `Notification` (8 indexes), `Product` (7 indexes), and `ProductImage` (5 indexes) incurs significant write amplification.

**Concern:** `InventoryLog` and `SystemLog` are append-heavy audit tables where write performance matters more than ad-hoc query flexibility. Low-selectivity indexes (e.g., `IX_Brand_IsActive` on a boolean, `IX_Cart_IsCheckedOut` on a boolean) provide minimal benefit and should be candidates for removal.

### 5.3 Low-Selectivity Boolean Indexes

Multiple indexes exist on boolean columns:
- `IX_Brand_IsActive`
- `IX_Cart_IsCheckedOut`
- `IX_Category_IsActive`
- `IX_Product_IsActive`
- `IX_Product_IsFeatured`
- `IX_ProductVariant_IsActive`
- `IX_OTP_IsUsed`

A boolean column has exactly 2 distinct values. The query optimizer will almost always prefer a table scan over using these indexes. They waste storage and slow writes for zero read benefit.

**Fix:** Remove standalone boolean indexes. Instead, include boolean columns as filters in composite indexes where they add value (e.g., `IX_Product_Price` already includes `IsActive` -- good).

### 5.4 Redundant Index on `PickupOrder.OrderId`

`PickupOrder` has both a UNIQUE constraint `UX_PickupOrder_Order` and a separate non-clustered index `IX_PickupOrder_OrderId` on the same column. The unique constraint already creates an index internally.

**Fix:** Drop `IX_PickupOrder_OrderId`.

### 5.5 Missing Composite Indexes for Common Queries

- **Order by user and status:** `WHERE UserId = @id AND OrderStatus = @status` lacks a composite index. Both `IX_Order_UserId` and `IX_Order_OrderStatus` exist separately.
- **Notification by user and read status:** No composite index on `(UserId, IsRead)` for "unread notifications" queries.

---

## 6. Scalability Risks

### 6.1 INT Primary Keys on High-Volume Audit Tables

`SystemLog`, `InventoryLog`, `OrderStatusAudit`, `SupportTicketReply`, and `Notification` all use `[int] IDENTITY(1,1)` as their primary key. A signed 32-bit INT maxes out at **2,147,483,647**. For high-velocity logging tables in an e-commerce system, this ceiling can be hit within a few years under production load.

**Fix:** Upgrade these tables' PKs to `BIGINT IDENTITY(1,1)` before production deployment:
```sql
ALTER TABLE SystemLog ALTER COLUMN SystemLogId BIGINT;
ALTER TABLE InventoryLog ALTER COLUMN InventoryLogId BIGINT;
ALTER TABLE OrderStatusAudit ALTER COLUMN AuditId BIGINT;
ALTER TABLE Notification ALTER COLUMN NotificationId BIGINT;
```

### 6.2 No Table Partitioning Strategy

`SystemLog`, `InventoryLog`, and `OrderStatusAudit` grow indefinitely with no archival or partitioning boundaries. As these tables swell into tens of millions of rows:
- Full table scans for reporting become prohibitively slow
- Index maintenance windows grow
- Backup sizes explode

**Fix:** Implement date-based table partitioning on `CreatedAt` with monthly or quarterly boundaries. Create an archival strategy for records older than N months.

### 6.3 No Pagination-Friendly Design on Audit Tables

`OrderStatusAudit` uses `datetime2(7)` for `CreatedAt` but the index `IX_OrderStatusAudit_OrderId` sorts by `(OrderId ASC, CreatedAt DESC)`. For global audit log pagination (not scoped to a single order), there's no efficient keyset pagination path.

---

## 7. Mismatch Between Schema and Real System

### 7.1 Walk-In POS Orders Skip Delivery Without Enforcement

A POS walk-in transaction (`Order.IsWalkIn = 1`) should immediately complete without delivery tracking. However, there's no CHECK constraint or trigger preventing a `Delivery` record from being created for a walk-in order.

**Fix:**
```sql
-- Add trigger or CHECK on Delivery:
-- Prevent delivery records for walk-in orders
CREATE TRIGGER TR_Delivery_NoWalkIn ON Delivery
AFTER INSERT AS
BEGIN
    IF EXISTS (SELECT 1 FROM inserted i JOIN [Order] o ON i.OrderId = o.OrderId WHERE o.IsWalkIn = 1)
    BEGIN
        RAISERROR('Cannot create delivery for walk-in orders', 16, 1);
        ROLLBACK;
    END
END;
```

### 7.2 POS_Session Has No Link to Orders

`POS_Session` tracks cashier shifts with a `TotalSales` column, but there's no FK from `Order` to `POS_Session`. The system cannot structurally determine which orders belong to which POS session. `TotalSales` is guaranteed to drift out of sync because there's no mechanism (trigger, FK, or computed column) to derive it from actual order data.

### 7.3 No Delivery-Method Column on Order

The UI presumably lets customers choose between delivery and pickup, but the `Order` table has no `DeliveryMethod` or `FulfillmentType` column. The only way to determine fulfillment type is to check for the existence of child records in `Delivery` or `PickupOrder`, which is fragile and race-condition-prone.

### 7.4 `Review.OrderId` Required But Walk-In Reviews Unclear

`Review.OrderId INT NOT NULL` forces every review to be tied to an order. Walk-in POS customers (who may not have a persistent user account) cannot leave reviews unless shadow user accounts are created. This may or may not match the intended system behavior.

### 7.5 No Refund / Return Table

The schema supports `SupportTicket` with categories like `ReturnRefund` and `SupportTask` with types like `ArrangeReturn` and `ShipReplacement`, but there is no `Refund` or `Return` table to track the actual financial reversal. Refund amounts, dates, approval states, and payment method reversals are not modeled.

---

## 8. Recommended Refactored Design (Priority Actions)

### Immediate (Before Production)

| # | Action | Severity |
|---|--------|----------|
| 1 | Add `CK_User_Credentials` CHECK constraint | Critical |
| 2 | Add `UNIQUE` to `IX_GCashPayment_TxnId` | Critical |
| 3 | Add `FulfillmentType` column to `Order` with CHECK | Critical |
| 4 | Add `GuestSessionId NULL` to `Order` OR document shadow-user pattern | Critical |
| 5 | Upgrade audit table PKs to `BIGINT` | Critical |
| 6 | Add `UX_User_Email` filtered unique index | Major |
| 7 | Hash OTP codes before storage | Major |
| 8 | Add `CartId` FK to `Order` | Major |
| 9 | Add `POSSessionId` FK to `Order` | Major |
| 10 | Make `BpiReferenceNumber` unique (filtered) | Major |

### Short-Term (Next Sprint)

| # | Action | Severity |
|---|--------|----------|
| 11 | Replace `ON DELETE CASCADE` with soft-delete on financial tables | Major |
| 12 | Add `Refund` table | Major |
| 13 | Standardize all datetime columns to `datetime2(7)` | Minor |
| 14 | Remove standalone boolean indexes | Minor |
| 15 | Remove redundant `IX_PickupOrder_OrderId` | Minor |
| 16 | Add composite indexes for common query patterns | Minor |
| 17 | Add `TotalAmount` computed column on `Order` | Minor |

### Long-Term (Before Scale)

| # | Action | Severity |
|---|--------|----------|
| 18 | Implement date-based partitioning on log tables | Scalability |
| 19 | Add `RefreshToken` / session management table | Security |
| 20 | Evaluate TPT -> TPH migration for Payment/Delivery subtypes | Performance |
| 21 | Add `CHECK (ISJSON(AdditionalSpecs) = 1)` on Product | Minor |
| 22 | Create archival strategy for SystemLog/InventoryLog | Scalability |

---

### End of Audit

*Action required: Address Critical Issues (items 1-5) immediately in the next EF Core migration. Prioritize upgrading log primary keys to BIGINT and adding unique constraints on financial reference IDs before deployment to production.*
