# Sprint 4: Order & Trip Management

**Date**: 2026-03-29  
**Author**: Derek6_PO (Product Owner)  
**App**: EasyTMS — Truck Transportation Management System  
**Status**: Planning

---

## Goal

> Core TMS value — manage the full shipment lifecycle from customer request through truck assignment, transit, delivery, and order closure. This is the module that turns EasyTMS from a fleet database into a real Transportation Management System.

**Priority**: CRITICAL  
**Depends On**: Sprint 3 (Customer module) — Done

**Business Rationale**

- **Orders** are the revenue-generating transactions of the business. Without them, there's nothing to manage.
- **Trips** are the physical execution — assigning trucks and drivers to move goods. They connect the fleet (Trucks, Drivers) to the business (Customers, Orders).
- The Customer entity (Sprint 3) is required since every Order references a Customer.
- Building Order + Trip now enables Invoicing & Billing in Sprint 5 (invoices are generated from completed orders).

**Scope Decision**

| In Scope                                                        | Out of Scope (Future)                                |
| --------------------------------------------------------------- | ---------------------------------------------------- |
| Order entity with CRUD + lifecycle management                   | Invoicing / billing (Sprint 5)                       |
| Trip entity with CRUD + lifecycle management                    | GPS / real-time tracking                             |
| Auto-generated OrderNumber / TripNumber (sequential per tenant) | Multi-stop routes / waypoints                        |
| Order status cascade driven by Trip status changes              | Automated email notifications on status change       |
| Trip cost recording (fuel, tolls, other) at completion          | Advanced cost analytics / cost-per-km calculations   |
| Order status pipeline summary (counts per status)               | Kanban/board view for orders (DataTable is Sprint 4) |
| Expandable order rows showing trip details                      | Separate Trip list page (trips are order-contextual) |
| Truck/Driver availability validation on trip creation           | Auto-scheduling / optimization                       |
| Order form dialog (create + edit)                               | Customer address autocomplete / maps integration     |
| Trip form dialog with truck/driver dropdowns                    | Bulk order creation / CSV import                     |

---

## Domain Analysis

### What is an Order?

In a household truck transportation business, an **Order** represents a customer's request to transport goods:

- **Who** hired us → Customer (individual or business)
- **What** are we moving → Cargo description + estimated weight
- **Where** → Pickup address → Delivery address
- **When** → Requested pickup/delivery dates
- **How much** → Quoted price (agreed with customer)

An Order is a **business record** — it exists from the moment a customer requests transport until the job is finalized or cancelled. Orders are never hard-deleted; they form the historical record needed for invoicing and reporting.

### What is a Trip?

A **Trip** is the physical execution of an Order — the act of assigning a specific Truck and Driver to move the goods:

- **Which truck** → Selected from available fleet
- **Which driver** → Selected from active drivers
- **When (planned)** → Planned pickup/delivery dates
- **When (actual)** → Actual pickup/delivery timestamps (recorded as the trip progresses)
- **How much did it cost** → Fuel, tolls, other expenses (recorded at completion)

A Trip bridges the gap between the business request (Order) and the fleet resources (Truck + Driver).

### Relationship: Order → Trip

```
┌──────────┐  1    0..N  ┌──────────┐
│  Order   │ ──────────> │   Trip   │
└──────────┘             └──────────┘
     │                        │ │ │
     │ N..1                   │ │ │
┌──────────┐           N..1  │ │ │ N..1
│ Customer │     ┌───────────┘ │ └───────────┐
└──────────┘     │             │             │
           ┌──────────┐  ┌──────────┐  ┌──────────┐
           │  Truck   │  │  Driver  │  │  Order   │
           └──────────┘  └──────────┘  └──────────┘
```

- **One** Order can have **many** Trips (if a trip is cancelled, a replacement trip can be created)
- **MVP constraint**: Only **one active trip** (Planned or InTransit) per Order at a time
- Each Trip belongs to exactly **one** Order, **one** Truck, and **one** Driver
- Trip status changes **cascade** to Order status automatically

### Order Status Model

```
                                ┌─────────────┐
                                │   Created   │ (1)
                                │  [initial]  │
                                └──────┬──────┘
                                       │ trip created
                                       ▼
                                ┌─────────────┐
                     ┌──────────│  Assigned   │ (2)
                     │          └──────┬──────┘
                     │                 │ trip started
                     │                 ▼
                     │          ┌─────────────┐
                     │          │  InTransit  │ (3)
                     │          └──────┬──────┘
                     │                 │ trip completed
                     │                 ▼
                     │          ┌─────────────┐
                     │          │  Delivered  │ (4)
                     │          └──────┬──────┘
                     │                 │ manager finalizes
                     │                 ▼
                     │          ┌─────────────┐
                     │          │  Completed  │ (5) [terminal]
                     │          └─────────────┘
                     │
                     │ cancel (from Created, Assigned, or InTransit)
                     ▼
              ┌─────────────┐
              │  Cancelled  │ (6) [terminal]
              └─────────────┘
```

| Value | Label     | Meaning                                          | Terminal? |
| ----- | --------- | ------------------------------------------------ | --------- |
| 1     | Created   | Order entered, no truck/driver assigned yet      | No        |
| 2     | Assigned  | A trip exists — truck + driver assigned          | No        |
| 3     | InTransit | Truck is on the road                             | No        |
| 4     | Delivered | Goods delivered to destination                   | No        |
| 5     | Completed | Order finalized by manager (ready for invoicing) | Yes       |
| 6     | Cancelled | Order cancelled, all trips cancelled             | Yes       |

### Trip Status Model

```
              ┌─────────────┐
              │   Planned   │ (1) [initial]
              └──────┬──────┘
                     │ driver starts
                     ▼
              ┌─────────────┐
              │  InTransit  │ (2)
              └──────┬──────┘
                     │ delivery confirmed
                     ▼
              ┌─────────────┐
              │  Completed  │ (3) [terminal]
              └─────────────┘

     cancel (from Planned or InTransit)
              ┌─────────────┐
              │  Cancelled  │ (4) [terminal]
              └─────────────┘
```

| Value | Label     | Meaning                                               | Terminal? |
| ----- | --------- | ----------------------------------------------------- | --------- |
| 1     | Planned   | Trip created, truck + driver assigned, not yet moving | No        |
| 2     | InTransit | Driver has picked up cargo and is on the road         | No        |
| 3     | Completed | Delivery confirmed, costs recorded                    | Yes       |
| 4     | Cancelled | Trip cancelled, truck + driver freed                  | Yes       |

### Status Cascading Rules (Trip → Order)

| Trip Event           | Order Status Change                                                |
| -------------------- | ------------------------------------------------------------------ |
| Trip Created         | Order: Created → **Assigned**                                      |
| Trip Started         | Order: Assigned → **InTransit**                                    |
| Trip Completed       | If no other active trips → Order: InTransit → **Delivered**        |
| Trip Cancelled       | If no other active trips → Order: Assigned/InTransit → **Created** |
| Order Cancel Request | All active trips (Planned/InTransit) → **auto-cancelled** first    |

**Edge cases:**

- If a cancelled trip's order has no other active trips, the order reverts to **Created** (so a new trip can be assigned, or the order itself can be cancelled).
- An order can only reach **Completed** via manual action by the fleet manager (after Delivered status).
- An order can only reach **Completed** if ALL its trips are in terminal states (Completed or Cancelled).

---

## Operation Analysis

### 1. Create Order

| Aspect            | Detail                                               |
| ----------------- | ---------------------------------------------------- |
| **Who**           | Fleet Manager (any authenticated user within tenant) |
| **Preconditions** | Customer must exist and be Active (status=1)         |
| **HTTP**          | `POST /api/Order/Create`                             |

**Payload:**

| Field                 | Type      | Required | Validation                                            |
| --------------------- | --------- | -------- | ----------------------------------------------------- |
| CustomerId            | Guid      | Yes      | Must reference an existing, Active customer in tenant |
| PickupAddress         | string    | Yes      | Max 500 chars                                         |
| DeliveryAddress       | string    | Yes      | Max 500 chars                                         |
| CargoDescription      | string    | Yes      | Max 500 chars                                         |
| CargoWeightKg         | decimal?  | No       | Must be > 0 if provided                               |
| RequestedPickupDate   | DateTime? | No       | Must not be in the past (if provided)                 |
| RequestedDeliveryDate | DateTime? | No       | Must be ≥ RequestedPickupDate (if both provided)      |
| QuotedPrice           | decimal?  | No       | Must be ≥ 0 if provided                               |
| Notes                 | string?   | No       | Free text                                             |

**Effects:**

- Order created with Status = **Created (1)**
- OrderNumber auto-generated: `ORD-{sequential:D6}` (unique per tenant)
- CreatedAt = now

**Invalid scenarios → 400 Bad Request:**

- Missing required fields (CustomerId, PickupAddress, DeliveryAddress, CargoDescription)
- CustomerId references a non-existent or Inactive customer
- RequestedDeliveryDate < RequestedPickupDate
- CargoWeightKg ≤ 0 or QuotedPrice < 0

---

### 2. Create Trip

| Aspect            | Detail                                                                       |
| ----------------- | ---------------------------------------------------------------------------- |
| **Who**           | Fleet Manager (any authenticated user within tenant)                         |
| **Preconditions** | Order must exist, not in terminal status. Truck available. Driver available. |
| **HTTP**          | `POST /api/Trip/Create`                                                      |

**Payload:**

| Field               | Type      | Required | Validation                                     |
| ------------------- | --------- | -------- | ---------------------------------------------- |
| OrderId             | Guid      | Yes      | Must reference an existing order in tenant     |
| TruckId             | Guid      | Yes      | Must reference an existing truck in tenant     |
| DriverId            | Guid      | Yes      | Must reference an existing driver in tenant    |
| PlannedPickupDate   | DateTime? | No       | If provided, should be a reasonable date       |
| PlannedDeliveryDate | DateTime? | No       | Must be ≥ PlannedPickupDate (if both provided) |
| Notes               | string?   | No       | Free text                                      |

**Validations:**

- Order.Status must NOT be Completed (5) or Cancelled (6)
- Order must NOT already have an active trip (Planned or InTransit) — MVP: one active trip at a time
- Truck must not be assigned to another active trip (Planned/InTransit) across all orders in tenant
- Driver must not be assigned to another active trip (Planned/InTransit) across all orders in tenant
- Driver.Status must be Active (1) — no OnLeave or Terminated drivers
- Driver license must not be expired (`LicenseExpiry >= today`, if set)

**Effects:**

- Trip created with Status = **Planned (1)**
- TripNumber auto-generated: `TRP-{sequential:D6}` (unique per tenant)
- **Cascade**: Order.Status → **Assigned (2)** (if was Created)
- CreatedAt = now

**Invalid scenarios → 400 Bad Request:**

- Missing required fields
- Order in terminal status (Completed/Cancelled)
- Order already has an active trip → `"This order already has an active trip. Cancel the existing trip first."`
- Truck already on active trip → `"This truck is already assigned to active trip {TripNumber}."`
- Driver already on active trip → `"This driver is already assigned to active trip {TripNumber}."`
- Driver not Active or license expired → `"Driver is not available (status: OnLeave)" / "Driver's license expired on {date}."`

---

### 3. Update Order

| Aspect            | Detail                                                               |
| ----------------- | -------------------------------------------------------------------- |
| **Who**           | Fleet Manager (any authenticated user within tenant)                 |
| **Preconditions** | Order must exist and not be in terminal status (Completed/Cancelled) |
| **HTTP**          | `PUT /api/Order/Update?orderId={id}`                                 |

**Payload (partial — all nullable):**

| Field                 | Type      | Updatable When          | Validation                     |
| --------------------- | --------- | ----------------------- | ------------------------------ |
| PickupAddress         | string?   | Any non-terminal status | Max 500 chars                  |
| DeliveryAddress       | string?   | Any non-terminal status | Max 500 chars                  |
| CargoDescription      | string?   | Any non-terminal status | Max 500 chars                  |
| CargoWeightKg         | decimal?  | Any non-terminal status | Must be > 0 if provided        |
| RequestedPickupDate   | DateTime? | Any non-terminal status | Cross-check with delivery date |
| RequestedDeliveryDate | DateTime? | Any non-terminal status | Must be ≥ pickup date          |
| QuotedPrice           | decimal?  | Any non-terminal status | Must be ≥ 0 if provided        |
| Notes                 | string?   | Any non-terminal status | Free text                      |

**Fields that CANNOT be updated via this endpoint:**

- `CustomerId` — changing the customer means a different business transaction → create a new order
- `Status` — managed exclusively through lifecycle endpoints (Complete, Cancel)
- `OrderNumber` — immutable auto-generated identifier

**Effects:**

- Only provided (non-null) fields are updated (same partial-update pattern as existing entities)
- UpdatedAt = now

**Invalid scenarios → 400 Bad Request:**

- Order not found → 404
- Order is Completed or Cancelled → `"Cannot update a closed order."`
- RequestedDeliveryDate < RequestedPickupDate → `"Delivery date must be on or after pickup date."`

---

### 4. Update Trip

| Aspect            | Detail                                                              |
| ----------------- | ------------------------------------------------------------------- |
| **Who**           | Fleet Manager (any authenticated user within tenant)                |
| **Preconditions** | Trip must exist and not be in terminal status (Completed/Cancelled) |
| **HTTP**          | `PUT /api/Trip/Update?tripId={id}`                                  |

**Payload (partial — all nullable):**

| Field               | Type      | Updatable When      | Validation                                                |
| ------------------- | --------- | ------------------- | --------------------------------------------------------- |
| TruckId             | Guid?     | Planned only        | Must reference available truck not on another active trip |
| DriverId            | Guid?     | Planned only        | Must reference active driver not on another active trip   |
| PlannedPickupDate   | DateTime? | Planned only        | Reasonable date                                           |
| PlannedDeliveryDate | DateTime? | Planned only        | Must be ≥ planned pickup date                             |
| Notes               | string?   | Planned / InTransit | Free text                                                 |

**Fields that CANNOT be updated via this endpoint:**

- `OrderId` — a trip belongs to its order permanently
- `Status` — managed exclusively through lifecycle endpoints (Start, Complete, Cancel)
- `TripNumber` — immutable auto-generated identifier
- `TruckId / DriverId` — cannot change once trip is InTransit (truck is on the road)
- `ActualPickupDate / ActualDeliveryDate` — set via lifecycle endpoints (Start, Complete)
- `Costs` — recorded via Complete endpoint

**Effects:**

- Only provided (non-null) fields are updated
- If TruckId/DriverId changed: same availability validations as Create
- UpdatedAt = now

**Invalid scenarios → 400 Bad Request:**

- Trip not found → 404
- Trip is Completed or Cancelled → `"Cannot update a closed trip."`
- Changing Truck/Driver while InTransit → `"Cannot change truck or driver while trip is in transit."`
- New Truck/Driver already on another active trip → same as Create validation

---

### 5. Start Trip

| Aspect            | Detail                                              |
| ----------------- | --------------------------------------------------- |
| **Who**           | Fleet Manager (records that the driver has started) |
| **Preconditions** | Trip must be in Planned status                      |
| **HTTP**          | `PATCH /api/Trip/Start?tripId={id}`                 |

**Payload:**

| Field            | Type     | Required | Validation                                 |
| ---------------- | -------- | -------- | ------------------------------------------ |
| ActualPickupDate | DateTime | No       | Defaults to `DateTime.Now` if not provided |

**Effects:**

- Trip.Status → **InTransit (2)**
- Trip.ActualPickupDate = provided date or now
- Trip.UpdatedAt = now
- **Cascade**: Order.Status → **InTransit (3)** (if was Assigned)

**Invalid scenarios → 400 Bad Request:**

- Trip not found → 404
- Trip not in Planned status → `"Trip can only be started from Planned status. Current status: {label}."`

---

### 6. Complete Order (Close — Success)

| Aspect            | Detail                                                                  |
| ----------------- | ----------------------------------------------------------------------- |
| **Who**           | Fleet Manager — manual action to finalize the order after delivery      |
| **Preconditions** | Order must be in Delivered status. All trips must be in terminal state. |
| **HTTP**          | `PATCH /api/Order/Complete?orderId={id}`                                |

**Payload:**

| Field | Type    | Required | Validation                                           |
| ----- | ------- | -------- | ---------------------------------------------------- |
| Notes | string? | No       | Optional final notes (e.g., "Customer paid in full") |

**Validations:**

- Order.Status must be Delivered (4)
- All trips for this order must be Completed (3) or Cancelled (4) — no active trips allowed

**Effects:**

- Order.Status → **Completed (5)**
- Order.CompletedAt = now
- Order.UpdatedAt = now

**Invalid scenarios → 400 Bad Request:**

- Order not found → 404
- Order not in Delivered status → `"Order can only be completed from Delivered status. Current: {label}."`
- Order has active trips → `"Cannot complete order while trips are still active."`

---

### 7. Cancel Order (Close — Abort)

| Aspect            | Detail                                                             |
| ----------------- | ------------------------------------------------------------------ |
| **Who**           | Fleet Manager — cancels the entire order                           |
| **Preconditions** | Order must NOT already be in terminal status (Completed/Cancelled) |
| **HTTP**          | `PATCH /api/Order/Cancel`                                          |

**Payload:**

| Field              | Type   | Required | Validation                                 |
| ------------------ | ------ | -------- | ------------------------------------------ |
| OrderId            | Guid   | Yes      | Must reference existing order in tenant    |
| CancellationReason | string | Yes      | Max 500 chars, required — must explain why |

**Effects:**

- **First**: All active trips (Planned/InTransit) for this order are **auto-cancelled** with reason `"Parent order cancelled"`
- **Then**: Order.Status → **Cancelled (6)**
- Order.CancellationReason = provided reason
- Order.CancelledAt = now
- Order.UpdatedAt = now

**Invalid scenarios → 400 Bad Request:**

- Order not found → 404
- Order already Completed → `"Cannot cancel a completed order."`
- Order already Cancelled → `"Order is already cancelled."`
- Missing CancellationReason → `"Cancellation reason is required."`

---

### 8. Complete Trip (Close — Success)

| Aspect            | Detail                                                                    |
| ----------------- | ------------------------------------------------------------------------- |
| **Who**           | Fleet Manager — records that delivery is confirmed and costs are recorded |
| **Preconditions** | Trip must be in InTransit status                                          |
| **HTTP**          | `PATCH /api/Trip/Complete`                                                |

**Payload:**

| Field              | Type     | Required | Validation                             |
| ------------------ | -------- | -------- | -------------------------------------- |
| TripId             | Guid     | Yes      | Must reference existing trip in tenant |
| ActualDeliveryDate | DateTime | Yes      | Must be ≥ ActualPickupDate             |
| FuelCost           | decimal? | No       | Must be ≥ 0 if provided                |
| TollCost           | decimal? | No       | Must be ≥ 0 if provided                |
| OtherCost          | decimal? | No       | Must be ≥ 0 if provided                |
| CostNotes          | string?  | No       | Max 500 chars, e.g. "Parking fee"      |
| Notes              | string?  | No       | Optional completion notes              |

**Validations:**

- Trip.Status must be InTransit (2)
- Trip.ActualPickupDate must be set (trip must have been started)
- ActualDeliveryDate must be ≥ ActualPickupDate

**Effects:**

- Trip.Status → **Completed (3)**
- Trip.ActualDeliveryDate = provided date
- Trip.FuelCost, TollCost, OtherCost = provided values
- Trip.CostNotes = provided notes
- Trip.CompletedAt = now
- Trip.UpdatedAt = now
- **Cascade**: If no other active trips for parent order → Order.Status → **Delivered (4)**

**Invalid scenarios → 400 Bad Request:**

- Trip not found → 404
- Trip not InTransit → `"Trip can only be completed from InTransit status. Current: {label}."`
- ActualDeliveryDate < ActualPickupDate → `"Delivery date cannot be before pickup date."`
- Negative cost values → `"Cost values must be non-negative."`

---

### 9. Cancel Trip (Close — Abort)

| Aspect            | Detail                                                           |
| ----------------- | ---------------------------------------------------------------- |
| **Who**           | Fleet Manager — cancels a specific trip (e.g., truck broke down) |
| **Preconditions** | Trip must NOT be in terminal status (Completed/Cancelled)        |
| **HTTP**          | `PATCH /api/Trip/Cancel`                                         |

**Payload:**

| Field              | Type   | Required | Validation                                 |
| ------------------ | ------ | -------- | ------------------------------------------ |
| TripId             | Guid   | Yes      | Must reference existing trip in tenant     |
| CancellationReason | string | Yes      | Max 500 chars, required — must explain why |

**Effects:**

- Trip.Status → **Cancelled (4)**
- Trip.CancellationReason = provided reason
- Trip.CancelledAt = now
- Trip.UpdatedAt = now
- **Cascade**: If no other active trips remain for parent order:
  - Order.Status → **Created (1)** (reverted — so a new trip can be assigned)

**Invalid scenarios → 400 Bad Request:**

- Trip not found → 404
- Trip already Completed → `"Cannot cancel a completed trip."`
- Trip already Cancelled → `"Trip is already cancelled."`
- Missing CancellationReason → `"Cancellation reason is required."`

---

## Epic A: Order Management

> As a fleet manager, I want to create and manage transportation orders from my customers, so I can track what needs to be moved, where, and when — and close orders when the job is done.

### User Stories

#### S-23: Order Model & Migration [M]

> As a system, I need an Order entity to store transportation order data scoped to a tenant.

**Acceptance Criteria:**

- [ ] `Order` model exists with:
  - `OrderId` (Guid v7, PK)
  - `TenantId` (Guid, FK → Tenant)
  - `OrderNumber` (string, max 20, auto-generated, unique per tenant)
  - `CustomerId` (Guid, FK → Customer)
  - `PickupAddress` (string, required, max 500)
  - `DeliveryAddress` (string, required, max 500)
  - `CargoDescription` (string, required, max 500)
  - `CargoWeightKg` (decimal?)
  - `RequestedPickupDate` (DateTime?)
  - `RequestedDeliveryDate` (DateTime?)
  - `QuotedPrice` (decimal?)
  - `Status` (int, default 1) — 1=Created, 2=Assigned, 3=InTransit, 4=Delivered, 5=Completed, 6=Cancelled
  - `CancellationReason` (string?, max 500)
  - `Notes` (string?)
  - `CompletedAt` (DateTimeOffset?)
  - `CancelledAt` (DateTimeOffset?)
  - `CreatedAt` (DateTimeOffset)
  - `UpdatedAt` (DateTimeOffset?)
  - Navigation: `Customer` (nav prop), `ICollection<Trip> Trips` (collection)
- [ ] Implements `ITenantEntity`
- [ ] `DbSet<Order>` registered in `AppDbContext`
- [ ] EF config: Tenant FK (Restrict delete), Customer FK (Restrict delete), TenantId index, (TenantId + OrderNumber) unique index, required fields + max lengths
- [ ] Seed data: 6 sample orders for default tenant (mix of statuses: 2× Created, 2× Assigned, 1× Delivered, 1× Completed)

#### S-24: Order CRUD + Lifecycle API [XL]

> As a fleet manager, I want API endpoints to create, view, list, update, complete, and cancel orders.

**Acceptance Criteria:**

- [ ] `OrderController` with endpoints:
  - `POST Create` — create new order
  - `GET GetById` — get single order by ID
  - `GET List` — paginated + status filter + customer filter + search term (searches OrderNumber, PickupAddress, DeliveryAddress, CargoDescription)
  - `GET StatusSummary` — returns count per status for pipeline summary display
  - `PUT Update` — partial update of order details
  - `PATCH Complete` — transition Delivered → Completed
  - `PATCH Cancel` — transition to Cancelled (with reason, auto-cancels active trips)
- [ ] `OrderService` with business logic:
  - Auto-generate OrderNumber on create (`ORD-{sequential:D6}`)
  - Validate CustomerId references Active customer
  - Validate date ordering (delivery ≥ pickup)
  - Prevent updates on terminal-status orders
  - Complete validation: must be Delivered, no active trips
  - Cancel: auto-cancel active trips, require reason
  - Cross-repo access: `CustomerRepo` (validate customer), `TripRepo` (check active trips, auto-cancel)
- [ ] DTOs: `CreateOrderDto`, `UpdateOrderDto`, `OrderDto` (response with computed: `StatusLabel`, `CustomerName`, `TripCount`)
- [ ] `CompleteOrderDto` (optional Notes), `CancelOrderDto` (OrderId + CancellationReason)
- [ ] AutoMapper profile for Order ↔ DTOs
- [ ] All endpoints `[Authorize]`, data scoped by tenant (via global filter)
- [ ] Follows same patterns as `CustomerController` / `CustomerService`

#### S-25: Order List Page [L]

> As a fleet manager, I want to see all my orders in a searchable, filterable table with status pipeline summary.

**Acceptance Criteria:**

- [ ] **Status pipeline summary cards** at top of page — one card per status showing count (e.g., "Created: 3", "InTransit: 2"). Color-coded to match status tags.
- [ ] PrimeNG DataTable at `/orders/list` with lazy loading (server-side pagination)
- [ ] Columns: OrderNumber, CustomerName, PickupAddress, DeliveryAddress, CargoDescription, QuotedPrice, Status, CreatedAt
- [ ] Status tags: Created → blue, Assigned → orange, InTransit → cyan, Delivered → teal, Completed → green, Cancelled → red
- [ ] Search by order number, addresses, or cargo description (single search input)
- [ ] Filter by status dropdown
- [ ] Filter by customer dropdown (populated from Customer API)
- [ ] **Expandable rows**: clicking expand icon shows the trip panel for that order (loads Trip List lazily)
- [ ] Row actions: Edit, Complete (if Delivered), Cancel (if not terminal)
- [ ] Confirm dialog for Complete and Cancel actions
- [ ] Follows same patterns as Customer/Driver list page

#### S-26: Order Form Dialog + Status Actions [L]

> As a fleet manager, I want to create and edit orders through a form dialog.

**Acceptance Criteria:**

- [ ] PrimeNG Dialog with form fields:
  - Customer (dropdown populated from Customer API — Active customers only)
  - PickupAddress (text, required)
  - DeliveryAddress (text, required)
  - CargoDescription (text, required)
  - CargoWeightKg (number)
  - RequestedPickupDate (date picker)
  - RequestedDeliveryDate (date picker)
  - QuotedPrice (currency input)
  - Notes (textarea)
- [ ] Reusable for both Create and Edit modes
- [ ] Validation: required fields, delivery date ≥ pickup date, weight > 0
- [ ] Edit mode: Status shown as read-only tag; fields disabled if order is terminal
- [ ] Cancel action: shows textarea for CancellationReason (required) + confirm button
- [ ] On save: calls API, refreshes table + status summary, shows success toast
- [ ] Follows same patterns as Customer form dialog

---

## Epic B: Trip Management

> As a fleet manager, I want to assign trucks and drivers to orders, track trip progress, and record costs — so I can manage the physical execution of each transportation job.

### User Stories

#### S-27: Trip Model & Migration [M]

> As a system, I need a Trip entity to store trip execution data scoped to a tenant.

**Acceptance Criteria:**

- [ ] `Trip` model exists with:
  - `TripId` (Guid v7, PK)
  - `TenantId` (Guid, FK → Tenant)
  - `TripNumber` (string, max 20, auto-generated, unique per tenant)
  - `OrderId` (Guid, FK → Order)
  - `TruckId` (Guid, FK → Truck)
  - `DriverId` (Guid, FK → Driver)
  - `PlannedPickupDate` (DateTime?)
  - `PlannedDeliveryDate` (DateTime?)
  - `ActualPickupDate` (DateTime?)
  - `ActualDeliveryDate` (DateTime?)
  - `Status` (int, default 1) — 1=Planned, 2=InTransit, 3=Completed, 4=Cancelled
  - `FuelCost` (decimal?)
  - `TollCost` (decimal?)
  - `OtherCost` (decimal?)
  - `CostNotes` (string?, max 500)
  - `CancellationReason` (string?, max 500)
  - `Notes` (string?)
  - `CompletedAt` (DateTimeOffset?)
  - `CancelledAt` (DateTimeOffset?)
  - `CreatedAt` (DateTimeOffset)
  - `UpdatedAt` (DateTimeOffset?)
  - Navigation: `Order`, `Truck`, `Driver`
- [ ] Implements `ITenantEntity`
- [ ] `DbSet<Trip>` registered in `AppDbContext`
- [ ] EF config: Tenant FK (Restrict), Order FK (Cascade delete — if order deleted during draft, trips go too), Truck FK (Restrict), Driver FK (Restrict), TenantId index, OrderId index, (TenantId + TripNumber) unique index
- [ ] **Single migration** for both Order + Trip tables (since Trip has FK to Order)
- [ ] Seed data: 5 sample trips linked to seed orders (mix of statuses matching order statuses)

#### S-28: Trip CRUD + Lifecycle API [XL]

> As a fleet manager, I want API endpoints to create, view, list, update, start, complete, and cancel trips.

**Acceptance Criteria:**

- [ ] `TripController` with endpoints:
  - `POST Create` — create trip (assign truck + driver to order)
  - `GET GetById` — get single trip by ID
  - `GET List` — paginated + orderId filter + status filter
  - `PUT Update` — partial update of trip details (Planned status only for truck/driver changes)
  - `PATCH Start` — transition Planned → InTransit (records ActualPickupDate)
  - `PATCH Complete` — transition InTransit → Completed (records delivery date + costs)
  - `PATCH Cancel` — transition to Cancelled (with reason)
- [ ] `TripService` with business logic and cascading:
  - Auto-generate TripNumber on create (`TRP-{sequential:D6}`)
  - Validate Order exists, not terminal, no active trip (one-active-trip-per-order rule)
  - Validate Truck not on another active trip
  - Validate Driver is Active, license not expired, not on another active trip
  - On Create: cascade Order → Assigned
  - On Start: cascade Order → InTransit
  - On Complete: cascade Order → Delivered (if no other active trips)
  - On Cancel: cascade Order → Created (if no other active trips)
  - Cross-repo access: `OrderRepo`, `TruckRepo`, `DriverRepo` for validation and cascading
- [ ] DTOs: `CreateTripDto`, `UpdateTripDto`, `TripDto` (response with computed: `StatusLabel`, `OrderNumber`, `TruckLicensePlate`, `DriverFullName`, `TotalCost`)
- [ ] `StartTripDto` (optional ActualPickupDate), `CompleteTripDto` (TripId, ActualDeliveryDate, costs), `CancelTripDto` (TripId, CancellationReason)
- [ ] AutoMapper profile for Trip ↔ DTOs
- [ ] All endpoints `[Authorize]`, data scoped by tenant (via global filter)

#### S-29: Trip Panel in Order List [L]

> As a fleet manager, I want to see trips for an order inside the order list's expandable row.

**Acceptance Criteria:**

- [ ] **Expandable row template** in Order DataTable — click to expand shows a trip sub-table
- [ ] Trip sub-table columns: TripNumber, TruckLicensePlate, DriverFullName, PlannedPickupDate, ActualPickupDate, Status, TotalCost
- [ ] Status tags: Planned → blue, InTransit → cyan, Completed → green, Cancelled → red
- [ ] Trip sub-table loads lazily when row is expanded (calls `GET /api/Trip/List?orderId=xxx`)
- [ ] "Assign Truck & Driver" button visible when order has no active trip (opens Trip form dialog)
- [ ] Per-trip row actions: Start (if Planned), Complete (if InTransit), Cancel (if not terminal), Edit (if Planned)
- [ ] After any trip action: refresh both the trip sub-table and the order row (status may have changed via cascading)

#### S-30: Trip Form Dialog + Status Actions [L]

> As a fleet manager, I want to create and manage trips through form dialogs.

**Acceptance Criteria:**

- [ ] **Trip Create/Edit Dialog** (PrimeNG Dialog):
  - OrderNumber shown as read-only label (context)
  - Truck dropdown: shows `LicensePlate (Brand)` — loads from Truck API, only shows trucks not on active trips
  - Driver dropdown: shows `FullName (PhoneNumber)` — loads from Driver API (Active + no active trip + license valid)
  - PlannedPickupDate (date picker)
  - PlannedDeliveryDate (date picker)
  - Notes (textarea)
- [ ] **Start Trip action**: confirm dialog → calls Start API → refreshes UI
- [ ] **Complete Trip Dialog** (PrimeNG Dialog):
  - ActualDeliveryDate (date picker, required, defaults to today)
  - FuelCost (currency input)
  - TollCost (currency input)
  - OtherCost (currency input)
  - CostNotes (text)
  - Notes (textarea)
- [ ] **Cancel Trip action**: textarea for CancellationReason (required) + confirm button
- [ ] After any action: refresh trip panel + parent order row in table
- [ ] Follows same dialog/form patterns as existing components

---

## Tasks

| #   | Task                                                                   | Agent    | Story      | Depends On | Status      |
| --- | ---------------------------------------------------------------------- | -------- | ---------- | ---------- | ----------- |
| 1   | [BE] Create Order + Trip models + EF config + migration                | Backend  | S-23, S-27 | —          | Not Started |
| 2   | [BE] Create seed data for Orders + Trips                               | Backend  | S-23, S-27 | 1          | Not Started |
| 3   | [BE] Create OrderRepo + TripRepo                                       | Backend  | S-24, S-28 | 1          | Not Started |
| 4   | [BE] Create Order DTOs + AutoMapper profile                            | Backend  | S-24       | 1          | Not Started |
| 5   | [BE] Create Trip DTOs + AutoMapper profile                             | Backend  | S-28       | 1          | Not Started |
| 6   | [BE] Create OrderService (CRUD + Complete + Cancel + Summary)          | Backend  | S-24       | 3, 4       | Not Started |
| 7   | [BE] Create TripService (CRUD + Start + Complete + Cancel + cascading) | Backend  | S-28       | 3, 5       | Not Started |
| 8   | [BE] Create OrderController                                            | Backend  | S-24       | 6          | Not Started |
| 9   | [BE] Create TripController                                             | Backend  | S-28       | 7          | Not Started |
| 10  | [FE] Create Order + Trip TypeScript models                             | Frontend | S-25, S-29 | 4, 5       | Not Started |
| 11  | [FE] Create OrderService (HTTP client)                                 | Frontend | S-25       | 8          | Not Started |
| 12  | [FE] Create TripService (HTTP client)                                  | Frontend | S-29       | 9          | Not Started |
| 13  | [FE] Create Order list page with DataTable + status summary            | Frontend | S-25       | 10, 11     | Not Started |
| 14  | [FE] Create Order form dialog (Create/Edit)                            | Frontend | S-26       | 10, 11     | Not Started |
| 15  | [FE] Create Trip expandable panel in order rows                        | Frontend | S-29       | 12, 13     | Not Started |
| 16  | [FE] Create Trip form dialog (Create/Edit + truck/driver select)       | Frontend | S-30       | 12, 13     | Not Started |
| 17  | [FE] Create status action buttons + Complete/Cancel dialogs            | Frontend | S-26, S-30 | 14, 15, 16 | Not Started |
| 18  | [FE] Add order routes + sidebar nav                                    | Frontend | S-25       | 13         | Not Started |

**Parallelism Notes:**

- Task 1 has no dependencies — start here
- After T1: tasks 2, 3, 4, 5 can start in parallel
- Backend Order track (T1 → T3+T4 → T6 → T8) and Backend Trip track (T1 → T3+T5 → T7 → T9) can run in parallel
- Frontend tasks (10–18) depend on backend APIs being ready
- Frontend Order (T13, T14) and Frontend Trip (T15, T16) can be parallelized once services are ready
- T17 (action buttons/dialogs) is the last UI task — depends on all form/panel components

---

## Dependencies

```
Backend:
  T1 (Models + Migration) ──→ T2 (Seed Data)
          ├──→ T3 (Repos) ──┬──→ T6 (OrderService) ──→ T8 (OrderController)
          ├──→ T4 (Order DTOs) ─┘
          ├──→ T3 (Repos) ──┬──→ T7 (TripService) ──→ T9 (TripController)
          └──→ T5 (Trip DTOs) ──┘

Frontend:
  T4+T5 ──→ T10 (TS Models)
  T8 ──→ T11 (OrderService) ──┬──→ T13 (Order List) ──→ T18 (Routes+Nav)
  T9 ──→ T12 (TripService)    ├──→ T14 (Order Dialog)
                               └──→ T15 (Trip Panel) ──┐
  T12+T13 ──→ T16 (Trip Dialog)                        ├──→ T17 (Status Actions)
                                           T14+T16 ────┘
```

**Critical Path**: T1 → T3+T4 → T6 → T8 → T11 → T13 → T15 → T17

---

## Decisions

| Date       | Decision                                                        | Rationale                                                                                                                                            |
| ---------- | --------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------- |
| 2026-03-29 | One-to-many Order → Trip relationship                           | Supports trip replacement if cancelled. Historical record preserved. Even though MVP limits to one active trip per order.                            |
| 2026-03-29 | One active trip per order (MVP constraint)                      | Simplifies cascading logic. Household businesses typically run 1 truck per order. Can be relaxed in a future sprint.                                 |
| 2026-03-29 | Trip status cascades to Order status automatically              | Minimizes manual clicks. Fleet manager manages trips; order status follows naturally. Reduces human error.                                           |
| 2026-03-29 | No separate Trip page — trips in expandable order rows          | Trips are always contextual to an order. Keeps the UX focused. Avoids page proliferation. PrimeNG DataTable natively supports row expansion.         |
| 2026-03-29 | Auto-generated OrderNumber / TripNumber (sequential per tenant) | Human-readable references for customer calls, printed docs, and quick lookup. `ORD-000001` / `TRP-000001` format.                                    |
| 2026-03-29 | No hard delete for Orders/Trips — use Cancellation              | Business records must be preserved for history, audit, and future invoicing. Cancel is the soft-close mechanism.                                     |
| 2026-03-29 | Cost recording as fields on Trip (not a separate Cost entity)   | Three cost fields (fuel, toll, other) cover 90%+ of household trucking expenses. A separate entity overcomplicates Sprint 4. Extend later if needed. |
| 2026-03-29 | Services may access multiple repos (cross-repo pattern)         | `TripService` needs `OrderRepo`, `TruckRepo`, `DriverRepo` for validation and cascading. Acceptable for business logic services.                     |
| 2026-03-29 | Order + Trip in a single EF migration                           | Trip has FK to Order, so both tables must exist in the same migration to avoid FK errors.                                                            |
| 2026-03-29 | Status pipeline summary via dedicated `StatusSummary` endpoint  | Cannot compute accurate totals from paginated list data. Simple `GROUP BY` query is efficient.                                                       |

---

## Notes

_Notes will be added as tasks are completed._

### Implementation Hints (for dev agents)

- **OrderNumber generation**: `var count = await orderRepo.Query().CountAsync(); var num = $"ORD-{(count + 1):D6}";` — protected by unique index on (TenantId, OrderNumber). Low concurrency in household business makes race conditions negligible.
- **Trip availability checks**: Query `tripRepo.Query().Where(t => t.TruckId == truckId && (t.Status == 1 || t.Status == 2))` to find active trips for a truck/driver.
- **Cascading pattern**: After any trip status change, call a private `SyncOrderStatusAsync(orderId)` method that evaluates the current state of all trips and sets the appropriate order status.
- **Navigation properties**: Order has `Customer` + `Trips` nav props. Trip has `Order` + `Truck` + `Driver` nav props. Use `.Include()` only when needed for DTO mapping (e.g., `CustomerName`, `TruckLicensePlate`).
- **Existing patterns to follow**: `CustomerService` for CRUD, `TruckService.UpdateStatusAsync` for PATCH-style operations, `CustomerModelConfig` for EF config, `CustomerDataSeeder` for seed data.
