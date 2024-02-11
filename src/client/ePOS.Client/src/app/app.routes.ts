import { Routes } from '@angular/router';
import { authGuard } from '@app-shared/core/guards/auth.guard';
import { signedInGuard } from '@app-shared/core/guards/signed-in.guard';

export const routes: Routes = [
  {
    path: '',
    loadChildren: () =>
      import(
        '@app-shared/components/main-container/main-container.routes'
      ).then((r) => r.routes),
    canActivate: [authGuard],
  },
  {
    path: 'auth',
    loadChildren: () =>
      import('@app-features/auth/auth.routes').then((r) => r.routes),
    canActivate: [signedInGuard],
  },
];
