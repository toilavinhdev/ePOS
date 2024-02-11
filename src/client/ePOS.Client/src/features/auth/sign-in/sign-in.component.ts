import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { Store } from '@ngrx/store';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { AsyncPipe } from '@angular/common';
import { ISignInRequest } from '@app-shared/models/user.models';
import { Observable, takeUntil } from 'rxjs';
import { signIn, userLoadingSelector } from '@app-shared/store/user';
import { BaseComponent } from '@app-shared/core/abtractions';
import { RouterLink } from '@angular/router';
import { PasswordModule } from 'primeng/password';
import { emailRegex } from '@app-shared/utilities';

@Component({
  selector: 'app-sign-in',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    InputTextModule,
    ButtonModule,
    AsyncPipe,
    RouterLink,
    PasswordModule,
  ],
  templateUrl: './sign-in.component.html',
  styles: ``,
})
export class SignInComponent extends BaseComponent implements OnInit {
  form!: UntypedFormGroup;
  loading$!: Observable<boolean>;

  constructor(
    private formBuilder: FormBuilder,
    private store: Store,
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
      email: [
        'adminepos@gmail.com',
        [Validators.required, Validators.pattern(emailRegex)],
      ],
      password: ['Admin@123', [Validators.required, Validators.minLength(6)]],
    });
  }

  onSignIn() {
    if (this.form.invalid) return;
    this.store.dispatch(signIn({ payload: this.form.value as ISignInRequest }));
  }
}
