import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { UserService } from '@app-shared/services';

export const signedInGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const userService = inject(UserService);

  const active = !!userService.getUserClaimsValidExp();

  if (active) {
    router.navigate(['']).then();
  }

  return !active;
};
