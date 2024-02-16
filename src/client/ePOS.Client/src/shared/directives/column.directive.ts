import {
  booleanAttribute,
  ContentChild,
  Directive,
  Input,
} from '@angular/core';
import { HeaderDirective } from '@app-shared/directives/header.directive';
import { CellDirective } from '@app-shared/directives/cell.directive';

@Directive({
  selector: 'app-column',
  standalone: true,
})
export class ColumnDirective {
  @ContentChild(HeaderDirective, { static: true }) headerTpl?: HeaderDirective;
  @ContentChild(CellDirective, { static: true }) cellTpl?: CellDirective;
  @Input() key: string = '';
  @Input() header: string = '';
  @Input() headerStyleClass = '';
  @Input() columnStyleClass = '';
  @Input({ transform: booleanAttribute }) sortable = false;
  constructor() {}
}
