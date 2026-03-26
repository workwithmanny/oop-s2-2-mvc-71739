# Food Safety Inspection Tracker

ASP.NET Core MVC app for tracking food premises inspections, follow-ups, and management reporting.
Built for the Modern Programming Principles and Practice assessment with Serilog logging, role-based access, EF Core + SQLite, and CI.

## Key Features
- Premises, inspections, and follow-ups with relationships.
- Dashboard aggregations:
  - Inspections this month
  - Failed inspections this month
  - Open follow-ups
  - Overdue open follow-ups
  - Filter by Town and Risk Rating
- Role-based access:
  - Admin: full access
  - Inspector: create/edit/delete inspections and follow-ups
  - Viewer: read-only access
- Serilog logging with console + rolling file sink and enriched properties.
- Global error handling with a friendly error page.
- Seed data for realistic reporting.
- xUnit tests and GitHub Actions CI.

## Tech Stack
- ASP.NET Core MVC
- EF Core + SQLite
- Identity + Roles
- Serilog
- xUnit
- GitHub Actions

## Getting Started
1. Restore and build:
   ```bash
   dotnet restore
   dotnet build
   ```
2. Run the app:
   ```bash
   dotnet run --project oop-s2-2-mvc-71739
   ```
3. Open the app and navigate to:
   - `/Dashboard` for reporting
   - `/Premises`, `/Inspections`, `/FollowUps` for CRUD

## Seed Users
These users are created on startup if they do not exist:
- Admin: `admin@foodsafety.local` / `Pass123$`
- Inspector: `inspector@foodsafety.local` / `Pass123$`
- Viewer: `viewer@foodsafety.local` / `Pass123$`

## Data Seeding
The database is seeded with:
- 12 premises across 3 towns
- 25 inspections across multiple months
- 10 follow-ups (open, closed, and overdue)

## Logging
Serilog is configured with:
- Console sink
- Rolling file sink (daily) at `Logs/log-YYYYMMDD.txt`
- Enriched properties: `Application`, `Environment`, `UserName`

Examples of logged events:
- Create/update/delete actions
- Validation warnings (e.g., due date before inspection date)
- Exceptions and concurrency errors

## Tests
Run tests:
```bash
dotnet test --configuration Release
```

Coverage includes:
- Overdue follow-up query behavior
- Dashboard monthly counts
- Follow-up closing rule
- Role name separation

## CI
GitHub Actions workflow builds and tests on pushes and PRs to `main`.

## Project Structure
- `oop-s2-2-mvc-71739`: main MVC app
- `oop-s2-2-mvc-71739.Tests`: xUnit tests

