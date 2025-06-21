import { Component, model } from '@angular/core';
import { SearchBar } from '../search-bar/search-bar';

@Component({
  selector: 'app-nav-bar',
  imports: [SearchBar],
  templateUrl: './nav-bar.html',
  styleUrl: './nav-bar.css',
})
export class NavBar {
  public isStudent = model.required();
  public isLoginForm = model.required();
  toggleRole() {
    this.isStudent.set(!this.isStudent());
  }
  setLoginForm() {
    this.isLoginForm.set(true);
  }
  setSignUpForm() {
    this.isLoginForm.set(false);
  }
}
