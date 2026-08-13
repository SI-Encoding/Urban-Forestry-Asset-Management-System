# UFAMS

**UFAMS (Urban Forestry Asset Management System)** is a GIS-enabled tree asset management application designed to demonstrate modern asset management, GIS integration, backend architecture, frontend development, and database design.

The system manages urban tree assets and related information including species, parks, inspections, work orders, geographic locations, and tree health.

UFAMS also integrates with **ArcGIS Feature Services** to support GIS asset synchronization.

## Features

UFAMS currently includes:

- Tree asset management.
- Tree health tracking.
- Geographic tree locations.
- Species management.
- Park management.
- Park inventory information.
- Tree inspections.
- Inspection recommendations and follow-ups.
- Work order management.
- Work order assignment and status transitions.
- GeoJSON tree export.
- Nearby tree searches.
- ArcGIS Feature Service integration.
- ArcGIS OAuth authentication.
- ArcGIS synchronization preview.
- ArcGIS synchronization application.
- Single-tree synchronization.
- Synchronization audit history.
- Swagger/OpenAPI development tooling.

## Technology Stack

### Backend

- C#
- .NET
- ASP.NET Core
- Entity Framework Core
- SQL Server

### Frontend

- Next.js
- React
- TypeScript
- Tailwind CSS

### GIS

- ArcGIS Feature Services
- GeoJSON
- Geographic coordinate-based queries
- ArcGIS OAuth

### Testing

- Vitest
- Playwright

## Architecture

UFAMS follows a layered architecture:

```text
┌──────────────────────┐
│      Frontend        │
│       Next.js        │
└──────────┬───────────┘
           │
           ▼
┌──────────────────────┐
│      API Layer       │
│    ASP.NET Core      │
└──────────┬───────────┘
           │
           ▼
┌──────────────────────┐
│  Application Layer   │
│ Commands / Queries   │
│ Handlers / Services  │
└──────────┬───────────┘
           │
           ▼
┌──────────────────────┐
│     Domain Layer     │
│ Business Rules/Data  │
└──────────┬───────────┘
           │
           ▼
┌──────────────────────┐
│ Infrastructure Layer │
│ EF Core / ArcGIS     │
│ SQL Server / APIs    │
└──────────────────────┘
```

The API layer is intentionally thin.

Business rules primarily belong in the Domain and Application layers, while Infrastructure handles persistence and external integrations.

## GIS Architecture

ArcGIS integration is an important component of UFAMS.

The synchronization architecture separates ArcGIS from the core domain:

```text
                    UFAMS
                      │
                      ▼
             ArcGIS Sync Service
                 /          \
                /            \
               ▼              ▼
      ArcGIS Feature       UFAMS
         Service        Repositories
                              │
                              ▼
                         SQL Server
```

Synchronization supports:

- Previewing synchronization changes.
- Applying synchronization changes.
- Synchronizing an individual tree asset.
- Recording synchronization audit information.

## Domain Model

The primary domain entities include:

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

## Project Structure

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
    ├── AGENTS.md
    └── ...
```

## Getting Started

### Prerequisites

The development environment requires:

- .NET SDK
- Node.js
- npm
- SQL Server
- Git

### Clone the Repository

```bash
git clone <repository-url>
cd UFAMS
```

### Backend

Navigate to the backend:

```bash
cd backend
```

Restore dependencies:

```bash
dotnet restore
```

Build the backend:

```bash
dotnet build
```

Run the API using the project's configured ASP.NET Core launch configuration.

### Frontend

Navigate to the frontend:

```bash
cd frontend
```

Install dependencies:

```bash
npm install
```

Build the frontend:

```bash
npm run build
```

## API Documentation

UFAMS provides Swagger/OpenAPI documentation during development.

When the API is running, Swagger UI is available at:

```text
/swagger
```

See [`docs/API.md`](docs/API.md) for the complete API documentation.

## Documentation

Additional project documentation is available under `docs/`.

| Document | Description |
|---|---|
| [`API.md`](docs/API.md) | REST API endpoints and behavior |
| [`ARCHITECTURE.md`](docs/ARCHITECTURE.md) | System architecture and separation of concerns |
| [`DATABASE.md`](docs/DATABASE.md) | Database architecture and Entity Framework Core |
| [`DECISIONLOG.md`](docs/DECISIONLOG.md) | Important technical and architectural decisions |
| [`DEVELOPMENT.md`](docs/DEVELOPMENT.md) | Development environment and setup instructions |
| [`AGENTS.md`](docs/AGENTS.md) | Guidance for AI coding agents |

## Database

UFAMS uses **SQL Server** with **Entity Framework Core**.

Database schema changes are managed using EF Core migrations.

Typical workflow:

```text
Domain / Persistence Change
            │
            ▼
    Create EF Migration
            │
            ▼
      Review Migration
            │
            ▼
      Apply Migration
            │
            ▼
        SQL Server
```

See [`docs/DATABASE.md`](docs/DATABASE.md) for more information.

## Development

Backend development:

```bash
cd backend
dotnet restore
dotnet build
```

Frontend development:

```bash
cd frontend
npm install
npm run build
```

See [`docs/DEVELOPMENT.md`](docs/DEVELOPMENT.md) for detailed development instructions.

## Testing

UFAMS uses:

- **Vitest** for unit testing.
- **Playwright** for end-to-end testing.

Run the relevant tests for the area being modified.

## Security

Do not commit sensitive information to the repository.

This includes:

- Passwords.
- API keys.
- ArcGIS OAuth secrets.
- Access tokens.
- Database credentials.
- Production connection strings containing credentials.

Environment-specific configuration should be provided through appropriate configuration mechanisms or environment variables.

## Current Development Status

UFAMS is an actively developed portfolio project.

Implemented areas include:

- Core tree asset management.
- Tree geographic data.
- Inspections.
- Work orders.
- SQL Server persistence.
- Entity Framework Core migrations.
- GeoJSON export.
- GIS-oriented spatial queries.
- ArcGIS Feature Service integration.
- ArcGIS OAuth authentication.
- ArcGIS synchronization.
- Synchronization audit history.

Future improvements may include additional synchronization workflow features, improved filtering, scheduled/background synchronization, and expanded GIS workflows.

## Project Goals

UFAMS is intended to demonstrate practical experience with:

- GIS application development.
- Asset management systems.
- REST API design.
- Clean/layered architecture.
- Domain-driven application design.
- Entity Framework Core.
- SQL Server.
- Next.js.
- ArcGIS integration.
- OAuth authentication.
- Spatial data synchronization.
- Auditability.
- Automated testing.

## License

This project is currently intended as a portfolio and demonstration project.
