
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ContactUs } from '../models/contact-us.model';

@Injectable({
  providedIn: 'root'
})
export class ContactUsService {
  private http = inject(HttpClient);
  private baseUrl = 'http://localhost:5127/api/ContactUs';

  send(message: ContactUs) {
    return this.http.post(this.baseUrl, message);
  }
}
