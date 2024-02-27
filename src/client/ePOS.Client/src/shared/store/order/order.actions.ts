import { createAction, props } from '@ngrx/store';
import {
  ICreateOrderRequest,
  IInvoiceLine,
  IOrderViewModel,
} from '@app-shared/models/order.models';
import { HttpErrorResponse } from '@angular/common/http';

export const addToInvoice = createAction(
  '[Order] Add To Invoice',
  props<{ line: IInvoiceLine }>(),
);

export const updateLine = createAction(
  '[Order] Update Line',
  props<{ line: IInvoiceLine }>(),
);

export const deleteLine = createAction(
  '[Order] Delete Line',
  props<{ idx: number }>(),
);

export const checkout = createAction(
  '[Order] Checkout',
  props<{ payload: ICreateOrderRequest }>(),
);

export const checkoutSuccess = createAction(
  '[Order] Checkout Success',
  props<{ data: IOrderViewModel }>(),
);

export const checkoutFailed = createAction(
  '[Order] Checkout Failed',
  props<{ error: HttpErrorResponse }>(),
);

export const deleteAllLine = createAction('[Order] Delete All Line');
