import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { BaseComponent } from '@app-shared/core/abtractions';
import { Store } from '@ngrx/store';
import { categoryListSelector, listCategory } from '@app-shared/store/category';
import { Observable, takeUntil } from 'rxjs';
import { ICategoryViewModel } from '@app-shared/models/category.models';
import { AsyncPipe, NgIf } from '@angular/common';

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

  @Output() categoryIdChange = new EventEmitter<string | undefined>();

  constructor(private store: Store) {
    super();
  }

  ngOnInit() {
    this.loadCategories();
    this.categories$ = this.store
      .select(categoryListSelector)
      .pipe(takeUntil(this.destroy$));
  }

  loadCategories() {
    this.store.dispatch(
      listCategory({
        payload: { pageIndex: this.pageIndex, pageSize: this.pageSize },
      }),
    );
  }

  selectCategory(id: string | undefined) {
    this.selectedCategoryId = id;
    this.categoryIdChange.emit(id);
  }
}
