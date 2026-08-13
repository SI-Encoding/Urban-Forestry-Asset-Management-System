# UFAMS REST API

## Overview

UFAMS exposes an ASP.NET Core Minimal API used by the Next.js frontend and
ArcGIS integration.

The API provides functionality for:

- Tree asset management
- Tree searching and spatial queries
- GeoJSON export
- Species lookup
- Park inventory
- Tree inspections
- Maintenance work orders
- ArcGIS authentication
- ArcGIS Feature Service access
- ArcGIS synchronization
- Synchronization audit history

Unless otherwise noted, successful responses return JSON.

## Base URL

### Development

```text
https://localhost:<port>
```

The exact development port is configured by the ASP.NET Core launch configuration.

### Production

The production API URL is environment-specific and should not be hard-coded into this documentation.

---

## Trees

### Register Tree

```http
POST /trees
```

Registers a new tree in UFAMS.

#### Request Body

`RegisterTreeCommand`

#### Response

```text
201 Created
```

The response contains the newly created tree.

### Get Trees

```http
GET /trees
```

Returns trees matching optional filter criteria.

#### Query Parameters

| Parameter | Type | Required | Description |
|---|---|---|---|
| `parkId` | GUID | No | Filter by park |
| `speciesId` | GUID | No | Filter by species |
| `healthStatus` | enum | No | Filter by tree health |

#### Example

```http
GET /trees?healthStatus=Poor
```

#### Response

```text
200 OK
```

### Get Tree

```http
GET /trees/{id}
```

Returns a specific tree.

#### Route Parameters

| Parameter | Type | Description |
|---|---|---|
| `id` | GUID | Tree ID |

#### Response

```text
200 OK
```

### Update Tree Measurements

```http
PUT /trees/{id}/measurements
```

Updates measurements for a tree.

#### Route Parameters

| Parameter | Type | Description |
|---|---|---|
| `id` | GUID | Tree ID |

#### Request Body

`UpdateTreeMeasurementsCommand`

#### Response

```text
200 OK
```

### Relocate Tree

```http
PUT /trees/{id}/location
```

Updates the geographic location of a tree.

#### Route Parameters

| Parameter | Type | Description |
|---|---|---|
| `id` | GUID | Tree ID |

#### Request Body

`RelocateTreeCommand`

#### Response

```text
200 OK
```

### Update Tree Health

```http
PUT /trees/{id}/health
```

Updates the health status of a tree.

#### Route Parameters

| Parameter | Type | Description |
|---|---|---|
| `id` | GUID | Tree ID |

#### Request Body

`UpdateTreeHealthCommand`

#### Response

```text
200 OK
```

### Search Trees

```http
GET /trees/search
```

Searches trees using multiple optional criteria.

#### Query Parameters

| Parameter | Type | Required | Description |
|---|---|---|---|
| `parkId` | GUID | No | Filter by park |
| `speciesId` | GUID | No | Filter by species |
| `healthStatus` | enum | No | Filter by health |
| `minLatitude` | number | No | Minimum latitude |
| `maxLatitude` | number | No | Maximum latitude |
| `minLongitude` | number | No | Minimum longitude |
| `maxLongitude` | number | No | Maximum longitude |

#### Example

```http
GET /trees/search?healthStatus=Poor&parkId=<guid>
```

#### Response

```text
200 OK
```

### Export Trees as GeoJSON

```http
GET /trees/geojson
```

Exports trees as GeoJSON for GIS and mapping workflows.

#### Query Parameters

| Parameter | Type | Required |
|---|---|---|
| `parkId` | GUID | No |
| `speciesId` | GUID | No |
| `healthStatus` | enum | No |
| `minLatitude` | number | No |
| `maxLatitude` | number | No |
| `minLongitude` | number | No |
| `maxLongitude` | number | No |

#### Response

```text
200 OK
```

Returns a GeoJSON feature collection.

### Find Nearby Trees

```http
GET /trees/nearby
```

Returns trees within a specified radius of a geographic coordinate.

#### Query Parameters

| Parameter | Type | Required | Description |
|---|---|---|---|
| `latitude` | number | Yes | Latitude |
| `longitude` | number | Yes | Longitude |
| `radiusMeters` | number | Yes | Search radius in metres |

#### Example

```http
GET /trees/nearby?latitude=49.2827&longitude=-123.1207&radiusMeters=500
```

#### Response

```text
200 OK
```

Invalid geographic/search parameters may return:

```text
400 Bad Request
```

---

## Species

### Get Species

```http
GET /species
```

Returns all tree species managed by UFAMS.

#### Response

```text
200 OK
```

### Search Species

```http
GET /species/search
```

Searches species using a search term.

#### Query Parameters

| Parameter | Type | Required |
|---|---|---|
| `query` | string | Yes |

#### Example

```http
GET /species/search?query=cedar
```

#### Response

```text
200 OK
```

---

## Parks

### Get Parks

```http
GET /parks
```

Returns all parks managed by UFAMS.

#### Response

```text
200 OK
```

### Get Park Inventory

```http
GET /parks/{parkId}/inventory
```

Returns inventory information for a park.

The inventory includes tree counts, health statistics, and species breakdown.

#### Route Parameters

| Parameter | Type | Description |
|---|---|---|
| `parkId` | GUID | Park ID |

#### Response

```text
200 OK
```

---

## Inspections

### Create Inspection

```http
POST /trees/{treeId}/inspections
```

Creates an inspection for a specific tree.

#### Route Parameters

| Parameter | Type |
|---|---|
| `treeId` | GUID |

#### Request Body

`CreateInspectionCommand`

#### Response

```text
201 Created
```

### Get Tree Inspections

```http
GET /trees/{treeId}/inspections
```

Returns all inspections associated with a tree.

#### Response

```text
200 OK
```

### Get All Inspections

```http
GET /inspections
```

Returns all inspection records.

#### Response

```text
200 OK
```

### Get Inspection

```http
GET /inspections/{id}
```

Returns a specific inspection.

#### Route Parameters

| Parameter | Type |
|---|---|
| `id` | GUID |

#### Response

```text
200 OK
```

### Update Inspection Notes

```http
PUT /inspections/{id}/notes
```

Updates notes associated with an inspection.

#### Request Body

`UpdateInspectionNotesCommand`

#### Response

```text
200 OK
```

### Update Inspection Recommendation

```http
PUT /inspections/{id}/recommendation
```

Updates the recommendation associated with an inspection.

#### Request Body

`UpdateInspectionRecommendationCommand`

#### Response

```text
200 OK
```

### Schedule Inspection Follow-Up

```http
PUT /inspections/{id}/follow-up
```

Schedules a follow-up action for an inspection.

#### Request Body

`ScheduleFollowUpCommand`

#### Response

```text
200 OK
```

---

## Work Orders

### Create Work Order

```http
POST /trees/{treeId}/work-orders
```

Creates a work order associated with a tree.

#### Request Body

`CreateWorkOrderCommand`

#### Response

```text
201 Created
```

### Get Tree Work Orders

```http
GET /trees/{treeId}/work-orders
```

Returns all work orders associated with a tree.

#### Response

```text
200 OK
```

### Get All Work Orders

```http
GET /work-orders
```

## Work Orders

### Get All Work Orders

`GET /work-orders`

Returns all work orders, including related tree, species, park, and employee information.

**Response**

`200 OK`

### Get Work Order

`GET /work-orders/{id}`

Returns a specific work order.

**Response**

`200 OK`

### Assign Work Order

`PUT /work-orders/{id}/assign`

Assigns a work order to an employee/user.

**Request Body**

`AssignWorkOrderCommand`

**Response**

`200 OK`

### Start Work Order

`PUT /work-orders/{id}/start`

Starts a work order.

**Response**

`200 OK`

### Complete Work Order

`PUT /work-orders/{id}/complete`

Completes a work order.

**Response**

`200 OK`

### Cancel Work Order

`PUT /work-orders/{id}/cancel`

Cancels a work order.

**Response**

`200 OK`

## ArcGIS

UFAMS integrates with ArcGIS Feature Services for GIS asset management and synchronization.

### Test ArcGIS Authentication

`GET /arcgis/token-test`

Requests an ArcGIS access token and returns a success result without exposing the token itself.

**Response**

**Example:**

```json
{
  "success": true,
  "tokenLength": 123
}
```

Deployment note: The current callback redirects to the development frontend URL. This must be made environment-specific before production deployment.

ArcGIS Authentication Status
GET /api/arcgis/auth/status

Returns whether UFAMS currently has an authenticated ArcGIS session.

**Example Response**

```json
{
  "authenticated": true
}
```

ArcGIS Logout
POST /api/arcgis/auth/logout

Clears the in-memory ArcGIS token and persisted token.

**Example Response**

```json
{
  "authenticated": false
}
```

## ArcGIS Synchronization

### Preview ArcGIS Synchronization

`GET /arcgis/sync/preview`

Generates a synchronization result without applying the changes to the UFAMS database.

#### Response

`200 OK`

The response contains the synchronization result.

### Apply ArcGIS Synchronization

`POST /arcgis/sync/apply`

Applies ArcGIS synchronization changes to UFAMS.

#### Response

`200 OK`

### Apply Single ArcGIS Synchronization

`POST /arcgis/sync/apply/{assetTag}`

Applies synchronization for a single tree asset.

#### Route Parameters

| Parameter | Type | Required |
|---|---|---|
| `assetTag` | `string` | Yes |

An empty asset tag returns:

`400 Bad Request`

### Synchronization Audits

#### Get Recent Synchronization Audits

`GET /arcgis/sync/audits`

Returns recent ArcGIS synchronization audit history.

The current implementation requests the 50 most recent audits.

#### Response

Each audit includes information such as:

- Audit ID
- Start time
- Completion time
- Status
- Created count
- Updated count
- Failed count
- Ignored count
- Synchronization entries

Each synchronization entry includes:

- Entry ID
- Asset tag
- Action
- Reason
- Creation time

#### Response

`200 OK`

## HTTP Status Codes

UFAMS uses standard HTTP response status codes.

| Status | Meaning |
|---|---|
| `200 OK` | Request completed successfully |
| `201 Created` | Resource was successfully created |
| `400 Bad Request` | Request parameters or input are invalid |
| `404 Not Found` | Requested resource does not exist |
| `409 Conflict` | Request conflicts with the current state |
| `500 Internal Server Error` | Unexpected server-side failure |

Exception handling is centralized through the UFAMS API exception-handling middleware.

## API Architecture

The API layer is intentionally thin.

Endpoint classes are responsible primarily for:

- Receiving HTTP requests.
- Binding route/query/body parameters.
- Constructing commands or queries.
- Invoking application handlers/services.
- Returning HTTP responses.

Business logic is implemented primarily in the Application and Domain layers.

### Example

```text
HTTP Request
     │
     ▼
TreeEndpoints
     │
     ▼
GetTreesQuery
     │
     ▼
GetTreesHandler
     │
     ▼
ITreeRepository
     │
     ▼
EF Core / SQL Server
```

ArcGIS synchronization follows a similar separation:

```text
HTTP Request
     │
     ▼
ArcGisSyncEndpoints
     │
     ▼
SpatialDataSyncService
     │
     ├── ArcGIS Feature Service
     │
     └── UFAMS repositories
              │
              ▼
          SQL Server
```

## Development API Tools

Swagger/OpenAPI is enabled in the ASP.NET Core development environment.

When running UFAMS in development, Swagger UI is available through the configured development API URL:

`/swagger`

Swagger is not enabled by the current production pipeline configuration.