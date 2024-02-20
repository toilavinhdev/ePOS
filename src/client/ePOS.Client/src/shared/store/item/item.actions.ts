import { createAction, props } from '@ngrx/store';
import {
  ICreateItemRequest,
  IItemViewModel,
  IListItemRequest,
  IListItemResponse,
} from '@app-shared/models/item.models';
import { HttpErrorResponse } from '@angular/common/http';

export const listItem = createAction(
  '[Item] List',
  props<{ payload: IListItemRequest }>(),
);

export const listItemSuccess = createAction(
  '[Item] List Success',
  props<{ data: IListItemResponse }>(),
);

export const listItemFailed = createAction(
  '[Item] List Failed',
  props<{ error: HttpErrorResponse }>(),
);

export const createItem = createAction(
  '[Item] Create',
  props<{ payload: ICreateItemRequest }>(),
);

export const createItemSuccess = createAction(
  '[Item] Create Success',
  props<{ data: IItemViewModel }>(),
);

export const createItemFailed = createAction(
  '[Item] Create Failed',
  props<{ error: HttpErrorResponse }>(),
);
