import { Component, inject, input } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CustomValidator } from '../Services/custom-validator';
import { FormErrorToast } from "../form-error-toast/form-error-toast";

@Component({
  selector: 'app-login-form',
  imports: [ReactiveFormsModule, FormErrorToast],
  templateUrl: './login-form.html',
  styleUrl: './login-form.css',
})
export class LoginForm {
  public fb = new FormBuilder();
  public forRole = input.required();
  private customValidator = inject(CustomValidator);
  public loginForm = this.fb.group({
    email: ['',[Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6), this.customValidator.checkForSymbolAndNumber()]],
  });
  constructor() {}
  onSubmit() {}
}
