import { Component, OnInit } from '@angular/core';
import { NgIf } from '@angular/common';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { BaseComponent } from '@app-shared/core/abtractions/base.component';
import { LoadingService } from '@app-shared/services/loading.service';
import { takeUntil } from 'rxjs';

@Component({
  selector: 'app-spinner-overlay',
  standalone: true,
  imports: [NgIf, ProgressSpinnerModule],
  templateUrl: './spinner-overlay.component.html',
})
export class SpinnerOverlayComponent extends BaseComponent implements OnInit {
  progress = false;

  constructor(private _loadingService: LoadingService) {
    super();
  }

  ngOnInit() {
    this._loadingService
      .getObservable()
      .pipe(takeUntil(this.destroy$))
      .subscribe((state) => (this.progress = state));
  }
}
