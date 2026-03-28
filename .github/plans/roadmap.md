# EasyTMS Product Roadmap

**Last Updated**: 2026-03-22  
**Author**: Derek6_PO (Product Owner)

---

## Vision

EasyTMS provides an **Easy – Fast – Save** solution for household truck transportation businesses. The roadmap progresses from data foundation → core operations → business value → advanced features.

---

## Roadmap Overview

```
Phase 1 ✅  →  Phase 2 🔄  →  Phase 3  →  Phase 4  →  Phase 5  →  Phase 6+
Auth +         Tenant +       Orders &     Customer    Invoicing    Advanced
Truck          Driver         Trips        Mgmt        & Billing    Features
```

---

## Phase 1: Auth + Truck ✅ DONE

**Delivered**: 2026-03-22

| Feature | Status |
|---|---|
| JWT Authentication (Register, Login, Refresh, Logout) | Done |
| User Profile (GetMe) | Done |
| Truck CRUD with pagination, search, status filter | Done |
| Angular SPA with PrimeNG DataTable, form dialogs | Done |
| Backend 4-layer architecture, GlobalExceptionHandler, AutoMapper | Done |
| Seed data (Identity + Trucks) | Done |

**Report**: [phase-1-report.md](phase-1-report.md)

---

## Phase 2: Multi-Tenant + Core Features ✅ DONE

**Plan**: [phase-2-plan.md](phase-2-plan.md)

### 2A: Multi-Tenant Data Isolation (Sprint 1) ✅ DONE
| Deliverable | MoSCoW |
|---|---|
| `Tenant` entity + migration | Must |
| `TenantId` on Truck + AppUser | Must |
| `ITenantEntity` interface | Must |
| `TenantContext` scoped service + JWT middleware | Must |
| EF Core Global Query Filter | Must |
| Auto-stamp TenantId on SaveChanges | Must |
| Auth flow updated (Register creates Tenant, Login includes TenantId) | Must |
| Frontend stores tenant context | Must |

### 2B: Driver Management (Sprint 2) ✅ DONE

| Deliverable | MoSCoW |
|---|---|
| Driver model + CRUD API | Must |
| Driver list page (PrimeNG DataTable) | Must |
| Driver form dialog (Create/Edit) | Must |
| Driver-Truck assignment | Should |
| Salary page placeholder | Could |

### 2C: Core Operations (Sprint 3) — SHOULD HAVE

| Deliverable | MoSCoW |
|---|---|
| User profile page (view/edit + change password) | Should |
| Reset password (complete implementation) | Should |
| Truck maintenance records | Should |
| Role-based UI visibility | Could |

---

## Phase 3: Order & Trip Management — MUST HAVE

> Core TMS value — managing shipments from pickup to delivery.

| Feature | Priority |
|---|---|
| Order entity (Customer, Pickup, Delivery, Cargo, Status) | Must |
| Trip/Route entity (assign Truck + Driver to Order) | Must |
| Order lifecycle (Created → Assigned → InTransit → Delivered → Completed) | Must |
| Order list page with status pipeline view | Must |
| Trip tracking (status updates, timestamps) | Should |
| Trip cost recording (fuel, tolls, other expenses) | Should |

---

## Phase 4: Customer Management — SHOULD HAVE

> Track who goods are being transported for.

| Feature | Priority |
|---|---|
| Customer entity (Name, Phone, Address, ContactPerson) | Must |
| Customer CRUD + list page | Must |
| Link customers to orders | Must |
| Customer order history view | Should |
| Customer address autocomplete | Could |

---

## Phase 5: Invoicing & Billing — SHOULD HAVE

> Financial tracking for transportation services.

| Feature | Priority |
|---|---|
| Invoice entity linked to Orders | Must |
| Invoice generation from completed orders | Must |
| Invoice list + detail pages | Must |
| Payment status tracking (Pending, Paid, Overdue) | Should |
| PDF export / print | Could |
| Revenue summary by period | Could |

---

## Phase 6: Reporting & Analytics — COULD HAVE

| Feature | Priority |
|---|---|
| Dashboard charts (Chart.js via PrimeNG) | Should |
| Fleet utilization report | Should |
| Revenue by customer/period | Should |
| Driver performance metrics | Could |
| CSV/Excel export | Could |

---

## Phase 7: Notifications & Alerts — COULD HAVE

| Feature | Priority |
|---|---|
| License expiry alerts (Driver + Truck registration) | Should |
| Maintenance due reminders | Should |
| In-app notification center | Could |
| Email notifications | Could |

---

## Phase 8: Advanced Features — WON'T HAVE (Yet)

| Feature | Notes |
|---|---|
| GPS / real-time truck tracking | Requires hardware integration |
| Route optimization | Complex algorithm, low ROI for household businesses |
| Mobile app | Consider after web MVP stabilizes |
| SQL Server RLS hardening | Layer on top of application-level tenancy if needed |
| Multi-language (i18n) | English + Vietnamese later |
| Audit log / activity history | Nice for compliance |

---

## Decision Log

| Date | Decision | Rationale |
|---|---|---|
| 2026-03-22 | Application-level multi-tenancy (EF Core Global Query Filters) over SQL Server RLS | Practical for household business scale. Simpler to implement, test, and debug. Clear upgrade path to RLS if needed. |
| 2026-03-22 | Tenant entity named `Tenant` (not Company) | Generic, future-proof naming. |
| 2026-03-22 | Data isolation before new features | Every entity needs `TenantId`. Building without it means costly refactoring. |
| 2026-03-22 | Shared Table, Private Row strategy | One database, one schema. Cost-effective, simple ops. Suitable for household business customer base. |
