import { Component, OnInit } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import {
  FormBuilder,
  FormsModule,
  ReactiveFormsModule,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { BaseComponent } from '@app-shared/core/abtractions';
import { emailRegex } from '@app-shared/utilities';
import { RouterLink } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable, takeUntil } from 'rxjs';
import { signUp, userLoadingSelector } from '@app-shared/store/user';
import { NotificationService } from '@app-shared/services';
import { ISignUpRequest } from '@app-shared/models/user.models';

@Component({
  selector: 'app-sign-up',
  standalone: true,
  imports: [
    AsyncPipe,
    ButtonModule,
    FormsModule,
    InputTextModule,
    PasswordModule,
    ReactiveFormsModule,
    RouterLink,
  ],
  templateUrl: './sign-up.component.html',
  styles: ``,
})
export class SignUpComponent extends BaseComponent implements OnInit {
  form!: UntypedFormGroup;
  loading$!: Observable<boolean>;

  constructor(
    private formBuilder: FormBuilder,
    private store: Store,
    private notificationService: NotificationService,
  ) {
    super();
    this.loading$ = this.store
      .select(userLoadingSelector)
      .pipe(takeUntil(this.destroy$));
  }

  ngOnInit() {
    this.buildForm();
  }

  private buildForm() {
    this.form = this.formBuilder.group({
      fullName: [null, Validators.required],
      tenantName: [null, Validators.required],
      email: [null, [Validators.required, Validators.pattern(emailRegex)]],
      password: ['Admin@123', [Validators.required, Validators.minLength(6)]],
      confirmPassword: [
        'Admin@123',
        [Validators.required, Validators.minLength(6)],
      ],
    });
  }

  onSignUp() {
    if (this.form.invalid) return;
    if (
      this.form.get('password')?.value !==
      this.form.get('confirmPassword')?.value
    ) {
      this.notificationService.warning('Mật khẩu nhập lại không khớp');
      return;
    }
    this.store.dispatch(signUp({ payload: this.form.value as ISignUpRequest }));
  }
}
