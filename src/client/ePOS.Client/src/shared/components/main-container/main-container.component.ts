import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { SidebarComponent, UserAvatarComponent } from '@app-shared/components';
import { NgIf } from '@angular/common';
import { BaseComponent } from '@app-shared/core/abtractions';
import { Store } from '@ngrx/store';
import { getTenant } from '@app-shared/store/tenant';
import { getMe } from '@app-shared/store/user';

@Component({
  selector: 'app-main-container',
  standalone: true,
  imports: [RouterOutlet, SidebarComponent, NgIf, UserAvatarComponent],
  templateUrl: './main-container.component.html',
  styles: ``,
})
export class MainContainerComponent extends BaseComponent implements OnInit {
  @ViewChild('sidebar', { static: true }) sidebarComponent!: SidebarComponent;

  constructor(
    private store: Store,
    private router: Router,
  ) {
    super();
  }

  ngOnInit() {
    this.store.dispatch(getMe());
    this.store.dispatch(getTenant());
  }

  isPosMode() {
    return this.router.url.includes('/pos');
  }
}
