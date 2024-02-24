import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { ItemService, NotificationService } from '@app-shared/services';
import {
  createItem,
  createItemFailed,
  createItemSuccess,
  deleteItem,
  deleteItemFailed,
  deleteItemSuccess,
  listItem,
  listItemFailed,
  listItemSuccess,
} from '@app-shared/store/item/item.actions';
import { catchError, map, of, switchMap, tap } from 'rxjs';

@Injectable()
export class ItemEffects {
  constructor(
    private actions$: Actions,
    private itemService: ItemService,
    private notificationService: NotificationService,
  ) {}

  listItem$ = createEffect(() =>
    this.actions$.pipe(
      ofType(listItem),
      switchMap(({ payload }) =>
        this.itemService.list(payload).pipe(
          map((response) => listItemSuccess({ data: response })),
          catchError((err) => of(listItemFailed({ error: err }))),
        ),
      ),
    ),
  );

  listItemFailed$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(listItemFailed),
        tap(() =>
          this.notificationService.error('Lấy danh sách món ăn thất bại'),
        ),
      ),
    { dispatch: false },
  );

  createItem$ = createEffect(() =>
    this.actions$.pipe(
      ofType(createItem),
      switchMap(({ payload }) =>
        this.itemService.create(payload).pipe(
          map((data) => createItemSuccess({ data: data })),
          catchError((err) => of(createItemFailed({ error: err }))),
        ),
      ),
    ),
  );

  createItemSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(createItemSuccess),
        tap(() => this.notificationService.success('Tạo món ăn thành công')),
      ),
    { dispatch: false },
  );

  createItemFailed$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(createItemFailed),
        tap(({ error }) => {
          const message = error.error.message;
          if (message === 'DuplicateSku') {
            this.notificationService.warning('SKU đã được sử dụng');
          } else {
            this.notificationService.error('Tạo món ăn thất bại');
          }
        }),
      ),
    { dispatch: false },
  );

  deleteItem$ = createEffect(() =>
    this.actions$.pipe(
      ofType(deleteItem),
      switchMap(({ ids }) =>
        this.itemService.delete(ids).pipe(
          map((data) => deleteItemSuccess()),
          catchError((err) => of(deleteItemFailed({ error: err }))),
        ),
      ),
    ),
  );

  deleteItemSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(deleteItemSuccess),
        tap(() => this.notificationService.success('Đã xóa món ăn')),
      ),
    { dispatch: false },
  );

  deleteItemFailed$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(deleteItemFailed),
        tap(({ error }) => {
          this.notificationService.error('Xóa món ăn thất bại');
        }),
      ),
    { dispatch: false },
  );
}
