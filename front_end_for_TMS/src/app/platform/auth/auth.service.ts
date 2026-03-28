import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { tap, catchError, map, take, filter } from 'rxjs/operators';
import { throwError } from 'rxjs';
import {
  LoginDto,
  RegisterDto,
  TokenDto,
  AuthResult,
  UserProfile,
  ApiResponse,
  UpdateProfileDto,
  ChangePasswordDto,
  ChangePasswordResult,
  ForgotPasswordDto,
  ResetPasswordDto,
  ForgotPasswordResult,
} from './auth.models';
import { EnvService } from '../env/env.service';
import { CookieService } from '../cookie/cookie.service';

/**
 * Authentication Service
 * Handles both HTTP API calls and authentication state management
 */
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl: string;
  private readonly TOKEN_KEY = 'token';
  private readonly REFRESH_TOKEN_KEY = 'refreshToken';
  private isRefreshing = false;

  // Observable state
  private isAuthenticatedSubject!: BehaviorSubject<boolean>;
  private currentUserSubject!: BehaviorSubject<UserProfile | null>;
  private refreshTokenSubject = new BehaviorSubject<string | null>(null);

  // Public observables
  isAuthenticated$!: Observable<boolean>;
  currentUser$!: Observable<UserProfile | null>;

  constructor(
    private http: HttpClient,
    private configService: EnvService,
    private cookieService: CookieService
  ) {
    this.apiUrl = this.configService.getApiUrl('/api/Account');

    this.isAuthenticatedSubject = new BehaviorSubject<boolean>(this.isTokenValid());
    this.currentUserSubject = new BehaviorSubject<UserProfile | null>(null);

    this.isAuthenticated$ = this.isAuthenticatedSubject.asObservable();
    this.currentUser$ = this.currentUserSubject.asObservable();
  }

  /**
   * Check if user is currently authenticated
   */
  get isAuthenticated(): boolean {
    return this.isAuthenticatedSubject.value;
  }

  /**
   * Get current user synchronously
   */
  get currentUser(): UserProfile | null {
    return this.currentUserSubject.value;
  }

  /**
   * Check if the current user has a specific role (case-insensitive)
   */
  hasRole(role: string): boolean {
    return this.currentUser?.roles?.some(r => r.toLowerCase() === role.toLowerCase()) ?? false;
  }

  initAuth(): Observable<UserProfile | null> {
    if (this.isTokenValid()) {
      return this.loadCurrentUser().pipe(
        catchError(() => {
          this.clearAuth();
          return of(null);
        })
      );
    }
    return of(null);
  }

  /**
   * Check authentication status on app initialization
   */
  private checkAuthStatus(): void {
    if (this.isTokenValid()) {
      this.isAuthenticatedSubject.next(true);
      // Load user profile in background
      this.loadCurrentUser().subscribe({
        error: () => this.clearAuth()
      });
    }
  }

  /**
   * Login user
   */
  login(credentials: LoginDto): Observable<AuthResult> {
    return this.http
      .post<ApiResponse<AuthResult>>(`${this.apiUrl}/Login`, credentials)
      .pipe(
        map(response => response.data),
        tap((result) => {
          if (result.success && result.token) {
            this.setTokens(result.token, result.refreshToken);
            this.isAuthenticatedSubject.next(true);
            this.loadCurrentUser().subscribe();
          }
        }),
        catchError((error) => {
          this.clearAuth();
          return this.handleError(error);
        })
      );
  }

  /**
   * Register new user
   */
  register(userData: RegisterDto): Observable<AuthResult> {
    return this.http
      .post<ApiResponse<AuthResult>>(`${this.apiUrl}/Register`, userData)
      .pipe(
        map(response => response.data),
        tap((result) => {
          if (result.success && result.token) {
            this.setTokens(result.token, result.refreshToken);
            this.isAuthenticatedSubject.next(true);
            this.loadCurrentUser().subscribe();
          }
        }),
        catchError((error) => {
          this.clearAuth();
          return this.handleError(error);
        })
      );
  }

  /**
   * Logout user
   */
  logout(): Observable<{ message: string }> {
    return this.http
      .post<ApiResponse<{ message: string }>>(`${this.apiUrl}/Logout`, {})
      .pipe(
        map(response => response.data),
        tap(() => this.clearAuth()),
        catchError((error) => {
          this.clearAuth();
          return this.handleError(error);
        })
      );
  }

  /**
   * Refresh authentication token
   */
  refreshToken(): Observable<AuthResult> {
    const refreshToken = this.getRefreshToken();
    if (!refreshToken) {
      this.clearAuth();
      throw new Error('No refresh token available');
    }

    return this.http
      .post<ApiResponse<AuthResult>>(`${this.apiUrl}/RefreshToken`, { refreshToken })
      .pipe(
        map(response => response.data),
        tap((result) => {
          if (result.success && result.token) {
            this.setTokens(result.token, result.refreshToken);
          } else {
            this.clearAuth();
          }
        }),
        catchError((error) => {
          this.clearAuth();
          return this.handleError(error);
        })
      );
  }

  /**
   * Logic điều phối Refresh Token để tránh spam API
   */
  refreshTokenWithQueue(): Observable<string> {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      return this.refreshToken().pipe(
        map(result => {
          this.isRefreshing = false;
          this.refreshTokenSubject.next(result.token);
          return result.token;
        }),
        catchError(err => {
          this.isRefreshing = false;
          this.logout();
          return throwError(() => err);
        })
      );
    } else {
      // Đợi cho đến khi có token mới
      return this.refreshTokenSubject.pipe(
        filter(token => token !== null),
        take(1)
      );
    }
  }

  /**
   * Get current user profile
   */
  getProfile(): Observable<UserProfile> {
    return this.http
      .get<ApiResponse<UserProfile>>(`${this.apiUrl}/GetMe`)
      .pipe(
        map(response => response.data),
        catchError(error => this.handleError(error))
      );
  }

  /**
   * Get public info
   */
  getPublicInfo(): Observable<{ message: string }> {
    return this.http
      .get<ApiResponse<{ message: string }>>(`${this.apiUrl}/GetInfo`)
      .pipe(
        map(response => response.data),
        catchError(error => this.handleError(error))
      );
  }

  /**
   * Update authenticated user's profile
   */
  updateProfile(dto: UpdateProfileDto): Observable<UserProfile> {
    return this.http
      .put<ApiResponse<UserProfile>>(`${this.apiUrl}/UpdateProfile`, dto)
      .pipe(
        map(response => response.data),
        tap(user => this.currentUserSubject.next(user)),
        catchError(error => this.handleError(error))
      );
  }

  /**
   * Change authenticated user's password
   */
  changePassword(dto: ChangePasswordDto): Observable<ChangePasswordResult> {
    return this.http
      .post<ApiResponse<ChangePasswordResult>>(`${this.apiUrl}/ChangePassword`, dto)
      .pipe(
        map(response => response.data),
        catchError(error => this.handleError(error))
      );
  }

  /**
   * Request a password reset token
   */
  forgotPassword(dto: ForgotPasswordDto): Observable<ForgotPasswordResult> {
    return this.http
      .post<ApiResponse<ForgotPasswordResult>>(`${this.apiUrl}/ForgotPassword`, dto)
      .pipe(
        map(response => response.data),
        catchError(error => this.handleError(error))
      );
  }

  /**
   * Reset password using token
   */
  resetPassword(dto: ResetPasswordDto): Observable<AuthResult> {
    return this.http
      .post<ApiResponse<AuthResult>>(`${this.apiUrl}/ResetPassword`, dto)
      .pipe(
        map(response => response.data),
        catchError(error => this.handleError(error))
      );
  }

  /**
   * Load and cache current user profile
   */
  loadCurrentUser(): Observable<UserProfile> {
    return this.getProfile().pipe(
      tap((user) => {
        this.currentUserSubject.next(user);
      }),
      catchError((error) => {
        console.error('Failed to load user profile:', error);
        throw error;
      })
    );
  }

  /**
   * Get access token
   */
  getToken(): string | null {
    return this.cookieService.getCookie(this.TOKEN_KEY);
  }

  /**
   * Get refresh token
   */
  getRefreshToken(): string | null {
    return this.cookieService.getCookie(this.REFRESH_TOKEN_KEY);
  }

  /**
   * Check if stored token is still valid
   */
  private isTokenValid(): boolean {
    const token = this.getToken();
    if (!token) return false;

    try {
      const payload = this.parseJwt(token);
      const expiresIn = payload.exp * 1000;
      return expiresIn > Date.now();
    } catch {
      return false;
    }
  }

  /**
   * Parse JWT token
   */
  private parseJwt(token: string): any {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    );
    return JSON.parse(jsonPayload);
  }

  /**
   * Store tokens in cookies
   */
  private setTokens(token: string, refreshToken: string): void {
    this.cookieService.setCookie(this.TOKEN_KEY, token);
    this.cookieService.setCookie(this.REFRESH_TOKEN_KEY, refreshToken);
  }

  /**
   * Clear authentication state
   */
  private clearAuth(): void {
    this.cookieService.deleteCookie(this.TOKEN_KEY);
    this.cookieService.deleteCookie(this.REFRESH_TOKEN_KEY);
    this.currentUserSubject.next(null);
    this.isAuthenticatedSubject.next(false);
  }

  /**
   * Handle HTTP errors
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
