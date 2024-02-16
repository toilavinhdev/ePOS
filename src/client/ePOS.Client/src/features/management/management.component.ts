import { Component } from '@angular/core';
import {
  IMenuItem,
  MenuComponent,
  TabMenuComponent,
} from '@app-shared/components';
import { RouterOutlet } from '@angular/router';

const menuItems: IMenuItem[] = [
  {
    url: '/management',
    title: 'Cài đặt',
  },
];

@Component({
  selector: 'app-management',
  standalone: true,
  imports: [TabMenuComponent, MenuComponent, RouterOutlet],
  templateUrl: './management.component.html',
  styles: ``,
})
export class ManagementComponent {
  protected readonly menuItems = menuItems;
}
