import { HttpInterceptorFn } from '@angular/common/http';
import { ACCESS_TOKEN } from '@app-shared/constants';

export const authorizationInterceptor: HttpInterceptorFn = (req, next) => {
  const accessToken = localStorage.getItem(ACCESS_TOKEN);
  return !accessToken
    ? next(req)
    : next(
        req.clone({
          setHeaders: {
            Authorization: `Bearer ${accessToken}`,
          },
        }),
      );
};
