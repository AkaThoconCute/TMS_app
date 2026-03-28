# Sprint 3: User Management + Customer Management

**Date**: 2026-03-28  
**Author**: Derek6_PO (Product Owner)  
**App**: EasyTMS — Truck Transportation Management System  
**Status**: Planning

---

## Goal

> Complete user self-service features (profile, password management, role-based UI) and build the Customer module to prepare for Order & Trip management in the next phase.

**Priority**: HIGH  
**Depends On**: Sprint 0 (Foundation) - Done, Sprint 1 (Multi-Tenant) — Done

**Business Rationale**

- **User management**: Users currently cannot update their profile, change their password, or recover a forgotten password. These are table-stakes features for a production app.
- **Customer management**: In a TMS, a "customer" is the person or business that hires trucks to transport goods. Customers must exist before Orders can reference them . Building Customer now unblocks Order & Trip management next.

**Scope Decision**

| In Scope                                                | Out of Scope                                                   |
| ------------------------------------------------------- | -------------------------------------------------------------- |
| User profile page (view/edit username, change password) | Email sending (password reset token logged to console for now) |
| Forgot/Reset password backend flow                      | Truck maintenance records (deferred to future sprint)          |
| Reset password frontend page wired to backend           | Customer-Order linking                                         |
| Role-based UI visibility (admin vs user)                | Customer address autocomplete                                  |
| Customer entity with CRUD API                           | Customer order history                                         |
| Customer list page (paginated, searchable, filterable)  | Payment terms / credit limits                                  |
| Customer form dialog (create + edit)                    |                                                                |

---

## Epic A: User Management

> As a user, I want to manage my account — view and edit my profile, change my password, and recover my account if I forget my password.

### User Stories

#### S-15: User Profile API [M]

> As a user, I want backend endpoints to update my profile and change my password.

**Acceptance Criteria:**

- [ ] `PUT /api/Account/UpdateProfile` endpoint accepts `UpdateProfileDto` (`UserName` string required)
- [ ] Updates the authenticated user's `UserName` via `UserManager`
- [ ] Returns updated `UserProfile` DTO on success
- [ ] `POST /api/Account/ChangePassword` endpoint accepts `ChangePasswordDto` (`CurrentPassword`, `NewPassword`)
- [ ] Uses `UserManager.ChangePasswordAsync()` — validates current password before changing
- [ ] Returns success/failure with Identity error messages if password doesn't meet requirements
- [ ] Both endpoints `[Authorize]` — operate on authenticated user (no user ID in URL)

#### S-16: User Profile Page [M]

> As a user, I want a profile page to view my account info and change my password.

**Acceptance Criteria:**

- [ ] Page at `/profile` inside PrivateLayout
- [ ] Displays: Email (read-only), UserName (editable), Roles (read-only tags), TenantName (read-only)
- [ ] "Save Profile" button calls `UpdateProfile` endpoint, shows success toast
- [ ] "Change Password" section: Current Password, New Password, Confirm New Password fields
- [ ] "Change Password" button calls `ChangePassword` endpoint, shows success/error toast
- [ ] New Password and Confirm must match (frontend validation)
- [ ] Nav link to profile page (user icon / menu in header or sidebar)

#### S-17: Reset Password Flow [M]

> As a user, I want to reset my password if I forget it, so I can regain access to my account.

**Acceptance Criteria:**

- [ ] `POST /api/Account/ForgotPassword` endpoint accepts `ForgotPasswordDto` (`Email`)
- [ ] Generates a password reset token via `UserManager.GeneratePasswordResetTokenAsync()`
- [ ] Logs the reset token to console/logger (email integration deferred)
- [ ] Always returns success (no user enumeration — don't reveal if email exists)
- [ ] `POST /api/Account/ResetPassword` endpoint accepts `ResetPasswordDto` (`Email`, `Token`, `NewPassword`)
- [ ] Resets password via `UserManager.ResetPasswordAsync()`
- [ ] Returns success/failure with Identity error messages
- [ ] Frontend reset-password page at `/reset-password` (already exists) wired to both endpoints
- [ ] Flow: enter email → "token sent" message → enter token + new password → success → redirect to login

#### S-18: Role-Based UI Visibility [S]

> As an admin, I want to see admin-specific options that regular users don't see.

**Acceptance Criteria:**

- [ ] Frontend reads `roles` from `UserProfile` (already available via `GetMe`)
- [ ] Create a `hasRole()` utility or signal-based check (e.g., `authService.hasRole('Admin')`)
- [ ] Demo: at least one menu item or UI element that is visible only to Admin role
- [ ] Non-admin users do not see admin-only UI elements (structural hiding, not just CSS)

---

## Epic B: Customer Management

> As a fleet manager, I want to manage my customers — the people and businesses that hire my trucks — so I can track who I transport goods for and prepare for order management.

### Domain Analysis

In a household truck transportation business, a **Customer** is:

- The person or company that **hires trucks** to move goods
- Can be an **individual** (one-time or recurring) or a **business** (contract-based)
- Has contact info for coordination (phone, email, address)
- May have a **tax code** (needed later for invoicing in Phase 5)
- Has a **status** (active customers vs inactive/past customers)

**Why now?** Orders require `CustomerId` as a foreign key. Building the Customer module now means Order management can reference real customer data immediately.

### User Stories

#### S-19: Customer Model & Migration [M]

> As a system, I need a Customer entity to store customer information scoped to a tenant.

**Acceptance Criteria:**

- [ ] `Customer` model exists with:
  - `CustomerId` (Guid v7, PK)
  - `TenantId` (Guid, FK → Tenant)
  - `Name` (string, required, max 200) — company or individual name
  - `ContactPerson` (string?, max 100) — primary contact person
  - `PhoneNumber` (string, required, max 20) — primary phone
  - `Email` (string?, max 200)
  - `Address` (string?, max 500) — primary/billing address
  - `TaxCode` (string?, max 50) — tax identification number
  - `CustomerType` (int, default 1) — 1=Individual, 2=Business
  - `Status` (int, default 1) — 1=Active, 2=Inactive
  - `Notes` (string?)
  - `CreatedAt` (DateTimeOffset)
  - `UpdatedAt` (DateTimeOffset?)
- [ ] Implements `ITenantEntity`
- [ ] `DbSet<Customer>` registered in `AppDbContext`
- [ ] EF config: Tenant FK (Restrict delete), TenantId index, Name required + max length
- [ ] EF migration creates `Customers` table
- [ ] Seed data generates 8 sample customers for default tenant (mix of Individual + Business)

#### S-20: Customer CRUD API [L]

> As a fleet manager, I want API endpoints to create, read, update, delete, and list customers.

**Acceptance Criteria:**

- [ ] `CustomerController` with: `Create`, `GetById`, `List` (paginated + search + status filter + type filter), `Update`, `Delete`
- [ ] `CustomerService` with business logic and validation:
  - Required field validation (Name, PhoneNumber)
  - Unique phone number per tenant
  - Valid CustomerType (1 or 2) and Status (1 or 2)
- [ ] DTOs: `CreateCustomerDto`, `UpdateCustomerDto`, `CustomerDto`
- [ ] AutoMapper profile for Customer ↔ DTOs
- [ ] All endpoints `[Authorize]`, data scoped by tenant (via global filter)
- [ ] Follows same patterns as `DriverController` / `DriverService`

#### S-21: Customer List Page [L]

> As a fleet manager, I want to see all my customers in a searchable, filterable table.

**Acceptance Criteria:**

- [ ] PrimeNG DataTable at `/customers/list` with lazy loading (server-side pagination)
- [ ] Columns: Name, ContactPerson, PhoneNumber, Email, CustomerType, Status
- [ ] CustomerType tags: `Individual` → blue, `Business` → purple
- [ ] Status tags: `Active` → green, `Inactive` → gray
- [ ] Search by name, contact person, or phone number (single search input)
- [ ] Filter by status dropdown
- [ ] Filter by customer type dropdown
- [ ] Edit/Delete per row with confirm dialog + toast
- [ ] Follows same patterns as Driver list page

#### S-22: Customer Form Dialog [M]

> As a fleet manager, I want to create and edit customer details through a form.

**Acceptance Criteria:**

- [ ] PrimeNG Dialog with form fields:
  - Name (text, required)
  - ContactPerson (text)
  - PhoneNumber (text, required)
  - Email (text, email validation)
  - Address (textarea)
  - TaxCode (text)
  - CustomerType (dropdown: Individual, Business — default Individual)
  - Status (dropdown: Active, Inactive — default Active)
  - Notes (textarea)
- [ ] Reusable for both Create and Edit modes
- [ ] Validation: required fields, email format, phone format
- [ ] On save: calls API, refreshes table, shows success toast
- [ ] Follows same patterns as Driver form dialog

---

## Tasks

| #   | Task                                                  | Agent    | Story | Depends On | Status      |
| --- | ----------------------------------------------------- | -------- | ----- | ---------- | ----------- |
| 1   | [BE] Add `UpdateProfile` + `ChangePassword` endpoints | Backend  | S-15  | —          | Done        |
| 2   | [FE] Build profile page with edit + change password   | Frontend | S-16  | 1          | Done        |
| 3   | [BE] Add `ForgotPassword` + `ResetPassword` endpoints | Backend  | S-17  | —          | Not Started |
| 4   | [FE] Wire reset password page to backend endpoints    | Frontend | S-17  | 3          | Not Started |
| 5   | [FE] Add role-based visibility logic + demo           | Frontend | S-18  | —          | Not Started |
| 6   | [BE] Create `Customer` model + EF config + migration  | Backend  | S-19  | —          | Not Started |
| 7   | [BE] Create seed data for Customers                   | Backend  | S-19  | 6          | Not Started |
| 8   | [BE] Create `CustomerRepo`                            | Backend  | S-20  | 6          | Not Started |
| 9   | [BE] Create DTOs + AutoMapper profile for Customer    | Backend  | S-20  | 6          | Not Started |
| 10  | [BE] Create `CustomerService` with CRUD + validation  | Backend  | S-20  | 8, 9       | Not Started |
| 11  | [BE] Create `CustomerController`                      | Backend  | S-20  | 10         | Not Started |
| 12  | [FE] Create customer models/DTOs                      | Frontend | S-21  | 9          | Not Started |
| 13  | [FE] Create `CustomerService` (HTTP client)           | Frontend | S-21  | 11         | Not Started |
| 14  | [FE] Create Customer list page with PrimeNG DataTable | Frontend | S-21  | 12, 13     | Not Started |
| 15  | [FE] Create Customer form dialog                      | Frontend | S-22  | 12, 13     | Not Started |
| 16  | [FE] Add customer routes + update navbar              | Frontend | S-21  | 14         | Not Started |

**Parallelism Notes:**

- Tasks 1, 3, 5, 6 can all start in parallel (no dependencies)
- Backend User tasks (1, 3) and Backend Customer tasks (6–11) are independent tracks
- Frontend User tasks (2, 4) depend on their respective backend tasks
- Frontend Customer tasks (12–16) depend on backend Customer API being ready

---

## Dependencies

```
Epic A (User):                Epic B (Customer):
  T1 (BE Profile) ──→ T2      T6 (BE Model) ──→ T7 (Seed)
  T3 (BE Reset) ──→ T4                     ├──→ T8 (Repo)
  T5 (FE Roles) — standalone               ├──→ T9 (DTOs)
                                T8+T9 ──→ T10 (Service) ──→ T11 (Controller)
                                T9 ──→ T12 (FE Models)
                                T11 ──→ T13 (FE Service)
                                T12+T13 ──→ T14 (FE List) ──→ T16 (Routes+Nav)
                                T12+T13 ──→ T15 (FE Dialog)
```

---

## Notes

_Notes will be added as tasks are completed._
