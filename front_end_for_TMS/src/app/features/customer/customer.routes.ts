import { Routes } from '@angular/router';

export const CUSTOMER_ROUTES: Routes = [
  {
    path: '',
    children: [
      {
        path: 'list',
        loadComponent: () => import('./pages/customer-list/customer-list.page').then(m => m.CustomerListPage),
      }
    ]
  }
];
