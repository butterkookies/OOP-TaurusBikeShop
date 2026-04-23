# Taurus Bike Shop — Complete Feature & Capability Inventory

> Every feature below is verified against actual source code. Nothing is assumed.

---

## 🌐 PART A — WEB APPLICATION (Customer-Facing E-Commerce)

### A1. Homepage & Navigation
1. It can display a **homepage** with a hero section, marketing hooks, and curated featured products (8 products max).
2. It can show **featured products** on the homepage with wishlist indicators for logged-in users.
3. It can render a **Privacy Policy** static page accessible to all visitors.
4. It can display a **generic error page** in production that hides stack traces from end users.
5. It can show a **navigation bar** with links to catalog, cart, account, and wishlist across all pages.
6. It can display a **cart badge** in the navigation showing the current item count, updated live via AJAX.
7. It can show **toast notifications** (success, error, warning, info) on every page via a shared utility.
8. It can render a **loading spinner** on buttons during AJAX operations to prevent double-clicks.

### A2. Customer Registration & Authentication
9. It can **register** a new customer account with full name, email, phone number, and password.
10. It can **validate registration** fields (required fields, format checks) before submitting.
11. It can **send an OTP** (one-time password) to the customer's email for email verification during registration.
12. It can **verify the OTP** code submitted by the customer to confirm their email address.
13. It can **resend a fresh OTP** if the first one expired or wasn't received (via AJAX, returns JSON).
14. It can **create a user account** with a BCrypt-hashed password (work factor 12) upon successful OTP verification.
15. It can **automatically sign in** the user immediately after successful registration.
16. It can **log in** a customer using email and password with BCrypt hash verification.
17. It can **set an authentication cookie** (`.TaurusBikeShop.Auth`) with encrypted claims (UserId, FullName, Email, Role).
18. It can **enforce a 1-hour sliding session expiry** — each request resets the cookie timer.
19. It can **enforce a 30-minute idle timeout** on the session cookie (`.TaurusBikeShop.Session`).
20. It can **log out** a customer by clearing the authentication cookie and redirecting to the homepage.
21. It can **redirect unauthenticated users** to the login page when they try to access protected pages.

### A3. Customer Profile & Account Management
22. It can **display a customer dashboard** with recent orders, wishlist count, and quick navigation links.
23. It can **render a profile page** showing the customer's personal information (name, email, phone).
24. It can **update personal information** (first name, last name, phone number) via the profile form.
25. It can **change the customer's password** by verifying the current password and accepting a new one.
26. It can **add a new saved address** with full fields: street, city, province, zip code, country.
27. It can **delete a saved address** from the customer's address book.
28. It can **set a default address** which is pre-selected during checkout.
29. It can **display a paginated notification history** for the customer (order updates, promotions, etc.).
30. It can **show a notification count badge** via an AJAX endpoint for the nav bar bell icon.
31. It can **mark a single notification as read** via AJAX.
32. It can **mark all notifications as read** at once via AJAX.

### A4. Product Catalog & Browsing
33. It can **browse a paginated product catalog** showing product cards with images, names, and prices.
34. It can **filter products by category** (e.g., Bikes, Parts, Accessories, Apparel, Services).
35. It can **filter products by brand** from a list of active brands.
36. It can **filter products by price range** (minimum and maximum price).
37. It can **search products by keyword** matching product name and description.
38. It can **filter featured products only** for promotional sections.
39. It can **paginate catalog results** with 12 products per page and page navigation controls.
40. It can **resolve a category code from the URL** (friendly URLs like `/Product/List?categoryCode=bikes`) to the internal category ID.
41. It can **show wishlist heart indicators** on product cards for authenticated users.
42. It can **view a product detail page** with full description, multiple images, and variant selector.
43. It can **select a product variant** (size, color) and see the updated price and stock level via AJAX.
44. It can **return the price and stock for a variant** as JSON (called by `product-catalog.js` on variant selection).
45. It can **display product reviews** on the detail page with paginated "Load More" functionality.
46. It can **load more reviews via AJAX** as partial HTML fragments (5 reviews per page).

### A5. Shopping Cart
47. It can **support guest shopping carts** — unauthenticated users get a [GuestSession](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/Controllers/CartController.cs#199-204) with a cookie-tracked session ID.
48. It can **create a guest session automatically** on first cart interaction if none exists.
49. It can **add a product to the cart** via AJAX (with product ID, variant ID, and quantity).
50. It can **view the full cart page** with product thumbnails, names, variants, quantities, and line totals.
51. It can **update the quantity** of a cart item via AJAX (increment/decrement).
52. It can **remove a cart item** from the cart via AJAX.
53. It can **get the current cart count** via AJAX for the nav badge.
54. It can **merge a guest cart into an authenticated cart** when a guest user logs in — items are re-assigned.

### A6. Wishlist
55. It can **view the wishlist page** showing all saved products with images, names, and prices.
56. It can **toggle a product in/out of the wishlist** via AJAX (add if not present, remove if present).
57. It can **remove a product from the wishlist** explicitly via AJAX on the wishlist page.
58. It can **move a wishlist item directly to the cart** via AJAX — adds to cart, removes from wishlist, updates cart badge.

### A7. Voucher System (Customer Side)
59. It can **validate a voucher code** at checkout against the cart subtotal via AJAX — checks expiry, usage limits, minimum order amount.
60. It can **store the validated voucher** in the server-side session so it persists across checkout page refreshes.
61. It can **remove a previously applied voucher** from the session, restoring the full subtotal.
62. It can **display the discount amount** after voucher validation — percentage-based or flat-amount.

### A8. Checkout
63. It can **render the checkout page** with an order summary sidebar, address selector, delivery method picker, and payment method selector.
64. It can **redirect to the cart** if the customer tries to check out with an empty cart.
65. It can **select a saved delivery address** from the customer's address book.
66. It can **choose a fulfilment method**: Delivery or Pickup.
67. It can **auto-assign the delivery courier** (Lalamove for Metro Manila, LBC for provincial) based on the customer's shipping address province — no manual courier selection.
68. It can **choose a payment method**: GCash or Bank Transfer (BPI).
69. It can **display assigned vouchers** with remaining time-left formatting.
70. It can **place an order** — creates Order, OrderItems, locks inventory (InventoryLog), creates Payment record, creates Delivery (if applicable), records VoucherUsage (if applicable) — all in a single database transaction.
71. It can **clear voucher session keys** after a successful order placement.
72. It can **redirect to the order confirmation page** after successful order creation.

### A9. Payment
73. It can **render a payment submission page** showing the order details, chosen payment method, and store payment account details (GCash number or bank account).
74. It can **detect already-submitted payments** and redirect to order detail with an info message.
75. It can **submit a GCash payment proof** — upload a screenshot (image up to 15 MB) with GCash number and reference number.
76. It can **submit a bank transfer payment proof** — upload a deposit slip (image/PDF up to 15 MB) with depositor name and optional BPI reference number.
77. It can **upload files to Google Cloud Storage** via [FileUploadHelper](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/Utilities/FileUploadHelper.cs#59-79) with MIME type validation and unique GCS object paths.
78. It can **enforce a 15 MB file size limit** on payment proof uploads (both at Kestrel and multipart form level).
79. It can **transition order status** from [Pending](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/BackgroundJobs/NotificationDispatchJob.cs#71-133) to `PendingVerification` upon proof submission.

### A10. Order Management (Customer Side)
80. It can **view order confirmation** after placing an order — shows order number, items, total, and payment instructions.
81. It can **view paginated order history** (10 orders per page) showing order number, date, status, and total.
82. It can **view order detail** — full breakdown with items, quantities, prices, payment status, delivery status, and order timeline.
83. It can **cancel a pending order** — transitions status to Cancelled, unlocks reserved inventory (InventoryLog Unlock entries).
84. It can **confirm delivery receipt** — customer marks an out-for-delivery order as Delivered, finalising inventory.

### A11. Product Reviews
85. It can **view "My Reviews" page** listing all reviews the customer has submitted.
86. It can **show pending review prompts** for products from delivered orders that haven't been reviewed yet.
87. It can **submit a product review** with a star rating (1–5) and text comment.
88. It can **validate review submission** (rating required, product must exist, order must be delivered to this customer).
89. It can **display reviews publicly** on the product detail page — visible to all users including guests (no auth required).

### A12. Support Tickets (Customer Side)
90. It can **view a list of support tickets** submitted by the customer.
91. It can **create a new support ticket** with subject, description, category, and priority.
92. It can **pre-populate the order ID** when creating a ticket from the order detail page.
93. It can **view ticket detail** with the full reply thread (customer and admin messages).
94. It can **reply to an existing ticket** to continue the conversation with support staff.

### A13. Notifications & Email
95. It can **queue notification emails** (order status changes, payment reminders, delivery updates) in the database.
96. It can **dispatch queued emails** via Gmail SMTP using MailKit (NotificationDispatchJob: 60-second polling, batch of 10).
97. It can **retry failed email dispatches** up to 3 times with failure reason tracking.
98. It can **send OTP verification codes** via email during registration.
99. It can **send a 4-hour pre-expiry reminder** before auto-cancelling a pending order.

### A14. Background Automation
100. It can **auto-cancel pending orders after 24 hours** — PendingOrderMonitorJob runs every 5 minutes, cancels expired orders, unlocks locked inventory, and queues a cancellation email.
101. It can **send pre-expiry reminder notifications** 4 hours before the 24-hour order cancellation deadline.
102. It can **move orders to OnHold** when the bank transfer verification deadline expires — PaymentTimeoutJob checks every 5 minutes.
103. It can **monitor inventory levels** — InventorySyncJob runs every 60 seconds and counts variants with zero stock.
104. It can **alert admin and customers when stock drops below reorder threshold** — StockMonitorJob runs every 15 minutes with a 24-hour cooldown per variant.
105. It can **poll courier APIs for delivery status updates** — DeliveryStatusPollJob runs every 5 minutes (currently stubbed, returns null).
106. It can **write to the SystemLog table** for every background job event (start, complete, error, throttled).
107. It can **implement exponential backoff** on consecutive job failures, capped at 2 minutes.
108. It can **use staggered startup delays** (2s, 5s, 8s, 11s, 14s, 17s) to prevent all 6 jobs from hitting the database simultaneously.

### A15. Security & Infrastructure
109. It can **protect all POST endpoints against CSRF** with a global `AutoValidateAntiforgeryToken` filter.
110. It can **serve anti-forgery tokens** via a dedicated GET `/antiforgery/token` endpoint for AJAX-heavy pages.
111. It can **include the CSRF token in all AJAX calls** via [fetchWithCSRF()](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/wwwroot/js/utils.js#6-23) in utils.js.
112. It can **hash all passwords with BCrypt** (work factor 12) — never stores plaintext.
113. It can **enforce HTTPS-only cookies in production** (`CookieSecurePolicy.Always`) while allowing HTTP in development.
114. It can **manage secrets** via .NET User Secrets (development) and environment variables (production) — never hardcoded.
115. It can **retry transient database failures** with EF Core's retry policy (3 retries, 5-second max delay).
116. It can **pool DbContext instances** (pool size 32) for performance.
117. It can **handle unobserved task exceptions** and `AppDomain.UnhandledException` to write fatal errors to stderr.
118. It can **format currency as Philippine Peso** (₱) via [formatCurrency()](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/wwwroot/js/utils.js#66-73) in utils.js.
119. It can **debounce** and **throttle** event handlers via shared utility functions.

---

## 🖥️ PART B — ADMIN SYSTEM v2 (WPF Desktop Application)

### B1. Authentication & Session
120. It can **authenticate staff** via email + password with BCrypt verification using Dapper (direct SQL).
121. It can **show field-level validation errors** on the login form (email required, password required).
122. It can **toggle password visibility** on the login screen.
123. It can **display a "forgot password" dialog** instructing staff to contact the system administrator.
124. It can **store the current user session** in `App.CurrentUser` (static property) after successful login.
125. It can **transition from LoginWindow to MainWindow** on successful authentication.
126. It can **sign out** with a confirmation dialog, clearing `App.CurrentUser` and returning to the login screen.
127. It can **exit the application** with a confirmation dialog.

### B2. Navigation & Shell
128. It can **navigate between 9 pages** via a sidebar menu: Dashboard, Products, Orders, Reports, Staff, POS, Vouchers, Payment Accounts, Support Tickets.
129. It can **automatically load data** when navigating to each page (lazy loading on navigation).
130. It can **restrict Staff page access** to Admin-role users only (non-admins cannot navigate there).
131. It can **display breadcrumbs** showing `Admin → {current page}` in the header.
132. It can **show the logged-in user's name, initials, and role** in the sidebar/header.

### B3. Dashboard
133. It can **display the total product count** as a stat card.
134. It can **display the low stock item count** as a stat card.
135. It can **display today's total orders** as a stat card.
136. It can **display this week's total orders** as a stat card.
137. It can **show a list of low-stock variants** with product name, variant details, and current stock quantity.
138. It can **display a 7-day order bar chart** showing daily order counts with normalized height ratios and "Today" label.
139. It can **refresh all dashboard data** on demand via a Refresh button.

### B4. Order Management
140. It can **load and display a filterable order list** with status badges showing counts per status.
141. It can **filter orders by status** (Pending, PendingVerification, Processing, ReadyForPickup, Shipped, Delivered, OnHold, Cancelled).
142. It can **select an order to view its details** — loads order items, payment info, delivery info, and customer details.
143. It can **transition an order from Pending to Processing** (mark as processing).
144. It can **transition an order from Processing to ReadyForPickup** (for pickup orders).
145. It can **confirm a customer pickup** — transitions ReadyForPickup to PickedUp.
146. It can **mark an order as Out for Delivery** — transitions Processing to Shipped.
147. It can **mark an order as Delivered** — transitions Shipped/OutForDelivery to Delivered.
148. It can **approve a payment** — verifies GCash/bank transfer proof and transitions PendingVerification to Processing.
149. It can **hold a payment** — transitions PendingVerification to OnHold with a reason.
150. It can **cancel an order** — transitions to Cancelled with confirmation dialog.
151. It can **enforce valid status transitions** — throws `InvalidStatusTransitionException` for forbidden transitions (e.g., Delivered → Pending).
152. It can **select multiple orders** using checkboxes for bulk operations.
153. It can **bulk update status** for multiple selected orders simultaneously with progress tracking.
154. It can **bulk cancel** multiple selected orders simultaneously with progress tracking and confirmation.
155. It can **build color-coded status badge counts** (e.g., red for Pending, orange for OnHold, green for Delivered).
156. It can **track selection state** across the order list — enabling/disabling action buttons based on what's selected.

### B5. Point of Sale (POS)
157. It can **load all products with variants** for the POS product grid.
158. It can **search and filter products** in the POS view for quick lookup.
159. It can **add a product to the POS cart** — auto-selects the first available variant if no variant is specified.
160. It can **increment the quantity** of a POS cart item (respects max stock limit).
161. It can **decrement the quantity** of a POS cart item (minimum 1, or removes at 0).
162. It can **remove an item from the POS cart** explicitly.
163. It can **compute and display live cart totals** — subtotal, discount, and final total — refreshed on every cart change.
164. It can **search for registered customers** to associate a POS sale with their account.
165. It can **select a registered customer** for the POS transaction.
166. It can **set "Walk-In" mode** for anonymous customers (no account association).
167. It can **load voucher suggestions** for the selected customer based on their assigned vouchers.
168. It can **filter voucher suggestions** by code or description text.
169. It can **select a voucher** from suggestions for potential application.
170. It can **apply a voucher** to the POS cart — validates eligibility, calculates discount, updates totals.
171. It can **remove an applied voucher** from the POS cart — restores original subtotal.
172. It can **revalidate the applied voucher** when cart contents change to ensure it still meets minimum order requirements.
173. It can **complete a POS sale** — creates an Order, OrderItems, POS_Session, Payment (Cash), locks inventory, records VoucherUsage — all in a single operation.
174. It can **start a new sale** — clears the entire POS state (cart, customer, voucher) and reloads products.

### B6. Product Management
175. It can **load all products in a list** with search, category filter, and summary details.
176. It can **search products by name** with real-time results.
177. It can **select a product to view its details** — loads variants, images, and full metadata.
178. It can **begin adding a new product** — opens a blank form with category/brand selectors and markup fields.
179. It can **save a new product** or **update an existing product** (name, description, category, brand, base price, markup percentage, featured flag).
180. It can **cancel editing** — discards unsaved changes and closes the form.
181. It can **deactivate (soft-delete) a product** — marks as inactive with a confirmation dialog.
182. It can **open a "Add Variant" form** for a selected product — size, color, additional price, stock quantity, reorder threshold, SKU.
183. It can **save a new variant** to a product.
184. It can **open an "Edit Variant" form** — loads the existing variant data for modification.
185. It can **save an edited variant** — updates size, color, price, stock, reorder threshold, SKU.
186. It can **load product images** for the selected product.
187. It can **add a product image** — uploads via file dialog and saves the image URL.
188. It can **remove a product image** with confirmation.
189. It can **clone a product** — duplicates the product's properties for quick replication.
190. It can **refresh the product list** on demand.

### B7. Staff Management
191. It can **load and display all staff accounts** (admin, manager, staff roles).
192. It can **select a staff member** to view their details (name, email, role, active status).
193. It can **begin creating a new staff account** — opens a form with name, email, password, and role fields.
194. It can **save a new staff account** — creates the user with BCrypt-hashed password and assigned role.
195. It can **change a staff member's role** (Admin, Manager, Staff) with confirmation dialog.
196. It can **toggle a staff member's active status** (activate/deactivate) with confirmation dialog.
197. It can **reset a staff member's password** by entering a new password (hashed with BCrypt before saving).

### B8. Reports & Analytics
198. It can **switch between Sales and Inventory report tabs**.
199. It can **select a date range** for sales reports (start date, end date).
200. It can **apply period presets** (Today, This Week, This Month, This Year) that auto-fill the date range.
201. It can **load sales report data** — daily sales totals, revenue, order count, and top products.
202. It can **display a sales trend line chart** using OxyPlot with date-based X-axis and revenue Y-axis.
203. It can **show a sales summary** — total revenue, average order value, total orders in the selected period.
204. It can **show top-selling products** ranked by units sold in the selected period.
205. It can **load inventory report data** — product/variant stock levels, reorder thresholds, and stock values.
206. It can **build OxyPlot chart models** dynamically with styled axes, line series, and adaptive formatting.
207. It can **export sales report data to an Excel file** via `ExcelExportService` (generates `.xlsx` with structured worksheets).

### B9. Voucher Management
208. It can **load and display all vouchers** in a list with code, type, discount, dates, and usage count.
209. It can **switch between Vouchers list and User Assignment tabs**.
210. It can **filter vouchers** by status (Active, Expired, Used Up, All).
211. It can **open a "New Voucher" form** with fields for code, description, discount type (percentage/flat), discount value, minimum order amount, usage limit, start/end dates.
212. It can **auto-generate a random voucher code** via [GenerateCode()](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/AdminSystem_v2/ViewModels/VoucherViewModel.cs#455-462).
213. It can **open an "Edit Voucher" form** pre-populated with the selected voucher's data.
214. It can **save a new or edited voucher** (create/update) with validation.
215. It can **toggle a voucher's active status** (activate/deactivate) with confirmation dialog.
216. It can **search for registered users** by name/email for voucher assignment.
217. It can **add users to the assignment list** — builds a list of users to receive a voucher.
218. It can **remove users from the assignment list** before assigning.
219. It can **assign a voucher to multiple users and send notification emails** — creates UserVoucher records and queues notification emails.

### B10. Support Ticket Management (Admin Side)
220. It can **load and display all support tickets** from all customers.
221. It can **filter tickets by status** (Open, InProgress, Resolved, All) using a filter predicate.
222. It can **select a ticket to view its details** and load the full reply thread.
223. It can **reply to a customer's support ticket** — adds an admin reply to the thread.
224. It can **mark a ticket as Resolved** — closes the ticket with confirmation.

### B11. Store Payment Account Management
225. It can **load and display all store payment accounts** (GCash and BankTransfer accounts shown to customers on the payment page).
226. It can **select a payment account** to view and edit its details.
227. It can **create a new store payment account** — payment method (GCash/BankTransfer), account name, account number, bank name, QR image URL, instructions, active status, display order.
228. It can **update an existing store payment account** — modify any fields.
229. It can **delete a store payment account** with confirmation dialog.
230. It can **toggle account active status** — only active accounts are shown to customers on the website.
231. It can **set display order** — controls the sequence in which payment options appear to customers.

### B12. Admin Infrastructure & UX
232. It can **show loading spinners** on every page during data operations (inherited from `BaseViewModel.IsLoading`).
233. It can **display error messages** in red banners when operations fail (inherited from `BaseViewModel.ErrorMessage`).
234. It can **display success messages** in green banners when operations succeed (inherited from `BaseViewModel.SuccessMessage`).
235. It can **use confirmation dialogs** before destructive actions (delete, cancel, deactivate) via `IDialogService`.
236. It can **show info dialogs** for informational messages (e.g., forgot password instructions).
237. It can **bind all UI controls to ViewModel properties** via WPF data binding with `INotifyPropertyChanged`.
238. It can **use RelayCommand** for all button commands, with support for `CanExecute` to conditionally enable/disable buttons.
239. It can **use Dapper for all database queries** — raw SQL with parameterized queries, no ORM overhead.
240. It can **create a new SQL connection per operation** via `DatabaseHelper.GetConnection()` — no shared connection state.

---

## 🔗 PART C — SHARED / CROSS-SYSTEM CAPABILITIES

### C1. Database-Level Integration
241. Both systems **share a single SQL Server database** on Google Cloud SQL — no data replication needed.
242. Both systems **read and write the same Order table** — web creates orders, admin processes them.
243. Both systems **share the same InventoryLog** — web locks/unlocks stock, admin adjusts/receives stock.
244. Both systems **share the same User table** — customers created by web, staff created by admin.
245. Both systems **share the same Voucher and UserVoucher tables** — admin creates vouchers and assigns to users, web validates and redeems them.
246. Both systems **share the same SupportTicket and SupportTicketReply tables** — customers create tickets on web, admin responds via desktop app.
247. Both systems **share the same StorePaymentAccount table** — admin configures payment accounts, web displays them to customers during checkout.
248. Both systems **share the same Notification table** — background jobs create notifications, web displays them.

### C2. Security
249. Both systems **use BCrypt with work factor 12** for password hashing — same algorithm and strength.
250. Both systems **use the same connection string** to access the shared database.
251. Both systems **manage secrets via .NET User Secrets** in development and environment variables in production.
252. The web system **never exposes stack traces or SQL details** to end users in production.
253. The admin system **enforces role-based access** — Staff page restricted to Admin role, certain operations require Admin or Manager role.

### C3. File Storage
254. It can **upload product images to Google Cloud Storage** (bucket: `taurus-bikeshop-assets`, folder: `products`).
255. It can **upload payment proof screenshots to GCS** (folder: `payment-proofs`).
256. It can **upload support ticket attachments to GCS** (folder: `support-attachments`).
257. It can **upload product images to Cloudinary** as an alternative/fallback image host.
258. It can **validate file MIME types** — product images: JPEG/PNG, payment proofs: JPEG/PNG, support attachments: JPEG/PNG.
259. It can **enforce a 15 MB maximum file size** on all uploads.
260. It can **generate unique GCS object paths** using GUIDs to prevent filename collisions.

### C4. Email Integration
261. It can **send transactional emails via Gmail SMTP** (host: `smtp.gmail.com`, port: 587) using MailKit.
262. It can **send OTP verification codes** during customer registration.
263. It can **send order confirmation emails** after successful order placement.
264. It can **send payment reminder emails** 4 hours before pending order auto-cancellation.
265. It can **send order cancellation emails** when orders are auto-cancelled after 24 hours.
266. It can **send voucher assignment notification emails** when admin assigns vouchers to customers.
267. It can **queue all emails in the Notification table** and dispatch them asynchronously via [NotificationDispatchJob](file:///c:/Users/user/Documents/ANDREI_FILES/PDM_FILES/2ND%20YEAR/2ND%20SEM/OOP/OOP-TaurusBikeShop/WebApplication/BackgroundJobs/NotificationDispatchJob.cs#33-40).

### C5. Audit & Logging
268. It can **maintain an append-only InventoryLog** — every stock change (receive, sell, adjust, lock, unlock) is permanently recorded with timestamp, quantity, and notes.
269. It can **write SystemLog entries** for every background job event (start, complete, error, warning).
270. It can **log order status transitions** in the SystemLog with before/after status values.
271. It can **throttle certain SystemLog writes** (InventorySyncJob: 5-minute intervals) to prevent log flooding.
