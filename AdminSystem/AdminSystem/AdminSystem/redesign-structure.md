# AdminSystem — Redesign Structure
> Design blueprint per page. Layout zones, visible elements, component hierarchy.
> Theme: Dark. Brand color: Red `#CC0000`. Font: Segoe UI. Tokens: `AppStyles.xaml`.

---

## UX Audit — Standards Review

Before the blueprints, an honest pass over each page against common admin UI standards.

| Page | What's Good | UX Problems to Fix |
|------|-------------|-------------------|
| **Dashboard** | Stat cards + data-dense bottom row | Quick Actions sits between stats and data — visually interrupts the scan flow. Move to top-right or merge with header. |
| **Orders** | Split-panel (list + action) is standard | 10 filter buttons in a WrapPanel wrap onto 2–3 rows on narrow windows. Replace with a horizontal scrollable tab-strip or a grouped pill row with a "More" overflow. |
| **POS** | Classic POS split (browser left, cart right) is correct | Product tiles have no visual feedback on hover/click. No "empty cart" state shown. Cart shows no product image at all — hard to confirm the right item was added. |
| **Products** | Familiar list + right-panel pattern | Right panel is only 320px wide — cramped for a product form. **No image field at all.** Creating a new product and editing an existing one use the same cramped panel with no way to add images. Fix: dedicated full-page Create/Edit Product view. |
| **Inventory** | Movements log + low-stock alert side-by-side is useful | Variant ID is a free-text input — admin has to know the ID. Replace with a searchable dropdown of variants. |
| **Delivery** | Minimal, clear | Action panel shows no order info for the selected delivery — admin can't confirm they're acting on the right shipment. Show customer name + order # at top. |
| **Reports** | Date-range filter + stats + 3 tables is correct | "Top Orders by Value" table doesn't actually show the order value — it shows Order#, Customer, Date. Add the total amount column. |
| **Vouchers** | Create/edit panel is complete | No indication of how many times a voucher has been used vs max — useful to show in the grid. |
| **Users** | Detail panel is read-only, appropriately limited | No search/filter on the users list — hard to find a specific user in a large table. Add a search TextBox above the grid. |
| **Support** | Most complete page — replies + tasks + status filter | Replies list has a 160px MaxHeight — scrolls very early. Increase to at least 260px. |

### Global Standards Issues
1. **No consistent page header pattern** — some pages have `[Title] [Button]` in a grid row, others just a TextBlock. Every page should follow: `[Title + Subtitle] ──── [Actions]`.
2. **No breadcrumb / back navigation** — once on a sub-view there is no clear path back. Especially important for the new Create Product page.
3. **Primary action always red (`#CC0000`)** — fine as a brand choice, but destructive actions (Cancel Order, Deactivate) also use red. Destructive should be outlined-red or a separate `DangerButton` style; primary CTA should be solid-red.
4. **Refresh is manual everywhere** — no auto-refresh. At minimum, refresh on tab focus change.
5. **No empty-state messages** — when a DataGrid has zero rows, it shows a blank white/dark area. Every grid needs an empty-state message (`"No orders found"`, etc.).

---

## App Shell — `MainWindow`

```
┌──────────────────────────────────────────────────────────────────────┐
│  [≡]  Taurus Bike Shop — Admin                    [─]  [□]  [✕]     │
├─────────┬────────────────────────────────────────────────────────────┤
│  LOGO   │                                                            │
│ ─────── │               CONTENT FRAME                               │
│  Dash.  │        (active UserControl loads here)                     │
│  Orders │                                                            │
│  POS    │                                                            │
│  Prod.  │                                                            │
│  Invent │                                                            │
│  Deliv. │                                                            │
│  Report │                                                            │
│  Vouch. │                                                            │
│  Users  │                                                            │
│  Support│                                                            │
│         │                                                            │
│ ─────── │                                                            │
│ [avatar]│                                                            │
│  Admin  │                                                            │
│ [Logout]│                                                            │
└─────────┴────────────────────────────────────────────────────────────┘
```

### Sidebar rules
- Width: 220px fixed, collapsible to 56px (icon-only) on demand
- Each nav item: `[MDL2 icon]  [Label]` — 40px tall
- **Active state**: red `4px` left accent bar + `SurfaceBrush` row background
- **Hover state**: `HoverBrush` background
- Bottom dock: avatar circle (initials), admin username, logout icon-button

---

## 1. Login

```
┌──────────────────────────────────────────────────────┐
│                                                      │
│              [🚲 TAURUS BIKE SHOP]                   │
│              Admin Portal                            │
│                                                      │
│  ┌────────────────────────────────────────────────┐  │
│  │                                                │  │
│  │  Email                                         │  │
│  │  [_________________________________________]   │  │
│  │                                                │  │
│  │  Password                              [👁]    │  │
│  │  [_________________________________________]   │  │
│  │                                                │  │
│  │  [error text — hidden until failed login]      │  │
│  │                                                │  │
│  │  [            Sign In            ]             │  │
│  │                                                │  │
│  └────────────────────────────────────────────────┘  │
│                                                      │
└──────────────────────────────────────────────────────┘
```

**Visible elements:**
- Full-window dark background
- Centered card (`SurfaceBrush`, `RadiusLG`, shadow)
- Brand logo + "Admin Portal" subtitle
- Email TextBox (with label above — not placeholder-only)
- Password PasswordBox + show/hide toggle icon
- Inline error message (ErrorBrush, hidden by default)
- Sign In button (PrimaryButton, full-width inside card)

---

## 2. Dashboard

```
┌─────────────────────────────────────────────────────────────────┐
│  Dashboard                                                       │
│  Wednesday, April 1, 2026                   [↻ Refresh]         │
├───────────┬───────────┬───────────┬──────────────────────────────┤
│ ACTIVE    │ PENDING   │ LOW STOCK │ TODAY'S SALES                │
│ ORDERS    │ PAYMENTS  │ ITEMS     │ ₱ 0.00                       │
│ [count]   │ [count]   │ [count]   │ Delivered orders today       │
├───────────┴───────────┴───────────┴──────────────────────────────┤
│ QUICK ACTIONS  [🛒 Walk-in Order (POS)]   [📦 View Inventory]    │
├──────────────────────────┬────────────────┬───────────────────────┤
│  Recent Orders           │ Pending Pmts   │ Low Stock Alert       │
│  ─────────────────────── │ ─────────────  │ ─────────────────     │
│  Order#  Customer  Date  │ # │Method│Amt  │ • Variant   [qty]     │
│  ──────  ────────  ────  │   │      │     │ • Variant   [qty]     │
│  (up to 8 rows)          │ (up to 6 rows) │ (scrollable)          │
│                          │               │                       │
│  [empty: "No orders yet"]│               │ [empty: "All stocked"] │
└──────────────────────────┴────────────────┴───────────────────────┤
│  [⚠ error banner — hidden by default, red strip at bottom]       │
└─────────────────────────────────────────────────────────────────┘
```

**UX notes for this page:**
- Stat cards are the first thing eyes land on — correct
- Quick Actions bar is a secondary row, not a card — keeps it lean
- All three bottom panels are equal height — use `*` row so they fill the space
- Refresh button top-right — standard position
- Error banner anchored at the very bottom — doesn't disrupt content

---

## 3. Orders

```
┌──────────────────────────────────────────────────────┬────────────┐
│  Orders                                              │            │
│  ─────────────────────────────────────────────────── │  ORDER     │
│  [All]·[Pending]·[Pend.Verif]·[On Hold]·[Processing] │  ACTIONS   │
│  [Ready]·[Picked Up]·[Shipped]·[Delivered]·[Cancelled]│            │
│                                   [↻ Refresh]        │  ────────  │
│  [error bar — hidden]                                │  No order  │
│                                                      │  selected  │
│  ┌────────────┬──────────┬────────┬────────┬───────┐ │            │
│  │ Order #    │ Customer │ Date   │ Deliv. │Status │ │  ────────  │
│  │            │          │        │        │       │ │  [Order #] │
│  │            │          │        │        │       │ │  Customer  │
│  │            │          │        │        │       │ │  [badge]   │
│  │ (empty msg if no rows)│        │        │       │ │  ────────  │
│  └────────────┴──────────┴────────┴────────┴───────┘ │  [Approve] │
│                                                      │  [Cancel]  │
│                                                      │  ────────  │
│                                                      │  Set Status│
│                                                      │  [combo▼]  │
│                                                      │  [Apply]   │
│                                                      │  [feedback]│
└──────────────────────────────────────────────────────┴────────────┘
```

**UX notes:**
- Filter strip uses pill buttons (rounded) — active pill = solid red, inactive = outlined
- Filter strip scrolls horizontally if it doesn't fit (no wrapping to second line)
- Orders grid takes the full remaining height (`*` row)
- Right panel is 280px fixed — shows "Select an order" hint when nothing is selected
- **Approve** = solid red (primary action), **Cancel** = outlined red (destructive, distinct)

---

## 4. POS (Point of Sale)

```
┌───────────────────────────────────────┬─────────────────────────┐
│  PRODUCT BROWSER                      │  Cart              [Clear]
│  ┌─────────────────────────────────┐  ├─────────────────────────┤
│  │ 🔍  Search products...          │  │ [empty cart msg if empty]│
│  └─────────────────────────────────┘  │                         │
│  [error bar — hidden]                 │  Item      Qty    Price  │
│                                       │  ──────────────────────  │
│  ┌────────┐ ┌────────┐ ┌────────┐    │  [product] [−][n][+] ₱xx │
│  │ [img]  │ │ [img]  │ │ [img]  │    │  [product] [−][n][+] ₱xx │
│  │  Name  │ │  Name  │ │  Name  │    │  (scrollable)       [✕]  │
│  │ ₱ price│ │ ₱ price│ │ ₱ price│    │                         │
│  └────────┘ └────────┘ └────────┘    ├─────────────────────────┤
│  (scrollable WrapPanel)               │  CHECKOUT               │
│                                       │  Discount [______][Apply]│
│                                       │  Subtotal         ₱ xxx │
│                                       │  Discount       − ₱ xxx │
│                                       │  VAT 12%          ₱ xxx │
│                                       │  ─────────────────────  │
│                                       │  TOTAL DUE       ₱X,XXX │
│                                       │  [      ✓ CHECKOUT     ]│
└───────────────────────────────────────┴─────────────────────────┘
```

**UX notes:**
- Product tile on hover: slight `HoverBrush` background + scale 1.02
- Clicking tile adds 1 to cart; if already in cart, increments qty
- Cart "Clear" button should ask for confirmation (MessageBox) — destructive
- Empty cart: show a centered muted message "Cart is empty"
- Checkout button disabled when cart is empty

---

## 5. Products (List + Edit)

```
┌────────────────────────────────────────────────┬─────────────────┐
│  Products                        [+ New Product]│  Product Details│
│  ┌────────────────────────────────────────────┐ │  (click row to  │
│  │  🔍  Search products...                    │ │  populate)      │
│  └────────────────────────────────────────────┘ │                 │
│  [error bar — hidden]                           │  NAME           │
│                                                 │  [___________]  │
│  ┌──────────┬───────┬───────┬────────┬────────┐ │                 │
│  │ Name     │ Cat.  │ Brand │ Price  │ Active │ │  PRICE (₱)      │
│  │          │       │       │        │        │ │  [___________]  │
│  │          │       │       │        │        │ │                 │
│  │ (empty: "No products found")       │        │ │  CATEGORY  [▼]  │
│  └──────────┴───────┴───────┴────────┴────────┘ │  BRAND     [▼]  │
│                                                 │                 │
│                                                 │  DESCRIPTION    │
│                                                 │  [multiline]    │
│                                                 │                 │
│                                                 │  [x] Active     │
│                                                 │  [x] Featured   │
│                                                 │                 │
│                                                 │  [feedback]     │
│                                                 │  [Save][Deact.] │
└────────────────────────────────────────────────┴─────────────────┘
```

**UX notes:**
- **[+ New Product]** navigates to the dedicated **Create Product** page (page 5a below)
- Right panel = **quick-edit only** for existing products (name, price, category, etc.)
- Right panel does NOT handle image management — that's on the full Create/Edit page
- Clicking a row in the grid loads it into the right panel for quick edits
- Double-clicking a row (or a dedicated "Full Edit" link) navigates to the Edit page pre-populated

---

## 5a. Create / Edit Product *(new dedicated page)*

This is a full-page form, navigated to from the Products list via **[+ New Product]** or
a **[Full Edit]** button per row. It replaces the cramped 320px side-panel for product creation.

```
┌──────────────────────────────────────────────────────────────────┐
│  [← Back to Products]                                            │
│                                                                  │
│  Create New Product                                              │
│  Fill in all required fields. Changes reflect on the website.    │
├───────────────────────────────────┬──────────────────────────────┤
│  PRODUCT DETAILS                  │  IMAGE PREVIEW               │
│                                   │                              │
│  NAME *                           │  ┌────────────────────────┐  │
│  [________________________________]│  │                        │  │
│                                   │  │   [primary image here] │  │
│  CATEGORY *        BRAND          │  │   (loaded from URL)    │  │
│  [dropdown ▼]     [dropdown ▼]    │  │                        │  │
│                                   │  └────────────────────────┘  │
│  PRICE (₱) *       FEATURED       │                              │
│  [____________]   [ ] Featured    │  IMAGE URL                   │
│                                   │  [______________________________]
│  DESCRIPTION                      │  [+ Add Image]               │
│  [multiline textarea, 4 rows]     │                              │
│                                   │  ADDED IMAGES                │
│  STATUS                           │  ┌─────────────────────────┐ │
│  (•) Active  ( ) Inactive         │  │ [img] url… [★ Primary][✕]│ │
│                                   │  │ [img] url… [  Primary][✕]│ │
│                                   │  └─────────────────────────┘ │
│                                   │  (★ = marked as primary)    │
│  [error / success message]        │                              │
│                                   │                              │
│  [Cancel]          [Save Product] │                              │
└───────────────────────────────────┴──────────────────────────────┘
```

### Field details

| Field | Type | Required | Notes |
|-------|------|----------|-------|
| Name | TextBox | Yes | max 200 chars |
| Category | ComboBox | Yes | populated from DB |
| Brand | ComboBox | No | populated from DB |
| Price | TextBox (numeric) | Yes | ₱ prefix, 2 decimal places |
| Description | TextBox multiline | No | 4 visible rows, accepts return |
| Status | RadioButton group | Yes | Active / Inactive (default: Active) |
| Featured | CheckBox | No | marks product as featured in WebApp |
| Image URL | TextBox | No | any direct image URL |
| Primary flag | Per-image toggle | — | only one image can be primary |

### Image workflow
1. Admin pastes a URL into **Image URL** TextBox
2. Clicks **[+ Add Image]** — app loads the URL into the preview pane on the right
3. If the image loads successfully, it is added to the **Added Images** list below
4. Admin can click **[★ Primary]** on any image to mark it as the primary image shown in the WebApp product card
5. Admin can click **[✕]** to remove an image from the list before saving
6. On **[Save Product]**: calls `IProductService.CreateProduct(product)` first to get the new `ProductId`, then calls `IProductService.AddProductImage(image)` once per entry in the list

### Navigation
- **[Cancel]** → navigates back to Products list, no save
- **[Save Product]** → saves, shows success message, then navigates back to Products list after 1.5 s
- Back button (`[← Back to Products]`) → same as Cancel

### How it reflects on WebApp
`ProductImage.ImageUrl` is shared between AdminSystem and WebApp via the same SQL Server database.
As soon as a row is inserted into `ProductImages` with `IsPrimary = true`, the WebApp product
card query picks it up on the next page load — no separate sync step required.

---

## 6. Inventory

```
┌──────────────────────────────────────────────┬────────────────────┐
│  Inventory                                   │  Stock Adjustment  │
│  ─────────────────────────────────────────── │  ─────────────────│
│  Recent Movements                            │  PRODUCT / VARIANT │
│  ┌──────────────────────────────────────┐    │  [searchable ▼]    │
│  │ Product │ Variant │ Type │ Qty │ Date │    │  (type to filter)  │
│  │         │         │      │     │      │    │                    │
│  │ (empty: "No movements recorded")     │    │  QUANTITY (+/-)    │
│  └──────────────────────────────────────┘    │  [______________]  │
│                                              │  (negative = deduct│
│  ⚠ Low Stock Alerts                          │                    │
│  ┌──────────────────────────────────────┐    │  TYPE              │
│  │ Product │ Variant              │Stock │    │  [dropdown ▼]      │
│  │ (yellow-amber border card)      │      │    │   Adjustment       │
│  │ (empty: "All variants stocked") │      │    │   Damage           │
│  └──────────────────────────────────────┘    │   Loss / Return    │
│                                              │                    │
│                                              │  NOTES             │
│                                              │  [multiline]       │
│                                              │                    │
│                                              │  [error / success] │
│                                              │  [Apply Adjustment]│
└──────────────────────────────────────────────┴────────────────────┘
```

**UX fix:** Replace the raw Variant ID TextBox with a searchable ComboBox so admin never
needs to know database IDs. The ComboBox should display `"Product Name — Variant"` per item.

---

## 7. Delivery

```
┌──────────────────────────────────────────┬──────────────────────┐
│  Deliveries                   [↻ Refresh]│  Delivery Actions    │
│  [error bar — hidden]                    │                      │
│                                          │  (nothing selected)  │
│  ┌────────┬────────┬────────┬──────┬───┐ │  "Select a delivery  │
│  │Order ID│Courier │Status  │Delay?│Cre│ │   from the list."    │
│  │        │        │        │      │   │ │                      │
│  │        │(empty: "No deliveries")     │ │  ──── when selected ─│
│  └────────┴────────┴────────┴──────┴───┘ │  Order #: [value]    │
│                                          │  Customer: [value]   │
│                                          │  Status: [badge]     │
│                                          │  ────────────────    │
│                                          │  TRACKING REF        │
│                                          │  [______________]    │
│                                          │  [Assign Tracking]   │
│                                          │  [Mark Delivered]    │
│                                          │  [Mark Failed]       │
│                                          │  [feedback]          │
└──────────────────────────────────────────┴──────────────────────┘
```

**UX fix:** Show Order # + Customer name at the top of the action panel when a delivery
is selected — confirms admin is acting on the right shipment.

---

## 8. Reports

```
┌──────────────────────────────────────────────────────────────────┐
│  Reports                                         [↻ Refresh]     │
│  ─────────────────────────────────────────────────────────────── │
│  DATE RANGE   From [date ▼]   To [date ▼]   [Apply Filter]       │
│  [error bar — hidden]                                            │
├──────────────┬────────────┬─────────────────┬────────────────────┤
│ TODAY'S      │ PERIOD     │ DELIVERED       │ CANCELLED          │
│ SALES        │ SALES      │ ORDERS          │ ORDERS             │
│ ₱ 0.00       │ ₱ 0.00     │ 0               │ 0                  │
├──────────────┴────────────┴─────────────────┴────────────────────┤
│  ┌──────────────────┬───────────────────┬────────────────────┐   │
│  │ Top Orders       │ Low Stock Products│ Inventory Movements│   │
│  │ by Value         │                   │                    │   │
│  │ Order# │Customer │ Product │ Brand   │ Product│Type│Qty   │   │
│  │        │  Value  │         │         │        │    │Date  │   │
│  │ (max 300px tall) │ (max 300px tall)  │ (max 300px tall)   │   │
│  └──────────────────┴───────────────────┴────────────────────┘   │
└──────────────────────────────────────────────────────────────────┘
```

**UX fix:** "Top Orders by Value" must include an **Amount** column — it's the whole point of
the table. Without it, the table is just a duplicate of the Recent Orders on Dashboard.

---

## 9. Vouchers

```
┌──────────────────────────────────────────┬──────────────────────┐
│  Vouchers      [+ New Voucher] [↻ Refresh]│  New Voucher        │
│  [error bar — hidden]                    │  ─────────────────── │
│                                          │  CODE *              │
│  ┌──────┬──────┬─────┬──────┬─────┬────┐ │  [______________]    │
│  │Code  │Type  │Value│Uses/ │Exp. │Act.│ │                      │
│  │      │      │     │ Max  │     │    │ │  DISCOUNT TYPE       │
│  │ (empty: "No vouchers yet")          │ │  [Percentage ▼]      │
│  └──────┴──────┴─────┴──────┴─────┴────┘ │                      │
│                                          │  VALUE *             │
│                                          │  [____________]      │
│                                          │                      │
│                                          │  MIN ORDER           │
│                                          │  [____________]      │
│                                          │                      │
│                                          │  MAX USES  MAX/USER  │
│                                          │  [______] [________] │
│                                          │                      │
│                                          │  EXPIRES AT          │
│                                          │  [date picker]       │
│                                          │                      │
│                                          │  [Save Voucher]      │
│                                          │  [Deactivate] (edit) │
│                                          │  [feedback]          │
└──────────────────────────────────────────┴──────────────────────┘
```

**UX fix:** Grid "Uses / Max" column should show `"used / max"` format (e.g. `"3 / 50"`)
so admin can see voucher consumption at a glance.

---

## 10. Users

```
┌──────────────────────────────────────────┬──────────────────────┐
│  Users                        [↻ Refresh]│  User Detail         │
│  ┌──────────────────────────────────────┐│  ─────────────────── │
│  │  🔍  Search by name or email…        ││  NAME                │
│  └──────────────────────────────────────┘│  [value]             │
│  [error bar — hidden]                    │                      │
│                                          │  EMAIL               │
│  ┌──┬──────┬──────────┬──────┬────┬────┐ │  [value]             │
│  │# │Name  │Email     │Phone │Ver.│Act.│ │                      │
│  │  │      │          │      │    │    │ │  PHONE               │
│  │  │      │          │      │    │    │ │  [value]             │
│  │  (empty: "No users found")  │    │    │ │                      │
│  └──┴──────┴──────────┴──────┴────┴────┘ │  [Active] [Verified] │
│  (Last Login col also present)           │  Joined [date]       │
│                                          │  ─────────────────── │
│                                          │  [Deactivate User]   │
│                                          │  [feedback]          │
└──────────────────────────────────────────┴──────────────────────┘
```

**UX fix:** Add search TextBox above the grid so admin can find users by name or email
without scrolling through a potentially large list.

---

## 11. Support

```
┌─────────────────────────────────────────┬──────────────────────┐
│  Support Tickets             [↻ Refresh]│  Ticket Detail       │
│  ─────────────────────────────────────  │  ─────────────────── │
│  [All]·[Open]·[In Progress]·[Awaiting]  │  Subject: [text]     │
│  [Resolved]                             │  Customer · Category  │
│  [error bar — hidden]                   │  Date                 │
│                                         │  [✔ Mark Resolved]   │
│  ┌──┬────────┬───────┬──────┬─────┬───┐ │  ─────────────────── │
│  │# │Customer│Subject│ Cat. │Stat.│Dt.│ │  REPLIES             │
│  │  │        │       │      │     │   │ │  (scrollable, 260px) │
│  │  │ (empty: "No tickets found")     │ │  [bubble per reply]  │
│  └──┴────────┴───────┴──────┴─────┴───┘ │                      │
│                                         │  ADD REPLY           │
│                                         │  [textarea, 72px]    │
│                                         │  [Send Reply]        │
│                                         │  ─────────────────── │
│                                         │  TASKS               │
│                                         │  [task list]         │
│                                         │  [type dropdown]     │
│                                         │  [Add Task]          │
│                                         │  [feedback]          │
└─────────────────────────────────────────┴──────────────────────┘
```

**UX fix:** Replies list `MaxHeight` raised from 160px → 260px so at least 3–4 replies
are visible without scrolling.

---

## Design Token Reference

| Token key | Hex | Used for |
|-----------|-----|----------|
| `BgBrush` | `#18181A` | Window background |
| `SurfaceBrush` | `#232325` | Cards, panels, sidebar |
| `HoverBrush` | `#2E2E30` | Row hover, button hover |
| `BorderBrush` | `#3F3F42` | Input borders, dividers |
| `AccentBrush` | `#8AB4F8` | Focus rings, links, info |
| `TextPrimaryBrush` | `#F9FAFB` | Body text, headings |
| `TextSecondaryBrush` | `#D1D5DB` | Subtitles, labels |
| `TextMutedBrush` | `#9CA3AF` | Hints, timestamps |
| `SuccessBrush` | `#81C995` | Delivered, verified, active |
| `WarningBrush` | `#FDE293` | Pending, low-stock, on-hold |
| `AlertBrush` | `#FFB74D` | Delayed, near-expiry |
| `ErrorBrush` | `#F28B82` | Errors, cancelled, destructive |
| Primary CTA | `#CC0000` | Main action buttons (hardcoded — migrate to token `PrimaryActionBrush`) |

---

## Structural Refactoring Plan (for redesign implementation)

| Component | Currently | Target |
|-----------|-----------|--------|
| DataGrid header style | Duplicated in every view | Single `DataGridColumnHeaderStyle` in `AppStyles.xaml` |
| DataGrid cell style | Duplicated in every view | Single `DataGridCellStyle` in `AppStyles.xaml` |
| Error banner | Duplicated in 6 views | `ErrorBanner` UserControl with a `Message` property |
| Filter pill bar | Different in Orders vs Support | `FilterPillBar` UserControl with `ItemsSource` + `SelectedFilter` |
| Right-side action panel | Repeated pattern in 7 views | `DetailPanel` ContentControl with consistent header + scroll |
| Page header (`Title + actions`) | Inconsistent across all views | `PageHeader` UserControl with `Title`, `Subtitle`, `Actions` slots |
| Empty-state placeholder | Missing everywhere | `EmptyState` UserControl with `Icon` + `Message` properties |
