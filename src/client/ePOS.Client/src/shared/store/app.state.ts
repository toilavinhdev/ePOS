import { IUserState } from '@app-shared/store/user/user.reducer';
import { ITenantState } from '@app-shared/store/tenant/tenant.reducer';
import { IUnitState } from '@app-shared/store/unit';
import { IStorageState } from '@app-shared/store/storage';

export interface IAppState {
  feature_user: IUserState;
  feature_tenant: ITenantState;
  feature_unit: IUnitState;
  feature_storage: IStorageState;
}

export const appStateKey = {
  feature_user: 'feature_user',
  feature_tenant: 'feature_tenant',
  feature_unit: 'feature_unit',
  feature_storage: 'feature_storage',
};
