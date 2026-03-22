# EasyTMS

A transportation management system built for household trucking businesses. Easy to use, fast to set up, cost-effective to run.

## What It Does

EasyTMS helps small trucking operators manage their day-to-day operations in one place:

- **Account Management** — Register, log in, and secure access with JWT-based authentication
- **Truck Management** — Add, edit, remove, and search trucks with pagination and filtering

More modules (drivers, routes, orders, invoicing) are planned as the product grows.

## Tech Stack

| Layer    | Technology                             |
| -------- | -------------------------------------- |
| Frontend | Angular 21, PrimeNG 21, Tailwind CSS 4 |
| Backend  | ASP.NET Core 10, EF Core 10            |
| Database | SQL Server                             |
| Auth     | JWT + ASP.NET Identity                 |

## Project Structure

```
TMS_app/
├── back_end_for_TMS/    # ASP.NET Core Web API
└── front_end_for_TMS/   # Angular SPA
```

### Backend (`back_end_for_TMS/back_end_for_TMS`)

```
Api/                  # Controllers — HTTP endpoints
Business/             # Services & business logic
Common/               # Shared utilities (pagination, filtering, guards)
Models/               # EF Core entities
Infrastructure/       # Cross-cutting wiring
Migrations/           # EF Core migration files
Program.cs            # App entry point
```

### Frontend (`front_end_for_TMS/src/app/`)

```
features/             # Feature modules (one folder per domain)
platform/             # App-wide services (non-UI)
shell/                # Layout components
common/               # Reusable UI components
app.ts                # Root component
app.routes.ts         # Route definitions
app.config.ts         # App providers & config
app.prime.ts          # PrimeNG theme setup
```

## Getting Started

### Prerequisites

- .NET 10 SDK
- Node.js (LTS) + Yarn
- SQL Server instance

### Backend

```bash
cd back_end_for_TMS/back_end_for_TMS
dotnet ef database update
dotnet run
```

The API runs at `https://localhost:9090` by default.

### Frontend

```bash
cd front_end_for_TMS
yarn install
yarn dev
```

The app runs at `http://localhost:4200` by default.

## Contributing

1. Pick an issue or propose a feature
2. Create a branch from `main`
3. Make your changes and test locally
4. Open a pull request with a clear description
