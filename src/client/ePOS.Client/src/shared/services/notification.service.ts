import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';

export interface INotification {
  status: string;
  message: string;
  duration?: number;
}

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private _subject = new Subject<INotification>();

  constructor() {}

  getObservable(): Observable<INotification> {
    return this._subject.asObservable();
  }

  success(message: string) {
    this.show('success', message);
  }

  info(message: string) {
    this.show('info', message);
  }

  warning(message: string) {
    this.show('warn', message);
  }

  error(message: string) {
    this.show('error', message);
  }

  private show(status: string, message: string) {
    this._subject.next({
      status: status,
      message: message,
    } as INotification);
  }
}
