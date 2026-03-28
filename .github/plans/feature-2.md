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
**Status**: Done
**Plan**: See [sprint-1-plan.md](sprint-1-plan.md) for full stories, acceptance criteria, tasks, and notes.

**Stories**: S-01 → S-08 (all Done)  
**Scope**: Tenant entity, TenantId on all entities, global query filters, auto-stamp, JWT tenant claim, frontend tenant context

---

## Sprint 2: Driver Management

**Priority**: HIGH  
**Goal**: CRUD for drivers with tenant scoping, license tracking, and license expiry warnings.  
**Status**: Active  
**Plan**: See [sprint-2-plan.md](sprint-2-plan.md) for full stories, acceptance criteria, tasks, and notes.

**Stories**: S-09 (Done), S-10, S-11, S-12, S-14  
**Scope**: Driver core data CRUD, license management, salary placeholder page  
**Out of Scope**: Timeline / history tracking, driver-truck assignment, salary calculation

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
| 1   | [BE] Create `GET /api/Dashboard/Summary` endpoint     | Backend  | S-15  | Phase 2A   |
| 2   | [FE] Build dashboard page with KPI cards              | Frontend | S-15  | 1          |
| 3   | [BE] Add `UpdateProfile` + `ChangePassword` endpoints | Backend  | S-16  | —          |
| 4   | [FE] Build profile page                               | Frontend | S-16  | 3          |
| 5   | [BE] Add `ForgotPassword` + `ResetPassword` endpoints | Backend  | S-17  | —          |
| 6   | [FE] Wire reset password page to backend              | Frontend | S-17  | 5          |
| 7   | [BE] Create `MaintenanceRecord` model + migration     | Backend  | S-18  | Phase 2A   |
| 8   | [BE] Create maintenance CRUD API                      | Backend  | S-18  | 7          |
| 9   | [FE] Build truck maintenance page with records table  | Frontend | S-18  | 8          |
| 10  | [FE] Add role-based visibility logic                  | Frontend | S-19  | —          |

---

## Summary

| Sub-Phase | Sprint   | Focus                       | Stories           | Blocking?                 | Plan                                 |
| --------- | -------- | --------------------------- | ----------------- | ------------------------- | ------------------------------------ |
| **2A**    | Sprint 1 | Multi-Tenant Data Isolation | S-01 → S-08       | Yes — blocks everything   | [sprint-1-plan.md](sprint-1-plan.md) |
| **2B**    | Sprint 2 | Driver Management           | S-09 → S-12, S-14 | No — independent after 2A | [sprint-2-plan.md](sprint-2-plan.md) |
| **2C**    | Sprint 3 | Core Operations & Polish    | S-15 → S-19       | No — independent after 2A | TBD                                  |

**Deferred stories**: S-13 (Driver-Truck Assignment) — moved out of Sprint 2, to be scheduled in a future sprint  
**Total stories**: 18 (was 19 — S-13 deferred)  
**Agents**: Derek6_Backend + Derek6_Frontend  
**Approach**: Practical, non-over-engineered, application-level multi-tenancy with clear upgrade path to SQL Server RLS if needed in the future.
