Act as a Senior Software Architect and Technical Communicator.

Generate a complete, accurate architectural and operational blueprint for the
Taurus Bike Shop project — a dual-application system consisting of:

  · WebApplication  — customer-facing e-commerce (ASP.NET Core MVC, .NET 8)
  · AdminSystem_v2  — POS and admin desktop (WPF MVVM, .NET 8)

Both share a single SQL Server database hosted on Google Cloud SQL and
communicate exclusively through that shared database (no direct API between them).

═══════════════════════════════════════════════════════
TONE & AUDIENCE INSTRUCTIONS
═══════════════════════════════════════════════════════

This blueprint will be read by:
  (a) developers who need to onboard quickly
  (b) non-technical stakeholders (bike shop owner, professors)

Rules:
  · Lead every technical concept with a one-sentence plain-English explanation
    before the technical details. Use concrete real-world analogies where helpful
    (e.g., "A Repository is like a librarian — you ask for data, it fetches it
    from the shelf so you never have to go to the database directly").
  · Never use jargon without immediately defining it.
  · Keep analogies short — one sentence maximum. Do not over-explain them.
  · For code references, name the exact file path and class/method. Never say
    "some controller" or "a service somewhere" — be specific.

═══════════════════════════════════════════════════════
GROUND TRUTH — USE THIS, DO NOT CONTRADICT IT
═══════════════════════════════════════════════════════

Before generating the blueprint, anchor your output to these verified facts:

  Database:       SQL Server on Google Cloud SQL (35.221.161.150:1433)
  Schema file:    SQL\Schema\Taurus_schema_10.0.sql  (43 tables, 11 domains)
  ORM (WebApp):   Entity Framework Core 8, Fluent API only, Generic IRepository<T>
  ORM (Admin):    Dapper 2.1 (raw parameterized SQL)
  Auth:           Cookie-based sessions, 30-min idle timeout, BCrypt work factor 12
  File Storage:   Google Cloud Storage (bucket: taurus-bikeshop-assets)
  Frontend:       Vanilla JS · Bootstrap 5 (CDN) · jQuery 3.x (CDN)
  Jobs:           5 IHostedService background jobs (see Pillar 5)
  Design:         Glassmorphism — deep gradient bg, backdrop-filter cards, crimson #DC143C
  Fonts:          Bebas Neue (display) · DM Sans (body) · Oswald (prices/stats)
  Couriers:       Lalamove and LBC ONLY (both currently stubbed — no live API)
  Payment:        GCash and BankTransfer in WebApp; Cash is POS-only in AdminSystem

  Entity constants (use these exact values, never magic strings):
    OrderStatuses:   Pending · PendingVerification · OnHold · Processing ·
                     ReadyForPickup · PickedUp · Shipped · Delivered · Cancelled
    PaymentStatuses: Pending · VerificationPending · VerificationRejected ·
                     Completed · Failed
    DeliveryStatuses: Pending · PickedUp · InTransit · Delivered · Failed

  Layered architecture:
    WebApp:  Controller → Service → Repository → AppDbContext → SQL Server
    Admin:   View (XAML) → ViewModel → Service → Repository (Dapper) → SQL Server

  Key files:
    WebApplication/DataAccess/Context/AppDbContext.cs     1,341 lines, 38 DbSets
    WebApplication/Program.cs                            500+ lines, DI + middleware
    WebApplication/Models/ApiResponse.cs                 AJAX envelope (Ok/Fail)
    WebApplication/wwwroot/js/utils.js                   fetchWithCSRF(), showAlert()
    .claude/CONTEXT.md                                   Full project context
    .claude/BRAND.md                                     Complete design system v2.0

═══════════════════════════════════════════════════════
OUTPUT STRUCTURE — 13 PILLARS (REQUIRED, IN ORDER)
═══════════════════════════════════════════════════════

Generate each pillar as a clearly labeled section. Within each pillar follow this
format: plain-English summary first → technical details second → file references third.

──────────────────────────────────────────────────────
PILLAR 1 — ARCHITECTURE OVERVIEW (The City Map)
──────────────────────────────────────────────────────

  1a. State the high-level purpose of each application in one paragraph each.
  1b. Explain how the two systems communicate (shared database, no direct API).
      Name the exact synchronization pattern (indirect via DB + background jobs).
  1c. Produce a text-based ASCII architecture diagram showing:
      - Client layer (Browser + WPF Desktop)
      - Application layer (MVC + MVVM stacks with their sub-layers)
      - Data layer (SQL Server)
      - External services (Google Cloud Storage, Gmail SMTP, Lalamove/LBC stubs)
  1d. Explain why the two systems share one database instead of having their own.
      Name the tradeoff: consistency vs. coupling.

──────────────────────────────────────────────────────
PILLAR 2 — DATABASE & SCHEMA (The Central Filing Room)
──────────────────────────────────────────────────────

  2a. List all 11 table domains (Identity, Orders, Payments, Products, Inventory,
      Delivery, Cart/Session, Engagement, Promotions, Support, System/Audit).
      For each domain: list its tables and state what business data they own.
  2b. Explain the 5 shared-PK 1:1 subtypes (GCashPayment, BankTransferPayment,
      LalamoveDelivery, LBCDelivery, PickupOrder). State what ValueGeneratedNever()
      means in plain English.
  2c. Explain the circular FK between User and Address (DefaultAddressId) and why
      OnDelete(NoAction) is used to break it.
  2d. State the two most important cascade rules in the schema and their business impact.
  2e. Confirm that the schema is read-only — no ALTER, DROP, or CREATE TABLE is
      ever performed from application code.

──────────────────────────────────────────────────────
PILLAR 3 — DATA I/O & HANDLERS (The Gatekeepers)
──────────────────────────────────────────────────────

  3a. Inputs: List the 5 most data-intensive user-facing forms (e.g., checkout,
      registration, support ticket) and name the controller action that receives each.
  3b. Read path: Trace a product catalog page load end-to-end:
      browser → Controller → Service → Repository → EF Core → SQL Server → ViewModel → View
  3c. Write path: Trace an order creation end-to-end (same layer-by-layer breakdown).
  3d. Validation: Explain where validation happens — model binding, service layer,
      and database constraints. Name the utility classes involved (ValidationHelper, etc.).
  3e. AJAX: Explain the ApiResponse envelope pattern (ApiResponse.Ok / ApiResponse.Fail)
      and the fetchWithCSRF() wrapper. Why does every AJAX call need the CSRF token?

──────────────────────────────────────────────────────
PILLAR 4 — THIRD-PARTY INTEGRATIONS (The Outside Hires)
──────────────────────────────────────────────────────

  4a. For each external service, state: purpose · how it is called · what it returns.

      Google Cloud SQL      — Database hosting
      Google Cloud Storage  — Product images, payment proofs, support attachments
                              FileUploadHelper.cs handles uploads
      Gmail SMTP (MailKit)  — Transactional email (OTP, order confirmations)
      Lalamove API          — Delivery booking/tracking (currently stubbed)
      LBC API               — Delivery booking/tracking (currently stubbed)

  4b. For the two courier APIs, explain what "stubbed" means in plain English and
      what the system does in the meantime (manual tracking, no auto-updates).
  4c. Explain how Google Cloud Storage credentials are stored and accessed
      (appsettings.json / environment variables — never hardcoded).

──────────────────────────────────────────────────────
PILLAR 5 — BACKGROUND JOBS (The Invisible Night Shift)
──────────────────────────────────────────────────────

  Document each of the 5 hosted services with:
    · Class name · Interval · What it reads · What it writes · Business impact

  InventorySyncJob         Every 6 hours
  PendingOrderMonitorJob   Every 30 minutes
  PaymentTimeoutJob        Every 15 minutes
  StockMonitorJob          Every 60 minutes
  DeliveryStatusPollJob    Every 5 minutes (currently has a LINQ bug — document it)

  5a. For DeliveryStatusPollJob: name the exact bug (HashSet<T>.Contains() in EF Core
      .Where() fails translation to SQL) and the correct fix (explicit || conditions).
  5b. Explain what IHostedService means in plain English and where all 5 are registered
      (Program.cs DI container).
  5c. Explain why jobs log to SystemLog on startup and why this fails in development
      when the dev IP is not whitelisted in Google Cloud Console.

──────────────────────────────────────────────────────
PILLAR 6 — DESIGN SYSTEM & UX STANDARDS (The Brand Wardrobe)
──────────────────────────────────────────────────────

  6a. Brand identity: NBA Chicago Bulls energy translated to UI. Bold, sporty, premium.
      Mood reference: Rapha × Ducati × Apple Vision Pro.

  6b. Color system (reference .claude/BRAND.md):
      · Background: deep blue-black gradient (NOT flat black — explain why the gradient
        matters for glassmorphism depth)
      · Primary accent: Crimson #DC143C — ONE element per section max
      · Glass surfaces: rgba(255,255,255,0.06) + backdrop-filter: blur(16px)
      · Text: #F0F0F0 (primary), rgba(240,240,240,0.65) (secondary)

  6c. Typography stack:
      · Bebas Neue / Barlow Condensed — display, headings, hero (condensed, athletic)
      · DM Sans — body text, UI labels
      · Oswald — prices, stats, performance numbers

  6d. Glass elevation tiers (Tier 0–4). Explain what each tier is used for and
      why skipping tiers breaks visual hierarchy.

  6e. WebApp frontend stack: Vanilla JS · Bootstrap 5 · jQuery 3 · Google Fonts CDN.
      State that there is no frontend build step (no Webpack, Vite, or npm for the app).

  6f. AdminSystem design: WPF XAML with OxyPlot for charts. How is visual consistency
      maintained between the desktop and the web app given they use different stacks?

──────────────────────────────────────────────────────
PILLAR 7 — WEB APP FRONTEND FLOW (The Customer Storefront)
──────────────────────────────────────────────────────

  Map the complete customer journey. For each step name the exact controller,
  action method, view file, and any significant service or repository involved.

  Step 1  — Browse catalog        (ProductController, catalog view, filtering + pagination)
  Step 2  — View product detail   (ProductController, detail view, variant selection)
  Step 3  — Add to cart           (CartController AJAX, CartService, CartRepository)
  Step 4  — View cart             (CartController, cart view, guest vs auth session)
  Step 5  — Apply voucher         (CheckoutController AJAX, VoucherService)
  Step 6  — Checkout              (CheckoutController, address selection, delivery method)
  Step 7  — Select payment method (GCash vs BankTransfer — never Cash in WebApp)
  Step 8  — Upload payment proof  (PaymentController, FileUploadHelper → GCS)
  Step 9  — Payment verification  (PaymentTimeoutJob + admin action in AdminSystem)
  Step 10 — Order confirmation    (OrderController, order history view)

  7a. Guest vs authenticated cart: explain how GuestSession works and how the
      cart is merged on login.
  7b. Explain how anti-forgery tokens work in plain English and why every POST
      action requires them.
  7c. Explain fetchWithCSRF() — what it does and why it exists (CSRF protection on AJAX).
  7d. State that all AJAX responses use ApiResponse { success, message, data } and
      explain how the frontend reads this to show alerts or update the UI.

──────────────────────────────────────────────────────
PILLAR 8 — ADMIN SYSTEM FLOW (The Employee Back-Office)
──────────────────────────────────────────────────────

  Map the real admin workflow. For each workflow name the XAML view, the ViewModel,
  and the Dapper service or repository.

  Workflow 1  — Login / authentication   (LoginView, LoginViewModel, AuthService)
  Workflow 2  — Dashboard / KPIs         (DashboardView, DashboardViewModel, OxyPlot charts)
  Workflow 3  — Order management         (OrdersView — status transitions, action buttons)
  Workflow 4  — Payment verification     (approve GCash/BankTransfer proofs)
  Workflow 5  — POS transaction          (POS_Session, cash payment, walk-in orders)
  Workflow 6  — Inventory management     (InventoryLog append-only, stock adjustments)
  Workflow 7  — Product management       (Products, Variants, Categories, Brands)
  Workflow 8  — Supplier / Purchase Orders

  8a. Explain the valid order status transition graph. Which transitions are allowed
      and which are forbidden? (e.g., Pending → PendingVerification → Processing →
      ReadyForPickup → PickedUp / Shipped → Delivered)
  8b. Explain why InventoryLog is append-only (audit trail, immutability principle).
  8c. Explain the MVVM binding: how does a property change in a ViewModel automatically
      update the XAML view? (INotifyPropertyChanged, data binding in plain English.)
  8d. Dapper vs EF Core: explain the tradeoff. Why does AdminSystem use Dapper
      (performance, direct SQL control) instead of EF Core?

──────────────────────────────────────────────────────
PILLAR 9 — CRITICAL CORE SCRIPTS (The VIP Engines)
──────────────────────────────────────────────────────

  Identify and deeply explain these 5 files. For each:
    · File path
    · Purpose in one sentence
    · What the most critical method does (trace the logic)
    · Real-world analogy for what it accomplishes for the business

  Required files to cover:
  1. WebApplication/DataAccess/Context/AppDbContext.cs
     (38 DbSets, all Fluent API config, cascade rules, shared-PK subtypes)
  2. WebApplication/Program.cs
     (DI registration, middleware pipeline, EF Core retry policy, 5 job registrations)
  3. WebApplication/Models/ApiResponse.cs
     (Standard JSON envelope — the contract between server and all JS code)
  4. WebApplication/wwwroot/js/utils.js
     (fetchWithCSRF, showAlert, formatCurrency — the shared frontend toolkit)
  5. One background job of your choice that best illustrates the async job pattern

──────────────────────────────────────────────────────
PILLAR 10 — SECURITY & AUTHENTICATION (The Locks & Keys)
──────────────────────────────────────────────────────

  10a. Auth flow: explain cookie-based session auth step by step
       (login → BCrypt verify → session created → 30-min idle expiry → logout).
  10b. Password hashing: BCrypt.Net-Next, work factor 12. Explain what work factor
       means and why 12 is the chosen value.
  10c. Anti-forgery: [ValidateAntiForgeryToken] global filter + fetchWithCSRF() on JS.
       Explain what CSRF attacks are in plain English and how this prevents them.
  10d. Secrets management: connection strings in appsettings.json /
       appsettings.Development.json, production secrets via environment variables.
       Reference SECRETS.md for the secrets inventory.
  10e. HTTPS: SecurePolicy=None in development (expected), must be Always in production.
       Flag this as a deployment checklist item.
  10f. Role-based access: RoleNames (Admin, Manager, Staff, Customer).
       Which routes/actions require which roles?

──────────────────────────────────────────────────────
PILLAR 11 — ERROR HANDLING & LOGGING (The Safety Nets)
──────────────────────────────────────────────────────

  11a. Global error handling: what happens when an unhandled exception occurs in the
       ASP.NET Core pipeline? (Developer exception page vs production error view)
  11b. EF Core retry policy: the connection retry configuration in Program.cs.
       Why is this needed for Google Cloud SQL? What transient errors does it handle?
  11c. SystemLog table: what events are logged, which service writes to it, and why
       this fails at startup in development (IP whitelist issue).
  11d. Background job error handling: what happens if InventorySyncJob or
       PaymentTimeoutJob throws an exception mid-run? Does it retry?
  11e. Payment consistency: if a payment upload to GCS succeeds but the database
       write fails, what is the failure mode? Is there a rollback? (Be honest —
       if there is a gap, state it clearly as a known limitation.)

──────────────────────────────────────────────────────
PILLAR 12 — TESTING & QA WORKFLOWS (The Health Inspectors)
──────────────────────────────────────────────────────

  12a. Automated tests: state honestly whether unit or integration tests exist.
       If they do not, state this clearly and recommend what should be added first.
  12b. Manual QA: describe the current QA approach (build checks, browser testing,
       Postman for API endpoints — .postman/ config is included in the repo).
  12c. Build verification: dotnet build with 0 warnings, 0 errors as the baseline.
  12d. LINQ safety: explain the rule about verifying that LINQ queries translate to
       valid SQL (no client-side evaluation). Reference the DeliveryStatusPollJob bug
       as a concrete example of what happens when this rule is violated.
  12e. Recommended next steps: what 3 tests would provide the most coverage value
       given the current state of the codebase?

──────────────────────────────────────────────────────
PILLAR 13 — BUILD & INFRASTRUCTURE (Turning on the Lights)
──────────────────────────────────────────────────────

  13a. WebApplication — exact commands to build and run:
       cd WebApplication
       dotnet build
       dotnet run
       Expected: Listening on http://localhost:5000 (or configured port)

  13b. AdminSystem_v2 — exact steps:
       Open AdminSystem_v2.sln in Visual Studio 2022
       Set AdminSystem_v2 as startup project
       Build → Run (F5)
       Requires: Windows (WPF is Windows-only), SQL Server accessible

  13c. Prerequisites checklist:
       .NET 8 SDK  ·  Visual Studio 2022  ·  SQL Server Express
       SSMS (schema management)  ·  Google Cloud credentials (appsettings)
       Postman (API testing — .postman/ config in repo)

  13d. Configuration: explain the two appsettings files
       (appsettings.json for shared config, appsettings.Development.json for dev overrides).
       What values must be set before first run?

  13e. Known startup issue: SystemLog write fails at boot if developer IP is not
       whitelisted in Google Cloud Console firewall rules. This is expected and safe
       to ignore in development — no code change needed.

  13f. No CI/CD pipeline exists. No EF Core migrations are committed — the schema
       is managed externally via the SQL scripts in SQL\Schema\.

═══════════════════════════════════════════════════════
STRICT PROHIBITIONS
═══════════════════════════════════════════════════════

❌ Do NOT generalize — every technical claim must name a specific file, class, or method.
❌ Do NOT invent integrations or features not listed in the Ground Truth section above.
❌ Do NOT say "Cash" is a WebApp payment method — it is POS/AdminSystem only.
❌ Do NOT imply the courier APIs (Lalamove/LBC) are live — they are stubbed.
❌ Do NOT say EF Core data annotations are used for relationships — Fluent API only.
❌ Do NOT omit known bugs and limitations — include BUG-1 (DeliveryStatusPollJob LINQ)
   and BUG-2 (_Layout.cshtml partial path) in the relevant pillars.
❌ Do NOT produce a generic software architecture document — every statement must
   be traceable to this specific codebase.

═══════════════════════════════════════════════════════
OUTPUT FORMAT
═══════════════════════════════════════════════════════

  · Use the exact 13-pillar headings above.
  · Use section headers, sub-headers, and bullet points for scanability.
  · Code references: always include the file path (e.g., WebApplication/Program.cs:L42).
  · Analogies: italicize them and keep them to one sentence.
  · Length: comprehensive but not redundant. One clear paragraph per sub-point is enough.
  · Do not add a preamble or conclusion outside the 13 pillars.
  · Generate all 13 pillars in a single response. Do not stop mid-document.
