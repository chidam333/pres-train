import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";
import { Component } from '@angular/core';

export function textValidator():ValidatorFn
{
  return(control:AbstractControl):ValidationErrors|null=>{
    const value = control.value;
    if(value?.length<6)
      return {lenError:'password is of worng length'}
    return null;
    
  }
}



@Component({
  selector: 'app-add-user-form',
  imports: [],
  templateUrl: './add-user-form.html',
  styleUrl: './add-user-form.css'
})
export class AddUserForm {

}
