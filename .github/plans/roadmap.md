# EasyTMS Product Roadmap

**Last Updated**: 2026-03-28  
**Author**: Derek6_PO (Product Owner)

---

## Vision

EasyTMS provides an **Easy – Fast – Save** solution for household truck transportation businesses. The roadmap progresses from data foundation → core operations → business value → advanced features.

---

## Roadmap Overview

```
Foundation ✅  →  Sprint 1 ✅  →  Sprint 2 ✅  →  Sprint 3 🔄  →  Sprint 4    →  Sprint 5    →  Sprint 6+
Auth + Truck      Multi-tenant     Driver          User +           Order &       Invoicing      Reporting,
                                                   Customer         Trip Mgmt     & Billing      Notifications
```

---

## Foundation: Auth + Truck ✅ DONE

**Delivered**: 2026-03-24  
**Plan**: [sprint-0.md](sprint-0.md)  
**Stories**: 

| Deliverable                                           | Status |
| ----------------------------------------------------- | ------ |
| JWT Authentication (Register, Login, Refresh, Logout) | Done   |
| User Profile (GetMe)                                  | Done   |
| Truck CRUD with pagination, search, status filter     | Done   |
| Angular SPA with PrimeNG DataTable, form dialogs      | Done   |

---

## Sprint 1: Multi-Tenant ✅ DONE

**Delivered**: 2026-03-24  
**Plan**: [sprint-1.md](sprint-1.md)  
**Stories**: S-01 → S-08

| Deliverable                                           | Status |
| ----------------------------------------------------- | ------ |
| Tenant entity + TenantId on all entities              | Done   |
| EF Core Global Query Filters + auto-stamp TenantId    | Done   |
| TenantContext scoped service + JWT tenant claim       | Done   |
| Frontend stores tenant context, displays tenant name  | Done   |

---

## Sprint 2: Driver Management ✅ DONE

**Delivered**: 2026-03-28  
**Plan**: [sprint-2.md](sprint-2.md)  
**Stories**: S-09 → S-14 (S-13 deferred)

| Deliverable                                        | Status   |
| -------------------------------------------------- | -------- |
| Driver model + CRUD API (tenant-scoped)            | Done     |
| Driver list page (PrimeNG DataTable, lazy loading) | Done     |
| Driver form dialog (Create/Edit)                   | Done     |
| License expiry warnings (30-day highlight)         | Done     |
| Salary placeholder page                            | Done     |
| Driver-Truck assignment (S-13)                     | Deferred |

---

## Sprint 3: User Management + Customer Management 🔄 ACTIVE

**Plan**: [sprint-3.md](sprint-3.md)  
**Stories**: S-15 → S-22

| Deliverable                                          | Status      |
| ---------------------------------------------------- | ----------- |
| User profile page (view/edit username)               | Not Started |
| Change password (authenticated)                      | Not Started |
| Forgot/Reset password flow                           | Not Started |
| Role-based UI visibility                             | Not Started |
| Customer model + CRUD API (tenant-scoped)            | Not Started |
| Customer list page (PrimeNG DataTable, lazy loading) | Not Started |
| Customer form dialog (Create/Edit)                   | Not Started |

---

## Sprint 4 (Planned): Order & Trip Management

> Core TMS value — managing shipments from pickup to delivery.

| Feature                                                      | Priority |
| ------------------------------------------------------------ | -------- |
| Order entity (Customer FK, Pickup, Delivery, Cargo, Status)  | Must     |
| Trip/Route entity (assign Truck + Driver to Order)           | Must     |
| Order lifecycle (Created → Assigned → InTransit → Delivered) | Must     |
| Order list page with status pipeline view                    | Must     |
| Trip tracking (status updates, timestamps)                   | Should   |
| Trip cost recording (fuel, tolls, other expenses)            | Should   |

---

## Sprint 5 (Planned): Invoicing & Billing

> Financial tracking for transportation services.

| Feature                                          | Priority |
| ------------------------------------------------ | -------- |
| Invoice entity linked to Orders                  | Must     |
| Invoice generation from completed orders         | Must     |
| Invoice list + detail pages                      | Must     |
| Payment status tracking (Pending, Paid, Overdue) | Should   |
| PDF export / print                               | Could    |
| Revenue summary by period                        | Could    |

---

## Future Sprints (Backlog)

### Reporting & Analytics

| Feature                    | Priority |
| -------------------------- | -------- |
| Dashboard charts (PrimeNG) | Should   |
| Fleet utilization report   | Should   |
| Revenue by customer/period | Should   |
| Driver performance metrics | Could    |
| CSV/Excel export           | Could    |

### Notifications & Alerts

| Feature                                | Priority |
| -------------------------------------- | -------- |
| License expiry alerts (Driver + Truck) | Should   |
| Maintenance due reminders              | Should   |
| In-app notification center             | Could    |
| Email notifications                    | Could    |

### Operations & Polish

| Feature                   | Priority |
| ------------------------- | -------- |
| Truck maintenance records | Should   |
| Driver-Truck assignment   | Should   |
| Driver salary management  | Should   |

### Advanced (Won't Have Yet)

| Feature                        | Notes                                          |
| ------------------------------ | ---------------------------------------------- |
| GPS / real-time truck tracking | Requires hardware integration                  |
| Route optimization             | Complex algorithm, low ROI for household scale |
| Mobile app                     | Consider after web MVP stabilizes              |
| SQL Server RLS hardening       | Layer on top of app-level tenancy if needed    |
| Multi-language (i18n)          | English + Vietnamese later                     |
| Audit log / activity history   | Nice for compliance                            |

---

## Decision Log

| Date       | Decision                                                                           | Rationale                                                                                                                                         |
| ---------- | ---------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------- |
| 2026-03-22 | Application-level multi-tenancy (EF Core Global Query Filters) over SQL Server RLS | Practical for household business scale. Simpler to implement, test, and debug. Clear upgrade path to RLS if needed.                               |
| 2026-03-22 | Tenant entity named `Tenant` (not Company)                                         | Generic, future-proof naming.                                                                                                                     |
| 2026-03-22 | Data isolation before new features                                                 | Every entity needs `TenantId`. Building without it means costly refactoring.                                                                      |
| 2026-03-22 | Shared Table, Private Row strategy                                                 | One database, one schema. Cost-effective, simple ops. Suitable for household business customer base.                                              |
| 2026-03-28 | Customer pulled forward into Sprint 3 (before Orders)                              | Orders require `CustomerId` FK. Building Customer now unblocks Order management in Sprint 4.                                                      |
| 2026-03-28 | Truck maintenance records deferred to backlog                                      | Sprint 3 focuses on User + Customer. Maintenance is independent and can be added later without blocking anything.                                 |
| 2026-03-28 | Sprint-based planning replaces phase-based                                         | Each sprint is self-contained with its own plan document. Roadmap stays as high-level overview only. Cleaner tracking, less document duplication. |
