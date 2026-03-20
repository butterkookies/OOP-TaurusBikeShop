# Taurus Bike Shop — Cowork Project Context
# Place this file in the OOP-TaurusBikeShop root folder.
# Cowork reads this at the start of every session.

---

## What This Project Is

A two-application system for a bike shop business built as an OOP school project.

| App | Tech | Purpose |
|-----|------|---------|
| WebApplication | ASP.NET Core MVC (.NET 8), EF Core 8 | Customer-facing website |
| AdminSystem | WPF .NET 4.8, Dapper | Staff/admin desktop app |

Both apps share one database: **TaurusBikeShopDB** on Google Cloud SQL for SQL Server.
Server IP: `35.221.161.150,1433`

---

## Solution Structure

```
OOP-TaurusBikeShop/
├── WebApplication/
│   ├── BackgroundJobs/          5 background services
│   ├── BusinessLogic/
│   │   ├── Interfaces/          10 service interfaces + INotificationService
│   │   └── Services/            10 service implementations + NotificationService
│   ├── Controllers/             11 controllers
│   ├── DataAccess/
│   │   ├── Context/             AppDbContext.cs — 38 DbSets
│   │   └── Repositories/        IRepository, Repository, + 11 specific repos
│   ├── Models/
│   │   ├── Entities/            38 entity classes + constants
│   │   └── ViewModels/          all view models
│   ├── Utilities/               FileUploadHelper, PasswordHelper, ValidationHelper
│   ├── Views/
│   │   ├── Customer/            ALL customer views go here (no feature subfolders)
│   │   │   └── Partials/        ALL partial views go here
│   │   ├── Home/                Index.cshtml, Privacy.cshtml
│   │   └── Shared/              _Layout.cshtml, Error.cshtml, etc.
│   └── wwwroot/
│       ├── css/                 one CSS file per feature
│       └── js/                  one JS file per feature
└── AdminSystem/
    ├── Models/
    ├── Repositories/
    ├── Services/
    ├── ViewModels/
    ├── Views/                   XAML views
    ├── Helpers/
    └── Converters/
```

---

## Current Task Focus

**Working on: WebApplication only.**
Do NOT touch AdminSystem files unless explicitly told to.

---

## Known Issues To Fix (Priority Order)

### 1. DeliveryStatusPollJob.cs — LINQ Translation Error
File: `WebApplication/BackgroundJobs/DeliveryStatusPollJob.cs`
Error: `IReadOnlySet<string>.Contains` cannot be translated to SQL by EF Core.
Fix: Replace the `.Where()` using a HashSet with explicit `||` conditions:
```csharp
// WRONG
.Where(d => new HashSet<string> { "PickedUp", "InTransit" }.Contains(d.DeliveryStatus))

// CORRECT
.Where(d => d.DeliveryStatus == DeliveryStatuses.PickedUp
         || d.DeliveryStatus == DeliveryStatuses.InTransit)
```

### 2. _Layout.cshtml — Wrong Partial Path
File: `WebApplication/Views/Shared/_Layout.cshtml`
Error: `The partial view '_NotificationAlert' was not found`
Fix: Change the partial reference to the full path:
```razor
@await Html.PartialAsync("~/Views/Customer/Partials/_NotificationAlert.cshtml")
```

### 3. AppDbContext.cs — Missing HasPrecision on Decimal Properties
File: `WebApplication/DataAccess/Context/AppDbContext.cs`
Warning: Many decimal properties missing `HasPrecision(18, 2)` in `OnModelCreating`.
Affected: CartItem.PriceAtAdd, Order.DiscountAmount, Order.ShippingFee, Order.SubTotal,
OrderItem.UnitPrice, POS_Session.TotalSales, Payment.Amount, PriceHistory.NewPrice,
PriceHistory.OldPrice, Product.Price, ProductVariant.AdditionalPrice,
PurchaseOrderItem.UnitPrice, Voucher.DiscountValue, Voucher.MinimumOrderAmount,
VoucherUsage.DiscountAmount.
Fix: Add `.HasPrecision(18, 2)` to each decimal property in its Configure* method.

### 4. Background Jobs — DB Unavailable Gracefully
All 5 jobs currently try to write to SystemLog at startup which fails when DB is
unreachable (Error 10060 — dev machine IP not whitelisted in Cloud Console).
This is expected behavior, not a code bug. The jobs log the failure and retry on
next cycle. App stays running due to HostOptions.BackgroundServiceExceptionBehavior = Ignore.
No code fix needed here — whitelist the IP in Google Cloud Console to resolve.

---

## Absolute Business Rules — Never Violate These

1. **No COD, no card payments** — WebApplication only accepts GCash and BankTransfer
2. **No refunds** — PaymentStatus = 'Refunded' must never exist anywhere
3. **Couriers: Lalamove and LBC only** — no other courier values ever
4. **Stock in ProductVariant.StockQuantity only** — never on Product
5. **Never store computed fields** — Order.TotalAmount, OrderItem.Subtotal always computed
6. **5 shared-PK entities** must have ValueGeneratedNever(): GCashPayment, BankTransferPayment,
   LalamoveDelivery, LBCDelivery, PickupOrder
7. **Circular FK** — User.DefaultAddressId uses OnDelete(NoAction)
8. **IssueRefund task type must never exist** anywhere in code
9. **InventoryLog is append-only** — never update or delete rows
10. **Address snapshots (IsSnapshot=true) are immutable** after creation

---

## Architecture Rules — Always Follow These

- Controllers are thin — one service call per action, no business logic
- Services own all business logic
- Repositories own all data access — no IQueryable exposed outside repository
- No direct DbContext in controllers (except CartController for guest sessions)
- All relationships via Fluent API in AppDbContext.OnModelCreating only
- No relationship data annotations on entities
- All views use explicit `~/Views/Customer/` paths in controller return statements
- AJAX actions return `ApiResponse.Ok()` or `ApiResponse.Fail()`
- All POST actions have `[ValidateAntiForgeryToken]`
- All status values use constants classes — no magic strings

---

## Controllers and Their View Paths

Every controller uses explicit view paths. Pattern:
```csharp
return View("~/Views/Customer/ViewName.cshtml", vm);
```

| Controller | Views It Renders |
|---|---|
| CustomerController | Login, Register, Dashboard, Profile |
| ProductController | ProductCatalog, ProductDetails |
| CartController | Cart |
| WishlistController | Wishlist |
| CheckoutController | Checkout |
| PaymentController | Payment |
| OrderController | Confirmation, OrderHistory, OrderDetail |
| ReviewController | Review, Reviews |
| SupportController | SupportList, SupportCreate, SupportDetail |
| SupplierController | SupplierList, PurchaseOrderList, PurchaseOrderDetail |
| HomeController | Views/Home/Index, Views/Home/Privacy |

---

## Partials — All In Views/Customer/Partials/

_CartItem.cshtml
_ConfirmationModal.cshtml
_DeliverySelection.cshtml
_NotificationAlert.cshtml
_OrderStatusBadge.cshtml       — model: string (OrderStatus value)
_OrderSummary.cshtml           — model: CheckoutViewModel
_OTPModal.cshtml
_PaymentMethod.cshtml
_PaymentStatusBadge.cshtml     — model: string (PaymentStatus value)
_ProductCard.cshtml            — model: ProductViewModel
_ReviewForm.cshtml             — model: ReviewViewModel
_ReviewList.cshtml             — model: IReadOnlyList<ReviewViewModel>
_SupportTicketCard.cshtml      — model: SupportTicketViewModel
_TicketStatusBadge.cshtml      — model: string (TicketStatus value)

---

## Service Registrations (Program.cs)

All scoped:
- IUserService → UserService
- IProductService → ProductService
- IBrandService → BrandService
- ICartService → CartService
- IWishlistService → WishlistService
- IVoucherService → VoucherService
- IOrderService → OrderService
- IPaymentService → PaymentService
- IReviewService → ReviewService
- ISupportService → SupportService
- INotificationService → NotificationService

Hosted services (background jobs):
- InventorySyncJob (every 6h)
- PendingOrderMonitorJob (every 30min)
- PaymentTimeoutJob (every 15min)
- StockMonitorJob (every 60min)
- DeliveryStatusPollJob (every 20min)

HostOptions.BackgroundServiceExceptionBehavior = Ignore
(prevents job failures from killing the host)

---

## Entity Constants — Always Use These, Never Magic Strings

```
OrderStatuses:         Pending, PendingVerification, OnHold, Processing,
                       ReadyForPickup, PickedUp, Shipped, Delivered, Cancelled
PaymentMethods:        GCash, BankTransfer, Cash (Cash = POS only, never in WebApp)
PaymentStatuses:       Pending, VerificationPending, VerificationRejected,
                       Completed, Failed
PaymentStages:         Upfront, Remaining, Full
DeliveryStatuses:      Pending, PickedUp, InTransit, Delivered, Failed
Couriers:              Lalamove, LBC
TicketStatuses:        Open, InProgress, AwaitingResponse, Resolved, Closed
TicketCategories:      DamagedItem, WrongItem, DeliveryIssue, PaymentIssue,
                       ProductInquiry, ReturnRefund, General
InventoryChangeTypes:  Lock, Unlock, Sale, Purchase, Adjustment, Damage, Loss
NotifTypes:            PaymentReceived, PaymentHeld, PendingOrderAlert,
                       LowStockAlert, WishlistRestock, DeliveryConfirmation,
                       DeliveryDelay, SupportTicketCreated
NotifChannels:         Email, SMS, Push
SystemLogEvents:       BackgroundJobStart, BackgroundJobComplete, BackgroundJobError
RoleNames:             Admin, Manager, Staff, Customer
PurchaseOrderStatuses: Pending, Received, Cancelled
```

---

## NuGet Packages (WebApplication)

- Microsoft.EntityFrameworkCore.SqlServer 8.0.0
- BCrypt.Net-Next 4.0.3
- Google.Cloud.Storage.V1 4.7.0

---

## Key Files Reference

| File | Purpose |
|------|---------|
| `WebApplication/Program.cs` | DI registrations, middleware pipeline |
| `WebApplication/DataAccess/Context/AppDbContext.cs` | 38 DbSets + all Fluent API config |
| `WebApplication/Models/ViewModels/IUserService.cs` | ServiceResult + ServiceResult<T> records |
| `WebApplication/Models/ApiResponse.cs` | ApiResponse.Ok() / ApiResponse.Fail() |
| `WebApplication/wwwroot/js/utils.js` | fetchWithCSRF, showAlert, formatCurrency |

---

## What NOT To Do

- Do not create Views in feature subfolders (no Views/Order/, Views/Review/ etc.)
- Do not add Cash/COD payment options to any WebApp view or service
- Do not add refund logic anywhere
- Do not add any courier other than Lalamove or LBC
- Do not update or delete InventoryLog rows
- Do not expose IQueryable outside of repository classes
- Do not put business logic in controllers
- Do not store computed subtotals or totals in the database
- Do not touch AdminSystem unless explicitly asked

---

## How To Report Fixes

When you fix something, state:
1. Which file was changed
2. What line(s) were changed
3. What the before/after looks like
4. Confirm the fix does not violate any business rule above
