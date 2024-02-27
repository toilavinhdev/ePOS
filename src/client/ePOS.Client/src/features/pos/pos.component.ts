import { Component } from '@angular/core';
import { PosMenuComponent } from '@app-features/pos/pos-menu/pos-menu.component';
import { PosBillComponent } from '@app-features/pos/pos-bill/pos-bill.component';
import { InputTextModule } from 'primeng/inputtext';
import { UserAvatarComponent } from '@app-shared/components';

@Component({
  selector: 'app-pos',
  standalone: true,
  imports: [
    PosMenuComponent,
    PosBillComponent,
    InputTextModule,
    UserAvatarComponent,
  ],
  templateUrl: './pos.component.html',
  styles: ``,
})
export class PosComponent {}
