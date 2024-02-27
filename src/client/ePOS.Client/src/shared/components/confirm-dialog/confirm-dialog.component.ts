import { Component, OnInit } from '@angular/core';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService } from 'primeng/api';
import { ConfirmService, IConfirmModel } from '@app-shared/services';

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  imports: [ConfirmDialogModule],
  templateUrl: './confirm-dialog.component.html',
  providers: [ConfirmationService],
})
export class ConfirmDialogComponent implements OnInit {
  constructor(
    private confirmationService: ConfirmationService,
    private confirmService: ConfirmService,
  ) {}

  ngOnInit() {
    this.confirmService.subject$.subscribe((model) => this.onShowDialog(model));
  }

  onShowDialog(model: IConfirmModel) {
    this.confirmationService.confirm({
      header: model.header,
      message: model.message,
      acceptIcon: '',
      rejectIcon: '',
      acceptButtonStyleClass: 'p-button-outlined',
      accept: model.accept,
      reject: model.reject,
    });
  }
}
