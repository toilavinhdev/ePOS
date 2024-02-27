import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ICategoryState } from '@app-shared/store/category/category.reducer';
import { appStateKey } from '@app-shared/store/app.state';

export const featureCategory = createFeatureSelector<ICategoryState>(
  appStateKey.feature_category,
);

export const categoryLoadingListSelector = createSelector(
  featureCategory,
  (state) => state.loadingList,
);

export const categoryLoadingCreateOrUpdateSelector = createSelector(
  featureCategory,
  (state) => state.loadingCreateOrUpdate,
);

export const categoryListSelector = createSelector(
  featureCategory,
  (state) => state.categories,
);

export const categoryTotalRecordSelector = createSelector(
  featureCategory,
  (state) => state.totalRecords,
);

export const categoryPaginatorSelector = createSelector(
  featureCategory,
  (state) => state.paginator,
);
