# UFAMS Development Guide

## Overview

This document describes the development workflow for UFAMS.

It covers:

- Development prerequisites.
- Repository setup.
- Project structure.
- Configuration.
- Database setup.
- Running the backend.
- Running the frontend.
- Applying database migrations.
- Development tools.
- Testing and validation.

## Prerequisites

The following tools are required for local development.

### .NET

UFAMS uses ASP.NET Core and .NET 8.

The .NET 8 SDK should be installed and available through the command line.

Verify the installation with:

```bash
dotnet --version
```

### Node.js

The frontend requires Node.js and npm.

Verify the installation with:

```bash
node --version
npm --version
```

### SQL Server

UFAMS uses Microsoft SQL Server for persistence.

A local SQL Server instance or another accessible SQL Server environment is required for backend development.

### Git

Git is used for source control.

Verify the installation with:

```bash
git --version
```

## Repository Setup

Clone the UFAMS repository and navigate into the project directory.

```bash
git clone <repository-url>
cd UFAMS
```

The repository contains both the backend and frontend applications.

## Project Structure

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
│   ├── package.json
│   └── ...
│
└── docs/
    ├── API.md
    ├── ARCHITECTURE.md
    ├── DATABASE.md
    ├── DECISIONLOG.md
    └── DEVELOPMENT.md
```

## Backend Setup

Navigate to the backend directory:

```bash
cd backend
```

Restore the .NET dependencies:

```bash
dotnet restore
```

Build the solution:

```bash
dotnet build
```

A successful build confirms that the backend projects and their dependencies can be compiled locally.

## Frontend Setup

Navigate to the frontend directory:

```bash
cd frontend
```

Install the npm dependencies:

```bash
npm install
```

Build the frontend to verify that the application compiles:

```bash
npm run build
```