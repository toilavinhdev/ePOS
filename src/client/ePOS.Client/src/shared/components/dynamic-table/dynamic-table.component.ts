import {
  booleanAttribute,
  Component,
  ContentChildren,
  EventEmitter,
  Input,
  Output,
  QueryList,
} from '@angular/core';
import { ColumnDirective } from '@app-shared/directives/column.directive';
import { TableModule } from 'primeng/table';
import { CommonModule } from '@angular/common';
import { PaginatorModule, PaginatorState } from 'primeng/paginator';
import { IPaginator } from '@app-shared/core/models/common.models';

@Component({
  selector: 'app-dynamic-table',
  standalone: true,
  imports: [TableModule, CommonModule, PaginatorModule],
  templateUrl: './dynamic-table.component.html',
  styles: ``,
})
export class DynamicTableComponent<T> {
  @ContentChildren(ColumnDirective) columns!: QueryList<ColumnDirective>;
  @Input() data: T[] = [];
  @Input() dataKey = 'id';
  @Input({ transform: booleanAttribute }) gridLines = false;
  @Input() paginator?: IPaginator;
  @Input() loading = false;
  @Output() pageChange = new EventEmitter<IPaginator>();
  @Output() sortChange = new EventEmitter<string>();
  first = 0;
  saveSortField = '';

  get useGridLines() {
    return this.gridLines ? 'p-datatable-gridlines' : '';
  }

  get rowsPerPageOptions() {
    let options = [5, 10, 20, 50];
    const size = this.paginator?.pageSize ?? 0;
    if (size === 0 || options.some((x) => x === size)) return options;
    else {
      options.push(size);
      options.sort((a, b) => b - a);
      return options;
    }
  }

  onPageChange(state: PaginatorState) {
    this.first = state.first!;
    this.pageChange.emit({
      pageIndex: state.page! + 1,
      pageSize: state.rows,
    } as IPaginator);
  }

  onSort(event: any) {
    if (!event.field) return;
    const sortField = (event.order > 0 ? '+' : '-') + event.field;
    if (this.saveSortField === sortField) return;
    this.saveSortField = sortField;
    this.sortChange.emit(sortField);
  }
}
