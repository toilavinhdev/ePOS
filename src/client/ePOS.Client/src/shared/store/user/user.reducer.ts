import { createReducer, on } from '@ngrx/store';
import {
  getMe,
  getMeFailed,
  getMeSuccess,
  signIn,
  signInFailed,
  signInSuccess,
  signUp,
  signUpFailed,
  signUpSuccess,
} from '@app-shared/store/user/user.actions';
import { IUserClaimsValue } from '@app-shared/core/models/common.models';
import { USER_DATA } from '@app-shared/constants';
import { IGetMeResponse } from '@app-shared/models/user.models';

export interface IUserState {
  loading: boolean;
  loadingMe: boolean;
  profile?: IGetMeResponse;
}

const initialState: IUserState = {
  loading: false,
  loadingMe: false,
  profile: undefined,
};

export const userReducer = createReducer(
  initialState,
  on(signIn, (state) => ({
    ...state,
    loading: true,
  })),
  on(signInSuccess, (state) => ({
    ...state,
    loading: false,
  })),
  on(signInFailed, (state) => ({
    ...state,
    loading: false,
  })),
  on(signUp, (state) => ({
    ...state,
    loading: true,
  })),
  on(signUpSuccess, (state) => ({
    ...state,
    loading: false,
  })),
  on(signUpFailed, (state) => ({
    ...state,
    loading: false,
  })),
  on(getMe, (state) => ({
    ...state,
  })),
  on(getMeSuccess, (state, { data }) => ({
    ...state,
    profile: data,
  })),
  on(getMeFailed, (state) => ({
    ...state,
  })),
);
