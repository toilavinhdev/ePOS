import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadChildren: () =>
      import(
        '@app-shared/components/main-container/main-container.routes'
      ).then((r) => r.routes),
  },
  {
    path: 'auth',
    loadChildren: () =>
      import('@app-features/auth/auth.routes').then((r) => r.routes),
  },
];
