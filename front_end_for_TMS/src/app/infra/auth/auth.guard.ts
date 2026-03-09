import { Injectable, inject } from '@angular/core';
import { Router, CanActivateFn, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../../features/account/services/auth.service';

/**
 * Functional guard to protect routes that require authentication
 * Redirects to login if user is not authenticated
 */
export const authGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isAuthenticated) {
    return true;
  }

  // Store the attempted URL for use after login
  sessionStorage.setItem('returnUrl', state.url);

  // Redirect to login
  router.navigate(['/account/login']);
  return false;
};

/**
 * Guard to protect routes from authenticated users
 * Redirects to home if user is already logged in
 */
export const notAuthGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (!authService.isAuthenticated) {
    return true;
  }

  // Already authenticated, redirect to home
  router.navigate(['/']);
  return false;
};
