import { Routes } from '@angular/router';
import { authGuard, notAuthGuard } from '@platform/auth/auth.guard';
import { PublicLayout } from './shell/public-layout/public-layout';
import { PrivateLayout } from './shell/private-layout/private-layout';

export const appRoutes: Routes = [
  {
    path: '',
    component: PublicLayout,
    canActivateChild: [notAuthGuard],
    children: [
      {
        path: 'button-demo',
        loadComponent: () => import('./common/button/button-demo.component').then(m => m.ButtonDemo)
      },
      {
        path: '',
        loadChildren: () => import('./features/home/home.routes').then(m => m.HOME_ROUTES)
      },
      {
        path: '',
        loadChildren: () => import('./features/account/account.routes').then(m => m.ACCOUNT_ROUTES)
      },
    ]
  },
  {
    path: '',
    component: PrivateLayout,
    canActivateChild: [authGuard],
    children: [
      {
        path: 'home',
        loadChildren: () => import('./features/home/home.routes').then(m => m.HOME_ROUTES)
      },
      {
        path: 'trucks',
        loadChildren: () => import('./features/truck/truck.routes').then(m => m.TRUCK_ROUTES)
      },
      {
        path: 'drivers',
        loadChildren: () => import('./features/driver/driver.routes').then(m => m.DRIVER_ROUTES)
      },
      {
        path: 'demo/button',
        loadComponent: () => import('./common/button/button-demo.component').then(m => m.ButtonDemo)
      },
      {
        path: 'demo/upcoming',
        loadComponent: () => import('./common/development/development.page').then(m => m.DevelopmentPage)
      },
      {
        path: 'under-development',
        loadComponent: () => import('./common/development/development.page').then(m => m.DevelopmentPage)
      },
      {
        path: '**',
        redirectTo: 'under-development'
      },
    ]
  }
];
