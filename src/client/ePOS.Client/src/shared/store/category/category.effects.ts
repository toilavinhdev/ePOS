import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { CategoryService, NotificationService } from '@app-shared/services';
import {
  createCategory,
  createCategoryFailed,
  createCategorySuccess,
  deleteCategory,
  deleteCategoryFailed,
  deleteCategorySuccess,
  listCategory,
  listCategoryFailed,
  listCategorySuccess,
} from '@app-shared/store/category/category.actions';
import { catchError, map, of, switchMap, tap } from 'rxjs';

@Injectable()
export class CategoryEffects {
  constructor(
    private actions$: Actions,
    private categoryService: CategoryService,
    private notificationService: NotificationService,
  ) {}

  listCategory$ = createEffect(() =>
    this.actions$.pipe(
      ofType(listCategory),
      switchMap(({ payload }) =>
        this.categoryService.list(payload).pipe(
          map((response) => listCategorySuccess({ response: response })),
          catchError((err) => of(listCategoryFailed({ error: err }))),
        ),
      ),
    ),
  );

  listCategoryFailed$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(listCategoryFailed),
        tap(() =>
          this.notificationService.error('Lấy danh sách danh mục thất bại'),
        ),
      ),
    { dispatch: false },
  );

  createCategory$ = createEffect(() =>
    this.actions$.pipe(
      ofType(createCategory),
      switchMap(({ payload }) =>
        this.categoryService.create(payload).pipe(
          map((response) => createCategorySuccess({ data: response })),
          catchError((err) => of(createCategoryFailed({ error: err }))),
        ),
      ),
    ),
  );

  createCategorySuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(createCategorySuccess),
        tap(() => this.notificationService.success('Tạo danh mục thành công')),
      ),
    { dispatch: false },
  );

  createCategoryFailed$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(createCategoryFailed),
        tap(() => this.notificationService.error('Tạo danh mục thất bại')),
      ),
    { dispatch: false },
  );

  deleteCategory$ = createEffect(() =>
    this.actions$.pipe(
      ofType(deleteCategory),
      switchMap(({ ids }) =>
        this.categoryService.delete(ids).pipe(
          map(() => deleteCategorySuccess()),
          catchError((err) => of(deleteCategoryFailed({ error: err }))),
        ),
      ),
    ),
  );

  deleteCategorySuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(deleteCategorySuccess),
        tap(() => this.notificationService.success('Đã xóa danh mục')),
      ),
    { dispatch: false },
  );

  deleteCategoryFailed$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(deleteCategoryFailed),
        tap(() => this.notificationService.error('Xóa danh mục thất bại')),
      ),
    { dispatch: false },
  );
}
