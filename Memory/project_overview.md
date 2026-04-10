# Project Overview

## Identity
- **Name**: OOP-TaurusBikeShop
- **Description**: A two-application system for a bike shop business — a customer-facing e-commerce website and a staff/admin desktop app. Finals project for DB-1 and OOP courses.
- **Current Version**: 1.0.0 (from package.json; no git tags exist)
- **Current State**: Active development (school project, features still being added and fixed)

## Stack
- **Language(s)**: C# (.NET 8.0), JavaScript (frontend), SQL
- **Runtime**: .NET 8.0 (both projects target net8.0)
- **Framework(s)**:
  - WebApplication: ASP.NET Core MVC (.NET 8.0) with Entity Framework Core 8.0
  - AdminSystem_v2: WPF (.NET 8.0-windows) with Dapper 2.1.35
- **UI Layer**:
  - WebApplication: Razor Views (cshtml) with vanilla JS
  - AdminSystem_v2: WPF XAML
- **Styling**:
  - WebApplication: Custom CSS (one file per feature in wwwroot/css/), Tailwind CSS 4.2.2 (devDependency in package.json)
  - AdminSystem_v2: WPF ResourceDictionary styles (App.xaml)
- **State Management**: Server-side sessions (ASP.NET Core distributed memory cache); WPF MVVM pattern (ViewModels with INotifyPropertyChanged)

## Data
- **Database**: SQL Server (Google Cloud SQL instance: `Taurus-bike-shop-sqlserver-2026`). Compatibility level 160 (SQL Server 2022).
- **ORM / Query Layer**:
  - WebApplication: Entity Framework Core 8.0 (SqlServer provider) — 38+ DbSets, Fluent API configuration split across partial classes (AppDbContext.Auth.cs, .Catalog.cs, .Commerce.cs, .Delivery.cs, .Orders.cs, .Payments.cs, .SupplyChain.cs, .SupportComms.cs)
  - AdminSystem_v2: Dapper (raw SQL via repositories)
- **Cache**: In-memory distributed cache (ASP.NET Core DistributedMemoryCache for sessions)
- **Storage**: Google Cloud Storage (bucket: `taurus-bikeshop-assets` for product images, payment proofs, support attachments), Cloudinary (product photos via CloudinaryDotNet 1.28.0)

## Architecture
- **Type**: Multi-project solution (monorepo with 2 apps sharing one database)
- **Entry Points**:
  - `WebApplication/Program.cs` — ASP.NET Core web app startup
  - `AdminSystem_v2/App.xaml.cs` → `AdminSystem_v2/MainWindow.xaml.cs` — WPF desktop app startup
- **Key Directories**:
  ```
  OOP-TaurusBikeShop/
  ├── WebApplication/                    ASP.NET Core MVC customer-facing web app
  │   ├── BackgroundJobs/                5 hosted background services (inventory sync, order monitor, payment timeout, stock monitor, delivery poll)
  │   ├── BusinessLogic/
  │   │   ├── Interfaces/                11 service interfaces (IUserService, IProductService, etc.)
  │   │   └── Services/                  11 service implementations
  │   ├── Controllers/                   12 MVC controllers (Home, Customer, Product, Cart, Wishlist, Checkout, Payment, Order, Review, Support, Supplier, Voucher)
  │   ├── DataAccess/
  │   │   ├── Context/                   AppDbContext split into 8 partial class files
  │   │   └── Repositories/              IRepository<T> generic + 11 specific repositories
  │   ├── Models/
  │   │   ├── Entities/                  38+ entity classes
  │   │   └── ViewModels/               13 view models
  │   ├── Utilities/                     FileUploadHelper, PasswordHelper, ValidationHelper
  │   ├── Views/
  │   │   ├── Customer/                  37 views (all customer pages + Partials/ subfolder with 14 partial views)
  │   │   ├── Home/                      Index.cshtml, Privacy.cshtml
  │   │   └── Shared/                    _Layout.cshtml, _Navbar.cshtml, _Footer.cshtml, Error.cshtml
  │   └── wwwroot/
  │       ├── css/                       32 CSS files (one per feature)
  │       └── js/                        17 JS files (one per feature)
  ├── AdminSystem_v2/                    WPF desktop admin/staff app
  │   ├── Converters/                    4 WPF value converters
  │   ├── Helpers/                       DatabaseHelper, PageNames, PasswordHelper, RelayCommand
  │   ├── Models/                        20+ data models
  │   ├── Repositories/                  7 repository interfaces + implementations
  │   ├── Services/                      8 service interfaces + implementations
  │   ├── ViewModels/                    10 view models (MVVM)
  │   └── Views/                         9 XAML views (Login, Dashboard, Products, Orders, POS, Voucher, Reports, Staff, Main)
  ├── SQL/
  │   ├── Schema/                        Taurus_schema_8.0.sql (SSMS-exported full DDL)
  │   └── Seed/                          Taurus_seed_v7.1.sql
  ├── Documentation/                     Project context docs, image URLs
  ├── Mermaid/                           Architecture diagrams (v1-v7)
  └── .github/workflows/                Empty (no CI/CD pipelines configured)
  ```
- **API Style**: MVC (server-rendered Razor views + AJAX endpoints returning ApiResponse JSON)

## Deployment
- **Target Platform**: Google Cloud (SQL Server on Google Cloud SQL); web app deployment target not yet configured
- **Deploy Method**: None found (no Dockerfile, no deploy scripts, no cloud config files)
- **CI/CD**: None — `.github/workflows/` directory exists but is empty
- **Environments**: Development (local with User Secrets) and Production (environment variables) — no staging environment configured

## Repository State
- **Current Branch**: main
- **Latest Commits**:
  ```
  9b322ca Removed AdminSystem folder, keep v2 only
  27338e3 feat: Update security remediation plan with completed tasks and credential management improvements
  5a8a099 Git backup unchanged main
  f12dc78 fix security issues
  47153f1 feat: Implement full voucher integration in AdminSystem_v2 POS module with validation and audit trail
  9b5570b Remove obsolete SQL schema files for version 7.1 and 7.2; add new model for POS voucher results
  227ec02 feat: Enhance search bar visibility and update styles across views
  da9b423 Add Voucher management functionality with ViewModel, View, and code-behind
  bd09a2d feat: Add new POS module with integrated payment processing and UI enhancements
  7b20007 Added features to AdminSystem_v2
  ```
- **Latest Tags**: None
- **Working Tree**: Clean (only untracked file: `prompt.md`)
- **Open Branches**:
  - Local: `main` (current), `Andrei/Project/AdminSystem`, `Andrei/Web/Fix-1`, `TempBranch`, `claude/awesome-mccarthy`, `claude/clever-jemison`, `claude/condescending-elgamal`, `claude/infallible-benz`, `claude/optimistic-cerf`, `main-temp`, `phase1`
  - Remote: `origin/main`, `origin/Admin1`, `origin/AdminBranch`, `origin/Andrei/Project/AdminSystem`, `origin/AndreiWeb1`, `origin/CelonAdmin1`, `origin/phase1`, plus 10 `origin/claude/*` branches

## Dependencies

### WebApplication (NuGet)
| Package | Version | Purpose |
|---------|---------|---------|
| Microsoft.EntityFrameworkCore.SqlServer | 8.0.0 | EF Core SQL Server provider |
| Microsoft.EntityFrameworkCore.Tools | 8.0.0 | EF Core migrations & scaffolding (dev) |
| BCrypt.Net-Next | 4.0.3 | Password hashing (work factor 12) |
| Google.Cloud.Storage.V1 | 4.7.0 | File uploads to GCS |
| CloudinaryDotNet | 1.28.0 | Product image hosting via Cloudinary |

### AdminSystem_v2 (NuGet)
| Package | Version | Purpose |
|---------|---------|---------|
| Microsoft.Data.SqlClient | 5.2.1 | SQL Server ADO.NET driver |
| Dapper | 2.1.35 | Micro-ORM for data access |
| BCrypt.Net-Next | 4.0.3 | Password hashing |
| Microsoft.Extensions.Configuration | 8.0.0 | App configuration |
| Microsoft.Extensions.Configuration.Json | 8.0.0 | JSON config file support |
| Microsoft.Extensions.Configuration.UserSecrets | 8.0.0 | Dev secrets management |
| Microsoft.Extensions.Configuration.EnvironmentVariables | 8.0.0 | Prod env vars |
| OxyPlot.Wpf | 2.1.0 | Charts/graphs in reports |

### Root package.json (npm — tooling only)
| Package | Version | Purpose |
|---------|---------|---------|
| @mermaid-js/mermaid-cli | ^11.12.0 | Render Mermaid architecture diagrams |
| tailwindcss | ^4.2.2 | CSS utility framework (dev) |
| @tailwindcss/postcss | ^4.2.2 | PostCSS integration for Tailwind (dev) |
| autoprefixer | ^10.4.27 | CSS vendor prefixes (dev) |
| postcss | ^8.5.8 | CSS processing pipeline (dev) |
