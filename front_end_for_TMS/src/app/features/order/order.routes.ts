import { Routes } from '@angular/router';

export const ORDER_ROUTES: Routes = [
  {
    path: '',
    children: [
      {
        path: 'list',
        loadComponent: () => import('./pages/order-list/order-list.page').then(m => m.OrderListPage),
      }
    ]
  }
];
