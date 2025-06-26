import { Component, effect, input, InputSignal, model, Signal } from '@angular/core';
import { SearchBar } from '../search-bar/search-bar';
import { Profile } from "../profile/profile";

@Component({
  selector: 'app-nav-bar',
  imports: [SearchBar, Profile],
  templateUrl: './nav-bar.html',
  styleUrl: './nav-bar.css',
})
export class NavBar {
  public isStudent = model();
  public isLoginForm = model();
  isHome: InputSignal<boolean | undefined> = input();

  constructor(){
    effect(() => {
      console.log({ishome:this.isHome()});
    });
  }
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
