import { createAction, props } from '@ngrx/store';
import {
  ICreateUnitRequest,
  IListUnitRequest,
  IListUnitResponse,
  IUnitViewModel,
  IUpdateUnitRequest,
} from '@app-shared/models/unit.models';
import { HttpErrorResponse } from '@angular/common/http';

export const listUnit = createAction(
  '[Unit] List',
  props<{ payload: IListUnitRequest }>(),
);

export const listUnitSuccess = createAction(
  '[Unit] List Success',
  props<{ data: IListUnitResponse }>(),
);

export const listUnitFailed = createAction(
  '[Unit] List Failed',
  props<{ error: HttpErrorResponse }>(),
);

export const createUnit = createAction(
  '[Unit] Create',
  props<{ payload: ICreateUnitRequest }>(),
);

export const createUnitSuccess = createAction(
  '[Unit] Create Success',
  props<{ data: IUnitViewModel }>(),
);

export const createUnitFailed = createAction(
  '[Unit] Create Failed',
  props<{ error: string }>(),
);

export const updateUnit = createAction(
  '[Unit] Update',
  props<{ payload: IUpdateUnitRequest }>(),
);

export const updateUnitSuccess = createAction(
  '[Unit] Update Success',
  props<{ data: IUnitViewModel }>(),
);

export const updateUnitFailed = createAction(
  '[Unit] Update Failed',
  props<{ error: string }>(),
);
