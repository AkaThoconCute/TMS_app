import { Routes } from '@angular/router';
import { LayoutComponent } from './shell/layout/layout.compo';

export const appRoutes: Routes = [
  // {
  //   path: 'button-demo',
  //   loadComponent: () => import('./common/button/button-demo.component').then(m => m.ButtonDemo)
  // },
  {
    path: '',
    component: LayoutComponent,
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
      {
        path: 'trucks',
        loadChildren: () => import('./features/truck/truck.routes').then(m => m.TRUCK_ROUTES)
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
