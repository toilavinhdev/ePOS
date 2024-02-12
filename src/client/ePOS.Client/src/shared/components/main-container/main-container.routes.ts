import { Routes } from '@angular/router';
import { MainContainerComponent } from '@app-shared/components';

export const routes: Routes = [
  {
    path: '',
    component: MainContainerComponent,
    children: [
      {
        path: '',
        redirectTo: 'report',
        pathMatch: 'full',
      },
      {
        path: 'profile',
        loadChildren: () =>
          import('@app-features/profile/profile.routes').then((r) => r.routes),
      },
      {
        path: 'report',
        loadChildren: () =>
          import('@app-features/report/report.routes').then((r) => r.routes),
      },
      {
        path: 'management',
        loadChildren: () =>
          import('@app-features/management/management.routes').then(
            (r) => r.routes,
          ),
      },
      {
        path: 'library',
        loadChildren: () =>
          import('@app-features/library/library.routes').then((r) => r.routes),
      },
      {
        path: 'inventory',
        loadChildren: () =>
          import('@app-features/inventory/inventory.routes').then(
            (r) => r.routes,
          ),
      },
    ],
  },
];
