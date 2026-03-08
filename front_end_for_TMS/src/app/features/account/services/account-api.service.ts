import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import {
  LoginDto,
  RegisterDto,
  TokenDto,
  AuthResult,
  UserProfile,
  ApiResponse
} from '../models/auth.models';
import { AppConfigService } from '../../../shared/services/app-config.service';

@Injectable({
  providedIn: 'root'
})
export class AccountApiService {
  private readonly apiUrl: string;

  constructor(
    private http: HttpClient,
    private configService: AppConfigService
  ) {
    this.apiUrl = this.configService.getApiUrl('/api/Account');
  }

  /**
   * Login with email and password
   * @param credentials Login credentials
   * @returns Observable of AuthResult
   */
  login(credentials: LoginDto): Observable<AuthResult> {
    return this.http
      .post<ApiResponse<AuthResult>>(`${this.apiUrl}/Login`, credentials)
      .pipe(
        map(response => response.data),
        catchError(this.handleError)
      );
  }

  /**
   * Register new user
   * @param userData Registration data
   * @returns Observable of AuthResult
   */
  register(userData: RegisterDto): Observable<AuthResult> {
    return this.http
      .post<ApiResponse<AuthResult>>(`${this.apiUrl}/Register`, userData)
      .pipe(
        map(response => response.data),
        catchError(this.handleError)
      );
  }

  /**
   * Refresh authentication token
   * @param dto Token refresh data
   * @returns Observable of AuthResult
   */
  refreshToken(dto: TokenDto): Observable<AuthResult> {
    return this.http
      .post<ApiResponse<AuthResult>>(`${this.apiUrl}/RefreshToken`, dto)
      .pipe(
        map(response => response.data),
        catchError(this.handleError)
      );
  }

  /**
   * Logout user
   * @returns Observable of success message
   */
  logout(): Observable<{ message: string }> {
    return this.http
      .post<ApiResponse<{ message: string }>>(`${this.apiUrl}/Logout`, {})
      .pipe(
        map(response => response.data),
        catchError(this.handleError)
      );
  }

  /**
   * Get current user profile
   * @returns Observable of UserProfile
   */
  getProfile(): Observable<UserProfile> {
    return this.http
      .get<ApiResponse<UserProfile>>(`${this.apiUrl}/GetMe`)
      .pipe(
        map(response => response.data),
        catchError(this.handleError)
      );
  }

  /**
   * Get public info
   * @returns Observable of public data
   */
  getPublicInfo(): Observable<{ message: string }> {
    return this.http
      .get<ApiResponse<{ message: string }>>(`${this.apiUrl}/GetInfo`)
      .pipe(
        map(response => response.data),
        catchError(this.handleError)
      );
  }

  /**
   * Handle HTTP errors
   * @param error HTTP error response
   * @returns Observable that errors
   */
  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'An error occurred';

    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = error.error.message;
    } else {
      // Server-side error
      errorMessage = error.error?.data?.errors?.[0] || error.message || 'Unknown error';
    }

    console.error('API Error:', errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}
