import { Component, computed, input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-form-error-toast',
  imports: [],
  templateUrl: './form-error-toast.html',
  styleUrl: './form-error-toast.css'
})
export class FormErrorToast implements OnInit{
  public fields = input.required<FormGroup>();
  controls = computed(() => {
    return Object.entries(this.fields().controls);
  });
  ngOnInit(): void {
    console.log(this.fields())
    console.log(this.controls(),typeof(this.controls()));
  }
  stringifyErrors(errors: any): string {
    return JSON.stringify(errors);
  }
}
