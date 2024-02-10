import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {
  NotificationComponent,
  SpinnerOverlayComponent,
  SvgDefinitionsComponent,
} from '@app-shared/components';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    SpinnerOverlayComponent,
    NotificationComponent,
    SvgDefinitionsComponent,
  ],
  templateUrl: './app.component.html',
})
export class AppComponent {}
