import { Injectable, inject } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { AccountService } from './account.service';
import { AuthResult, UserProfile } from '../models/auth.models';

/**
 * Authentication Service
 * Manages authentication state, tokens, and user information
 */
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly accountService = inject(AccountService);

  private readonly TOKEN_KEY = 'token';
  private readonly REFRESH_TOKEN_KEY = 'refreshToken';

  // Observable state
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.isTokenValid());
  private currentUserSubject = new BehaviorSubject<UserProfile | null>(null);

  // Public observables
  isAuthenticated$ = this.isAuthenticatedSubject.asObservable();
  currentUser$ = this.currentUserSubject.asObservable();

  constructor() {
    this.checkAuthStatus();
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
   * Login user
   */
  login(email: string, password: string): Observable<AuthResult> {
    return this.accountService.login({ email, password }).pipe(
      tap((result) => {
        if (result.success && result.token) {
          this.setTokens(result.token, result.refreshToken);
          this.isAuthenticatedSubject.next(true);
          this.loadCurrentUser();
        }
      }),
      catchError((error) => {
        this.clearAuth();
        throw error;
      })
    );
  }

  /**
   * Register new user
   */
  register(fullName: string, email: string, password: string): Observable<AuthResult> {
    return this.accountService.register({ fullName, email, password }).pipe(
      tap((result) => {
        if (result.success && result.token) {
          this.setTokens(result.token, result.refreshToken);
          this.isAuthenticatedSubject.next(true);
          this.loadCurrentUser();
        }
      }),
      catchError((error) => {
        this.clearAuth();
        throw error;
      })
    );
  }

  /**
   * Logout user
   */
  logout(): void {
    this.accountService.logout().subscribe({
      next: () => this.clearAuth(),
      error: () => this.clearAuth(),
      complete: () => { }
    });
    // Clear auth immediately
    this.clearAuth();
  }

  /**
   * Refresh authentication token
   */
  refreshAuthToken(): Observable<AuthResult> {
    const refreshToken = this.getRefreshToken();
    if (!refreshToken) {
      this.clearAuth();
      throw new Error('No refresh token available');
    }

    return this.accountService.refreshToken({ refreshToken }).pipe(
      tap((result) => {
        if (result.success && result.token) {
          this.setTokens(result.token, result.refreshToken);
        } else {
          this.clearAuth();
        }
      }),
      catchError((error) => {
        this.clearAuth();
        throw error;
      })
    );
  }

  /**
   * Load and cache current user profile
   */
  loadCurrentUser(): Observable<UserProfile> {
    return this.accountService.getProfile().pipe(
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
    return localStorage.getItem(this.TOKEN_KEY);
  }

  /**
   * Get refresh token
   */
  getRefreshToken(): string | null {
    return localStorage.getItem(this.REFRESH_TOKEN_KEY);
  }

  /**
   * Store tokens
   */
  private setTokens(token: string, refreshToken: string): void {
    localStorage.setItem(this.TOKEN_KEY, token);
    localStorage.setItem(this.REFRESH_TOKEN_KEY, refreshToken);
  }

  /**
   * Clear authentication state
   */
  private clearAuth(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.REFRESH_TOKEN_KEY);
    this.currentUserSubject.next(null);
    this.isAuthenticatedSubject.next(false);
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
}
