# Sprint 1 Plan: Multi-Tenant Data Isolation

**Date**: 2026-03-22  
**Author**: Derek6_PO (Product Owner)  
**App**: EasyTMS — Truck Transportation Management System  
**Status**: In Progress

---

## Goal

> Every data entity is scoped to a Tenant. Users see only their business's data.

**Priority**: MUST HAVE — blocks all future features

**Strategy Decision**

| Decision              | Choice                                            | Reasoning                                                                                                             |
| --------------------- | ------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------- |
| Multi-tenant approach | **Shared Table, Private Row** (application-level) | Practical for household businesses. Single database, single schema, data separated by `TenantId` column.              |
| Enforcement mechanism | **EF Core Global Query Filters**                  | Simple, fits existing EF Core patterns, testable, non-over-engineered. SQL Server RLS can be layered later if needed. |
| Tenant entity naming  | **`Tenant`** (not Company)                        | Generic, future-proof. A tenant represents one household business.                                                    |
| Tenant resolution     | **JWT claim → scoped `TenantContext` service**    | Stateless, no extra DB lookup per request. `TenantId` embedded in token at login.                                     |

---

## Epic: Multi-Tenant Data Isolation

> As a business owner, I want my fleet data to be private to my business, so that other businesses cannot see, edit, or delete my trucks.

## User Stories

### S-01: Tenant Entity & Migration [M]

> As a system, I need a Tenant entity so that each business has a unique identity in the database.

- **Acceptance Criteria:**
  - [ ] `Tenant` model exists with: `TenantId` (Guid v7), `Name` (required), `OwnerId` (FK to AppUser.Id), `CreatedAt`
  - [ ] `AppUser` has a new `TenantId` (Guid, FK to Tenant) property
  - [ ] EF migration creates `Tenants` table and adds `TenantId` column to `AspNetUsers`
  - [ ] Seed data creates a default tenant and assigns existing seed users to it

### S-02: Add TenantId to Truck [M]

> As a system, I need every Truck record linked to a Tenant so queries can be filtered by ownership.

- **Acceptance Criteria:**
  - [ ] `Truck` model has `TenantId` (Guid, required, FK to Tenant)
  - [ ] EF migration adds `TenantId` column to `Trucks` table with index
  - [ ] Existing seed trucks are assigned to the default tenant
  - [ ] `TenantId` is NOT exposed in create/update DTOs (auto-stamped by system)

### S-03: ITenantEntity Interface [S]

> As a developer, I need a shared interface so all tenant-scoped entities follow the same pattern.

- **Acceptance Criteria:**
  - [ ] `ITenantEntity` interface exists with `Guid TenantId { get; set; }` property
  - [ ] `Truck` implements `ITenantEntity`
  - [ ] Future entities (Driver, Order, etc.) will implement `ITenantEntity`

### S-04: TenantContext Service [M]

> As the backend system, I need to know which tenant the current request belongs to, so I can scope data access.

- **Acceptance Criteria:**
  - [ ] `TenantContext` is a scoped service with `Guid TenantId` property
  - [ ] A middleware or filter reads `TenantId` from JWT claims and sets it on `TenantContext`
  - [ ] `TenantContext` is registered in DI
  - [ ] Unauthenticated requests have `TenantId = Guid.Empty`

### S-05: EF Core Global Query Filter [S]

> As a system, I need all queries to automatically filter by TenantId so data isolation is enforced without manual filtering.

- **Acceptance Criteria:**
  - [ ] `AppDbContext.OnModelCreating` applies `.HasQueryFilter()` to all `ITenantEntity` implementations
  - [ ] `TruckService.ListAsync()` returns only trucks belonging to the current tenant — no code changes needed in the service
  - [ ] Direct `DbSet<Truck>` queries are also filtered (global filter is on the entity, not the repo)

### S-06: Auto-Stamp TenantId on Create [S]

> As a developer, I want TenantId to be automatically set when creating new records so I never forget to assign it.

- **Acceptance Criteria:**
  - [ ] `AppDbContext.SaveChangesAsync()` is overridden
  - [ ] For any new entity implementing `ITenantEntity`, `TenantId` is set from `TenantContext.TenantId`
  - [ ] `TruckService.CreateAsync()` does NOT manually set `TenantId` — it's auto-stamped

### S-07: Update Auth Flow for Tenant [M]

> As a user, when I log in or register, my JWT should contain my TenantId so the system knows my business context.

- **Acceptance Criteria:**
  - [ ] **Register**: creates a new `Tenant` (name = user's email domain or "My Business"), assigns user to it, includes `TenantId` in JWT
  - [ ] **Login**: reads user's `TenantId`, includes it in JWT claims
  - [ ] **Refresh Token**: preserves `TenantId` claim in new token
  - [ ] **GetMe**: returns `TenantId` and `TenantName` in `UserProfile` DTO

### S-08: Frontend — Store Tenant Context [S]

> As the frontend, I need to know the current tenant so I can display the business name and pass context if needed.

- **Acceptance Criteria:**
  - [ ] `AuthService` stores `tenantId` and `tenantName` from login/register response or `GetMe`
  - [ ] Tenant name is accessible for display in sidebar or header
  - [ ] No tenant selector needed (one user = one tenant for now)

## Tasks

| #   | Task                                                              | Agent    | Story      | Depends On | Status | Completed  |
| --- | ----------------------------------------------------------------- | -------- | ---------- | ---------- | ------ | ---------- |
| 1   | [BE] Create `ITenantEntity` interface                             | Backend  | S-03       | —          | Done   | 2026-03-22 |
| 2   | [BE] Create `Tenant` model                                        | Backend  | S-01       | 1          | Done   | 2026-03-22 |
| 3   | [BE] Add `TenantId` to `AppUser`                                  | Backend  | S-01       | 2          | Done   | 2026-03-22 |
| 4   | [BE] Add `TenantId` to `Truck` (implement `ITenantEntity`)        | Backend  | S-02       | 1          | Done   | 2026-03-22 |
| 5   | [BE] Create EF migration for Tenant + FK columns                  | Backend  | S-01, S-02 | 2, 3, 4    | Done   | 2026-03-22 |
| 6   | [BE] Update seed data (default tenant, assign to users & trucks)  | Backend  | S-01, S-02 | 5          | Done   | 2026-03-22 |
| 7   | [BE] Create `TenantContext` scoped service                        | Backend  | S-04       | —          | Done   | 2026-03-24 |
| 8   | [BE] Create tenant resolution middleware (JWT → TenantContext)    | Backend  | S-04       | 7          | Done   | 2026-03-24 |
| 9   | [BE] Add Global Query Filter for `ITenantEntity`                  | Backend  | S-05       | 7, 4       | Done   | 2026-03-24 |
| 10  | [BE] Override `SaveChangesAsync` for auto-stamp                   | Backend  | S-06       | 7, 1       | Done   | 2026-03-24 |
| 11  | [BE] Update `TokenService` — add `TenantId` claim to JWT          | Backend  | S-07       | 3          | Done   | 2026-03-24 |
| 12  | [BE] Update `AccountService.Register` — create Tenant on register | Backend  | S-07       | 2, 11      | Done   | 2026-03-24 |
| 13  | [BE] Update `AccountService.Login` — include TenantId             | Backend  | S-07       | 11         | Done   | 2026-03-24 |
| 14  | [BE] Update `AccountService.GetMe` — return TenantId + TenantName | Backend  | S-07       | 2          | Done   | 2026-03-24 |
| 15  | [FE] Update `AuthService` + models — store tenant info            | Frontend | S-08       | 14         | Done   | 2026-03-24 |
| 16  | [FE] Display tenant name in sidebar header                        | Frontend | S-08       | 15         | Done   | 2026-03-24 |

---

## Notes

**Task 1-4: [BE] Create EF migration for Tenant + FK columns**  
**Status:** Done  
**Details:**

- Task 1, `ITenantEntity` interface created at `Models/ITenantEntity.cs` with single `Guid TenantId` property
- Task 2, `Tenant` model created at `Models/Tenant.cs` with properties: `TenantId` (Guid v7 PK), `Name` (string, required, max 200), `OwnerId` (string FK to AppUser.Id), `CreatedAt` (DateTimeOffset). `DbSet<Tenant>` registered in `AppDbContext` with entity config (Name required + max length, OwnerId required + indexed).
- Task 3, Added `Guid? TenantId` property to `AppUser` (nullable — safe for existing seed data). Configured `AppUser → Tenant` relationship in `AppDbContext` via Fluent API: `HasOne<Tenant>().WithMany()`, FK on `TenantId`, `OnDelete(SetNull)`, index on `TenantId`. No navigation properties (keep Identity clean).
- Task 4, `Truck` now implements `ITenantEntity` with `public Guid TenantId { get; set; }` (non-nullable — every truck must belong to a tenant). Configured `Truck → Tenant` FK in `AppDbContext` with `OnDelete(Restrict)` (protect business data), index on `TenantId`. No nav properties, no DTO changes.

**Task 5: [BE] Create EF migration for Tenant + FK columns**  
**Status:** Done  
**Details:**

- Migration files created:
  - `20260314143846_Truck.cs`: Created `Trucks` table with all required columns, including `TenantId` (non-nullable, FK to `Tenant`), and enforced multi-tenant structure.
  - `20260322173044_Tenants.cs`: Added `TenantId` columns to `Trucks` and `AspNetUsers`, created `Tenants` table, and set up FKs.
- FKs:
  - `Truck.TenantId` → `Tenant.TenantId` (Restrict on delete)
  - `AppUser.TenantId` → `Tenant.TenantId` (SetNull on delete)
- Indexes:
  - Index on `Truck.TenantId` for fast tenant filtering.
- All changes align with the multi-tenant data model.

**Task 6: [BE] Update seed data (default tenant, assign to users & trucks)**  
**Status:** Done  
**Details:**

- Seeders implemented in `TenantDataSeeder` and `TruckDataSeeder`.
- `TenantDataSeeder` creates 5 tenants with fixed GUIDs and owners.
- `TruckDataSeeder` generates 10 trucks, all assigned to the default tenant (`AlphaTenantId`).
- Seeding is invoked in `AppDbContext.OnModelCreating` via `TenantModelConfig` and `TruckModelConfig`.
- Ensures all initial data is tenant-scoped.

**Task 7: [BE] Create `TenantContext` scoped service**  
**Status:** Done  
**Details:**

- `TenantContext` service implemented (see `Business/Context/ITenantContext.cs` and related files).
- Service is registered as scoped and injected where needed.
- Holds the current tenant's ID, resolved from the authenticated user/JWT.
- Enables tenant-aware business logic throughout the backend.

**Task 8: [BE] Create tenant resolution middleware (JWT → TenantContext)**  
**Status:** Done  
**Details:**

- Middleware implemented to extract `TenantId` from JWT claims on each request.
- Sets the resolved `TenantId` into the `TenantContext` service.
- Ensures all backend operations are tenant-scoped for the current user.
- Fully integrated with the authentication pipeline.

**Task 9: [BE] Add Global Query Filter for `ITenantEntity`**  
**Status:** Done  
**Details:**

- Modified `Infrastructure/Database/AppDbContext.cs` — single file change.
- Injected `ITenantContext` as 3rd constructor parameter, stored as explicit private field `_tenantContext` (required for expression tree reference in query filters).
- Added dynamic global query filter loop at the end of `OnModelCreating`: iterates `builder.Model.GetEntityTypes()`, checks `ITenantEntity` assignability, builds expression tree `e.TenantId == _tenantContext.TenantId`, applies via `HasQueryFilter()`.
- EF Core evaluates `_tenantContext.TenantId` per-query (member access expression), so the filter is always current.
- `TruckService`, `TruckRepo` — zero changes needed. All `DbSet<Truck>` queries automatically scoped to current tenant.
- Future entities implementing `ITenantEntity` (Driver, Order, etc.) will automatically get the filter — no code changes needed.

**Task 10: [BE] Override `SaveChangesAsync` for auto-stamp**  
**Status:** Done  
**Details:**

- Modified `Infrastructure/Database/AppDbContext.cs` — same file as Task 9.
- Overrode `SaveChangesAsync(CancellationToken)` to iterate `ChangeTracker.Entries<ITenantEntity>()`.
- Only `EntityState.Added` entries are stamped — `TenantId` is set from `_tenantContext.TenantId`.
- `Modified`/`Deleted` entries are untouched — `TenantId` should never change after creation.
- `TruckService.CreateAsync()` does NOT manually set `TenantId` — it's auto-stamped via this override.
- `TruckRepo.SaveChangesAsync()` calls `dbContext.SaveChangesAsync()`, so the override is invoked automatically.
- Future entities implementing `ITenantEntity` will also be auto-stamped — no service code changes needed.

**Task 11: [BE] Update `TokenService` — add `TenantId` claim to JWT**  
**Status:** Done (already implemented)  
**Details:**

- Verified `TokenService.CreateToken` already adds `tenantId` claims to JWT at lines ~86-90.
- When `user.TenantId` is non-null and non-empty, two claims are added: custom `"tenantId"` claim and `ClaimTypes.GroupSid`.
- The `TenantResolutionMiddleware` reads these claims to populate `TenantContext` on each request.
- No code changes needed — this was implemented as part of tasks 7-8.

**Task 12: [BE] Update `AccountService.Register` — create Tenant on register**  
**Status:** Done  
**Details:**

- Modified `Business/AccountService.cs`.
- Injected `AppDbContext dbContext` into `AccountService` primary constructor (4th parameter).
- In `Register` method, after user creation and role assignment:
  1. Creates a new `Tenant` with auto-generated name from email (generic domains like gmail/outlook → `"{username}'s Business"`, custom domains → domain name).
  2. Sets `OwnerId` to the new user's ID.
  3. Saves tenant to DB via `dbContext.Tenants.Add` + `SaveChangesAsync`.
  4. Assigns `tenant.TenantId` to `user.TenantId` and updates user.
  5. Token generation happens AFTER tenant assignment, so JWT includes the `tenantId` claim.
- Added private static helper `GenerateTenantName(string email)` with generic domain list: gmail.com, outlook.com, yahoo.com, hotmail.com.
- `Tenant` does NOT implement `ITenantEntity`, so the `SaveChangesAsync` auto-stamp override doesn't interfere.

**Task 13: [BE] Update `AccountService.Login` — include TenantId**  
**Status:** Done (no changes needed)  
**Details:**

- Verified `userManager.FindByEmailAsync` returns `AppUser` with `TenantId` populated.
- `TenantId` is a direct column on `AspNetUsers` table (not a navigation property), so it's always loaded — no eager loading or `.Include()` needed.
- `CreateToken(user, roles)` already picks up `user.TenantId` and adds the claim.
- No code changes were necessary.

**Task 14: [BE] Update `AccountService.GetMe` — return TenantId + TenantName**  
**Status:** Done  
**Details:**

- Modified `Business/Types/AccountTypes.cs` — added `Guid? TenantId` and `string? TenantName` to `UserProfile` class.
- Modified `Business/AccountService.cs` — `GetProfile` method now:
  1. Checks if `user.TenantId` is a valid non-empty Guid.
  2. If yes, queries `dbContext.Tenants.FindAsync(tenantId)` to get the tenant name.
  3. Returns both `TenantId` and `TenantName` in the `UserProfile` response.
- `Tenants` table does NOT have a global query filter (not an `ITenantEntity`), so `FindAsync` works without `IgnoreQueryFilters`.

**Task 15: [FE] Update `AuthService` + models — store tenant info**  
**Status:** Done  
**Details:**

- Modified `platform/auth/auth.models.ts` — updated `UserProfile` interface to match backend `GetMe` response:
  - Removed stale fields: `id`, `fullName`, `role` (singular string) — these didn't exist in the backend DTO.
  - Added `userName: string` (replaces `fullName`).
  - Changed `role: string` → `roles: string[]` (backend returns a list).
  - Added `tenantId: string | null` and `tenantName: string | null`.
- **No changes needed to `AuthService`** — the service already calls `loadCurrentUser()` → `getProfile()` (GetMe) after login/register and stores the full `UserProfile` in `currentUserSubject`. With the updated interface, tenant data flows through automatically.
- **Design decision**: No JWT decoding for tenant info — `GetMe` is the single source of truth and includes `tenantName` which the JWT doesn't carry. Simpler, more maintainable.
- Verified no downstream breakages — no component was accessing the old `id`, `fullName`, or `role` fields from `UserProfile`.

**Task 16: [FE] Display tenant name in sidebar header**  
**Status:** Done  
**Details:**

- Modified `shell/navbar/navbar.ts`:
  - Injected `AuthService` via `inject()` (modern Angular pattern, consistent with component's use of `input()`/`output()`).
  - Converted `currentUser$` to a signal using `toSignal()` from `@angular/core/rxjs-interop`.
  - Added `tenantName` computed signal: reads `currentUser()?.tenantName` with `'My Business'` fallback for null/empty state.
- Modified `shell/navbar/navbar.html`:
  - Wrapped "EasyTMS" brand and tenant name in a shared container `div`.
  - Added `{{ tenantName() }}` below the brand with `text-sm text-gray-500 truncate` styling.
  - Both gated by `*ngIf="isOpen()"` — hidden when sidebar is collapsed (no overflow).
  - `truncate` class handles long tenant names gracefully within the 288px (w-72) sidebar width.
- Tenant name updates reactively when `currentUser$` emits (e.g., after login via `loadCurrentUser`).
- No errors in collapsed state — tenant text is fully hidden.
