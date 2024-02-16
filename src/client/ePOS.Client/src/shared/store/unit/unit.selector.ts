import { createFeatureSelector, createSelector } from '@ngrx/store';
import { appStateKey } from '@app-shared/store/app.state';
import { IUnitState } from '@app-shared/store/unit/unit.reducer';

export const featureUnit = createFeatureSelector<IUnitState>(
  appStateKey.feature_unit,
);

export const unitLoadingListSelector = createSelector(
  featureUnit,
  (state) => state.loadingList,
);

export const unitPaginatorSelector = createSelector(
  featureUnit,
  (state) => state.paginator,
);

export const unitLoadingCreateOrUpdateSelector = createSelector(
  featureUnit,
  (state) => state.loadingCreateOrUpdate,
);

export const unitListSelector = createSelector(
  featureUnit,
  (state) => state.units,
);
