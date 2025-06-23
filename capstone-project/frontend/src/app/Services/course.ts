import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { AuthFetch } from '../auth/auth-service/auth-fetch';
@Injectable({
  providedIn: 'root',
})
export class Course {
  authFetch = inject(AuthFetch);
  
  async createCourse(
    title: string,
    description: string,
    thumbnail: string
  ): Promise<any | { error: string }> {
    let auth_token = this.authFetch.getStoredToken();
    const response = await fetch(`${environment.apiUrl}/course`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${auth_token}`,
      },
      body: JSON.stringify({
        title,
        description,
        thumbnail,
      }),
    });
    if (!response.ok) {
      const errorData = await response.text();
      return { error: errorData || 'Course creation failed' };
    }
    return await response.json();
  }
}
