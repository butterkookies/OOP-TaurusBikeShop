# Secrets Management Guide

This project uses **.NET User Secrets** for local development and **environment variables** for production/CI.

> **Never commit real credentials to `appsettings.json` or any tracked file.**

---

## Local Development (User Secrets)

Each project has its own User Secrets store, identified by the `<UserSecretsId>` in its `.csproj`:

| Project | UserSecretsId |
|---------|---------------|
| AdminSystem_v2 | `adminsystem-v2-taurus` |
| WebApplication | `fbe64185-b3a5-41ff-9715-6b03fc7c1472` |

### Setting secrets

From the project directory, run:

```bash
# Connection string (both projects need this)
dotnet user-secrets set "ConnectionStrings:Taurus-bike-shop-sqlserver-2026" \
  "Server=<HOST>,1433;Database=Taurus-bike-shop-sqlserver-2026;User Id=<USER>;Password=<PASSWORD>;TrustServerCertificate=True;MultipleActiveResultSets=True;"

# Cloudinary (WebApplication only)
dotnet user-secrets set "Cloudinary:ApiKey"    "<YOUR_API_KEY>"
dotnet user-secrets set "Cloudinary:ApiSecret" "<YOUR_API_SECRET>"
```

### Viewing current secrets

```bash
dotnet user-secrets list
```

Secrets are stored at `%APPDATA%\Microsoft\UserSecrets\<UserSecretsId>\secrets.json`.

---

## Production / CI (Environment Variables)

Set these environment variables on your deployment target:

| Variable | Description |
|----------|-------------|
| `ConnectionStrings__Taurus-bike-shop-sqlserver-2026` | SQL Server connection string |
| `Cloudinary__ApiKey` | Cloudinary API key (WebApplication only) |
| `Cloudinary__ApiSecret` | Cloudinary API secret (WebApplication only) |

> Note the double underscore (`__`) — this is .NET's convention for nested config keys in environment variables.

---

## How it works

`DatabaseHelper.cs` loads configuration in this priority order (last wins):

1. `appsettings.json` (placeholder values checked into source)
2. User Secrets (local dev only, never committed)
3. Environment Variables (production override)

---

## Rules for contributors

1. **Never** put real passwords, API keys, or IP addresses in any tracked file.
2. `appsettings.json` should only contain `SET_VIA_USER_SECRETS_OR_ENVIRONMENT_VARIABLE` placeholders.
3. If you add a new secret, update this document and the placeholder in `appsettings.json`.
4. If credentials are accidentally committed, rotate them immediately and purge git history with `git filter-repo`.
