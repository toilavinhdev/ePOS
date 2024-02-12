import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { NotificationService, UserService } from '@app-shared/services';
import {
  signIn,
  signInFailed,
  signInSuccess,
  signUp,
  signUpFailed,
  signUpSuccess,
} from '@app-shared/store/user/user.actions';
import { catchError, map, of, switchMap, tap } from 'rxjs';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { IUserClaimsValue } from '@app-shared/core/models/common.models';
import { ACCESS_TOKEN, REFRESH_TOKEN, USER_DATA } from '@app-shared/constants';

@Injectable()
export class UserEffects {
  constructor(
    private actions$: Actions,
    private userService: UserService,
    private notificationService: NotificationService,
    private router: Router,
  ) {}

  signIn$ = createEffect(() =>
    this.actions$.pipe(
      ofType(signIn),
      switchMap(({ payload }) =>
        this.userService.signIn(payload).pipe(
          map((data) => signInSuccess({ data })),
          catchError((error) => of(signInFailed({ error }))),
        ),
      ),
    ),
  );

  signInSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(signInSuccess),
        tap(({ data }) => {
          const userClaimsValue = jwtDecode<IUserClaimsValue>(data.accessToken);
          localStorage.setItem(ACCESS_TOKEN, data.accessToken);
          localStorage.setItem(REFRESH_TOKEN, data.refreshToken);
          localStorage.setItem(USER_DATA, JSON.stringify(userClaimsValue));
          this.notificationService.success('Đăng nhập thành công');
          this.router.navigate(['']).then();
        }),
      ),
    { dispatch: false },
  );

  signInFailed$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(signInFailed),
        tap(({ error }) => {
          const message = error.error.message;
          if (message === 'EmailNotFound') {
            this.notificationService.error('Email không tồn tại');
          } else if (message === 'IncorrectPassword') {
            this.notificationService.error('Mật khẩu không chính xác');
          } else {
            this.notificationService.error('Đăng nhập thất bại');
          }
        }),
      ),
    { dispatch: false },
  );

  signUp$ = createEffect(() =>
    this.actions$.pipe(
      ofType(signUp),
      switchMap(({ payload }) =>
        this.userService.signUp(payload).pipe(
          map(() => signUpSuccess()),
          catchError((err) => of(signUpFailed({ error: err }))),
        ),
      ),
    ),
  );

  signUpSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(signUpSuccess),
        tap(() => {
          this.notificationService.success('Đăng ký thành công');
          this.router.navigate(['/auth/sign-in']).then();
        }),
      ),
    { dispatch: false },
  );

  signUpFailed$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(signUpFailed),
        tap(({ error }) => {
          const message = error.error.message;
          if (message === 'DuplicateUserName;DuplicateEmail') {
            this.notificationService.error('Email đã được sử dụng');
          }
          if (message === 'DuplicateTenantName') {
            this.notificationService.error('Tên thương hiệu đã tồn tại');
          }
        }),
      ),
    {
      dispatch: false,
    },
  );
}
