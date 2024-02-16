import { Component } from '@angular/core';
import { IMenuItem, MenuComponent } from '@app-shared/components';
import { RouterOutlet } from '@angular/router';

const menuItems: IMenuItem[] = [
  {
    title: 'Món ăn',
    url: '/library',
  },
  {
    title: 'Danh mục',
    url: '/library/category',
  },
];

@Component({
  selector: 'app-library',
  standalone: true,
  imports: [MenuComponent, RouterOutlet],
  templateUrl: './library.component.html',
  styles: ``,
})
export class LibraryComponent {
  protected readonly menuItems = menuItems;
}
