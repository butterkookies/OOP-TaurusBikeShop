# Taurus Bike Shop — Project Context

## 1. Identity

- **Project Name**: Taurus Bike Shop (OOP-TaurusBikeShop)
- **Type**: Full-stack e-commerce platform + desktop admin system
- **Purpose**: Dual-purpose — a school project for an OOP course and a real business application for an actual bike shop
- **Target Users**: Students (developers), the system client (bike shop owner), and evaluators/professors
- **Repository**: `butterkookies/OOP-TaurusBikeShop`

---

## 2. Tech Stack

### WebApplication (ASP.NET Core MVC)
| Layer | Technology |
|-------|-----------|
| Language | C# (.NET 8) |
| Framework | ASP.NET Core 8 MVC |
| ORM | Entity Framework Core 8 (Fluent API only, no data annotations for relationships) |
| Database | Microsoft SQL Server on Google Cloud SQL (`35.221.161.150:1433`) |
| Authentication | Cookie-based + session (30-min idle timeout) |
| Password Hashing | BCrypt.Net-Next 4.0.3 (work factor 12) |
| File Storage | Google Cloud Storage v1 (bucket: `taurus-bikeshop-assets`) |
| Frontend | Vanilla JS, Bootstrap 5 (CDN), jQuery 3.x (CDN) |
| Fonts | Google Fonts (Oswald, Nunito Sans) |

### AdminSystem (WPF Desktop)
| Layer | Technology |
|-------|-----------|
| Language | C# (.NET 4.8) |
| Framework | WPF (Windows Presentation Foundation) |
| Data Access | Dapper 2.1.35 |
| Database | Same SQL Server instance |
| Pattern | MVVM |

### DevOps
| Tool | Details |
|------|---------|
| Version Control | Git + GitHub |
| Package Managers | NuGet (C#), npm (Mermaid CLI only) |
| IDE | Visual Studio 2022 |
| Cloud | Google Cloud SQL Server (free trial, ~88 days remaining as of March 2026) |

---

## 3. Architecture

### Layered Architecture (Both Systems)

```
Controllers (Thin — one service call per action)
    ↓
Services (Business logic, validation, transformation)
    ↓
Repositories (IRepository<T> generic + specialized repos)
    ↓
DbContext / Dapper (Data access)
    ↓
SQL Server (Google Cloud SQL)
```

### Folder Structure

```
OOP-TaurusBikeShop/
├── WebApplication/
│   ├── BackgroundJobs/          5 hosted services
│   ├── BusinessLogic/
│   │   ├── Interfaces/          10 service interfaces
│   │   └── Services/            10 service implementations
│   ├── Controllers/             11 controllers
│   ├── DataAccess/
│   │   ├── Context/             AppDbContext (1,341 lines, 38 DbSets)
│   │   └── Repositories/       12 repositories (generic + specialized)
│   ├── Models/
│   │   ├── Entities/            38 entity classes + constants
│   │   └── ViewModels/          13 view models
│   ├── Utilities/               FileUploadHelper, PasswordHelper, ValidationHelper
│   ├── Views/
│   │   ├── Customer/            All customer views + Partials/
│   │   ├── Home/                Index, Privacy
│   │   └── Shared/              _Layout, _Navbar, _Footer, Error
│   ├── wwwroot/
│   │   ├── css/                 30+ feature-scoped CSS files
│   │   ├── js/                  16 feature-scoped JS files
│   │   └── lib/                 Bootstrap, jQuery
│   ├── Program.cs               DI registration, middleware (500+ lines)
│   └── appsettings*.json        Config (connection strings, session, GCS)
│
├── AdminSystem/
│   └── AdminSystem/AdminSystem/
│       ├── Models/              Dapper DTOs (mirrors WebApp entities)
│       ├── Repositories/        5 repositories + IRepository
│       ├── Services/            8 services
│       ├── ViewModels/          10+ MVVM ViewModels
│       ├── Views/               12 XAML windows
│       ├── Helpers/             Converters, password, database helpers
│       └── Converters/          WPF value converters
│
├── Documentation/               Project docs
├── Mermaid/                     Diagrams
├── SQL/                         Database scripts (READ-ONLY reference)
└── prompt/                      AI prompt templates
```

### Key Files

| File | Lines | Purpose |
|------|-------|---------|
| `WebApplication/DataAccess/Context/AppDbContext.cs` | 1,341 | 38 DbSets, all Fluent API config, relationships, cascade rules, 5 shared-PK subtypes |
| `WebApplication/Program.cs` | 500+ | DI registration (11 services + 5 jobs), middleware pipeline, EF Core setup with retry |
| `WebApplication/Models/ApiResponse.cs` | ~50 | Standard JSON envelope for all AJAX: `{ success, message, data }` |
| `WebApplication/Utilities/FileUploadHelper.cs` | 150+ | Google Cloud Storage uploads |
| `WebApplication/Utilities/PasswordHelper.cs` | ~70 | BCrypt hash/verify wrapper |
| `WebApplication/wwwroot/js/utils.js` | 200+ | `fetchWithCSRF()`, `showAlert()`, `formatCurrency()` |

### Background Jobs (Hosted Services)

| Job | Interval | Purpose |
|-----|----------|---------|
| InventorySyncJob | 6 hours | Sync stock from AdminSystem POS → WebApp |
| PendingOrderMonitorJob | 30 min | Monitor orders awaiting payment verification |
| PaymentTimeoutJob | 15 min | Fail timed-out GCash/BankTransfer payments |
| StockMonitorJob | 60 min | Low-stock alerts and notifications |
| DeliveryStatusPollJob | 5 min | Poll courier APIs for delivery updates |

### External Services

| Service | Usage | Status |
|---------|-------|--------|
| Google Cloud SQL | SQL Server database hosting | Active |
| Google Cloud Storage | Product images, payment proofs, support attachments | Active |
| Lalamove API | Delivery booking & tracking | Stubbed (not yet integrated) |
| LBC API | Delivery booking & tracking | Stubbed (not yet integrated) |

---

## 4. Coding Conventions

### Naming
| Element | Convention | Example |
|---------|-----------|---------|
| Classes | PascalCase | `ProductService`, `OrderViewModel` |
| Methods | PascalCase, verb-first, `*Async` suffix | `GetFilteredAsync`, `CreateOrderAsync` |
| Properties | PascalCase, full names | `OrderId`, `ShippingAddress` |
| Private fields | `_camelCase` | `_productRepo`, `_logger` |
| Parameters | camelCase | `productId`, `cancellationToken` |
| Constants | PascalCase in static classes | `OrderStatuses.Pending`, `Couriers.Lalamove` |

### Patterns
- **Repository Pattern**: Generic `IRepository<T>` + specialized repos; never expose `IQueryable`
- **Service Layer**: All business logic in services, controllers are thin
- **Dependency Injection**: Constructor injection, all services scoped
- **Fluent API Only**: All EF Core config in `OnModelCreating` (no data annotations for relationships)
- **Async/Await**: All DB calls async with `CancellationToken`
- **ApiResponse Envelope**: `ApiResponse.Ok()` / `ApiResponse.Fail()` for all AJAX endpoints
- **MVVM** (AdminSystem): ViewModels with INotifyPropertyChanged, ICommand

### View Conventions
- All controllers use explicit view paths: `return View("~/Views/Customer/Cart.cshtml", vm);`
- Partials live in `Views/Customer/Partials/` with `_Prefix` naming
- One CSS file and one JS file per feature in `wwwroot/css/` and `wwwroot/js/`

### Security
- Anti-forgery tokens on all POST actions (global filter + `[ValidateAntiForgeryToken]`)
- `fetchWithCSRF()` wrapper in JavaScript for all AJAX calls
- BCrypt password hashing (work factor 12)
- HTTPS enforcement in production
- Nullable reference types enabled

---

## 5. Current State

### Working
- Full user auth (registration, login, OTP verification)
- Product catalog (filtering, search, pagination)
- Shopping cart (guest sessions + authenticated)
- Wishlist
- Checkout flow with delivery method selection
- GCash & BankTransfer payment creation and verification
- Order creation with inventory locking
- Order history and detail views
- Product reviews and ratings
- Customer support tickets
- Supplier management (PurchaseOrders)
- Voucher application and validation
- All 5 background jobs registered and running
- Google Cloud Storage file uploads
- Session management (30-min idle timeout)
- Responsive design (Bootstrap + custom CSS)
- AJAX endpoints with ApiResponse envelope

### Known Bugs (High Priority)

1. **DeliveryStatusPollJob — LINQ Translation Error**
   - `HashSet<string>.Contains()` in `.Where()` fails in EF Core LINQ translation
   - Fix: Replace with explicit `||` conditions using `DeliveryStatuses` constants

2. **_Layout.cshtml — Partial View Path Error**
   - `@await Html.PartialAsync("_NotificationAlert")` cannot resolve the partial
   - Fix: Use full path `~/Views/Customer/Partials/_NotificationAlert.cshtml`

### Known Issues (Medium Priority)

3. **AppDbContext — Missing HasPrecision on Decimal Properties**
   - Many decimal columns lack `.HasPrecision(18, 2)` — affects: CartItem.PriceAtAdd, Order.DiscountAmount, Order.ShippingFee, Order.SubTotal, OrderItem.UnitPrice, POS_Session.TotalSales, Payment.Amount, PriceHistory.NewPrice/OldPrice, Product.Price, ProductVariant.AdditionalPrice, PurchaseOrderItem.UnitPrice, Voucher.DiscountValue/MinimumOrderAmount, VoucherUsage.DiscountAmount

4. **Courier API Integration — Stubs Only**
   - `PollLalamoveAsync()` and `PollLBCAsync()` return null; awaiting API client implementation

### Technical Debt
- Background jobs attempt SystemLog write on startup; fail if dev IP not whitelisted in Cloud Console (expected, no code fix needed)
- Session cookie `SecurePolicy=None` in development (correct for dev, must be `Always` in production)
- No CI/CD pipeline configured
- No EF Core migrations committed (schema managed externally)

---

## 6. My Preferences

### Response Style
- **Concise, code-only** — no lengthy explanations unless asked
- Full file output for new files; patch-style diffs for updates
- Later (post-completion): will request a full project files library with context and purpose for each file

### Always Do
- Use async/await for all database operations
- Follow existing naming conventions and patterns
- Respect the layered architecture (Controller → Service → Repository → DbContext)
- Use existing constants (`OrderStatuses`, `PaymentMethods`, etc.) instead of magic strings
- Test that LINQ queries translate to valid SQL (no client-side evaluation)

### Never Do
- Never modify the database schema or structure (tables, columns, constraints, indexes)
- Never add new NuGet packages without explicit approval
- Never edit unrelated files unless explicitly instructed
- Never introduce `IQueryable` leaks from repositories
- Never put business logic in controllers
- Never use data annotations for EF Core relationship configuration

### Output Format
- Code in full files or patch-style diffs
- No emojis unless requested
- No markdown documentation files unless requested

---

## 7. Constraints

### Deadline
- **3rd week of April 2026** (end of semester)

### Resource Limits
- Google Cloud SQL Server free trial — ~88 days remaining (as of March 2026)
- No additional cloud budget assumed

### Off-Limits
- **Database schema**: Read and write only. No CREATE, ALTER, DROP on tables/columns without explicit approval
- **Connection strings**: Do not modify existing database connections
- **Unrelated files**: Do not edit files outside the scope of the current task

### Scope
- Both **WebApplication** (ASP.NET Core MVC) and **AdminSystem** (WPF) are in scope
- Claude can analyze, refactor, and fix broken or inefficient files in either system
- Priority: Get all components functional and coherent before implementing new features

---

## 8. Quick Reference — Entity Constants

```csharp
OrderStatuses:         Pending, PendingVerification, OnHold, Processing,
                       ReadyForPickup, PickedUp, Shipped, Delivered, Cancelled

PaymentMethods:        GCash, BankTransfer, Cash (Cash = POS only, NEVER in WebApp)

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

RoleNames:             Admin, Manager, Staff, Customer

PurchaseOrderStatuses: Pending, Received, Cancelled

CurrencyCodes:         PHP, USD, EUR
```

## 9. Critical Business Rules (Immutable)

1. **Payment Methods**: GCash, BankTransfer only in WebApp (Cash is POS-only via AdminSystem)
2. **No Refunds**: `PaymentStatus = 'Refunded'` must never exist
3. **Couriers**: Lalamove and LBC only — no other courier values ever
4. **Stock on Variants**: `ProductVariant.StockQuantity` only — never on `Product`
5. **No Stored Computed Fields**: `Order.TotalAmount = SubTotal - DiscountAmount + ShippingFee` (computed in view, never persisted)
6. **5 Shared-PK 1:1 Subtypes**: GCashPayment, BankTransferPayment, LalamoveDelivery, LBCDelivery, PickupOrder (`ValueGeneratedNever()`)
7. **Circular FK Break**: `User.DefaultAddressId → Address` uses `OnDelete(NoAction)` to prevent cascade
8. **InventoryLog is Append-Only**: Never update or delete rows — only insert
9. **Address Snapshots are Immutable**: `IsSnapshot=true` addresses are never updated after creation
10. **No IssueRefund Task Type**: Does not exist in `SupportTask.TaskType` constraint
