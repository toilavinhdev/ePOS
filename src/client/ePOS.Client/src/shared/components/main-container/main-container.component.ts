import { Component, ViewChild } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SidebarComponent, UserAvatarComponent } from '@app-shared/components';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-main-container',
  standalone: true,
  imports: [RouterOutlet, SidebarComponent, NgIf, UserAvatarComponent],
  templateUrl: './main-container.component.html',
  styles: ``,
})
export class MainContainerComponent {
  @ViewChild('sidebar', { static: true }) sidebarComponent!: SidebarComponent;

  constructor() {}
}
