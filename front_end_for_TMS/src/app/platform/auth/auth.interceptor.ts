import { HttpInterceptorFn, HttpErrorResponse, HttpRequest, HttpHandlerFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from './auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);

  const addTokenToRequest = (request: HttpRequest<unknown>, token: string) => {
    return request.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
  };

  const refreshAndRetryRequest = (failedRequest: HttpRequest<unknown>, next: HttpHandlerFn) => {
    return authService.refreshTokenWithQueue().pipe(
      switchMap(newToken => next(addTokenToRequest(failedRequest, newToken)))
    );
  };

  const token = authService.getToken();
  if (token) {
    req = addTokenToRequest(req, token);
  }

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && !req.url.includes('RefreshToken')) {
        return refreshAndRetryRequest(req, next);
      }
      return throwError(() => error);
    })
  );
};