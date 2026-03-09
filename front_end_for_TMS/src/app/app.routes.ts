import { Routes } from '@angular/router';
import { LayoutComponent } from './shell/layout/layout.compo';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      {
        path: '',
        loadChildren: () => import('./features/home/home.routes').then(m => m.HOME_ROUTES)
      },
      {
        path: '',
        loadChildren: () => import('./features/account/account.routes').then(m => m.ACCOUNT_ROUTES)
      }
    ]
  }
];
