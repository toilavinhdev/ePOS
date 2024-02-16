import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ITenantState } from '@app-shared/store/tenant/tenant.reducer';
import { appStateKey } from '@app-shared/store/app.state';

export const featureTenant = createFeatureSelector<ITenantState>(
  appStateKey.feature_tenant,
);

export const tenantLoadingGetSelector = createSelector(
  featureTenant,
  (state) => state.loadingGet,
);

export const tenantLoadingUpdateSelector = createSelector(
  featureTenant,
  (state) => state.loadingUpdate,
);

export const tenantSelector = createSelector(
  featureTenant,
  (state) => state.tenant,
);
