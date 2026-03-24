# Sprint 1 Progress: Multi-Tenant Data Isolation (Phase 2A)

**Started**: 2026-03-22  
**Goal**: Every data entity is scoped to a Tenant. Users see only their business's data.

## Task Progress

| #   | Task                                                              | Agent    | Story      | Status | Completed  |
| --- | ----------------------------------------------------------------- | -------- | ---------- | ------ | ---------- |
| 1   | [BE] Create `ITenantEntity` interface                             | Backend  | S-03       | Done   | 2026-03-22 |
| 2   | [BE] Create `Tenant` model                                        | Backend  | S-01       | Done   | 2026-03-22 |
| 3   | [BE] Add `TenantId` to `AppUser`                                  | Backend  | S-01       | Done   | 2026-03-22 |
| 4   | [BE] Add `TenantId` to `Truck` (implement `ITenantEntity`)        | Backend  | S-02       | Done   | 2026-03-22 |
| 5   | [BE] Create EF migration for Tenant + FK columns                  | Backend  | S-01, S-02 | Done   | 2026-03-22 |
| 6   | [BE] Update seed data (default tenant, assign to users & trucks)  | Backend  | S-01, S-02 | Done   | 2026-03-22 |
| 7   | [BE] Create `TenantContext` scoped service                        | Backend  | S-04       | Done   | 2026-03-24 |
| 8   | [BE] Create tenant resolution middleware (JWT → TenantContext)    | Backend  | S-04       | Done   | 2026-03-24 |
| 9   | [BE] Add Global Query Filter for `ITenantEntity`                  | Backend  | S-05       | Done   | 2026-03-24 |
| 10  | [BE] Override `SaveChangesAsync` for auto-stamp                   | Backend  | S-06       | Done   | 2026-03-24 |
| 11  | [BE] Update `TokenService` — add `TenantId` claim to JWT          | Backend  | S-07       | Done   | 2026-03-24 |
| 12  | [BE] Update `AccountService.Register` — create Tenant on register | Backend  | S-07       | Done   | 2026-03-24 |
| 13  | [BE] Update `AccountService.Login` — include TenantId             | Backend  | S-07       | Done   | 2026-03-24 |
| 14  | [BE] Update `AccountService.GetMe` — return TenantId + TenantName | Backend  | S-07       | Done   | 2026-03-24 |
| 15  | [FE] Update `AuthService` + models — store tenant info            | Frontend | S-08       | Done   | 2026-03-24 |
| 16  | [FE] Display tenant name in sidebar header                        | Frontend | S-08       | Done   | 2026-03-24 |

## Notes

- Task 1 completed: `ITenantEntity` interface created at `Models/ITenantEntity.cs` with single `Guid TenantId` property
- Task 2 completed: `Tenant` model created at `Models/Tenant.cs` with properties: `TenantId` (Guid v7 PK), `Name` (string, required, max 200), `OwnerId` (string FK to AppUser.Id), `CreatedAt` (DateTimeOffset). `DbSet<Tenant>` registered in `AppDbContext` with entity config (Name required + max length, OwnerId required + indexed).
- Task 3 completed: Added `Guid? TenantId` property to `AppUser` (nullable — safe for existing seed data). Configured `AppUser → Tenant` relationship in `AppDbContext` via Fluent API: `HasOne<Tenant>().WithMany()`, FK on `TenantId`, `OnDelete(SetNull)`, index on `TenantId`. No navigation properties (keep Identity clean).
- Task 4 completed: `Truck` now implements `ITenantEntity` with `public Guid TenantId { get; set; }` (non-nullable — every truck must belong to a tenant). Configured `Truck → Tenant` FK in `AppDbContext` with `OnDelete(Restrict)` (protect business data), index on `TenantId`. No nav properties, no DTO changes.

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
