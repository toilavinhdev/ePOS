import { Component, OnInit, ViewChild } from '@angular/core';
import {
  DynamicTableComponent,
  SectionContentComponent,
} from '@app-shared/components';
import { ButtonModule } from 'primeng/button';
import { CellDirective, ColumnDirective } from '@app-shared/directives';
import { BaseComponent } from '@app-shared/core/abtractions';
import { Store } from '@ngrx/store';
import { Observable, takeUntil } from 'rxjs';
import {
  createUnitSuccess,
  listUnit,
  unitListSelector,
  unitLoadingListSelector,
  unitPaginatorSelector,
  updateUnitSuccess,
} from '@app-shared/store/unit';
import {
  IListUnitRequest,
  IUnitViewModel,
} from '@app-shared/models/unit.models';
import { IPaginator } from '@app-shared/core/models/common.models';
import { AsyncPipe } from '@angular/common';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { SettingUnitDetailModalComponent } from '@app-features/management/setting/setting-unit/setting-unit-detail-modal/setting-unit-detail-modal.component';
import { Actions, ofType } from '@ngrx/effects';

@Component({
  selector: 'app-setting-unit',
  standalone: true,
  imports: [
    SectionContentComponent,
    ButtonModule,
    DynamicTableComponent,
    ColumnDirective,
    AsyncPipe,
    CellDirective,
    SettingUnitDetailModalComponent,
  ],
  templateUrl: './setting-unit.component.html',
  styles: ``,
})
export class SettingUnitComponent extends BaseComponent implements OnInit {
  loadingList$!: Observable<boolean>;
  records$!: Observable<IUnitViewModel[]>;
  paginator$!: Observable<IPaginator | null>;

  filterForm!: UntypedFormGroup;

  @ViewChild('modal') unitModal!: SettingUnitDetailModalComponent;

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
    this.paginator$.subscribe((model) => {
      if (model) {
        this.filterForm.patchValue({ ...model });
      }
    });
    this.actions$
      .pipe(
        ofType(createUnitSuccess, updateUnitSuccess),
        takeUntil(this.destroy$),
      )
      .subscribe(() => {
        this.unitModal.hideModal();
        this.loadData();
      });
  }

  private setSelector() {
    this.loadingList$ = this.store
      .select(unitLoadingListSelector)
      .pipe(takeUntil(this.destroy$));
    this.records$ = this.store
      .select(unitListSelector)
      .pipe(takeUntil(this.destroy$));
    this.paginator$ = this.store
      .select(unitPaginatorSelector)
      .pipe(takeUntil(this.destroy$));
  }

  onPageChange(paginator: IPaginator) {
    this.filterForm.patchValue({
      pageIndex: paginator.pageIndex,
      pageSize: paginator.pageSize,
    });
    this.loadData();
  }

  private loadData() {
    this.store.dispatch(
      listUnit({ payload: this.filterForm.value as IListUnitRequest }),
    );
  }

  private buildForm() {
    this.filterForm = this.formBuilder.group({
      pageIndex: [1, [Validators.required]],
      pageSize: [20, [Validators.required]],
      name: [null],
      isDefault: [null],
      sort: [null],
    });
  }
}
