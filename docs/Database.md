# UFAMS Database Architecture

## Overview

UFAMS uses a relational database to persist urban forestry asset-management data.

The database is responsible for storing:

- Tree assets.
- Tree species.
- Parks.
- Employees.
- Inspections.
- Maintenance work orders.
- ArcGIS feature identifiers.
- ArcGIS synchronization audits.
- ArcGIS synchronization entries.

The database is accessed through Entity Framework Core in the UFAMS Infrastructure layer.

The Application and Domain layers do not directly interact with SQL Server. Database access is isolated behind repository and persistence abstractions.

## Database Technology

UFAMS uses:

- **Database:** Microsoft SQL Server
- **ORM:** Entity Framework Core
- **Database Access:** Entity Framework Core repositories
- **Schema Management:** Entity Framework Core migrations
- **Initialization:** Database initialization and seed data

The database provider is configured in the Infrastructure layer rather than being referenced directly by the Domain layer.

## Persistence Architecture

Database access follows the application's layered architecture.

```text
Application
    │
    ▼
Repository Interfaces
    │
    ▼
Infrastructure
    │
    ▼
Entity Framework Core
    │
    ▼
SQL Server
```

The Application layer depends on abstractions such as repositories and unit-of-work interfaces.

The Infrastructure layer provides the concrete implementations.

This allows application and domain logic to remain independent of the database technology.

## Entity Framework Core

Entity Framework Core is responsible for:

- Mapping domain entities to database tables.
- Managing relationships between entities.
- Tracking entity changes.
- Persisting changes to SQL Server.
- Executing queries.
- Applying database migrations.

Entity Framework Core configuration is contained within the Infrastructure layer.

## Database Migrations

UFAMS uses Entity Framework Core migrations to manage database schema changes.

Migrations allow database changes to be versioned alongside the application source code.

For example, adding ArcGIS integration metadata to trees can be represented as a migration rather than requiring manual database changes.

A typical migration workflow is:

A typical migration workflow is:

```text
Domain / Persistence Model Change
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

Migrations should be committed to source control so that the database schema can be reproduced consistently across development and deployment environments.