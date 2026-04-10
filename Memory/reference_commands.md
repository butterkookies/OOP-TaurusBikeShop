# Reference Commands

> Every command that matters for this project. Inferred from .csproj files,
> package.json, SECRETS.md, and README. Verified where possible.

## Development

### WebApplication (ASP.NET Core MVC)
```bash
# Build
cd WebApplication
dotnet build

# Run (development mode)
dotnet run

# Run with watch (auto-reload on file changes)
dotnet watch run
```

### AdminSystem_v2 (WPF Desktop)
```bash
# Build
cd AdminSystem_v2
dotnet build

# Run
dotnet run
```

### Full Solution
```bash
# Build entire solution (both projects)
dotnet build OOP-TaurusBikeShop.sln
```

## Secrets Management

### Set connection string (both projects need this)
```bash
# WebApplication
cd WebApplication
dotnet user-secrets set "ConnectionStrings:Taurus-bike-shop-sqlserver-2026" \
  "Server=<HOST>,1433;Database=Taurus-bike-shop-sqlserver-2026;User Id=<USER>;Password=<PASSWORD>;TrustServerCertificate=True;MultipleActiveResultSets=True;"

# AdminSystem_v2
cd AdminSystem_v2
dotnet user-secrets set "ConnectionStrings:Taurus-bike-shop-sqlserver-2026" \
  "Server=<HOST>,1433;Database=Taurus-bike-shop-sqlserver-2026;User Id=<USER>;Password=<PASSWORD>;TrustServerCertificate=True;MultipleActiveResultSets=True;"
```

### Set Cloudinary keys (WebApplication only)
```bash
cd WebApplication
dotnet user-secrets set "Cloudinary:ApiKey" "<YOUR_API_KEY>"
dotnet user-secrets set "Cloudinary:ApiSecret" "<YOUR_API_SECRET>"
```

### View current secrets
```bash
dotnet user-secrets list
```

## Database

### EF Core Migrations (WebApplication only)
```bash
cd WebApplication

# Add a new migration
dotnet ef migrations add <MigrationName>

# Apply pending migrations
dotnet ef database update

# Generate SQL script from migrations
dotnet ef migrations script
```

> Note: AdminSystem_v2 uses Dapper with raw SQL — no EF Core migrations.

### SQL Schema Reference
- Full DDL: `SQL/Schema/Taurus_schema_8.0.sql` (SSMS export, UTF-16 encoded)
- Seed data: `SQL/Seed/Taurus_seed_v7.1.sql`

## Frontend Tooling

### Mermaid Diagrams
```bash
# Render Mermaid diagrams (requires node_modules)
npx mmdc -i input.mmd -o output.svg
```

### Tailwind CSS (if using PostCSS pipeline)
```bash
npx tailwindcss -o output.css
```

## Git

```bash
# Standard workflow
git status
git add <files>
git commit -m "message"
git push origin main

# Check remote branches
git branch -a
```

## Testing
> No test framework is configured. package.json test script is a stub.
> No .NET test project exists in the solution.

## Deployment
> No deployment pipeline is configured.
> No Dockerfile, no CI/CD workflows, no cloud deploy config found.
