import { Component, OnInit } from '@angular/core';
import { UploadImageComponent } from '@app-shared/components/upload-image/upload-image.component';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import {
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormControl,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { emailRegex } from '@app-shared/utilities';
import { Store } from '@ngrx/store';
import { userProfileSelector } from '@app-shared/store/user';

@Component({
  selector: 'app-profile-detail',
  standalone: true,
  imports: [
    UploadImageComponent,
    InputTextModule,
    ButtonModule,
    ReactiveFormsModule,
  ],
  templateUrl: './profile-detail.component.html',
  styles: ``,
})
export class ProfileDetailComponent implements OnInit {
  profileForm!: UntypedFormGroup;

  get avatarUrl(): UntypedFormControl {
    return this.profileForm.get('avatarUrl') as UntypedFormControl;
  }

  constructor(
    private formBuilder: UntypedFormBuilder,
    private store: Store,
  ) {}

  ngOnInit() {
    this.buildForm();
    this.patchProfileData();
  }

  private buildForm() {
    this.profileForm = this.formBuilder.group({
      id: [null, [Validators.required]],
      fullName: [null, [Validators.required]],
      email: [null, [Validators.required, Validators.pattern(emailRegex)]],
      avatarUrl: [null],
    });
  }

  private patchProfileData() {
    this.store.select(userProfileSelector).subscribe((data) => {
      this.profileForm.patchValue({
        id: data?.id,
        fullName: data?.fullName,
        email: data?.email,
        avatarUrl: data?.avatarUrl,
      });
    });
  }
}
