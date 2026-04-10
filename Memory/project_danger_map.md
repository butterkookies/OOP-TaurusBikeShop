# Danger Map

> High-risk files and areas. Updated as new risks are discovered.
> If you touch any file listed here, stop and re-read its entry first.

## Inferred from codebase scan

### WebApplication/Program.cs
- **Why dangerous**: Central DI container and middleware pipeline. All service registrations, auth config, session config, GCS client setup, and route mapping live here. A bad change breaks the entire web app.
- **Safe approach**: Read the full file before editing. Test that the app starts after changes. Verify middleware order (UseAuthentication before UseAuthorization before UseSession).
- **Known dependents**: Every controller, service, repository, and background job depends on registrations in this file.

### WebApplication/DataAccess/Context/AppDbContext*.cs (8 partial files)
- **Why dangerous**: 38+ DbSets and all Fluent API relationship configuration. Incorrect changes can cascade into data loss, broken migrations, or silent data corruption. Partial classes split across Auth, Catalog, Commerce, Delivery, Orders, Payments, SupplyChain, SupportComms.
- **Safe approach**: Read all 8 partial files before modifying any. Ensure decimal properties have `HasPrecision(18, 2)`. Never add data annotations on entities — use Fluent API only. Respect the 5 shared-PK entities with `ValueGeneratedNever()`.
- **Known dependents**: All repositories and services depend on the DbContext. EF Core migrations are generated from this.

### WebApplication/Utilities/PasswordHelper.cs
- **Why dangerous**: BCrypt password hashing for all user authentication. Work factor 12. Breaking this locks out all users.
- **Safe approach**: Never change the work factor without a migration plan for existing hashes. Never log or serialize hashes.
- **Known dependents**: UserService (login + registration)

### AdminSystem_v2/Helpers/DatabaseHelper.cs
- **Why dangerous**: Singleton connection string loaded once at startup. Shared by all repositories via `GetConnection()`. Supports User Secrets and environment variable overrides. Breaking this breaks the entire admin app.
- **Safe approach**: Read the full file before editing. Test `TestConnection()` after changes.
- **Known dependents**: Every AdminSystem_v2 repository calls `DatabaseHelper.GetConnection()`.

### WebApplication/Controllers/PaymentController.cs + BusinessLogic/Services/PaymentService.cs
- **Why dangerous**: Handles real payment flows (GCash, BankTransfer). Business rules: no COD, no card, no refunds. PaymentStatus = 'Refunded' must never exist.
- **Safe approach**: Re-read `project-context.md` business rules before touching. Never add Cash/COD/Card payment methods to WebApp. Never add refund logic.
- **Known dependents**: Checkout flow, order flow, payment proof uploads.

### WebApplication/BackgroundJobs/*.cs (5 files)
- **Why dangerous**: Long-running hosted services that modify database state (inventory sync, order monitoring, payment timeouts, stock alerts, delivery status). A bug here can silently corrupt production data across many rows.
- **Safe approach**: Ensure all queries are EF-translatable (no HashSet.Contains patterns). Test with DB available and unavailable. Jobs must be idempotent.
- **Known dependents**: InventoryLog (append-only), Order statuses, Payment statuses, SystemLog.

### SQL/Schema/Taurus_schema_8.0.sql
- **Why dangerous**: Full database DDL exported from SSMS. Running this recreates the entire database. The file is UTF-16 encoded.
- **Safe approach**: Never run this against a production database without a backup. Use only as reference.
- **Known dependents**: Both applications depend on this schema.

### appsettings.json (both projects)
- **Why dangerous**: Contains placeholders for secrets. If real credentials are accidentally committed, they must be rotated immediately.
- **Safe approach**: Only use `SET_VIA_USER_SECRETS_OR_ENVIRONMENT_VARIABLE` placeholders. See SECRETS.md for credential management.
- **Known dependents**: DatabaseHelper (AdminSystem_v2), Program.cs (WebApplication).

### WebApplication/Models/Entities/ (38+ entity files)
- **Why dangerous**: Entity changes cascade to AppDbContext Fluent API, repositories, services, view models, and views. Key constraints: InventoryLog is append-only, Address snapshots (IsSnapshot=true) are immutable, stock lives only in ProductVariant.StockQuantity, computed fields (TotalAmount, Subtotal) must never be stored.
- **Safe approach**: When modifying an entity, trace all usages through repository → service → controller → view. Respect all 10 business rules in project-context.md.
- **Known dependents**: AppDbContext, all repositories, all services, all views.
