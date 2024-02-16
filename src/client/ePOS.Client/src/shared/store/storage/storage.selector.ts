import { createFeatureSelector, createSelector } from '@ngrx/store';
import { appStateKey } from '@app-shared/store/app.state';
import { IStorageState } from '@app-shared/store/storage/storage.reducer';

export const featureStorage = createFeatureSelector<IStorageState>(
  appStateKey.feature_storage,
);

export const storageLoadingSelector = createSelector(
  featureStorage,
  (state) => state.loading,
);
