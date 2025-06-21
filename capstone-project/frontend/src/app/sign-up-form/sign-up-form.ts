import { Component, inject, input } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CustomValidator } from '../Services/custom-validator';
import { FormErrorToast } from "../form-error-toast/form-error-toast";

@Component({
  selector: 'app-sign-up-form',
  imports: [ReactiveFormsModule, FormErrorToast],
  templateUrl: './sign-up-form.html',
  styleUrl: './sign-up-form.css',
})
export class SignUpForm {
  forRole = input.required();
  fb = new FormBuilder();
  customValidator = inject(CustomValidator);
  signupForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6), this.customValidator.checkForSymbolAndNumber()]],
    confirmPassword: ['', [Validators.required, Validators.minLength(6), this.customValidator.checkForSymbolAndNumber()]],
  }, { validators: this.customValidator.passwordMatchValidator() });
  onSubmit() {}
}
