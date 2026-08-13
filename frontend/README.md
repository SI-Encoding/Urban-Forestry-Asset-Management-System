# UFAMS Frontend

The UFAMS frontend is a Next.js application that provides the user interface for the Urban Forestry Asset Management System.

It communicates with the UFAMS ASP.NET Core API and provides interfaces for managing trees, parks, species, inspections, work orders, and ArcGIS synchronization workflows.

## Technology Stack

- Next.js
- React
- TypeScript
- Tailwind CSS
- Leaflet
- Fetch API

## Frontend Structure

The frontend uses the Next.js App Router.

```text
frontend/
├── app/
│   ├── dashboard/
│   ├── map/
│   ├── trees/
│   ├── parks/
│   ├── species/
│   ├── inspections/
│   ├── work-orders/
│   └── ...
│
├── components/
│   └── ...
│
├── lib/
│   └── ...
│
├── public/
│   └── ...
│
├── package.json
└── ...
```

The exact directory structure may evolve as the frontend develops.

## Getting Started

### Prerequisites

Make sure the following are installed:

- Node.js
- npm
- UFAMS backend API

### Install Dependencies

From the frontend directory:

```bash
npm install
```

### Configure the API URL

The frontend communicates with the UFAMS ASP.NET Core API through the `NEXT_PUBLIC_API_URL` environment variable.

For local development, create a `.env.local` file if required:

```env
NEXT_PUBLIC_API_URL=http://localhost:5079
```

The exact backend port is determined by the ASP.NET Core launch configuration.

Do not commit environment-specific secrets or credentials.

## Run the Development Server

Start the Next.js development server:

```bash
npm run dev
```

The frontend is normally available at:

```text
http://localhost:3000
```

## Build the Frontend

Create a production build:

```bash
npm run build
```

A successful build confirms that the frontend can be compiled successfully.

## Frontend and Backend

The frontend communicates with the ASP.NET Core API.

The general request flow is:

```text
User Interface
      │
      ▼
Next.js Page / Component
      │
      ▼
Frontend API Client
      │
      ▼
ASP.NET Core API
      │
      ▼
Application Layer
      │
      ▼
SQL Server / ArcGIS
```

The frontend should not contain backend business rules.

Business rules belong primarily in the backend Application and Domain layers.

## API Communication

Frontend API requests use the UFAMS API client utilities.

The API base URL should be provided through:

```text
NEXT_PUBLIC_API_URL
```

Avoid hard-coding production API URLs into components.

## GIS / Mapping

UFAMS includes geographic functionality for tree assets.

The frontend uses mapping functionality to display tree locations and support GIS-oriented workflows.

Tree geographic data is provided by the backend API.

The frontend should treat the backend as the source of truth for UFAMS asset data.

## ArcGIS Integration

The frontend also supports ArcGIS synchronization workflows.

These workflows communicate with backend ArcGIS endpoints rather than directly implementing ArcGIS synchronization logic in the browser.

The general flow is:

```text
Frontend
   │
   ▼
UFAMS API
   │
   ▼
ArcGIS Integration Layer
   │
   ├── ArcGIS Feature Service
   │
   └── UFAMS Database
```

This keeps authentication, synchronization, persistence, and business rules on the backend.

## Main Application Areas

The frontend contains interfaces for areas including:

### Dashboard

Provides an overview of UFAMS information and operational activity.

### Map

Displays tree assets geographically.

### Trees

Provides tree asset management and geographic information.

### Parks

Provides park information and inventory information.

### Species

Provides tree species information.

### Inspections

Provides inspection records and inspection-related workflows.

### Work Orders

Provides work order management, assignment, and status workflows.

### ArcGIS Synchronization

Provides access to ArcGIS authentication and synchronization workflows.

## Development Guidelines

When modifying the frontend:

- Reuse existing components.
- Reuse existing API utilities.
- Follow the existing Next.js App Router structure.
- Keep API communication separate from UI presentation.
- Avoid duplicating backend business rules.
- Avoid unnecessary dependencies.
- Preserve existing navigation and application behavior.
- Keep environment-specific configuration outside the source code.

## Testing

UFAMS uses Playwright for end-to-end testing and Vitest for unit testing.

When changing frontend functionality, run the relevant tests when available.

## Production Build

Before deploying the frontend, verify that the production build succeeds:

```bash
npm run build
```

Production environment configuration should provide the appropriate API URL through:

```text
NEXT_PUBLIC_API_URL
```

Do not commit production credentials or secrets.

## Related Documentation

Project-wide documentation is located in the repository's `docs/` directory.

See:

- [`../README.md`](../README.md) — UFAMS project overview
- [`../docs/API.md`](../docs/API.md) — Backend API documentation
- [`../docs/ARCHITECTURE.md`](../docs/ARCHITECTURE.md) — System architecture
- [`../docs/DATABASE.md`](../docs/DATABASE.md) — Database architecture
- [`../docs/DEVELOPMENT.md`](../docs/DEVELOPMENT.md) — Development setup