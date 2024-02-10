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

@Component({
  selector: 'app-sign-in',
  standalone: true,
  imports: [ReactiveFormsModule, InputTextModule, ButtonModule, AsyncPipe],
  templateUrl: './sign-in.component.html',
  styles: ``,
})
export class SignInComponent implements OnInit {
  form!: UntypedFormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private store: Store,
  ) {}

  ngOnInit() {
    this.buildForm();
  }

  private buildForm() {
    this.form = this.formBuilder.group({
      email: [null, [Validators.required]],
      password: [null, [Validators.required, Validators.minLength(6)]],
    });
  }

  onSignIn() {
    if (this.form.invalid) return;
  }
}
