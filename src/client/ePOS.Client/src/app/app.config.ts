import { ApplicationConfig, isDevMode } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authorizationInterceptor } from '@app-shared/core/interceptors/authorization.interceptor';
import { provideStore } from '@ngrx/store';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import { provideEffects } from '@ngrx/effects';
import { UserEffects, userReducer } from '@app-shared/store/user';
import { TenantEffects, tenantReducer } from '@app-shared/store/tenant';
import { UnitEffects, unitReducer } from '@app-shared/store/unit';
import { StorageEffects, storageReducer } from '@app-shared/store/storage';
import { ItemEffects, itemReducer } from '@app-shared/store/item';
import { CategoryEffects, categoryReducer } from '@app-shared/store/category';
import { orderReducer } from '@app-shared/store/order/order.reducer';
import { OrderEffects } from '@app-shared/store/order/order.effects';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideAnimations(),
    provideHttpClient(withInterceptors([authorizationInterceptor])),
    provideStore({
      feature_user: userReducer,
      feature_tenant: tenantReducer,
      feature_unit: unitReducer,
      feature_storage: storageReducer,
      feature_item: itemReducer,
      feature_category: categoryReducer,
      feature_order: orderReducer,
    }),
    provideStoreDevtools({ maxAge: 25, logOnly: !isDevMode() }),
    provideEffects([
      UserEffects,
      TenantEffects,
      UnitEffects,
      StorageEffects,
      ItemEffects,
      CategoryEffects,
      OrderEffects,
    ]),
  ],
};
