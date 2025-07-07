import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home {
  formGroup: FormGroup;

  constructor(private formBuilder: FormBuilder) {
    this.formGroup = this.formBuilder.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      contact: ['', [Validators.required, Validators.pattern(/^[6-9]\d{9}$/)]],
      amount: ['', [Validators.required, Validators.min(1)]]
    });
  }

  pay() {
    if (this.formGroup.invalid) {
      alert('Please fill out the form correctly.');
      return;
    }

    const form = this.formGroup.value;

    const options: any = {
      key: environment.key_id,
      amount: form.amount * 100, 
      currency: 'INR',
      name: form.name,
      description: 'Custom Payment',
      image: 'https://your-logo-url.com/logo.png',
      handler: function (response: any) {
        console.log({response})
        alert('Payment successful!');
      },
      prefill: {
        name: form.name,
        email: form.email,
        contact: form.contact
      },
      notes: {
        purpose: 'payment simulation'
      },
      theme: {
        color: '#007bff'
      }
    };

    const rzp = new (window as any).Razorpay(options);
    rzp.open();
  }
}
