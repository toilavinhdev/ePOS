import { ApplicationConfig, isDevMode } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authorizationInterceptor } from '@app-shared/core/interceptors/authorization.interceptor';
import { provideStore } from '@ngrx/store';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import { provideEffects } from '@ngrx/effects';
import { userReducer } from '@app-shared/store/user/user.reducer';
import { UserEffects } from '@app-shared/store/user/user.effects';
import { appStateKey } from '@app-shared/store/app.state';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideAnimations(),
    provideHttpClient(withInterceptors([authorizationInterceptor])),
    provideStore({
      [appStateKey.feature_user]: userReducer,
    }),
    provideStoreDevtools({ maxAge: 25, logOnly: !isDevMode() }),
    provideEffects([UserEffects]),
  ],
};
