import { ITenantViewModel } from '@app-shared/models/tenant.models';
import { createReducer, on } from '@ngrx/store';
import {
  getTenant,
  getTenantFailed,
  getTenantSuccess,
  updateTenant,
  updateTenantFailed,
  updateTenantSuccess,
} from '@app-shared/store/tenant/tenant.actions';

export interface ITenantState {
  loadingGet: boolean;
  loadingUpdate: boolean;
  tenant?: ITenantViewModel;
}

const initialState: ITenantState = {
  loadingGet: false,
  loadingUpdate: false,
};

export const tenantReducer = createReducer(
  initialState,
  on(getTenant, (state) => ({
    ...state,
    loadingGet: true,
  })),
  on(getTenantSuccess, (state, { data }) => ({
    ...state,
    loadingGet: false,
    tenant: data,
  })),
  on(getTenantFailed, (state) => ({
    ...state,
    loadingGet: false,
  })),
  on(updateTenant, (state) => ({
    ...state,
    loadingUpdate: true,
  })),
  on(updateTenantSuccess, (state, { data }) => ({
    ...state,
    loadingUpdate: false,
    tenant: data,
  })),
  on(updateTenantFailed, (state) => ({
    ...state,
    loadingUpdate: false,
  })),
);
