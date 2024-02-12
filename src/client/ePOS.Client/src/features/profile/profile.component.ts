import { Component } from '@angular/core';
import { IMenuItem, MenuComponent } from '@app-shared/components';
import { RouterOutlet } from '@angular/router';

const menuItems: IMenuItem[] = [
  {
    title: 'Thông tin',
    url: '/profile',
  },
];

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [MenuComponent, RouterOutlet],
  templateUrl: './profile.component.html',
  styles: ``,
})
export class ProfileComponent {
  protected readonly menuItems = menuItems;
}
