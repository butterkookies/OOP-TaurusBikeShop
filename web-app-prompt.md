You are a professional database architect and technical diagramming expert. Your task is to generate a complete, multi-tab Entity Relationship Diagram (ERD) from the SQL schema provided below. Follow every instruction precisely. Do not skip, omit, or approximate any rule.

═══════════════════════════════════════════════════════
SECTION 1 — ERD NOTATION STANDARDS (NON-NEGOTIABLE)
═══════════════════════════════════════════════════════

1.1 CROW'S FOOT NOTATION — Use crow's foot notation exclusively for all cardinality markers. Render the following markers correctly:
  - One and only one  →  single vertical bar (|) on both ends
  - Zero or one       →  circle + single vertical bar (o|)
  - One or many       →  single vertical bar + crow's foot (|<)
  - Zero or many      →  circle + crow's foot (o<)
  - Mandatory         →  double vertical bars (||)
  Place the correct marker at each end of every relationship line based on the actual FK constraint, nullability, and business rule implied by the schema.

1.2 LINES — Use only straight orthogonal (right-angle) lines. Curved lines are strictly prohibited. All connectors must route along horizontal and vertical axes only, with 90-degree bends where needed. No diagonal lines.

1.3 PRIMARY KEYS — Mark every PK column with a 🔑 key icon or a bold "PK" tag in a distinct color (gold or amber). Every table must clearly show its PK.

1.4 FOREIGN KEYS — Mark every FK column with a small chain-link icon or an italic "FK" tag in a muted accent color (e.g., steel blue). FK columns must be visually distinguishable from regular columns.

1.5 NOT NULL vs NULLABLE — Use a filled circle (●) for NOT NULL and an open circle (○) for nullable columns, placed to the left of each column name.

1.6 DATA TYPES — Show the abbreviated data type for every column to the right of the column name (e.g., int, nvarchar(255), decimal(10,2), bit, datetime2).

1.7 COMPUTED COLUMNS — Mark computed columns (e.g., TotalAmount AS ...) with a formula icon (ƒ) or a "CALC" badge in a distinct light color (e.g., mint green).

1.8 UNIQUE CONSTRAINTS — Mark columns or column combinations that have a unique constraint with a "UQ" badge or a diamond icon (◇).

1.9 IDENTITY / AUTO-INCREMENT — Mark identity columns with an "AI" or "IDENTITY" badge in a small gray tag.

═══════════════════════════════════════════════════════
SECTION 2 — TABLE STYLING
═══════════════════════════════════════════════════════

2.1 LAYOUT — Each table is rendered as a clean, bordered rectangle with:
  - A distinct HEADER ROW (colored background) showing the table name in bold, capitalized
  - A table icon appropriate to the domain (e.g., 👤 for User, 🛒 for Cart, 💳 for Payment) placed left of the table name in the header
  - Column rows below the header, each row clearly separated by a thin horizontal rule

2.2 COLOR SCHEME — Apply the following consistent domain color palette for table headers. Do NOT use random colors. Keep it professional and accessible:
  - Identity / Auth (User, Role, UserRole, ActiveSession, OTPVerification): Deep navy blue #1E3A5F
  - Orders / Fulfillment (Order, OrderItem, PickupOrder, OrderStatusAudit): Deep teal #1B5E5A
  - Payments (Payment, GCashPayment, BankTransferPayment, Refund, StorePaymentAccount): Forest green #2D5016
  - Products / Catalog (Product, ProductVariant, ProductImage, Category, Brand, PriceHistory): Burgundy #5C1A1A
  - Inventory / Procurement (InventoryLog, PurchaseOrder, PurchaseOrderItem, Supplier): Dark orange #7A3B00
  - Delivery / Logistics (Delivery, LBCDelivery, LalamoveDelivery): Indigo #2E1A6E
  - Cart / Session (Cart, CartItem, GuestSession, POS_Session): Slate #3A3F5C
  - Customer Engagement (Review, Wishlist, Notification): Olive green #3B4A1A
  - Promotions / Vouchers (Voucher, UserVoucher, VoucherUsage): Plum #4A1A4A
  - Support (SupportTicket, SupportTicketReply, SupportTask): Dark cyan #1A4A4A
  - System / Audit (SystemLog, __EFMigrationsHistory): Charcoal #2A2A2A
  - Header text: Always white #FFFFFF
  - Column row background: Alternating #F9F9F9 and #FFFFFF
  - Column text: #222222
  - Table border: #CCCCCC, 1px

2.3 FONT — Use a clean monospace or sans-serif font (e.g., Inter, Roboto, or Consolas). Font size for column names: 11–12px. Table name in header: 13–14px bold. All text must be legible when printed at 100% zoom.

2.4 NO MISSPELLINGS — All table names, column names, data types, and labels must be spelled exactly as they appear in the schema. Do not abbreviate or rename anything unless adding a display badge (PK, FK, UQ, etc.).

═══════════════════════════════════════════════════════
SECTION 3 — MULTI-TAB LAYOUT STRUCTURE
═══════════════════════════════════════════════════════

Generate the ERD across multiple tabs. Each tab must be a self-contained view that fits within a standard A3 or A4 landscape printed page when exported. Tab separation must be domain-logical, not arbitrary. The required tabs are:

──────────────────────────────────────────────────────
TAB 1 — FULL SCHEMA OVERVIEW (Master Map)
──────────────────────────────────────────────────────
Show ALL 43 tables. Use compact table representation: header + PK + FK columns only (omit non-key columns for space). Show all relationship lines. Use a zoomed-out layout. Add a legend panel in the corner showing: color domain key, notation symbols (PK, FK, UQ, CALC, AI, ●, ○), and crow's foot cardinality symbols. Label this tab: "Overview — All Tables".

──────────────────────────────────────────────────────
TAB 2 — IDENTITY & ACCESS
──────────────────────────────────────────────────────
Tables: User, Role, UserRole, ActiveSession, OTPVerification, Address
Show ALL columns with full detail (data types, nullability, PK/FK badges). Show all relationships between these tables and to external tables (e.g., User → Order, User → Cart shown as stub arrows pointing offscreen with labels). Label this tab: "Identity & Access".

──────────────────────────────────────────────────────
TAB 3 — ORDERS & FULFILLMENT
──────────────────────────────────────────────────────
Tables: Order, OrderItem, PickupOrder, OrderStatusAudit, Delivery, LBCDelivery, LalamoveDelivery
Show ALL columns. Show relationships to User, Product, ProductVariant, Address, Cart, GuestSession, POS_Session as stub arrows with labels. Label this tab: "Orders & Fulfillment".

──────────────────────────────────────────────────────
TAB 4 — PAYMENTS & REFUNDS
──────────────────────────────────────────────────────
Tables: Payment, GCashPayment, BankTransferPayment, Refund, StorePaymentAccount, VoucherUsage
Show ALL columns. Show relationships to Order, User, SupportTicket, Voucher as stubs. Label this tab: "Payments & Refunds".

──────────────────────────────────────────────────────
TAB 5 — PRODUCT CATALOG & INVENTORY
──────────────────────────────────────────────────────
Tables: Product, ProductVariant, ProductImage, Category, Brand, PriceHistory, InventoryLog, PurchaseOrder, PurchaseOrderItem, Supplier
Show ALL columns. Show relationships to User, Order as stubs. Label this tab: "Product Catalog & Inventory".

──────────────────────────────────────────────────────
TAB 6 — CUSTOMER ENGAGEMENT & SUPPORT
──────────────────────────────────────────────────────
Tables: Review, Wishlist, Notification, SupportTicket, SupportTicketReply, SupportTask, Voucher, UserVoucher
Show ALL columns. Show relationships to User, Order, Product, Refund as stubs. Label this tab: "Customer Engagement & Support".

──────────────────────────────────────────────────────
TAB 7 — SESSION, POS & AUDIT
──────────────────────────────────────────────────────
Tables: Cart, CartItem, GuestSession, POS_Session, SystemLog, __EFMigrationsHistory
Show ALL columns. Show relationships to User, Product, ProductVariant, Order as stubs. Label this tab: "Session, POS & Audit".

═══════════════════════════════════════════════════════
SECTION 4 — RELATIONSHIP LINE RULES
═══════════════════════════════════════════════════════

4.1 Every FK relationship in the schema must be drawn as a line connecting the FK column of the child table to the PK column of the parent table.

4.2 Label every relationship line with the constraint name (e.g., FK_Order_User) in small italic text (9–10px), positioned near the midpoint of the line, rotated to follow the line direction.

4.3 Place crow's foot markers at the "many" end and single bar at the "one" end. Use the nullability of the FK column to determine zero-or vs one-or: a nullable FK column = "zero or many / zero or one"; a NOT NULL FK column = "one or many / one and only one".

4.4 For self-referencing tables (Category → Category via ParentCategoryId), draw a loopback arrow on the same table box using a straight L-shaped orthogonal line exiting from the bottom and re-entering at the side.

4.5 Lines must not overlap table boxes. Route lines around tables. Maintain a minimum 10px clearance from table edges.

4.6 In TAB 1 (overview), where cross-domain lines would create extreme clutter, use short stub lines with directional labels (e.g., "→ Product") instead of full crossing lines. Only show intra-domain full lines in the overview.

═══════════════════════════════════════════════════════
SECTION 5 — PRINT & EXPORT QUALITY
═══════════════════════════════════════════════════════

5.1 Each tab must be sized to fit on A3 landscape (420mm × 297mm) or at minimum A4 landscape (297mm × 210mm). Tables and lines must not be cut off at the edge.

5.2 Add a footer on each tab with: diagram name ("Taurus Bike Shop — Database ERD"), the tab name, tab number (e.g., "Tab 3 of 7"), schema version ("v10.0"), and date.

5.3 Add a title header banner at the top of each tab with the tab name in large bold text and a brief one-sentence description of what domain it covers.

5.4 Minimum resolution for export: 150 DPI. All text must remain legible at this resolution.

5.5 Background of the entire diagram: White #FFFFFF. No dark backgrounds except table headers.

═══════════════════════════════════════════════════════
SECTION 6 — LEGEND PANEL (REQUIRED ON EVERY TAB)
═══════════════════════════════════════════════════════

Place a compact legend box in the bottom-right corner of every tab containing:
- 🔑 PK — Primary Key
- FK — Foreign Key
- UQ — Unique Constraint
- AI — Identity / Auto-Increment
- ƒ — Computed Column
- ● — NOT NULL
- ○ — Nullable
- Crow's foot symbol key: one-to-one, one-to-many, zero-or-one, zero-or-many
- Domain color swatches with labels

═══════════════════════════════════════════════════════
SECTION 7 — STRICT PROHIBITIONS
═══════════════════════════════════════════════════════

❌ No curved lines of any kind (bezier, arc, spline)
❌ No diagonal connector lines
❌ No missing tables — all 43 tables must appear in TAB 1
❌ No misspelled table names or column names
❌ No omitted PK or FK markers
❌ No unlabeled relationship lines
❌ No tables overlapping each other
❌ No text overflowing outside table boundaries
❌ No random or inconsistent color choices outside the defined palette
❌ No missing crow's foot notation on any relationship

═══════════════════════════════════════════════════════
SECTION 8 — SCHEMA INPUT
═══════════════════════════════════════════════════════

Use schema in SQL\Schema\Taurus_schema_10.0.sql

Generate the ERD now following all rules above. Produce the output as an interactive HTML file with tabbed navigation, or as a structured diagram in the tool you are using (e.g., draw.io XML, Mermaid ERD, dbdiagram.io DSL, or Figma-ready SVG). If generating code, ensure it is complete, valid, and self-contained with no external dependencies beyond a CDN link.