
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ContactUsService } from '../../services/contact-us.service';

@Component({
  selector: 'app-contact-us',
  template: `
    <h2 class="text-2xl font-bold mb-4">Contact Us</h2>
    <form [formGroup]="contactForm" (ngSubmit)="onSubmit()" class="space-y-4">
      <div>
        <label for="name" class="block text-sm font-medium text-gray-700">Name</label>
        <input id="name" formControlName="name" class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm">
      </div>
      <div>
        <label for="email" class="block text-sm font-medium text-gray-700">Email</label>
        <input id="email" formControlName="email" class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm">
      </div>
      <div>
        <label for="message" class="block text-sm font-medium text-gray-700">Message</label>
        <textarea id="message" formControlName="message" rows="4" class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"></textarea>
      </div>
      <button type="submit" class="bg-blue-500 text-white px-4 py-2 rounded">Send</button>
    </form>
  `,
  imports: [ReactiveFormsModule]
})
export class ContactUsComponent {
  fb = inject(FormBuilder);
  contactUsService = inject(ContactUsService);

  contactForm = this.fb.group({
    name: '',
    email: '',
    message: ''
  });

  onSubmit() {
    const { name, email, message } = this.contactForm.value;
    this.contactUsService.send({
      name: name ?? '',
      email: email ?? '',
      message: message ?? ''
    }).subscribe(() => {
      this.contactForm.reset();
    });
  }
}
