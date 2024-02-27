import { Component, OnInit } from '@angular/core';
import { DialogModule } from 'primeng/dialog';
import {
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { MultiSelectModule } from 'primeng/multiselect';
import { ButtonModule } from 'primeng/button';
import { BaseComponent } from '@app-shared/core/abtractions';
import { Store } from '@ngrx/store';
import { itemListSelector, listItem } from '@app-shared/store/item';
import {
  IItemViewModel,
  IListItemRequest,
} from '@app-shared/models/item.models';
import { FormType } from '@app-shared/core/models/form.models';
import { Observable, takeUntil } from 'rxjs';
import { AsyncPipe, JsonPipe } from '@angular/common';
import { createCategory } from '@app-shared/store/category';
import { ICreateCategoryRequest } from '@app-shared/models/category.models';
import { Actions } from '@ngrx/effects';

@Component({
  selector: 'app-lib-category-detail-modal',
  standalone: true,
  imports: [
    DialogModule,
    ReactiveFormsModule,
    InputTextModule,
    MultiSelectModule,
    ButtonModule,
    AsyncPipe,
    JsonPipe,
  ],
  templateUrl: './lib-category-detail-modal.component.html',
  styles: ``,
})
export class LibCategoryDetailModalComponent
  extends BaseComponent
  implements OnInit
{
  form!: UntypedFormGroup;
  itemFilterForm!: UntypedFormGroup;

  visible = false;
  formType: FormType = FormType.Undefined;

  items$!: Observable<IItemViewModel[]>;

  constructor(
    private formBuilder: UntypedFormBuilder,
    private store: Store,
  ) {
    super();
  }

  ngOnInit() {
    this.buildForm();
    this.setSelector();
    this.loadItems();
  }

  private loadItems() {
    this.store.dispatch(
      listItem({ payload: this.itemFilterForm.value as IListItemRequest }),
    );
  }

  onSubmit() {
    if (this.form.invalid) return;
    if (this.formType === FormType.Create) {
      this.store.dispatch(
        createCategory({ payload: this.form.value as ICreateCategoryRequest }),
      );
    } else if (this.formType === FormType.Update) {
    }
  }

  showModal(formType: FormType) {
    this.visible = true;
    this.formType = formType;
    if (formType === FormType.Create) {
    }
  }

  hideModal() {
    this.visible = false;
    this.formType = FormType.Undefined;
    this.form.reset();
  }

  private setSelector() {
    this.items$ = this.store
      .select(itemListSelector)
      .pipe(takeUntil(this.destroy$));
  }

  private buildForm() {
    this.form = this.formBuilder.group({
      id: [null],
      name: [null, [Validators.required]],
      itemIds: [null],
    });
    this.itemFilterForm = this.formBuilder.group({
      pageIndex: [1, [Validators.required]],
      pageSize: [20, [Validators.required]],
      name: [null],
      categoryId: [null],
      isActive: [null],
      sort: [null],
    });
  }

  protected readonly FormType = FormType;
}
