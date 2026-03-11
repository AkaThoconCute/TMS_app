import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors, withXsrfConfiguration } from '@angular/common/http';
import { routes } from './app.routes';
import { authInterceptor } from './platform/auth/auth.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(
      // Add auth interceptor using function-based approach
      withInterceptors([authInterceptor]),
      // Configure XSRF protection
      // withXsrfConfiguration({
      //   cookieName: 'X-CSRF-TOKEN',
      //   headerName: 'X-CSRF-TOKEN'
      // })
    )
  ]
};
