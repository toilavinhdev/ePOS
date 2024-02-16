import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { NotificationService, UnitService } from '@app-shared/services';
import {
  createUnit,
  createUnitFailed,
  createUnitSuccess,
  listUnit,
  listUnitFailed,
  listUnitSuccess,
  updateUnit,
  updateUnitFailed,
  updateUnitSuccess,
} from '@app-shared/store/unit/unit.actions';
import { catchError, map, of, switchMap, tap } from 'rxjs';

@Injectable()
export class UnitEffects {
  constructor(
    private actions$: Actions,
    private unitService: UnitService,
    private notificationService: NotificationService,
  ) {}

  listUnit$ = createEffect(() =>
    this.actions$.pipe(
      ofType(listUnit),
      switchMap(({ payload }) =>
        this.unitService.list(payload).pipe(
          map((response) => listUnitSuccess({ data: response })),
          catchError((err) => of(listUnitFailed({ error: err }))),
        ),
      ),
    ),
  );

  listUnitFailed$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(listUnitFailed),
        tap(() =>
          this.notificationService.error('Lấy danh sách đơn vị thất bại'),
        ),
      ),
    { dispatch: false },
  );

  createUnit$ = createEffect(() =>
    this.actions$.pipe(
      ofType(createUnit),
      switchMap(({ payload }) =>
        this.unitService.create(payload).pipe(
          map((data) => createUnitSuccess({ data: data })),
          catchError((err) => of(createUnitFailed({ error: err }))),
        ),
      ),
    ),
  );

  createUnitSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(createUnitSuccess),
        tap(() => this.notificationService.success('Tạo đơn vị thành công')),
      ),
    { dispatch: false },
  );

  createUnitFailed$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(createUnitFailed),
        tap(() => this.notificationService.error('Tạo đơn vị thất bại')),
      ),
    { dispatch: false },
  );

  updateUnit$ = createEffect(() =>
    this.actions$.pipe(
      ofType(updateUnit),
      switchMap(({ payload }) =>
        this.unitService.update(payload).pipe(
          map((data) => updateUnitSuccess({ data: data })),
          catchError((err) => of(updateUnitFailed({ error: err }))),
        ),
      ),
    ),
  );

  updateUnitSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(updateUnitSuccess),
        tap(() =>
          this.notificationService.success('Cập nhật đơn vị thành công'),
        ),
      ),
    { dispatch: false },
  );

  updateUnitFailed$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(updateUnitFailed),
        tap(() => this.notificationService.error('Cập nhật đơn vị thất bại')),
      ),
    { dispatch: false },
  );
}
