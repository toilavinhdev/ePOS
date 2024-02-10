import { CanActivateFn } from '@angular/router';

export const signedInGuard: CanActivateFn = (route, state) => {
  return true;
};
