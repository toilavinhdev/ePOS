import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

export interface IConfirmModel {
  header?: string;
  message?: string;
  accept?: Function;
  reject?: Function;
}

@Injectable({
  providedIn: 'root',
})
export class ConfirmService {
  subject$ = new Subject<IConfirmModel>();

  show(model: IConfirmModel) {
    if (!model.header) model.header = 'Xác nhận';
    this.subject$.next(model);
  }
}
