import { Component, OnInit } from '@angular/core';
import {
  DynamicTableComponent,
  SectionContentComponent,
} from '@app-shared/components';
import { ButtonModule } from 'primeng/button';
import { LibCategoryDetailModalComponent } from '@app-features/library/lib-category/lib-category-detail-modal/lib-category-detail-modal.component';
import { CellDirective, ColumnDirective } from '@app-shared/directives';
import { BaseComponent } from '@app-shared/core/abtractions';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { Store } from '@ngrx/store';
import {
  categoryListSelector,
  categoryPaginatorSelector,
  listCategory,
} from '@app-shared/store/category';
import {
  ICategoryViewModel,
  IListCategoryRequest,
} from '@app-shared/models/category.models';
import { Observable, takeUntil } from 'rxjs';
import { IPaginator } from '@app-shared/core/models/common.models';
import { AsyncPipe } from '@angular/common';
import { TagModule } from 'primeng/tag';
import { FormType } from '@app-shared/core/models/form.models';

@Component({
  selector: 'app-lib-category',
  standalone: true,
  imports: [
    SectionContentComponent,
    ButtonModule,
    LibCategoryDetailModalComponent,
    DynamicTableComponent,
    ColumnDirective,
    AsyncPipe,
    CellDirective,
    TagModule,
  ],
  templateUrl: './lib-category.component.html',
  styles: ``,
})
export class LibCategoryComponent extends BaseComponent implements OnInit {
  filterForm!: UntypedFormGroup;
  categories$!: Observable<ICategoryViewModel[]>;
  paginator$!: Observable<IPaginator | undefined>;

  constructor(
    private formBuilder: UntypedFormBuilder,
    private store: Store,
  ) {
    super();
  }

  ngOnInit() {
    this.buildForm();
    this.setSelector();
    this.loadData();
  }

  private setSelector() {
    this.categories$ = this.store
      .select(categoryListSelector)
      .pipe(takeUntil(this.destroy$));
    this.paginator$ = this.store
      .select(categoryPaginatorSelector)
      .pipe(takeUntil(this.destroy$));
  }

  private loadData() {
    this.store.dispatch(
      listCategory({ payload: this.filterForm.value as IListCategoryRequest }),
    );
  }

  private buildForm() {
    this.filterForm = this.formBuilder.group({
      pageIndex: [1, Validators.required],
      pageSize: [20, Validators.required],
      name: [null],
      isActive: [null],
      sort: [null],
    });
  }

  protected readonly FormType = FormType;
}
