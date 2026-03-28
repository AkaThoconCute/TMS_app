import { Routes } from '@angular/router';

export const DRIVER_ROUTES: Routes = [
  {
    path: '',
    children: [
      {
        path: 'list',
        loadComponent: () => import('./pages/driver-list/driver-list.page').then(m => m.DriverListPage),
      },
      {
        path: 'salary',
        loadComponent: () => import('./pages/driver-salary/driver-salary.page').then(m => m.DriverSalaryPage),
      }
    ]
  }
];
