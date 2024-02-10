import { createReducer, on } from '@ngrx/store';
import {
  signIn,
  signInFailed,
  signInSuccess,
} from '@app-shared/store/user/user.actions';

export interface IUserState {
  loadingSignIn: boolean;
  error?: string;
}

const initialState: IUserState = {
  loadingSignIn: false,
};

export const userReducer = createReducer(
  initialState,
  on(signIn, (state, { payload }) => ({
    ...state,
    loadingSignIn: true,
  })),
  on(signInSuccess, (state, { data }) => ({
    ...state,
    loadingSignIn: false,
    error: undefined,
  })),
  on(signInFailed, (state, { error }) => ({
    ...state,
    loadingSignIn: false,
    error: error.error.message,
  })),
);
