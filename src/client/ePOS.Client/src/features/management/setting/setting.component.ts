import { Component } from '@angular/core';
import { ITabMenuItem, TabMenuComponent } from '@app-shared/components';
import { SettingTenantComponent } from '@app-features/management/setting/setting-tenant/setting-tenant.component';
import { SettingUnitComponent } from '@app-features/management/setting/setting-unit/setting-unit.component';

const tabItems: ITabMenuItem[] = [
  {
    id: 'tenant',
    title: 'Cài đặt chung',
  },
  {
    id: 'unit',
    title: 'Thiết lập đơn vị',
  },
];

@Component({
  selector: 'app-setting',
  standalone: true,
  imports: [TabMenuComponent, SettingTenantComponent, SettingUnitComponent],
  templateUrl: './setting.component.html',
  styles: ``,
})
export class SettingComponent {
  protected readonly tabItems = tabItems;
  currentTabId!: string;
}
