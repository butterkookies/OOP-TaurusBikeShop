# Taurus Bike Shop — Architectural & Operational Blueprint

---

## PILLAR 1 — ARCHITECTURE OVERVIEW (The City Map)

### 1a. Purpose of Each Application

**WebApplication** is the customer-facing e-commerce storefront built with ASP.NET Core MVC on .NET 8. It allows customers to browse the product catalog, manage a shopping cart (as a guest or authenticated user), check out with GCash or bank transfer, upload payment proofs, track orders, submit support tickets, leave product reviews, and manage their profile and addresses. *Think of it as the showroom and cash register that faces the street.*

**AdminSystem_v2** is the internal desktop application built with WPF MVVM on .NET 8. Staff use it for point-of-sale (POS) transactions, order management (status transitions, payment verification), inventory management, product/category/brand CRUD, supplier and purchase order management, report generation with charts, voucher management, support ticket handling, and staff account administration. *Think of it as the manager's office behind the showroom.*

### 1b. How the Two Systems Communicate

The two applications share a **single SQL Server database** hosted on Google Cloud SQL (`35.221.161.150:1433`). There is **no direct API, message queue, or service bus** between them. They communicate exclusively through the shared database — one writes a row, the other reads it on its next query or polling cycle.

**Synchronization pattern: Indirect via DB + background jobs.** For example:
- A customer places an order on the WebApp → writes an [Order](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/Controllers/CheckoutController.cs#124-196) row with status [Pending](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/BackgroundJobs/NotificationDispatchJob.cs#71-133).
- The AdminSystem polls orders and sees the new order → staff updates it to `Processing`.
- The WebApp's [PendingOrderMonitorJob](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/BackgroundJobs/PendingOrderMonitorJob.cs#26-300) background job checks every 5 minutes for Pending orders older than 24 hours and auto-cancels them.

### 1c. Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                       CLIENT LAYER                              │
│                                                                 │
│  ┌──────────────────┐              ┌──────────────────────┐     │
│  │  Browser (Chrome) │              │  WPF Desktop (Win)   │     │
│  │  HTML/CSS/JS/jQ   │              │  XAML + C#            │     │
│  │  Bootstrap 5 CDN  │              │  OxyPlot Charts       │     │
│  └────────┬─────────┘              └─────────┬────────────┘     │
└───────────┼──────────────────────────────────┼──────────────────┘
            │ HTTP/HTTPS                       │ Direct SQL (Dapper)
┌───────────┼──────────────────────────────────┼──────────────────┐
│           │     APPLICATION LAYER            │                  │
│  ┌────────▼─────────────┐        ┌───────────▼──────────────┐   │
│  │   ASP.NET Core MVC   │        │     WPF MVVM (.NET 8)    │   │
│  │  ┌─────────────────┐ │        │  ┌────────────────────┐  │   │
│  │  │  Controllers(12)│ │        │  │   Views (XAML)(12)  │  │   │
│  │  │  Services (13)  │ │        │  │  ViewModels (14)    │  │   │
│  │  │  Repos (14, EF) │ │        │  │  Services (12)      │  │   │
│  │  │  Jobs (6)       │ │        │  │  Repos (10, Dapper) │  │   │
│  │  └─────────────────┘ │        │  └────────────────────┘  │   │
│  └────────┬─────────────┘        └───────────┬──────────────┘   │
└───────────┼──────────────────────────────────┼──────────────────┘
            │ EF Core 8                        │ Dapper 2.1
┌───────────┼──────────────────────────────────┼──────────────────┐
│           ▼        DATA LAYER                ▼                  │
│         ┌─────────────────────────────────────┐                 │
│         │    SQL Server on Google Cloud SQL    │                 │
│         │   43 tables · 11 domains · 38 DbSets│                 │
│         └─────────────────────────────────────┘                 │
└─────────────────────────────────────────────────────────────────┘
            ┌───────────────────────────────────┐
            │       EXTERNAL SERVICES           │
            │  Google Cloud Storage (assets)    │
            │  Gmail SMTP via MailKit (email)   │
            │  Cloudinary (product images)      │
            │  Lalamove API (STUBBED)           │
            │  LBC API (STUBBED)                │
            └───────────────────────────────────┘
```

### 1d. Why One Shared Database?

**Tradeoff: Consistency vs. Coupling.** A single database guarantees that both systems always see the same order statuses, inventory levels, and payment records in real time without replication lag. The cost is tight coupling — a schema change affects both applications, and both must be updated in lockstep. For a small-team bike shop project, the simplicity of one database far outweighs the operational overhead of synchronizing two separate databases.

---

## PILLAR 2 — DATABASE & SCHEMA (The Central Filing Room)

### 2a. The 11 Table Domains

| # | Domain | Tables | Business Data |
|---|--------|--------|---------------|
| 1 | **Identity** | User, Role, UserRole, Address, OTPVerification, GuestSession | Customer/staff accounts, roles, saved addresses, email/SMS verification codes, anonymous browser sessions |
| 2 | **Product Catalog** | Category, Brand, Product, ProductVariant, ProductImage, PriceHistory | Bikes, parts, accessories — each product has variants (size/color) with independent stock and prices |
| 3 | **Supply Chain** | Supplier, PurchaseOrder, PurchaseOrderItem | Vendor records, inbound purchase orders for restocking |
| 4 | **Vouchers/Promotions** | Voucher, UserVoucher, VoucherUsage | Discount codes, per-user assignment and redemption tracking |
| 5 | **Cart & Wishlist** | Cart, CartItem, Wishlist | Shopping cart (supports guest + auth), saved-for-later items |
| 6 | **Orders** | Order, OrderItem, PickupOrder | Customer orders with line items; PickupOrder is a 1:1 subtype for walk-in collection |
| 7 | **Inventory** | InventoryLog | Append-only ledger of every stock change (receive, sell, adjust, lock, unlock) |
| 8 | **Payments** | Payment, GCashPayment, BankTransferPayment, StorePaymentAccount | Payment records with method subtypes; store's own GCash/bank account details |
| 9 | **Delivery** | Delivery, LalamoveDelivery, LBCDelivery | Shipment records with courier-specific subtypes for booking references and tracking |
| 10 | **Reviews & POS** | Review, POS_Session | Product reviews; point-of-sale transaction sessions for walk-in customers |
| 11 | **Support & Audit** | SupportTicket, SupportTicketReply, SupportTask, Notification, SystemLog | Customer help desk, internal task assignments, notification queue, system event audit trail |

### 2b. The 5 Shared-PK 1:1 Subtypes

These entities use the **shared primary key** pattern — their PK is also a foreign key to the parent table. In EF Core, `ValueGeneratedNever()` tells the framework "don't auto-generate this key; I will set it myself to match the parent's PK." *It's like a file folder that has the same label number as the master record it extends.*

| Subtype | Parent | Purpose |
|---------|--------|---------|
| `GCashPayment` | Payment | GCash reference number, screenshot URL |
| `BankTransferPayment` | Payment | Reference number, proof screenshot, verification deadline |
| `LalamoveDelivery` | Delivery | Booking ref, driver name/phone, vehicle type |
| `LBCDelivery` | Delivery | Tracking number (waybill), branch info |
| `PickupOrder` | Order | Scheduled pickup date/time, pickup notes |

Configured in [AppDbContext.Payments.cs](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/DataAccess/Context/AppDbContext.Payments.cs), [AppDbContext.Delivery.cs](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/DataAccess/Context/AppDbContext.Delivery.cs), and [AppDbContext.Orders.cs](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/DataAccess/Context/AppDbContext.Orders.cs) via `HasOne(...).WithOne(...).HasForeignKey<T>(x => x.ParentId)` with `.ValueGeneratedNever()`.

### 2c. Circular FK: User ↔ Address

`User.DefaultAddressId` points to `Address.AddressId`, while `Address.UserId` points back to `User.UserId`. This creates a circular dependency. The solution: `User.DefaultAddressId` is configured with `IsRequired(false)` (nullable FK) and `OnDelete(DeleteBehavior.NoAction)` in [AppDbContext.Auth.cs](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/DataAccess/Context/AppDbContext.Auth.cs). *Without this, SQL Server would refuse to create the tables because it can't determine which delete to cascade first.*

### 2d. Two Most Important Cascade Rules

1. **Order → OrderItem (Cascade Delete):** Deleting an Order automatically deletes all its OrderItems. Business impact: ensures no orphaned line items exist if an order record is removed.
2. **User → Address (NoAction on DefaultAddressId):** Prevents cascade cycles. Business impact: deleting an address doesn't accidentally cascade to the user, and deleting a user doesn't trigger conflicting cascades through two paths.

### 2e. Schema is Read-Only from Application Code

The schema is managed externally via SQL scripts in `SQL\Schema\`. No application code ever runs `ALTER TABLE`, `DROP TABLE`, or `CREATE TABLE`. The schema file `Taurus_schema_10.0.sql` is the single source of truth. EF Core Migrations exist in a `Migrations/` folder but are not used for production — the schema is applied directly via SSMS.

---

## PILLAR 3 — DATA I/O & HANDLERS (The Gatekeepers)

### 3a. The 5 Most Data-Intensive User-Facing Forms

| Form | Controller Action | Key Data |
|------|------------------|----------|
| **Customer Registration** | `CustomerController.Register` (POST) | Name, email, phone, password, OTP verification |
| **Checkout** | `CheckoutController.PlaceOrder` (POST) | Address selection, delivery method, payment method, voucher, all cart items |
| **Payment Proof Upload** | `PaymentController.UploadProof` (POST) | Multipart file upload (image/PDF up to 15 MB) + order reference |
| **Support Ticket** | `SupportController.Create` (POST) | Subject, description, category, priority, optional file attachments |
| **Product Review** | `ReviewController.Create` (POST) | Rating (1–5), review text, product ID, order verification |

### 3b. Read Path: Product Catalog Page Load

```
Browser GET /Product/List?categoryId=2&page=1
  → ProductController.List()
    → IProductService.GetFilteredAsync()
      → ProductRepository (inherits Repository<Product>)
        → AppDbContext.Products (DbSet<Product>)
          → EF Core generates SQL: SELECT ... FROM Products JOIN ProductVariants ...
            → SQL Server on Google Cloud SQL
              ← Result rows returned
          ← EF Core materializes Product entities (NoTracking)
        ← Repository returns IReadOnlyList<ProductViewModel>
      ← Service returns (products, totalCount)
    ← Controller sets ViewBag (brands, categories, pagination)
  ← View ~/Views/Customer/ProductCatalog.cshtml renders HTML
← Browser receives HTML page with product grid
```

### 3c. Write Path: Order Creation

```
Browser POST /Checkout/PlaceOrder (form data + CSRF token)
  → CheckoutController.PlaceOrder()
    → Validates anti-forgery token (global filter)
    → Parses selected cart item IDs
    → IOrderService.CreateOrderAsync(userId, addressId, items, paymentMethod, voucherId)
      → OrderRepository.CreateOrderAsync()
        → AppDbContext.BeginTransactionAsync()
          → INSERT Order → INSERT OrderItems → UPDATE ProductVariant.StockQuantity
          → INSERT InventoryLog (Lock entries) → INSERT Payment
          → If delivery: INSERT Delivery + courier subtype
          → If voucher: INSERT VoucherUsage
        → AppDbContext.SaveChangesAsync()
        → Transaction.CommitAsync()
      ← Returns Order with OrderNumber
    ← Service returns success result
  ← Controller clears voucher session, redirects to OrderConfirmation
← Browser receives redirect → GET /Order/Confirmation/{id}
```

### 3d. Validation Layers

1. **Model Binding (ASP.NET Core):** `[Required]`, `[MaxLength]`, `[Range]` data annotations on entity classes and view models. Invalid input is caught before the controller action executes.
2. **Service Layer:** Business rule validation (e.g., "is this voucher still valid?", "does this variant have enough stock?"). Returns structured error messages.
3. **Database Constraints:** UNIQUE indexes, FK constraints, CHECK constraints defined in the SQL schema. These are the last line of defense.

### 3e. AJAX: The ApiResponse Envelope Pattern

Every AJAX response from the WebApp uses `ApiResponse` ([ApiResponse.cs](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/Models/ApiResponse.cs)):

```csharp
// Success: return Json(ApiResponse.Ok(new { cartCount = 3 }, "Added to cart!"));
// Failure: return Json(ApiResponse.Fail("Voucher code is expired."));
```

Shape: `{ success: bool, message: string?, data: object? }`

The frontend uses `fetchWithCSRF(url, data)` from [utils.js](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/wwwroot/js/utils.js) for all AJAX calls. It reads the `__RequestVerificationToken` hidden input from the page and includes it as a `RequestVerificationToken` header. **Why?** Every POST action has a global `[AutoValidateAntiforgeryToken]` filter (registered in [Program.cs:L303](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/Program.cs#L303)). Without the token, AJAX POSTs would be rejected with HTTP 400. The CSRF token prevents cross-site request forgery — *where a malicious website tricks your browser into submitting a form to Taurus while you're logged in.*

---

## PILLAR 4 — THIRD-PARTY INTEGRATIONS (The Outside Hires)

### 4a. Service Inventory

| Service | Purpose | How Called | What It Returns |
|---------|---------|------------|-----------------|
| **Google Cloud SQL** | Database hosting | SQL Server connection string in appsettings, EF Core + Dapper connect directly | Query results, rows affected |
| **Google Cloud Storage** | Product images, payment proofs, support attachments | `FileUploadHelper.cs` → `StorageClient.UploadObjectAsync()` | `UploadResult` record (PublicUrl, StorageBucket, StoragePath) |
| **Cloudinary** | Product image hosting (alternative to GCS) | `PhotoService.cs` → Cloudinary .NET SDK | Upload result with public URL |
| **Gmail SMTP (MailKit)** | Transactional email: OTP codes, order confirmations, reminders, delivery updates | `GmailEmailSender.SendAsync()` via `NotificationDispatchJob` | Success/exception |
| **Lalamove API** | Delivery booking and tracking for Metro Manila | **STUBBED** — `PollLalamoveAsync()` returns `null` | Would return delivery status string |
| **LBC API** | Delivery booking and tracking for provincial | **STUBBED** — `PollLBCAsync()` returns `null` | Would return delivery status string |

### 4b. What "Stubbed" Means

*"Stubbed" means the code structure exists but the actual API calls are replaced with placeholder methods that do nothing.* The methods `PollLalamoveAsync()` and `PollLBCAsync()` in [DeliveryStatusPollJob.cs](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/BackgroundJobs/DeliveryStatusPollJob.cs) log a debug message and return `null` (no status change). In the meantime: delivery tracking is manual — admin staff update delivery statuses by hand in the AdminSystem. The courier auto-assignment logic (Lalamove for Metro Manila provinces, LBC for provincial) is implemented server-side in `OrderService`, but the actual booking API call is not wired.

### 4c. Google Cloud Storage Credentials

GCS uses **Application Default Credentials (ADC)**. In production on Google Cloud infrastructure, credentials are resolved automatically. For local development, the `GOOGLE_APPLICATION_CREDENTIALS` environment variable must point to a service account JSON key file. The bucket name (`taurus-bikeshop-assets`) is stored in `appsettings.json` under `GoogleCloudStorage:BucketName`. Credentials are **never hardcoded** — if GCS is unavailable at startup, the app logs a warning and continues (support-ticket attachments disabled, but product images fall back to Cloudinary).

---

## PILLAR 5 — BACKGROUND JOBS (The Invisible Night Shift)

The WebApp registers **6** `IHostedService` background jobs in [Program.cs:L273–278](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/Program.cs#L273-L278):

| # | Class | Interval | Reads | Writes | Business Impact |
|---|-------|----------|-------|--------|-----------------|
| 1 | `InventorySyncJob` | 60 seconds | ProductVariants (count) | SystemLog (throttled to 5min) | Keeps a live inventory count available; detects variants with zero stock |
| 2 | `PendingOrderMonitorJob` | 5 minutes | Orders (Pending), Notifications | Order.OrderStatus → Cancelled, InventoryLog (Unlock), SystemLog, Notification queue | Auto-cancels unpaid orders after 24h; sends 4h pre-expiry reminder emails. Unlocks reserved stock. |
| 3 | `PaymentTimeoutJob` | 5 minutes | Orders (PaymentVerification), BankTransferPayment | Order.OrderStatus → OnHold, SystemLog, Notification queue | Moves orders to OnHold when bank transfer verification deadline expires |
| 4 | `StockMonitorJob` | 15 minutes | ProductVariants (below ReorderThreshold) | SystemLog (LowStockTriggered), Notification queue | Alerts admin + customer when variants drop below reorder threshold; 24h cooldown per variant |
| 5 | `DeliveryStatusPollJob` | 5 minutes | Deliveries (PickedUp/InTransit), LalamoveDelivery, LBCDelivery | Delivery.DeliveryStatus, SystemLog, Notification queue | Polls courier APIs for status updates; currently **stubbed** — no real API calls |
| 6 | `NotificationDispatchJob` | 60 seconds | Notifications (Pending, retryCount < 3) | Notification.Status → Sent/Failed, SentAt, FailureReason | Dispatches queued email notifications via Gmail SMTP; 3 retries, batch of 10 |

### 5a. DeliveryStatusPollJob — Known Integration State

The original LINQ bug (HashSet<T>.Contains() in .Where()) referenced in the prompt has been **fixed**. The current query in [DeliveryStatusPollJob.cs:L123–124](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/BackgroundJobs/DeliveryStatusPollJob.cs#L123-L124) uses explicit `||` conditions:
```csharp
.Where(d => d.DeliveryStatus == DeliveryStatuses.PickedUp
         || d.DeliveryStatus == DeliveryStatuses.InTransit)
```
This translates cleanly to SQL `WHERE DeliveryStatus IN ('PickedUp', 'InTransit')`.

### 5b. What Is IHostedService?

*An `IHostedService` is a long-running background task that starts automatically when the web application boots and stops when it shuts down — like a night-shift worker who clocks in when the store opens and clocks out when it closes.* Each job runs independently on its own thread, creating a new DI scope per cycle to get a fresh `AppDbContext`.

All 6 jobs are registered in `Program.cs` via `builder.Services.AddHostedService<T>()`.

### 5c. SystemLog Startup & IP Whitelist Issue

Every job writes a `SystemLog` entry on its first cycle. This writes to SQL Server on Google Cloud SQL. In development, if the developer's IP address is **not whitelisted** in the Google Cloud Console firewall rules, the database connection fails — causing the SystemLog write to throw. All jobs handle this gracefully with a try/catch that logs a warning and continues (`context.ChangeTracker.Clear()` in PendingOrderMonitorJob to prevent the dirty entity from contaminating future saves). **This is expected in development and safe to ignore — no code change needed.**

All jobs use **staggered startup delays** (2s, 5s, 8s, 11s, 14s, 17s) to prevent all 6 from hitting the database simultaneously on boot. They also implement **exponential backoff** on consecutive failures, capped at 2 minutes.

---

## PILLAR 6 — DESIGN SYSTEM & UX STANDARDS (The Brand Wardrobe)

### 6a. Brand Identity

NBA Chicago Bulls energy translated to UI: **bold, sporty, premium.** The design mood draws from the intersection of *Rapha* (cycling elegance), *Ducati* (performance aggression), and *Apple Vision Pro* (spatial glass aesthetics). The result is a high-contrast, glassmorphic interface that feels like a premium sports showroom.

### 6b. Color System

| Token | Value | Usage |
|-------|-------|-------|
| **Background** | Deep blue-black gradient (`#0a0e1a` → `#1a1f35`) | NOT flat black — the gradient creates depth perception essential for glassmorphism. Flat black makes glass surfaces look like grey boxes. |
| **Primary Accent** | Crimson `#DC143C` | ONE element per section maximum — buttons, active states, hero highlights. Overuse dilutes impact. |
| **Glass Surface** | `rgba(255,255,255,0.06)` + `backdrop-filter: blur(16px)` | Cards, modals, dropdowns — the signature look |
| **Text Primary** | `#F0F0F0` | Headings, body text, labels |
| **Text Secondary** | `rgba(240,240,240,0.65)` | Captions, metadata, helper text |

### 6c. Typography Stack

| Font | Role | Character |
|------|------|-----------|
| **Bebas Neue** / Barlow Condensed | Display, headings, hero text | Condensed, athletic — echoes jersey numbers |
| **DM Sans** | Body text, UI labels, form inputs | Clean, highly legible at small sizes |
| **Oswald** | Prices, stats, performance numbers | Numerical impact — looks like a scoreboard |

All loaded via Google Fonts CDN — no local font files.

### 6d. Glass Elevation Tiers

| Tier | Opacity | Blur | Usage |
|------|---------|------|-------|
| Tier 0 | 0% | None | Page background |
| Tier 1 | 4–6% white | 12px | Section containers, page wrappers |
| Tier 2 | 8–10% white | 16px | Cards, content blocks |
| Tier 3 | 12–15% white | 20px | Modals, dropdowns, popovers |
| Tier 4 | 18–20% white | 24px | Critical overlays, active element highlights |

**Why skipping tiers breaks hierarchy:** If a card (Tier 2) and a modal (Tier 3) use the same opacity, the modal doesn't visually "float" above the card. The progressive increase in opacity and blur creates a consistent z-axis depth that users intuitively understand.

### 6e. WebApp Frontend Stack

- **Vanilla JavaScript** — no framework (React, Vue, etc.)
- **Bootstrap 5** — layout grid, utility classes (loaded via CDN)
- **jQuery 3.x** — DOM manipulation, AJAX helpers (loaded via CDN)
- **Google Fonts CDN** — typography
- **No frontend build step** — no Webpack, Vite, or npm for the application itself. JS and CSS files are served directly from `wwwroot/`.

### 6f. AdminSystem Design Consistency

AdminSystem uses WPF XAML with a custom dark theme defined in [App.xaml](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/AdminSystem_v2/App.xaml) (22,354 bytes of styles and resource dictionaries). OxyPlot is used for dashboard charts. Visual consistency between desktop and web is maintained through:
1. **Shared color palette** — the same crimson `#DC143C`, dark backgrounds, and text colors
2. **Similar typography** — XAML uses similar font families
3. **Shared brand language** — both apps use the same terminology, status labels, and visual hierarchy patterns

---

## PILLAR 7 — WEB APP FRONTEND FLOW (The Customer Storefront)

### Complete Customer Journey

| Step | Action | Controller | Action Method | View File | Key Service/Repo |
|------|--------|------------|--------------|-----------|-----------------|
| 1 | Browse catalog | `ProductController` | `List` (GET) | `~/Views/Customer/ProductCatalog.cshtml` | `ProductService`, `BrandService` |
| 2 | View product detail | `ProductController` | `Detail` (GET) | `~/Views/Customer/ProductDetails.cshtml` | `ProductService` |
| 3 | Add to cart (AJAX) | `CartController` | `AddToCart` (POST) | — (JSON response) | `CartService`, `CartRepository` |
| 4 | View cart | `CartController` | `ViewCart` (GET) | `~/Views/Customer/Cart.cshtml` | `CartService` |
| 5 | Apply voucher (AJAX) | `CheckoutController` | (voucher validation) | — (JSON response) | `VoucherService` |
| 6 | Checkout | `CheckoutController` | `Index` (GET) | `~/Views/Customer/Checkout.cshtml` | `CartService`, `OrderService` |
| 7 | Select payment | `CheckoutController` | `PlaceOrder` (POST) | — (redirect) | `OrderService`, `PaymentService` |
| 8 | Upload payment proof | `PaymentController` | `UploadProof` (POST) | `~/Views/Customer/PaymentUpload.cshtml` | `PaymentService`, `FileUploadHelper` → GCS |
| 9 | Payment verification | — (background) | `PaymentTimeoutJob` + admin via AdminSystem | — | `PaymentTimeoutJob`, admin OrderViewModel |
| 10 | Order confirmation | `OrderController` | `Confirmation` (GET) | `~/Views/Customer/OrderHistory.cshtml` | `OrderService` |

### 7a. Guest vs Authenticated Cart

- **Guest users** get a `GuestSession` record created via `CartController.EnsureGuestSessionAsync()`. The session ID is stored in a browser cookie. Cart items are linked to the `GuestSession.GuestSessionId`.
- **Authenticated users** have cart items linked directly to their `UserId`.
- **On login**, the `CartService` merges guest cart items into the user's authenticated cart — items are re-assigned from the guest session to the user ID. The guest session is then marked as expired.

### 7b. Anti-Forgery Tokens

*An anti-forgery token is a secret code embedded in every form that proves the POST request really came from your own page, not from a malicious website.* ASP.NET Core generates a unique token pair — one in a cookie, one in a hidden `<input>` field. When the form is submitted, both must match. The global `AutoValidateAntiforgeryTokenAttribute` filter in `Program.cs` enforces this on every POST action.

### 7c. fetchWithCSRF()

`fetchWithCSRF(url, data)` in [utils.js](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/wwwroot/js/utils.js#L9-L22) does three things:
1. Finds the `__RequestVerificationToken` hidden input on the page
2. Includes it as a `RequestVerificationToken` HTTP header
3. Sends a `POST` with `Content-Type: application/json` and the serialized data body

It exists because standard `fetch()` doesn't automatically include the CSRF token header, and without it, the global anti-forgery filter would reject every AJAX POST with HTTP 400.

### 7d. AJAX Response Handling

All AJAX responses follow `ApiResponse { success, message, data }`. Frontend code pattern:
```javascript
fetchWithCSRF('/Cart/AddToCart', { productId: 42, variantId: 7, quantity: 1 })
    .then(function(res) {
        if (res.success) {
            showToast('success', res.message);  // "Added to cart!"
            updateCartBadge(res.data.cartCount);
        } else {
            showToast('error', res.message);    // "Out of stock."
        }
    });
```
