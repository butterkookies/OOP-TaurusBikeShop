# Taurus Bike Shop — Project Structure Guide

**Solution:** `OOP-TaurusBikeShop`  
**Sub-projects:** `WebApplication` (ASP.NET Core 8.0 MVC) · `AdminSystem_v2` (WPF .NET 8.0)  
**Database:** SQL Server on Google Cloud — 42 tables, 12 views (see `Database-guide.md`)

This guide explains every folder and file in the solution — what it does, why it exists, and how it connects to everything else. Read it top to bottom the first time; afterwards use it as a reference when you are adding a feature or tracing a bug.

---

## How to read this guide

Each folder or file section answers three questions:

| Label | What it answers |
|-------|-----------------|
| **What it does** | The concrete job this code performs |
| **Why it exists** | The design or architectural reason it was separated out |
| **Key members** | The classes, methods, or files worth understanding in detail |

---

## Table of contents

1. [Solution overview](#1-solution-overview)
2. [Root-level files and folders](#2-root-level-files-and-folders)
3. [WebApplication — entry point and configuration](#3-webapplication--entry-point-and-configuration)
4. [WebApplication — Controllers](#4-webapplication--controllers)
5. [WebApplication — DataAccess](#5-webapplication--dataaccess)
6. [WebApplication — Models](#6-webapplication--models)
7. [WebApplication — BusinessLogic](#7-webapplication--businesslogic)
8. [WebApplication — BackgroundJobs](#8-webapplication--backgroundjobs)
9. [WebApplication — Utilities](#9-webapplication--utilities)
10. [WebApplication — Views and wwwroot](#10-webapplication--views-and-wwwroot)
11. [AdminSystem_v2 — entry point and configuration](#11-adminsystem_v2--entry-point-and-configuration)
12. [AdminSystem_v2 — Models](#12-adminsystem_v2--models)
13. [AdminSystem_v2 — Repositories](#13-adminsystem_v2--repositories)
14. [AdminSystem_v2 — Services](#14-adminsystem_v2--services)
15. [AdminSystem_v2 — ViewModels](#15-adminsystem_v2--viewmodels)
16. [AdminSystem_v2 — Views](#16-adminsystem_v2--views)
17. [AdminSystem_v2 — Converters](#17-adminsystem_v2--converters)
18. [AdminSystem_v2 — Helpers](#18-adminsystem_v2--helpers)
19. [AdminSystem_v2 — Migrations](#19-adminsystem_v2--migrations)
20. [Shared design patterns](#20-shared-design-patterns)
21. [Infrastructure and deployment](#21-infrastructure-and-deployment)

---

## 1. Solution overview

The solution is split into two separate applications that share a single SQL Server database.

| Sub-project | Framework | Who uses it | Deployment |
|-------------|-----------|-------------|------------|
| `WebApplication` | ASP.NET Core 8.0 MVC (Razor) | Online customers — browse products, checkout, track orders | Google Cloud Run (container) |
| `AdminSystem_v2` | WPF desktop (.NET 8.0-windows) | Store staff and admins — manage orders, inventory, POS, reports | Windows machine at the store |

Both applications read and write the same database tables. This means a purchase made on the website immediately appears in the admin desktop app and vice versa. They use different data-access strategies — Entity Framework Core in the web app and Dapper in the desktop app — because each technology suits its workload better.

---

## 2. Root-level files and folders

These files sit directly inside `OOP-TaurusBikeShop/`.

| File / Folder | Purpose |
|---------------|---------|
| `WebApplication/` | The ASP.NET Core web project |
| `AdminSystem_v2/` | The WPF desktop project |
| `Database-guide.md` | Table-by-table documentation for the SQL Server database |
| `Taurus_seed_5.sql` | Complete database schema + seed data (292 KB) — run this once to create all 42 tables |
| `Taurus-schema-v8.x.sql` | Schema-only versions used during migrations |
| `master-blueprint-prompt.md` | Original product requirements and design decisions (read this to understand *why* features exist) |
| `taurus-blueprint.md` / `taurus-blueprint-part2.md` | Technical specification — API contracts, flowchart references, feature descriptions |
| `feature-list.md` | Numbered feature list cross-referenced to Mermaid flowchart files |
| `Project-structure.md` | This file |
| `Mermaid/Claude/v3.0/` | 17 `.mmd` sequence diagram files — one per major user flow (login, cart, checkout, payment, etc.) |
| `GUIDE/` | Additional architectural notes |
| `.postman/` | Postman collection for testing web app API endpoints |
| `.claude/settings.json` | Claude Code permission settings for this project |

---

## 3. WebApplication — entry point and configuration

### `Program.cs`

**What it does:** Bootstraps the entire web application. Every service, middleware, and configuration the app needs is registered here before the app starts accepting requests.

**Why it exists:** ASP.NET Core uses a builder pattern — you declare everything the app needs before it starts. `Program.cs` is the single place where that declaration lives. If a service is not registered here, injecting it anywhere in the app will throw a runtime exception.

**Key sections:**

| Section | What it configures |
|---------|--------------------|
| `AddDbContextPool` | Registers `AppDbContext` with a pool of 32 reusable instances. Pooling avoids the cost of creating a new EF Core context on every request |
| `MaxPoolSize: 30` | Caps the underlying SQL Server connection pool at 30 open connections, preventing memory exhaustion on Cloud Run |
| `QueryTrackingBehavior.NoTracking` | Tells EF Core not to track entity changes for read-only queries — saves memory and CPU |
| `AddAuthentication / AddCookie` | Cookie-based auth. Session idle timeout is 30 minutes; auth cookie expires after 1 hour with sliding renewal |
| `AddAntiforgery` | Adds CSRF token generation. Combined with `ValidateAntiForgeryToken` on every POST action, this blocks cross-site request forgery |
| `AddHostedService` (×6) | Registers 6 background jobs that run in parallel with the web server. See [Section 8](#8-webapplication--backgroundjobs) |
| `MaxRequestBodySize: 15 MB` | Allows file uploads up to 15 MB (proof-of-payment images, support attachments) |
| `UseSession` | Enables the server-side session store for guest cart tracking |
| `UseCors` | Wide-open CORS policy (AllowAnyOrigin) — intended for development; should be tightened before public production |

### `appsettings.json` / `appsettings.Development.json`

**What it does:** Stores non-sensitive application settings. Sensitive values (passwords, API keys) are supplied at runtime via environment variables or .NET User Secrets — they are never committed to source control.

**Key sections:**

| Key | Purpose |
|-----|---------|
| `ConnectionStrings:DefaultConnection` | SQL Server connection string — empty in source control, injected at deploy time |
| `Cloudinary` | Account name, API key, and secret for the Cloudinary image CDN |
| `GmailSmtp` | Gmail address and app password for sending transactional email |
| `GoogleCloud:BucketName` | Name of the GCS bucket where support-ticket attachments are stored |
| `Session:IdleTimeoutMinutes` | How long a session stays alive without activity |

### `WebApplication.csproj`

**What it does:** Defines the project — target framework, NuGet dependencies, and build settings.

**Key packages:**

| Package | Purpose |
|---------|---------|
| `Microsoft.EntityFrameworkCore.SqlServer` | EF Core provider for SQL Server |
| `BCrypt.Net-Next` | Password hashing with work factor 12 |
| `CloudinaryDotNet` | Image upload and optimization CDN |
| `Google.Cloud.Storage.V1` | File upload to Google Cloud Storage |
| `MailKit` | SMTP client for Gmail transactional email |
| `Newtonsoft.Json` | JSON serialization for API responses |

---

## 4. WebApplication — Controllers

**Location:** `WebApplication/Controllers/`

**What they do:** Controllers receive HTTP requests, call the appropriate service to perform business logic, and return either a rendered Razor view or a JSON response. They own no business logic themselves — they are thin coordinators between the HTTP layer and the service layer.

**Why they exist:** Separating controllers from business logic means you can change how data is fetched or processed without touching the HTTP-handling code, and vice versa.

**Common patterns across all controllers:**
- Constructor injection of one or more service interfaces (e.g., `IProductService`, `ICartService`)
- `int? userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))` to identify the logged-in user
- `[ValidateAntiForgeryToken]` on every POST action
- `CancellationToken cancellationToken` parameter on every async action
- `TempData["Error"]` and `TempData["Success"]` for flash messages to the next request

---

### `HomeController.cs`

**What it does:** Serves the homepage and the error page.

| Action | Route | Returns |
|--------|-------|---------|
| `Index` | `GET /` | Homepage Razor view with featured products |
| `Error` | `GET /Error` | Error page with request ID |

---

### `CustomerController.cs`

**What it does:** Handles everything related to user accounts — registration, login, logout, profile editing, and address management.

| Action | Route | Notes |
|--------|-------|-------|
| `Register` (GET) | `GET /Customer/Register` | Shows registration form |
| `Register` (POST) | `POST /Customer/Register` | Validates form, calls `IUserService.RegisterAsync`, sends OTP email |
| `Login` (GET) | `GET /Customer/Login` | Shows login form |
| `Login` (POST) | `POST /Customer/Login` | Validates credentials, creates auth cookie |
| `Logout` | `GET /Customer/Logout` | Signs out, deletes cookie |
| `Profile` | `GET /Customer/Profile` | Shows profile and saved addresses |
| `UpdateProfile` | `POST /Customer/UpdateProfile` | Name, phone, default address changes |
| `ChangePassword` | `POST /Customer/ChangePassword` | BCrypt re-hashing on success |

---

### `ProductController.cs`

**What it does:** Serves the product catalog — the listing page with filters and the detail page for a single product.

| Action | Route | Notes |
|--------|-------|-------|
| `List` | `GET /Product/List` | Paginated list with category/brand/price filters and search |
| `Detail` | `GET /Product/Detail/{id}` | Single product with variants, images, and reviews |
| `GetVariantPrice` | `POST /Product/GetVariantPrice` | JSON endpoint — returns price when the user picks a variant (size, color) |

---

### `CartController.cs`

**What it does:** Manages the shopping cart. Works for both logged-in users (cart is stored in the database) and guests (cart is stored in the session).

| Action | Route | Notes |
|--------|-------|-------|
| `Index` | `GET /Cart` | Shows cart contents |
| `AddToCart` | `POST /Cart/AddToCart` | Adds a variant; returns JSON `{success, itemCount}` |
| `UpdateQuantity` | `POST /Cart/UpdateQuantity` | Changes quantity; returns updated line totals as JSON |
| `RemoveItem` | `POST /Cart/RemoveItem` | Deletes one line item |

---

### `CheckoutController.cs`

**What it does:** Orchestrates the checkout flow — address selection, shipping method choice, voucher application, and order creation.

| Action | Route | Notes |
|--------|-------|-------|
| `Index` | `GET /Checkout` | Shows checkout form; requires login |
| `Process` | `POST /Checkout/Process` | Validates address + stock, creates `Order` and `OrderItem` rows, clears cart |

---

### `PaymentController.cs`

**What it does:** Handles payment method selection and proof upload after an order is created.

| Action | Route | Notes |
|--------|-------|-------|
| `Index` | `GET /Payment/{orderId}` | Shows payment options (GCash, bank transfer, store pickup) |
| `ProcessGCash` | `POST /Payment/ProcessGCash` | Uploads GCash screenshot to GCS, creates `GCashPayment` row |
| `ProcessBankTransfer` | `POST /Payment/ProcessBankTransfer` | Same but for bank transfer receipts |

---

### `OrderController.cs`

**What it does:** Lets customers view their order history and track individual orders.

| Action | Route | Notes |
|--------|-------|-------|
| `Index` | `GET /Orders` | Paginated order list filtered by status |
| `Detail` | `GET /Order/{id}` | Full order detail — items, payment status, delivery tracking |
| `Cancel` | `POST /Order/Cancel` | Cancels an order; only allowed when status is `Pending` |

---

### `ReviewController.cs`

**What it does:** Allows customers to leave star ratings and written reviews on products they have actually purchased.

| Action | Route | Notes |
|--------|-------|-------|
| `Add` | `POST /Review/Add` | Service checks purchase history before accepting review |
| `Update` | `POST /Review/Update` | Only the author can edit their own review |

---

### `VoucherController.cs`

**What it does:** Lets customers browse available vouchers, apply a voucher code during checkout, and redeem vouchers assigned to their account.

| Action | Route | Notes |
|--------|-------|-------|
| `Index` | `GET /Vouchers` | Lists all active public vouchers |
| `Apply` | `POST /Voucher/Apply` | Validates code + eligibility; returns discount amount as JSON |
| `Redeem` | `POST /Voucher/Redeem` | Marks a user-specific voucher as redeemed |

---

### `WishlistController.cs`

**What it does:** Lets logged-in customers save products they are interested in for later.

| Action | Route | Notes |
|--------|-------|-------|
| `Toggle` | `POST /Wishlist/Toggle` | Adds the product if not in wishlist, removes it if already there; returns JSON |
| `Index` | `GET /Wishlist` | Shows the full wishlist |

---

### `SupportController.cs`

**What it does:** Provides a help desk portal where customers can open tickets, attach files, and exchange messages with staff.

| Action | Route | Notes |
|--------|-------|-------|
| `Create` | `GET /Support/Create` | Shows new ticket form |
| `Submit` | `POST /Support/Submit` | Creates ticket; uploads any attachments to GCS |
| `Detail` | `GET /Support/{id}` | Thread view of all replies |
| `Reply` | `POST /Support/Reply` | Adds a reply; staff and customer both use this same action |

---

### `SupplierController.cs`

**What it does:** A separate portal for suppliers to view purchase orders that the store has raised against them.

| Action | Route | Notes |
|--------|-------|-------|
| `Invoices` | `GET /Supplier/Invoices` | Purchase order list (supplier-facing, separate auth check) |

---

## 5. WebApplication — DataAccess

**Location:** `WebApplication/DataAccess/`

This folder contains the Entity Framework Core database context and the repository classes that wrap it.

---

### `AppDbContext.cs` (partial across 9 files)

**What it does:** Declares all 38 EF Core `DbSet<T>` properties — one per database table — and configures relationships, constraints, and column mappings via the Fluent API in `OnModelCreating`.

**Why it exists:** EF Core needs a single class that inherits `DbContext` to know which tables exist and how they map to C# classes. The context is the bridge between your C# objects and the SQL database.

**Why it is split into partial classes:** The context has 38 entities. Putting all `OnModelCreating` configuration in one file would make it 2 000+ lines long. Splitting it by domain group (Auth, Catalog, Orders, etc.) keeps each file focused and easier to navigate.

| Partial file | Entities it configures |
|--------------|----------------------|
| `AppDbContext.cs` | Base class — all 38 `DbSet` declarations |
| `AppDbContext.Auth.cs` | User, Role, UserRole, OTPVerification, GuestSession, ActiveSession |
| `AppDbContext.Catalog.cs` | Category, Brand, Product, ProductVariant, ProductImage, PriceHistory |
| `AppDbContext.Commerce.cs` | Cart, CartItem, Wishlist |
| `AppDbContext.Orders.cs` | Order, OrderItem, PickupOrder |
| `AppDbContext.Delivery.cs` | Delivery, LalamoveDelivery, LBCDelivery |
| `AppDbContext.Payments.cs` | Payment, GCashPayment, BankTransferPayment, StorePaymentAccount |
| `AppDbContext.SupplyChain.cs` | Supplier, PurchaseOrder, PurchaseOrderItem |
| `AppDbContext.SupportComms.cs` | SupportTicket, SupportTicketReply, SupportTask, Notification |

**Special configurations set here:**
- **Shared-PK 1:1 subtypes** — `GCashPayment.PaymentId` is both its PK and a FK to `Payment.PaymentId`. This enforces that every `GCashPayment` must have a parent `Payment`. The same pattern applies to `BankTransferPayment`, `LalamoveDelivery`, `LBCDelivery`, and `PickupOrder`.
- **Circular FK break** — `User.DefaultAddressId` references `Address`, but `Address.UserId` references `User`. EF Core cannot delete a User if that delete would cascade through `Address` back to the same User. The circular reference is broken by marking `DefaultAddressId` as optional (`NoAction` on delete).
- **Split queries** — Loading an `Order` together with its `OrderItems` and `Delivery` in one query causes a Cartesian product (every OrderItem row is duplicated for every Delivery row). `UseQuerySplittingBehavior(SplitQuery)` issues separate SELECT statements instead.

---

### `Repositories/` (13 files)

**What they do:** Wrap all database queries in named methods with clear intent. A repository is the only place in the codebase that is allowed to write SQL (through EF Core LINQ or Dapper).

**Why they exist:** Without repositories every controller or service would write its own LINQ queries. If a table column is renamed you would need to find every place the old name was used across the entire codebase. With repositories you change the query in exactly one place.

**Structure:**

```
Repositories/
  IRepository.cs          ← generic interface
  Repository.cs           ← generic base class
  BrandRepository.cs
  CartRepository.cs
  OrderRepository.cs
  PaymentRepository.cs
  ProductRepository.cs
  ReviewRepository.cs
  StorePaymentAccountRepository.cs
  SupplierRepository.cs
  SupportRepository.cs
  UserRepository.cs
  VoucherRepository.cs
  WishlistRepository.cs
```

**`IRepository<T>` — generic interface:**

| Method | What it returns |
|--------|----------------|
| `GetByIdAsync(int id)` | Single entity or null |
| `GetAllAsync()` | All rows as a list |
| `InsertAsync(T entity)` | Auto-generated ID of the new row |
| `UpdateAsync(T entity)` | Void — applies changes |
| `DeleteAsync(int id)` | Void — removes the row |

**`Repository<T>` — generic base class:** Provides default implementations of the five methods above using EF Core. Specialized repositories extend this class and add domain-specific methods on top.

**Specialized repositories and their extra methods:**

| Repository | Extra methods beyond CRUD |
|------------|--------------------------|
| `ProductRepository` | `SearchAsync(filters)`, `GetVariantsAsync(productId)`, `GetFeaturedAsync()`, `GetByCategoryAsync()` |
| `CartRepository` | `GetWithItemsAsync(userId)`, `GetGuestCartAsync(token)`, `ClearAsync(cartId)` |
| `OrderRepository` | `GetUserOrdersAsync(userId)`, `GetWithDetailAsync(orderId)`, `GetPendingAsync()` |
| `PaymentRepository` | `GetByOrderAsync(orderId)`, `GetUnverifiedAsync()`, `GetAccountsAsync()` |
| `UserRepository` | `GetByEmailAsync(email)`, `GetWithAddressesAsync(userId)`, `GetActiveSessionsAsync(userId)` |
| `VoucherRepository` | `GetActiveAsync()`, `GetByCodeAsync(code)`, `GetUserVouchersAsync(userId)`, `GetUsageHistoryAsync(voucherId)` |
| `SupportRepository` | `GetTicketWithRepliesAsync(ticketId)`, `GetUserTicketsAsync(userId)`, `GetOpenTicketsAsync()` |
| `ReviewRepository` | `GetProductReviewsAsync(productId)`, `HasPurchasedAsync(userId, productId)` |
| `WishlistRepository` | `GetProductIdsAsync(userId)`, `ToggleAsync(userId, productId)` |

**Important:** No repository exposes `IQueryable<T>`. All methods return materialized lists or single objects. This prevents callers from accidentally adding `.Where()` or `.Include()` outside the repository, which would bypass the intended query structure.

---

## 6. WebApplication — Models

**Location:** `WebApplication/Models/`

The Models folder is divided into three sub-folders, each with a different purpose.

---

### `Models/Entities/` (39 files)

**What they do:** Plain C# classes that mirror the database tables one-to-one. EF Core uses these to generate SQL queries and map result rows back to objects.

**Why they exist:** Keeping entity classes separate from view models prevents a common mistake — accidentally exposing internal data (like `PasswordHash`) in a public-facing view because the same class is used for both.

**Key entities to understand:**

| Entity | Table | Notable properties |
|--------|-------|--------------------|
| `User` | `User` | `PasswordHash`, `IsWalkIn`, `FailedLoginAttempts`, `LockoutUntil`, `DefaultAddressId` |
| `Product` | `Product` | `IsActive` (soft-delete), `CategoryId`, `BrandId` |
| `ProductVariant` | `ProductVariant` | `Price`, `StockQuantity`, `SKU`, `Size`, `Color` |
| `Order` | `Order` | `Status` (enum-as-string), `TotalAmount`, `UserId`, `VoucherId` |
| `OrderItem` | `OrderItem` | `Quantity`, `UnitPrice` (snapshot at purchase time) |
| `Payment` | `Payment` | `Method` (`GCash`, `BankTransfer`, `Store`), `Status` (`Pending`, `Verified`, `Failed`) |
| `GCashPayment` | `GCashPayment` | Inherits PK from `Payment`; adds `ProofImageUrl`, `ReferenceNumber` |
| `Delivery` | `Delivery` | `Method` (`Lalamove`, `LBC`, `Pickup`), `Status`, `TrackingNumber` |
| `Voucher` | `Voucher` | `DiscountType` (`Percentage`, `Fixed`), `MinOrderAmount`, `UsageLimit`, `ExpiresAt` |
| `SupportTicket` | `SupportTicket` | `Status` (`Open`, `InProgress`, `Closed`), `Priority`, `Category` |
| `InventoryLog` | `InventoryLog` | `ChangeType` (`Purchase`, `Sale`, `Adjustment`), `QuantityChange`, `Reason` |
| `Notification` | `Notification` | `Type` (`Email`, `InApp`), `IsRead`, `SentAt` |

**Conventions:**
- No data-annotation relationship attributes (`[ForeignKey]`, `[InverseProperty]`). All relationships are configured in `AppDbContext` using the Fluent API.
- Validation attributes (`[Required]`, `[MaxLength(n)]`) are present where the business rule must be enforced at the C# layer as well as the database layer.

---

### `Models/ViewModels/` (12 files)

**What they do:** Shaped data objects built specifically for what a Razor view needs to render. They contain only the fields the view uses — nothing more.

**Why they exist:** If a controller passed an `Order` entity directly to a view, the view template would have access to every column in the `Order` table including internal fields. ViewModels are a security and simplicity boundary — the view sees exactly what it needs.

| ViewModel | Used by | Contains |
|-----------|---------|----------|
| `ProductViewModel` | Product list page | Id, name, price, main image, brand, category, rating average |
| `ProductDetailViewModel` | Product detail page | Full product info + list of variants + list of reviews |
| `CartViewModel` | Cart page | List of `CartItemViewModel`, subtotal, item count |
| `CheckoutViewModel` | Checkout page | Saved addresses, selected shipping method, voucher input field |
| `OrderViewModel` | Order detail page | Order header + items + payment status + delivery status |
| `LoginViewModel` | Login page | Email, password fields + validation messages |
| `RegisterViewModel` | Registration page | Email, password, confirm password, name, phone |
| `DashboardViewModel` | Customer homepage | Recent orders, wishlist count, unread notifications |
| `ProfileViewModel` | Profile page | Name, phone, email, saved addresses |
| `SupportViewModel` | Support ticket form | Category, subject, message, file attachment |
| `VoucherViewModels` | Voucher page | Code, discount description, expiry, eligibility |
| `NotificationViewModel` | Notification dropdown | Message, type, timestamp, is-read flag |

---

### `Models/` — other model files

| File | Purpose |
|------|---------|
| `ApiResponse.cs` | Standard JSON wrapper `{ success, message, data }` returned from AJAX endpoints |
| `CloudinarySettings.cs` | Typed binding class for the `Cloudinary` section of `appsettings.json` |
| `SmtpSettings.cs` | Typed binding class for the `GmailSmtp` section of `appsettings.json` |
| `ErrorViewModel.cs` | Carries the request ID to the error page — auto-generated by the project template |

---

## 7. WebApplication — BusinessLogic

**Location:** `WebApplication/BusinessLogic/`

```
BusinessLogic/
  Interfaces/      ← one interface file per service
  Services/        ← one implementation file per service
```

**What it does:** Contains all business logic — the rules that decide whether an operation is allowed, in what order steps happen, and what the system does when something goes wrong.

**Why it exists:** If business logic lived in controllers, you could not reuse it. For example, the rule "a review can only be submitted by someone who bought the product" would need to be copied into every place that accepts reviews. Putting it in `ReviewService.HasPurchasedAsync` means it is tested and maintained in one place.

**Why the Interfaces/ folder exists:** Interfaces define the contract without the implementation. Controllers and background jobs depend on the interface (e.g., `IOrderService`), not the concrete class (`OrderService`). This makes it possible to swap implementations (e.g., replace the real email sender with a fake one during testing) without touching the code that uses it.

---

### Service interface summary

All methods are `async`, return `Task<T>` or `Task`, and accept a `CancellationToken` so they can be cancelled if the HTTP request is aborted before the operation completes.

| Interface | Key methods | Business rules it enforces |
|-----------|-------------|---------------------------|
| `IUserService` | `RegisterAsync`, `LoginAsync`, `LogoutAsync`, `UpdateProfileAsync`, `ChangePasswordAsync` | Duplicate email check; lockout after N failed logins; BCrypt verification |
| `IProductService` | `GetFilteredAsync`, `GetByIdAsync`, `GetFeaturedAsync`, `GetVariantPriceAsync`, `GetCategoriesAsync`, `GetBrandsAsync` | Only active products are returned; out-of-stock variants are flagged |
| `ICartService` | `GetCartAsync`, `AddItemAsync`, `UpdateQuantityAsync`, `RemoveItemAsync`, `ClearAsync`, `CalculateTotalAsync` | Stock check on add; maximum quantity per line item; guest-to-user cart merge |
| `IOrderService` | `CreateAsync`, `GetByIdAsync`, `GetUserOrdersAsync`, `CancelAsync`, `UpdateStatusAsync` | Stock reservation on create; cancellation only from `Pending` status |
| `IPaymentService` | `ProcessGCashAsync`, `ProcessBankTransferAsync`, `ValidateProofAsync`, `MarkPaidAsync`, `HandleTimeoutAsync` | Proof upload before marking paid; timeout auto-cancels after 12 hours |
| `IReviewService` | `SubmitAsync`, `GetProductReviewsAsync`, `UpdateAsync`, `DeleteAsync`, `HasPurchasedAsync` | Must have purchased the product before reviewing; one review per product per user |
| `IVoucherService` | `GetAllAsync`, `ValidateAsync`, `RedeemAsync`, `GetUserVouchersAsync`, `CreateAsync` | Minimum order amount check; usage limit enforcement; expiry validation |
| `IWishlistService` | `AddAsync`, `RemoveAsync`, `GetByUserIdAsync`, `IsInWishlistAsync` | Requires login; duplicate prevention |
| `IBrandService` | `GetAllAsync`, `GetByIdAsync`, `SearchAsync` | Read-only brand lookups |
| `INotificationService` | `SendAsync`, `GetUnreadAsync`, `MarkAsReadAsync`, `GetHistoryAsync` | Creates `Notification` row; enqueues email if type is `Email` |
| `IEmailSender` | `SendEmailAsync(to, subject, body)` | SMTP-level interface — used by `NotificationService` and OTP flow |
| `IPhotoService` | `UploadAsync`, `DeleteAsync`, `GetUrlAsync` | Wraps Cloudinary SDK; returns CDN URL for use in views |
| `ISupportService` | `CreateTicketAsync`, `ReplyAsync`, `CloseAsync`, `AttachFileAsync`, `GetTicketAsync`, `GetUserTicketsAsync` | File size validation before upload; status transitions enforced |

---

## 8. WebApplication — BackgroundJobs

**Location:** `WebApplication/BackgroundJobs/`

**What they do:** Long-running tasks that run in the background while the web server handles HTTP requests. They do not respond to user actions — they poll the database on a timer and take automated actions.

**Why they exist:** Some operations cannot wait for a user to trigger them. If a customer abandons an order for 12 hours without paying, the system needs to release the reserved stock automatically. Background jobs handle these time-based rules.

**How they work:** Each job extends `BackgroundService`, which gives it a `ExecuteAsync` loop. Inside the loop, the job calls `IServiceProvider.CreateScope()` to get a fresh `AppDbContext` and service instances, does its work, then sleeps until the next interval. Errors are caught and logged but do not crash the job.

| Job class | Poll interval | What it does |
|-----------|---------------|-------------|
| `InventorySyncJob` | 30 seconds | Pulls stock levels from an external API and updates `ProductVariant.StockQuantity` |
| `PendingOrderMonitorJob` | 15 seconds | Finds orders that have been `Pending` for more than 30 minutes and marks them `AutoCancelled` |
| `PaymentTimeoutJob` | 60 seconds | Finds orders with unverified payment proof older than 12 hours and transitions them to `Failed` |
| `StockMonitorJob` | 10 seconds | Finds variants below their low-stock threshold and creates an `InApp` notification for staff |
| `DeliveryStatusPollJob` | 60 seconds | Calls the Lalamove and LBC APIs to pull the latest delivery status and updates the `Delivery` table |
| `NotificationDispatchJob` | 5 seconds | Dequeues `Notification` rows with `Status = Pending` and sends them via `IEmailSender` |

---

## 9. WebApplication — Utilities

**Location:** `WebApplication/Utilities/`

Small, stateless helper classes that do not fit into services or repositories.

| File | What it does |
|------|-------------|
| `PasswordHelper.cs` | `Hash(password)` — BCrypt with work factor 12. `Verify(password, hash)` — BCrypt comparison. Used by `UserService` |
| `FileUploadHelper.cs` | Uploads a file stream to Google Cloud Storage. Falls back to writing to `wwwroot/uploads/` if GCS credentials are not present (local development mode) |
| `ValidationHelper.cs` | Regex-based checks for email format, Philippine phone numbers (`09xxxxxxxxx`), and address field lengths |

---

## 10. WebApplication — Views and wwwroot

### `Views/`

**What they do:** Razor templates that produce the HTML sent to the browser. Each `.cshtml` file is a mix of HTML and C# expressions. The C# expressions are evaluated on the server and the result is a plain HTML string sent to the client.

**Why they exist:** Keeping HTML in separate files rather than in controller code makes it easy for someone who knows HTML/CSS to work on the front-end without touching C#.

```
Views/
  Shared/
    _Layout.cshtml          ← master page — header, nav, footer, script imports
    _Header.cshtml          ← navigation bar partial
    _Footer.cshtml          ← footer partial
    Error.cshtml            ← error page
  Home/
    Index.cshtml            ← homepage (hero, featured products, categories)
  Customer/
    Cart.cshtml
    Checkout.cshtml
    Orders.cshtml
    OrderDetail.cshtml
    Profile.cshtml
    WishlistList.cshtml
    SupportCreate.cshtml
    SupportDetail.cshtml
    Notifications.cshtml
  Product/
    List.cshtml
    Detail.cshtml
  Payment/
    Index.cshtml
  Voucher/
    Index.cshtml
```

### `wwwroot/`

**What it does:** Static files served directly by the web server — no C# processing occurs.

```
wwwroot/
  css/
    site.css          ← custom styles on top of Bootstrap
  js/
    site.js           ← AJAX calls, anti-forgery token injection, client-side validation
  lib/
    bootstrap/        ← Bootstrap 5 (grid, components)
    jquery/           ← jQuery (DOM, AJAX)
    jquery-validation/ ← client-side form validation
```

---

## 11. AdminSystem_v2 — entry point and configuration

### `App.xaml.cs`

**What it does:** The desktop equivalent of `Program.cs`. It manually creates every repository, service, and ViewModel the app needs, then shows the login window. After a successful login it opens the main window.

**Why it exists:** WPF does not have a built-in dependency injection container. This file acts as the composition root — the one place where all concrete classes are instantiated and wired together. This means if you want to change which implementation of `IProductService` the app uses, you change it here.

**Startup sequence:**
1. `OnStartup` fires when the app launches
2. `DatabaseHelper` is initialised with the connection string from `appsettings.json`
3. All 10 repositories are instantiated with `new`
4. All 10 services are instantiated, receiving their repositories via constructor
5. All 11 ViewModels are instantiated, receiving their services via constructor
6. `LoginView` is shown
7. On successful login, `MainWindow` is shown and `LoginView` is closed

### `appsettings.json`

Minimal config — only the connection string is here. Credentials are provided via .NET User Secrets during development and environment variables in production.

### `AdminSystem_v2.csproj`

| Package | Purpose |
|---------|---------|
| `Dapper` | Micro-ORM — maps SQL result rows to C# objects without EF Core overhead |
| `Microsoft.Data.SqlClient` | SQL Server driver used by Dapper |
| `OxyPlot.Wpf` | Charting library for sales line charts and bar charts |
| `ClosedXML` | Excel (`.xlsx`) file generation for sales and inventory exports |
| `BCrypt.Net-Next` | Password hashing — same library as the web app so hashes are cross-compatible |

---

## 12. AdminSystem_v2 — Models

**Location:** `AdminSystem_v2/Models/`

**What they do:** Data classes used by the desktop app. These are not EF Core entities — they have no `[Key]` or `[Column]` attributes. Dapper maps SQL query result rows into these classes by matching column names to property names.

**Why they exist separately from WebApplication entities:** The desktop app uses Dapper, not EF Core. Dapper does not need the navigation properties or relationship configurations that EF Core entities have. Using lighter classes keeps the desktop app simpler.

**Groups:**

**Database-mapped models** — mirror the database structure:

| Class | Maps to |
|-------|---------|
| `User` | `User` table |
| `Product` | `Product` table |
| `ProductVariant` | `ProductVariant` table |
| `ProductImage` | `ProductImage` table |
| `Order` | `Order` table |
| `OrderItem` | `OrderItem` table |
| `PickupOrder` | `PickupOrder` table |
| `Category` | `Category` table |
| `Brand` | `Brand` table |
| `Supplier` | `Supplier` table |
| `Voucher` | `Voucher` table |
| `StorePaymentAccount` | `StorePaymentAccount` table |
| `SupportTicket` | `SupportTicket` table |

**Report and display models** — assembled from JOINs or calculations:

| Class | Where it comes from | Used in |
|-------|--------------------|----|
| `DailySales` | `ReportRepository` daily GROUP BY query | Reports chart |
| `SalesSummary` | Aggregated totals (revenue, orders, avg order value) | Dashboard KPI cards |
| `TopProduct` | JOIN across `OrderItem` and `Product` | Reports top-products table |
| `InventoryReportItem` | JOIN across `ProductVariant` and `Product` | Inventory report export |
| `LowStockVariant` | `ProductVariant` rows below threshold | Dashboard alert section |
| `POSCustomer` | `User` rows used in POS lookup | POS view |
| `POSOrderResult` | Returned after a POS transaction completes | POS receipt |
| `POSVoucherResult` | Returned after applying a voucher at POS | POS view |
| `VoucherListItem` | Voucher with usage count | Voucher management page |
| `VoucherUserRow` | User assigned to a voucher | Voucher management page |
| `DeliveryDetail` | `Delivery` joined to its subtype | Order detail view |

**UI-helper models:**

| Class | Purpose |
|-------|---------|
| `OrderBarItem` | One item in the horizontal order-status progress bar |
| `StatusBadge` | A status string paired with a display colour |
| `SelectableOrder` | An `Order` with an `IsSelected` checkbox property for batch operations |

**Exception:**

| Class | Purpose |
|-------|---------|
| `InvalidStatusTransitionException` | Thrown by `OrderService` when code tries to move an order to an illegal status (e.g., `Delivered` → `Pending`) |

---

## 13. AdminSystem_v2 — Repositories

**Location:** `AdminSystem_v2/Repositories/`

**What they do:** Execute raw SQL via Dapper and return strongly-typed C# objects. Every database query in the desktop app lives here.

**Why Dapper instead of EF Core:** The admin app runs complex analytical queries (sales reports, inventory pivots) that are easier to write and tune as raw SQL than as LINQ. Dapper executes SQL directly with minimal overhead.

**Structure:**

```
Repositories/
  IRepository.cs            ← generic interface
  Repository.cs             ← generic base class
  ProductRepository.cs
  OrderRepository.cs
  InventoryRepository.cs
  ReportRepository.cs
  POSRepository.cs
  VoucherRepository.cs
  UserRepository.cs
  StorePaymentAccountRepository.cs
  SupportTicketRepository.cs
```

**`Repository<T>` base class — protected helper methods:**

| Method | What it does |
|--------|-------------|
| `QueryAsync<T>(sql, param)` | Runs a SELECT and returns `IEnumerable<T>` |
| `QueryFirstOrDefaultAsync<T>(sql, param)` | Runs a SELECT and returns the first row or null |
| `ExecuteAsync(sql, param)` | Runs an INSERT, UPDATE, or DELETE; returns rows affected |
| `ExecuteScalarAsync<T>(sql, param)` | Runs a SELECT that returns a single value (e.g., `COUNT(*)`), casts to T |

All helpers call `DatabaseHelper.GetConnection()` to get a pooled `SqlConnection`.

**Specialized repository responsibilities:**

| Repository | Responsibility |
|------------|---------------|
| `ProductRepository` | Full product CRUD + variant management + image gallery + stock level queries |
| `OrderRepository` | Order list with JOINs to `Delivery` and `Payment`; status update; batch status queries |
| `InventoryRepository` | Current stock levels; low-stock report; movement history from `InventoryLog` |
| `ReportRepository` | Daily and monthly sales GROUP BY queries; top products by revenue; these typically hit database views |
| `POSRepository` | POS session lifecycle (open, add item, close); transaction history; guest customer lookup |
| `VoucherRepository` | Voucher CRUD; user assignment; usage history; active voucher list |
| `UserRepository` | Staff account CRUD; role assignment queries; lockout management |
| `StorePaymentAccountRepository` | GCash and bank account configuration — the accounts customers are instructed to pay into |
| `SupportTicketRepository` | Ticket list (open/closed); full thread with replies; reply insertion |

---

## 14. AdminSystem_v2 — Services

**Location:** `AdminSystem_v2/Services/`

**What they do:** Apply business rules and coordinate between repositories. A ViewModel never calls a repository directly — it always goes through a service.

**Why they exist:** Same reason as in the web app — centralised business logic is tested once and maintained in one place.

| Service | Key methods | What it enforces |
|---------|-------------|-----------------|
| `AuthService` | `LoginAsync`, `GetCurrentUserAsync` | BCrypt credential check; lockout after failed attempts; returns typed `User` object on success |
| `ProductService` | `GetAllAsync`, `SearchAsync`, `CreateAsync`, `UpdateAsync`, `DeactivateAsync`, `AdjustStockAsync`, `AddVariantAsync` | Soft-delete for deactivation; stock cannot go below zero; SKU uniqueness |
| `InventoryService` | `GetLowStockAsync`, `GetInventoryReportAsync`, `GetMovementHistoryAsync`, `AdjustStockAsync` | Writes an `InventoryLog` row on every stock adjustment so changes are auditable |
| `OrderService` | `GetAllAsync`, `GetByIdAsync`, `UpdateStatusAsync`, `ProcessRefundAsync`, `PrintReceiptAsync` | Status transition validation (throws `InvalidStatusTransitionException` on illegal moves); refund requires `Delivered` or verified payment |
| `ReportService` | `GetDailySalesAsync`, `GetMonthlySalesAsync`, `GetTopProductsAsync`, `ExportSalesAsync` | Date range validation; calls `ExcelExportService` when exporting |
| `POSService` | `StartSessionAsync`, `AddItemAsync`, `ApplyVoucherAsync`, `ProcessPaymentAsync`, `CloseSessionAsync`, `PrintReceiptAsync` | Stock check on add; payment tendered must cover total; auto-creates walk-in `User` row if customer is anonymous |
| `VoucherService` | `GetAllAsync`, `CreateAsync`, `UpdateAsync`, `DeactivateAsync`, `AssignToUserAsync`, `ValidateAsync` | Expiry and usage limit checks; prevents assigning an already-assigned voucher |
| `UserService` | `GetAllAsync`, `CreateAsync`, `UpdateAsync`, `AssignRoleAsync`, `DeactivateAsync` | Role assignment validation; admin cannot deactivate themselves |
| `DialogService` | `ShowAsync`, `ConfirmAsync`, `AlertAsync`, `FilePickerAsync` | Wraps WPF `MessageBox` and `OpenFileDialog` calls so ViewModels do not depend on WPF UI classes directly |
| `ExcelExportService` | `ExportSalesAsync`, `ExportInventoryAsync`, `ExportOrdersAsync` | Uses ClosedXML to write `.xlsx` files; opens a Save dialog for the user to choose the destination |
| `ReceiptPrintService` | `PrintAsync`, `PreviewAsync` | Formats receipt data and sends it to the default printer or shows a WPF print preview window |

---

## 15. AdminSystem_v2 — ViewModels

**Location:** `AdminSystem_v2/ViewModels/`

**What they do:** Hold the data a view displays and the commands a view triggers. A ViewModel is the link between the view (what the user sees) and the service layer (what the app does). The view never touches a service directly.

**Why they exist:** This is the MVVM pattern. Keeping logic in ViewModels rather than in `.xaml.cs` code-behind files makes the logic testable — you can instantiate a ViewModel in a unit test without launching any WPF windows.

**`BaseViewModel` — base class for all ViewModels:**

| Member | Purpose |
|--------|---------|
| `INotifyPropertyChanged` | Tells WPF bindings to refresh when a property value changes |
| `SetProperty<T>(ref T field, T value)` | Sets the backing field and fires `PropertyChanged` if the value changed |
| `IsLoading` | Boolean — bound to a loading spinner in the view |
| `ErrorMessage` | String — displayed in an error banner at the top of the view |
| `SuccessMessage` | String — displayed in a success banner |

**Concrete ViewModels:**

| ViewModel | Services it uses | What the view binds to |
|-----------|-----------------|------------------------|
| `LoginViewModel` | `AuthService`, `DialogService` | `Email`, `Password`, `LoginCommand`, `IsLoading`, `ErrorMessage` |
| `DashboardViewModel` | `ProductService`, `InventoryService`, `OrderService` | `TodaySalesAmount`, `PendingOrderCount`, `LowStockCount`, `RecentOrders` |
| `ProductViewModel` | `ProductService` | `Products` (filterable list), `SelectedProduct`, `Variants`, `Images`, `SaveProductCommand`, `DeactivateCommand`, `AdjustStockCommand` |
| `OrderViewModel` | `OrderService`, `DialogService` | `Orders` (filterable/sortable list), `SelectedOrder`, `StatusOptions`, `UpdateStatusCommand`, `RefundCommand` |
| `POSViewModel` | `POSService`, `ReceiptPrintService` | `CartItems`, `CartTotal`, `VoucherCode`, `Tendered`, `Change`, `AddItemCommand`, `ProcessPaymentCommand`, `PrintReceiptCommand` |
| `ReportViewModel` | `ReportService`, `ExcelExportService` | `SalesChart` (OxyPlot model), `DailySalesRows`, `TopProducts`, `StartDate`, `EndDate`, `ExportCommand` |
| `StaffViewModel` | `UserService`, `DialogService` | `StaffList`, `SelectedStaff`, `Roles`, `CreateStaffCommand`, `AssignRoleCommand`, `DeactivateCommand` |
| `VoucherViewModel` | `VoucherService`, `DialogService` | `Vouchers`, `SelectedVoucher`, `AssignedUsers`, `CreateVoucherCommand`, `AssignCommand`, `DeactivateCommand` |
| `StorePaymentAccountViewModel` | `StorePaymentAccountService`, `DialogService` | `GCashAccount`, `BankAccount`, `QRCodeImage`, `SaveCommand` |
| `SupportTicketsViewModel` | `SupportTicketService`, `DialogService` | `OpenTickets`, `ClosedTickets`, `SelectedTicket`, `Replies`, `ReplyText`, `ReplyCommand`, `CloseTicketCommand` |
| `MainWindowViewModel` | `AuthService` + all above | `CurrentPage`, `CurrentUser`, `NavigateCommand`, `SignOutCommand` |

---

## 16. AdminSystem_v2 — Views

**Location:** `AdminSystem_v2/Views/`

**What they do:** XAML files that define the visual layout of each screen. They bind to ViewModel properties using WPF data binding — the XAML never accesses a service or repository directly.

**Why XAML instead of code:** XAML lets you describe the UI structure declaratively (what elements exist and how they relate) while keeping event handlers and business logic out of the UI file. The `.xaml.cs` code-behind files are kept intentionally thin — usually just `InitializeComponent()` and `DataContext` assignment.

| View file | ViewModel | Screen purpose |
|-----------|-----------|---------------|
| `LoginView.xaml` | `LoginViewModel` | Email / password form; shows error message on bad credentials |
| `MainWindow.xaml` | `MainWindowViewModel` | App shell — sidebar navigation, top bar with current user name and sign-out, content region |
| `DashboardView.xaml` | `DashboardViewModel` | Four KPI cards (sales, orders, low stock, open tickets) plus recent activity |
| `ProductsView.xaml` | `ProductViewModel` | Two-panel layout: product DataGrid on the left, variant/image editor on the right |
| `OrdersView.xaml` | `OrderViewModel` | Order DataGrid with colour-coded status badges, detail pane at the bottom, refund dialog |
| `POSView.xaml` | `POSViewModel` | Cart table, product search, payment tendering section, "Print Receipt" button |
| `ReportsView.xaml` | `ReportViewModel` | Date-range picker, line chart (OxyPlot), daily pivot table, export button |
| `StaffView.xaml` | `StaffViewModel` | Staff DataGrid, role assignment dropdown, create/deactivate buttons |
| `VoucherView.xaml` | `VoucherViewModel` | Voucher list, create form, user assignment panel, usage count |
| `StorePaymentAccountView.xaml` | `StorePaymentAccountViewModel` | GCash and bank fields, QR code preview image |
| `SupportTicketsView.xaml` | `SupportTicketsViewModel` | Open/closed tab, message thread list, reply text box |

---

## 17. AdminSystem_v2 — Converters

**Location:** `AdminSystem_v2/Converters/`

**What they do:** WPF value converters sit between a ViewModel property and a XAML binding. They transform a value from one type to another without changing the ViewModel.

**Why they exist:** A ViewModel stores `bool IsLoading`. A progress spinner needs `Visibility.Visible` or `Visibility.Collapsed`. A converter bridges the gap without adding UI logic to the ViewModel.

| Converter | Input → Output | Example usage |
|-----------|---------------|---------------|
| `BoolToVisibilityConverter` | `bool` → `Visibility` | Show a spinner when `IsLoading = true` |
| `InverseBoolConverter` | `bool` → `bool` (negated) | Disable a button while `IsLoading = true` |
| `EqualityConverter` | two `object` values → `bool` | Highlight the currently selected sidebar item |
| `CurrencyConverter` | `decimal` → `string` | Format `1234.50` as `₱1,234.50` |
| `StatusToColorConverter` | `string` (order status) → `Brush` | Colour "Pending" yellow, "Delivered" green, "Cancelled" red |
| `RatioToHeightConverter` | `double` (ratio) → `double` (pixels) | Scale chart bar heights proportionally |

---

## 18. AdminSystem_v2 — Helpers

**Location:** `AdminSystem_v2/Helpers/`

| File | What it does |
|------|-------------|
| `DatabaseHelper.cs` | Static factory — returns a pooled `SqlConnection`. Used by every repository. Pooling is handled transparently by `Microsoft.Data.SqlClient` |
| `PageNames.cs` | String constants for every page name. `MainWindowViewModel` uses these when navigating between views so that magic strings are not scattered across the codebase |
| `PasswordHelper.cs` | Same BCrypt wrap as the web app — `Hash(password)` and `Verify(password, hash)`. Identical implementation ensures staff passwords set on the web are verifiable on the desktop |
| `RelayCommand.cs` | Implements `ICommand`. XAML buttons bind to `ICommand` properties on ViewModels. `RelayCommand` wraps a lambda so you can define a command inline: `new RelayCommand(_ => SaveProduct())` |
| `RoleGuard.cs` | Helper methods that check `App.CurrentUser.Roles` and throw or return false if the current user lacks the required role. Called at the start of sensitive ViewModel operations |

---

## 19. AdminSystem_v2 — Migrations

**Location:** `AdminSystem_v2/Migrations/`

**What they do:** Incremental SQL scripts that alter the existing database schema. Each script makes one focused change and is designed to be run exactly once on an already-deployed database.

**Why they exist:** After the initial database was created using `Taurus_seed_5.sql`, new features required new columns or tables. Instead of dropping and recreating the whole database (which would destroy all data), these scripts add only what is new. Each script is numbered to indicate the order it must be run.

| File | What it changes |
|------|----------------|
| `001_AddLoginLockoutColumns.sql` | Adds `FailedLoginAttempts INT` and `LockoutUntil DATETIME2` to `User` — required for the account lockout feature |
| `002_AllowInAppVoucherNotifications.sql` | Extends `Notification.Type` allowed values to include voucher-related notification types |
| `003_AddAutoCancelledAndReminderNotifTypes.sql` | Adds `AutoCancelled` and `Reminder` notification types — required for the pending-order auto-cancel background job |
| `003_AddOrderStatusAuditTable.sql` | Creates `OrderStatusAudit` — records every status transition an order goes through (old status, new status, who changed it, when) |
| `004_RenameShippedToOutForDelivery.sql` | Renames the `Shipped` order status to `OutForDelivery` across all relevant columns and constraints |
| `005_AddActiveSessionTable.sql` | Creates `ActiveSession` — used by the session revocation / "log out from all devices" feature |

---

## 20. Shared design patterns

This section explains the architectural decisions that appear in both projects. Understanding these patterns helps you predict where code should live when you are adding a new feature.

---

### Layered architecture

Both applications are organised in layers. Each layer is only allowed to talk to the layer directly below it:

```
[ View / Controller ]    ← shows data, receives user input
        ↓
[ ViewModel / Service ]  ← business rules, validation
        ↓
[ Repository ]           ← database queries only
        ↓
[ Database ]             ← SQL Server
```

Breaking this rule — for example, a View that calls a repository directly — creates code that is hard to change. A database column rename would require editing UI files.

---

### Repository pattern

A repository is a named collection of database queries for one aggregate. You would never write a product query inside the order service — you would call `IProductRepository.GetByIdAsync`.

Both projects implement the same two-level structure:
1. `IRepository<T>` — five generic CRUD operations
2. Specialised subclass — domain-specific query methods on top of CRUD

The web app repositories use EF Core LINQ; the desktop app repositories use Dapper raw SQL. The interface is identical, so a service cannot tell the difference.

---

### Dependency injection

**WebApplication:** Uses `IServiceCollection` in `Program.cs`. All repositories and services are registered with `AddScoped` (one instance per HTTP request). Controllers and services declare their dependencies in their constructors and ASP.NET Core fills them in automatically.

**AdminSystem_v2:** Uses manual instantiation in `App.xaml.cs`. There is no framework doing the wiring — you create each dependency by hand and pass it to the next layer's constructor. The effect is the same: every class receives its dependencies rather than creating them itself.

---

### Async / await

All service and repository methods are async. The `CancellationToken` parameter travels from the HTTP request (in the web app) all the way down to the database query. If the user closes their browser tab mid-request, the database query is cancelled immediately, freeing the connection.

---

### Authentication

| Concern | WebApplication | AdminSystem_v2 |
|---------|---------------|----------------|
| How users log in | Cookie-based (`CookieAuthenticationDefaults`) | `AuthService.LoginAsync` — BCrypt verify, returns `User` object |
| Current user identity | `User.FindFirstValue(ClaimTypes.NameIdentifier)` in controllers | `App.CurrentUser` static field set after login |
| Authorization | `[Authorize]` attribute on controller actions | `RoleGuard.Require(role)` called inside ViewModel methods |
| Session storage | Server-side session (30-minute idle timeout) | In-memory only — closing the app ends the session |

---

### MVVM (AdminSystem_v2 only)

MVVM stands for Model-View-ViewModel. It is the standard pattern for WPF applications.

| Layer | File types | Knows about |
|-------|-----------|-------------|
| Model | `Models/*.cs` | Only data — no UI concepts |
| ViewModel | `ViewModels/*.cs` | Model + Services. No WPF imports except `INotifyPropertyChanged` |
| View | `Views/*.xaml` | ViewModel only. Binds to ViewModel properties via `{Binding}` |

The `{Binding}` expressions in XAML connect properties in the ViewModel to controls in the view. When `IsLoading` changes in the ViewModel, WPF automatically updates every control that binds to it — no manual `label.Text = ...` code required.

---

### Soft delete

Entities are never permanently removed from the database during normal operation. Instead, an `IsActive` or `IsDeleted` column is flipped. All SELECT queries filter by `IsActive = 1`.

This preserves order history — if a product is deactivated, old orders that reference it still have accurate records. It also makes accidental deletions recoverable.

---

### 1:1 subtype tables

Payments and deliveries use an inheritance-by-table pattern:

```
Payment (PaymentId, Method, Status, OrderId)
  ├── GCashPayment       (PaymentId ← FK+PK, ReferenceNumber, ProofImageUrl)
  └── BankTransferPayment (PaymentId ← FK+PK, BankName, ProofImageUrl)

Delivery (DeliveryId, Method, Status, TrackingNumber)
  ├── LalamoveDelivery   (DeliveryId ← FK+PK, LalamoveOrderId, DriverName)
  └── LBCDelivery        (DeliveryId ← FK+PK, TrackingUrl, EstimatedDate)
```

Shared columns (status, order link) live in the parent table. Method-specific columns live in the child table. A `GCashPayment` row can only exist if a `Payment` row with the same `PaymentId` exists first. This enforces that every payment detail record must have a parent payment record.

---

## 21. Infrastructure and deployment

### WebApplication (Google Cloud)

| Component | What it does |
|-----------|-------------|
| **Cloud Run** | Containerised hosting — the app runs in a Docker container; scales to zero when idle |
| **Google Cloud SQL (SQL Server)** | Managed SQL Server — backups, patching, and high availability handled by Google |
| **Google Cloud Storage** | Blob storage for support-ticket file attachments; fallback image store when Cloudinary is unavailable |
| **Cloudinary CDN** | Product photo and payment-proof uploads — Cloudinary transforms and caches images globally |
| **Gmail SMTP** | Transactional email (OTPs, order confirmations, notification emails) via app password |
| **Environment variables** | Connection string, API keys, and SMTP credentials are never in source control — injected at container start time |

### AdminSystem_v2 (Windows desktop)

| Component | What it does |
|-----------|-------------|
| **Local installation** | `.exe` deployed on the store's Windows machine; no cloud hosting required |
| **Google Cloud SQL** | Same database as the web app — the desktop app connects directly over the internet |
| **Thermal printer** | `ReceiptPrintService` sends formatted receipt data to the Windows default printer |
| **Local file system** | Excel exports are saved to the user's machine via a Save dialog |

### Local development setup

To run either application locally you need:

1. **.NET 8.0 SDK** — verify with `dotnet --version`
2. **SQL Server Express** or access credentials for the cloud instance
3. **.NET User Secrets** — supply the connection string without putting it in source control:
   ```
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=...;Database=...;"
   ```
4. **Visual Studio 2022** or **JetBrains Rider** (AdminSystem_v2 requires Windows for WPF)
5. **Postman** — import the collection from `.postman/` to test web app endpoints

**Build commands:**

```bash
# WebApplication
cd WebApplication
dotnet build
dotnet run

# AdminSystem_v2 (Windows only)
cd AdminSystem_v2
dotnet build
dotnet run
```
