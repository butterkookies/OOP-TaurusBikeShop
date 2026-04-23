
---

## PILLAR 8 — ADMIN SYSTEM FLOW (The Employee Back-Office)

### Admin Workflows

| # | Workflow | XAML View | ViewModel | Key Service/Repository |
|---|----------|-----------|-----------|----------------------|
| 1 | **Login / Authentication** | [LoginView.xaml](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/AdminSystem_v2/Views/LoginView.xaml) | `LoginViewModel` | `AuthService` → `UserRepository` (Dapper BCrypt verify) |
| 2 | **Dashboard / KPIs** | [DashboardView.xaml](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/AdminSystem_v2/Views/DashboardView.xaml) | `DashboardViewModel` | `ProductService`, `InventoryService`, `OrderService` — OxyPlot charts for sales trends, order volume, top products |
| 3 | **Order Management** | [OrdersView.xaml](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/AdminSystem_v2/Views/OrdersView.xaml) (80 KB) | `OrderViewModel` (32 KB) | `OrderService` → `OrderRepository` — status transitions, payment verification, delivery booking, bulk actions |
| 4 | **Payment Verification** | (within [OrdersView.xaml](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/AdminSystem_v2/Views/OrdersView.xaml)) | `OrderViewModel` | `OrderRepository` — approve/reject GCash screenshots and bank transfer proofs |
| 5 | **POS Transaction** | [POSView.xaml](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/AdminSystem_v2/Views/POSView.xaml) (69 KB) | `POSViewModel` (25 KB) | `POSService` → `POSRepository` — walk-in cash sales, barcode scanning, receipt generation |
| 6 | **Inventory Management** | (within [ProductsView.xaml](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/AdminSystem_v2/Views/ProductsView.xaml)) | `ProductViewModel` | `InventoryService` → `InventoryRepository` — stock adjustments, InventoryLog entries |
| 7 | **Product Management** | [ProductsView.xaml](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/AdminSystem_v2/Views/ProductsView.xaml) (43 KB) | `ProductViewModel` (19 KB) | `ProductService` → `ProductRepository` — products, variants, categories, brands, images |
| 8 | **Supplier / Purchase Orders** | (within [ProductsView.xaml](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/AdminSystem_v2/Views/ProductsView.xaml)) | `ProductViewModel` | `ProductService` → `ProductRepository` — supplier records, PO creation and receiving |

Additional workflows: **Staff Management** ([StaffView.xaml](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/AdminSystem_v2/Views/StaffView.xaml) / `StaffViewModel`), **Reports** ([ReportsView.xaml](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/AdminSystem_v2/Views/ReportsView.xaml) / `ReportViewModel` with `ExcelExportService`), **Vouchers** ([VoucherView.xaml](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/AdminSystem_v2/Views/VoucherView.xaml) / [VoucherViewModel](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/Controllers/CheckoutController.cs#221-233)), **Support Tickets** ([SupportTicketsView.xaml](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/AdminSystem_v2/Views/SupportTicketsView.xaml) / `SupportTicketsViewModel`), **Store Payment Accounts** ([StorePaymentAccountView.xaml](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/AdminSystem_v2/Views/StorePaymentAccountView.xaml) / `StorePaymentAccountViewModel`).

### 8a. Order Status Transition Graph

```text
Pending ──→ PendingVerification ──→ Processing ──→ ReadyForPickup ──→ PickedUp
   │              │                      │                                │
   │              ▼                      │                                ▼
   │         OnHold ◄──────────────────  │                           Delivered
   │              │                      │
   ▼              ▼                      ▼
Cancelled    Cancelled              Shipped ──→ Delivered
```

**Allowed transitions:**
- [Pending](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/BackgroundJobs/NotificationDispatchJob.cs#71-133) → `PendingVerification` (payment proof uploaded) or `Cancelled` (auto/manual)
- `PendingVerification` → `Processing` (payment verified) or `OnHold` (verification timeout) or `Cancelled`
- `OnHold` → `Processing` (issue resolved) or `Cancelled`
- `Processing` → `ReadyForPickup` (pickup orders) or `Shipped` (delivery orders)
- `ReadyForPickup` → `PickedUp` → `Delivered`
- `Shipped` → `Delivered`

**Forbidden:** Any backward transition (e.g., `Processing` → [Pending](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/BackgroundJobs/NotificationDispatchJob.cs#71-133)), `Delivered` → anything. The AdminSystem's `OrderViewModel` enforces this with an `InvalidStatusTransitionException`.

### 8b. InventoryLog — Append-Only Audit Trail

`InventoryLog` is an **append-only ledger** — rows are only ever INSERT-ed, never UPDATE-d or DELETE-d. *It's like a bank statement: once a transaction is recorded, it can never be erased.* Every stock change (receive from supplier, sell to customer, manual adjustment, lock for pending order, unlock on cancellation) creates a new row with `ChangeType` (Receive, Sell, Adjust, Lock, Unlock), `ChangeQuantity`, and `Notes`.

### 8c. MVVM Binding: How ViewModel Changes Update the View

All ViewModels inherit from [BaseViewModel.cs](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/AdminSystem_v2/ViewModels/BaseViewModel.cs) which implements `INotifyPropertyChanged`. When a property changes:
1. The setter calls [SetProperty(ref field, value)](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/AdminSystem_v2/ViewModels/BaseViewModel.cs#13-20) which fires [PropertyChanged](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/AdminSystem_v2/ViewModels/BaseViewModel.cs#10-12)
2. WPF data bindings listen for [PropertyChanged](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/AdminSystem_v2/ViewModels/BaseViewModel.cs#10-12) events
3. The bound XAML element re-reads the property value and re-renders

### 8d. Dapper vs EF Core Tradeoff

| Aspect | EF Core (WebApp) | Dapper (AdminSystem) |
|--------|-------------------|---------------------|
| **Abstraction** | High — LINQ queries, change tracking | Low — raw SQL, manual mapping |
| **Performance** | Slightly slower due to change tracker overhead | Faster — minimal overhead, direct ADO.NET |
| **Control** | Less — EF Core decides the SQL shape | Full — you write exactly the SQL you want |
| **Why chosen** | WebApp has 38 DbSets with complex relationships | AdminSystem needs fast, focused queries for dashboards/reports |

---

## PILLAR 9 — CRITICAL CORE SCRIPTS (The VIP Engines)

### 1. AppDbContext.cs — The Database Blueprint

- **File:** [WebApplication/DataAccess/Context/AppDbContext.cs](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/DataAccess/Context/AppDbContext.cs) + 8 partial class files
- **Purpose:** Defines the EF Core model for all 38 tables using Fluent API exclusively.
- **Critical method:** [OnModelCreating()](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/DataAccess/Context/AppDbContext.cs#106-151) delegates to 39 `Configure*()` methods across domain-specific partial files (Auth, Catalog, Commerce, Delivery, Orders, Payments, SupplyChain, SupportComms).
- *Real-world analogy: The architectural drawing of the building — every wall, door, and load-bearing column is defined here.*

### 2. Program.cs — The Master Control Panel

- **File:** [WebApplication/Program.cs](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/Program.cs) (369 lines)
- **Purpose:** Configures DI container, database connection (pooled, 32 max), authentication, middleware pipeline, and all 6 background job registrations.
- **Critical sections:** DbContext pooling, EF Core retry policy (3 retries, 5s max), cookie auth (30-min idle, 1-hour sliding), global anti-forgery filter, GCS fallback, Kestrel 15 MB upload limit.
- *Real-world analogy: The electrical panel — flip the wrong breaker and the whole shop goes dark.*

### 3. ApiResponse.cs — The Communication Contract

- **File:** [WebApplication/Models/ApiResponse.cs](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/Models/ApiResponse.cs) (62 lines)
- **Purpose:** Standard JSON envelope: `{ success: bool, message: string?, data: object? }`
- **Factory methods:** `ApiResponse.Ok(data?, message?)` and `ApiResponse.Fail(message)`
- *Real-world analogy: The standard reply form — yes or no, always in the same format.*

### 4. utils.js — The Frontend Toolkit

- **File:** [WebApplication/wwwroot/js/utils.js](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/wwwroot/js/utils.js) (165 lines)
- **Key functions:** [fetchWithCSRF()](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/wwwroot/js/utils.js#6-23), [showToast()](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/wwwroot/js/utils.js#29-65), [formatCurrency()](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/wwwroot/js/utils.js#66-73), [showAlert()](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/wwwroot/js/utils.js#74-102), [showSpinner()](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/wwwroot/js/utils.js#103-110)/[hideSpinner()](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/wwwroot/js/utils.js#111-116), [debounce()](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/wwwroot/js/utils.js#130-139), [throttle()](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/wwwroot/js/utils.js#140-151), [updateCartBadge()](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/wwwroot/js/utils.js#122-129)
- *Real-world analogy: The tool belt every employee wears.*

### 5. PendingOrderMonitorJob.cs — The Async Job Pattern Exemplar

- **File:** [WebApplication/BackgroundJobs/PendingOrderMonitorJob.cs](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/BackgroundJobs/PendingOrderMonitorJob.cs) (300 lines)
- **Purpose:** Enforces 24-hour pending order policy — pre-expiry reminders + auto-cancellation + inventory unlock.
- **Key detail:** Uses `DateTime.Now` (not UtcNow) because `OrderDate` is stored in local time (UTC+8). Saves status changes BEFORE queueing notifications.
- *Real-world analogy: The manager who checks every morning for unpaid layaway — after 24 hours, items go back on the shelf.*

---

## PILLAR 10 — SECURITY & AUTHENTICATION (The Locks & Keys)

### 10a. Auth Flow

1. Customer POST `/Customer/Login` with email + password
2. `UserService.AuthenticateAsync()` → `UserRepository` loads user → `BCrypt.Verify(password, hash)`
3. [SignInUserAsync()](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/Controllers/CustomerController.cs#622-646) creates `ClaimsIdentity` (NameIdentifier, Name, Email, Role)
4. `HttpContext.SignInAsync()` sets encrypted `.TaurusBikeShop.Auth` cookie
5. 1-hour `ExpireTimeSpan` with `SlidingExpiration = true` (each request resets timer)
6. Session cookie `.TaurusBikeShop.Session`: 30-minute idle timeout
7. Logout: `HttpContext.SignOutAsync()` clears cookie

### 10b. Password Hashing

BCrypt.Net-Next, **work factor 12** (2^12 = 4,096 iterations, ~250ms per hash). *Too fast for users to notice, too slow for attackers to brute-force millions of passwords.*

### 10c. Anti-Forgery (CSRF Protection)

- **Server:** `AutoValidateAntiforgeryTokenAttribute` global filter (Program.cs:L303)
- **Client:** [fetchWithCSRF()](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/wwwroot/js/utils.js#6-23) sends `RequestVerificationToken` header
- **Token endpoint:** GET `/antiforgery/token` for pages without `<form>` elements
- *CSRF attack explained: A malicious site tricks your browser into submitting a form to Taurus while you're logged in. The anti-forgery token blocks this.*

### 10d. Secrets Management

- **Dev:** .NET User Secrets (`dotnet user-secrets set`)
- **Prod:** Environment variables (double-underscore: `ConnectionStrings__Taurus-bike-shop-sqlserver-2026`)
- **appsettings.json:** Only placeholder values. Priority: appsettings → User Secrets → Env Vars
- **Full inventory:** See [SECRETS.md](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/SECRETS.md)

### 10e. HTTPS

- **Dev:** `CookieSecurePolicy.None` (HTTP OK for localhost)
- **Prod:** `CookieSecurePolicy.Always` (auto-switched via `builder.Environment.IsDevelopment()`)

> [!IMPORTANT]
> Deployment checklist: Verify SecurePolicy is Always in production.

### 10f. Role-Based Access

| Role | Access |
|------|--------|
| **Customer** | WebApp: catalog, cart, checkout, orders, reviews, support, profile |
| **Admin** | Full AdminSystem access — all workflows + staff management |
| **Manager** | AdminSystem: orders, inventory, reports, POS |
| **Staff** | AdminSystem: POS, order processing, basic inventory |

---

## PILLAR 11 — ERROR HANDLING & LOGGING (The Safety Nets)

### 11a. Global Error Handling

- **Dev:** Developer Exception Page (full stack traces, SQL queries)
- **Prod:** `UseExceptionHandler("/Home/Error")` — generic error page
- **Fatal crash:** `AppDomain.UnhandledException` + `TaskScheduler.UnobservedTaskException` handlers write to stderr (Program.cs:L18–29)

### 11b. EF Core Retry Policy

```csharp
sqlOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null);
```
**Why:** Google Cloud SQL connections can drop during maintenance/failover. Transient errors are auto-retried.

### 11c. SystemLog Table

Events logged: job start/complete/error, order status changes, inventory sync, low stock alerts, delivery polls, payment timeouts. **Fails at dev startup** when IP not whitelisted — expected and safe.

### 11d. Background Job Error Handling

Jobs catch exceptions, implement **exponential backoff** (capped at 2 min), and **never stop permanently**. `BackgroundServiceExceptionBehavior.Ignore` prevents a job crash from killing the app.

### 11e. Payment Upload Consistency Gap

> [!WARNING]
> **Known limitation:** If GCS upload succeeds but the DB write fails, the file is orphaned in GCS with no database record. No automatic cleanup exists. Risk is low (GCS + EF retry are reliable) but this is a recognized gap.

---

## PILLAR 12 — TESTING & QA WORKFLOWS (The Health Inspectors)

### 12a. Automated Tests

**No unit or integration tests exist.** No `*.Tests` projects, no CI pipeline. This is the most significant quality gap.

### 12b. Manual QA

- **Build check:** `dotnet build` — 0 warnings, 0 errors
- **Browser testing:** Manual flow-through of all customer journeys
- **Postman:** API endpoint testing (`.postman/` config in repo)
- **XAML audit:** Visual consistency checks via `/code-quality-suite` workflow

### 12c. Build Verification Baseline

```bash
cd WebApplication && dotnet build    # 0 warnings, 0 errors
cd AdminSystem_v2 && dotnet build    # 0 warnings, 0 errors
```

### 12d. LINQ Safety Rule

Every LINQ query must translate to valid SQL. The DeliveryStatusPollJob bug (`HashSet<T>.Contains()` failing SQL translation) demonstrates the danger — replaced with explicit `||` conditions. **Verify SQL output** via EF Core logging at `Information` level.

### 12e. Recommended First 3 Tests

1. **Order creation integration test** — Cart → `CreateOrderAsync()` → verify Order, OrderItems, InventoryLog Lock entries, Payment
2. **PendingOrderMonitorJob unit test** — Seed 25h-old Pending order → run cycle → verify Cancelled + Unlock entries
3. **ApiResponse serialization test** — Verify exact JSON shape `{ success, message, data }`

---

## PILLAR 13 — BUILD & INFRASTRUCTURE (Turning on the Lights)

### 13a. WebApplication

```bash
cd WebApplication
dotnet build    # 0 warnings, 0 errors
dotnet run      # Kestrel → http://localhost:5000 (or PORT env var)
```

### 13b. AdminSystem_v2

1. Open [AdminSystem_v2.slnx](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/AdminSystem_v2/AdminSystem_v2.slnx) in Visual Studio 2022
2. Set `AdminSystem_v2` as startup project
3. Build → Run (F5)
4. **Requires:** Windows (WPF), SQL Server accessible

### 13c. Prerequisites

| Prerequisite | Purpose |
|-------------|---------|
| .NET 8 SDK | Build both apps |
| Visual Studio 2022 | WPF/XAML designer |
| SQL Server Express | Local DB (or Cloud SQL access) |
| SSMS | Schema management |
| Google Cloud creds | GCS + Cloud SQL access |
| Postman | API testing (`.postman/` in repo) |

### 13d. Configuration

| File | Purpose |
|------|---------|
| [appsettings.json](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/AdminSystem_v2/appsettings.json) | Shared: session, GCS bucket, SMTP host, logging |
| [appsettings.Development.json](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/appsettings.Development.json) | Dev: verbose logging, `SecurePolicy: None` |

**Before first run** (via User Secrets): connection string, SMTP credentials, Cloudinary keys, `GOOGLE_APPLICATION_CREDENTIALS` env var.

### 13e. Known Startup Issue

SystemLog write fails if dev IP not whitelisted in Google Cloud Console firewall. **Expected, safe to ignore.** Jobs handle gracefully and retry. Fix: add IP to Cloud SQL authorized networks.

### 13f. No CI/CD, No EF Migrations

- No CI/CD pipeline (GitHub Actions, etc.)
- No EF Core migrations used in production
- Schema managed via `SQL\Schema\` scripts applied through SSMS
