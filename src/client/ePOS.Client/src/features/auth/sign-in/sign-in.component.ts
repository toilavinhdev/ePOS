import { Component, OnInit } from '@angular/core';
import { FormBuilder, UntypedFormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-sign-in',
  standalone: true,
  imports: [],
  templateUrl: './sign-in.component.html',
  styles: ``,
})
export class SignInComponent implements OnInit {
  form!: UntypedFormGroup;

  constructor(private formBuilder: FormBuilder) {}

  ngOnInit() {}

  private buildForm() {
    this.form = this.formBuilder.group({
      email: [null, [Validators.required]],
      password: [null, [Validators.required, Validators.minLength(6)]],
    });
  }
}
