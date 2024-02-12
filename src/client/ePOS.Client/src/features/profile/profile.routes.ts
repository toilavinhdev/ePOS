import { Routes } from '@angular/router';
import { ProfileComponent } from '@app-features/profile/profile.component';

export const routes: Routes = [
  {
    path: '',
    component: ProfileComponent,
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./profile-detail/profile-detail.component').then(
            (c) => c.ProfileDetailComponent,
          ),
      },
    ],
  },
];
