import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {
  ConfirmDialogComponent,
  NotificationComponent,
  SpinnerOverlayComponent,
  SvgDefinitionsComponent,
} from '@app-shared/components';
import { PrimeNGConfig } from 'primeng/api';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    SpinnerOverlayComponent,
    NotificationComponent,
    SvgDefinitionsComponent,
    ConfirmDialogComponent,
  ],
  templateUrl: './app.component.html',
})
export class AppComponent implements OnInit {
  constructor(private primengConfig: PrimeNGConfig) {}

  ngOnInit() {
    this.primengConfig.ripple = true;
  }
}
