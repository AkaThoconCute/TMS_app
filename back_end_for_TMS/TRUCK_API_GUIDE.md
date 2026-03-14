# Truck Management APIs - Phase 1.1

## Overview

The Truck Management APIs provide full CRUD operations for managing trucks in the TMS system. All endpoints require authentication (Bearer token).

## Endpoints

### 1. Create Truck

**POST** `/api/truck/create`

Creates a new truck in the system. License plate must be unique.

**Request:**

```json
{
  "licensePlate": "0123-AB01",
  "brand": "Hino",
  "modelYear": 2020,
  "purchaseDate": "2020-01-15",
  "truckType": "Thùng kín",
  "maxPayloadKg": 15000.0,
  "lengthMm": 9000,
  "widthMm": 2500,
  "heightMm": 3000,
  "ownershipType": 1,
  "vinNumber": "LFVJVPA112345678",
  "engineNumber": "ABC123456789"
}
```

**Response (201):**

```json
{
  "truckId": "550e8400-e29b-41d4-a716-446655440000",
  "licensePlate": "0123-AB01",
  "brand": "Hino",
  "modelYear": 2020,
  "purchaseDate": "2020-01-15",
  "vinNumber": "LFVJVPA112345678",
  "engineNumber": "ABC123456789",
  "truckType": "Thùng kín",
  "maxPayloadKg": 15000.0,
  "lengthMm": 9000,
  "widthMm": 2500,
  "heightMm": 3000,
  "ownershipType": 1,
  "currentStatus": 1,
  "odometerReading": 0.0,
  "lastMaintenanceDate": null,
  "createdAt": "2026-03-14T10:30:00+00:00",
  "updatedAt": null
}
```

---

### 2. Get Truck by ID

**GET** `/api/truck/getbyid/{truckId}`

Retrieves a truck by its ID.

**Parameters:**

- `truckId` (path): UUID of the truck

**Response (200):**

```json
{
  "truckId": "550e8400-e29b-41d4-a716-446655440000",
  "licensePlate": "0123-AB01",
  "brand": "Hino",
  ...
}
```

---

### 3. Get Truck by License Plate

**GET** `/api/truck/getbylicenseplate/{licensePlate}`

Retrieves a truck by its license plate number.

**Parameters:**

- `licensePlate` (path): License plate string (e.g., "0123-AB01")

**Response (200):** Same as Get by ID

---

### 4. List Trucks (Paginated)

**GET** `/api/truck/list`

Lists all trucks with pagination and optional filtering.

**Query Parameters:**

- `pageNumber` (optional, default: 1): Page number for pagination
- `pageSize` (optional, default: 10, max: 100): Number of records per page
- `status` (optional): Filter by current status (e.g., 1, 2, 3...)
- `searchTerm` (optional): Search by license plate or brand name

**Example:**

```
GET /api/truck/list?pageNumber=1&pageSize=10&status=1&searchTerm=Hino
```

**Response (200):**

```json
{
  "data": [
    {
      "truckId": "550e8400-e29b-41d4-a716-446655440000",
      "licensePlate": "0123-AB01",
      "brand": "Hino",
      ...
    }
  ],
  "totalCount": 25,
  "pageSize": 10,
  "pageNumber": 1,
  "totalPages": 3
}
```

---

### 5. Update Truck

**PUT** `/api/truck/update/{truckId}`

Updates truck information. Only provide fields that need to be updated.

**Parameters:**

- `truckId` (path): UUID of the truck

**Request (partial update example):**

```json
{
  "currentStatus": 2,
  "lastMaintenanceDate": "2026-03-14",
  "odometerReading": 50000.5
}
```

**Response (200):** Updated truck object

---

### 6. Delete Truck

**DELETE** `/api/truck/delete/{truckId}`

Permanently deletes a truck from the system.

**Parameters:**

- `truckId` (path): UUID of the truck

**Response (200):**

```json
true
```

---

### 7. Update Odometer

**PATCH** `/api/truck/updateodometer/{truckId}`

Updates only the odometer reading and updates the `UpdatedAt` timestamp.

**Parameters:**

- `truckId` (path): UUID of the truck

**Request:**

```json
{
  "odometerReading": 75000.25
}
```

**Response (200):**

```json
true
```

---

### 8. Update Status

**PATCH** `/api/truck/updatestatus/{truckId}`

Updates only the truck status and updates the `UpdatedAt` timestamp.

**Parameters:**

- `truckId` (path): UUID of the truck

**Request:**

```json
{
  "status": 3
}
```

**Response (200):**

```json
true
```

**Status Values:**

- 1: Available
- 2: In Use
- 3: Maintenance
- 4: Broken Down
- 5: Retired

---

### 9. Update Maintenance Date

**PATCH** `/api/truck/updatemaintenance/{truckId}`

Updates the last maintenance date and updates the `UpdatedAt` timestamp.

**Parameters:**

- `truckId` (path): UUID of the truck

**Request:**

```json
{
  "maintenanceDate": "2026-03-14"
}
```

**Response (200):**

```json
true
```

---

## Data Models

### TruckDto

```json
{
  "truckId": "guid",
  "licensePlate": "string (20 chars)",
  "vinNumber": "string (50 chars)",
  "engineNumber": "string (50 chars)",
  "brand": "string (50 chars)",
  "modelYear": "integer",
  "purchaseDate": "date",
  "truckType": "string (50 chars)",
  "maxPayloadKg": "decimal (18,2)",
  "lengthMm": "integer",
  "widthMm": "integer",
  "heightMm": "integer",
  "ownershipType": "integer (1=owned, 2=leased)",
  "currentStatus": "integer (1-5)",
  "odometerReading": "decimal (18,2)",
  "lastMaintenanceDate": "datetime",
  "createdAt": "datetimeoffset",
  "updatedAt": "datetimeoffset or null"
}
```

### CreateTruckDto

Required fields:

- `licensePlate` (required, unique)

Optional fields:

- `vinNumber`, `engineNumber`, `brand`, `modelYear`, `purchaseDate`
- `truckType`, `maxPayloadKg`, `lengthMm`, `widthMm`, `heightMm`
- `ownershipType` (default: 1)

### UpdateTruckDto

All fields optional. Only provided fields will be updated.

---

## Testing with REST Client

Add these requests to `back_end_for_TMS.http`:

```http
### Create Truck
POST http://localhost:5000/api/truck/create
Authorization: Bearer {token}
Content-Type: application/json

{
  "licensePlate": "0001-AB01",
  "brand": "Hino",
  "modelYear": 2022,
  "purchaseDate": "2022-05-15",
  "truckType": "Thùng kín",
  "maxPayloadKg": 12000,
  "lengthMm": 8500,
  "widthMm": 2500,
  "heightMm": 2800,
  "ownershipType": 1
}

### Get Truck by ID
GET http://localhost:5000/api/truck/getbyid/{truckId}
Authorization: Bearer {token}

### List Trucks
GET http://localhost:5000/api/truck/list?pageNumber=1&pageSize=10
Authorization: Bearer {token}

### List Trucks - With Filters
GET http://localhost:5000/api/truck/list?pageNumber=1&pageSize=10&status=1&searchTerm=Hino
Authorization: Bearer {token}

### Update Truck Status
PATCH http://localhost:5000/api/truck/updatestatus/{truckId}
Authorization: Bearer {token}
Content-Type: application/json

{
  "status": 2
}

### Update Odometer
PATCH http://localhost:5000/api/truck/updateodometer/{truckId}
Authorization: Bearer {token}
Content-Type: application/json

{
  "odometerReading": 50000.50
}

### Update Full Truck Info
PUT http://localhost:5000/api/truck/update/{truckId}
Authorization: Bearer {token}
Content-Type: application/json

{
  "lastMaintenanceDate": "2026-03-10",
  "currentStatus": 3
}

### Delete Truck
DELETE http://localhost:5000/api/truck/delete/{truckId}
Authorization: Bearer {token}
```

---

## Implementation Details

### Architecture

- **Controller**: [TruckController](Api/TruckController.cs)
- **Service**: [TruckService](Business/TruckService.cs)
- **Model**: [AppTruck](Models/AppTruck.cs)
- **DTOs**: [TruckTypes](Business/Types/TruckTypes.cs)
- **Database**: Trucks table (auto-created from migration)

### Validation & Error Handling

- **Unique License Plate**: Cannot create/update truck with duplicate license plate
- **Not Found**: Returns 404 when truck doesn't exist
- **BadRequest**: Returns 400 for invalid input
- **Unauthorized**: Returns 401 if no valid token provided
- **Global Exception Handling**: All exceptions are caught and formatted via `GlobalExceptionHandler`

### Pagination

- Default page size: 10
- Maximum page size: 100
- Total pages calculated from total count and page size

### Filtering

- **Status Filter**: Search trucks by current status
- **Text Search**: Case-insensitive search on license plate and brand

### Database

- Migration: `20260314000000_AddTrucksTable`
- Table: `Trucks`
- Primary Key: `TruckId` (GUID)
- Unique Index on `LicensePlate`
- Sample data: 10 trucks seeded on application startup

---

## Authentication

All endpoints require a valid JWT Bearer token in the Authorization header:

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

Obtain a token by calling:

- **POST** `/api/account/login` - For existing users
- **POST** `/api/account/register` - To create new account

---

## Status: Complete (Phase 1.1)

All CRUD operations + List with pagination and filtering implemented and ready for use.

---

**Last Updated**: March 14, 2026
