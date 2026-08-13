# UFAMS Architecture

## Overview

UFAMS (Urban Forestry Asset Management System) is a GIS-oriented asset management application designed to manage urban tree assets, inspections, maintenance work orders, and spatial data synchronization with ArcGIS.

The system is implemented using a layered architecture with clear separation between:

- Presentation and HTTP concerns
- Application use cases
- Domain logic
- Infrastructure and external systems
- Persistence

The architecture is designed to keep business rules independent from infrastructure concerns while allowing UFAMS to integrate with external GIS services such as ArcGIS Feature Services.

## Architecture Goals

The primary architectural goals are:

- Maintain a clear separation of responsibilities.
- Keep domain logic independent from infrastructure technologies.
- Support GIS-specific workflows and spatial data.
- Provide a stable API for the frontend and external integrations.
- Make ArcGIS synchronization independently testable.
- Keep persistence concerns isolated behind repositories and application abstractions.
- Allow infrastructure implementations to change without requiring major changes to domain logic.
- Provide a structure that can grow as additional asset-management capabilities are introduced.

## Architectural Style

UFAMS follows a layered architecture influenced by Domain-Driven Design principles.

The major layers are:

1. **API**
2. **Application**
3. **Domain**
4. **Infrastructure**

The dependency direction is intentionally controlled so that the Domain layer does not depend on infrastructure or external services.

```text
┌───────────────────────────────────────────────┐
│                    Frontend                   │
│                  Next.js / UI                 │
└───────────────────────┬───────────────────────┘
                        │ HTTP
                        ▼
┌───────────────────────────────────────────────┐
│                     API                       │
│          ASP.NET Core Minimal API             │
│                                               │
│  Endpoints / HTTP Binding / Responses         │
└───────────────────────┬───────────────────────┘
                        │
                        ▼
┌───────────────────────────────────────────────┐
│                 Application                   │
│                                               │
│ Commands / Queries / Handlers / Services      │
│ Application Interfaces / Use Cases            │
└───────────────────────┬───────────────────────┘
                        │
                        ▼
┌───────────────────────────────────────────────┐
│                    Domain                     │
│                                               │
│ Entities / Value Objects / Enums / Rules      │
│                                               │
│ Tree / Park / Species / Inspection /          │
│ WorkOrder / Employee / GeoCoordinate          │
└───────────────────────────────────────────────┘
                        ▲
                        │
┌───────────────────────┴───────────────────────┐
│                Infrastructure                 │
│                                               │
│ EF Core / SQL Server / Repositories           │
│ ArcGIS Integration / External Services        │
└───────────────────────────────────────────────┘
```

## Separation of Concerns

Each layer has a specific responsibility.

### API Layer

The API layer is responsible for HTTP concerns.

It handles:

- Routing.
- Request binding.
- Query parameters.
- Request bodies.
- HTTP response codes.
- Calling application use cases.
- Translating application results into HTTP responses.

The API layer should avoid containing business rules.

### Application Layer

The Application layer coordinates application use cases.

It contains:

- Commands.
- Queries.
- Command handlers.
- Query handlers.
- Application services.
- Repository abstractions.
- Unit-of-work abstractions.

The Application layer determines **what the system needs to accomplish**, while the Domain layer determines **what the business rules allow**.

### Domain Layer

The Domain layer contains the core business concepts and rules of UFAMS.

Examples include:

- Tree
- Species
- Park
- Employee
- Inspection
- WorkOrder
- GeoCoordinate
- TreeHealthStatus
- WorkOrderStatus

The Domain layer should not depend on:

- Entity Framework Core
- SQL Server
- ArcGIS SDKs
- HTTP
- ASP.NET Core
- Frontend frameworks

This keeps the core business model independent of infrastructure technologies.

### Infrastructure Layer

The Infrastructure layer provides implementations for external and technical concerns.

Examples include:

- Entity Framework Core.
- SQL Server persistence.
- Repository implementations.
- Database initialization and seeding.
- ArcGIS authentication.
- ArcGIS Feature Service access.
- ArcGIS synchronization.
- External service integrations.

Infrastructure implements interfaces defined by the Application layer where appropriate.