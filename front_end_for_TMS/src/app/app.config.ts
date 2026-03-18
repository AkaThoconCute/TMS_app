import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { ApplicationConfig, inject, provideAppInitializer, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { providePrimeNG } from 'primeng/config';
import { appPrimeNG } from './app.prime';
import { appRoutes } from './app.routes';
import { AuthService } from '@platform/auth/auth.service';
import { authInterceptor } from '@platform/auth/auth.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(appRoutes),
    provideHttpClient(
      withInterceptors([authInterceptor]),
    ),
    providePrimeNG(appPrimeNG),
    provideAppInitializer(() => {
      const authService = inject(AuthService);
      return authService.initAuth();
    })
  ]
};