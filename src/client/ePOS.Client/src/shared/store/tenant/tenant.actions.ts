import { createAction, props } from '@ngrx/store';
import {
  ITenantViewModel,
  IUpdateTenantRequest,
} from '@app-shared/models/tenant.models';
import { HttpErrorResponse } from '@angular/common/http';

export const getTenant = createAction('[Tenant] Get');

export const getTenantSuccess = createAction(
  '[Tenant] Get Success',
  props<{ data: ITenantViewModel }>(),
);

export const getTenantFailed = createAction(
  '[Tenant] Get Failed',
  props<{ error: HttpErrorResponse }>(),
);

export const updateTenant = createAction(
  '[Tenant] Update',
  props<{ payload: IUpdateTenantRequest }>(),
);

export const updateTenantSuccess = createAction(
  '[Tenant] Update Success',
  props<{ data: ITenantViewModel }>(),
);

export const updateTenantFailed = createAction(
  '[Tenant] Update Failed',
  props<{ error: HttpErrorResponse }>(),
);
