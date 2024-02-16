import { Component, OnInit, ViewChild } from '@angular/core';
import { UploadImageComponent } from '@app-shared/components/upload-image/upload-image.component';
import { InputTextModule } from 'primeng/inputtext';
import {
  FormBuilder,
  ReactiveFormsModule,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { BaseComponent } from '@app-shared/core/abtractions';
import { ButtonModule } from 'primeng/button';
import { Store } from '@ngrx/store';
import {
  getTenant,
  tenantLoadingGetSelector,
  tenantLoadingUpdateSelector,
  tenantSelector,
  updateTenant,
} from '@app-shared/store/tenant';
import { SectionContentComponent } from '@app-shared/components';
import { Observable, takeUntil } from 'rxjs';
import { AsyncPipe, DatePipe } from '@angular/common';
import {
  ITenantViewModel,
  IUpdateTenantRequest,
} from '@app-shared/models/tenant.models';
import { NotificationService, StorageService } from '@app-shared/services';

@Component({
  selector: 'app-setting-tenant',
  standalone: true,
  imports: [
    UploadImageComponent,
    InputTextModule,
    ReactiveFormsModule,
    ButtonModule,
    SectionContentComponent,
    AsyncPipe,
    DatePipe,
  ],
  templateUrl: './setting-tenant.component.html',
  styles: ``,
  providers: [DatePipe],
})
export class SettingTenantComponent extends BaseComponent implements OnInit {
  form!: UntypedFormGroup;
  formIsIdle = true;
  loadingGet!: Observable<boolean>;
  loadingUpdate!: Observable<boolean>;
  tenant!: Observable<ITenantViewModel | undefined>;

  @ViewChild('uploadImageComponent')
  uploadImageComponent!: UploadImageComponent;

  constructor(
    private formBuilder: FormBuilder,
    private store: Store,
    private datePipe: DatePipe,
    private storageService: StorageService,
    private notificationService: NotificationService,
  ) {
    super();
    this.store.dispatch(getTenant());
  }

  ngOnInit() {
    this.loadingGet = this.store
      .select(tenantLoadingGetSelector)
      .pipe(takeUntil(this.destroy$));
    this.loadingUpdate = this.store
      .select(tenantLoadingUpdateSelector)
      .pipe(takeUntil(this.destroy$));
    this.tenant = this.store
      .select(tenantSelector)
      .pipe(takeUntil(this.destroy$));
    this.buildForm();
    this.patchData();
    this.form.valueChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => (this.formIsIdle = false));
  }

  onUpdateTenant() {
    if (this.form.invalid) return;
    let payload = this.form.value as IUpdateTenantRequest;
    const logoFile = this.uploadImageComponent.file;

    if (logoFile === undefined) {
      this.store.dispatch(
        updateTenant({
          payload: payload,
        }),
      );
    } else {
      this.storageService
        .upload(logoFile)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (url) => {
            payload.logoUrl = url;
            this.store.dispatch(
              updateTenant({
                payload: payload,
              }),
            );
          },
          error: () => {
            this.notificationService.error('Có lỗi xảy ra khi upload ảnh');
          },
        });
    }
  }

  private patchData() {
    this.tenant.subscribe((data) => {
      this.form.patchValue({
        ...data,
        createdAt: this.datePipe.transform(data?.createdAt, 'dd/MM/yyyy'),
      });
      this.formIsIdle = true;
    });
  }

  private buildForm() {
    this.form = this.formBuilder.group({
      id: [{ value: null, disabled: true }, [Validators.required]],
      code: [null, [Validators.required]],
      name: [null, [Validators.required]],
      logoUrl: [null],
      taxId: [null],
      companyName: [null],
      companyAddress: [null],
      createdAt: [{ value: null, disabled: true }],
    });
  }
}
