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
  id: string;
  email: string;
  fullName: string;
  role: string;
}

// API Response wrapper
export interface ApiResponse<T> {
  instance: string;
  success: boolean;
  status: number;
  data: T;
}
