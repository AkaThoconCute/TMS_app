import { Routes } from '@angular/router';
import { notAuthGuard, authGuard } from '../../platform/auth/auth.guard';

export const ACCOUNT_ROUTES: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./pages/login/login.page').then(m => m.LoginPage),
    canActivate: [notAuthGuard]
  },
  {
    path: 'register',
    loadComponent: () => import('./pages/register/register.page').then(m => m.RegisterPage),
    canActivate: [notAuthGuard]
  },
  {
    path: 'reset-password',
    loadComponent: () => import('./pages/reset-password/reset-password.page').then(m => m.ResetPasswordPage),
    canActivate: [notAuthGuard]
  }
];
