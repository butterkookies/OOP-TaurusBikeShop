# AdminSystem_v2 — Security Remediation Plan

Generated: 2026-04-08 | Last updated: 2026-04-09 | Status: All steps complete (manual tests remain)
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

### Step 3 — Verify Login Filters to Staff-Only Accounts (VERIFY-001) ✅ DONE 2026-04-09

- [x] **3a.** Inspected `AuthService.LoginAsync` — `FindByEmailAsync` had no role filter; any active user (including customers) could authenticate.
- [x] **3b.** Added staff-only role check in `AuthService.LoginAsync`: after password verification, fetches the user's role via `GetUserRoleAsync` and rejects if the role is not in `{Admin, Manager, Staff}`.
- [x] **3c.** Roles are in a separate `UserRole`/`Role` table — the existing `GetUserRoleAsync` join handles this correctly; the new check uses its result.

---

### Step 4 — Add Role Guard to Staff Navigation (VULN-002) ✅ DONE 2026-04-09

- [x] **4a.** Added `if (page == PageNames.Staff && !IsAdmin) return;` guard at top of `Navigate()` in `MainWindowViewModel.cs`.
- [x] **4b.** Added `public bool IsAdmin => UserRole == RoleNames.Admin;` property to `MainWindowViewModel`. Staff nav button in `MainWindow.xaml` now has `Visibility="{Binding IsAdmin, Converter={StaticResource BoolToVis}}"`.
- [ ] **4c.** Manual test: log in as a non-Admin Staff user — confirm the Staff button is hidden and the route is blocked.

---

### Step 5 — Add Role Guards to Staff Service Operations (VULN-002) ✅ DONE 2026-04-09

- [x] **5a.** Added `callerRole` parameter and `RequireAdmin()` check to all four sensitive methods in `UserService.cs`: `CreateStaffAsync`, `SetUserRoleAsync`, `ResetPasswordAsync`, `ToggleActiveAsync`. Throws `UnauthorizedAccessException` if caller is not Admin.
- [x] **5b.** Updated `IUserService` interface signatures. Updated all callers in `StaffViewModel.cs` to pass `App.CurrentUser?.Role`.
- [ ] **5c.** Manual test: invoke each operation from a Staff-role session and confirm `UnauthorizedAccessException` is thrown.

---

### Step 6 — Implement Login Brute-Force Protection (VULN-003) ✅ DONE 2026-04-09

- [x] **6a.** SQL migration `Migrations/001_AddLoginLockoutColumns.sql` adds `FailedLoginAttempts` (INT, default 0) and `LockoutUntil` (DATETIME, nullable) to `[User]`. Fields also added to `User.cs` model.
- [x] **6b.** `AuthService.LoginAsync` increments `FailedLoginAttempts` on wrong password via `IncrementFailedLoginsAsync`. At 5 failures, `LockoutUntil` is set to GETUTCDATE()+15 minutes (all in one atomic SQL UPDATE).
- [x] **6c.** Lockout check runs before password verification — returns "Account locked. Try again later." if `LockoutUntil > UtcNow`.
- [x] **6d.** On successful login, `ResetFailedLoginsAsync` clears `FailedLoginAttempts` and `LockoutUntil`.
- [x] **6e.** `LoginAsync` return type changed from `User?` to `LoginResult` (success/fail + error message). `IAuthService`, `LoginViewModel` updated accordingly. UI now shows specific lockout vs invalid-credentials messages.
- [ ] **6f.** Manual test:
  - Submit 5 wrong passwords → confirm account locks.
  - Wait for lockout to expire → confirm login succeeds with correct password.
  - Correct password after lock expires → confirm attempt counter resets.
- **Note:** Run `Migrations/001_AddLoginLockoutColumns.sql` against the database before testing.

---

### Step 7 — Final Verification Pass ✅ DONE 2026-04-09

- [ ] **7a.** Manual test: Run the app as each role (Admin, Staff) and walk every nav route. Confirm no unauthorized screens are reachable.
- [x] **7b.** Verified: `appsettings.json` (AdminSystem_v2 and WebApplication) contains only `SET_VIA_USER_SECRETS_OR_ENVIRONMENT_VARIABLE` placeholders. No real credentials in working directory or `bin/` output.
- [x] **7c.** Verified: `git log --all -p -- "*.json" | grep Password` shows only `REDACTED` and `<PASSWORD>` placeholder tokens — no real credentials in history.
- [x] **7d.** Firewall rules were scoped in Step 1d (new Cloud SQL instance).
- [x] **7e.** Created `SECRETS.md` at repo root documenting: User Secrets setup for both projects, environment variable names for prod, config loading priority, and contributor rules.

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
