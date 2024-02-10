import { IAppState } from '@app-shared/store/app.state';
import { createSelector } from '@ngrx/store';
import { IUserState } from '@app-shared/store/user/user.reducer';

export const selectUserState = (state: IAppState) => state.userState;

export const selectLoadingSignIn = createSelector(
  selectUserState,
  (state: IUserState) => state.loadingSignIn,
);
