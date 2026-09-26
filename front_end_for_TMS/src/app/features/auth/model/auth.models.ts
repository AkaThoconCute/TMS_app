// Request DTOs
export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterDto {
  fullName: string;
  email: string;
  password: string;
}

export interface TokenDto {
  refreshToken: string;
}

// Response DTOs
export interface AuthDTO {
  success: boolean;
  accessToken: string;
  refreshToken: string;
  errors: string[] | null;
}

export interface UserProfile {
  email: string;
  userName: string;
  roles: string[];
  tenantId: string | null;
  tenantName: string | null;
}

// API Response wrapper
export interface ApiResponse<T> {
  instance: string;
  success: boolean;
  status: number;
  data: T;
}

export interface AppResult<T> {
  success: boolean;
  result: T | null;
  error: {
    code: number;
    message: string;
  } | null;
}

// Profile update
export interface UpdateProfileDto {
  userName: string;
}

// Change password
export interface ChangePasswordDto {
  currentPassword: string;
  newPassword: string;
}

export interface ChangePasswordResult {
  success: boolean;
  errors: string[];
}

export interface ForgotPasswordDto {
  email: string;
}

export interface ResetPasswordDto {
  email: string;
  token: string;
  newPassword: string;
}

export interface ForgotPasswordResult {
  success: boolean;
  message: string;
}
