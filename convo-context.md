# Pending Flagged Items — Context for New Session

**Project:** `OOP-TaurusBikeShop` — AdminSystem (WPF, C#)
**Base path:** `AdminSystem/AdminSystem/AdminSystem/`

All items below were identified during `/code-quality-suite` audits but **not yet fixed**. Each entry includes the exact file, line reference, and what needs to be done.

---

## MainWindow.xaml + MainWindow.xaml.cs

### 1. Dead nav buttons — `PaymentVerificationNavButton` + `OrderHistoryNavButton`
- **File:** `Views/MainWindow.xaml` ~line 598, 660
- **Problem:** Both buttons exist in the sidebar but have **no `Click` handlers wired** in the constructor. Their `Tag` values (`"PaymentVerification"`, `"OrderHistory"`) do not match any `PageNames` constant and no `Navigate()` case handles them. Clicking either does nothing.
- **Fix needed:** Decide what page each should navigate to (likely `PageNames.Orders` filtered by status, or dedicated pages), add `PageNames` constants if needed, wire the handlers in the constructor alongside the other nav buttons.

### 2. `AdminRoleTextBlock` — hardcoded role
- **File:** `Views/MainWindow.xaml` ~line 513, `Views/MainWindow.xaml.cs` ~line 62
- **Problem:** The sidebar profile card shows `"System Administrator"` hardcoded. `AdminNameTextBlock` and `AdminAvatarTextBlock` were fixed to populate from `App.CurrentUser`, but `AdminRoleTextBlock` was not — the `User` model needs to be checked for a `Role` or `UserRole` property.
- **Fix needed:** Check `App.CurrentUser` for a role field. If it exists: `AdminRoleTextBlock.Text = App.CurrentUser.Role` (or equivalent). If not, add it to the `User` model.

### 3. Hardcoded sidebar badge counts
- **File:** `Views/MainWindow.xaml` ~lines 591, 610, 703
- **Problem:** Three badge `TextBlock`s show static numbers baked into XAML:
  - `PendingOrdersBadgeTextBlock` -> `"7"`
  - `PaymentVerificationBadgeTextBlock` -> `"3"`
  - `SupportTicketsBadgeTextBlock` -> `"2"`
- **Fix needed:** `LoadActivityFeed()` already queries live data. Extend it to also set these badge values — `_orderSvc.GetActiveOrders().Count()`, `_paymentSvc.GetPendingVerification().Count()`, and the equivalent support ticket count.

### 4. System Status panel — hardcoded green indicators
- **File:** `Views/MainWindow.xaml` ~lines 926-931
- **Problem:** "POS Terminal - Active" and "Payment Gateway - Online" status rows are hardcoded green `Ellipse` fills. They never reflect real state.
- **Fix needed:** Either remove the fake rows (leaving only the real `ActivityDbDot` DB status which IS live), or implement real health checks. Requires a design decision.

### 5. `NotificationsButton` — no handler
- **File:** `Views/MainWindow.xaml` ~line 803
- **Problem:** Bell icon button in the top bar has no `Click` handler. Likely a placeholder for a future notifications panel.
- **Fix needed:** Either wire it to a notification flyout/panel when built, or add a `// TODO` comment and leave it. Low priority.

---

## Inventory.xaml + Inventory.xaml.cs

### 6. Dead `LoadCommand` property
- **File:** `ViewModels/InventoryViewModel.cs` line 58
- **Problem:** `LoadCommand` (`RelayCommand`) is declared and initialized but **never called or bound anywhere**. `Inventory.xaml.cs` calls `_vm.Load()` directly; no XAML binding references `LoadCommand`.
- **Fix needed (choose one):**
  - **Remove it** — delete `LoadCommand` property and the `new RelayCommand(Load)` line in the constructor (cleanest).
  - **Use it** — add a Refresh button to `Inventory.xaml` bound to `{Binding LoadCommand}`, removing the code-behind call to `_vm.Load()` in `Refresh()`.

### 7. Duplicated inline DataGrid styles (deferred to UI/UX pass)
- **File:** `Views/Inventory.xaml` ~lines 49-84 and 118-148
- **Problem:** Both `DgInventoryLog` and `DgLowStock` define identical `ColumnHeaderStyle`, `RowStyle`, and `CellStyle` inline. ~70 lines of duplication.
- **Fix needed:** Extract to a shared style key in `AppStyles.xaml`, then reference with `Style="{StaticResource ...}"` on both DataGrids. **Deferred until the UI/UX pass.**

### 8. `BtnAdjust` — fully inline styles (deferred to UI/UX pass)
- **File:** `Views/Inventory.xaml` ~line 225
- **Problem:** The Apply Adjustment button sets `Background`, `Foreground`, `BorderThickness`, `Cursor`, `FontFamily`, `FontSize`, `FontWeight` all inline. Does not reference the `PrimaryButton` style from `AppStyles.xaml`.
- **Fix needed:** Replace inline properties with `Style="{StaticResource PrimaryButton}"`. **Deferred until the UI/UX pass.**

---

## Quick-start prompt for next session

```
Continue the code-quality-suite work on OOP-TaurusBikeShop AdminSystem.

Pending items to fix:

MAINWINDOW:
1. Wire PaymentVerificationNavButton + OrderHistoryNavButton (Views/MainWindow.xaml ~598, 660)
2. Populate AdminRoleTextBlock from App.CurrentUser (Views/MainWindow.xaml.cs ~62)
3. Set PendingOrdersBadgeTextBlock, PaymentVerificationBadgeTextBlock, SupportTicketsBadgeTextBlock
   dynamically in LoadActivityFeed() (Views/MainWindow.xaml.cs ~194)
4. Decide on fake system status rows (MainWindow.xaml ~926)

INVENTORY:
5. Remove dead LoadCommand from InventoryViewModel (ViewModels/InventoryViewModel.cs:58)
6-8. UI/UX pass: extract duplicate DataGrid styles + BtnAdjust inline styles to AppStyles.xaml (deferred)
```
