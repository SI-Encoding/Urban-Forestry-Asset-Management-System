# UFAMS Claude Code Instructions

## Project Overview

UFAMS is a GIS-enabled tree asset management system designed to demonstrate practical asset management, GIS integration, backend architecture, frontend development, and database design.

The application consists of:

- .NET backend
- Next.js frontend
- SQL Server database
- Entity Framework Core
- ArcGIS Feature Service integration
- ArcGIS OAuth authentication
- ArcGIS synchronization and audit functionality

## Repository Structure

```text
UFAMS/
├── backend/
│   ├── UFAMS.sln
│   ├── src/
│   │   ├── UFAMS.Api/
│   │   ├── UFAMS.Application/
│   │   ├── UFAMS.Domain/
│   │   └── UFAMS.Infrastructure/
│   └── tests/
│
├── frontend/
│   ├── app/
│   ├── components/
│   ├── lib/
│   └── package.json
│
└── docs/
```

## Architecture

UFAMS follows a layered architecture:

```text
API
 │
 ▼
Application
 │
 ▼
Domain
 ▲
 │
Infrastructure
```

The API layer handles HTTP concerns.

The Application layer coordinates use cases.

The Domain layer contains business concepts and business rules.

The Infrastructure layer implements persistence and external integrations.

Keep these responsibilities separate.

## Important Rule

Before modifying code, inspect the existing implementation.

Do not assume that a feature is missing simply because it is not visible in one file.

Search the repository for:

- Existing endpoints.
- Commands.
- Queries.
- Handlers.
- Services.
- Repository interfaces.
- Repository implementations.
- Domain entities.
- EF Core configurations.
- Migrations.
- Frontend API clients.
- Existing UI components.
- Existing tests.

Reuse existing functionality whenever possible.

## Domain Model

Important domain entities include:

- `Tree`
- `Species`
- `Park`
- `Employee`
- `Inspection`
- `WorkOrder`

Important value objects and enums include:

- `GeoCoordinate`
- `TreeHealthStatus`
- `WorkOrderStatus`

Business rules should not be placed directly inside API endpoints.

## API Guidelines

Keep API endpoints thin.

Endpoints should primarily:

1. Receive HTTP requests.
2. Bind route, query, and body parameters.
3. Construct commands or queries.
4. Invoke application handlers or services.
5. Return appropriate HTTP responses.

Avoid putting domain/business logic directly into endpoint classes.

When adding or modifying an endpoint, check whether the corresponding Application-layer abstraction already exists.

## Application Guidelines

The Application layer contains use cases and abstractions.

Prefer existing patterns for:

- Commands.
- Queries.
- Handlers.
- Services.
- Repository interfaces.
- Unit-of-work interfaces.

Do not introduce a second pattern for functionality that can follow an existing pattern.

## Domain Guidelines

The Domain layer should remain independent from infrastructure technologies.

Do not introduce dependencies on:

- ASP.NET Core.
- Entity Framework Core.
- SQL Server.
- ArcGIS SDKs.
- HTTP.
- Next.js.
- React.

Domain logic should remain testable without requiring external infrastructure.

## Infrastructure Guidelines

Infrastructure contains technical implementations such as:

- EF Core.
- SQL Server.
- Repository implementations.
- Database initialization.
- Database seeding.
- ArcGIS authentication.
- ArcGIS Feature Service access.
- ArcGIS synchronization.

Infrastructure may depend on Application and Domain abstractions where appropriate.

## Database Changes

UFAMS uses Entity Framework Core migrations.

When modifying the persistence model:

1. Update the appropriate model/configuration.
2. Create an EF Core migration.
3. Review the generated migration.
4. Apply the migration locally.
5. Verify the resulting database behavior.
6. Keep the migration in source control.

Example:

```bash
dotnet ef migrations add <MigrationName> \
  --project src/UFAMS.Infrastructure \
  --startup-project src/UFAMS.Api
```

Do not manually modify database schema when an EF migration is appropriate.

## ArcGIS Integration

ArcGIS is a core part of UFAMS.

Existing functionality includes:

- ArcGIS authentication.
- ArcGIS OAuth.
- Feature Service information.
- Feature retrieval.
- Layer information.
- Synchronization preview.
- Synchronization application.
- Single-asset synchronization.
- Synchronization audit history.

Preserve the separation between ArcGIS infrastructure and UFAMS domain logic.

### Security

Never hard-code or commit:

- ArcGIS client secrets.
- OAuth tokens.
- Access tokens.
- API keys.
- Database credentials.
- Passwords.
- Secrets contained in configuration files.

Environment-specific values should be supplied through configuration or environment variables.

## Synchronization

UFAMS supports previewing and applying ArcGIS synchronization.

Keep these operations distinct:

```text
Preview
   │
   ▼
Calculate synchronization changes
   │
   ▼
Return result
```

and:

```text
Apply
   │
   ▼
Calculate synchronization changes
   │
   ▼
Modify UFAMS
   │
   ▼
Record audit information
```

Preview operations should not unexpectedly modify the database.

Synchronization should remain auditable.

## Frontend

The frontend uses Next.js.

When modifying frontend functionality:

- Reuse existing components.
- Reuse existing API utilities.
- Preserve the existing navigation structure.
- Avoid unnecessary dependencies.
- Keep API communication separate from UI presentation.
- Ensure API changes remain compatible with the frontend.

Do not rewrite existing UI architecture unless the task requires it.

## Testing

The project uses:

- Vitest for unit tests.
- Playwright for end-to-end testing.

Prefer Playwright for browser-level testing.

When modifying backend functionality, run the relevant backend tests when available.

When modifying frontend functionality, run the relevant frontend tests when available.

## Build Verification

Backend:

```bash
cd backend
dotnet restore
dotnet build
```

Frontend:

```bash
cd frontend
npm install
npm run build
```

Do not claim a task is complete if the affected project no longer builds.

## Documentation

Documentation is located under `docs/`.

Important files include:

- `API.md`
- `ARCHITECTURE.md`
- `DATABASE.md`
- `DECISIONLOG.md`
- `DEVELOPMENT.md`
- `AGENTS.md`

When implementation behavior changes, determine whether the relevant documentation also needs to be updated.

Documentation should describe the current implementation.

Do not document planned functionality as though it already exists.

## Change Scope

Keep changes focused.

If the requested task is small, do not automatically refactor unrelated code.

Avoid:

- Unrelated dependency upgrades.
- Large architectural rewrites.
- Renaming unrelated classes.
- Reformatting unrelated files.
- Replacing working implementations without a requirement.
- Modifying unrelated frontend or backend functionality.

Prefer the smallest change that correctly solves the requested problem.

## Existing Changes

Before modifying files, run:

```bash
git status
```

Review existing modifications before making additional changes.

Do not discard existing user changes unless explicitly instructed to do so.

Do not reset, checkout, or clean unrelated changes without confirmation.

## Debugging Process

When something does not work:

1. Reproduce the problem.
2. Inspect the relevant implementation.
3. Trace the request through the application layers.
4. Identify the actual failure.
5. Make the smallest appropriate fix.
6. Build or test the affected project.
7. Re-check the original behavior.

Avoid making multiple speculative changes at once.

## API Changes

When changing an API endpoint, check all affected layers:

```text
Frontend
   │
   ▼
API Endpoint
   │
   ▼
Application Handler/Service
   │
   ▼
Domain
   │
   ▼
Repository
   │
   ▼
SQL Server
```

For ArcGIS functionality:

```text
Frontend
   │
   ▼
ArcGIS Endpoint
   │
   ▼
ArcGIS Application/Service Layer
   │
   ├── ArcGIS Feature Service
   │
   └── UFAMS repositories
              │
              ▼
          SQL Server
```

Verify the entire affected flow when practical.

## Code Style

Follow the existing style of the repository.

Prefer:

- Clear names.
- Small focused methods.
- Existing project conventions.
- Existing dependency injection patterns.
- Existing error-handling patterns.
- Existing DTO/command/query patterns.

Avoid introducing abstractions without a clear reason.

## Completion Checklist

Before reporting a task as complete:

- [ ] The requested behavior works.
- [ ] The appropriate architectural layer was changed.
- [ ] Existing functionality was preserved.
- [ ] No unrelated files were changed.
- [ ] Backend builds successfully when affected.
- [ ] Frontend builds successfully when affected.
- [ ] Relevant tests pass.
- [ ] Database migrations were created when required.
- [ ] Documentation was updated when required.
- [ ] No secrets were introduced.
- [ ] `git status` was reviewed.

## Guiding Principle

When working on UFAMS, prioritize:

```text
Understand
   ↓
Inspect
   ↓
Reuse
   ↓
Modify
   ↓
Verify
   ↓
Document
```

Preserve the existing architecture unless there is a clear reason to change it.