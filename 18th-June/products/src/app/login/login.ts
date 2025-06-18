import { Component, signal } from '@angular/core';

@Component({
  selector: 'app-login',
  imports: [],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  isTrue = signal(true);

  setLoginStatus(status: boolean) {
    this.isTrue.set(status);
    localStorage.setItem("token", status ? "true" : "false");
    console.log('Login status:', status);
  }
}
