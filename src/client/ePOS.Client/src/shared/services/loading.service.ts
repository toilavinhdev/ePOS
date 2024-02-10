import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LoadingService {
  private _subject$ = new Subject<boolean>();

  constructor() {}

  getObservable() {
    return this._subject$;
  }

  show() {
    this._subject$.next(true);
  }

  hide() {
    this._subject$.next(false);
  }
}
