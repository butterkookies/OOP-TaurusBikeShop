# **Taurus Bike Shop Ordering System**

### *A Case Study Document*

**Presented to the subjects of:**

* Object-Oriented Programming
* Database Management 1

**In Partial Fulfillment of the Requirements for the Degree of**
**Bachelor of Science in Information Technology**

---

### **Submitted to:**

Ms. Krizia Genovia
Mr. Lionel Cambe

### **Submitted by:**

Dancel, Troy Lits D.
Capisonda, Jhon Edrian
Celon, Brian Howard U.
Geronimo, Andrei John P.
Maula, John Lawrence F.
Ramos, Ervin James D.M.

### **Date of Submission:**

*April 17, 2026*

---

# **Table of Contents**

1. Introduction
     1.1 Background
     1.2 Scope and Limitations
     1.3 Key Terms

2. Design and Methodology
     2.1 Database Design
     2.2 Entity-Relationship Diagram (ERD)
     2.3 Data Dictionary (Per Table)
     2.4 Storyboard / System Flow

3. SQL Command Extraction (Real Queries)

4. System Capabilities and Limitations

5. Unique Features of the System

6. Tech Stack (Auto-Detected)

7. References

---

# **1. Introduction**

This section presents the background of the study, including the problem statement, objectives, scope and limitations, as well as key terms used throughout the document. It provides an overview of the system and its intended purpose.

---

## **1.1 Background**

The **Taurus Bike Shop Ordering System (TBSOS)** was developed to address the absence of an online platform and a dedicated Point of Sale (POS) system for Taurus Bike Shop. In today's digital era, having an online presence is essential as a significant portion of transactions now occur online. The system provides an alternative channel for the business to expand its market reach and increase potential customers.

Currently, Taurus Bike Shop relies on handwritten receipts for recording transactions. This method is prone to human errors, miscalculations, and inefficiencies, which affect the accuracy of sales reports and inventory tracking. By implementing a POS system, these issues are minimized through automation and centralized data management, ensuring accuracy, reliability, and efficiency.

The system consists of two sub-systems that share a single MSSQL database:

1. **WebApplication** — An ASP.NET Core 8 MVC web storefront used by customers for browsing products, placing orders, submitting payments, managing wishlists, writing reviews, and filing support tickets.
2. **AdminSystem_v2** — A WPF desktop application used by store staff for order management, POS walk-in sales, product/inventory management, staff management, voucher management, report generation, and payment verification.

Overall, the system aims to modernize business operations, improve transaction handling, and enhance customer interaction.

---

## **1.2 Scope and Limitations**

**Scope:**

* Handles online customer order processing (delivery, pickup, and walk-in via POS)
* Manages product catalog with categories, brands, variants, and image galleries
* Supports three payment methods: GCash, BankTransfer, and Cash (POS-only)
* Tracks inventory through variant-level stock quantities and audit logs
* Includes a customer support ticket system with threaded replies
* Provides a voucher/discount system with per-user assignment and usage caps
* Offers wishlist functionality with move-to-cart capability
* Generates sales and inventory reports via the Admin desktop application
* Supports OTP-based email verification during customer registration
* Maintains order status audit trails for accountability
* Sends multi-channel notifications (Email, SMS, InApp) via background jobs

**Limitations:**

* No mobile application support — web storefront is responsive but not a native app
* No real-time payment gateway integration — payments are verified manually by admin staff
* Cash payments are limited to POS walk-in transactions only
* Guest checkout is not fully implemented — guests can add to cart but must register to place an order
* Delivery tracking relies on manual admin updates rather than real-time courier API integration
* Report generation is limited to the desktop admin application
* No multi-currency support at checkout — Philippine Peso (PHP) is the primary currency used

---

## **1.3 Key Terms**

* **Taurus Bike Shop Ordering System (TBSOS):** A dual-platform system used to manage sales, inventory, and customer orders for a bike shop.
* **Point of Sale (POS):** Software used to process walk-in transactions and manage in-store sales.
* **Database:** Structured collection of digital data stored in Microsoft SQL Server.
* **IDE (Integrated Development Environment):** Software for coding, debugging, and testing.
* **Microsoft Visual Studio:** Development environment used for building both sub-systems.
* **ASP.NET Core 8 MVC:** Web framework used for building the online storefront.
* **WPF (Windows Presentation Foundation):** Framework for building the desktop admin UI.
* **C#:** Programming language used for all system logic across both sub-systems.
* **Entity Framework Core (EF Core):** ORM used by the WebApplication for database access.
* **Dapper:** Micro-ORM used by AdminSystem_v2 for high-performance database access.
* **Microsoft SQL Server:** Relational database management system used for backend storage.
* **Google Cloud Storage (GCS):** Cloud storage used for product images and payment proof uploads.
* **Cloudinary:** Cloud-based image management service used as an alternative image hosting provider.
* **BCrypt:** Cryptographic hashing algorithm used for password storage.
* **OTP (One-Time Password):** Time-limited verification code sent via email/SMS during registration.
* **MVVM (Model-View-ViewModel):** Architectural pattern used in the WPF admin application.
* **MVC (Model-View-Controller):** Architectural pattern used in the ASP.NET Core web application.
* **Version Control:** System for tracking code changes.
* **Git:** Version control system.
* **GitHub:** Platform for collaboration and repository management.

---

# **2. Design and Methodology**

---

## **2.1 Database Design**

The Taurus Bike Shop database (`Taurus-bike-shop-sqlserver-2026`) is hosted on Microsoft SQL Server and shared between both sub-systems. It contains **42 application tables** (plus 1 EF Core migrations tracking table) and **12 database views** that pre-join commonly queried data.

The database is structured around the following core domains:

### **User & Authentication Domain**
The `User` table is the central identity table, supporting both registered customers and a special "walk-in" user for POS transactions. Authentication is secured via BCrypt password hashing. Users are assigned roles through the `UserRole` junction table linking to the `Role` table. The `ActiveSession` table tracks login sessions, while `OTPVerification` stores time-limited verification codes used during registration. The system supports soft-deletion via the `IsDeleted` flag on the `User` table.

### **Product & Catalog Domain**
Products are organized by `Category` (which supports hierarchical parent-child nesting) and `Brand`. Each product can have multiple `ProductVariant` records that hold variant-specific SKUs, additional pricing, and stock quantities. Product images are stored in the `ProductImage` table with references to Google Cloud Storage buckets and paths. Price changes are tracked in `PriceHistory` for audit purposes. Customer reviews are stored in the `Review` table, linked to both the product and the verified purchase order.

### **Shopping & Cart Domain**
The `Cart` table supports both authenticated users and guest sessions (via `GuestSession`). Cart ownership is enforced by a CHECK constraint ensuring mutual exclusivity between `UserId` and `GuestSessionId`. Each `CartItem` captures the product, variant, quantity, and the price at the time of adding (for price-change detection).

### **Order & Fulfillment Domain**
The `Order` table is the system's transactional core. It supports three fulfillment types: Delivery, Pickup, and WalkIn. `TotalAmount` is a computed persisted column calculated as `(SubTotal - DiscountAmount) + ShippingFee`. Each order has line items in `OrderItem`. Delivery orders are tracked through the `Delivery` table with subtype-specific tables `LalamoveDelivery` and `LBCDelivery` (Table-Per-Type inheritance). Pickup orders use the `PickupOrder` table with ready/expiry/confirmed timestamps.

### **Payment Domain**
The `Payment` table records all payment transactions with support for staged payments (Upfront and Confirmation). It uses Table-Per-Type inheritance with `GCashPayment` and `BankTransferPayment` subtypes for method-specific proof storage (screenshots, reference numbers, verification status). The `StorePaymentAccount` table stores the shop's receiving account details displayed to customers during checkout.

### **Voucher & Discount Domain**
The `Voucher` table defines promotional codes with configurable discount types (Percentage or Fixed), date ranges, minimum order amounts, and usage caps (global and per-user). `UserVoucher` assigns specific vouchers to users, while `VoucherUsage` records each redemption.

### **Support & Notification Domain**
The `SupportTicket` table tracks customer issues with category tagging, order linking, and admin assignment. Threaded conversations are stored in `SupportTicketReply`. Admin task management is tracked in `SupportTask`. The `Notification` table handles multi-channel (Email, SMS, InApp) notifications with retry logic and read tracking.

### **Inventory & Audit Domain**
Stock levels live in `ProductVariant.StockQuantity`. The `InventoryLog` table creates an immutable audit trail of all stock changes (Sale, Purchase, Adjustment, Return, Damage, Loss, Lock, Unlock). The `OrderStatusAudit` table logs every order status transition attempt (both successful and rejected) for accountability. `SystemLog` provides a general-purpose event logging table. The `POS_Session` table tracks cashier shifts with total sales.

### **Supply Chain Domain**
The `Supplier` table manages vendor information. `PurchaseOrder` and `PurchaseOrderItem` track procurement from suppliers, linked to products/variants and the creating admin user.

---

## **2.2 Entity-Relationship Diagram (ERD)**

```mermaid
erDiagram
    User {
        int UserId PK
        nvarchar Email
        nvarchar PasswordHash
        nvarchar FirstName
        nvarchar LastName
        nvarchar PhoneNumber
        int DefaultAddressId FK
        bit IsActive
        bit IsWalkIn
        bit IsDeleted
        datetime2 CreatedAt
    }

    Role {
        int RoleId PK
        nvarchar RoleName
        nvarchar Description
    }

    UserRole {
        int UserRoleId PK
        int UserId FK
        int RoleId FK
        datetime2 AssignedAt
    }

    Address {
        int AddressId PK
        int UserId FK
        nvarchar Label
        nvarchar Street
        nvarchar City
        nvarchar PostalCode
        nvarchar Province
        nvarchar Country
        bit IsDefault
        bit IsSnapshot
    }

    ActiveSession {
        int SessionId PK
        int UserId FK
        nvarchar SessionToken
        bit IsRevoked
        datetime2 ExpiresAt
    }

    OTPVerification {
        int OTPId PK
        nvarchar Email
        nvarchar OTPCode
        bit IsUsed
        datetime2 ExpiresAt
    }

    Category {
        int CategoryId PK
        int ParentCategoryId FK
        nvarchar Name
        nvarchar CategoryCode
        bit IsActive
        int DisplayOrder
    }

    Brand {
        int BrandId PK
        nvarchar BrandName
        nvarchar Country
        bit IsActive
    }

    Product {
        int ProductId PK
        int CategoryId FK
        int BrandId FK
        varchar SKU
        nvarchar Name
        decimal Price
        char Currency
        bit IsActive
        bit IsFeatured
    }

    ProductVariant {
        int ProductVariantId PK
        int ProductId FK
        nvarchar VariantName
        nvarchar SKU
        decimal AdditionalPrice
        int StockQuantity
        int ReorderThreshold
        bit IsActive
    }

    ProductImage {
        int ProductImageId PK
        int ProductId FK
        nvarchar ImageUrl
        nvarchar StorageBucket
        nvarchar StoragePath
        bit IsPrimary
        int DisplayOrder
    }

    PriceHistory {
        int PriceHistoryId PK
        int ProductId FK
        decimal OldPrice
        decimal NewPrice
        int ChangedByUserId FK
    }

    GuestSession {
        int GuestSessionId PK
        nvarchar SessionToken
        nvarchar Email
        int ConvertedToUserId FK
        datetime2 ExpiresAt
    }

    Cart {
        int CartId PK
        int UserId FK
        int GuestSessionId FK
        bit IsCheckedOut
    }

    CartItem {
        int CartItemId PK
        int CartId FK
        int ProductId FK
        int ProductVariantId FK
        int Quantity
        decimal PriceAtAdd
    }

    Order {
        int OrderId PK
        int UserId FK
        nvarchar OrderNumber
        nvarchar OrderStatus
        decimal SubTotal
        decimal DiscountAmount
        decimal ShippingFee
        decimal TotalAmount "COMPUTED"
        nvarchar FulfillmentType
        nvarchar PaymentMethod
        bit IsWalkIn
        bit IsDeleted
    }

    OrderItem {
        int OrderItemId PK
        int OrderId FK
        int ProductId FK
        int ProductVariantId FK
        int Quantity
        decimal UnitPrice
    }

    Payment {
        int PaymentId PK
        int OrderId FK
        nvarchar PaymentMethod
        nvarchar PaymentStage
        nvarchar PaymentStatus
        decimal Amount
        bit IsDeleted
    }

    GCashPayment {
        int PaymentId PK_FK
        nvarchar GcashTransactionId
        nvarchar ScreenshotUrl
        nvarchar StorageBucket
        nvarchar StoragePath
    }

    BankTransferPayment {
        int PaymentId PK_FK
        nvarchar BpiReferenceNumber
        nvarchar ProofUrl
        int VerifiedByUserId FK
        nvarchar VerificationNotes
        datetime2 VerifiedAt
    }

    Delivery {
        int DeliveryId PK
        int OrderId FK
        nvarchar Courier
        nvarchar DeliveryStatus
        bit IsDelayed
        datetime2 EstimatedDeliveryTime
        datetime2 ActualDeliveryTime
    }

    LalamoveDelivery {
        int DeliveryId PK_FK
        nvarchar BookingRef
        nvarchar DriverName
        nvarchar DriverPhone
    }

    LBCDelivery {
        int DeliveryId PK_FK
        nvarchar TrackingNumber
    }

    PickupOrder {
        int PickupOrderId PK
        int OrderId FK
        datetime2 PickupReadyAt
        datetime2 PickupExpiresAt
        datetime2 PickupConfirmedAt
    }

    Voucher {
        int VoucherId PK
        nvarchar Code
        nvarchar DiscountType
        decimal DiscountValue
        decimal MinimumOrderAmount
        int MaxUses
        int MaxUsesPerUser
        datetime2 StartDate
        datetime2 EndDate
        bit IsActive
    }

    UserVoucher {
        int UserVoucherId PK
        int UserId FK
        int VoucherId FK
        datetime2 ExpiresAt
    }

    VoucherUsage {
        int VoucherUsageId PK
        int VoucherId FK
        int UserId FK
        int OrderId FK
        decimal DiscountAmount
    }

    Wishlist {
        int WishlistId PK
        int UserId FK
        int ProductId FK
        datetime2 AddedAt
    }

    Review {
        int ReviewId PK
        int UserId FK
        int ProductId FK
        int OrderId FK
        int Rating
        nvarchar Comment
        bit IsVerifiedPurchase
    }

    SupportTicket {
        int TicketId PK
        int UserId FK
        int OrderId FK
        nvarchar TicketCategory
        nvarchar Subject
        nvarchar TicketStatus
        int AssignedToUserId FK
    }

    SupportTicketReply {
        bigint ReplyId PK
        int TicketId FK
        int UserId FK
        bit IsAdminReply
        nvarchar Message
    }

    SupportTask {
        int TaskId PK
        int TicketId FK
        int AssignedToUserId FK
        nvarchar TaskType
        nvarchar TaskStatus
    }

    Notification {
        int NotificationId PK
        int UserId FK
        int OrderId FK
        int TicketId FK
        nvarchar Channel
        nvarchar NotifType
        nvarchar Status
        bit IsRead
    }

    Supplier {
        int SupplierId PK
        nvarchar Name
        nvarchar ContactPerson
        nvarchar Email
        bit IsActive
    }

    PurchaseOrder {
        int PurchaseOrderId PK
        int SupplierId FK
        nvarchar Status
        int CreatedByUserId FK
        datetime2 OrderDate
    }

    PurchaseOrderItem {
        int PurchaseOrderItemId PK
        int PurchaseOrderId FK
        int ProductId FK
        int ProductVariantId FK
        int Quantity
        decimal UnitPrice
    }

    InventoryLog {
        bigint InventoryLogId PK
        int ProductId FK
        int ProductVariantId FK
        int ChangeQuantity
        nvarchar ChangeType
        int OrderId FK
        int PurchaseOrderId FK
    }

    OrderStatusAudit {
        bigint AuditId PK
        int OrderId FK
        nvarchar FromStatus
        nvarchar ToStatus
        bit Success
    }

    POS_Session {
        int POSSessionId PK
        int UserId FK
        nvarchar TerminalName
        datetime2 ShiftStart
        datetime2 ShiftEnd
        decimal TotalSales
    }

    Refund {
        int RefundId PK
        int OrderId FK
        int PaymentId FK
        decimal RefundAmount
        nvarchar RefundReason
        nvarchar RefundStatus
        int RequestedByUserId FK
        int ApprovedByUserId FK
        int TicketId FK
    }

    StorePaymentAccount {
        int StorePaymentAccountId PK
        nvarchar PaymentMethod
        nvarchar AccountName
        nvarchar AccountNumber
        nvarchar BankName
        bit IsActive
    }

    SystemLog {
        bigint SystemLogId PK
        int UserId FK
        nvarchar EventType
        nvarchar EventDescription
    }

    User ||--o{ Address : "has"
    User ||--o{ UserRole : "has"
    Role ||--o{ UserRole : "assigned"
    User ||--o{ ActiveSession : "has"
    User ||--o{ Cart : "owns"
    User ||--o{ Order : "places"
    User ||--o{ Wishlist : "saves"
    User ||--o{ Review : "writes"
    User ||--o{ SupportTicket : "creates"
    User ||--o{ Notification : "receives"
    User ||--o{ UserVoucher : "assigned"
    User ||--o{ VoucherUsage : "uses"
    User ||--o{ SystemLog : "generates"
    User ||--o{ POS_Session : "operates"

    GuestSession ||--o| Cart : "owns"
    GuestSession }o--o| User : "converts to"

    Category ||--o{ Product : "contains"
    Category ||--o{ Category : "parent"
    Brand ||--o{ Product : "manufactures"
    Product ||--o{ ProductVariant : "has"
    Product ||--o{ ProductImage : "has"
    Product ||--o{ PriceHistory : "tracks"
    Product ||--o{ Review : "reviewed"
    Product ||--o{ Wishlist : "wishlisted"
    Product ||--o{ CartItem : "added to"
    Product ||--o{ OrderItem : "ordered"

    Cart ||--o{ CartItem : "contains"
    CartItem }o--o| ProductVariant : "specifies"

    Order ||--o{ OrderItem : "contains"
    Order ||--o{ Payment : "paid by"
    Order ||--o| Delivery : "shipped"
    Order ||--o| PickupOrder : "picked up"
    Order ||--o{ VoucherUsage : "redeems"
    Order ||--o{ Notification : "triggers"
    Order ||--o{ OrderStatusAudit : "audited"
    Order ||--o{ Refund : "refunded"
    Order ||--o{ Review : "reviewed"
    Order ||--o{ SupportTicket : "referenced"
    Order ||--o{ InventoryLog : "logged"
    OrderItem }o--o| ProductVariant : "specifies"

    Payment ||--o| GCashPayment : "extends"
    Payment ||--o| BankTransferPayment : "extends"

    Delivery ||--o| LalamoveDelivery : "extends"
    Delivery ||--o| LBCDelivery : "extends"

    Voucher ||--o{ UserVoucher : "assigned"
    Voucher ||--o{ VoucherUsage : "used"

    SupportTicket ||--o{ SupportTicketReply : "has"
    SupportTicket ||--o{ SupportTask : "has"
    SupportTicket ||--o{ Notification : "triggers"
    SupportTicket ||--o{ Refund : "linked"

    Supplier ||--o{ PurchaseOrder : "supplies"
    PurchaseOrder ||--o{ PurchaseOrderItem : "contains"
    PurchaseOrder ||--o{ InventoryLog : "logged"
```

---

## **2.3 Data Dictionary (Per Table)**

### **User**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| UserId | int | PK, IDENTITY, NOT NULL | Unique identifier for each user |
| Email | nvarchar(255) | NULLABLE | User's email address, used for login |
| PasswordHash | nvarchar(255) | NULLABLE | BCrypt-hashed password (work factor 12) |
| FirstName | nvarchar(100) | NOT NULL | User's first name |
| LastName | nvarchar(100) | NOT NULL | User's last name |
| PhoneNumber | nvarchar(20) | NULLABLE | Contact phone number |
| DefaultAddressId | int | NULLABLE | User's default shipping address |
| IsActive | bit | NOT NULL | Whether the account is active |
| IsWalkIn | bit | NOT NULL | True for the special POS walk-in user |
| CreatedAt | datetime2(7) | NOT NULL | Account creation timestamp |
| LastLoginAt | datetime2(7) | NULLABLE | Most recent login timestamp |
| FailedLoginAttempts | int | NOT NULL | Counter for lockout logic |
| LockoutUntil | datetime2(7) | NULLABLE | Lockout expiry timestamp |
| IsDeleted | bit | NOT NULL | Soft-delete flag |

### **Role**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| RoleId | int | PK, IDENTITY, NOT NULL | Unique role identifier |
| RoleName | nvarchar(50) | PK, NOT NULL | Role name (e.g., Admin, Manager, Customer) |
| Description | nvarchar(255) | NULLABLE | Human-readable role description |
| CreatedAt | datetime2(7) | NOT NULL | Role creation timestamp |

### **UserRole**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| UserRoleId | int | PK, IDENTITY, NOT NULL | Unique assignment identifier |
| UserId | int | PK, NOT NULL | The user being assigned |
| RoleId | int | PK, NOT NULL | The role being assigned |
| AssignedAt | datetime2(7) | NOT NULL | When the role was assigned |

### **Address**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| AddressId | int | PK, IDENTITY, NOT NULL | Unique address identifier |
| UserId | int | NOT NULL | Owner of the address |
| Label | nvarchar(50) | NOT NULL | Address label type |
| Street | nvarchar(500) | NOT NULL | Street address line |
| City | nvarchar(100) | NOT NULL | City name |
| PostalCode | nvarchar(20) | NOT NULL | ZIP/postal code |
| Province | nvarchar(100) | NULLABLE | Province or state |
| Country | nvarchar(100) | NOT NULL | Country name |
| IsDefault | bit | NOT NULL | Whether this is the user's default address |
| IsSnapshot | bit | NOT NULL | True for order-time address snapshots (immutable) |
| CreatedAt | datetime2(7) | NOT NULL | Creation timestamp |

### **ActiveSession**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| SessionId | int | PK, IDENTITY, NOT NULL | Unique session identifier |
| UserId | int | NOT NULL | The logged-in user |
| RefreshToken | nvarchar(500) | NOT NULL | (Missing description) |
| DeviceInfo | nvarchar(500) | NULLABLE | (Missing description) |
| IpAddress | nvarchar(50) | NULLABLE | (Missing description) |
| IsRevoked | bit | NOT NULL | Whether the session was manually revoked |
| ExpiresAt | datetime2(7) | NOT NULL | Session expiry timestamp |
| CreatedAt | datetime2(7) | NOT NULL | Session creation timestamp |
| RevokedAt | datetime2(7) | NULLABLE | (Missing description) |

### **OTPVerification**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| OTPId | int | PK, IDENTITY, NOT NULL | Unique OTP identifier |
| Email | nvarchar(255) | NOT NULL | Target email address |
| OTPCode | nvarchar(128) | NOT NULL | Hashed OTP code |
| IsUsed | bit | NOT NULL | Whether the code has been consumed |
| ExpiresAt | datetime2(7) | NOT NULL | Code expiration timestamp |
| CreatedAt | datetime2(7) | NOT NULL | Code creation timestamp |

### **Category**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| CategoryId | int | PK, IDENTITY, NOT NULL | Unique category identifier |
| CategoryCode | varchar(20) | PK, NOT NULL | URL-friendly category slug |
| Name | nvarchar(100) | NOT NULL | Display name |
| Description | nvarchar(500) | NULLABLE | Category description |
| ParentCategoryId | int | NULLABLE | Parent category for hierarchy |
| IsActive | bit | NOT NULL | Whether category is visible |
| DisplayOrder | int | NOT NULL | Sort order in listings |

### **Brand**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| BrandId | int | PK, IDENTITY, NOT NULL | Unique brand identifier |
| BrandName | nvarchar(100) | PK, NOT NULL | Brand name |
| Country | nvarchar(100) | NULLABLE | Brand's country of origin |
| Website | nvarchar(255) | NULLABLE | Brand website URL |
| Description | nvarchar(500) | NULLABLE | Brand description |
| IsActive | bit | NOT NULL | Whether brand is active |
| CreatedAt | datetime2(7) | NOT NULL | Creation timestamp |

### **Product**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| ProductId | int | PK, IDENTITY, NOT NULL | Unique product identifier |
| CategoryId | int | NOT NULL | Product's category |
| BrandId | int | NULLABLE | Product's brand |
| SKU | varchar(50) | NULLABLE | Stock Keeping Unit code |
| Name | nvarchar(200) | NOT NULL | Product display name |
| ShortDescription | nvarchar(300) | NULLABLE | Brief product summary |
| Description | nvarchar(max) | NULLABLE | Full product description |
| Price | decimal(10, 2) | NOT NULL | Base price in specified currency |
| Currency | char(3) | NOT NULL | Price currency code |
| Material | nvarchar(100) | NULLABLE | Material specification |
| Color | nvarchar(100) | NULLABLE | Product color |
| WheelSize | nvarchar(20) | NULLABLE | Bike wheel size |
| SpeedCompatibility | nvarchar(50) | NULLABLE | Speed/gear compatibility |
| BoostCompatible | bit | NULLABLE | Whether boost-compatible |
| TubelessReady | bit | NULLABLE | Whether tubeless-ready |
| AxleStandard | nvarchar(50) | NULLABLE | Axle standard specification |
| SuspensionTravel | nvarchar(50) | NULLABLE | Suspension travel distance |
| BrakeType | nvarchar(100) | NULLABLE | Brake type specification |
| AdditionalSpecs | nvarchar(1000) | NULLABLE | Additional specs as JSON |
| IsActive | bit | NOT NULL | Whether product is listed |
| IsFeatured | bit | NOT NULL | Whether shown on homepage |
| CreatedAt | datetime2(7) | NOT NULL | Creation timestamp |
| UpdatedAt | datetime2(7) | NULLABLE | Last modification timestamp |

### **ProductVariant**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| ProductVariantId | int | PK, IDENTITY, NOT NULL | Unique variant identifier |
| ProductId | int | NOT NULL | Parent product |
| VariantName | nvarchar(100) | NOT NULL | Variant display name (e.g., "Size M", "Red") |
| SKU | nvarchar(50) | NULLABLE | Variant-specific SKU |
| AdditionalPrice | decimal(10, 2) | NOT NULL | Price added to base product price |
| StockQuantity | int | NOT NULL | Current stock level |
| IsActive | bit | NOT NULL | Whether variant is available |
| CreatedAt | datetime2(7) | NOT NULL | Creation timestamp |
| UpdatedAt | datetime2(7) | NULLABLE | Last modification timestamp |
| ReorderThreshold | int | NOT NULL | Low-stock alert threshold |

### **ProductImage**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| ProductImageId | int | PK, IDENTITY, NOT NULL | Unique image identifier |
| ProductId | int | NOT NULL | Associated product |
| StorageBucket | nvarchar(200) | NOT NULL | GCS bucket name |
| StoragePath | nvarchar(1000) | NOT NULL | GCS storage path |
| ImageUrl | nvarchar(1000) | NOT NULL | Public image URL |
| ImageType | nvarchar(50) | NOT NULL | Image type (e.g., "Gallery", "Thumbnail") |
| IsPrimary | bit | NOT NULL | Whether this is the primary display image |
| DisplayOrder | int | NOT NULL | Sort order in gallery |
| AltText | nvarchar(200) | NULLABLE | Accessibility alt text |
| FileSizeBytes | int | NULLABLE | File size in bytes |
| MimeType | nvarchar(50) | NULLABLE | MIME type (e.g., image/webp) |
| Width | int | NULLABLE | Image width in pixels |
| Height | int | NULLABLE | Image height in pixels |
| UploadedByUserId | int | NULLABLE | Admin who uploaded the image |
| CreatedAt | datetime2(7) | NOT NULL | Upload timestamp |

### **PriceHistory**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| PriceHistoryId | int | PK, IDENTITY, NOT NULL | Unique history identifier |
| ProductId | int | NOT NULL | Product whose price changed |
| OldPrice | decimal(10, 2) | NOT NULL | Previous price |
| NewPrice | decimal(10, 2) | NOT NULL | Updated price |
| ChangedAt | datetime2(7) | NOT NULL | When the change occurred |
| ChangedByUserId | int | NULLABLE | Admin who made the change |
| Notes | nvarchar(500) | NULLABLE | Reason for price change |

### **GuestSession**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| GuestSessionId | int | PK, IDENTITY, NOT NULL | Unique guest session identifier |
| SessionToken | nvarchar(100) | PK, NOT NULL | Secure session token stored in cookie |
| Email | nvarchar(255) | NULLABLE | Guest's email if provided |
| PhoneNumber | nvarchar(20) | NULLABLE | Guest's phone if provided |
| ConvertedToUserId | int | NULLABLE | Set when guest registers |
| ExpiresAt | datetime2(7) | NOT NULL | Session expiry (7 days) |
| CreatedAt | datetime2(7) | NOT NULL | Creation timestamp |

### **Cart**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| CartId | int | PK, IDENTITY, NOT NULL | Unique cart identifier |
| UserId | int | NULLABLE | Authenticated cart owner |
| GuestSessionId | int | NULLABLE | Guest cart owner |
| CreatedAt | datetime2(7) | NOT NULL | Cart creation timestamp |
| LastUpdatedAt | datetime2(7) | NULLABLE | (Missing description) |
| IsCheckedOut | bit | NOT NULL | Whether cart has been converted to an order |

> **CHECK Constraint:** `CK_Cart_Owner` — Exactly one of UserId or GuestSessionId must be non-null.

### **CartItem**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| CartItemId | int | PK, IDENTITY, NOT NULL | Unique cart item identifier |
| CartId | int | NOT NULL | Parent cart |
| ProductId | int | NOT NULL | Product added |
| ProductVariantId | int | NULLABLE | Selected variant |
| Quantity | int | NOT NULL | Quantity in cart |
| PriceAtAdd | decimal(10, 2) | NOT NULL | Price captured at add time |
| AddedAt | datetime2(7) | NOT NULL | When item was added |

### **Order**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| OrderId | int | PK, IDENTITY, NOT NULL | Unique order identifier |
| UserId | int | NULLABLE | Customer who placed the order |
| OrderNumber | nvarchar(50) | PK, NOT NULL | Formatted order number (e.g., TBS-2026-00001) |
| OrderDate | datetime2(7) | NOT NULL | When the order was placed |
| OrderStatus | nvarchar(50) | NOT NULL | Current order status |
| SubTotal | decimal(10, 2) | NOT NULL | Sum of line item totals |
| DiscountAmount | decimal(10, 2) | NOT NULL | Voucher discount applied |
| ShippingFee | decimal(10, 2) | NOT NULL | Delivery fee |
| ShippingAddressId | int | NULLABLE | Snapshot of shipping address |
| ContactPhone | nvarchar(20) | NULLABLE | Customer contact phone |
| DeliveryInstructions | nvarchar(500) | NULLABLE | Special delivery instructions |
| IsWalkIn | bit | NOT NULL | True for POS transactions |
| CreatedAt | datetime2(7) | NOT NULL | Creation timestamp |
| UpdatedAt | datetime2(7) | NULLABLE | Last modification timestamp |
| IsDeleted | bit | NOT NULL | Soft-delete flag |
| FulfillmentType | nvarchar(20) | NOT NULL | How the order is fulfilled |
| GuestSessionId | int | NULLABLE | Guest who placed the order |
| CartId | int | NULLABLE | Source cart |
| POSSessionId | int | NULLABLE | POS session for walk-in orders |
| TotalAmount | computed | NULLABLE | `(SubTotal - DiscountAmount) + ShippingFee` |
| PaymentMethod | nvarchar(50) | NOT NULL | Selected payment method |

### **OrderItem**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| OrderItemId | int | PK, IDENTITY, NOT NULL | Unique line item identifier |
| OrderId | int | NOT NULL | Parent order |
| ProductId | int | NOT NULL | Product ordered |
| ProductVariantId | int | NULLABLE | Specific variant |
| Quantity | int | NOT NULL | Quantity ordered |
| UnitPrice | decimal(10, 2) | NOT NULL | Price at time of order |

### **Payment**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| PaymentId | int | PK, IDENTITY, NOT NULL | Unique payment identifier |
| OrderId | int | NOT NULL | Associated order |
| PaymentMethod | nvarchar(50) | NOT NULL | Payment method used |
| PaymentStage | nvarchar(20) | NOT NULL | Payment stage |
| PaymentStatus | nvarchar(50) | NOT NULL | Current payment status |
| Amount | decimal(10, 2) | NOT NULL | Payment amount |
| PaymentDate | datetime2(7) | NULLABLE | When payment was completed |
| CreatedAt | datetime2(7) | NOT NULL | Payment record creation |
| IsDeleted | bit | NOT NULL | Soft-delete flag |
| PaidToAccountName | nvarchar(150) | NULLABLE | Store account name used |
| PaidToAccountNumber | nvarchar(50) | NULLABLE | Store account number used |
| PaidToBankName | nvarchar(100) | NULLABLE | Store bank name used |

### **GCashPayment**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| PaymentId | int | PK, NOT NULL | Shared key (TPT inheritance) |
| GcashTransactionId | nvarchar(255) | NULLABLE | GCash reference/transaction ID |
| ScreenshotUrl | nvarchar(1000) | NULLABLE | Public URL of payment screenshot |
| StorageBucket | nvarchar(200) | NULLABLE | GCS bucket for screenshot |
| StoragePath | nvarchar(1000) | NULLABLE | GCS path for screenshot |
| SubmittedAt | datetime2(7) | NULLABLE | When proof was submitted |

### **BankTransferPayment**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| PaymentId | int | PK, NOT NULL | Shared key (TPT inheritance) |
| BpiReferenceNumber | nvarchar(255) | NULLABLE | Bank reference/transaction number |
| ProofUrl | nvarchar(1000) | NULLABLE | Public URL of deposit slip |
| ProofStorageBucket | nvarchar(200) | NULLABLE | GCS bucket for deposit slip |
| ProofStoragePath | nvarchar(1000) | NULLABLE | GCS path for deposit slip |
| VerifiedByUserId | int | NULLABLE | Admin who verified the payment |
| VerificationNotes | nvarchar(500) | NULLABLE | Admin notes on verification |
| VerifiedAt | datetime2(7) | NULLABLE | When verification occurred |
| VerificationDeadline | datetime2(7) | NULLABLE | Deadline for verification |
| SubmittedAt | datetime2(7) | NULLABLE | When proof was submitted |

### **Delivery**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| DeliveryId | int | PK, IDENTITY, NOT NULL | Unique delivery identifier |
| OrderId | int | NOT NULL | Associated order |
| Courier | nvarchar(20) | NOT NULL | Courier service used |
| DeliveryStatus | nvarchar(50) | NOT NULL | Current delivery status |
| IsDelayed | bit | NOT NULL | Whether delivery is delayed |
| DelayedUntil | datetime2(7) | NULLABLE | Expected delay resolution |
| EstimatedDeliveryTime | datetime2(7) | NULLABLE | Estimated arrival |
| ActualDeliveryTime | datetime2(7) | NULLABLE | Actual arrival |
| CreatedAt | datetime2(7) | NOT NULL | Creation timestamp |

### **LalamoveDelivery**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| DeliveryId | int | PK, NOT NULL | Shared key (TPT inheritance) |
| BookingRef | nvarchar(255) | NULLABLE | Lalamove booking reference |
| DriverName | nvarchar(100) | NULLABLE | Assigned driver name |
| DriverPhone | nvarchar(20) | NULLABLE | Driver contact number |

### **LBCDelivery**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| DeliveryId | int | PK, NOT NULL | Shared key (TPT inheritance) |
| TrackingNumber | nvarchar(255) | NULLABLE | LBC tracking number |

### **PickupOrder**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| PickupOrderId | int | PK, IDENTITY, NOT NULL | Unique pickup identifier |
| OrderId | int | PK, NOT NULL | Associated order (one-to-one) |
| PickupReadyAt | datetime2(7) | NULLABLE | When order was marked ready |
| PickupExpiresAt | datetime2(7) | NULLABLE | Pickup expiry (7 days) |
| PickupConfirmedAt | datetime2(7) | NULLABLE | When customer picked up |

### **Voucher**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| VoucherId | int | PK, IDENTITY, NOT NULL | Unique voucher identifier |
| Code | nvarchar(50) | PK, NOT NULL | Promo code entered by customer |
| Description | nvarchar(500) | NULLABLE | Human-readable description |
| DiscountType | nvarchar(20) | NOT NULL | "Percentage" or "Fixed" |
| DiscountValue | decimal(10, 2) | NOT NULL | Discount amount or percentage |
| MinimumOrderAmount | decimal(10, 2) | NULLABLE | Minimum subtotal required |
| MaxUses | int | NULLABLE | Global usage cap |
| MaxUsesPerUser | int | NULLABLE | Per-user usage cap |
| StartDate | datetime2(7) | NOT NULL | Voucher start date |
| EndDate | datetime2(7) | NULLABLE | Voucher end date |
| IsActive | bit | NOT NULL | Whether voucher is active |
| CreatedAt | datetime2(7) | NOT NULL | Creation timestamp |

### **UserVoucher**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| UserVoucherId | int | PK, IDENTITY, NOT NULL | Unique assignment identifier |
| UserId | int | PK, NOT NULL | Assigned user |
| VoucherId | int | PK, NOT NULL | Assigned voucher |
| AssignedAt | datetime2(7) | NOT NULL | Assignment timestamp |
| ExpiresAt | datetime2(7) | NULLABLE | Per-user assignment expiry |

### **VoucherUsage**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| VoucherUsageId | int | PK, IDENTITY, NOT NULL | Unique usage identifier |
| VoucherId | int | NOT NULL | Voucher that was used |
| UserId | int | NOT NULL | User who used it |
| OrderId | int | NOT NULL | Order it was applied to |
| DiscountAmount | decimal(10, 2) | NOT NULL | Actual discount given |
| UsedAt | datetime2(7) | NOT NULL | When it was redeemed |

### **Wishlist**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| WishlistId | int | PK, IDENTITY, NOT NULL | Unique wishlist entry identifier |
| UserId | int | PK, NOT NULL | User who saved the product |
| ProductId | int | PK, NOT NULL | Saved product |
| AddedAt | datetime2(7) | NOT NULL | When the item was wishlisted |

### **Review**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| ReviewId | int | PK, IDENTITY, NOT NULL | Unique review identifier |
| UserId | int | NOT NULL | Reviewer |
| ProductId | int | NOT NULL | Reviewed product |
| OrderId | int | NOT NULL | The order that verifies the purchase |
| Rating | int | NOT NULL | Star rating (1–5) |
| Comment | nvarchar(1000) | NULLABLE | Review text |
| IsVerifiedPurchase | bit | NOT NULL | Whether the reviewer actually bought the product |
| CreatedAt | datetime2(7) | NOT NULL | Review submission timestamp |

### **SupportTicket**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| TicketId | int | PK, IDENTITY, NOT NULL | Unique ticket identifier |
| UserId | int | NOT NULL | Customer who created the ticket |
| OrderId | int | NULLABLE | Related order (if applicable) |
| TicketSource | nvarchar(50) | NOT NULL | Source channel (e.g., "Web") |
| TicketCategory | nvarchar(100) | NOT NULL | Issue category |
| Subject | nvarchar(200) | NOT NULL | Ticket subject line |
| Description | nvarchar(max) | NULLABLE | Full issue description |
| AttachmentUrl | nvarchar(1000) | NULLABLE | Attachment public URL |
| AttachmentBucket | nvarchar(200) | NULLABLE | GCS bucket for attachment |
| AttachmentPath | nvarchar(1000) | NULLABLE | GCS path for attachment |
| TicketStatus | nvarchar(50) | NOT NULL | Current status (Open/InProgress/Resolved/Closed) |
| AssignedToUserId | int | NULLABLE | Admin staff assigned |
| ResolvedAt | datetime2(7) | NULLABLE | Resolution timestamp |
| CreatedAt | datetime2(7) | NOT NULL | Creation timestamp |
| UpdatedAt | datetime2(7) | NULLABLE | Last update timestamp |

### **SupportTicketReply**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| ReplyId | bigint | PK, IDENTITY, NOT NULL | Unique reply identifier |
| TicketId | int | NOT NULL | Parent ticket |
| UserId | int | NOT NULL | Author of the reply |
| IsAdminReply | bit | NOT NULL | True if reply is from admin staff |
| Message | nvarchar(max) | NOT NULL | Reply message content |
| AttachmentUrl | nvarchar(1000) | NULLABLE | Attachment public URL |
| AttachmentBucket | nvarchar(200) | NULLABLE | GCS bucket for attachment |
| AttachmentPath | nvarchar(1000) | NULLABLE | GCS path for attachment |
| CreatedAt | datetime2(7) | NOT NULL | Reply timestamp |

### **SupportTask**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| TaskId | int | PK, IDENTITY, NOT NULL | Unique task identifier |
| TicketId | int | NOT NULL | Parent ticket |
| AssignedToUserId | int | NULLABLE | Staff assigned to the task |
| TaskType | nvarchar(50) | NOT NULL | Type of task |
| TaskStatus | nvarchar(20) | NOT NULL | Task status |
| DueDate | datetime2(7) | NULLABLE | Task deadline |
| Notes | nvarchar(500) | NULLABLE | Task notes |
| CreatedAt | datetime2(7) | NOT NULL | Creation timestamp |
| CompletedAt | datetime2(7) | NULLABLE | Completion timestamp |

### **Notification**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| NotificationId | int | PK, IDENTITY, NOT NULL | Unique notification identifier |
| UserId | int | NOT NULL | Recipient user |
| OrderId | int | NULLABLE | Related order |
| TicketId | int | NULLABLE | Related ticket |
| Channel | nvarchar(20) | NOT NULL | Delivery channel |
| NotifType | nvarchar(100) | NOT NULL | Notification type |
| Recipient | nvarchar(255) | NOT NULL | Target address/number |
| Subject | nvarchar(255) | NULLABLE | Notification subject |
| Body | nvarchar(max) | NULLABLE | Notification body content |
| Status | nvarchar(20) | NOT NULL | Delivery status |
| RetryCount | int | NOT NULL | Number of delivery attempts |
| SentAt | datetime2(7) | NULLABLE | When successfully sent |
| FailureReason | nvarchar(500) | NULLABLE | Reason for delivery failure |
| CreatedAt | datetime2(7) | NOT NULL | Creation timestamp |
| IsRead | bit | NOT NULL | Whether user has read it |
| ReadAt | datetime2(7) | NULLABLE | When user marked it as read |

### **Supplier**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| SupplierId | int | PK, IDENTITY, NOT NULL | Unique supplier identifier |
| Name | nvarchar(200) | NOT NULL | Supplier company name |
| ContactPerson | nvarchar(100) | NULLABLE | Primary contact name |
| PhoneNumber | nvarchar(20) | NULLABLE | Contact phone |
| Email | nvarchar(255) | NULLABLE | Contact email |
| Address | nvarchar(500) | NULLABLE | Business address |
| IsActive | bit | NOT NULL | Whether supplier is active |
| CreatedAt | datetime2(7) | NOT NULL | Creation timestamp |

### **PurchaseOrder**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| PurchaseOrderId | int | PK, IDENTITY, NOT NULL | Unique PO identifier |
| SupplierId | int | NOT NULL | Supplier being ordered from |
| OrderDate | datetime2(7) | NOT NULL | When the PO was placed |
| ExpectedDeliveryDate | datetime2(7) | NULLABLE | Expected supplier delivery |
| Status | nvarchar(50) | NOT NULL | PO status |
| CreatedByUserId | int | NULLABLE | Admin who created the PO |
| CreatedAt | datetime2(7) | NOT NULL | Creation timestamp |

### **PurchaseOrderItem**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| PurchaseOrderItemId | int | PK, IDENTITY, NOT NULL | Unique PO line item identifier |
| PurchaseOrderId | int | NOT NULL | Parent PO |
| ProductId | int | NOT NULL | Product being ordered |
| ProductVariantId | int | NULLABLE | Specific variant |
| Quantity | int | NOT NULL | Quantity ordered |
| UnitPrice | decimal(10, 2) | NOT NULL | Supplier unit price |

### **InventoryLog**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| InventoryLogId | bigint | PK, IDENTITY, NOT NULL | Unique log entry identifier |
| ProductId | int | NOT NULL | Product affected |
| ProductVariantId | int | NULLABLE | Variant affected |
| ChangeQuantity | int | NOT NULL | Stock change (negative = decrease) |
| ChangeType | nvarchar(50) | NOT NULL | Type of stock change |
| OrderId | int | NULLABLE | Related customer order |
| PurchaseOrderId | int | NULLABLE | Related purchase order |
| ChangedByUserId | int | NULLABLE | Staff who made the change |
| Notes | nvarchar(500) | NULLABLE | Reason/description |
| CreatedAt | datetime2(7) | NOT NULL | Timestamp |

### **OrderStatusAudit**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| AuditId | bigint | PK, IDENTITY, NOT NULL | Unique audit entry identifier |
| OrderId | int | NOT NULL | Order being transitioned |
| FromStatus | nvarchar(50) | NOT NULL | Previous status |
| ToStatus | nvarchar(50) | NOT NULL | New status |
| Success | bit | NOT NULL | Whether transition was allowed |
| Reason | nvarchar(500) | NULLABLE | Reason for rejection (if failed) |
| CreatedAt | datetime2(7) | NOT NULL | Timestamp |

### **POS_Session**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| POSSessionId | int | PK, IDENTITY, NOT NULL | Unique session identifier |
| UserId | int | NOT NULL | Cashier operating the POS |
| TerminalName | nvarchar(50) | NOT NULL | POS terminal identifier |
| ShiftStart | datetime2(7) | NOT NULL | Shift start time |
| ShiftEnd | datetime2(7) | NULLABLE | Shift end time |
| TotalSales | decimal(10, 2) | NOT NULL | Total sales during shift |
| CreatedAt | datetime2(7) | NOT NULL | Creation timestamp |

### **Refund**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| RefundId | int | PK, IDENTITY, NOT NULL | Unique refund identifier |
| OrderId | int | NOT NULL | Order being refunded |
| PaymentId | int | NULLABLE | Original payment |
| RefundAmount | decimal(10, 2) | NOT NULL | Refund amount |
| RefundReason | nvarchar(500) | NOT NULL | Reason for the refund |
| RefundStatus | nvarchar(50) | NOT NULL | Refund status |
| RefundMethod | nvarchar(50) | NULLABLE | Method for returning funds |
| RequestedByUserId | int | NULLABLE | Customer who requested |
| ApprovedByUserId | int | NULLABLE | Admin who approved |
| TicketId | int | NULLABLE | Related support ticket |
| Notes | nvarchar(500) | NULLABLE | Internal notes |
| CreatedAt | datetime2(7) | NOT NULL | Creation timestamp |
| ProcessedAt | datetime2(7) | NULLABLE | When refund was processed |

### **StorePaymentAccount**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| StorePaymentAccountId | int | PK, IDENTITY, NOT NULL | Unique account identifier |
| PaymentMethod | nvarchar(50) | NOT NULL | Method (GCash/BankTransfer) |
| AccountName | nvarchar(150) | NOT NULL | Account holder name |
| AccountNumber | nvarchar(50) | NOT NULL | Account number |
| BankName | nvarchar(100) | NULLABLE | Bank name (for BankTransfer) |
| QrImageUrl | nvarchar(1000) | NULLABLE | QR code image URL |
| Instructions | nvarchar(500) | NULLABLE | Payment instructions for customer |
| IsActive | bit | NOT NULL | Whether currently displayed |
| DisplayOrder | int | NOT NULL | Sort order on checkout page |
| CreatedAt | datetime2(7) | NOT NULL | Creation timestamp |
| UpdatedAt | datetime2(7) | NOT NULL | Last modification timestamp |

### **SystemLog**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| SystemLogId | bigint | PK, IDENTITY, NOT NULL | Unique log identifier |
| UserId | int | NULLABLE | User who triggered the event |
| EventType | nvarchar(100) | NOT NULL | Type of system event |
| EventDescription | nvarchar(1000) | NULLABLE | Event details |
| CreatedAt | datetime2(7) | NOT NULL | Event timestamp |

---


### **__EFMigrationsHistory**
| Column Name | Data Type | Constraints | Description |
|---|---|---|---|
| MigrationId | nvarchar(150) | PK, NOT NULL | EF Core Migration History |
| ProductVersion | nvarchar(32) | NOT NULL | EF Core Migration History |

## **2.4 Storyboard / System Flow**

### **Customer Flow (WebApplication)**

1. **Homepage** — Customer visits the website and sees featured products loaded via `HomeController.Index`.
2. **Registration** — Customer fills out the registration form → system sends an OTP to email via `CustomerController.Register` → customer verifies the OTP via `CustomerController.VerifyOTP` → account is created, user is signed in with cookie authentication, and redirected to Dashboard.
3. **Login** — Customer enters email/password → `CustomerController.Login` validates credentials via `IUserService.LoginAsync` → BCrypt hash comparison → cookie-based sign-in → redirect to Dashboard.
4. **Product Browsing** — Customer browses the product catalog via `ProductController.List` with filters (category, brand, price range, search text) → views product detail page via `ProductController.Detail` with image gallery, variants, and reviews.
5. **Add to Cart** — Customer clicks "Add to Cart" → AJAX POST to `CartController.AddToCart` → item added with price snapshot → cart count badge updated.
6. **Cart Management** — Customer views cart via `CartController.ViewCart` → updates quantities via `CartController.UpdateQuantity` → removes items via `CartController.RemoveFromCart`.
7. **Checkout** — Customer proceeds to `CheckoutController.Index` → selects saved shipping address → chooses fulfillment type (Delivery via Lalamove/LBC, or Pickup) → selects payment method (GCash or BankTransfer) → optionally applies a voucher via `VoucherController.Validate` → places order via `CheckoutController.PlaceOrder`.
8. **Order Confirmation** — System creates the Order, OrderItems, snaps the shipping address, deducts inventory, records VoucherUsage → redirects to `OrderController.Confirmation`.
9. **Payment Submission** — For GCash/BankTransfer orders, customer navigates to `PaymentController.Submit` → uploads payment screenshot/deposit slip → stored in GCS → payment status set to `VerificationPending`.
10. **Order Tracking** — Customer views order history via `OrderController.History` → views order detail via `OrderController.Detail` → can cancel pending orders via `OrderController.Cancel` → confirms delivery via `OrderController.ConfirmDelivery`.
11. **Reviews** — After delivery, customer submits reviews via `ReviewController.Submit` → linked to verified purchase → displayed on product page.
12. **Support** — Customer creates tickets via `SupportController.Create` → views threaded replies via `SupportController.Detail` → adds replies via `SupportController.Reply`.
13. **Wishlist** — Customer toggles products on/off wishlist via `WishlistController.Toggle` → moves items to cart via `WishlistController.MoveToCart`.
14. **Notifications** — Customer views notification history via `CustomerController.Notifications` → marks individual/all as read → unread count shown via AJAX badge via `CustomerController.NotificationCount`.
15. **Profile** — Customer manages personal info, password, and addresses via `CustomerController.Profile`, `ChangePassword`, `AddAddress`, `DeleteAddress`, `SetDefaultAddress`.

### **Admin Flow (AdminSystem_v2 — WPF Desktop)**

1. **Login** — Admin/staff logs in via `LoginViewModel` → credentials validated with BCrypt via `UserRepository`.
2. **Dashboard** — `DashboardViewModel` displays sales summary, order counts by status, and key metrics.
3. **Order Management** — `OrderViewModel` lists all online orders → admin changes status (Pending → Processing → OutForDelivery → Delivered) → approves/rejects payment proofs → marks orders as ready for pickup → confirms pickup → creates delivery records.
4. **POS (Point of Sale)** — `POSViewModel` handles walk-in transactions → product search → customer search → voucher application → atomic sale creation via `POSRepository.CreatePOSSaleAsync` (inserts Order, OrderItems, deducts stock, logs inventory, records Payment, records VoucherUsage — all in a single transaction).
5. **Product Management** — `ProductViewModel` manages products, variants, images, categories, and brands.
6. **Staff Management** — `StaffViewModel` manages user accounts and role assignments via `UserRepository`.
7. **Voucher Management** — `VoucherViewModel` creates/edits/deactivates vouchers and assigns them to users.
8. **Reports** — `ReportViewModel` generates sales over time, top products, order status distribution, and payment method breakdowns with OxyPlot charts.
9. **Store Payment Accounts** — `StorePaymentAccountViewModel` manages the shop's GCash and BankTransfer receiving accounts.

---

# **3. SQL Command Extraction (Real Queries)**

All queries below are extracted from the actual codebase — no fabricated examples.

### **SELECT — Order List with Customer Info**
*Source: `AdminSystem_v2/Repositories/OrderRepository.cs`*
```sql
SELECT
    o.OrderId, o.UserId, o.OrderNumber, o.OrderDate, o.OrderStatus,
    o.SubTotal, o.DiscountAmount, o.ShippingFee,
    o.ContactPhone, o.DeliveryInstructions, o.IsWalkIn,
    o.CreatedAt, o.UpdatedAt,
    u.FirstName + ' ' + u.LastName  AS CustomerName,
    u.Email                          AS CustomerEmail,
    CASE WHEN po.PickupOrderId IS NOT NULL THEN 'Pickup' ELSE 'Delivery' END
                                     AS DeliveryType,
    (SELECT COUNT(*) FROM vw_OrderItemDetail vi WHERE vi.OrderId = o.OrderId)
                                     AS ItemCount
FROM [Order] o
INNER JOIN [User]      u  ON o.UserId  = u.UserId
LEFT  JOIN PickupOrder po ON o.OrderId = po.OrderId
WHERE o.IsWalkIn = 0
ORDER BY o.OrderDate DESC
```
**Usage:** Populates the admin order management dashboard with all online orders including customer names, order status, item counts, and delivery types.

---

### **SELECT — POS Product Search**
*Source: `AdminSystem_v2/Repositories/POSRepository.cs`*
```sql
SELECT TOP 30
    p.ProductId,
    pv.ProductVariantId,
    p.Name                        AS ProductName,
    pv.VariantName,
    p.Price + pv.AdditionalPrice  AS UnitPrice,
    pv.StockQuantity,
    ISNULL(pv.SKU, '')            AS SKU
FROM Product p
INNER JOIN ProductVariant pv
    ON p.ProductId = pv.ProductId
    AND pv.IsActive = 1
WHERE p.IsActive = 1
    AND (
        p.Name            LIKE @Search
        OR pv.VariantName LIKE @Search
        OR pv.SKU         LIKE @Search
    )
ORDER BY p.Name, pv.VariantName
```
**Usage:** Powers the POS product search bar — cashiers type a product name, variant, or SKU and get real-time results with computed unit prices and stock levels.

---

### **SELECT — Payment Proof Details (JOIN)**
*Source: `AdminSystem_v2/Repositories/OrderRepository.cs`*
```sql
SELECT TOP 1
    p.PaymentMethod          AS PaymentProofMethod,
    p.PaymentStatus          AS PaymentStatus,
    COALESCE(g.ScreenshotUrl, bt.ProofUrl) AS PaymentProofUrl
FROM Payment p
LEFT JOIN GCashPayment        g  ON g.PaymentId  = p.PaymentId
LEFT JOIN BankTransferPayment bt ON bt.PaymentId = p.PaymentId
WHERE p.OrderId = @Id
ORDER BY p.CreatedAt DESC
```
**Usage:** Retrieves the most recent payment proof for an order. Uses `LEFT JOIN` to handle both GCash and BankTransfer payment types, and `COALESCE` to return whichever proof URL exists.

---

### **INSERT — Atomic POS Sale (Order + Items + Inventory + Payment)**
*Source: `AdminSystem_v2/Repositories/POSRepository.cs`*
```sql
-- 1. Insert Order
INSERT INTO [Order]
    (UserId, OrderNumber, OrderDate, OrderStatus,
     SubTotal, DiscountAmount, ShippingFee,
     IsWalkIn, FulfillmentType, PaymentMethod, CreatedAt, UpdatedAt)
VALUES
    (@UserId, @OrderNumber, GETUTCDATE(), @Status,
     @SubTotal, @Discount, 0,
     1, 'WalkIn', N'Cash', GETUTCDATE(), GETUTCDATE());
SELECT CAST(SCOPE_IDENTITY() AS INT);

-- 2. Insert Order Items (per line item)
INSERT INTO OrderItem
    (OrderId, ProductId, ProductVariantId, Quantity, UnitPrice)
VALUES
    (@OrderId, @ProductId, @VariantId, @Qty, @Price)

-- 3. Insert Payment
INSERT INTO Payment
    (OrderId, PaymentMethod, PaymentStage, PaymentStatus,
     Amount, PaymentDate, CreatedAt)
VALUES
    (@OrderId, @Method, 'Upfront', 'Completed',
     @Amount, GETUTCDATE(), GETUTCDATE())
```
**Usage:** Creates a complete POS walk-in sale atomically within a database transaction. Inserts the order, all line items, and a completed payment record. `SCOPE_IDENTITY()` captures the new order ID for subsequent inserts.

---

### **UPDATE — Order Status Progression**
*Source: `AdminSystem_v2/Repositories/OrderRepository.cs`*
```sql
UPDATE [Order]
SET OrderStatus = @Status, UpdatedAt = GETUTCDATE()
WHERE OrderId = @OrderId
```
**Usage:** Advances an order through its lifecycle (Pending → Processing → OutForDelivery → Delivered). Always preceded by server-side validation of the transition against an allowed forward-only map, and followed by customer notification queuing and audit logging.

---

### **UPDATE — Stock Deduction on Sale**
*Source: `AdminSystem_v2/Repositories/POSRepository.cs`*
```sql
UPDATE ProductVariant
SET StockQuantity = StockQuantity - @Qty,
    UpdatedAt     = GETUTCDATE()
WHERE ProductVariantId = @Id
```
**Usage:** Deducts stock from the variant-level `StockQuantity` column during a POS sale. Runs within the same transaction as the order creation to prevent overselling.

---

### **UPDATE — Payment Approval and Rejection**
*Source: `AdminSystem_v2/Repositories/OrderRepository.cs`*
```sql
-- Approve: Mark payment as Completed
UPDATE Payment
SET PaymentStatus = 'Completed', PaymentDate = GETUTCDATE()
WHERE OrderId = @OrderId;

-- Reject: Mark payment as VerificationRejected
UPDATE Payment
SET PaymentStatus = 'VerificationRejected'
WHERE OrderId = @OrderId;
```
**Usage:** Admin approves or rejects customer-submitted payment proofs. Approval advances the order to Processing; rejection returns the order to Pending so the customer can resubmit.

---

### **INSERT — Inventory Audit Log**
*Source: `AdminSystem_v2/Repositories/POSRepository.cs`*
```sql
INSERT INTO InventoryLog
    (ProductId, ProductVariantId, OrderId,
     ChangeQuantity, ChangeType, ChangedByUserId, Notes, CreatedAt)
VALUES
    (@ProductId, @VariantId, @OrderId,
     @Qty, @ChangeType, @CashierId, @Notes, GETUTCDATE())
```
**Usage:** Creates an immutable audit trail entry for every stock change. Negative `ChangeQuantity` for sales, positive for purchases. Linked to the originating order and the staff member who initiated the change.

---

### **INSERT — Customer Notification Queuing**
*Source: `AdminSystem_v2/Repositories/OrderRepository.cs`*
```sql
INSERT INTO Notification
    (UserId, OrderId, Channel, NotifType, Recipient,
     Subject, Body, Status, RetryCount, CreatedAt, IsRead)
VALUES
    (@UserId, @OrderId, 'Email', @NotifType, @Email,
     @Subject, @Body, 'Pending', 0, GETUTCDATE(), 0)
```
**Usage:** Queues an email notification for the customer atomically within the same transaction as the order status change. Background jobs process pending notifications with retry logic (max 3 attempts).

---

### **DELETE — Not Used Directly**
The system uses **soft-delete patterns** (`IsDeleted` flag) rather than physical `DELETE` statements for orders, payments, and users. Cart items are physically removed via EF Core's `Remove()` method when customers clear items from their cart.

---

### **JOIN — Delivery Detail View**
*Source: `SQL/Schema/Taurus_schema_10.0.sql` (vw_DeliveryDetail)*
```sql
CREATE VIEW vw_DeliveryDetail AS
SELECT
    d.DeliveryId, d.OrderId, d.Courier,
    d.DeliveryStatus, d.IsDelayed, d.DelayedUntil,
    d.EstimatedDeliveryTime, d.ActualDeliveryTime, d.CreatedAt,
    lm.BookingRef   AS LalamoveBookingRef,
    lm.DriverName, lm.DriverPhone,
    lbc.TrackingNumber AS LbcTrackingNumber
FROM Delivery d
LEFT JOIN LalamoveDelivery lm  ON d.DeliveryId = lm.DeliveryId
LEFT JOIN LBCDelivery      lbc ON d.DeliveryId = lbc.DeliveryId;
```
**Usage:** Combines the base `Delivery` table with both courier-specific subtype tables into a single flat view. Columns for the non-applicable courier are NULL. Used by the admin order detail panel to display driver info (Lalamove) or tracking numbers (LBC).

---

# **4. System Capabilities and Limitations**

## **Capabilities (Implemented and Observable in Code)**

* **Processes customer orders via `CheckoutController` + `IOrderService`** — Supports delivery (Lalamove/LBC), pickup, and walk-in fulfillment types.
* **Manages product catalog via `ProductController`** — Full CRUD with categories, brands, variants, image galleries, and bike-specific specs (wheel size, brake type, suspension, etc.).
* **Handles variant-level inventory via `ProductVariant.StockQuantity`** — Stock is tracked at the variant level with automatic deduction on sale and purchase order receipts.
* **Supports three payment methods** — GCash (screenshot upload), BankTransfer (deposit slip upload), and Cash (POS-only via `POSRepository`).
* **Provides POS walk-in sales via `POSViewModel`** — Atomic transactions with product search, customer lookup, discount application, and receipt generation.
* **Applies voucher discounts via `VoucherController` + `IVoucherService`** — Validates date ranges, minimum order amounts, global/per-user usage caps, and assignment restrictions.
* **Supports customer wishlists via `WishlistController`** — Toggle, remove, and move-to-cart functionality.
* **Manages customer support tickets via `SupportController`** — Ticketing system with categories, order linking, threaded replies, and admin assignment.
* **Sends multi-channel notifications via `Notification` table + background jobs** — Supports Email, SMS, and InApp channels with retry logic.
* **Tracks order status lifecycle with validation** — Forward-only status transitions enforced by `OrderStatuses.IsValidTransition()` with audit logging.
* **Verifies customer identity via OTP email verification** — Registration requires OTP confirmation before account creation.
* **Implements BCrypt password hashing** — Passwords stored securely with `BCrypt.Net-Next` (work factor 12).
* **Generates admin reports via `ReportViewModel`** — Sales over time, top products, order status breakdown, payment method distribution with OxyPlot charts.
* **Manages supplier procurement via `Supplier` + `PurchaseOrder`** — Tracks purchase orders from suppliers with expected delivery dates.
* **Records complete audit trails** — `OrderStatusAudit` for transitions, `InventoryLog` for stock changes, `PriceHistory` for price changes, `SystemLog` for general events.
* **Cookie-based authentication with claims** — `ClaimTypes.NameIdentifier`, `ClaimTypes.Name`, `ClaimTypes.Email` used across controllers.
* **Anti-forgery token validation** — All POST endpoints use `[ValidateAntiForgeryToken]`.
* **Guest cart support** — Unauthenticated users can browse and add to cart via `GuestSession` with secure cookie tokens.

## **Limitations (Code-Based Observations)**

* **No real-time payment gateway** — GCash and BankTransfer payments require manual admin verification via `ApprovePaymentAsync` / `RejectPaymentAsync`.
* **No mobile native app** — The web interface is responsive but not accompanied by a native mobile application.
* **No real-time courier integration** — Delivery tracking (Lalamove/LBC) is managed manually by admin; no API fetches live driver positions or tracking updates.
* **Guest checkout is limited** — Guests can add items to cart but must register (with OTP verification) before placing an order.
* **No email password reset** — The codebase shows `ChangePassword` requiring the current password; there is no forgot-password/email-reset flow.
* **Hardcoded shipping fees** — `CheckoutViewModel.LalamoveFee` and `CheckoutViewModel.LBCFee` are constants, not calculated dynamically based on distance or weight.
* **Reports are desktop-only** — Sales and inventory reports can only be generated through the WPF AdminSystem, not the web interface.
* **Single-store design** — The system does not support multi-branch or multi-warehouse configurations.
* **No automated reorder alerts** — `ProductVariant.ReorderThreshold` exists in the schema but automated low-stock purchasing is not implemented.
* **No product variant images** — Images are linked at the product level, not per-variant.
* **Limited refund workflow** — The `Refund` table exists in the schema but full refund processing logic is not extensively implemented in the current controllers.

---

# **5. Unique Features of the System**

The following features exist in the system and are somewhat uncommon for a capstone-level ordering system:

* **Dual-Platform Architecture with Shared Database** — The system uniquely combines an ASP.NET Core MVC web storefront (EF Core) and a WPF MVVM desktop admin app (Dapper), both operating on the same SQL Server database. This provides a real-world multi-client architecture.

* **Table-Per-Type (TPT) Inheritance for Payments and Deliveries** — Payment method-specific data (GCash screenshots, BankTransfer deposit slips) is stored in subtype tables (`GCashPayment`, `BankTransferPayment`), and delivery courier-specific data (`LalamoveDelivery`, `LBCDelivery`) follows the same pattern. This is a sophisticated OOP-influenced database design pattern.

* **Computed Persisted Total Amount** — `Order.TotalAmount` is defined as `AS ((SubTotal - DiscountAmount) + ShippingFee) PERSISTED`, meaning SQL Server automatically calculates and physically stores the value. This prevents calculation errors and allows efficient indexing.

* **Atomic POS Transaction with Full Audit Trail** — The `CreatePOSSaleAsync` method performs 7 operations in a single database transaction: stock validation, order number generation, order creation, item insertion, stock deduction, inventory logging, and payment recording. This ensures complete data consistency.

* **Order Status Audit with Rejected Transitions** — The `OrderStatusAudit` table logs both successful and rejected status transitions, with the `Success` bit flag and `Reason` column, providing full accountability. The system validates transitions against a forward-only state machine before applying them.

* **Bike-Specific Product Specs in Schema** — Unlike generic e-commerce systems, the `Product` table includes domain-specific columns like `WheelSize`, `SpeedCompatibility`, `BoostCompatible`, `TubelessReady`, `AxleStandard`, `SuspensionTravel`, and `BrakeType` — reflecting actual bike shop product attributes.

* **Google Cloud Storage Integration** — Product images and payment proofs are stored in Google Cloud Storage with bucket/path tracking in the database, providing scalable cloud-native file storage.

* **Voucher System with User Assignment** — Beyond simple promo codes, the `UserVoucher` table enables targeted voucher distribution to specific customers with individual expiry dates, used by admin through the `VoucherViewModel`.

* **12 Pre-Built Database Views** — The schema includes 12 optimized views (`vw_OrderSummary`, `vw_PaymentDetail`, `vw_DeliveryDetail`, `vw_ActiveProducts`, etc.) that pre-join complex queries, reducing application-layer complexity and improving query performance.

* **Multi-Channel Notification System with Retry Logic** — The `Notification` table supports Email, SMS, and InApp channels with a retry counter (max 3 attempts), failure reason tracking, and read receipts — implementing a production-grade notification infrastructure.

---

# **6. Tech Stack (Auto-Detected)**

| Component | Technology | Version |
|---|---|---|
| **Language** | C# | .NET 8.0 |
| **Web Framework** | ASP.NET Core MVC | 8.0 |
| **Desktop Framework** | WPF (Windows Presentation Foundation) | .NET 8.0-windows |
| **Database** | Microsoft SQL Server | 2019+ (Google Cloud SQL) |
| **Web ORM** | Entity Framework Core (SqlServer Provider) | 8.0.0 |
| **Admin ORM** | Dapper | 2.1.35 |
| **Authentication** | Cookie Authentication (ASP.NET Core) | Built-in |
| **Password Hashing** | BCrypt.Net-Next | 4.0.3 |
| **Email Service** | MailKit | 4.3.0 |
| **Cloud Storage** | Google Cloud Storage (google-cloud-dotnet) | 4.7.0 |
| **Image CDN** | Cloudinary (CloudinaryDotNet) | 1.28.0 |
| **Charting Library** | OxyPlot.WPF | 2.1.0 |
| **DB Client (Admin)** | Microsoft.Data.SqlClient | 5.2.1 |
| **Configuration** | Microsoft.Extensions.Configuration (JSON, UserSecrets, Env) | 8.0.0 |
| **Architecture (Web)** | MVC (Model-View-Controller) | — |
| **Architecture (Admin)** | MVVM (Model-View-ViewModel) | — |
| **Version Control** | Git + GitHub | — |
| **Containerization** | Docker (Dockerfile present) | — |
| **IDE** | Microsoft Visual Studio | — |

---

# **7. References**

* **Microsoft .NET 8 Documentation** — https://learn.microsoft.com/en-us/dotnet/
* **ASP.NET Core MVC Documentation** — https://learn.microsoft.com/en-us/aspnet/core/
* **Entity Framework Core Documentation** — https://learn.microsoft.com/en-us/ef/core/
* **Dapper Documentation** — https://github.com/DapperLib/Dapper
* **WPF Documentation** — https://learn.microsoft.com/en-us/dotnet/desktop/wpf/
* **Microsoft SQL Server Documentation** — https://learn.microsoft.com/en-us/sql/sql-server/
* **Google Cloud Storage Client Libraries** — https://cloud.google.com/storage/docs/reference/libraries
* **Cloudinary .NET SDK** — https://cloudinary.com/documentation/dotnet_integration
* **BCrypt.Net-Next** — https://github.com/BcryptNet/bcrypt.net
* **MailKit Documentation** — https://github.com/jstedfast/MailKit
* **OxyPlot Documentation** — https://oxyplot.github.io/
* **Git Version Control** — https://git-scm.com/
* **GitHub** — https://github.com/
