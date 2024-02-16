import { Component } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DynamicTableComponent } from '@app-shared/components';
import { ColumnDirective } from '@app-shared/directives';

@Component({
  selector: 'app-lib-item',
  standalone: true,
  imports: [ButtonModule, DynamicTableComponent, ColumnDirective],
  templateUrl: './lib-item.component.html',
  styles: ``,
})
export class LibItemComponent {}
