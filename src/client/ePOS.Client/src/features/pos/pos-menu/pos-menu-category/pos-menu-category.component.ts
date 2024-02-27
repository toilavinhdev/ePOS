import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { BaseComponent } from '@app-shared/core/abtractions';
import { Store } from '@ngrx/store';
import {
  categoryListSelector,
  categoryPaginatorSelector,
  categoryTotalRecordSelector,
  listCategory,
} from '@app-shared/store/category';
import { Observable, takeUntil } from 'rxjs';
import { ICategoryViewModel } from '@app-shared/models/category.models';
import { AsyncPipe, NgIf } from '@angular/common';
import { IPaginator } from '@app-shared/core/models/common.models';
import { itemPaginatorSelector } from '@app-shared/store/item';

@Component({
  selector: 'app-pos-menu-category',
  standalone: true,
  imports: [AsyncPipe, NgIf],
  templateUrl: './pos-menu-category.component.html',
  styles: ``,
})
export class PosMenuCategoryComponent extends BaseComponent implements OnInit {
  pageIndex = 1;
  pageSize = 10;
  categories$!: Observable<ICategoryViewModel[]>;
  selectedCategoryId: string | undefined = undefined;

  totalRecords$!: Observable<number>;

  @Output() categoryIdChange = new EventEmitter<string | undefined>();

  constructor(private store: Store) {
    super();
  }

  ngOnInit() {
    this.categories$ = this.store
      .select(categoryListSelector)
      .pipe(takeUntil(this.destroy$));
    this.totalRecords$ = this.store
      .select(categoryTotalRecordSelector)
      .pipe(takeUntil(this.destroy$));
  }

  selectCategory(id: string | undefined) {
    this.selectedCategoryId = id;
    this.categoryIdChange.emit(id);
  }
}
