# UFAMS Architecture Decision Log

## Purpose

This document records significant technical and architectural decisions made during the development of UFAMS.

The goal is to document not only **what** technologies and patterns were selected, but also **why** they were selected and what trade-offs were considered.

These decisions provide context for future development and help maintain consistency as the system evolves.

## Decision Format

Each decision records:

- **Decision:** What was chosen.
- **Context:** Why the decision was necessary.
- **Rationale:** Why the selected approach was preferred.
- **Alternatives:** Other approaches that were considered.
- **Consequences:** The benefits and trade-offs resulting from the decision.

---

## ADR-001: Use a Layered Architecture

**Status:** Accepted

### Decision

UFAMS uses a layered architecture consisting of:

- API
- Application
- Domain
- Infrastructure

### Context

UFAMS contains both standard asset-management functionality and GIS-specific integration with ArcGIS.

Without clear separation between these concerns, HTTP endpoints, business rules, database access, and external GIS integrations could become tightly coupled.

### Rationale

A layered architecture provides clear boundaries between responsibilities.

The Domain layer can contain core urban forestry business concepts without depending on ASP.NET Core, Entity Framework Core, SQL Server, or ArcGIS.

The Application layer coordinates use cases, while the Infrastructure layer provides implementations for persistence and external integrations.

### Alternatives Considered

- A monolithic structure with minimal separation between concerns.
- A feature-only folder structure without explicit architectural boundaries.
- A fully distributed microservice architecture.

### Consequences

**Benefits:**

- Clear separation of responsibilities.
- Easier testing of business logic.
- Infrastructure can evolve independently.
- ArcGIS integration remains isolated from core domain logic.
- New application features can follow established architectural boundaries.

**Trade-offs:**

- More projects and abstractions than a simple monolithic application.
- Additional interfaces and dependency configuration.
- Developers must understand the intended dependency direction.

---

## ADR-002: Use ASP.NET Core Minimal APIs

**Status:** Accepted

### Decision

UFAMS exposes its backend functionality through ASP.NET Core Minimal APIs.

### Context

UFAMS requires a REST API for communication between the Next.js frontend and backend application services.

The API also provides integration points for GIS workflows and ArcGIS synchronization.

### Rationale

Minimal APIs provide a lightweight HTTP layer while allowing the application to maintain clear separation between endpoint handling and business logic.

Endpoints primarily handle:

- HTTP routing.
- Request binding.
- Query parameters.
- Command/query construction.
- Application service invocation.
- HTTP responses.

Business logic remains in the Application and Domain layers.

### Alternatives Considered

- ASP.NET Core MVC controllers.
- A GraphQL API.
- Direct frontend access to the database.

### Consequences

**Benefits:**

- Lightweight endpoint implementation.
- Clear HTTP boundary.
- Good integration with application commands and queries.
- Straightforward REST API design.
- Built-in compatibility with ASP.NET Core middleware and OpenAPI tooling.

**Trade-offs:**

- Endpoint organization must be maintained carefully as the API grows.
- Some developers may be more familiar with controller-based APIs.

---

## ADR-003: Use Entity Framework Core for Persistence

**Status:** Accepted

### Decision

UFAMS uses Entity Framework Core as its object-relational mapper for SQL Server.

### Context

UFAMS contains multiple related entities, including trees, species, parks, inspections, employees, work orders, and synchronization records.

The application requires reliable persistence while maintaining domain-oriented application code.

### Rationale

Entity Framework Core provides:

- Strong integration with .NET.
- LINQ-based querying.
- Relationship mapping.
- Change tracking.
- Database migrations.
- SQL Server support.

Database-specific implementation details remain within the Infrastructure layer.

### Alternatives Considered

- Dapper.
- Raw ADO.NET.
- Direct SQL queries throughout the application.

### Consequences

**Benefits:**

- Reduced persistence boilerplate.
- Strong integration with the .NET type system.
- Migration support.
- Consistent repository implementation.
- Easier management of entity relationships.

**Trade-offs:**

- ORM abstraction can hide some SQL behavior.
- Developers need to understand EF Core query behavior and tracking.
- Complex queries may require careful performance analysis.

---

## ADR-004: Use ArcGIS Feature Services for GIS Integration

**Status:** Accepted

### Decision

UFAMS integrates with ArcGIS through ArcGIS Feature Services.

### Context

UFAMS is intended to demonstrate an enterprise-style urban forestry asset-management workflow.

Tree assets need to be exchanged between UFAMS and an external GIS system while preserving UFAMS as the source of application-specific business data.

### Rationale

ArcGIS Feature Services provide a standard mechanism for working with geographic features and attributes through ArcGIS.

Using the Feature Service API allows UFAMS to:

- Retrieve GIS features.
- Read feature attributes.
- Identify geographic locations.
- Export UFAMS tree assets.
- Synchronize external GIS data with UFAMS.

This approach also provides a realistic integration pattern for an ESRI-based asset-management environment.

### Alternatives Considered

- Storing GIS data exclusively in UFAMS.
- Using a custom GIS API.
- Direct database integration with an ArcGIS database.
- Using a different GIS platform.

### Consequences

**Benefits:**

- Demonstrates real-world ArcGIS integration.
- Keeps GIS integration behind infrastructure services.
- Supports synchronization workflows.
- Allows UFAMS to interact with an external GIS system without tightly coupling the Domain layer to ArcGIS.

**Trade-offs:**

- Requires handling external service availability.
- ArcGIS authentication and API behavior must be managed.
- Synchronization introduces additional failure scenarios.

---

## ADR-005: Use GeoJSON for GIS Data Export

**Status:** Accepted

### Decision

UFAMS exposes tree geographic data as GeoJSON.

### Context

Tree assets contain geographic coordinates and need to be usable by GIS and mapping tools.

A standard geographic interchange format is preferable to exposing internal database representations.

### Rationale

GeoJSON is widely supported by modern GIS and mapping tools and represents geographic features and their properties in a standard JSON-based format.

UFAMS can therefore expose tree assets as a GeoJSON FeatureCollection without requiring consumers to understand the internal domain or database model.

### Alternatives Considered

- Returning raw database records.
- Returning custom JSON geographic objects.
- Using CSV with latitude and longitude columns.
- Using a proprietary GIS exchange format.

### Consequences

**Benefits:**

- Standard GIS-compatible format.
- Easy integration with mapping libraries.
- Human-readable JSON representation.
- Separates external GIS representation from internal persistence models.

**Trade-offs:**

- GeoJSON is not intended to represent every possible GIS data structure.
- Coordinate reference system considerations must be handled consistently.

---

## ADR-006: Use External ArcGIS Feature IDs

**Status:** Accepted

### Decision

UFAMS stores the external ArcGIS feature identifier on tree records.

### Context

A UFAMS tree and its corresponding ArcGIS feature represent the same real-world asset but exist in different systems.

The systems therefore require a stable identifier that allows a feature in ArcGIS to be associated with the corresponding UFAMS tree.

### Rationale

An external ArcGIS identifier allows synchronization logic to distinguish between:

- Existing matching assets.
- New ArcGIS assets.
- UFAMS assets without an ArcGIS counterpart.
- Previously synchronized assets.

This prevents synchronization logic from relying exclusively on mutable attributes such as tree names, descriptions, or geographic coordinates.

### Alternatives Considered

- Matching trees only by geographic coordinates.
- Matching trees by asset tag only.
- Using the UFAMS database ID as the ArcGIS identifier.
- Maintaining a separate mapping table immediately.

### Consequences

**Benefits:**

- Provides a direct relationship between UFAMS and ArcGIS features.
- Supports repeatable synchronization.
- Reduces the risk of duplicate assets.
- Makes external-system identity explicit.

**Trade-offs:**

- External identifiers are dependent on the ArcGIS system.
- Synchronization must handle missing or changed external identifiers.
- The UFAMS internal identifier remains separate from the external GIS identifier.

---

## ADR-007: Use ArcGIS OAuth for Authentication

**Status:** Accepted

### Decision

UFAMS uses ArcGIS OAuth for user authentication when accessing protected ArcGIS resources.

### Context

The ArcGIS integration requires authenticated access to ArcGIS resources.

Credentials and access tokens should not be exposed to the frontend unnecessarily or embedded directly into application code.

### Rationale

OAuth provides a standard authorization flow in which the user authenticates with ArcGIS and UFAMS receives authorization information that can be used to access permitted resources.

UFAMS separates the authentication flow from the rest of the ArcGIS integration.

The API handles:

- Beginning the OAuth flow.
- Receiving the OAuth callback.
- Validating OAuth state.
- Exchanging the authorization code.
- Managing the resulting token.
- Reporting authentication status.
- Logging out.

### Alternatives Considered

- Hard-coded ArcGIS credentials.
- Sharing a single long-lived token.
- Passing ArcGIS credentials through the frontend.
- Anonymous access to protected resources.

### Consequences

**Benefits:**

- Avoids embedding user credentials in application code.
- Uses ArcGIS's supported authorization mechanism.
- Provides an explicit authentication lifecycle.
- Allows the application to determine whether an ArcGIS session is authenticated.

**Trade-offs:**

- OAuth introduces additional flow complexity.
- Tokens must be handled securely.
- Callback URLs must be configured correctly for each environment.

---

## ADR-008: Keep ArcGIS Integration in Infrastructure

**Status:** Accepted

### Decision

ArcGIS authentication, Feature Service access, and synchronization implementations belong in the Infrastructure layer.

### Context

ArcGIS is an external system and should not become a dependency of the core UFAMS business model.

The Domain layer should represent urban forestry concepts rather than ArcGIS-specific implementation details.

### Rationale

Keeping ArcGIS integrations in Infrastructure maintains the dependency direction of the application.

The Application layer can coordinate synchronization through abstractions, while Infrastructure handles:

- ArcGIS authentication.
- HTTP communication with ArcGIS.
- Feature Service requests.
- External response parsing.
- ArcGIS-specific synchronization behavior.

### Alternatives Considered

- Calling ArcGIS directly from API endpoints.
- Placing ArcGIS services in the Domain layer.
- Allowing Application handlers to depend directly on ArcGIS SDK types.

### Consequences

**Benefits:**

- Keeps the Domain layer technology-independent.
- Makes ArcGIS-specific code easier to isolate.
- Improves testability.
- Allows the external GIS implementation to change without redesigning the domain model.

**Trade-offs:**

- Requires interfaces between Application and Infrastructure.
- Synchronization involves more layers than a direct API-to-ArcGIS implementation.