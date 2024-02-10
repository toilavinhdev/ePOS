import { Routes } from '@angular/router';
import { AuthComponent } from '@app-features/auth/auth.component';

export const routes: Routes = [
  {
    path: '',
    component: AuthComponent,
    children: [
      {
        path: '',
        redirectTo: 'sign-in',
        pathMatch: 'full',
      },
      {
        path: 'sign-in',
        loadComponent: () =>
          import('./sign-in/sign-in.component').then((c) => c.SignInComponent),
      },
      {
        path: 'sign-up',
        loadComponent: () =>
          import('./sign-up/sign-up.component').then((c) => c.SignUpComponent),
      },
    ],
  },
];
