import { ManagementComponent } from '@app-features/management/management.component';
import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    component: ManagementComponent,
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./setting/setting.component').then((c) => c.SettingComponent),
      },
    ],
  },
];
