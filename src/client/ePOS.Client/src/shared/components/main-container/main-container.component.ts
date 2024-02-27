import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { SidebarComponent, UserAvatarComponent } from '@app-shared/components';
import { AsyncPipe, NgIf } from '@angular/common';
import { BaseComponent } from '@app-shared/core/abtractions';
import { Store } from '@ngrx/store';
import { getTenant, tenantSelector } from '@app-shared/store/tenant';
import { getMe } from '@app-shared/store/user';
import { Observable, takeUntil } from 'rxjs';
import { ITenantViewModel } from '@app-shared/models/tenant.models';

@Component({
  selector: 'app-main-container',
  standalone: true,
  imports: [
    RouterOutlet,
    SidebarComponent,
    NgIf,
    UserAvatarComponent,
    AsyncPipe,
  ],
  templateUrl: './main-container.component.html',
  styles: ``,
})
export class MainContainerComponent extends BaseComponent implements OnInit {
  @ViewChild('sidebar', { static: true }) sidebarComponent!: SidebarComponent;
  tenant$!: Observable<ITenantViewModel | undefined>;

  constructor(
    private store: Store,
    private router: Router,
  ) {
    super();
  }

  ngOnInit() {
    this.store.dispatch(getMe());
    this.store.dispatch(getTenant());
    this.tenant$ = this.store
      .select(tenantSelector)
      .pipe(takeUntil(this.destroy$));
  }

  isPosMode() {
    return this.router.url.includes('/pos');
  }
}
