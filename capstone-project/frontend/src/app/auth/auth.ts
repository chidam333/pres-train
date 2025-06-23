import { Component, computed, signal } from '@angular/core';
import { NavBar } from '../nav-bar/nav-bar';
import { LoginForm } from "../login-form/login-form";
import { SignUpForm } from "../sign-up-form/sign-up-form";
import { FormErrorToast } from "../form-error-toast/form-error-toast";

@Component({
  selector: 'app-auth',
  imports: [NavBar, LoginForm, SignUpForm],
  templateUrl: './auth.html',
  styleUrl: './auth.css',
})
export class Auth {
  isStudent = signal(true);
  isLoginForm = signal(true);
  role = computed(() => (this.isStudent() ? 'student' : 'instructor'));
}

