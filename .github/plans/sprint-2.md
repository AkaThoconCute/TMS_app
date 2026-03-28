# Sprint 2: Driver Management

**Date**: 2026-03-28  
**Author**: Derek6_PO (Product Owner)  
**App**: EasyTMS — Truck Transportation Management System  
**Status**: Active

---

## Goal

> CRUD for drivers with tenant scoping, license tracking, and license expiry warnings.

**Priority**: HIGH  
**Depends On**: Sprint 1 (Multi-Tenant Data Isolation) — Done

**Scope Decision**

| In Scope                                                       | Out of Scope                                               |
| -------------------------------------------------------------- | ---------------------------------------------------------- |
| Driver core data CRUD (create, read, update, delete, list)     | Driver-Truck assignment (S-13 — deferred to future sprint) |
| Driver license management (license number, class, expiry)      | Timeline / history tracking                                |
| License expiry warnings (highlight if expiring within 30 days) | Salary calculation                                         |
| Paginated, searchable, filterable driver list                  |                                                            |
| Driver form dialog (create + edit)                             |                                                            |
| Salary placeholder page (nav link already exists)              |                                                            |

---

## Epic: Driver Management

> As a fleet manager, I want to manage my drivers so I can track who is available and their license status.

## User Stories

### S-09: Driver Model & Migration [M]

> As a system, I need a Driver entity to store driver information scoped to a tenant.

**Acceptance Criteria:**

- [x] `Driver` model exists with: `DriverId` (Guid v7), `TenantId` (FK), `FullName`, `PhoneNumber`, `LicenseNumber`, `LicenseClass`, `LicenseExpiry`, `DateOfBirth`, `Status` (int), `HireDate`, `Notes`, `CreatedAt`, `UpdatedAt`
- [x] Implements `ITenantEntity`
- [x] `DbSet<Driver>` registered in `AppDbContext`
- [x] EF migration creates `Drivers` table with index on `TenantId`
- [x] Seed data generates 8 sample drivers for default tenant

### S-10: Driver CRUD API [L]

> As a fleet manager, I want API endpoints to create, read, update, delete, and list drivers.

**Acceptance Criteria:**

- [x] `DriverController` with: `Create`, `GetById`, `List` (paginated + search + status filter), `Update`, `Delete`
- [x] `DriverService` with business logic and validation:
  - Unique phone number per tenant
  - License expiry checks (warn if expiring within 30 days)
  - Required field validation (FullName, PhoneNumber, LicenseNumber)
- [x] DTOs: `CreateDriverDto`, `UpdateDriverDto`, `DriverDto`
- [x] `DriverDto` includes computed `IsLicenseExpiringSoon` (bool) — true if `LicenseExpiry` is within 30 days or already expired
- [x] AutoMapper profile for Driver ↔ DTOs
- [x] All endpoints `[Authorize]`, data scoped by tenant (via global filter)
- [x] Follows same patterns as `TruckController` / `TruckService`

### S-11: Driver List Page [L]

> As a fleet manager, I want to see all my drivers in a searchable, filterable table.

**Acceptance Criteria:**

- [x] PrimeNG DataTable at `/drivers/list` with lazy loading (server-side pagination)
- [x] Columns: FullName, PhoneNumber, LicenseNumber, LicenseClass, LicenseExpiry, Status, HireDate
- [x] Status tags with color coding:
  - `Active` → green
  - `OnLeave` → orange/yellow
  - `Terminated` → red
- [x] Search by name or phone number (single search input)
- [x] Filter by status dropdown
- [x] License expiry warning: highlight row/cell if license expiring within 30 days (red text or warning icon)
- [x] Follows same patterns as Truck list page

### S-12: Driver Form Dialog [M]

> As a fleet manager, I want to create and edit driver details through a form.

**Acceptance Criteria:**

- [x] PrimeNG Dialog with form fields for all driver properties:
  - FullName (text, required)
  - PhoneNumber (text, required)
  - LicenseNumber (text, required)
  - LicenseClass (dropdown: B2, C, D, FC)
  - LicenseExpiry (date picker)
  - DateOfBirth (date picker)
  - Status (dropdown: Active, On Leave, Terminated)
  - HireDate (date picker)
  - Notes (textarea)
- [x] Reusable for both Create and Edit modes
- [x] Validation: required fields, phone format
- [x] On save: calls API, refreshes table, shows success toast
- [x] Follows same patterns as Truck form dialog

### S-14: Driver Salary Page (Placeholder) [S]

> As a fleet manager, I want a salary page so I can see it's coming and the nav link works.

**Acceptance Criteria:**

- [x] Page at `/drivers/salary` with heading and description
- [x] Placeholder message: "Salary management will be available in a future update"
- [x] Nav link works (no more `under-development` redirect)

---

## Tasks

| #   | Task                                                                 | Agent    | Story      | Depends On | Status | Completed  |
| --- | -------------------------------------------------------------------- | -------- | ---------- | ---------- | ------ | ---------- |
| 1   | [BE] Create `Driver` model (implements `ITenantEntity`)              | Backend  | S-09       | Phase 2A   | Done   | 2026-03-26 |
| 2   | [BE] Create EF migration for Drivers table                           | Backend  | S-09       | 1          | Done   | 2026-03-26 |
| 3   | [BE] Create seed data for Drivers                                    | Backend  | S-09       | 2          | Done   | 2026-03-26 |
| 4   | [BE] Create `DriverRepo`                                             | Backend  | S-10       | 1          | Done   | 2026-03-26 |
| 5   | [BE] Create DTOs (`CreateDriverDto`, `UpdateDriverDto`, `DriverDto`) | Backend  | S-10       | 1          | Done   | 2026-03-28 |
| 6   | [BE] Add AutoMapper profile for Driver                               | Backend  | S-10       | 1, 5       | Done   | 2026-03-28 |
| 7   | [BE] Create `DriverService` with CRUD + validation                   | Backend  | S-10       | 4, 5       | Done   | 2026-03-28 |
| 8   | [BE] Create `DriverController`                                       | Backend  | S-10       | 7          | Done   | 2026-03-28 |
| 9   | [FE] Create driver models/DTOs                                       | Frontend | S-11       | 5          | Done   | 2026-03-28 |
| 10  | [FE] Create `DriverService` (HTTP client)                            | Frontend | S-11       | 8          | Done   | 2026-03-28 |
| 11  | [FE] Create Driver list page with PrimeNG DataTable                  | Frontend | S-11       | 9, 10      | Done   | 2026-03-28 |
| 12  | [FE] Create Driver form dialog                                       | Frontend | S-12       | 9, 10      | Done   | 2026-03-28 |
| 13  | [FE] Add driver routes (`/drivers/list`, `/drivers/salary`)          | Frontend | S-11, S-14 | 11         | Done   | 2026-03-28 |
| 14  | [FE] Update navbar — wire driver links to real routes                | Frontend | S-11       | 13         | Done   | 2026-03-28 |
| 15  | [FE] Create salary placeholder page                                  | Frontend | S-14       | 13         | Done   | 2026-03-28 |

---

## Notes

**Tasks 1-4: [BE] Driver model, migration, seed data, repo**  
**Status:** Done  
**Details:**

- Task 1: `Driver` model created with all fields (DriverId, TenantId, FullName, PhoneNumber, LicenseNumber, LicenseClass, LicenseExpiry, DateOfBirth, Status, HireDate, Notes, CreatedAt, UpdatedAt). Implements `ITenantEntity`. Status uses int: 1=Active, 2=OnLeave, 3=Terminated.
- Task 2: `DriverModelConfig` created. Configures Tenant FK (Restrict delete), TenantId index, seed data. Migration applied successfully.
- Task 3: `DriverDataSeeder` created. Generates 8 drivers with Bogus, all assigned to Alpha tenant. License classes: B2, C, D, FC.
- Task 4: `DriverRepo` created. Standard repo pattern matching `TruckRepo`: Find, Query, Add, Update, Remove, SaveChanges.
- `DbSet<Driver>` registered in `AppDbContext`. Driver model + seeding invoked in `OnModelCreating`.
- Global query filter automatically applies via `ITenantEntity` loop. Auto-stamp `TenantId` on create via `SaveChangesAsync` override.

**Tasks 5-8: [BE] DTOs, AutoMapper, DriverService, DriverController**  
**Status:** Done  
**Details:**

- Task 5: `DriverTypes.cs` created in `Business/Types/` with `CreateDriverDto`, `UpdateDriverDto`, `DriverDto`. `DriverDto` includes computed `IsLicenseExpiringSoon` (bool).
- Task 6: `AppMapperProfile.cs` updated with Driver mappings. `UpdateDriverDto → Driver` skips null fields. `Driver → DriverDto` computes `IsLicenseExpiringSoon` (true if LicenseExpiry ≤ 30 days from now).
- Task 7: `DriverService.cs` created with `CreateAsync`, `GetByIdAsync`, `ListAsync` (paginated + search by name/phone + status filter), `UpdateAsync`, `DeleteAsync`. Validates required fields, enforces unique phone per tenant. Registered as scoped in `BusinessExtensions.cs`.
- Task 8: `DriverController.cs` created with 5 endpoints: Create (POST), GetById (GET), List (GET), Update (PUT), Delete (DELETE). All `[Authorize]`, follows TruckController pattern exactly.

**Tasks 9-10: [FE] Driver models & HTTP service**  
**Status:** Done  
**Details:**

- Task 9: `driver.models.ts` created in `features/driver/models/` with `ApiResult<T>`, `DriverDto` (13 fields including `isLicenseExpiringSoon`), and `PaginatedDriversDto`.
- Task 10: `driver.service.ts` created in `features/driver/services/` with `create`, `getById`, `list`, `update`, `delete` methods. Uses `inject()` pattern, unwraps `ApiResult<T>`, matches TruckService pattern exactly.

**Tasks 11-15: [FE] Driver list page, form dialog, routes, navbar, salary page**  
**Status:** Done  
**Details:**

- Task 11: `driver-list.page.ts` + `driver-list.page.html` created in `features/driver/pages/driver-list/`. PrimeNG DataTable with lazy loading, server-side pagination (5/10/20 rows). Search input (by name/phone, Enter or click). Status filter dropdown (All/Active/On Leave/Terminated). License expiry warning: red text + `pi-exclamation-triangle` icon when `isLicenseExpiringSoon === true`. Status tags: Active=green, On Leave=warn, Terminated=danger. Edit/Delete per row with confirm dialog + toast.
- Task 12: `driver-form-dialog.ts` + `driver-form-dialog.html` created in `features/driver/components/driver-form-dialog/`. 2-column grid form: FullName (required), PhoneNumber (required), LicenseNumber (required), LicenseClass (Select: B2/C/D/FC), LicenseExpiry (DatePicker), DateOfBirth (DatePicker), Status (Select: Active/On Leave/Terminated, default 1), HireDate (DatePicker), Notes (Textarea, col-span-2). Uses `model()`/`input()`/`output()` pattern, `effect()` for edit mode population. Matches TruckFormDialog exactly.
- Task 13: `driver.routes.ts` created with `list` and `salary` child routes. `app.routes.ts` updated — `drivers` lazy-loaded path added in PrivateLayout after `trucks`.
- Task 14: `navbar.ts` updated — "List driver" route changed from `/drivers` to `/drivers/list`.
- Task 15: `driver-salary.page.ts` created in `features/driver/pages/driver-salary/`. Inline template placeholder: "Salary management will be available in a future update." Follows TruckMaintenancePage pattern.
