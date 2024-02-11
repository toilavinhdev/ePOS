import { createAction, props } from '@ngrx/store';
import {
  ISignInRequest,
  ISignInResponse,
} from '@app-shared/models/user.models';
import { HttpErrorResponse } from '@angular/common/http';

export const signIn = createAction(
  '[User] Sign In',
  props<{ payload: ISignInRequest }>(),
);

export const signInSuccess = createAction(
  '[User] Sign In Success',
  props<{ data: ISignInResponse }>(),
);

export const signInFailed = createAction(
  '[User] Sign In Failed',
  props<{ error: HttpErrorResponse }>(),
);
