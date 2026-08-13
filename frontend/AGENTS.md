# UFAMS Agent Guidelines

## Purpose

This document provides guidance for AI coding agents working on the UFAMS repository.

Agents should understand the project's architecture, preserve existing design decisions, and avoid making unnecessary changes outside the scope of the requested task.

## Project Overview

UFAMS is a GIS-enabled tree asset management system.

The system consists of:

- A .NET backend.
- A Next.js frontend.
- SQL Server persistence.
- Entity Framework Core.
- ArcGIS Feature Service integration.
- ArcGIS authentication and synchronization.
- GIS-oriented tree asset management workflows.

The project is intended to demonstrate practical GIS, asset management, backend, frontend, database, and integration architecture.

## Repository Structure

The repository is organized approximately as follows:

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
    ├── API.md
    ├── ARCHITECTURE.md
    ├── DATABASE.md
    ├── DECISIONLOG.md
    ├── DEVELOPMENT.md
    └── AGENTS.md
```

## Architecture Guidelines

UFAMS follows a layered architecture.

### API Layer

The API layer handles HTTP concerns.

Agents should keep endpoint implementations thin and avoid placing business rules directly inside endpoints.

### Application Layer

The Application layer contains application use cases.

This includes:

- Commands.
- Queries.
- Command handlers.
- Query handlers.
- Application services.
- Repository abstractions.
- Unit-of-work abstractions.

### Domain Layer

The Domain layer contains the core business model and business rules.

Agents should avoid introducing infrastructure dependencies into the Domain layer.

The Domain layer should not depend on:

- Entity Framework Core.
- SQL Server.
- ArcGIS SDKs.
- ASP.NET Core.
- HTTP.
- Frontend frameworks.

### Infrastructure Layer

The Infrastructure layer contains implementations for persistence and external integrations.

This includes:

- Entity Framework Core.
- SQL Server.
- Repository implementations.
- Database initialization.
- Database seeding.
- ArcGIS authentication.
- ArcGIS Feature Service access.
- ArcGIS synchronization.

## General Development Rules

Before making changes, agents should:

1. Understand the existing implementation.
2. Identify the appropriate architectural layer.
3. Check for existing abstractions that can be reused.
4. Avoid duplicating existing functionality.
5. Make the smallest reasonable change that satisfies the requirement.
6. Preserve existing behavior unless the task explicitly requires changing it.
7. Update documentation when a change materially affects system behavior or architecture.

Agents should not restructure unrelated parts of the application simply because another design might be preferred.

## Domain Model Guidelines

Important UFAMS domain concepts include:

- `Tree`
- `Species`
- `Park`
- `Employee`
- `Inspection`
- `WorkOrder`
- `GeoCoordinate`
- `TreeHealthStatus`
- `WorkOrderStatus`

Business rules should generally be implemented within domain entities or appropriate domain/application services rather than directly inside API endpoints.

## Database Guidelines

UFAMS uses Entity Framework Core with SQL Server.

When changing the persistence model:

1. Update the domain or persistence configuration as required.
2. Create an EF Core migration.
3. Review the generated migration.
4. Apply the migration during development.
5. Verify the resulting database schema.
6. Commit the migration to source control.

Do not manually modify the database schema when an EF Core migration should be used.

Example migration command:

```bash
dotnet ef migrations add <MigrationName> \
  --project src/UFAMS.Infrastructure \
  --startup-project src/UFAMS.Api
```

## ArcGIS Guidelines

ArcGIS integration is an important part of UFAMS.

Agents should preserve the separation between UFAMS business logic and ArcGIS infrastructure.

ArcGIS functionality includes:

- Authentication.
- OAuth.
- Feature Service access.
- Feature retrieval.
- Layer information.
- Synchronization preview.
- Synchronization application.
- Single-asset synchronization.
- Synchronization audit history.

ArcGIS credentials, access tokens, and other sensitive authentication information must not be hard-coded or committed to source control.

## Synchronization Guidelines

ArcGIS synchronization should preserve the distinction between:

- Previewing changes.
- Applying changes.
- Applying synchronization to a single asset.
- Recording synchronization audit information.

Preview operations should not unexpectedly modify the UFAMS database.

Synchronization changes should remain traceable through the audit system.

## API Guidelines

API endpoints should:

- Validate HTTP input.
- Bind route, query, and body parameters.
- Construct commands or queries.
- Invoke application handlers or services.
- Return appropriate HTTP status codes.

Business logic should not be unnecessarily duplicated between endpoints.

When adding an API endpoint, update `docs/API.md` if the endpoint becomes part of the documented public application interface.

## Frontend Guidelines

The frontend uses Next.js.

When modifying frontend functionality:

- Reuse existing components where practical.
- Keep API communication separate from presentation logic.
- Preserve the existing application navigation.
- Avoid introducing unnecessary dependencies.
- Ensure frontend changes remain compatible with the backend API.

## Testing Guidelines

Agents should verify changes whenever practical.

For backend changes:

```bash
dotnet build
```

For frontend changes:

```bash
npm run build
```

When tests exist for the affected functionality, agents should run the relevant tests.

The project uses Vitest for unit testing and Playwright for end-to-end testing.

## Documentation Guidelines

Documentation is maintained under `docs/`.

Important documentation files include:

- `API.md` — REST API endpoints and behavior.
- `ARCHITECTURE.md` — application architecture and separation of concerns.
- `DATABASE.md` — database architecture, Entity Framework Core, and migrations.
- `DECISIONLOG.md` — important architectural and technical decisions.
- `DEVELOPMENT.md` — development environment and setup instructions.
- `AGENTS.md` — guidance for AI agents working on the repository.

Documentation should reflect the actual implementation rather than an intended future architecture.

## Change Scope

Agents should avoid unrelated modifications.

For example, if asked to modify an ArcGIS synchronization endpoint, an agent should not automatically:

- Rewrite the database layer.
- Replace the frontend framework.
- Rename unrelated domain entities.
- Change authentication architecture.
- Upgrade unrelated dependencies.

Changes should remain focused on the requested functionality.

## Security

Agents must not commit:

- Passwords.
- API keys.
- OAuth client secrets.
- Access tokens.
- Database credentials.
- Connection strings containing credentials.
- Other sensitive authentication information.

Configuration values that vary by environment should use appropriate configuration or environment variables.

## Git Guidelines

Before committing changes, agents should inspect the working tree:

```bash
git status
```

Agents should review modified files and avoid accidentally committing unrelated changes.

When possible, commits should represent a coherent change rather than a collection of unrelated modifications.

## Verification Checklist

Before considering a development task complete, agents should verify:

- [ ] The requested functionality has been implemented.
- [ ] The appropriate architectural layer was modified.
- [ ] Existing behavior was preserved where appropriate.
- [ ] No unnecessary files were changed.
- [ ] The project builds successfully.
- [ ] Relevant tests pass.
- [ ] Database migrations were created when required.
- [ ] API documentation was updated when required.
- [ ] No secrets or credentials were introduced.
- [ ] Git status has been reviewed.

## Working Principle

The primary goal when modifying UFAMS is to improve the system while preserving its existing architecture and intent.

Agents should prefer:

**Understand → Reuse → Modify → Verify → Document**

rather than making large speculative changes.