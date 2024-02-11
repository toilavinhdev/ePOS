import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { NotificationService, UserService } from '@app-shared/services';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const userService = inject(UserService);
  const notificationService = inject(NotificationService);

  const active = !!userService.getUserClaimsValidExp();

  if (!active) {
    router.navigate(['/auth/sign-in']).then();
    notificationService.warning('Bạn cần phải đăng nhập');
  }

  return active;
};
