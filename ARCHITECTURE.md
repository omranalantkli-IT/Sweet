# Project structure

This is a pragmatic, single-project layered backend and a feature-based React frontend. It does not introduce separate assemblies or a generic repository abstraction on top of Entity Framework.

## Backend

```text
backend/
  Api/Controllers/              HTTP routes, authorization and response mapping
  Application/Contracts/        Request and response DTOs
  Application/Services/         Authentication, workers, products, production and reporting
  Domain/Entities/              User, Product and WorkEntry entities
  Infrastructure/
    Authentication/             JWT token creation
    Persistence/                EF Core context, mappings and initial seed
  Program.cs                    Dependency injection and middleware
```

Controllers delegate database operations to feature services. Services use the EF Core context; this is intentionally layered organization, not strict dependency-inverted Clean Architecture. Entities and DTOs do not depend on controllers. Existing HTTP paths, role restrictions and database tables remain unchanged.

## Frontend

```text
frontend/src/
  app/                          Routes and global providers
  features/
    auth/                       Login, auth context and route protection
    dashboard/                  Admin overview
    products/                   Product management
    workers/                    Worker management
    production/                 Entry history, worker dashboard and shared entry table
    reports/                    Monthly financial reports
  shared/
    api/                        HTTP client and token handling
    components/                 Error boundary
    layout/                     Application shell
    ui/                         Shared presentation components and formatting
  assets/                       Brand assets
  styles.css                    Existing global theme and responsive rules
  main.jsx                      React bootstrap
```

Place new feature-specific pages and components inside their feature. Promote a component to `shared` only when unrelated features need it. Pages must not import reusable components from another page; production tables live in `features/production/components`.

## Validation

```powershell
dotnet build backend/backend.csproj
npm --prefix frontend run build
```

Database connection settings stay separate from the architecture. LocalDB connectivity issues must be resolved independently; do not persist machine-specific named-pipe addresses in Git.
