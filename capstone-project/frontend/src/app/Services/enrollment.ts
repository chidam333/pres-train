import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { AuthFetch } from '../auth/auth-service/auth-fetch';
@Injectable({
  providedIn: 'root',
})
export class Enrollment {
  private apiUrl: string;
  authFetch = inject(AuthFetch);

  constructor() {
    this.apiUrl = environment.apiUrl;
  }

  async enrollCourse(courseId: number): Promise<any | { error: string }> {
    const token = await this.authFetch.getStoredToken();
    const response = await fetch(`${this.apiUrl}/enrollment`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      },
      body: JSON.stringify({ courseId }),
    });
    if (!response.ok) {
      const errorMsg = await response.text();
      return { error: `Enrollment failed: ${errorMsg}` };
    }
    const data = await response.json();
    return data;
  }
}
