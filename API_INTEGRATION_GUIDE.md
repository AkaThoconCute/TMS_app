# REST API Integration Summary

## Overview

Successfully integrated ASP.NET REST API endpoints for login and registration into the Angular application using a feature-based architecture.

## Directory Structure Created

```
src/app/features/account/
├── models/
│   └── auth.models.ts          # DTOs and interfaces
└── services/
    └── account-api.service.ts   # API service for account operations
```

## Files Created

### 1. Models (`src/app/features/account/models/auth.models.ts`)

- **Request DTOs**: `LoginDto`, `RegisterDto`, `TokenDto`
- **Response DTOs**: `AuthResult`, `UserProfile`
- **API Wrapper**: `ApiResponse<T>` - handles ASP.NET response format

### 2. API Service (`src/app/features/account/services/account-api.service.ts`)

Provides methods:

- `login(credentials: LoginDto)` - POST `/api/Account/Login`
- `register(userData: RegisterDto)` - POST `/api/Account/Register`
- `refreshToken(dto: TokenDto)` - POST `/api/Account/RefreshToken`
- `logout()` - POST `/api/Account/Logout` (requires auth)
- `getProfile()` - GET `/api/Account/GetMe` (requires auth)
- `getPublicInfo()` - GET `/api/Account/GetInfo`

### 3. Updated Components

#### Login Component (`src/app/pages/login/login.component.ts`)

- Integrated API service
- Added validation (email format, required fields)
- Handles loading states
- Stores tokens in localStorage
- Redirects to `/home` on successful login
- Displays error/success messages

**Template changes**:

- Added error/success alert messages
- Added loading indicator on button
- Disabled inputs during API call

#### Register Component (`src/app/pages/register/register.component.ts`)

- Integrated API service
- Added password confirmation validation
- Validates password length (min 6 characters)
- Handles loading states
- Redirects to `/login` on successful registration
- Displays error/success messages

**Template changes**:

- Added confirm password field
- Added error/success alert messages
- Added loading indicator on button
- Disabled inputs during API call

### 4. App Configuration (`src/app/app.config.ts`)

- Added `provideHttpClient()` for HTTP support

## API Response Format

The ASP.NET backend returns responses in this format:

```json
{
  "instance": "/api/Account/Login",
  "success": true,
  "status": 200,
  "data": {
    "success": true,
    "token": "...",
    "refreshToken": "...",
    "errors": null
  }
}
```

The service automatically extracts the `data` field and returns it to components.

## Token Storage

- Access tokens are stored in `localStorage` with key: `token`
- Refresh tokens are stored in `localStorage` with key: `refreshToken`

## Error Handling

- Validates user input before API calls
- Displays user-friendly error messages
- Logs errors to browser console for debugging

## Features

### Login Feature

- Email and password validation
- Automatic token storage
- Redirect to home on success
- Error message display
- Loading state during API call

### Register Feature

- Full name, email, password validation
- Password confirmation matching
- Password strength requirement (min 6 chars)
- Automatic redirect to login on success
- Error message display
- Loading state during API call

## Next Steps (Optional Enhancements)

1. **HTTP Interceptor** for automatic token injection:

   ```typescript
   // Would automatically add Authorization header to all requests
   ```

2. **Auth Guard** for protected routes:

   ```typescript
   // Would prevent access to protected pages without token
   ```

3. **Token Refresh Logic**:

   ```typescript
   // Automatic token refresh using refreshToken
   ```

4. **Session Storage**:
   ```typescript
   // Store current user profile in a service
   ```

## Testing the Integration

### Prerequisites

- ASP.NET backend running on `http://localhost:8080`
- Ensure CORS is configured on backend

### Test Credentials (if available from backend)

- Email: `user1@asp.app`
- Password: `Nhan@111`

### Manual Testing Steps

1. **Register New User**:
   - Navigate to `/register`
   - Fill in name, email, password, confirm password
   - Click "Get Started"
   - Should redirect to login on success

2. **Login**:
   - Navigate to `/login`
   - Enter email and password
   - Click "Sign In"
   - Should redirect to `/home` on success
   - Check localStorage for tokens (`token` and `refreshToken`)

3. **Error Scenarios**:
   - Invalid email format
   - Mismatched passwords
   - Passwords too short
   - Missing fields
   - Network errors

## Configuration Notes

### API Endpoint

The service is configured for:

- **Base URL**: `http://localhost:8080`
- **Controller**: `Api/Account`
- **Route Pattern**: `[Route("api/[controller]/[action]")]`

To change the API endpoint, modify line in `account-api.service.ts`:

```typescript
private readonly apiUrl = 'http://localhost:8080/api/Account';
```

### CORS Configuration

Ensure the ASP.NET backend has CORS configured to accept requests from your Angular app domain.
