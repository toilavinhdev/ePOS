import { createReducer, on } from '@ngrx/store';
import {
  signIn,
  signInFailed,
  signInSuccess,
  signUp,
  signUpFailed,
  signUpSuccess,
} from '@app-shared/store/user/user.actions';
import { IUserClaimsValue } from '@app-shared/core/models/common.models';
import { USER_DATA } from '@app-shared/constants';

export interface IUserState {
  loading: boolean;
}

const initialState: IUserState = {
  loading: false,
};

export const userReducer = createReducer(
  initialState,
  on(signIn, (state, { payload }) => ({
    ...state,
    loading: true,
  })),
  on(signInSuccess, (state, { data }) => ({
    ...state,
    loading: false,
  })),
  on(signInFailed, (state, { error }) => ({
    ...state,
    loading: false,
  })),
  on(signUp, (state, { payload }) => ({
    ...state,
    loading: true,
  })),
  on(signUpSuccess, (state) => ({
    ...state,
    loading: false,
  })),
  on(signUpFailed, (state, { error }) => ({
    ...state,
    loading: false,
  })),
);
