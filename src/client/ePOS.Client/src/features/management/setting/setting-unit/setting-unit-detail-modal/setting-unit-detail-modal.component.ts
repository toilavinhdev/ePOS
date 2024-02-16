import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@app-shared/core/abtractions';
import {
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { Store } from '@ngrx/store';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { AsyncPipe, JsonPipe } from '@angular/common';
import { FormType } from '@app-shared/core/models/form.models';
import { NoWhitespaceValidator } from '@app-shared/validators';
import {
  createUnit,
  unitLoadingCreateOrUpdateSelector,
  updateUnit,
} from '@app-shared/store/unit';
import {
  ICreateUnitRequest,
  IUnitViewModel,
  IUpdateUnitRequest,
} from '@app-shared/models/unit.models';
import { Observable, takeUntil } from 'rxjs';

@Component({
  selector: 'app-setting-unit-detail-modal',
  standalone: true,
  imports: [
    DialogModule,
    ReactiveFormsModule,
    InputTextModule,
    ButtonModule,
    JsonPipe,
    AsyncPipe,
  ],
  templateUrl: './setting-unit-detail-modal.component.html',
  styles: ``,
})
export class SettingUnitDetailModalComponent
  extends BaseComponent
  implements OnInit
{
  protected readonly FormType = FormType;
  visible = false;
  currentFormType: FormType = FormType.Undefined;
  form!: UntypedFormGroup;
  loadingCreateOrUpdate$!: Observable<boolean>;

  constructor(
    private formBuilder: UntypedFormBuilder,
    private store: Store,
  ) {
    super();
  }

  ngOnInit() {
    this.buildForm();
    this.loadingCreateOrUpdate$ = this.store
      .select(unitLoadingCreateOrUpdateSelector)
      .pipe(takeUntil(this.destroy$));
  }

  onSubmit() {
    if (this.form.invalid) return;
    if (this.currentFormType === FormType.Create) {
      this.store.dispatch(
        createUnit({ payload: this.form.value as ICreateUnitRequest }),
      );
    } else if (this.currentFormType === FormType.Update) {
      this.store.dispatch(
        updateUnit({ payload: this.form.value as IUpdateUnitRequest }),
      );
    }
  }

  showCreateModal() {
    this.currentFormType = FormType.Create;
    this.visible = true;
  }

  showUpdateModal(record: IUnitViewModel) {
    this.currentFormType = FormType.Update;
    this.visible = true;
    this.form.patchValue({ ...record });
  }

  hideModal() {
    this.visible = false;
    this.currentFormType = FormType.Undefined;
    this.form.reset();
  }

  private buildForm() {
    this.form = this.formBuilder.group({
      id: [null],
      name: [null, [Validators.required, NoWhitespaceValidator()]],
    });
  }
}
