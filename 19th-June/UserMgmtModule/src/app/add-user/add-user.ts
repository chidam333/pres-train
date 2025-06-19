import { Component } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, FormsModule, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { UserModel } from '../user-model';
import { UserList } from '../user-list/user-list';
import { BehaviorSubject } from 'rxjs';
import { UsersListS } from '../users-list-s/users-list-s';

function validatePassword(confirmPassword:string):ValidatorFn{
  return (control: AbstractControl): ValidationErrors| null => {
    const password = control.value;
    if(!(/[0-9]/.test(password)) || !(/[!@#$%^&*(),.?":{}|<>]/.test(password))) {
      return { 'weakPassword': "Password must contain number and special character" };
    }
    console.log({password, confirmPassword});
    return null
    // return password === confirmPassword ? null : { 'passwordMismatch': "Passwords do not match" };
  };
}

function validateUsername():ValidatorFn{
  return(control:AbstractControl) : ValidationErrors | null => {
    const username = control.value;
    if(username.toLowerCase() === 'admin' || username.toLowerCase() === 'root') {
      return { 'invalidUsername': "Username cannot be 'admin' or 'root'" };
    }
    return null;
  };
}

@Component({
  selector: 'app-add-user',
  imports: [FormsModule, ReactiveFormsModule, UsersListS],
  templateUrl: './add-user.html',
  styleUrl: './add-user.css'
})
export class AddUser {
  user: UserModel | null = null;
  users: BehaviorSubject<UserModel[]> = new BehaviorSubject<UserModel[]>([]);
  addForm: FormGroup;
  confirmPassword: string = '';
  formSubmitted: boolean = false;
  JSON = JSON;
  constructor() {
    this.addForm = new FormGroup({
      username: new FormControl('', [Validators.required, validateUsername()]),
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required, Validators.minLength(6), validatePassword(this.confirmPassword)]),
      role: new FormControl('', [Validators.required])
    });
  }
  
  public get username() : any {
    return this.addForm.get('username');
  }

  public get email() : any {
    return this.addForm.get('email');
  }

  public get password() : any {
    return this.addForm.get('password');
  }

  public get role() : any {
    return this.addForm.get('role');
  }
  logConfirmPassword() {
    console.log(this.confirmPassword);
  }
  handleSubmit(){
    if(this.addForm.valid) {
      const formValue = this.addForm.value;
      this.user = new UserModel(formValue.username, formValue.email, formValue.password, formValue.role);
      this.users.next([...this.users.getValue(), this.user]);
      console.log("User added successfully:", this.user);
      this.formSubmitted = true;
      this.addForm.reset();
    } else {
      console.log("Form is invalid");
    }
  }

}
