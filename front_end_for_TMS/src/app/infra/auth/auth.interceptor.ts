import { Injectable, inject } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { AuthService } from '../../features/account/services/auth.service';

/**
 * HTTP Interceptor for authentication
 * Adds access token to outgoing requests and handles token refresh
 */
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private readonly authService = inject(AuthService);
  private isRefreshing = false;

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    // Add token to request if available
    const token = this.authService.getToken();
    if (token) {
      req = this.addTokenToRequest(req, token);
    }

    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        // If unauthorized and not already refreshing, try to refresh token
        if (error.status === 401 && !this.isRefreshing) {
          return this.handleTokenRefresh(req, next);
        }
        return throwError(() => error);
      })
    );
  }

  /**
   * Add JWT token to request headers
   */
  private addTokenToRequest(
    req: HttpRequest<any>,
    token: string
  ): HttpRequest<any> {
    return req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  /**
   * Handle 401 response by refreshing token and retrying request
   */
  private handleTokenRefresh(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    this.isRefreshing = true;

    return this.authService.refreshAuthToken().pipe(
      switchMap((result) => {
        this.isRefreshing = false;
        const newToken = result.token;

        // Retry original request with new token
        return next.handle(this.addTokenToRequest(req, newToken));
      }),
      catchError((error) => {
        this.isRefreshing = false;
        // Token refresh failed, logout user
        this.authService.logout();
        return throwError(() => error);
      })
    );
  }
}
