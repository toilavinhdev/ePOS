import { Component, OnInit, ViewChild } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import {
  FormBuilder,
  ReactiveFormsModule,
  UntypedFormArray,
  UntypedFormControl,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { InputNumberModule } from 'primeng/inputnumber';
import { AsyncPipe, JsonPipe, NgForOf, NgIf } from '@angular/common';
import { BaseComponent } from '@app-shared/core/abtractions';
import { FormType } from '@app-shared/core/models/form.models';
import { Store } from '@ngrx/store';
import { Observable, takeUntil } from 'rxjs';
import {
  IListUnitRequest,
  IUnitViewModel,
} from '@app-shared/models/unit.models';
import { IPaginator } from '@app-shared/core/models/common.models';
import {
  listUnit,
  unitListSelector,
  unitPaginatorSelector,
} from '@app-shared/store/unit';
import { UploadMultipleImageComponent } from '@app-shared/components';
import { createItem, createItemFailed } from '@app-shared/store/item';
import {
  ICreateItemRequest,
  IItemViewModel,
} from '@app-shared/models/item.models';
import { NotificationService, StorageService } from '@app-shared/services';

@Component({
  selector: 'app-lib-item-detail-modal',
  standalone: true,
  imports: [
    ButtonModule,
    DialogModule,
    ReactiveFormsModule,
    InputTextModule,
    DropdownModule,
    InputNumberModule,
    NgIf,
    NgForOf,
    AsyncPipe,
    UploadMultipleImageComponent,
    JsonPipe,
  ],
  templateUrl: './lib-item-detail-modal.component.html',
  styles: ``,
})
export class LibItemDetailModalComponent
  extends BaseComponent
  implements OnInit
{
  visible = false;
  currentFormType: FormType = FormType.Undefined;
  form!: UntypedFormGroup;

  unit$!: Observable<IUnitViewModel[]>;
  unitPaginator!: Observable<IPaginator | null>;
  unitFilterForm!: UntypedFormGroup;

  @ViewChild('uploadMultiple')
  uploadMultipleComponent!: UploadMultipleImageComponent;

  constructor(
    private formBuilder: FormBuilder,
    private store: Store,
    private storageService: StorageService,
    private notificationService: NotificationService,
  ) {
    super();
  }

  get sizePrices(): UntypedFormArray {
    return this.form.get('sizePrices') as UntypedFormArray;
  }

  get price(): UntypedFormControl {
    return this.form.get('price') as UntypedFormControl;
  }

  get imageUrls(): UntypedFormControl {
    return this.form.get('imageUrls') as UntypedFormControl;
  }

  ngOnInit() {
    this.buildForm();
    this.loadUnits();
    this.setSelector();
  }

  setSelector() {
    this.unit$ = this.store
      .select(unitListSelector)
      .pipe(takeUntil(this.destroy$));
    this.unitPaginator = this.store
      .select(unitPaginatorSelector)
      .pipe(takeUntil(this.destroy$));
  }

  onSubmit() {
    if (this.form.invalid) return;
    if (
      this.sizePrices.length === 0 &&
      (this.price.value === 0 || !this.price.value)
    ) {
      this.notificationService.warning('Bạn chưa nhập giá cho món ăn');
      return;
    }
    if (this.currentFormType === FormType.Create) {
      const files = this.uploadMultipleComponent.getFiles();
      if (files.length === 0) {
        this.store.dispatch(
          createItem({ payload: this.form.value as ICreateItemRequest }),
        );
      } else {
        this.storageService
          .uploadMultiple(files)
          .pipe(takeUntil(this.destroy$))
          .subscribe({
            next: (urls) => {
              this.form.patchValue({
                imageUrls: urls,
              });
              this.store.dispatch(
                createItem({ payload: this.form.value as ICreateItemRequest }),
              );
            },
            error: (err) =>
              this.store.dispatch(createItemFailed({ error: err })),
          });
      }
    } else if (this.currentFormType === FormType.Update) {
      const uploadModels = this.uploadMultipleComponent.getBothFileAndSrc();
      const files = uploadModels
        ?.map((x) => (x instanceof File ? x : undefined))
        .filter((x) => x !== undefined);

      console.log('FILES', files);

      if (files!.length === 0) {
      }
    }
  }

  onShowCreateModal() {
    this.currentFormType = FormType.Create;
    this.visible = true;
  }

  onShowUpdateModal(item: IItemViewModel) {
    this.currentFormType = FormType.Update;
    this.visible = true;
    item.sizePrices?.forEach((_) => this.addSizePriceControl());
    this.form.patchValue({
      ...item,
      imageUrls: item.images?.map((x) => x.url),
    });
  }

  onHideModal() {
    this.visible = false;
    this.form.reset();
    this.sizePrices.clear();
    this.uploadMultipleComponent.reset();
    this.currentFormType = FormType.Undefined;
    this.uploadMultipleComponent.reset();
  }

  private loadUnits() {
    this.store.dispatch(
      listUnit({ payload: this.unitFilterForm.value as IListUnitRequest }),
    );
  }

  private buildForm() {
    this.form = this.formBuilder.group({
      id: [null],
      name: [null, [Validators.required]],
      sku: [null, [Validators.required]],
      imageUrls: [null],
      price: [null],
      sizePrices: this.formBuilder.array([]),
      unitId: [null, [Validators.required]],
      toppingIds: [null],
      optionAttributeIds: [null],
    });
    this.unitFilterForm = this.formBuilder.group({
      pageIndex: [1],
      pageSize: [20],
      name: [null],
    });
  }

  protected readonly FormType = FormType;

  addSizePriceControl() {
    this.form.patchValue({
      price: null,
    });
    if (this.sizePrices.length === 5) return;
    this.sizePrices.push(
      this.formBuilder.group({
        name: [null, Validators.required],
        price: [null, Validators.required],
      }),
    );
  }

  removeSizePrice(idx: number) {
    this.sizePrices.removeAt(idx);
  }

  removeAllSizePrice() {
    this.sizePrices.clear();
  }
}
