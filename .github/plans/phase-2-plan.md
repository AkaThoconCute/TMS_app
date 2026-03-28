# Phase 2 Plan: Multi-Tenant Foundation + Core Features

**Date**: 2026-03-22  
**Author**: Derek6_PO (Product Owner)  
**App**: EasyTMS — Truck Transportation Management System  
**Status**: Planning

---

## Goal

Establish data isolation so each household transportation business sees only its own data, then expand the domain with Driver management and core operational features.

## Strategy Decision

| Decision | Choice | Reasoning |
|---|---|---|
| Multi-tenant approach | **Shared Table, Private Row** (application-level) | Practical for household businesses. Single database, single schema, data separated by `TenantId` column. |
| Enforcement mechanism | **EF Core Global Query Filters** | Simple, fits existing EF Core patterns, testable, non-over-engineered. SQL Server RLS can be layered later if needed. |
| Tenant entity naming | **`Tenant`** (not Company) | Generic, future-proof. A tenant represents one household business. |
| Tenant resolution | **JWT claim → scoped `TenantContext` service** | Stateless, no extra DB lookup per request. `TenantId` embedded in token at login. |

---

## Phase 2B: Driver Management (Sprint 2)

**Priority**: HIGH  
**Goal**: CRUD for drivers with tenant scoping, and basic driver-truck assignment.

### Epic: Driver Management

> As a fleet manager, I want to manage my drivers so I can track who is available, their license status, and which truck they're assigned to.

### User Stories

#### S-09: Driver Model & Migration [M]

> As a system, I need a Driver entity to store driver information scoped to a tenant.

**Acceptance Criteria:**
- [ ] `Driver` model exists with: `DriverId` (Guid v7), `TenantId` (FK), `FullName`, `PhoneNumber`, `LicenseNumber`, `LicenseClass`, `LicenseExpiry`, `DateOfBirth`, `Status` (int), `HireDate`, `Notes`, `CreatedAt`, `UpdatedAt`
- [ ] Implements `ITenantEntity`
- [ ] `DbSet<Driver>` registered in `AppDbContext`
- [ ] EF migration creates `Drivers` table with index on `TenantId`
- [ ] Seed data generates sample drivers for default tenant

#### S-10: Driver CRUD API [L]

> As a fleet manager, I want API endpoints to create, read, update, delete, and list drivers.

**Acceptance Criteria:**
- [ ] `DriverController` with: `Create`, `GetById`, `List` (paginated + search + status filter), `Update`, `Delete`
- [ ] `DriverService` with business logic and validation (unique phone per tenant, license expiry checks)
- [ ] `DriverRepo` following same pattern as `TruckRepo`
- [ ] DTOs: `CreateDriverDto`, `UpdateDriverDto`, `DriverDto`
- [ ] All endpoints `[Authorize]`, data scoped by tenant (via global filter)

#### S-11: Driver List Page [L]

> As a fleet manager, I want to see all my drivers in a searchable, filterable table.

**Acceptance Criteria:**
- [ ] PrimeNG DataTable at `/drivers/list` with lazy loading
- [ ] Columns: FullName, PhoneNumber, LicenseNumber, LicenseClass, LicenseExpiry, Status, HireDate
- [ ] Status tags with color coding (Active, OnLeave, Terminated)
- [ ] Search by name or phone number
- [ ] Filter by status dropdown
- [ ] License expiry warning (highlight if expiring within 30 days)

#### S-12: Driver Form Dialog [M]

> As a fleet manager, I want to create and edit driver details through a form.

**Acceptance Criteria:**
- [ ] PrimeNG Dialog with form fields for all driver properties
- [ ] Reusable for both Create and Edit modes
- [ ] Validation: required fields (FullName, PhoneNumber, LicenseNumber), date format, phone format
- [ ] On save: calls API, refreshes table, shows toast

#### S-13: Driver-Truck Assignment [M]

> As a fleet manager, I want to assign a driver to a truck so I know who is driving which vehicle.

**Acceptance Criteria:**
- [ ] `Truck` gets nullable `CurrentDriverId` (FK to Driver)
- [ ] API endpoint `PATCH /api/Truck/{id}/assign-driver` with `{ driverId: Guid? }`
- [ ] Assigning a driver already assigned to another truck unassigns them from the old truck
- [ ] Driver status is shown on the Truck list (column: "Current Driver")
- [ ] Truck assignment is shown on the Driver list (column: "Assigned Truck")

#### S-14: Driver Salary Page (Placeholder) [S]

> As a fleet manager, I want a salary page so I can see it's coming and the nav link works.

**Acceptance Criteria:**
- [ ] Page at `/drivers/salary` with heading and description
- [ ] Placeholder message: "Salary management will be available in a future update"
- [ ] Nav link works (no more "under-development" redirect)

### Tasks (Sprint 2)

| # | Task | Agent | Story | Depends On |
|---|---|---|---|---|
| 1 | [BE] Create `Driver` model (implements `ITenantEntity`) | Backend | S-09 | Phase 2A |
| 2 | [BE] Create EF migration for Drivers table | Backend | S-09 | 1 |
| 3 | [BE] Create seed data for Drivers | Backend | S-09 | 2 |
| 4 | [BE] Create `DriverRepo` | Backend | S-10 | 1 |
| 5 | [BE] Create DTOs (`CreateDriverDto`, `UpdateDriverDto`, `DriverDto`) | Backend | S-10 | 1 |
| 6 | [BE] Create `DriverService` with CRUD + validation | Backend | S-10 | 4, 5 |
| 7 | [BE] Create `DriverController` | Backend | S-10 | 6 |
| 8 | [BE] Add AutoMapper profile for Driver | Backend | S-10 | 1, 5 |
| 9 | [BE] Add `CurrentDriverId` FK to Truck + migration | Backend | S-13 | 1 |
| 10 | [BE] Add assign-driver endpoint to TruckController | Backend | S-13 | 6, 9 |
| 11 | [FE] Create `DriverService` (HTTP client) | Frontend | S-11 | 7 |
| 12 | [FE] Create driver models/DTOs | Frontend | S-11 | 5 |
| 13 | [FE] Create Driver list page with PrimeNG DataTable | Frontend | S-11 | 11, 12 |
| 14 | [FE] Create Driver form dialog | Frontend | S-12 | 11, 12 |
| 15 | [FE] Add driver routes (`/drivers/list`, `/drivers/salary`) | Frontend | S-11, S-14 | 13 |
| 16 | [FE] Update navbar — wire driver links to real routes | Frontend | S-11 | 15 |
| 17 | [FE] Create salary placeholder page | Frontend | S-14 | 15 |
| 18 | [FE] Show driver assignment on truck list | Frontend | S-13 | 10, 13 |

---

## Phase 2C: Core Operations (Sprint 3)

**Priority**: SHOULD HAVE  
**Goal**: Polish the foundation with dashboard, profile management, and completing partial features.

### Epic: Core Operations & Polish

> As a user, I want a functional dashboard, the ability to manage my profile, and completed features so the app feels production-ready.

### User Stories

#### S-15: Dashboard Page [M]

> As a fleet manager, I want a dashboard showing key metrics at a glance.

**Acceptance Criteria:**
- [ ] Home page (`/home`) displays KPI cards: Total Trucks, Available Trucks, In-Use Trucks, Total Drivers, Active Drivers
- [ ] Cards use PrimeNG Card component with icons and color coding
- [ ] Data fetched from new summary API endpoint `GET /api/Dashboard/Summary`
- [ ] Counts are tenant-scoped (global filter handles it)

#### S-16: User Profile Page [S]

> As a user, I want to view and edit my profile information.

**Acceptance Criteria:**
- [ ] Page at `/profile` showing: email (read-only), username, tenant name (read-only)
- [ ] Edit username functionality
- [ ] Change password form (current password + new password + confirm)
- [ ] Uses Identity's built-in password change

#### S-17: Reset Password (Complete) [S]

> As a user, I want to reset my password if I forget it.

**Acceptance Criteria:**
- [ ] Backend: `ForgotPassword` endpoint sends reset token (log to console for now — email integration later)
- [ ] Backend: `ResetPassword` endpoint accepts token + new password
- [ ] Frontend: Reset password page wired to backend endpoints
- [ ] Flow: enter email → receive token → enter new password → success

#### S-18: Truck Maintenance Records [M]

> As a fleet manager, I want to log maintenance activities against my trucks.

**Acceptance Criteria:**
- [ ] `MaintenanceRecord` model: `RecordId`, `TenantId`, `TruckId` (FK), `MaintenanceDate`, `Description`, `Cost`, `NextDueDate`, `CreatedAt`
- [ ] CRUD API for maintenance records scoped to a truck
- [ ] Truck maintenance page shows records for selected truck
- [ ] PrimeNG DataTable with sorting by date
- [ ] Link from truck list → maintenance records for that truck

#### S-19: Role-Based UI [S]

> As an admin, I want to see admin-specific options that regular users don't see.

**Acceptance Criteria:**
- [ ] Frontend reads roles from JWT/profile
- [ ] Admin menu items shown only to Admin role users
- [ ] Structural directive or signal-based check for role-based visibility

### Tasks (Sprint 3)

| # | Task | Agent | Story | Depends On |
|---|---|---|---|---|
| 1 | [BE] Create `GET /api/Dashboard/Summary` endpoint | Backend | S-15 | Phase 2A |
| 2 | [FE] Build dashboard page with KPI cards | Frontend | S-15 | 1 |
| 3 | [BE] Add `UpdateProfile` + `ChangePassword` endpoints | Backend | S-16 | — |
| 4 | [FE] Build profile page | Frontend | S-16 | 3 |
| 5 | [BE] Add `ForgotPassword` + `ResetPassword` endpoints | Backend | S-17 | — |
| 6 | [FE] Wire reset password page to backend | Frontend | S-17 | 5 |
| 7 | [BE] Create `MaintenanceRecord` model + migration | Backend | S-18 | Phase 2A |
| 8 | [BE] Create maintenance CRUD API | Backend | S-18 | 7 |
| 9 | [FE] Build truck maintenance page with records table | Frontend | S-18 | 8 |
| 10 | [FE] Add role-based visibility logic | Frontend | S-19 | — |

---

## Summary

| Sub-Phase | Sprint | Focus | Stories | Blocking? |
|---|---|---|---|---|
| **2A** | Sprint 1 | Multi-Tenant Data Isolation | S-01 → S-08 | Yes — blocks everything |
| **2B** | Sprint 2 | Driver Management | S-09 → S-14 | No — independent after 2A |
| **2C** | Sprint 3 | Core Operations & Polish | S-15 → S-19 | No — independent after 2A |

**Total stories**: 19  
**Agents**: Derek6_Backend + Derek6_Frontend  
**Approach**: Practical, non-over-engineered, application-level multi-tenancy with clear upgrade path to SQL Server RLS if needed in the future.
