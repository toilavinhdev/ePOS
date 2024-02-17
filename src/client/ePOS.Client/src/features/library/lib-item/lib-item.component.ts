import { Component, OnInit, ViewChild } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DynamicTableComponent } from '@app-shared/components';
import { CellDirective, ColumnDirective } from '@app-shared/directives';
import { BaseComponent } from '@app-shared/core/abtractions';
import { Store } from '@ngrx/store';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { Observable, takeUntil } from 'rxjs';
import {
  createItemSuccess,
  itemListSelector,
  itemPaginatorSelector,
  listItem,
} from '@app-shared/store/item';
import {
  IItemViewModel,
  IListItemRequest,
} from '@app-shared/models/item.models';
import { IPaginator } from '@app-shared/core/models/common.models';
import { AsyncPipe, DecimalPipe, NgForOf, NgIf } from '@angular/common';
import { TagModule } from 'primeng/tag';
import { RippleModule } from 'primeng/ripple';
import { ImageModule } from 'primeng/image';
import { defaultImagePath } from '@app-shared/constants';
import { LibItemDetailModalComponent } from '@app-features/library/lib-item/lib-item-detail-modal/lib-item-detail-modal.component';
import { Actions, ofType } from '@ngrx/effects';

@Component({
  selector: 'app-lib-item',
  standalone: true,
  imports: [
    ButtonModule,
    DynamicTableComponent,
    ColumnDirective,
    AsyncPipe,
    CellDirective,
    TagModule,
    RippleModule,
    ImageModule,
    DecimalPipe,
    NgIf,
    NgForOf,
    LibItemDetailModalComponent,
  ],
  templateUrl: './lib-item.component.html',
  styles: ``,
})
export class LibItemComponent extends BaseComponent implements OnInit {
  filterForm!: UntypedFormGroup;
  item$!: Observable<IItemViewModel[]>;
  paginator$!: Observable<IPaginator | undefined>;

  @ViewChild('modal') modal!: LibItemDetailModalComponent;

  constructor(
    private store: Store,
    private formBuilder: UntypedFormBuilder,
    private actions$: Actions,
  ) {
    super();
  }

  ngOnInit() {
    this.buildForm();
    this.loadData();
    this.setSelector();

    this.actions$
      .pipe(ofType(createItemSuccess), takeUntil(this.destroy$))
      .subscribe(() => {
        this.modal.onHideModal();
        this.loadData();
      });
  }

  private setSelector() {
    this.item$ = this.store
      .select(itemListSelector)
      .pipe(takeUntil(this.destroy$));
    this.paginator$ = this.store
      .select(itemPaginatorSelector)
      .pipe(takeUntil(this.destroy$));
  }

  private loadData() {
    this.store.dispatch(
      listItem({ payload: this.filterForm.value as IListItemRequest }),
    );
  }

  private buildForm() {
    this.filterForm = this.formBuilder.group({
      pageIndex: [1, [Validators.required]],
      pageSize: [20, [Validators.required]],
      name: [null],
      categoryId: [null],
      isActive: [null],
      sort: [null],
    });
  }

  protected readonly defaultImagePath = defaultImagePath;

  onPageChange(event: IPaginator) {
    this.filterForm.patchValue({
      pageIndex: event.pageIndex,
      pageSize: event.pageSize,
    });
  }

  onSortChange(event: string) {
    this.filterForm.patchValue({
      sort: event,
    });
  }
}
