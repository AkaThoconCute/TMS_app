// Request DTOs
export interface LoginDto {
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
export interface AuthResult {
  success: boolean;
  token: string;
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
