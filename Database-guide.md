# Taurus Bike Shop — Database Guide

**Database:** `Taurus-bike-shop-sqlserver-2026`  
**Schema version:** Seed v5 (exported 04/24/2026)  
**Total tables:** 42 | **Total views:** 12

This guide explains every table in the database — what it stores, why it exists, and how it connects to the rest of the system. Read it top to bottom the first time; afterwards use it as a reference when you are writing queries or tracing a bug.

---

## How to read this guide

Each table section answers three questions:

| Label | What it answers |
|-------|-----------------|
| **What it stores** | The concrete data sitting in the rows |
| **Why it exists** | The business or design reason the table was added |
| **Key columns** | The columns worth understanding in detail |
| **Linked to** | Other tables that reference or are referenced by this one |

---

## Table of contents

1. [User identity and roles](#1-user-identity-and-roles)
   - User, Role, UserRole
2. [Authentication and sessions](#2-authentication-and-sessions)
   - ActiveSession, GuestSession, OTPVerification
3. [Addresses](#3-addresses)
   - Address
4. [Product catalog](#4-product-catalog)
   - Category, Brand, Product, ProductVariant, ProductImage
5. [Shopping cart and wishlist](#5-shopping-cart-and-wishlist)
   - Cart, CartItem, Wishlist
6. [Orders](#6-orders)
   - Order, OrderItem, PickupOrder
7. [Delivery](#7-delivery)
   - Delivery, LalamoveDelivery, LBCDelivery
8. [Payments](#8-payments)
   - Payment, GCashPayment, BankTransferPayment, StorePaymentAccount, Refund
9. [Vouchers and discounts](#9-vouchers-and-discounts)
   - Voucher, UserVoucher, VoucherUsage
10. [Inventory and suppliers](#10-inventory-and-suppliers)
    - Supplier, PurchaseOrder, PurchaseOrderItem, InventoryLog, PriceHistory
11. [Point of sale (POS)](#11-point-of-sale-pos)
    - POS_Session
12. [Customer support](#12-customer-support)
    - SupportTicket, SupportTicketReply, SupportTask
13. [Notifications](#13-notifications)
    - Notification
14. [Reviews](#14-reviews)
    - Review
15. [Audit and system logs](#15-audit-and-system-logs)
    - OrderStatusAudit, SystemLog
16. [Database views](#16-database-views)
17. [Design patterns explained](#17-design-patterns-explained)
18. [Entity-relationship summary](#18-entity-relationship-summary)
19. [Study questions](#19-study-questions)

---

## 1. User identity and roles

### `User`

**What it stores:** Every person who interacts with the system — online shoppers, walk-in customers processed through the POS terminal, and staff/admin accounts. One table holds all of them.

**Why it exists:** Centralizing all people in one table avoids duplication. A staff member who also shops online is still one row. Roles (see `UserRole`) determine what each person can do.

**Key columns:**

| Column | Type | Notes |
|--------|------|-------|
| `UserId` | `int IDENTITY` | Auto-incrementing primary key |
| `Email` | `nvarchar(255)` | NULL for walk-in customers who don't register |
| `PasswordHash` | `nvarchar(255)` | Stored as a hash (bcrypt/PBKDF2), never plain text |
| `FirstName`, `LastName` | `nvarchar(100)` | Minimum required info for any account |
| `IsActive` | `bit` | Whether the account can log in |
| `IsWalkIn` | `bit` | `1` = this was created automatically at the POS for an anonymous customer |
| `FailedLoginAttempts` | `int` | Increments on each wrong password |
| `LockoutUntil` | `datetime2` | Account is locked until this timestamp after too many failures |
| `IsDeleted` | `bit` | Soft-delete flag — the row stays but is treated as gone |
| `DefaultAddressId` | `int` | FK to `Address` — pre-selected shipping address at checkout |

**Linked to:** `Address`, `Order`, `Cart`, `ActiveSession`, `UserRole`, `Wishlist`, `Review`, `SupportTicket`, `Notification`

---

### `Role`

**What it stores:** The named roles available in the system (e.g., `Admin`, `Staff`, `Customer`).

**Why it exists:** Separating roles into their own table means you can add a new role without changing any code — just insert a row. The `UserRole` junction table then assigns that role to users.

**Key columns:**

| Column | Notes |
|--------|-------|
| `RoleId` | PK |
| `RoleName` | Unique. Likely values: `Admin`, `Staff`, `Customer` |
| `Description` | Human-readable explanation of what the role can do |

**Linked to:** `UserRole`

---

### `UserRole`

**What it stores:** Which roles each user has. One user can have multiple roles (a shop owner might be both `Admin` and `Staff`).

**Why it exists:** This is a many-to-many junction table between `User` and `Role`. Without it you would have to store a comma-separated list of roles inside `User`, which is bad practice.

**Key columns:**

| Column | Notes |
|--------|-------|
| `UserId` + `RoleId` | Unique pair — enforced by `UX_UserRole_Pair` constraint |
| `AssignedAt` | When the role was granted |

**Linked to:** `User`, `Role`

---

## 2. Authentication and sessions

### `ActiveSession`

**What it stores:** One row per active login session. When a user logs in they receive a short-lived JWT access token (not stored in the DB) and a long-lived refresh token (stored here).

**Why it exists:** Storing refresh tokens in the database lets the system revoke a session remotely (e.g., "log out from all devices", or ban a stolen token). Without this table there would be no way to invalidate a token before it expires.

**Key columns:**

| Column | Notes |
|--------|-------|
| `RefreshToken` | Unique, long random string sent to the browser |
| `DeviceInfo` | Browser/OS info — useful for the "active devices" UI |
| `IpAddress` | Logged for security auditing |
| `IsRevoked` | Set to `1` when the session is ended |
| `ExpiresAt` | Hard expiry — a non-revoked but expired token is still invalid |
| `RevokedAt` | Timestamp of when revocation happened |

**Linked to:** `User`

---

### `GuestSession`

**What it stores:** A temporary session for shoppers who browse and add items to a cart without creating an account.

**Why it exists:** Many e-commerce customers do not want to register before they can shop. A guest session ties a cart to a browser cookie without a real account. If the guest later registers, `ConvertedToUserId` records the link so their cart and order history can be merged.

**Key columns:**

| Column | Notes |
|--------|-------|
| `SessionToken` | Unique token stored in the browser cookie |
| `Email`, `PhoneNumber` | Optionally collected at checkout |
| `ConvertedToUserId` | FK to `User` — filled in when the guest registers |
| `ExpiresAt` | Sessions expire automatically if the guest never returns |

**Linked to:** `User`, `Cart`, `Order`

---

### `OTPVerification`

**What it stores:** One-time passwords sent by email to verify identity (e.g., during registration or password reset).

**Why it exists:** A one-time password (OTP) is a temporary code that proves you control the email address. Storing it in the database allows the server to validate the code submitted by the user and mark it as used so it cannot be reused.

**Key columns:**

| Column | Notes |
|--------|-------|
| `Email` | The address the OTP was sent to |
| `OTPCode` | The hashed or plain code (128-char field allows hashing) |
| `IsUsed` | `1` after the code is consumed — prevents replay attacks |
| `ExpiresAt` | OTPs are short-lived (typically 5–10 minutes) |
| `UserId` | FK to `User` — nullable because OTP can be sent before the account exists |

**Linked to:** `User`

---

## 3. Addresses

### `Address`

**What it stores:** Shipping addresses for users. A user can have multiple saved addresses.

**Why it exists:** Separating addresses into their own table lets a user save several addresses (Home, Office, etc.) and pick one at checkout. It also lets orders snapshot the address at purchase time so that if the user later edits their address the historical order still shows where the package was sent.

**Key columns:**

| Column | Notes |
|--------|-------|
| `UserId` | Which user owns this address |
| `Label` | Friendly name: "Home", "Office" |
| `Street`, `City`, `PostalCode`, `Province`, `Country` | Full address fields |
| `IsDefault` | `1` = this is the pre-selected address at checkout |
| `IsSnapshot` | `1` = this row is a copy frozen at order time, not a live editable address |

**Why `IsSnapshot`?** When an order is placed, the current address is copied into a new `Address` row with `IsSnapshot = 1` and that row's `AddressId` is saved on the order. If the user later changes their address, all past orders still point to the frozen copy.

**Linked to:** `User`, `Order`

---

## 4. Product catalog

### `Category`

**What it stores:** The categories products belong to (e.g., "Bikes", "Accessories", "Wheels").

**Why it exists:** Categories let customers filter and browse products. The self-referencing `ParentCategoryId` column allows a hierarchy: "Bikes" → "Mountain Bikes" → "Full-Suspension".

**Key columns:**

| Column | Notes |
|--------|-------|
| `CategoryCode` | Short unique code like `BIKES`, `ACC` — used in URLs and business logic |
| `ParentCategoryId` | FK to itself — NULL means top-level category |
| `DisplayOrder` | Controls the order categories appear in the navigation menu |
| `IsActive` | Inactive categories are hidden from customers |

**Linked to:** `Product`

---

### `Brand`

**What it stores:** Bicycle and parts brands (e.g., Shimano, Sram, Trek, Polygon).

**Why it exists:** Brands are a key filter on the product listing page. Storing them in their own table avoids repeating the brand name in every product row and lets you add brand-level info like their website and country of origin.

**Key columns:**

| Column | Notes |
|--------|-------|
| `BrandName` | Unique |
| `Country` | Country of origin — useful for import/brand display |
| `Website` | Link to the brand's official site |

**Linked to:** `Product`

---

### `Product`

**What it stores:** The master listing for each product sold in the store — bikes, frames, components, and accessories.

**Why it exists:** This is the heart of the catalog. Every orderable item starts here.

**Key columns:**

| Column | Notes |
|--------|-------|
| `CategoryId` | Which category the product belongs to |
| `BrandId` | Nullable — some generic items have no brand |
| `SKU` | Stock-Keeping Unit — internal product code |
| `Price` | The base price before variant adjustments |
| `Currency` | Fixed-length `char(3)` — always "PHP" for this store |
| `WheelSize` | Bike-specific field: "26\"", "27.5\"", "29\"" |
| `SpeedCompatibility` | Drivetrain speeds: "1x12", "2x11" etc. |
| `BoostCompatible` | Whether the hub spacing is Boost (148mm rear) |
| `TubelessReady` | Whether the rims accept tubeless setup |
| `AxleStandard` | Thru-axle spec: "12x148", "15x110" etc. |
| `SuspensionTravel` | Fork travel in mm: "120mm", "160mm" |
| `BrakeType` | "Hydraulic Disc", "Mechanical Disc", "Rim" |
| `AdditionalSpecs` | Free-form JSON-like text for specs that don't have their own column |
| `IsActive` | Hidden from the storefront when `0` |
| `IsFeatured` | Shown on the homepage featured section |

**Why so many bike-specific columns?** Rather than storing all specs in a generic key-value table, the team chose dedicated columns for the most common bike specs. This makes filtering ("show 29\" wheels only") much simpler with regular SQL `WHERE` clauses.

**Linked to:** `Category`, `Brand`, `ProductVariant`, `ProductImage`, `OrderItem`, `CartItem`, `Wishlist`, `Review`, `InventoryLog`, `PriceHistory`

---

### `ProductVariant`

**What it stores:** Size or configuration variations of a product (e.g., "Small", "Medium", "Large" for a frame; or "Black", "White" for a jersey).

**Why it exists:** A single product listing can have multiple purchasable options that differ in price and stock. Without variants you would need a separate product row for each size — duplicating the name, description, images, and all specs. Variants keep the parent product row clean and add only what differs.

**Key columns:**

| Column | Notes |
|--------|-------|
| `ProductId` | Which product this is a variant of |
| `VariantName` | "Small", "Large", "Black/Red" — displayed in the size picker |
| `AdditionalPrice` | Added to the base `Product.Price` to get the total price |
| `StockQuantity` | Current inventory count for this specific variant |
| `ReorderThreshold` | An alert fires when stock falls to or below this number |

**Note:** Products without real variants still have one `ProductVariant` row with `VariantName = 'Default'`. This simplifies the code — stock is always read from `ProductVariant`, never from `Product` directly.

**Linked to:** `Product`, `OrderItem`, `CartItem`, `InventoryLog`, `PurchaseOrderItem`

---

### `ProductImage`

**What it stores:** One or more images for each product, stored as URLs pointing to a cloud storage bucket.

**Why it exists:** Products need photos. Separating images into their own table allows multiple images per product (gallery view) and lets you control which one is the primary display image.

**Key columns:**

| Column | Notes |
|--------|-------|
| `StorageBucket` | The GCS/S3 bucket name |
| `StoragePath` | Path within the bucket |
| `ImageUrl` | Full public URL to the image |
| `ImageType` | "primary", "gallery", "thumbnail" |
| `IsPrimary` | `1` for the main listing image |
| `DisplayOrder` | Sorting order in the gallery |
| `AltText` | Accessibility description |
| `FileSizeBytes`, `MimeType`, `Width`, `Height` | Metadata for the image file |
| `UploadedByUserId` | Which admin uploaded this |

**Linked to:** `Product`, `User`

---

## 5. Shopping cart and wishlist

### `Cart`

**What it stores:** An active shopping basket. One cart exists per user (or guest) until checkout.

**Why it exists:** A cart persists between sessions — if you add a bike to your cart on Monday and come back on Friday, it should still be there. The database cart also enables the admin to see abandoned carts for remarketing.

**Key columns:**

| Column | Notes |
|--------|-------|
| `UserId` | FK to `User` — NULL for guest carts |
| `GuestSessionId` | FK to `GuestSession` — NULL for logged-in carts |
| `IsCheckedOut` | `1` after the cart has been converted to an `Order`. Unique indexes prevent two active carts for the same user/guest. |

**Linked to:** `User`, `GuestSession`, `CartItem`, `Order`

---

### `CartItem`

**What it stores:** Individual line items inside a cart — each product + variant combination the customer has added.

**Why it exists:** Each product selected by the user needs its own row so quantities can be tracked per item.

**Key columns:**

| Column | Notes |
|--------|-------|
| `PriceAtAdd` | The price at the moment the item was added — protects against the customer gaming a price change |
| `AddedAt` | Timestamp for cart analytics ("how long does it take customers to check out?") |

**Linked to:** `Cart`, `Product`, `ProductVariant`

---

### `Wishlist`

**What it stores:** Products a logged-in user has saved for later.

**Why it exists:** Customers who are not ready to buy can bookmark products. It also gives the store insight into what items are most desired but not yet purchased.

**Key columns:**

| Column | Notes |
|--------|-------|
| `UserId` + `ProductId` | Unique pair — `UX_Wishlist_UserProduct` prevents duplicates |

**Linked to:** `User`, `Product`

---

## 6. Orders

### `Order`

**What it stores:** A confirmed purchase. An order is created when a cart is checked out (online) or when a sale is processed at the POS terminal.

**Why it exists:** This is the central record of a transaction. Everything financial and logistical hangs off an order: payments, delivery, refunds, reviews, and support tickets.

**Key columns:**

| Column | Notes |
|--------|-------|
| `OrderNumber` | Human-readable, unique code like `ORD-20260424-001`. Displayed on receipts. |
| `OrderStatus` | Lifecycle state: `Pending` → `Confirmed` → `Processing` → `ReadyForPickup`/`Shipped` → `Delivered`/`Completed` → `Cancelled`/`Refunded` |
| `SubTotal` | Sum of all `OrderItem.UnitPrice × Quantity` |
| `DiscountAmount` | Total discount applied (from vouchers) |
| `ShippingFee` | Delivery charge — zero for walk-in and pickup orders |
| `TotalAmount` | **Computed column**: `SubTotal - DiscountAmount + ShippingFee`. SQL Server calculates and stores this automatically. |
| `FulfillmentType` | `"Delivery"`, `"Pickup"`, or `"WalkIn"` |
| `IsWalkIn` | `1` for POS walk-in orders (no shipping, no account needed) |
| `ShippingAddressId` | FK to `Address` — the snapshot address for delivery orders |
| `CartId` | FK to the cart that was checked out (nullable — POS orders have no cart) |
| `GuestSessionId` | FK to `GuestSession` — set for guest checkouts |
| `POSSessionId` | FK to `POS_Session` — set for walk-in sales |
| `PaymentMethod` | Quick reference: `"GCash"`, `"BankTransfer"`, `"Cash"` |
| `IsDeleted` | Soft-delete — cancelled orders are not physically removed |

**Linked to:** `User`, `Address`, `Cart`, `GuestSession`, `POS_Session`, `OrderItem`, `Payment`, `Delivery`, `PickupOrder`, `VoucherUsage`, `SupportTicket`, `Refund`, `Review`, `Notification`, `InventoryLog`, `OrderStatusAudit`

---

### `OrderItem`

**What it stores:** Each product line in an order — what was bought, how many, and at what price.

**Why it exists:** An order can contain multiple products. A separate table with one row per line item is the standard relational approach (called a "one-to-many" relationship from `Order` to `OrderItem`).

**Key columns:**

| Column | Notes |
|--------|-------|
| `UnitPrice` | The price at the time of purchase — frozen so that future price changes do not alter the order history |
| `ProductVariantId` | Nullable — NULL if the product has no variants |

**Linked to:** `Order`, `Product`, `ProductVariant`

---

### `PickupOrder`

**What it stores:** Extra details for orders where the customer will collect the item from the store (instead of delivery).

**Why it exists:** Delivery orders need courier tracking; pickup orders need a "ready for collection" window. Rather than adding nullable delivery/pickup columns to `Order` (which would pollute every row), this table extends only the rows that are pickup orders. This is called **table-per-type inheritance**.

**Key columns:**

| Column | Notes |
|--------|-------|
| `OrderId` | One-to-one with `Order` — enforced by unique constraint `UX_PickupOrder_Order` |
| `PickupReadyAt` | When staff marked the order as ready for collection |
| `PickupExpiresAt` | Deadline by which the customer must collect (after this, the order may be cancelled) |
| `PickupConfirmedAt` | When the customer actually picked up the item |

**Linked to:** `Order`

---

## 7. Delivery

### `Delivery`

**What it stores:** Tracking information for orders shipped to a customer's address.

**Why it exists:** Like `PickupOrder`, this is a table-per-type extension. Only delivery orders need a courier and tracking status. Each order has at most one delivery record.

**Key columns:**

| Column | Notes |
|--------|-------|
| `OrderId` | Unique FK to `Order` |
| `Courier` | `"Lalamove"` or `"LBC"` — determines which sub-table has extra detail |
| `DeliveryStatus` | `"Pending"`, `"PickedUp"`, `"InTransit"`, `"Delivered"`, `"Failed"` |
| `IsDelayed` | `1` if the delivery is behind schedule |
| `DelayedUntil` | New expected delivery date when delayed |
| `EstimatedDeliveryTime` | Original ETA |
| `ActualDeliveryTime` | When it was actually delivered |

**Linked to:** `Order`, `LalamoveDelivery`, `LBCDelivery`

---

### `LalamoveDelivery`

**What it stores:** Lalamove-specific data for deliveries made through that courier.

**Why it exists:** Lalamove provides a driver name, phone number, and booking reference. These fields are meaningless for LBC orders. Storing them in a separate table means the `Delivery` base table stays clean.

**Key columns:**

| Column | Notes |
|--------|-------|
| `DeliveryId` | Shares the PK with `Delivery` (one-to-one) |
| `BookingRef` | Lalamove booking ID |
| `DriverName`, `DriverPhone` | Assigned driver's details |

**Linked to:** `Delivery`

---

### `LBCDelivery`

**What it stores:** LBC courier-specific data for deliveries shipped via LBC Express.

**Why it exists:** Same reason as `LalamoveDelivery`. LBC uses a tracking number system instead of a driver assignment.

**Key columns:**

| Column | Notes |
|--------|-------|
| `DeliveryId` | One-to-one with `Delivery` |
| `TrackingNumber` | The LBC tracking number customers can use on lbcexpress.com |

**Linked to:** `Delivery`

---

## 8. Payments

### `Payment`

**What it stores:** A payment record for an order. Multiple payments can exist for one order (e.g., split payment or a refund followed by a new payment).

**Why it exists:** Payments are a financial record and must be stored independently of orders. An order can have multiple payment attempts (first attempt failed, second succeeded). The `PaymentStage` column tracks where in the payment lifecycle each record sits.

**Key columns:**

| Column | Notes |
|--------|-------|
| `PaymentMethod` | `"GCash"`, `"BankTransfer"`, `"Cash"` |
| `PaymentStage` | `"Deposit"` or `"FullPayment"` — useful for installment or pre-order flows |
| `PaymentStatus` | `"Pending"`, `"Verified"`, `"Failed"`, `"Refunded"` |
| `Amount` | How much was paid |
| `PaidToAccountName/Number/BankName` | Snapshot of which store account received the payment |
| `StorePaymentAccountId` | FK to `StorePaymentAccount` — the account the customer paid into |

**Linked to:** `Order`, `StorePaymentAccount`, `GCashPayment`, `BankTransferPayment`, `Refund`

---

### `GCashPayment`

**What it stores:** GCash-specific proof of payment for a payment record.

**Why it exists:** When a customer pays via GCash, they upload a screenshot of the transaction. These fields (screenshot URL, transaction ID, storage path) are only relevant for GCash payments. Using a separate table avoids polluting the base `Payment` table with columns that are always NULL for cash and bank transfer payments.

**Key columns:**

| Column | Notes |
|--------|-------|
| `GcashTransactionId` | The reference number shown in the GCash app — must be unique |
| `ScreenshotUrl` | Public URL to the uploaded screenshot |
| `StorageBucket`, `StoragePath` | Cloud storage location of the proof image |
| `SubmittedAt` | When the customer uploaded the proof |

**Linked to:** `Payment`

---

### `BankTransferPayment`

**What it stores:** Bank transfer-specific proof and verification data for a payment.

**Why it exists:** Bank transfers need more verification workflow than GCash: a BPI reference number, a proof image, and a verification deadline with admin notes. This data is irrelevant for GCash or cash payments.

**Key columns:**

| Column | Notes |
|--------|-------|
| `BpiReferenceNumber` | Unique bank reference — system rejects duplicate submissions |
| `ProofUrl`, `ProofStorageBucket`, `ProofStoragePath` | Uploaded bank deposit slip |
| `VerifiedByUserId` | Which admin staff member approved this payment |
| `VerificationNotes` | Admin's notes when approving or rejecting |
| `VerificationDeadline` | If not verified by this date, the order may be cancelled |
| `VerifiedAt`, `SubmittedAt` | Audit timestamps |

**Linked to:** `Payment`, `User`

---

### `StorePaymentAccount`

**What it stores:** The store's own GCash accounts and bank accounts that customers send money to.

**Why it exists:** The store may have multiple GCash numbers and bank accounts. This table lets admin staff add, edit, or deactivate payment accounts without any code changes. The checkout page reads this table to show customers where to send money.

**Key columns:**

| Column | Notes |
|--------|-------|
| `PaymentMethod` | `"GCash"`, `"BPI"`, etc. |
| `AccountName`, `AccountNumber` | The account details shown to customers |
| `BankName` | For bank transfer accounts |
| `QrImageUrl` | QR code image that customers scan in the GCash app |
| `Instructions` | Step-by-step payment instructions shown to customers |
| `DisplayOrder` | Order in which accounts appear at checkout |
| `IsActive` | Inactive accounts are hidden |

**Linked to:** `Payment`

---

### `Refund`

**What it stores:** A refund request and its approval status.

**Why it exists:** Refunds are separate from payments because they go in the opposite direction (money leaves the store). They also need an approval workflow — a staff member must verify and approve before money is returned.

**Key columns:**

| Column | Notes |
|--------|-------|
| `OrderId` | Which order is being refunded |
| `PaymentId` | Which specific payment to reverse (nullable — some refunds are manual) |
| `RefundAmount` | May be less than the full payment (partial refund) |
| `RefundReason` | Customer's stated reason |
| `RefundStatus` | `"Pending"`, `"Approved"`, `"Processed"`, `"Rejected"` |
| `RefundMethod` | How the money is returned: `"GCash"`, `"BankTransfer"`, `"StoreCredit"` |
| `RequestedByUserId` | Customer or staff who requested the refund |
| `ApprovedByUserId` | Staff who approved it |
| `TicketId` | FK to `SupportTicket` if the refund came through a support request |

**Linked to:** `Order`, `Payment`, `User`, `SupportTicket`

---

## 9. Vouchers and discounts

### `Voucher`

**What it stores:** Discount codes that customers can enter at checkout.

**Why it exists:** Vouchers are a common marketing tool. Storing them in the database lets admin staff create, expire, and track usage without any code deployments.

**Key columns:**

| Column | Notes |
|--------|-------|
| `Code` | The unique code customers type in (e.g., `SUMMER20`) |
| `DiscountType` | `"Percentage"` or `"Fixed"` |
| `DiscountValue` | The amount: 20 means 20% off or ₱20 off depending on type |
| `MinimumOrderAmount` | Voucher only applies if the cart subtotal meets this floor |
| `MaxUses` | Total number of times this code can be used across all customers |
| `MaxUsesPerUser` | How many times a single customer can use this code |
| `StartDate`, `EndDate` | Validity window |

**Linked to:** `UserVoucher`, `VoucherUsage`

---

### `UserVoucher`

**What it stores:** Vouchers that have been assigned to specific users (private, targeted vouchers).

**Why it exists:** Some vouchers are public (anyone who knows the code can use them). Others are issued personally to a customer — a birthday voucher, a loyalty reward. `UserVoucher` tracks those personal assignments. The unique constraint `UX_UserVoucher_Pair` ensures a voucher is not double-assigned to the same user.

**Key columns:**

| Column | Notes |
|--------|-------|
| `UserId` + `VoucherId` | Unique pair |
| `AssignedAt` | When the voucher was given to this user |
| `ExpiresAt` | Can override the voucher's general `EndDate` for this user |

**Linked to:** `User`, `Voucher`

---

### `VoucherUsage`

**What it stores:** A record of every time a voucher was actually applied to an order.

**Why it exists:** This is the audit log for voucher redemptions. It allows the system to check whether a user has hit the per-user limit (`MaxUsesPerUser`) and to track total redemptions globally.

**Key columns:**

| Column | Notes |
|--------|-------|
| `VoucherId`, `UserId`, `OrderId` | Tri-key identifying exactly which voucher, who used it, and on which order |
| `DiscountAmount` | The actual money saved — may differ from the voucher's face value if the cart total was lower |
| `UsedAt` | Exact timestamp of redemption |

**Linked to:** `Voucher`, `User`, `Order`

---

## 10. Inventory and suppliers

### `Supplier`

**What it stores:** Companies the store buys products from.

**Why it exists:** When stock runs low, the store places purchase orders with suppliers. This table records who those suppliers are and how to contact them.

**Key columns:**

| Column | Notes |
|--------|-------|
| `Name` | Supplier company name |
| `ContactPerson` | The person at the supplier to call |
| `PhoneNumber`, `Email` | Contact details |
| `Address` | Supplier's physical address |

**Linked to:** `PurchaseOrder`

---

### `PurchaseOrder`

**What it stores:** An order placed by the store to a supplier to restock inventory.

**Why it exists:** This is the buy side of the business. A customer-facing `Order` is a sale; a `PurchaseOrder` is a purchase of stock. The system tracks these to reconcile expected incoming inventory.

**Key columns:**

| Column | Notes |
|--------|-------|
| `SupplierId` | Which supplier |
| `OrderDate` | When the PO was placed |
| `ExpectedDeliveryDate` | When the supplier says stock will arrive |
| `Status` | `"Draft"`, `"Sent"`, `"Received"`, `"Cancelled"` |
| `CreatedByUserId` | Which staff member placed the order |

**Linked to:** `Supplier`, `PurchaseOrderItem`, `InventoryLog`, `User`

---

### `PurchaseOrderItem`

**What it stores:** Line items in a purchase order — each product/variant being ordered from the supplier.

**Why it exists:** Same pattern as `OrderItem` — one row per line item avoids storing a list inside the header record.

**Key columns:**

| Column | Notes |
|--------|-------|
| `ProductId`, `ProductVariantId` | What is being restocked |
| `Quantity` | How many units ordered |
| `UnitPrice` | Purchase cost from the supplier |

**Linked to:** `PurchaseOrder`, `Product`, `ProductVariant`

---

### `InventoryLog`

**What it stores:** A complete history of every stock change for every product variant.

**Why it exists:** The current stock level is stored in `ProductVariant.StockQuantity`. But knowing *why* it changed is equally important. Was stock reduced because of a sale? Added because of a supplier delivery? Manually adjusted by staff? `InventoryLog` answers these questions and lets you reconstruct the stock history at any point in time.

**Key columns:**

| Column | Notes |
|--------|-------|
| `InventoryLogId` | `bigint` — this table grows fast, needs a large PK |
| `ChangeQuantity` | Positive = stock in, Negative = stock out |
| `ChangeType` | `"Sale"`, `"PurchaseReceived"`, `"ManualAdjustment"`, `"Damaged"`, `"Return"` |
| `OrderId` | FK if the change was caused by a customer order |
| `PurchaseOrderId` | FK if the change was caused by receiving supplier stock |
| `ChangedByUserId` | Staff who made a manual adjustment |
| `Notes` | Free text explanation |

**Linked to:** `Product`, `ProductVariant`, `Order`, `PurchaseOrder`, `User`

---

### `PriceHistory`

**What it stores:** A log of every price change made to a product.

**Why it exists:** Prices change over time for promotions, cost adjustments, or mistakes. This table lets the admin see price history ("when did we raise the price of this bike?") and lets the system audit any suspicious changes.

**Key columns:**

| Column | Notes |
|--------|-------|
| `ProductId` | Which product was changed |
| `OldPrice`, `NewPrice` | Before and after values |
| `ChangedAt` | When it happened |
| `ChangedByUserId` | Who did it |
| `Notes` | Reason for the change |

**Linked to:** `Product`, `User`

---

## 11. Point of sale (POS)

### `POS_Session`

**What it stores:** A cashier's shift at a physical POS terminal — start time, end time, and total sales processed.

**Why it exists:** The store has a physical counter where customers can walk in and buy. Each time a staff member opens the POS and starts taking sales, a session is created. When they close up, the session ends and the total is recorded. This allows end-of-day cash reconciliation and per-staff-member sales reporting.

**Key columns:**

| Column | Notes |
|--------|-------|
| `UserId` | The staff member who opened this session |
| `TerminalName` | Which physical terminal: `"Counter 1"`, `"Counter 2"` |
| `ShiftStart`, `ShiftEnd` | Duration of the shift |
| `TotalSales` | Sum of all walk-in orders processed in this session |

**Linked to:** `User`, `Order`

---

## 12. Customer support

### `SupportTicket`

**What it stores:** A customer complaint, inquiry, or request submitted to the store's support team.

**Why it exists:** When a customer has a problem (missing delivery, wrong product, refund request) they need a tracked channel to communicate. A support ticket system ensures nothing falls through the cracks, can be assigned to a staff member, and has a clear status.

**Key columns:**

| Column | Notes |
|--------|-------|
| `UserId` | The customer who submitted the ticket |
| `OrderId` | Nullable — many tickets are about a specific order |
| `TicketSource` | `"Web"`, `"Email"`, `"Phone"`, `"WalkIn"` |
| `TicketCategory` | `"Delivery"`, `"Refund"`, `"Product"`, `"Technical"` |
| `Subject`, `Description` | What the customer wrote |
| `AttachmentUrl` | An uploaded photo/document |
| `TicketStatus` | `"Open"`, `"InProgress"`, `"Resolved"`, `"Closed"` |
| `AssignedToUserId` | Which staff member is handling it |
| `ResolvedAt` | When it was marked resolved |

**Linked to:** `User`, `Order`, `SupportTicketReply`, `SupportTask`, `Refund`, `Notification`

---

### `SupportTicketReply`

**What it stores:** Individual messages in the back-and-forth conversation on a support ticket.

**Why it exists:** Support is a conversation. Both the customer and staff need to be able to post messages, and all messages must be preserved in order with timestamps.

**Key columns:**

| Column | Notes |
|--------|-------|
| `ReplyId` | `bigint` — conversations can be long |
| `IsAdminReply` | `1` = the message came from staff, `0` = from the customer |
| `Message` | Full message text (`nvarchar(max)`) |
| `AttachmentUrl` | Optional attached image or document |

**Linked to:** `SupportTicket`, `User`

---

### `SupportTask`

**What it stores:** Actionable tasks created from a support ticket and assigned to a specific staff member.

**Why it exists:** A ticket may require multiple internal actions — "contact the courier", "process the refund", "send a replacement". Tasks break a ticket into trackable sub-actions with deadlines.

**Key columns:**

| Column | Notes |
|--------|-------|
| `TicketId` | Which ticket spawned this task |
| `AssignedToUserId` | Staff responsible |
| `TaskType` | `"ContactCourier"`, `"ProcessRefund"`, `"SendReplacement"` |
| `TaskStatus` | `"Pending"`, `"InProgress"`, `"Completed"`, `"Cancelled"` |
| `DueDate` | Deadline for the task |
| `CompletedAt` | When it was finished |

**Linked to:** `SupportTicket`, `User`

---

## 13. Notifications

### `Notification`

**What it stores:** A log of every notification sent (or attempted to be sent) to a user — order status emails, OTP SMS, delivery alerts.

**Why it exists:** When the system sends an email or SMS, something can go wrong (the email server is down, the SMS number is invalid). Storing notifications in the database with a retry count and failure reason lets the system retry failed notifications without losing them. It also serves as a delivery audit log.

**Key columns:**

| Column | Notes |
|--------|-------|
| `Channel` | `"Email"`, `"SMS"`, `"Push"` |
| `NotifType` | `"OrderConfirmed"`, `"OrderShipped"`, `"OTPCode"`, `"TicketReply"` |
| `Recipient` | The email address or phone number |
| `Subject`, `Body` | The content of the notification |
| `Status` | `"Pending"`, `"Sent"`, `"Failed"` |
| `RetryCount` | How many times the system has tried to send this |
| `SentAt` | When it succeeded |
| `FailureReason` | Error message if it failed |
| `IsRead`, `ReadAt` | For in-app notifications shown in the UI |

**Linked to:** `User`, `Order`, `SupportTicket`

---

## 14. Reviews

### `Review`

**What it stores:** Star ratings and written comments left by customers about products they purchased.

**Why it exists:** Reviews build customer trust and help other shoppers make purchasing decisions. Requiring an `OrderId` ensures only people who actually bought the product can leave a review.

**Key columns:**

| Column | Notes |
|--------|-------|
| `UserId` | Who wrote the review |
| `ProductId` | Which product is being reviewed |
| `OrderId` | Which purchase backs this review — prevents fake reviews |
| `Rating` | Integer 1–5 |
| `Comment` | Optional written review |
| `IsVerifiedPurchase` | `1` = system confirmed the reviewer actually bought this product in the linked order |

**Linked to:** `User`, `Product`, `Order`

---

## 15. Audit and system logs

### `OrderStatusAudit`

**What it stores:** Every state transition attempted on an order — both successful and failed ones.

**Why it exists:** Orders move through a strict lifecycle. When a status change is attempted (e.g., marking an order as "Shipped"), it either succeeds or is rejected (e.g., trying to ship a cancelled order). Logging every attempt — including failures — lets you audit exactly what happened and debug disputes ("why was this order cancelled?").

**Key columns:**

| Column | Notes |
|--------|-------|
| `AuditId` | `bigint` |
| `OrderId` | Which order changed |
| `FromStatus`, `ToStatus` | Before and after states |
| `Success` | `1` = transition was allowed, `0` = it was rejected |
| `Reason` | Why the transition was rejected (if `Success = 0`) |

**Linked to:** `Order`

---

### `SystemLog`

**What it stores:** General-purpose application events — logins, configuration changes, errors.

**Why it exists:** A system log is the "black box" of the application. When something goes wrong, the first place to look is the system log. Unlike `OrderStatusAudit` which is specific to orders, `SystemLog` captures anything the application considers worth recording.

**Key columns:**

| Column | Notes |
|--------|-------|
| `SystemLogId` | `bigint` |
| `UserId` | Nullable — some events happen without a user (e.g., scheduled jobs) |
| `EventType` | Short code: `"UserLogin"`, `"PasswordReset"`, `"ProductUpdated"` |
| `EventDescription` | Detailed message |

**Linked to:** `User`

---

## 16. Database views

Views are saved `SELECT` queries that you can use like tables. They pre-join related tables so you don't have to write complex `JOIN` statements every time. **Views are read-only** — you cannot `INSERT` or `UPDATE` through them.

| View | What it returns | Tables joined |
|------|----------------|---------------|
| `vw_OrderSummary` | One row per order with customer name, address, pickup/delivery times, and item counts | `Order`, `User`, `Address`, `PickupOrder`, `OrderItem` |
| `vw_OrderItemDetail` | Each order line with product name, variant name, and computed subtotal | `OrderItem`, `Product`, `ProductVariant` |
| `vw_PaymentDetail` | Payment record with GCash and bank transfer details unioned into one row | `Payment`, `GCashPayment`, `BankTransferPayment` |
| `vw_ActiveProducts` | All active products with their category, brand, and primary image URL | `Product`, `Category`, `Brand`, `ProductImage`, `ProductVariant` |
| `vw_ProductImageGallery` | All images for active products with storage paths | `ProductImage`, `Product` |
| `vw_ProductVariantDetails` | Variants with their computed total price (base + additional) | `ProductVariant`, `Product` |
| `vw_InventoryStatus` | Current total stock per product (sum across all variants) | `Product`, `Category`, `Brand`, `ProductVariant` |
| `vw_UserVoucherUsageCount` | How many times each user has used each voucher | `VoucherUsage` |
| `vw_PendingNotifications` | Unsent notifications with ≤ 3 retries for active users | `Notification`, `User` |
| `vw_OpenSupportTickets` | Unresolved tickets with customer and assigned-staff info | `SupportTicket`, `User`, `Order` |
| `vw_DeliveryDetail` | Delivery record with Lalamove and LBC specifics in one flat row | `Delivery`, `LalamoveDelivery`, `LBCDelivery` |
| `vw_PurchaseOrderDetail` | Purchase order header with supplier name, total amount, and line count | `PurchaseOrder`, `Supplier`, `PurchaseOrderItem` |

---

## 17. Design patterns explained

Understanding these recurring patterns will help you read any part of the schema.

### Soft delete (`IsDeleted`)

Instead of `DELETE FROM [User]` (which would break every FK that points to that user), the system sets `IsDeleted = 1`. All queries filter `WHERE IsDeleted = 0`. This preserves referential integrity and lets you recover accidentally deleted records.

**Tables that use it:** `User`, `Order`, `Payment`

---

### Address snapshots (`IsSnapshot`)

When an order is placed, the delivery address is copied into a new `Address` row with `IsSnapshot = 1`. The order points to this frozen copy. If the user later changes their real address, old orders are unaffected.

---

### Table-per-type inheritance

`Delivery` is the base table. `LalamoveDelivery` and `LBCDelivery` extend it — they share the same `DeliveryId` PK, which acts as a one-to-one FK to `Delivery`. The same pattern applies to `Payment` → `GCashPayment` / `BankTransferPayment`, and `Order` → `PickupOrder`.

This avoids putting 10 courier-specific nullable columns into the base `Delivery` table, which would be confusing and wasteful.

---

### Computed columns

`Order.TotalAmount` is defined as `(SubTotal - DiscountAmount) + ShippingFee` with `PERSISTED`. SQL Server calculates it automatically whenever the row is written — you cannot insert a wrong total, and you can query it without arithmetic.

---

### Price at purchase time

Both `CartItem.PriceAtAdd` and `OrderItem.UnitPrice` store the price at the moment the item was added/purchased. This prevents the order history from changing if an admin later updates a product's price.

---

## 18. Entity-relationship summary

```
User ──────────── UserRole ──────── Role
 │
 ├── Address ◄─── Order ──────────── OrderItem ──── Product ─── Category
 │                  │                                  │           └── (parent)
 │                  ├── PickupOrder               ProductVariant
 │                  ├── Delivery ──┬── LalamoveDelivery    Brand
 │                  │             └── LBCDelivery
 │                  ├── Payment ──┬── GCashPayment
 │                  │            └── BankTransferPayment
 │                  ├── VoucherUsage ── Voucher ◄── UserVoucher
 │                  ├── Refund
 │                  ├── Review
 │                  └── SupportTicket ──┬── SupportTicketReply
 │                                      └── SupportTask
 │
 ├── ActiveSession
 ├── Cart ─── CartItem
 ├── Wishlist
 ├── Notification
 ├── POS_Session
 └── OTPVerification

GuestSession ─── Cart
               └── Order

Supplier ─── PurchaseOrder ─── PurchaseOrderItem
                              │
InventoryLog ◄────────────────┘
PriceHistory ◄──── Product
OrderStatusAudit ◄── Order
SystemLog
StorePaymentAccount ◄── Payment
```

---

## 19. Study questions

Use these questions to test whether you understand the database. Try to answer each one using only SQL before looking at the schema.

### Beginner

1. **What is the difference between `IsActive` and `IsDeleted` on the `User` table?** When would a user have `IsActive = 0` but `IsDeleted = 0`?

2. **A customer buys three products in one order. How many rows appear in `OrderItem` for that transaction?**

3. **What does `IsPrimary` mean in `ProductImage`? Can a product have zero primary images? What would happen on the storefront?**

4. **Why does `CartItem` store `PriceAtAdd` instead of just reading the price from `Product`?**

5. **What is the difference between `Voucher.MaxUses` and `Voucher.MaxUsesPerUser`? Give a real example.**

6. **A user changes their home address after placing an order. Will the order still show the old address? Why or why not?**

### Intermediate

7. **Write a SQL query that returns the full name and email of every user who has the role `'Admin'`.**

8. **Write a query to find all products with stock below their reorder threshold.** (Hint: `ProductVariant.StockQuantity < ProductVariant.ReorderThreshold`)

9. **An order has `FulfillmentType = 'Delivery'`. Which tables would you join to get the courier name and tracking number in a single result row?**

10. **Why does `GCashPayment.GcashTransactionId` have a unique index instead of a unique constraint? What is the difference?** (Hint: look at the `WHERE` clause on the index.)

11. **What is the purpose of `OrderStatusAudit.Success`? Give an example of a failed status transition that would be logged with `Success = 0`.**

12. **A walk-in customer buys a bike without an account. Trace through the tables that are touched from POS session open to order creation.** Which tables are involved? Which foreign keys connect them?

### Advanced

13. **`Address.IsSnapshot` separates live addresses from order-time copies. Could you achieve the same result without `IsSnapshot` by using a separate `OrderAddress` table? What are the trade-offs?**

14. **`TotalAmount` in `Order` is a persisted computed column. What are the advantages of `PERSISTED` vs. calculating it in every query? Are there any disadvantages?**

15. **The system supports both GCash and bank transfer payments. A new payment method (e.g., Maya) needs to be added that has its own unique fields. What changes would you make to the schema?**

16. **`InventoryLog` uses `bigint` for its PK while most other tables use `int`. Why is this a good design decision?**

17. **A voucher has `MaxUses = 100` and `MaxUsesPerUser = 1`. How would you write a SQL query to check, before applying a voucher, whether it is still valid for a given user?** (Consider: has the global limit been hit? Has this user already used it?)

18. **What is the risk if you delete a `User` row (hard delete) instead of using `IsDeleted = 1`? Name at least three tables that would be affected and explain why.**

19. **`GuestSession.ConvertedToUserId` links a guest to the account they later created. If you were building the "merge cart" feature, what SQL steps would you take to transfer a guest's open cart to their new account?**

20. **`Notification.RetryCount` is capped at 3 in the `vw_PendingNotifications` view. Why is a retry cap important? What would happen without one?**
