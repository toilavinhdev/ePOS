import { Component, OnInit } from '@angular/core';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { takeUntil } from 'rxjs';
import { BaseComponent } from '@app-shared/core/abtractions';
import { NotificationService } from '@app-shared/services';

@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [ToastModule],
  templateUrl: './notification.component.html',
  providers: [MessageService],
})
export class NotificationComponent extends BaseComponent implements OnInit {
  constructor(
    private _notificationService: NotificationService,
    private _messageService: MessageService,
  ) {
    super();
  }

  ngOnInit() {
    this._notificationService
      .getObservable()
      .pipe(takeUntil(this.destroy$))
      .subscribe((data) => {
        this._messageService.add({
          severity: data.status,
          detail: data.message,
        });
      });
  }
}
