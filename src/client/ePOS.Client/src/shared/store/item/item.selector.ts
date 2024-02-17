import { createFeatureSelector, createSelector } from '@ngrx/store';
import { IItemState } from '@app-shared/store/item/item.reducer';
import { appStateKey } from '@app-shared/store/app.state';

export const featureItem = createFeatureSelector<IItemState>(
  appStateKey.feature_item,
);

export const itemLoadingListSelector = createSelector(
  featureItem,
  (state) => state.loadingList,
);

export const itemLoadingCreateOrUpdateSelector = createSelector(
  featureItem,
  (state) => state.loadingCreateOrUpdate,
);

export const itemListSelector = createSelector(
  featureItem,
  (state) => state.items,
);

export const itemPaginatorSelector = createSelector(
  featureItem,
  (state) => state.paginator,
);
