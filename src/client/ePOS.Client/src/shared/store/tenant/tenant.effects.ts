import { Actions, createEffect, ofType } from '@ngrx/effects';
import { NotificationService, TenantService } from '@app-shared/services';
import {
  getTenant,
  getTenantFailed,
  getTenantSuccess,
  updateTenant,
  updateTenantFailed,
  updateTenantSuccess,
} from '@app-shared/store/tenant/tenant.actions';
import { catchError, map, of, switchMap, tap } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable()
export class TenantEffects {
  constructor(
    private actions$: Actions,
    private tenantService: TenantService,
    private notificationService: NotificationService,
  ) {}

  getTenant$ = createEffect(() =>
    this.actions$.pipe(
      ofType(getTenant),
      switchMap(() =>
        this.tenantService.getTenant().pipe(
          map((data) => getTenantSuccess({ data: data })),
          catchError((err) => of(getTenantFailed({ error: err }))),
        ),
      ),
    ),
  );

  getTenantFailed$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(getTenantFailed),
        tap(() =>
          this.notificationService.error('Lấy dữ liệu thương hiệu thất bại'),
        ),
      ),
    { dispatch: false },
  );

  updateTenant$ = createEffect(() =>
    this.actions$.pipe(
      ofType(updateTenant),
      switchMap(({ payload }) => {
        return this.tenantService.updateTenant(payload).pipe(
          map((data) => updateTenantSuccess({ data: data })),
          catchError((err) => of(updateTenantFailed({ error: err }))),
        );
      }),
    ),
  );

  updateTenantSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(updateTenantSuccess),
        tap(() =>
          this.notificationService.success('Cập nhật thương hiệu thành công'),
        ),
      ),
    { dispatch: false },
  );

  updateTenantFailed$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(updateTenantFailed),
        tap(({ error }) => {
          const message = error.error.message;
          if (message === 'DuplicateTenantName') {
            this.notificationService.error('Mã thương hiệu đã được sử dụng');
          } else {
            this.notificationService.error('Cập nhật thương hiệu thất bại');
          }
        }),
      ),
    { dispatch: false },
  );
}
