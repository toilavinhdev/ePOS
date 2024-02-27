import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  checkout,
  checkoutFailed,
  checkoutSuccess,
} from '@app-shared/store/order/order.actions';
import { catchError, map, of, switchMap, take, tap } from 'rxjs';
import {
  LoadingService,
  NotificationService,
  OrderService,
} from '@app-shared/services';
import { Store } from '@ngrx/store';
import { orderLinesSelector } from '@app-shared/store/order/order.selector';
import {
  ICreateOrderItemRequest,
  ICreateOrderRequest,
} from '@app-shared/models/order.models';

@Injectable()
export class OrderEffects {
  constructor(
    private actions$: Actions,
    private store: Store,
    private orderService: OrderService,
    private notificationService: NotificationService,
    private loadingService: LoadingService,
  ) {}

  checkout$ = createEffect(() =>
    this.actions$.pipe(
      ofType(checkout),
      tap(() => this.loadingService.show()),
      switchMap(({ payload }) => {
        return this.orderService.checkout(payload).pipe(
          map((data) => checkoutSuccess({ data })),
          catchError((err) => of(checkoutFailed({ error: err }))),
        );
      }),
      tap(() => this.loadingService.hide()),
    ),
  );

  checkoutSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(checkoutSuccess),
        tap(() => this.notificationService.success('Thành công')),
      ),
    { dispatch: false },
  );

  checkoutFailed$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(checkoutFailed),
        tap(() => this.notificationService.error('Thất bại')),
      ),
    { dispatch: false },
  );
}
