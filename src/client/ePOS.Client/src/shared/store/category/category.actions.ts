import { createAction, props } from '@ngrx/store';
import {
  ICategoryViewModel,
  ICreateCategoryRequest,
  IListCategoryRequest,
  IListCategoryResponse,
} from '@app-shared/models/category.models';
import { HttpErrorResponse } from '@angular/common/http';

export const listCategory = createAction(
  '[Category] List',
  props<{ payload: IListCategoryRequest }>(),
);

export const listCategorySuccess = createAction(
  '[Category] List Success',
  props<{ response: IListCategoryResponse }>(),
);

export const listCategoryFailed = createAction(
  '[Category] List Failed',
  props<{ error: HttpErrorResponse }>(),
);

export const createCategory = createAction(
  '[Category] Create',
  props<{ payload: ICreateCategoryRequest }>(),
);

export const createCategorySuccess = createAction(
  '[Category] Create Success',
  props<{ data: ICategoryViewModel }>(),
);

export const createCategoryFailed = createAction(
  '[Category] Create Failed',
  props<{ error: HttpErrorResponse }>(),
);

export const deleteCategory = createAction(
  '[Category] Delete',
  props<{ ids: string[] }>(),
);

export const deleteCategorySuccess = createAction('[Category] Delete Success');

export const deleteCategoryFailed = createAction(
  '[Category] Delete Failed',
  props<{ error: HttpErrorResponse }>(),
);
