import { inject, Injectable } from '@angular/core';
import { AuthFetch } from '../auth/auth-service/auth-fetch';

@Injectable({
  providedIn: 'root',
})
export class Material {
  authFetch = inject(AuthFetch);
  async uploadMaterial(
    lessonId: number,
    title: string,
    sequenceNo: number,
    file: File
  ): Promise<any | { error: string }> {
    const authToken = this.authFetch.getStoredToken();

    const formData = new FormData();
    formData.append('File', file, file.name);

    const response = await fetch(
      `http://localhost:5243/api/v1/material/upload?Title=${title}&LessonId=${lessonId}&SequenceNo=${sequenceNo}`,
      {
        method: 'POST',
        headers: {
          // 'Content-Type': 'multipart/form-data',
          'Authorization': `Bearer ${authToken}`,
        },
        body: formData,
      }
    );

    if (!response.ok) {
      const errorMessage = await response.text();
      return { error: errorMessage || 'Failed to upload material' };
    }
    const data = await response.json();
    if (response.ok) {
      return { success: true, id: data.id };
    }
  }
}
