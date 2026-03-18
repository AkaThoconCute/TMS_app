import { Routes } from '@angular/router';

export const TRUCK_ROUTES: Routes = [
  {
    path: '',
    children: [
      {
        path: 'list',
        loadComponent: () => import('./pages/truck-list/truck-list.page').then(m => m.TruckListPage),
      },
      {
        path: 'maintenance',
        loadComponent: () => import('./pages/truck-maintenance/truck-maintenance.page').then(m => m.TruckMaintenancePage),
      }
    ]
  }
];