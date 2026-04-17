# Taurus Bike Shop — Database Design Report
**Database:** `Taurus-bike-shop-sqlserver-2026`  
**Platform:** Microsoft SQL Server  
**Report Date:** April 15, 2026  
**Prepared by:** Senior Database Architect Review  

---

## TABLE OF CONTENTS

1. [Schema Overview](#1-schema-overview)
2. [Entity-Relationship Analysis (Textual)](#2-entity-relationship-analysis-textual)
3. [Normalization Audit](#3-normalization-audit)
4. [Keys & Constraints Review](#4-keys--constraints-review)
5. [Indexing Strategy](#5-indexing-strategy)
6. [Design Problems & Risks](#6-design-problems--risks)
7. [Improved Database Design (Refactored Schema)](#7-improved-database-design-refactored-schema)
8. [Best Practices Checklist](#8-best-practices-checklist)

---

## 1. SCHEMA OVERVIEW

### System Purpose

This is the backend relational database for **Taurus Bike Shop**, a Philippine-based bicycle parts and accessories retailer operating both a physical walk-in store (POS) and an online e-commerce platform. The system handles:

- **Product Catalog Management** — bicycles, parts, and accessories with variants, brands, categories, and images
- **Customer Order Lifecycle** — from cart to delivery or pickup, including payment verification
- **Multi-Channel Payments** — Cash (POS), GCash (digital wallet), and BPI Bank Transfer with proof verification
- **Inventory Management** — stock tracking, reorder thresholds, purchase orders from suppliers
- **Customer Accounts & Roles** — authentication, role-based access (Customer, Staff, Cashier, Manager, Admin), walk-in guest support
- **Voucher & Promotions System** — percentage and fixed discount vouchers with per-user usage caps
- **Support & After-Sales** — ticketing system, task assignments, and threaded replies
- **Audit & Observability** — system event logs, order status audit trail, inventory change logs

### Main Entities (39 Tables + 1 EF Migrations Table)

| Domain | Tables |
|---|---|
| Identity & Access | User, Role, UserRole, Address, OTPVerification, GuestSession, POS_Session |
| Product Catalog | Product, ProductVariant, Category, Brand, ProductImage, PriceHistory |
| Shopping | Cart, CartItem, Wishlist |
| Orders | Order, OrderItem, OrderStatusAudit |
| Fulfillment | Delivery, LBCDelivery, LalamoveDelivery, PickupOrder |
| Payments | Payment, GCashPayment, BankTransferPayment |
| Promotions | Voucher, UserVoucher, VoucherUsage |
| Procurement | Supplier, PurchaseOrder, PurchaseOrderItem |
| Inventory | InventoryLog |
| Support | SupportTicket, SupportTicketReply, SupportTask |
| Observability | Notification, SystemLog |
| Infrastructure | __EFMigrationsHistory |

---

## 2. ENTITY-RELATIONSHIP ANALYSIS (TEXTUAL)

### 2.1 All Entities and Their Roles

| Entity | Role |
|---|---|
| **User** | Central identity entity. Represents both registered customers and staff. Walk-in guests use `IsWalkIn=1`. |
| **Role** | Lookup table for RBAC: Customer, Staff, Cashier, Manager, Admin. |
| **UserRole** | M:M junction between User and Role. |
| **Address** | Shipping/billing addresses per user. Supports snapshot copies for historical order records. |
| **GuestSession** | Temporary session for unauthenticated shoppers. Can be converted to a User. |
| **OTPVerification** | Stores email OTP codes for authentication flows. Standalone, linked by email string only. |
| **POS_Session** | Records cashier shift sessions at a physical terminal. |
| **Category** | Hierarchical product categories (self-referential via `ParentCategoryId`). |
| **Brand** | Product brands/manufacturers. |
| **Product** | Core product entity. Contains bike-specific flat attributes (denormalized). |
| **ProductVariant** | Size/color variants of a product. All stock lives here. |
| **ProductImage** | GCS-hosted images for a product, with primary flag and display order. |
| **PriceHistory** | Audit trail for product price changes. |
| **Cart** | Belongs to a User or GuestSession. One active cart at a time (implied). |
| **CartItem** | Products added to a cart, with price snapshot at time of add. |
| **Wishlist** | User's saved/favorited products. M:M junction with UQ enforcement. |
| **Order** | The central transactional entity. Links user, shipping address, financial summary fields. |
| **OrderItem** | Line items within an order (product + variant + quantity + captured unit price). |
| **OrderStatusAudit** | Immutable append-only log of every order status transition. |
| **Payment** | Base payment record per order. One order can have multiple payments (Upfront + Confirmation stages). |
| **GCashPayment** | GCash-specific payment detail (extends Payment via shared PK — Table-Per-Type inheritance). |
| **BankTransferPayment** | Bank transfer detail with verification workflow (extends Payment). |
| **Delivery** | Base delivery record per order. Supports two courier types via TPT. |
| **LBCDelivery** | LBC-specific extension with tracking number. |
| **LalamoveDelivery** | Lalamove-specific extension with driver info. |
| **PickupOrder** | In-store pickup record. One-to-one with Order. |
| **Supplier** | Vendor/supplier of bicycle inventory. |
| **PurchaseOrder** | Restocking order placed with a supplier. |
| **PurchaseOrderItem** | Line items in a purchase order. |
| **InventoryLog** | Immutable audit trail of all stock quantity changes. |
| **Voucher** | Discount voucher definition with usage limits. |
| **UserVoucher** | Assignment of a voucher to a specific user. |
| **VoucherUsage** | Records each time a user redeems a voucher on an order. |
| **SupportTicket** | Customer or admin-initiated support case. |
| **SupportTicketReply** | Threaded replies on a support ticket. |
| **SupportTask** | Internal staff task spawned from a support ticket. |
| **Notification** | Outbound notification queue (Email/SMS/InApp) with retry tracking. |
| **SystemLog** | Application-level event log (login, payment events, inventory events, etc.). |
| **Review** | Product review tied to a verified purchase. |

### 2.2 Relationships and Cardinalities

```
User (1) ────── (M) Address                   [User has many addresses]
User (1) ────── (M) UserRole ──── (M:M) ─── Role
User (1) ────── (0..1) Cart                   [One active cart per user]
User (1) ────── (M) Wishlist                  [User wishes for many products]
User (1) ────── (M) Order
User (1) ────── (M) Review
User (1) ────── (M) Notification
User (1) ────── (M) SupportTicket             [Submitted by user]
User (1) ────── (M) SupportTicket             [Assigned to staff user]
User (1) ────── (M) SupportTicketReply
User (1) ────── (M) UserVoucher ──── (M:M) ─── Voucher
User (1) ────── (M) VoucherUsage
User (1) ────── (M) POS_Session
User (1) ────── (M) InventoryLog              [ChangedByUserId]
User (1) ────── (M) PriceHistory              [ChangedByUserId]
User (1) ────── (M) PurchaseOrder             [CreatedByUserId]
User (1) ────── (M) BankTransferPayment       [VerifiedByUserId]
User (1) ────── (M) SystemLog
User (1) ────── (M) ProductImage              [UploadedByUserId]

GuestSession (1) ─── (0..1) Cart

Category (1) ── (0..1) Category               [Self-ref: ParentCategoryId]
Category (1) ── (M) Product

Brand (1) ────── (M) Product

Product (1) ── (M) ProductVariant
Product (1) ── (M) ProductImage
Product (1) ── (M) OrderItem
Product (1) ── (M) CartItem
Product (1) ── (M) Wishlist
Product (1) ── (M) Review
Product (1) ── (M) PurchaseOrderItem
Product (1) ── (M) InventoryLog
Product (1) ── (M) PriceHistory

ProductVariant (1) ── (M) OrderItem
ProductVariant (1) ── (M) CartItem
ProductVariant (1) ── (M) PurchaseOrderItem
ProductVariant (1) ── (M) InventoryLog

Order (1) ────── (M) OrderItem
Order (1) ────── (M) Payment
Order (1) ────── (0..1) Delivery              [If shipped]
Order (1) ────── (0..1) PickupOrder           [If in-store pickup]
Order (1) ────── (M) VoucherUsage
Order (1) ────── (M) SupportTicket
Order (1) ────── (M) Notification
Order (1) ────── (M) InventoryLog
Order (1) ────── (M) OrderStatusAudit
Order (1) ────── (M) Review
Address (1) ──── (M) Order                   [ShippingAddressId]

Payment (1) ────── (0..1) GCashPayment        [TPT inheritance]
Payment (1) ────── (0..1) BankTransferPayment [TPT inheritance]

Delivery (1) ── (0..1) LBCDelivery           [TPT inheritance]
Delivery (1) ── (0..1) LalamoveDelivery       [TPT inheritance]

Supplier (1) ── (M) PurchaseOrder
PurchaseOrder (1) ── (M) PurchaseOrderItem
PurchaseOrderItem → Product, ProductVariant

Voucher (1) ── (M) UserVoucher
Voucher (1) ── (M) VoucherUsage

SupportTicket (1) ── (M) SupportTicketReply
SupportTicket (1) ── (M) SupportTask
```

### 2.3 Missing or Implied Relationships

| Issue | Description |
|---|---|
| **OTPVerification ↔ User** | Linked only by `Email` string, not `UserId` FK. Cannot reliably JOIN if email changes. |
| **GuestSession → Order** | A guest session can place orders, but there is no `GuestSessionId` on `Order`. The lineage is broken after conversion. |
| **POS_Session → Order** | No link between a POS shift session and the orders placed during that session. Cannot report sales per session accurately from data alone. |
| **Cart → Order** | No `CartId` on `Order`. Once an order is placed from a cart, the cart-to-order traceability is lost. |
| **Supplier ↔ Product** | No `Supplier` to `Product` relationship. Cannot query "which supplier provides this product?" |
| **Review → OrderItem** | Reviews link to `OrderId` but not to the specific `OrderItemId`. If an order has multiple products, it is unclear which product the review belongs to (Review does have ProductId, but the join via OrderItem is implicit). |

### 2.4 Orphaned or Weakly Connected Tables

| Table | Concern |
|---|---|
| **OTPVerification** | No FK to User. Fully isolated — cleanup of expired OTPs requires scanning the whole table. |
| **POS_Session** | No FK to Order. TotalSales is a stored aggregate with no referential backing. |
| **SystemLog** | No `EntityId`/`EntityType` structured fields. Cannot efficiently query logs for a specific order or product. |
| **__EFMigrationsHistory** | EF Core infrastructure table — should be excluded from application schema documentation. |

---

## 3. NORMALIZATION AUDIT

### 3.1 Per-Table Analysis

#### `User`
- **Current NF:** 2NF (all non-key attributes fully depend on `UserId`)
- **Issues:**
  - `Email NULL` and `PasswordHash NULL` — for non-walk-in users, these should be `NOT NULL`. The nullable design conflates two different entity subtypes (registered users vs. walk-ins) into one table.
  - `DefaultAddressId` creates a **circular FK dependency** with `Address.UserId`. Insertion order is complex: a user must be created before an address, but the address must exist before `DefaultAddressId` can be set.
  - `FailedLoginAttempts` and `LockoutUntil` are transient session-state fields that arguably belong in a separate `UserSecurityState` table for isolation and performance.
- **Recommendation:** Add a `CHECK` or partial index for `Email NOT NULL WHERE IsWalkIn = 0`.

#### `Product`
- **Current NF:** 2NF, **violates 3NF**
- **Issues:**
  - `WheelSize`, `SpeedCompatibility`, `BoostCompatible`, `TubelessReady`, `AxleStandard`, `SuspensionTravel`, `BrakeType`, `Material`, `Color` are **bicycle-specific technical attributes** stored as flat columns. These transitively depend on the *product category/type*, not on `ProductId` alone.
  - `AdditionalSpecs nvarchar(1000)` is a catch-all free text field masking further unnormalized data.
  - `Currency CHAR(3) NOT NULL` is stored per-product. If the shop changes currency policy, every product row must be updated (update anomaly).
  - `SKU` has no `UNIQUE` constraint — duplicate SKUs are possible.
  - `Color` is stored as a flat string rather than normalized — prevents multi-color products or consistent filtering.
- **Recommendation:** Extract bike-specific specs to a `ProductAttribute (ProductId, AttributeName, AttributeValue)` EAV table, or create a `BikeSpec` typed subtable. Move `Currency` to a system configuration table.

#### `ProductVariant`
- **Current NF:** 3NF
- **Issues:**
  - `SKU` has no `UNIQUE` constraint.
  - `VariantName` is a free text field — no enforcement of consistent naming (e.g., "Large", "L", "lrg" all valid).
  - `ReorderThreshold` is stored per-variant, which is correct, but there is no trigger or automated process to raise a `LowStockAlert` — this is a business logic gap.
- **Recommendation:** Add `UNIQUE` on `SKU` where not null. Consider a `VariantType` lookup table for consistent naming.

#### `Order`
- **Current NF:** 2NF, **partial 3NF violation**
- **Issues:**
  - `SubTotal`, `DiscountAmount`, `ShippingFee` are stored financial aggregates. `SubTotal` should be derivable from `SUM(OrderItem.UnitPrice * OrderItem.Quantity)`. Storing it risks inconsistency if `OrderItem` rows are modified post-order.
  - No `TotalAmount` column — computed only in the view `vw_OrderSummary`. This is actually good design (avoid redundancy), but the stored `SubTotal` undermines the principle.
  - `ContactPhone` duplicates `User.PhoneNumber` — arguably acceptable as a snapshot at order time.
- **Recommendation:** Document clearly whether `SubTotal` is a snapshot (valid) or a live computed field (risky). If snapshot, add a comment or `IsSnapshot` flag pattern similar to `Address.IsSnapshot`.

#### `Payment`
- **Current NF:** 3NF ✅
- **Issues:**
  - `PaymentDate NULL` — a completed payment without a date is semantically odd. Should be `NOT NULL` when `PaymentStatus = 'Completed'`. Enforce via CHECK or application layer.
  - No unique constraint on `(OrderId, PaymentStage)` — technically an order can have duplicate Upfront or Confirmation payment rows.
- **Recommendation:** Add `UNIQUE (OrderId, PaymentStage)`.

#### `GCashPayment` / `BankTransferPayment`
- **Current NF:** 3NF ✅ (Table-Per-Type inheritance is well implemented)
- **Issues:**
  - `GcashTransactionId` has no `UNIQUE` constraint — duplicate transaction IDs (potentially fraudulent double-submissions) are not blocked at the DB level.
  - `BpiReferenceNumber` name hard-codes the bank name. If the shop adds UnionBank or other banks, this field name becomes misleading. Should be renamed to `BankReferenceNumber`.
  - `ScreenshotUrl` and `StorageBucket`/`StoragePath` are redundant — the URL can be constructed from bucket + path. Three columns per file is a denormalized storage reference pattern repeated across multiple tables.
- **Recommendation:** Add `UNIQUE` on `GcashTransactionId WHERE GcashTransactionId IS NOT NULL`. Rename `BpiReferenceNumber`. Consider a normalized `StorageObject(Bucket, Path, Url)` table or at minimum a consistent naming convention.

#### `Delivery` / `LBCDelivery` / `LalamoveDelivery`
- **Current NF:** 3NF ✅ (TPT well implemented)
- **Issues:**
  - `CK_Delivery_Courier` only allows 'LBC' and 'Lalamove'. Adding a new courier requires a schema change.
  - No `UpdatedAt` on `Delivery` — cannot track when status last changed.
  - No `DeliveryAddress` on `Delivery` — must JOIN Order → Address to find it.
- **Recommendation:** Replace courier CHECK with a `Courier` lookup/reference table.

#### `Category`
- **Current NF:** 3NF ✅
- **Issues:**
  - `ParentCategoryId` self-reference is correct, but there is no `CHECK` or application-level guard against **circular references** (CategoryA is parent of CategoryB, CategoryB is parent of CategoryA).
  - No `Level` or `Path` column for efficient hierarchy traversal in SQL Server. Deep hierarchy queries require recursive CTEs.
- **Recommendation:** Consider adding a `HierarchyPath varchar(500)` (materialized path pattern) or use SQL Server's `hierarchyid` type for efficient tree traversal.

#### `Supplier`
- **Current NF:** 1NF only — **violates 2NF/3NF**
- **Issues:**
  - `Address nvarchar(500)` is a **free-text, unnormalized address** — inconsistent with the rest of the system which has a proper `Address` table.
  - No FK to the `Address` table.
  - No `UpdatedAt` column.
- **Recommendation:** Normalize by adding `AddressId INT NULL REFERENCES Address(AddressId)` or adding structured address columns (Street, City, Province, PostalCode, Country) consistent with the `Address` table.

#### `OTPVerification`
- **Current NF:** 3NF (as a standalone)
- **Issues:**
  - No FK to `User` — linked only by email string. If a user changes their email, historical OTP records become orphaned.
  - `OTPCode` is stored as plain text — should be hashed.
  - No `UserId` FK prevents efficient per-user OTP queries.
  - No index on `Email` + `IsUsed` — every OTP lookup requires a full scan.
- **Recommendation:** Add `UserId INT NULL REFERENCES [User](UserId)` and index `(Email, IsUsed, ExpiresAt)`.

#### `Review`
- **Current NF:** 3NF ✅
- **Issues:**
  - No `UNIQUE` constraint on `(UserId, ProductId)` or `(UserId, OrderId, ProductId)` — a user can submit unlimited reviews for the same product.
  - `IsVerifiedPurchase` is a stored flag — this should be derived from the existence of `OrderItem` rows, but storing it is acceptable for query performance.
- **Recommendation:** Add `UNIQUE (UserId, ProductId)` to enforce one review per product per customer.

#### `SystemLog`
- **Current NF:** 3NF (as structured)
- **Issues:**
  - No `EntityId INT` or `EntityType NVARCHAR(50)` columns — all contextual data is embedded in the free-text `EventDescription`. This makes it nearly impossible to efficiently retrieve "all logs for OrderId = 1234".
  - The `EventType` CHECK constraint enumerates 22 specific values in a DDL statement — adding a new event type requires a schema migration.
  - No index on `EventType` or `(UserId, CreatedAt)`.
- **Recommendation:** Add `EntityId INT NULL` and `EntityType NVARCHAR(50) NULL` structured columns. Move event type values to a lookup table.

#### `Cart`
- **Current NF:** 3NF ✅
- **Issues:**
  - No `UpdatedAt` column.
  - No `ExpiresAt` on the cart itself — expiry is only tracked on `GuestSession`. Authenticated user carts never expire.
  - No enforcement of "one active cart per user" — multiple active carts possible.
- **Recommendation:** Add `ExpiresAt DATETIME NULL` and `UpdatedAt DATETIME NULL`. Add a filtered unique index: `UNIQUE (UserId) WHERE UserId IS NOT NULL AND [Status] = 'Active'` (after adding a `Status` column).

#### `POS_Session`
- **Current NF:** 3NF (structurally)
- **Issues:**
  - `TotalSales DECIMAL(10,2)` is a **stored aggregate** with no referential integrity backing. If an order associated with this session is cancelled, `TotalSales` becomes stale.
  - No FK to `Order` — cannot determine which orders were placed in which POS session.
- **Recommendation:** Remove `TotalSales` or compute it via a view. Add a `POSSessionId` FK on `Order` for proper traceability.

---

## 4. KEYS & CONSTRAINTS REVIEW

### 4.1 Primary Keys

| Table | PK | Assessment |
|---|---|---|
| All tables | `INT IDENTITY(1,1)` surrogate keys | ✅ Correct. Consistent across all tables. |
| `UserRole` | `UserRoleId` surrogate + UQ on `(UserId, RoleId)` | ✅ Correct pattern. |
| `UserVoucher` | `UserVoucherId` surrogate + UQ on `(UserId, VoucherId)` | ✅ Correct. |
| `Wishlist` | `WishlistId` surrogate + UQ on `(UserId, ProductId)` | ✅ Correct. |
| `PickupOrder` | `PickupOrderId` + UQ on `OrderId` | ✅ Enforces 1:1 with Order. |

### 4.2 Critical Missing UNIQUE Constraints

| Table | Column | Issue | Priority |
|---|---|---|---|
| `Product` | `SKU` | Duplicate SKUs permitted. Inventory systems break. | 🔴 Critical |
| `ProductVariant` | `SKU` | Duplicate variant SKUs permitted. | 🔴 Critical |
| `Voucher` | `Code` | No UNIQUE constraint on voucher code. Two vouchers can share the same code. | 🔴 Critical |
| `GCashPayment` | `GcashTransactionId` | Duplicate transactions not blocked — fraud/double-submission risk. | 🔴 Critical |
| `Payment` | `(OrderId, PaymentStage)` | Duplicate payment stages per order. | 🟡 Major |
| `Review` | `(UserId, ProductId)` | Multiple reviews per user per product. | 🟡 Major |
| `ProductImage` | `(ProductId) WHERE IsPrimary=1` | Multiple primary images per product. | 🟡 Major |
| `Address` | `(UserId, IsDefault=1)` | Multiple default addresses per user. | 🟡 Major |

### 4.3 Missing Foreign Keys

All critical FKs are present and declared. The following **implied** relationships lack FK enforcement:

| Source | Column | Missing Target | Impact |
|---|---|---|---|
| `OTPVerification` | `Email` | `User.Email` | Orphaned OTP records after email change |
| `Order` | *(no CartId)* | `Cart.CartId` | No cart-to-order traceability |
| `Order` | *(no POSSessionId)* | `POS_Session.POSSessionId` | No session-to-order linkage |

### 4.4 Circular FK (Design Risk)

`User.DefaultAddressId` → `Address.AddressId`  
`Address.UserId` → `User.UserId`  

This circular dependency means neither table can be populated first. Resolution requires:
1. Insert `User` with `DefaultAddressId = NULL`
2. Insert `Address`
3. `UPDATE User SET DefaultAddressId = @id`

This is the correct approach but must be documented. The FK on `User.DefaultAddressId` should be declared with `SET NULL` on delete of the address.

### 4.5 Missing CHECK Constraints

| Table | Column | Missing Constraint |
|---|---|---|
| `User` | `Email` | `CHECK (IsWalkIn = 1 OR Email IS NOT NULL)` |
| `User` | `FailedLoginAttempts` | `CHECK (FailedLoginAttempts >= 0)` |
| `Product` | `Price` | `CHECK (Price >= 0)` |
| `Product` | `Currency` | `CHECK (Currency IN ('PHP', 'USD', ...))` or move to config |
| `ProductVariant` | `AdditionalPrice` | `CHECK (AdditionalPrice >= 0)` |
| `OTPVerification` | `OTPCode` | Should be validated length (e.g., `CHECK (LEN(OTPCode) = 6)`) |
| `Notification` | `RetryCount` | `CHECK (RetryCount >= 0 AND RetryCount <= 3)` |
| `Address` | *(IsDefault dual-default)* | Partial unique index needed |
| `Payment` | *(PaymentDate)* | `CHECK (PaymentDate IS NOT NULL OR PaymentStatus != 'Completed')` |

---

## 5. INDEXING STRATEGY

### 5.1 Existing Indexes (Implicit)
All PKs are clustered indexes. Several UQ constraints create nonclustered indexes automatically.

### 5.2 Recommended Additional Indexes

#### HIGH PRIORITY — Query Critical

```sql
-- 1. Order lookup by user (most common e-commerce query)
CREATE INDEX IX_Order_UserId_OrderDate ON [Order] (UserId, OrderDate DESC);
-- WHY: Every "My Orders" page executes this join. Without it, a full table scan occurs.

-- 2. Product filtering by category and active status
CREATE INDEX IX_Product_CategoryId_IsActive ON Product (CategoryId, IsActive) INCLUDE (Name, Price, SKU);
-- WHY: Homepage/browse pages filter active products by category constantly.

-- 3. Product variant lookup by product
CREATE INDEX IX_ProductVariant_ProductId_IsActive ON ProductVariant (ProductId, IsActive);
-- WHY: Every product detail page fetches variants. Critical path query.

-- 4. OrderItem by OrderId (line item fetch)
CREATE INDEX IX_OrderItem_OrderId ON OrderItem (OrderId);
-- WHY: Every order detail view fetches all items. High-frequency join.

-- 5. Payment by OrderId
CREATE INDEX IX_Payment_OrderId ON Payment (OrderId);
-- WHY: Payment status checks are on the critical checkout path.

-- 6. Delivery by OrderId
CREATE INDEX IX_Delivery_OrderId ON Delivery (OrderId);
-- WHY: Order status views always check delivery records.

-- 7. SupportTicket by UserId and status
CREATE INDEX IX_SupportTicket_UserId_Status ON SupportTicket (UserId, TicketStatus);
-- WHY: "My Tickets" and admin open-ticket views run this constantly.

-- 8. Notification by UserId, IsRead, Status
CREATE INDEX IX_Notification_UserId_IsRead ON Notification (UserId, IsRead, Status) INCLUDE (NotifType, CreatedAt);
-- WHY: Notification bell (unread count) is fetched on every page load.

-- 9. Cart by UserId
CREATE INDEX IX_Cart_UserId ON Cart (UserId);
-- WHY: Every page load checks for an active cart.

-- 10. CartItem by CartId
CREATE INDEX IX_CartItem_CartId ON CartItem (CartId);
-- WHY: Cart display joins CartItem by CartId.
```

#### MEDIUM PRIORITY — Analytics & Admin

```sql
-- 11. InventoryLog by ProductVariantId, CreatedAt
CREATE INDEX IX_InventoryLog_VariantId_Date ON InventoryLog (ProductVariantId, CreatedAt DESC);
-- WHY: Low-stock monitoring and inventory history views filter by variant and date.

-- 12. ProductImage by ProductId, IsPrimary
CREATE INDEX IX_ProductImage_ProductId_Primary ON ProductImage (ProductId, IsPrimary) INCLUDE (ImageUrl);
-- WHY: Primary image fetch is in every product listing query. The vw_ActiveProducts view uses this join.

-- 13. VoucherUsage by VoucherId, UserId
CREATE INDEX IX_VoucherUsage_VoucherId_UserId ON VoucherUsage (VoucherId, UserId);
-- WHY: Per-user voucher usage count check (enforcing MaxUsesPerUser) runs on every checkout.

-- 14. Review by ProductId
CREATE INDEX IX_Review_ProductId ON Review (ProductId);
-- WHY: Product detail page fetches reviews by product.

-- 15. SystemLog by EventType, CreatedAt
CREATE INDEX IX_SystemLog_EventType_Date ON SystemLog (EventType, CreatedAt DESC);
-- WHY: Admin audit log views filter by event type in date-descending order.

-- 16. OTPVerification by Email, IsUsed, ExpiresAt
CREATE INDEX IX_OTPVerification_Email ON OTPVerification (Email, IsUsed, ExpiresAt);
-- WHY: OTP validation does a point-lookup by email on every auth attempt.

-- 17. PriceHistory by ProductId, ChangedAt
CREATE INDEX IX_PriceHistory_ProductId_Date ON PriceHistory (ProductId, ChangedAt DESC);
-- WHY: Price history views fetch by product in chronological order.

-- 18. OrderStatusAudit by OrderId
CREATE INDEX IX_OrderStatusAudit_OrderId ON OrderStatusAudit (OrderId);
-- WHY: Order timeline views fetch all status transitions for an order.

-- 19. ProductVariant stock monitoring
CREATE INDEX IX_ProductVariant_LowStock ON ProductVariant (StockQuantity, ReorderThreshold) 
WHERE IsActive = 1;
-- WHY: Background jobs and admin dashboards identify variants below reorder threshold.
-- Filtered index keeps it small and fast.
```

---

## 6. DESIGN PROBLEMS & RISKS

### 6.1 Critical Design Problems

**[P1] No UNIQUE constraint on Voucher.Code**  
Two vouchers with the same code can coexist. At checkout, the wrong voucher could be applied, causing incorrect discounts or customer disputes. This is a **data integrity and financial risk**.

**[P2] Product SKU has no UNIQUE constraint**  
Duplicate SKUs silently enter the system. Inventory management tools, barcode scanners, and import/export pipelines that rely on SKU uniqueness will malfunction. This is a **business-critical flaw**.

**[P3] Product attributes are denormalized (bike-specific flat columns)**  
`WheelSize`, `SpeedCompatibility`, `BoostCompatible`, `TubelessReady`, `AxleStandard`, `SuspensionTravel`, `BrakeType` are baked into the `Product` table as fixed columns. This means:
- Non-bike products (accessories, helmets) will have `NULL` in most of these columns
- Adding a new spec type requires an `ALTER TABLE` schema migration
- Filtering products by spec requires querying nullable columns with inconsistent data

**[P4] OTPVerification OTPCode stored in plaintext**  
Storing OTP codes in plaintext is a security vulnerability. If the database is compromised, an attacker could intercept active OTPs and take over accounts. OTPs should be hashed using a fast hash (SHA-256) before storage.

**[P5] GCashPayment.GcashTransactionId has no UNIQUE constraint**  
A malicious or buggy client could submit the same GCash transaction reference multiple times. Without DB-level uniqueness, multiple payments pointing to the same GCash transaction can be approved.

**[P6] User.Email is nullable with no conditional enforcement**  
A non-walk-in user can exist in the system with no email address. This breaks authentication, password reset flows, and notification delivery. There is no `CHECK` constraint to prevent this.

### 6.2 Scalability Issues

**[S1] Product.Currency stored per product**  
As product catalog grows to thousands of rows, changing the shop's currency requires a full-table update. This should be a system-level configuration value.

**[S2] SystemLog.EventType as CHECK constraint**  
With 22 hardcoded event types in a `CHECK` constraint, adding a new event type requires a DDL `ALTER TABLE` statement — a production schema migration. A `SystemEventType` lookup table would allow runtime addition of new event types.

**[S3] Delivery.Courier as CHECK constraint**  
Only 'LBC' and 'Lalamove' are allowed. Adding a new courier (e.g., J&T Express, Ninja Van) requires a production migration. Should be a lookup table.

**[S4] No partitioning strategy for audit/log tables**  
`SystemLog`, `InventoryLog`, `OrderStatusAudit` will grow unboundedly. Without archiving or partitioning by date, queries on these tables will degrade significantly over time.

**[S5] Product.Description as nvarchar(max)**  
This is stored in the row overflow page. For catalog listing queries, this column is fetched unnecessarily. It should be excluded from covering indexes and accessed only on detail page queries.

### 6.3 Data Integrity Risks

**[R1] POS_Session.TotalSales is a stored aggregate**  
If an order placed during a POS session is later cancelled or refunded, `TotalSales` becomes incorrect. There is no trigger or application mechanism described to keep this in sync.

**[R2] Order.SubTotal inconsistency risk**  
If `OrderItem` rows could theoretically be modified post-order creation (no immutability enforced by DB), `SubTotal` would diverge from the actual line item total.

**[R3] No immutability enforcement on OrderItem or InventoryLog**  
These are financial and audit records. A `DELETE` or `UPDATE` on `OrderItem` after order placement would silently corrupt financial history. Consider DB-level row-level security or application-enforced immutability.

**[R4] BankTransferPayment.VerificationDeadline not enforced**  
The deadline column exists, but there is no `CHECK` constraint ensuring `VerificationDeadline > SubmittedAt`, nor any enforced action when the deadline passes.

**[R5] Multiple default addresses per user**  
`Address.IsDefault = 1` can be set on multiple rows for the same user. There is no filtered unique index or trigger enforcing "at most one default address per user."

**[R6] Multiple primary product images**  
`ProductImage.IsPrimary = 1` can be set on multiple rows for the same product. The `vw_ActiveProducts` view could return duplicate rows or an unpredictable primary image.

### 6.4 Security Concerns

**[SEC1] OTPCode in plaintext** — See P4 above. Upgrade to SHA-256 hash immediately.

**[SEC2] PasswordHash column — algorithm not tracked**  
There is no `PasswordHashAlgorithm` or `PasswordHashVersion` column. If the hashing algorithm needs to be upgraded (e.g., from bcrypt to Argon2), there is no way to identify which rows use which algorithm for a graceful migration.

**[SEC3] No row-level security on User-owned data**  
There are no SQL Server Row-Level Security policies defined. Application layer must correctly enforce that users can only access their own orders, addresses, etc. A misconfigured API endpoint could expose all user data.

**[SEC4] GuestSession.SessionToken — exposure risk**  
If `SessionToken` values are guessable or short, session hijacking is possible. The DB cannot enforce entropy, but the application must use cryptographically random tokens (UUID v4 or equivalent). Length constraint (`nvarchar(100)`) allows for this, but is not documented.

---

## 7. IMPROVED DATABASE DESIGN (REFACTORED SCHEMA)

The following improvements are annotated with change rationale. Only changed/new DDL is shown for brevity.

### 7.1 Fix Critical Issues

```sql
-- FIX P2: Unique SKU on Product
ALTER TABLE [dbo].[Product]
  ADD CONSTRAINT [UQ_Product_SKU] UNIQUE NONCLUSTERED ([SKU])
  WHERE [SKU] IS NOT NULL;

-- FIX P2: Unique SKU on ProductVariant
ALTER TABLE [dbo].[ProductVariant]
  ADD CONSTRAINT [UQ_ProductVariant_SKU] UNIQUE NONCLUSTERED ([SKU])
  WHERE [SKU] IS NOT NULL;

-- FIX P1: Unique Voucher Code
ALTER TABLE [dbo].[Voucher]
  ADD CONSTRAINT [UQ_Voucher_Code] UNIQUE NONCLUSTERED ([Code]);

-- FIX P5: Unique GCash Transaction
ALTER TABLE [dbo].[GCashPayment]
  ADD CONSTRAINT [UQ_GCashPayment_TransactionId] UNIQUE NONCLUSTERED ([GcashTransactionId])
  WHERE [GcashTransactionId] IS NOT NULL;

-- FIX: One payment stage per order
ALTER TABLE [dbo].[Payment]
  ADD CONSTRAINT [UQ_Payment_OrderStage] UNIQUE NONCLUSTERED ([OrderId], [PaymentStage]);

-- FIX R5: One default address per user (filtered unique index)
CREATE UNIQUE NONCLUSTERED INDEX [UIX_Address_OneDefault]
  ON [dbo].[Address] ([UserId])
  WHERE [IsDefault] = 1;

-- FIX R6: One primary image per product (filtered unique index)
CREATE UNIQUE NONCLUSTERED INDEX [UIX_ProductImage_OnePrimary]
  ON [dbo].[ProductImage] ([ProductId])
  WHERE [IsPrimary] = 1;

-- FIX: One review per user per product
ALTER TABLE [dbo].[Review]
  ADD CONSTRAINT [UQ_Review_UserProduct] UNIQUE NONCLUSTERED ([UserId], [ProductId]);

-- FIX P6: Email required for non-walk-in users
ALTER TABLE [dbo].[User]
  ADD CONSTRAINT [CK_User_EmailRequired]
  CHECK ([IsWalkIn] = 1 OR [Email] IS NOT NULL);

-- FIX: Price >= 0 on Product
ALTER TABLE [dbo].[Product]
  ADD CONSTRAINT [CK_Product_Price] CHECK ([Price] >= 0);

-- FIX: AdditionalPrice >= 0 on ProductVariant
ALTER TABLE [dbo].[ProductVariant]
  ADD CONSTRAINT [CK_ProductVariant_AdditionalPrice] CHECK ([AdditionalPrice] >= 0);
```

### 7.2 Normalize Product Attributes

```sql
-- NEW TABLE: ProductAttribute — replaces flat bike spec columns
-- Allows any product type, not just bikes
CREATE TABLE [dbo].[ProductAttribute] (
  [AttributeId]   INT           IDENTITY(1,1) NOT NULL,
  [ProductId]     INT           NOT NULL,
  [AttributeKey]  NVARCHAR(100) NOT NULL,  -- e.g., 'WheelSize', 'BrakeType'
  [AttributeValue] NVARCHAR(200) NOT NULL,
  CONSTRAINT [PK_ProductAttribute] PRIMARY KEY ([AttributeId]),
  CONSTRAINT [FK_ProductAttribute_Product] FOREIGN KEY ([ProductId])
    REFERENCES [Product]([ProductId]) ON DELETE CASCADE,
  CONSTRAINT [UQ_ProductAttribute_Pair] UNIQUE ([ProductId], [AttributeKey])
);
CREATE INDEX [IX_ProductAttribute_ProductId] ON [dbo].[ProductAttribute] ([ProductId]);

-- REMOVE from Product table (migration):
-- ALTER TABLE [Product] DROP COLUMN WheelSize, SpeedCompatibility, BoostCompatible,
--   TubelessReady, AxleStandard, SuspensionTravel, BrakeType, Material, Color, AdditionalSpecs;
```

### 7.3 Normalize Supplier Address

```sql
-- OPTION A: Add structured address columns to Supplier (simpler)
ALTER TABLE [dbo].[Supplier]
  ADD [Street]     NVARCHAR(500) NULL,
      [City]       NVARCHAR(100) NULL,
      [Province]   NVARCHAR(100) NULL,
      [PostalCode] NVARCHAR(20)  NULL,
      [Country]    NVARCHAR(100) NULL DEFAULT 'Philippines',
      [UpdatedAt]  DATETIME      NULL;
-- Then migrate data from [Address] free-text column and DROP the old column.

-- OPTION B: Add FK to Address table (more normalized, requires Address.UserId to be nullable)
-- ALTER TABLE [Address] ALTER COLUMN [UserId] INT NULL;
-- ALTER TABLE [Supplier] ADD [AddressId] INT NULL REFERENCES [Address]([AddressId]);
```

### 7.4 Add Missing Traceability Columns

```sql
-- POS Session linkage on Order
ALTER TABLE [dbo].[Order]
  ADD [POSSessionId] INT NULL;
ALTER TABLE [dbo].[Order]
  ADD CONSTRAINT [FK_Order_POSSession]
  FOREIGN KEY ([POSSessionId]) REFERENCES [POS_Session]([POSSessionId]);

-- Cart-to-Order traceability
ALTER TABLE [dbo].[Order]
  ADD [CartId] INT NULL;
ALTER TABLE [dbo].[Order]
  ADD CONSTRAINT [FK_Order_Cart]
  FOREIGN KEY ([CartId]) REFERENCES [Cart]([CartId]);

-- ChangedByUserId on OrderStatusAudit
ALTER TABLE [dbo].[OrderStatusAudit]
  ADD [ChangedByUserId] INT NULL;
ALTER TABLE [dbo].[OrderStatusAudit]
  ADD CONSTRAINT [FK_OrderStatusAudit_User]
  FOREIGN KEY ([ChangedByUserId]) REFERENCES [User]([UserId]);

-- UserId FK on OTPVerification
ALTER TABLE [dbo].[OTPVerification]
  ADD [UserId] INT NULL;
ALTER TABLE [dbo].[OTPVerification]
  ADD CONSTRAINT [FK_OTP_User]
  FOREIGN KEY ([UserId]) REFERENCES [User]([UserId]);
```

### 7.5 Security Improvements

```sql
-- Hash tracking on User passwords
ALTER TABLE [dbo].[User]
  ADD [PasswordHashAlgorithm] NVARCHAR(20) NULL DEFAULT 'bcrypt';
-- Allows graceful migration between hashing algorithms.

-- OTPVerification: hash the code (application must hash before INSERT/SELECT)
-- Column type stays NVARCHAR(255) to accommodate hash output length.
-- Application change: store SHA-256(OTPCode) instead of plaintext.

-- Cart expiry for authenticated users
ALTER TABLE [dbo].[Cart]
  ADD [ExpiresAt]  DATETIME NULL,
      [UpdatedAt]  DATETIME NULL,
      [CartStatus] NVARCHAR(20) NOT NULL DEFAULT 'Active';
ALTER TABLE [dbo].[Cart]
  ADD CONSTRAINT [CK_Cart_Status]
  CHECK ([CartStatus] IN ('Active', 'Converted', 'Abandoned', 'Expired'));
```

### 7.6 Replace Hardcoded Enums with Lookup Tables

```sql
-- NEW: Courier lookup table (replaces CK_Delivery_Courier)
CREATE TABLE [dbo].[CourierProvider] (
  [CourierId]   INT            IDENTITY(1,1) NOT NULL,
  [CourierCode] NVARCHAR(50)   NOT NULL,
  [CourierName] NVARCHAR(100)  NOT NULL,
  [IsActive]    BIT            NOT NULL DEFAULT 1,
  CONSTRAINT [PK_CourierProvider] PRIMARY KEY ([CourierId]),
  CONSTRAINT [UQ_CourierProvider_Code] UNIQUE ([CourierCode])
);
INSERT INTO [dbo].[CourierProvider] VALUES ('LBC', 'LBC Express', 1);
INSERT INTO [dbo].[CourierProvider] VALUES ('Lalamove', 'Lalamove Philippines', 1);

-- ALTER Delivery: replace Courier NVARCHAR with CourierId FK
ALTER TABLE [dbo].[Delivery]
  ADD [CourierId] INT NULL;
ALTER TABLE [dbo].[Delivery]
  ADD CONSTRAINT [FK_Delivery_Courier] FOREIGN KEY ([CourierId])
  REFERENCES [CourierProvider]([CourierId]);
-- After data migration, drop old Courier column and CHECK constraint.
```

### 7.7 Add Missing UpdatedAt Columns

```sql
ALTER TABLE [dbo].[Delivery]  ADD [UpdatedAt] DATETIME NULL;
ALTER TABLE [dbo].[Supplier]  ADD [UpdatedAt] DATETIME NULL;
ALTER TABLE [dbo].[Cart]      ADD [UpdatedAt] DATETIME NULL;
ALTER TABLE [dbo].[SupportTask] ADD [UpdatedAt] DATETIME NULL;
```

### 7.8 Structured SystemLog

```sql
-- Add entity reference columns to SystemLog
ALTER TABLE [dbo].[SystemLog]
  ADD [EntityType] NVARCHAR(50) NULL,  -- e.g., 'Order', 'Payment', 'User'
      [EntityId]   INT          NULL;  -- e.g., OrderId value
CREATE INDEX [IX_SystemLog_Entity] ON [dbo].[SystemLog] ([EntityType], [EntityId]);
```

---

## 8. BEST PRACTICES CHECKLIST

### ✅ What Is Correctly Designed

| # | Item |
|---|---|
| 1 | Surrogate integer identity PKs on all tables — consistent and performant |
| 2 | Table-Per-Type (TPT) inheritance for Payment subtypes (GCash/BankTransfer) — correctly implemented |
| 3 | TPT inheritance for Delivery subtypes (LBC/Lalamove) — correctly implemented |
| 4 | Self-referential Category with ParentCategoryId — correct hierarchy design |
| 5 | CHECK constraints on Order.OrderStatus, Payment.PaymentMethod/Status/Stage, Delivery.Status |
| 6 | CHECK constraints on Review.Rating (1–5), Voucher discount values, PurchaseOrder status |
| 7 | UNIQUE on (UserId, RoleId) in UserRole — prevents duplicate role assignments |
| 8 | UNIQUE on (UserId, VoucherId) in UserVoucher — prevents duplicate voucher assignments |
| 9 | UNIQUE on (UserId, ProductId) in Wishlist — prevents duplicate wishlist entries |
| 10 | UNIQUE on OrderId in PickupOrder — enforces 1:1 relationship with Order |
| 11 | OrderItem.UnitPrice captures price at time of purchase — correct snapshot pattern |
| 12 | CartItem.PriceAtAdd captures price at cart add time — correct |
| 13 | Address.IsSnapshot flag for shipping address snapshots — good historical preservation design |
| 14 | InventoryLog as an append-only audit trail — correct pattern for stock change tracking |
| 15 | OrderStatusAudit as an append-only state transition log — correct |
| 16 | PriceHistory as an append-only audit trail for price changes — correct |
| 17 | VoucherUsage.DiscountAmount captures actual discount at time of use — correct snapshot |
| 18 | GuestSession with ConvertedToUserId tracks anonymous-to-registered conversion |
| 19 | Notification retry tracking with RetryCount and FailureReason — production-ready pattern |
| 20 | Role.RoleName CHECK constraint limiting valid roles |
| 21 | Comprehensive views (12 total) providing clean query abstractions |
| 22 | Product.IsFeatured and IsActive for catalog visibility control |
| 23 | Category.DisplayOrder for controlled sort order in the UI |
| 24 | IsWalkIn flag on both User and Order — supports POS walk-in flows |

### ⚠️ What Needs Improvement

| # | Item | Priority |
|---|---|---|
| 1 | Add `UNIQUE` on `Product.SKU`, `ProductVariant.SKU`, `Voucher.Code` | 🔴 Critical |
| 2 | Add `UNIQUE` on `GCashPayment.GcashTransactionId` | 🔴 Critical |
| 3 | Add `CHECK (IsWalkIn=1 OR Email IS NOT NULL)` on User | 🔴 Critical |
| 4 | Hash OTPCode before storage (application + schema concern) | 🔴 Critical |
| 5 | Add filtered unique index for one primary image per product | 🟡 Major |
| 6 | Add filtered unique index for one default address per user | 🟡 Major |
| 7 | Add `UNIQUE (UserId, ProductId)` on Review | 🟡 Major |
| 8 | Add `UNIQUE (OrderId, PaymentStage)` on Payment | 🟡 Major |
| 9 | Add `EntityId` / `EntityType` columns to SystemLog | 🟡 Major |
| 10 | Add `POSSessionId` FK on Order for POS traceability | 🟡 Major |
| 11 | Add `CartId` FK on Order for cart-to-order traceability | 🟡 Major |
| 12 | Normalize Supplier.Address to structured fields | 🟡 Major |
| 13 | Add `UpdatedAt` to Delivery, Supplier, Cart, SupportTask | 🟠 Minor |
| 14 | Add `ChangedByUserId` to OrderStatusAudit | 🟠 Minor |
| 15 | Add `PasswordHashAlgorithm` to User | 🟠 Minor |
| 16 | Move Product bike-specific attributes to EAV table | 🔵 Recommended |
| 17 | Replace Delivery.Courier CHECK with CourierProvider lookup table | 🔵 Recommended |
| 18 | Replace SystemLog.EventType CHECK with EventType lookup table | 🔵 Recommended |
| 19 | Add `UserId` FK to OTPVerification | 🔵 Recommended |
| 20 | Add `ExpiresAt` and `CartStatus` to Cart | 🔵 Recommended |
| 21 | Remove or audit POS_Session.TotalSales stored aggregate | 🔵 Recommended |
| 22 | Add hierarchy path support to Category (materialized path or hierarchyid) | 🔵 Recommended |
| 23 | Add `Product.Currency` → system config table migration | 🔵 Recommended |
| 24 | Rename `BankTransferPayment.BpiReferenceNumber` → `BankReferenceNumber` | 🔵 Recommended |

### 🚨 Critical vs. Optional Summary

| Classification | Count | Items |
|---|---|---|
| 🔴 **Critical (Fix Immediately)** | 4 | SKU uniqueness, Voucher code uniqueness, GCash transaction uniqueness, User email enforcement |
| 🟡 **Major (Fix Before Production Scale)** | 8 | Payment stage uniqueness, ProductImage primary enforcement, Address default enforcement, Review uniqueness, SystemLog structure, POS/Cart traceability, OTP hashing |
| 🟠 **Minor (Fix in Next Sprint)** | 4 | UpdatedAt columns, OrderStatusAudit author, PasswordHashAlgorithm |
| 🔵 **Recommended (Backlog)** | 10 | EAV attributes, courier lookup, event type lookup, cart expiry, hierarchy paths, currency normalization |

---

*Report prepared for internal engineering review. All recommendations should be evaluated against current application behavior before applying schema migrations to production.*
