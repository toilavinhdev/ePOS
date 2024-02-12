import { createFeatureSelector, createSelector } from '@ngrx/store';
import { IUserState } from '@app-shared/store/user/user.reducer';
import { appStateKey } from '@app-shared/store/app.state';

export const featureUser = createFeatureSelector<IUserState>(
  appStateKey.feature_user,
);

export const userLoadingSelector = createSelector(
  featureUser,
  (state) => state.loading,
);

export const userProfileSelector = createSelector(
  featureUser,
  (state) => state.profile,
);
