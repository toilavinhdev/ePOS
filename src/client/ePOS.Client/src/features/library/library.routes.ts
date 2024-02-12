import { LibraryComponent } from '@app-features/library/library.component';
import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    component: LibraryComponent,
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./lib-item/lib-item.component').then(
            (c) => c.LibItemComponent,
          ),
      },
      {
        path: 'category',
        loadComponent: () =>
          import('./lib-category/lib-category.component').then(
            (c) => c.LibCategoryComponent,
          ),
      },
    ],
  },
];
