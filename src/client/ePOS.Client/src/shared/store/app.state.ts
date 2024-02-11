import { IUserState } from '@app-shared/store/user/user.reducer';

export interface IAppState {
  feature_user: IUserState;
}

export const appStateKey = {
  feature_user: 'feature_user',
};
