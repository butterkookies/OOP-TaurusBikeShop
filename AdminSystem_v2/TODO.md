# AdminSystem_v2 — Security Remediation Plan

Generated: 2026-04-08 | Last updated: 2026-04-09
Source: Internal security review of AdminSystem_v2 codebase.

---

## Overview of Findings

| ID | Severity | Summary |
|----|----------|---------|
| VULN-001 | **Critical** | Hardcoded DB credentials + public IP in `appsettings.json` |
| VULN-002 | **High** | Vertical privilege escalation — Staff page accessible to any role |
| VULN-003 | **High** | No brute-force protection on login (`AuthService.LoginAsync`) |
| VERIFY-001 | **Medium** | Login query may allow non-staff (customer) accounts to authenticate |

---

## Remediation Tasks (Chronological Order)

---

### Step 1 — Rotate Compromised Credentials (VULN-001) ✅ DONE 2026-04-09

- [x] **1a.** Created a new Google Cloud SQL Server instance with a new login and strong password.
- [x] **1b.** New login granted minimum required permissions only.
- [x] **1c.** Old instance decommissioned (new instance replaces it entirely).
- [x] **1d.** Firewall rules scoped to known IP ranges on new instance.

---

### Step 2 — Remove Credentials from Source Code (VULN-001) ✅ DONE 2026-04-09

- [x] **2a.** Connection strings moved to .NET User Secrets (dev) for both AdminSystem_v2 and WebApplication. Cloudinary ApiKey + ApiSecret also moved to User Secrets.
- [x] **2b.** All `appsettings.json` and `appsettings.Development.json` files updated to `SET_VIA_USER_SECRETS_OR_ENVIRONMENT_VARIABLE` placeholders.
- [x] **2c.** `appsettings.Development.json` already in `.gitignore`. Confirmed no `.env` files present.
- [x] **2d.** Git history purged with `git filter-repo` (3 passes). The following were scrubbed from all commits:
  - Old DB passwords (`oop_db_bsit22A`, `oop_db_bsitt22A`)
  - Old DB server IPs (`34.81.116.25`, `35.221.161.150`)
  - New DB server IP (`34.87.100.66`)
  - Cloudinary ApiSecrets (`8zVyPr3g...`, `TUpmzr4V...`)
  - Cloudinary ApiKey (`342386339...`)
  - History force-pushed to `origin/main`. All SHAs rewritten.
- [x] **2e.** `DatabaseHelper.cs` updated to load config via `AddUserSecrets<DatabaseHelper>()` + `AddEnvironmentVariables()` in addition to `appsettings.json`. Dead config builder code removed from `App.xaml.cs`.
- [ ] **2f.** Change `TrustServerCertificate=True` to `False` once a valid TLS certificate is in place on the new Cloud SQL instance.

---

### Step 3 — Verify Login Filters to Staff-Only Accounts (VERIFY-001)

Do this before adding role guards so you know the auth baseline is sound.

- [ ] **3a.** Open `AuthService.LoginAsync` and inspect the SQL query that fetches the user by email.
- [ ] **3b.** Confirm the query includes a role/type filter (e.g., `WHERE Role IN ('Admin','Staff')`). If it does not:
  - [ ] Add the filter so customer accounts cannot log into the admin system.
  - [ ] Manual test: attempt login with a known customer-only account and confirm it is rejected.
- [ ] **3c.** If roles are stored in a separate `UserRole` table, adjust the join/filter accordingly.

---

### Step 4 — Add Role Guard to Staff Navigation (VULN-002)

Prevent non-Admin users from reaching Staff management screens.

- [ ] **4a.** In `MainWindowViewModel.cs`, add a role check inside `Navigate()` before routing to `_staffVm`:
  ```csharp
  case "Staff":
      if (_currentUser?.Role != "Admin") return;
      CurrentView = _staffVm;
      break;
  ```
- [ ] **4b.** In `MainWindow.xaml`, bind the Staff nav button's `Visibility` to an `IsAdmin` property:
  ```xml
  <Button Content="Staff"
          Visibility="{Binding IsAdmin, Converter={StaticResource BoolToVis}}"/>
  ```
  Add `public bool IsAdmin => CurrentUser?.Role == "Admin";` to `MainWindowViewModel`.
- [ ] **4c.** Test: log in as a non-Admin Staff user — confirm the Staff button is hidden and the route is blocked.

---

### Step 5 — Add Role Guards to Staff Service Operations (VULN-002)

The navigation guard stops the UI, but the service layer must also enforce authorization.

- [ ] **5a.** In `UserService.cs`, add a caller role check at the top of each sensitive method:
  - `CreateStaffAsync` — require `callerRole == "Admin"`
  - `SetUserRoleAsync` — require `callerRole == "Admin"`
  - `ResetPasswordAsync` — require `callerRole == "Admin"`
  - `ToggleActiveAsync` — require `callerRole == "Admin"`

  Example:
  ```csharp
  public async Task CreateStaffAsync(UserModel newUser, string callerRole)
  {
      if (callerRole != "Admin")
          throw new UnauthorizedAccessException("Only admins can create staff accounts.");
      // ... existing logic
  }
  ```
- [ ] **5b.** Update all callers of these methods to pass the current session user's role.
- [ ] **5c.** Test: invoke each operation from a Staff-role session and confirm `UnauthorizedAccessException` is thrown.

---

### Step 6 — Implement Login Brute-Force Protection (VULN-003)

- [ ] **6a.** Add `FailedLoginAttempts` (int, default 0) and `LockoutUntil` (DateTime?, nullable) columns to the staff/users table.
- [ ] **6b.** In `AuthService.LoginAsync`, after a failed password check:
  - Increment `FailedLoginAttempts` for that account.
  - If `FailedLoginAttempts >= 5`, set `LockoutUntil = GETUTCDATE() + 15 minutes`.
- [ ] **6c.** At the start of `LoginAsync`, before checking the password:
  - If `LockoutUntil IS NOT NULL AND LockoutUntil > GETUTCDATE()`, reject immediately with "Account locked. Try again later."
- [ ] **6d.** On successful login, reset `FailedLoginAttempts = 0` and `LockoutUntil = NULL`.
- [ ] **6e.** Optional: add a `LoginAuditLog` table (timestamp, email, success/failure) for incident review.
- [ ] **6f.** Test:
  - Submit 5 wrong passwords → confirm account locks.
  - Wait for lockout to expire → confirm login succeeds with correct password.
  - Correct password after lock expires → confirm attempt counter resets.

---

### Step 7 — Final Verification Pass

- [ ] **7a.** Run the app as each role (Admin, Staff) and walk every nav route. Confirm no unauthorized screens are reachable.
- [ ] **7b.** Confirm `appsettings.json` in the working directory and in any published output contains no real credentials.
- [ ] **7c.** Confirm git log no longer exposes any credentials (`git log --all -p -- "*.json" | grep Password`).
- [ ] **7d.** Confirm the database server's firewall allows only expected IP ranges.
- [ ] **7e.** Document the secrets management approach (User Secrets for dev, environment variable name for prod) so future developers don't re-introduce hardcoded credentials.

---

## Reference — Affected Files

| File | Steps |
|------|-------|
| `appsettings.json` (both projects) | 1, 2 |
| `appsettings.Development.json` (WebApplication) | 2 |
| `AdminSystem_v2.csproj` | 2 |
| `Helpers/DatabaseHelper.cs` | 2 |
| `App.xaml.cs` | 2 |
| `Services/AuthService.cs` | 3, 6 |
| `Services/UserService.cs` | 5 |
| `ViewModels/MainWindowViewModel.cs` | 4a, 4b |
| `Views/MainWindow.xaml` | 4b |
| Database schema (Users/staff table) | 6a |
