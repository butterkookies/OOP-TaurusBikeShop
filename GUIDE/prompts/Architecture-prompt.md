## **Task**

Analyze the provided system architecture for **Taurus Bike Shop**, a dual-application system consisting of:

* **WebApplication (ASP.NET Core MVC, .NET 8)** — customer-facing e-commerce
* **AdminSystem_v2 (WPF MVVM, .NET 8)** — POS and admin desktop system

Both systems:

* Share a **single SQL Server database (Google Cloud SQL)**
* Use **different data access strategies**:

  * WebApp → Entity Framework Core (Repository + Service Layer)
  * AdminSystem → Dapper (raw SQL)

---

## **1. High-Level Architecture Diagram**

Generate a **Mermaid `graph TD` diagram** that accurately reflects:

### Required Components:

* **Client Layer**

  * Browser (WebApplication)
  * WPF Desktop App (AdminSystem)

* **Application Layer**

  * ASP.NET Core MVC (Controllers → Services → Repositories → EF Core)
  * WPF MVVM (Views → ViewModels → Services → Repositories → Dapper)

* **Data Layer**

  * Shared SQL Server (Google Cloud SQL)

* **External Services**

  * Google Cloud Storage (file uploads)
  * Cloudinary (image CDN)
  * Gmail SMTP (MailKit)

### Must Highlight:

* Shared database access
* Separation of architectures (MVC vs MVVM)
* Background services (WebApp hosted services)
* No direct API between systems (if applicable — or label assumption)

---

## **2. Technical Breakdown**

### a. Frameworks & Technologies

Explicitly identify (DO NOT generalize):

* ASP.NET Core MVC (.NET 8)
* WPF (.NET 8 Windows)
* EF Core 8 vs Dapper 2.1
* Tailwind CSS + Vanilla JS
* OxyPlot (Admin charts)

---

### b. Entry Points

Identify realistic entry points based on .NET conventions:

* **WebApplication**

  * `Program.cs` (minimal hosting model)
  * Middleware pipeline (routing, auth, sessions)

* **AdminSystem_v2**

  * `App.xaml` / `App.xaml.cs`
  * `MainWindow.xaml`

---

### c. Core Modules (Must Be Concrete)**

#### WebApplication:

* Controllers (Cart, Checkout, Order, etc.)
* Services (business logic layer)
* Repositories (generic + specific)
* Background Jobs (InventorySyncJob, PaymentTimeoutJob, etc.)

#### AdminSystem:

* Views (POS, Orders, Dashboard)
* ViewModels (state + commands)
* Services (POS logic, reporting)
* Repositories (Dapper queries)

---

## **3. Data Flow Diagram (End-to-End)**

Generate a **Mermaid `sequenceDiagram`** showing:

### Flow:

1. Customer interacts with Web UI
2. Controller receives request
3. Service layer processes logic
4. Repository interacts with EF Core
5. Database operation (SQL Server)
6. Response returned to UI

### Include:

* Authentication (cookie-based)
* CSRF validation
* Background job triggers (if relevant)

---

## **4. Integration Flow (CRITICAL)**

Generate a **detailed Mermaid diagram** showing how:

### WebApplication ↔ AdminSystem_v2 interact via the shared database

Include:

* **Order Flow**

  * WebApp creates order → stored in DB
  * AdminSystem reads order → processes via POS

* **Inventory Flow**

  * Admin updates stock → writes to DB
  * WebApp reflects updated inventory

* **Synchronization Pattern**

  * Indirect communication via DB (NO direct API unless assumed)
  * Background jobs (InventorySyncJob, StockMonitorJob)

* **Concurrency Considerations (IMPORTANT)**

  * EF Core vs Dapper access
  * Potential race conditions or stale reads (label as assumptions)

---

## **5. Output Requirements**

* Use **Mermaid syntax ONLY**
* Include:

  * `graph TD` → Architecture
  * `sequenceDiagram` → Data flow
  * `graph TD` or `sequenceDiagram` → Integration
* Clearly label:

  * Layers
  * Technologies (EF Core, Dapper, etc.)
  * System boundaries

---

## **6. Constraints**

* DO NOT give generic architecture
* MUST reflect:

  * Dual architecture (MVC vs MVVM)
  * Shared database design
  * EF Core vs Dapper difference
* If assumptions are made:

  * Label them clearly as: `<<Assumption>>`
* Keep diagrams clean and readable

---

