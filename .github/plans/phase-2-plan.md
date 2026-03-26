# Phase 2 Plan: Multi-Tenant Foundation + Core Features

**Date**: 2026-03-22  
**Author**: Derek6_PO (Product Owner)  
**App**: EasyTMS — Truck Transportation Management System  
**Status**: Planning

---

## Goal

Establish data isolation so each household transportation business sees only its own data, then expand the domain with Driver management and core operational features.

## Strategy Decision

| Decision              | Choice                                            | Reasoning                                                                                                             |
| --------------------- | ------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------- |
| Multi-tenant approach | **Shared Table, Private Row** (application-level) | Practical for household businesses. Single database, single schema, data separated by `TenantId` column.              |
| Enforcement mechanism | **EF Core Global Query Filters**                  | Simple, fits existing EF Core patterns, testable, non-over-engineered. SQL Server RLS can be layered later if needed. |
| Tenant entity naming  | **`Tenant`** (not Company)                        | Generic, future-proof. A tenant represents one household business.                                                    |
| Tenant resolution     | **JWT claim → scoped `TenantContext` service**    | Stateless, no extra DB lookup per request. `TenantId` embedded in token at login.                                     |

---

## Sprint 1: Multi-Tenant Data Isolation

**Priority**: MUST HAVE — blocks all future features  
**Goal**: Every data entity is scoped to a Tenant. Users see only their business's data.

### Epic: Multi-Tenant Data Isolation

> As a business owner, I want my fleet data to be private to my business, so that other businesses cannot see, edit, or delete my trucks.

### User Stories

#### S-01: Tenant Entity & Migration [M]

> As a system, I need a Tenant entity so that each business has a unique identity in the database.

**Acceptance Criteria:**

- [ ] `Tenant` model exists with: `TenantId` (Guid v7), `Name` (required), `OwnerId` (FK to AppUser.Id), `CreatedAt`
- [ ] `AppUser` has a new `TenantId` (Guid, FK to Tenant) property
- [ ] EF migration creates `Tenants` table and adds `TenantId` column to `AspNetUsers`
- [ ] Seed data creates a default tenant and assigns existing seed users to it

#### S-02: Add TenantId to Truck [M]

> As a system, I need every Truck record linked to a Tenant so queries can be filtered by ownership.

**Acceptance Criteria:**

- [ ] `Truck` model has `TenantId` (Guid, required, FK to Tenant)
- [ ] EF migration adds `TenantId` column to `Trucks` table with index
- [ ] Existing seed trucks are assigned to the default tenant
- [ ] `TenantId` is NOT exposed in create/update DTOs (auto-stamped by system)

#### S-03: ITenantEntity Interface [S]

> As a developer, I need a shared interface so all tenant-scoped entities follow the same pattern.

**Acceptance Criteria:**

- [ ] `ITenantEntity` interface exists with `Guid TenantId { get; set; }` property
- [ ] `Truck` implements `ITenantEntity`
- [ ] Future entities (Driver, Order, etc.) will implement `ITenantEntity`

#### S-04: TenantContext Service [M]

> As the backend system, I need to know which tenant the current request belongs to, so I can scope data access.

**Acceptance Criteria:**

- [ ] `TenantContext` is a scoped service with `Guid TenantId` property
- [ ] A middleware or filter reads `TenantId` from JWT claims and sets it on `TenantContext`
- [ ] `TenantContext` is registered in DI
- [ ] Unauthenticated requests have `TenantId = Guid.Empty`

#### S-05: EF Core Global Query Filter [S]

> As a system, I need all queries to automatically filter by TenantId so data isolation is enforced without manual filtering.

**Acceptance Criteria:**

- [ ] `AppDbContext.OnModelCreating` applies `.HasQueryFilter()` to all `ITenantEntity` implementations
- [ ] `TruckService.ListAsync()` returns only trucks belonging to the current tenant — no code changes needed in the service
- [ ] Direct `DbSet<Truck>` queries are also filtered (global filter is on the entity, not the repo)

#### S-06: Auto-Stamp TenantId on Create [S]

> As a developer, I want TenantId to be automatically set when creating new records so I never forget to assign it.

**Acceptance Criteria:**

- [ ] `AppDbContext.SaveChangesAsync()` is overridden
- [ ] For any new entity implementing `ITenantEntity`, `TenantId` is set from `TenantContext.TenantId`
- [ ] `TruckService.CreateAsync()` does NOT manually set `TenantId` — it's auto-stamped

#### S-07: Update Auth Flow for Tenant [M]

> As a user, when I log in or register, my JWT should contain my TenantId so the system knows my business context.

**Acceptance Criteria:**

- [ ] **Register**: creates a new `Tenant` (name = user's email domain or "My Business"), assigns user to it, includes `TenantId` in JWT
- [ ] **Login**: reads user's `TenantId`, includes it in JWT claims
- [ ] **Refresh Token**: preserves `TenantId` claim in new token
- [ ] **GetMe**: returns `TenantId` and `TenantName` in `UserProfile` DTO

#### S-08: Frontend — Store Tenant Context [S]

> As the frontend, I need to know the current tenant so I can display the business name and pass context if needed.

**Acceptance Criteria:**

- [ ] `AuthService` stores `tenantId` and `tenantName` from login/register response or `GetMe`
- [ ] Tenant name is accessible for display in sidebar or header
- [ ] No tenant selector needed (one user = one tenant for now)

### Tasks (Sprint 1)

| #   | Task                                                              | Agent    | Story      | Depends On |
| --- | ----------------------------------------------------------------- | -------- | ---------- | ---------- |
| 1   | [BE] Create `ITenantEntity` interface                             | Backend  | S-03       | —          |
| 2   | [BE] Create `Tenant` model                                        | Backend  | S-01       | 1          |
| 3   | [BE] Add `TenantId` to `AppUser`                                  | Backend  | S-01       | 2          |
| 4   | [BE] Add `TenantId` to `Truck` (implement `ITenantEntity`)        | Backend  | S-02       | 1          |
| 5   | [BE] Create EF migration for Tenant + FK columns                  | Backend  | S-01, S-02 | 2, 3, 4    |
| 6   | [BE] Update seed data (default tenant, assign to users & trucks)  | Backend  | S-01, S-02 | 5          |
| 7   | [BE] Create `TenantContext` scoped service                        | Backend  | S-04       | —          |
| 8   | [BE] Create tenant resolution middleware (JWT → TenantContext)    | Backend  | S-04       | 7          |
| 9   | [BE] Add Global Query Filter for `ITenantEntity`                  | Backend  | S-05       | 7, 4       |
| 10  | [BE] Override `SaveChangesAsync` for auto-stamp                   | Backend  | S-06       | 7, 1       |
| 11  | [BE] Update `TokenService` — add `TenantId` claim to JWT          | Backend  | S-07       | 3          |
| 12  | [BE] Update `AccountService.Register` — create Tenant on register | Backend  | S-07       | 2, 11      |
| 13  | [BE] Update `AccountService.Login` — include TenantId             | Backend  | S-07       | 11         |
| 14  | [BE] Update `AccountService.GetMe` — return TenantId + TenantName | Backend  | S-07       | 2          |
| 15  | [FE] Update `AuthService` + models — store tenant info            | Frontend | S-08       | 14         |
| 16  | [FE] Display tenant name in sidebar header                        | Frontend | S-08       | 15         |

---

## Sprint 2: Driver Management

**Priority**: HIGH  
**Goal**: CRUD for drivers with tenant scoping and proper license status management.

### Epic: Driver Management

> As a fleet manager, I want to manage my drivers so I can track who is available and their license status.

**Scope note:** Driver-truck assignment is **out of scope** for this sprint. Assignment will happen at the Trip level when the Trips module is built.

### Status Design

#### Driver Status (employment)

All manual — user sets it, system stores it, no computation.

| DriverStatus (enum) | Meaning                              | Color  |
| ------------------- | ------------------------------------ | ------ |
| `Active = 0`        | Currently working                    | green  |
| `OnLeave = 1`       | Temporarily unavailable              | orange |
| `Terminated = 2`    | No longer employed, kept for history | red    |

Simple because it should be. No computed states needed — a manager knows when someone is on leave or terminated.

#### License Status (driving permit)

Mixed — user controls the manual states, system computes time-based states from `LicenseExpiry` date.

**Stored in DB** (`LicenseStatus` enum — only what the user controls):

| LicenseStatus (enum) | Meaning                      |
| -------------------- | ---------------------------- |
| `Valid = 0`          | License is fine              |
| `Blocked = 1`        | Suspended by authority       |
| `Lost = 2`           | Physical license lost/stolen |

**Computed at read time** (`EffectiveLicenseStatus` in `DriverDto` — no DB column):

| Priority | Condition                      | EffectiveLicenseStatus | Color    |
| -------- | ------------------------------ | ---------------------- | -------- |
| 1        | `LicenseStatus == Blocked`     | `Blocked`              | dark red |
| 2        | `LicenseStatus == Lost`        | `Lost`                 | gray     |
| 3        | `LicenseExpiry < today`        | `Expired`              | red      |
| 4        | `LicenseExpiry` within 30 days | `ExpiringSoon`         | orange   |
| 5        | Otherwise                      | `Valid`                | green    |

Why this split: expiry is a date fact — storing "Expired" as a status would go stale and need a background job to update. Compute it instead.

### User Stories

#### S-09: Driver Model & Migration [M]

> As a system, I need a Driver entity to store driver information scoped to a tenant.

**Acceptance Criteria:**

- [ ] `Driver` model exists with: `DriverId` (Guid v7), `TenantId` (FK), `FullName`, `PhoneNumber`, `LicenseNumber`, `LicenseClass`, `LicenseExpiry` (DateOnly), `LicenseStatus` (int enum), `DateOfBirth`, `Status` (int enum), `HireDate`, `Notes`, `CreatedAt`, `UpdatedAt`
- [ ] `DriverStatus` enum: `Active = 0`, `OnLeave = 1`, `Terminated = 2`
- [ ] `LicenseStatus` enum: `Valid = 0`, `Blocked = 1`, `Lost = 2` — only manual states; expiry-based states are computed
- [ ] Implements `ITenantEntity`
- [ ] `DbSet<Driver>` registered in `AppDbContext`
- [ ] EF migration creates `Drivers` table with index on `TenantId`
- [ ] Seed data generates sample drivers for default tenant (include variety: valid, expired, expiring soon)

#### S-10: Driver CRUD API [L]

> As a fleet manager, I want API endpoints to create, read, update, delete, and list drivers.

**Acceptance Criteria:**

- [ ] `DriverController` with: `Create`, `GetById`, `List` (paginated + search + filter), `Update`, `Delete`
- [ ] `DriverService` with business logic and validation (unique phone per tenant, required fields)
- [ ] `DriverRepo` following same pattern as `TruckRepo`
- [ ] DTOs: `CreateDriverDto`, `UpdateDriverDto`, `DriverDto`
- [ ] `DriverDto` includes `EffectiveLicenseStatus` (string) — computed in service from `LicenseStatus` + `LicenseExpiry`
- [ ] List supports filter by: `Status` (driver employment status), `EffectiveLicenseStatus`, search by name or phone
- [ ] All endpoints `[Authorize]`, data scoped by tenant (via global filter)

#### S-11: Driver List Page [L]

> As a fleet manager, I want to see all my drivers in a searchable, filterable table.

**Acceptance Criteria:**

- [ ] PrimeNG DataTable at `/drivers/list` with lazy loading
- [ ] Columns: FullName, PhoneNumber, LicenseNumber, LicenseClass, LicenseExpiry, License Status, Driver Status, HireDate
- [ ] **Driver Status** tags: `Active` (green), `OnLeave` (orange), `Terminated` (red)
- [ ] **License Status** tags using `EffectiveLicenseStatus` from API: `Valid` (green), `ExpiringSoon` (orange), `Expired` (red), `Blocked` (dark red), `Lost` (gray)
- [ ] Search by name or phone number
- [ ] Filter by driver status dropdown
- [ ] Filter by license status dropdown (all 5 effective values)

#### S-12: Driver Form Dialog [M]

> As a fleet manager, I want to create and edit driver details through a form.

**Acceptance Criteria:**

- [ ] PrimeNG Dialog with form fields for all driver properties
- [ ] Reusable for both Create and Edit modes
- [ ] `LicenseStatus` dropdown shows only **manual** states: Valid, Blocked, Lost (user doesn't set Expired/ExpiringSoon — those are computed)
- [ ] Validation: required fields (FullName, PhoneNumber, LicenseNumber), date format, phone format
- [ ] On save: calls API, refreshes table, shows toast

#### ~~S-13: Driver-Truck Assignment~~ — REMOVED

> **Reason**: Driver-truck assignment belongs at the Trip level. When we build Trips, a driver is assigned to a truck for a specific trip — not as a permanent standalone relationship. Removed to keep scope clean.

#### S-14: Driver Salary Page (Placeholder) [S]

> As a fleet manager, I want a salary page so I can see it's coming and the nav link works.

**Acceptance Criteria:**

- [ ] Page at `/drivers/salary` with heading and description
- [ ] Placeholder message: "Salary management will be available in a future update"
- [ ] Nav link works (no more "under-development" redirect)

### Tasks (Sprint 2)

| #   | Task                                                                                               | Agent    | Story      | Depends On |
| --- | -------------------------------------------------------------------------------------------------- | -------- | ---------- | ---------- |
| 1   | [BE] Create `DriverStatus` + `LicenseStatus` enums, `Driver` model (implements `ITenantEntity`)    | Backend  | S-09       | Sprint 1   |
| 2   | [BE] Create EF migration for Drivers table                                                         | Backend  | S-09       | 1          |
| 3   | [BE] Create seed data for Drivers                                                                  | Backend  | S-09       | 2          |
| 4   | [BE] Create `DriverRepo` (same pattern as `TruckRepo`)                                             | Backend  | S-10       | 1          |
| 5   | [BE] Create DTOs (`CreateDriverDto`, `UpdateDriverDto`, `DriverDto` with `EffectiveLicenseStatus`) | Backend  | S-10       | 1          |
| 6   | [BE] Create `DriverService` with CRUD + validation + effective license status logic                | Backend  | S-10       | 4, 5       |
| 7   | [BE] Create `DriverController`                                                                     | Backend  | S-10       | 6          |
| 8   | [BE] Add AutoMapper profile for Driver                                                             | Backend  | S-10       | 1, 5       |
| 9   | [FE] Create driver models/DTOs                                                                     | Frontend | S-11       | 5          |
| 10  | [FE] Create `DriverService` (HTTP client)                                                          | Frontend | S-11       | 7          |
| 11  | [FE] Create Driver list page with PrimeNG DataTable + license status tags                          | Frontend | S-11       | 9, 10      |
| 12  | [FE] Create Driver form dialog                                                                     | Frontend | S-12       | 9, 10      |
| 13  | [FE] Add driver routes (`/drivers/list`, `/drivers/salary`)                                        | Frontend | S-11, S-14 | 11         |
| 14  | [FE] Update navbar — wire driver links to real routes                                              | Frontend | S-11       | 13         |
| 15  | [FE] Create salary placeholder page                                                                | Frontend | S-14       | 13         |

---

## Sprint 3: Core Operations

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

| #   | Task                                                  | Agent    | Story | Depends On |
| --- | ----------------------------------------------------- | -------- | ----- | ---------- |
| 1   | [BE] Create `GET /api/Dashboard/Summary` endpoint     | Backend  | S-15  | Sprint 1   |
| 2   | [FE] Build dashboard page with KPI cards              | Frontend | S-15  | 1          |
| 3   | [BE] Add `UpdateProfile` + `ChangePassword` endpoints | Backend  | S-16  | —          |
| 4   | [FE] Build profile page                               | Frontend | S-16  | 3          |
| 5   | [BE] Add `ForgotPassword` + `ResetPassword` endpoints | Backend  | S-17  | —          |
| 6   | [FE] Wire reset password page to backend              | Frontend | S-17  | 5          |
| 7   | [BE] Create `MaintenanceRecord` model + migration     | Backend  | S-18  | Sprint 1   |
| 8   | [BE] Create maintenance CRUD API                      | Backend  | S-18  | 7          |
| 9   | [FE] Build truck maintenance page with records table  | Frontend | S-18  | 8          |
| 10  | [FE] Add role-based visibility logic                  | Frontend | S-19  | —          |

---

## Summary

| Sub-Phase | Sprint   | Focus                       | Stories                      | Blocking?                 |
| --------- | -------- | --------------------------- | ---------------------------- | ------------------------- |
| **2A**    | Sprint 1 | Multi-Tenant Data Isolation | S-01 → S-08                  | Yes — blocks everything   |
| **2B**    | Sprint 2 | Driver Management           | S-09, S-10, S-11, S-12, S-14 | No — independent after 2A |
| **2C**    | Sprint 3 | Core Operations & Polish    | S-15 → S-19                  | No — independent after 2A |

**Total stories**: 18 (S-13 removed — driver-truck assignment deferred to Trips module)  
**Agents**: Derek6_Backend + Derek6_Frontend  
**Approach**: Correct, smart, balanced, practical, non-over-engineered. Store only what the user controls; compute the rest.
