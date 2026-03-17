# Back-End for TMS (Transportation Management System)

## Overview

The Back-End for TMS is an ASP.NET Core 10.0 Web API that provides authentication, authorization, and account management services for the EasyTMS application. It follows a layered architecture with clear separation of concerns and implements JWT-based authentication with refresh token support.

## Tech Stack

- **Framework**: ASP.NET Core 10.0
- **Database**: SQL Server with Entity Framework Core 10.0.3
- **Authentication**: JWT Bearer Tokens with Microsoft.AspNetCore.Identity
- **Mapping**: AutoMapper for DTO-to-Model conversions
- **Data Seeding**: Bogus for generating test data
- **API Documentation**: OpenAPI/Swagger

## Project Structure

```
back_end_for_TMS/
├── Api/                          # API Controllers & Endpoints
│   └── AccountController.cs       # Account-related endpoints (Login, Register, Refresh Token)
├── Business/                      # Business Logic Layer
│   ├── AccountService.cs          # Account service implementation
│   ├── TokenService.cs            # JWT token generation and validation
│   └── Types/
│       └── AccountTypes.cs        # DTOs for account operations
├── Infrastructure/                # Infrastructure & Dependency Injection
│   ├── Api/
│   │   └── ApiExtensions.cs       # API service registration & configuration
│   ├── Business/
│   │   └── BusinessExtensions.cs  # Business service registration
│   ├── Database/
│   │   ├── AppDbContext.cs        # Entity Framework DbContext
│   │   ├── DatabaseExtensions.cs  # Database service registration
│   │   └── DatabaseSeeder.cs      # Database seeding with test data
│   ├── Mapper/
│   │   └── AppMapperProfile.cs    # AutoMapper profiles
│   ├── Response/
│   │   ├── GlobalExceptionHandler.cs   # Global exception handling middleware
│   │   └── GlobalResultFilter.cs       # Global result filter for consistent responses
│   ├── Security/
│   │   ├── AuthNExtensions.cs     # Authentication setup (JWT, Identity)
│   │   ├── AuthZExtensions.cs     # Authorization setup (roles, policies)
│   │   └── CorsExtensions.cs      # CORS configuration
├── Models/
│   └── AppUser.cs                 # User entity (extends IdentityUser)
├── Migrations/                    # EF Core migrations
├── Common/                        # Utility Classes
│   ├── FilterHelper.cs            # Helper utilities for filtering
│   └── Guard.cs                   # Input validation helpers
├── Program.cs                     # Application startup & configuration
├── back_end_for_TMS.csproj        # Project file with dependencies
└── appsettings.json               # Application configuration
```

## Architecture & Design Patterns

### Layered Architecture

The project follows a **4-layer architecture**:

1. **API Layer** (`Api/`)
   - Controllers handle HTTP requests
   - Route attribute: `api/[controller]/[action]`
   - Uses `[AllowAnonymous]` and `[Authorize]` for access control

2. **Business Layer** (`Business/`)
   - Contains core business logic
   - Services perform operations on data
   - Validates business rules before persisting to database

3. **Infrastructure Layer** (`Infrastructure/`)
   - Database context and migrations
   - Service registration and extensions
   - Security configuration (Authentication, Authorization, CORS)
   - Global middleware and filters (exception handling, result formatting)

4. **Models Layer** (`Models/`)
   - Database entities
   - `AppUser` extends `IdentityUser` for authentication

### Startup Pipeline (Program.cs)

```
Dependency Injection Registration Order:
1. Database Services (EF Core, DbContext)
2. CORS Services
3. Authentication Services (JWT)
4. Authorization Services (Roles, Policies)
5. Business Services
6. API Services (Controllers, Filters, OpenAPI)

Middleware Pipeline:
1. Exception Handler (Protect)
2. Database Connection Check
3. OpenAPI (Development only)
4. HTTPS Redirection
5. CORS
6. Authentication
7. Authorization
8. Controllers & Routes
```

## Key Features

### Authentication & Authorization

- **JWT Bearer Tokens**: Access token for API authentication
- **Refresh Tokens**: Long-lived tokens to generate new access tokens
- **Role-Based Access Control (RBAC)**: Authorization based on user roles
- **Identity Integration**: Built on ASP.NET Core Identity for user management

### Account Endpoints

```http
GET /api/account/getinfo
- Public endpoint for testing API availability
- Returns: { message: "This is public data" }

POST /api/account/register
- Public endpoint for user registration
- Body: { email, password }
- Returns: AuthResult (token, refreshToken, success)

POST /api/account/login
- Public endpoint for user login
- Body: { email, password }
- Returns: AuthResult (token, refreshToken, success)

POST /api/account/refreshtoken
- Public endpoint to refresh access token
- Body: { token, refreshToken }
- Returns: New AuthResult with fresh tokens

POST /api/account/logout
- Protected endpoint for logout
- Requires: Bearer token
- Returns: { message: "Logged out successfully" }
```

### Global Error Handling

- `GlobalExceptionHandler`: Catches all unhandled exceptions
- `GlobalResultFilter`: Wraps controller responses in consistent format
- Returns standardized error responses with status codes

### Data Seeding

- `DatabaseSeeder` populates database with test data using Bogus
- Automatic seeding on application startup

## Development Conventions

### Naming Conventions

- **Classes**: PascalCase (e.g., `AccountService`, `AppUser`)
- **Methods**: PascalCase (e.g., `RefreshToken`, `Register`)
- **Properties**: PascalCase (e.g., `RefreshToken`, `ExpiryTime`)
- **Parameters**: camelCase (e.g., `dto`, `userId`)
- **Private fields**: `_camelCase`

### File Organization

- One class per file (except DTOs which can be grouped)
- Namespaces follow folder structure: `back_end_for_TMS.[Folder]`
- Files named exactly as the primary class name

### Dependency Injection

- Constructor injection only (no service locator pattern)
- Use primary constructors (C# 12+): `public class Service(IRepository repo)`
- Extension methods for service registration: `services.AddBusinessServices(config)`

### DTOs (Data Transfer Objects)

Located in `Business/Types/`:

- `LoginDto`: { email, password }
- `RegisterDto`: { email, password }
- `TokenDto`: { token, refreshToken }
- `AuthResult`: { success, token, refreshToken, errors }

### Async Patterns

- Async methods marked with `async` keyword
- Use `await` for async operations
- Return `Task<T>` for async operations with results
- Return `Task` for async void-like operations

### Error Handling

- Throw exceptions for business logic failures
- Use descriptive exception messages
- Catch business exceptions in services, not in controllers
- Controllers return `ActionResult<T>` to handle both success and error cases

## Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=TMSDb;..."
  },
  "Jwt": {
    "SecretKey": "...",
    "ExpireMinutes": 15,
    "RefreshTokenExpireDays": 7
  },
  "Cors": {
    "AllowedOrigins": ["http://localhost:4200", "https://yourdomain.com"]
  }
}
```

### appsettings.Development.json

- Override settings for development environment
- Can include different connection strings, JWT secrets, etc.

## Database Migrations

### Creating a New Migration

```bash
Add-Migration [MigrationName]
```

### Applying Migrations

```bash
Update-Database
```

### Migration Files

- Located in `Migrations/` folder
- Named with timestamp: `20260219101043_[Name].cs`
- Designer files track model snapshots: `[Name].Designer.cs`
- `AppDbContextModelSnapshot.cs`: Current model state

## Common Tasks

### Adding a New API Endpoint

1. Add method to controller in `Api/AccountController.cs`
2. Define DTO if needed in `Business/Types/AccountTypes.cs`
3. Implement business logic in service (`Business/AccountService.cs`)
4. Register any new services in extension files
5. Update AutoMapper profiles if mapping needed

### Adding a New Database Entity

1. Create model class in `Models/`
2. Add DbSet to `AppDbContext`
3. Create migration: `Add-Migration [EntityName]`
4. Update database: `Update-Database`

### Debugging

- Enable development mode: `ASPNETCORE_ENVIRONMENT=Development`
- Check `appsettings.Development.json` for development-specific settings
- Review global exception handler for caught exceptions

## Security Considerations

- JWT tokens are short-lived (configurable, default 15 minutes)
- Refresh tokens are long-lived (configurable, default 7 days)
- All sensitive data in `appsettings.json` should use user secrets in development
- HTTPS is enforced in production
- CORS is configured to allow only specified domains
- Password hashing uses ASP.NET Core Identity default (PBKDF2)

## Testing the API

### Using HTTP Files

- `back_end_for_TMS.http` contains sample API requests
- Can be executed directly in VS Code with REST Client extension

### Sample Requests

```http
### Register
POST http://localhost:5000/api/account/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}

### Login
POST http://localhost:5000/api/account/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}

### Refresh Token
POST http://localhost:5000/api/account/refreshtoken
Content-Type: application/json

{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "abcd1234..."
}
```

## Running the Application

### Prerequisites

- .NET 10.0 SDK
- SQL Server (local or remote)
- Node.js (for front-end development)

### Build

```bash
dotnet build
```

### Run

```bash
dotnet run
```

### Development with Hot Reload

```bash
dotnet watch
```

## Troubleshooting

### Database Connection Issues

- Check `appsettings.json` for correct connection string
- Verify SQL Server is running
- Check firewall settings if using remote database

### JWT Token Errors

- Ensure `Jwt:SecretKey` in configuration is long enough
- Verify token hasn't expired
- Check refresh token is valid and hasn't expired

### CORS Issues

- Verify front-end URL is in `Cors:AllowedOrigins`
- Check `CorsExtensions.cs` for policy configuration
- Ensure credentials headers are correctly set if needed

## Documentation Files

- `API_INTEGRATION_GUIDE.md`: Detailed API integration guide
- This README: Project structure and conventions
- Source files: Inline documentation and comments in code

## Future Enhancements

- Add unit tests (xUnit, Moq)
- Implement logging (Serilog)
- Add API versioning
- Implement audit logging
- Add health check endpoints
- Implement rate limiting

---

**Last Updated**: March 2026  
**Framework Version**: .NET 10.0  
**Status**: Active Development
