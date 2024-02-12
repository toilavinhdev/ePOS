import { Component, OnInit } from '@angular/core';
import { AvatarModule } from 'primeng/avatar';
import { OverlayModule } from 'primeng/overlay';
import { AsyncPipe, NgIf, NgTemplateOutlet } from '@angular/common';
import { RouterLink } from '@angular/router';
import { RippleModule } from 'primeng/ripple';
import { BaseComponent } from '@app-shared/core/abtractions';
import { Store } from '@ngrx/store';
import { getMe, signOut, userProfileSelector } from '@app-shared/store/user';
import { Observable } from 'rxjs';
import { IGetMeResponse } from '@app-shared/models/user.models';
import { defaultAvatarPath } from '@app-shared/constants';

@Component({
  selector: 'app-user-avatar',
  standalone: true,
  imports: [
    AvatarModule,
    OverlayModule,
    NgTemplateOutlet,
    RouterLink,
    RippleModule,
    AsyncPipe,
    NgIf,
  ],
  templateUrl: './user-avatar.component.html',
  styles: ``,
})
export class UserAvatarComponent extends BaseComponent implements OnInit {
  visible = false;
  protected readonly defaultAvatarPath = defaultAvatarPath;
  profile$!: Observable<IGetMeResponse | undefined>;

  constructor(private store: Store) {
    super();
    this.profile$ = this.store.select(userProfileSelector);
  }

  ngOnInit() {
    this.store.dispatch(getMe());
  }

  onSignOut() {
    this.store.dispatch(signOut());
  }
}
