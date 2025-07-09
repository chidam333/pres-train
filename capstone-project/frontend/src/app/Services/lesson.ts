import { inject, Injectable } from '@angular/core';
import { AuthFetch } from '../auth/auth-service/auth-fetch';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class Lesson {
  authFetch = inject(AuthFetch);
  async addLesson(
    courseId: string,
    title: string,
    description: string,
    sequenceNo: number
  ): Promise<any | { error: string }> {
    let authToken = this.authFetch.getStoredToken();
    if (!authToken) {
      return { error: 'You are not authorized to add lesson' };
    }
    let response = await fetch(`${environment.apiUrl}/lesson`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${authToken}`,
      },
      body: JSON.stringify({
        title,
        description,
        sequenceNo,
        courseId
      }),
    });
    if (!response.ok) {
      let errorMessage = await response.text();
      return { error: errorMessage || 'Failed to add lesson' };
    }
    const data = await response.json();
    return data;
  }

  async getLessonsByCourseId(courseId: Number){
    let authToken = this.authFetch.getStoredToken();
    if (!authToken) {
      return { error: 'You are not authorized to get lessons' };
    }
    let response = await fetch(`${environment.apiUrl}/lesson/${courseId}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${authToken}`,
      },
    });
    if (!response.ok) {
      let errorMessage = await response.text();
      return { error: errorMessage || 'Failed to get lessons' };
    }
    const data = await response.json();
    if(!data['$values']) {
      return { error: 'No lessons found for this course' };
    }
    return data['$values'].sort((a: any, b: any) => a.sequenceNo - b.sequenceNo); 
  }
  async updateLesson(
    lessonId: number,
    title: string,
    description: string,
    courseId: number,
    sequenceNo: number
  ) {
    let authToken = this.authFetch.getStoredToken();
    if (!authToken) {
      return { error: 'You are not authorized to update lesson' };
    }
    let response = await fetch(`${environment.apiUrl}/lesson/${lessonId}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${authToken}`,
      },
      body: JSON.stringify({
        title,
        description,
        courseId,
        sequenceNo,
      }),
    });
    if (!response.ok) {
      let errorMessage = await response.text();
      return { error: errorMessage || 'Failed to update lesson' };
    }
    const data = await response.json();
    return data;
  }

  async deleteLesson(lessonId: number): Promise<any | { error: string }> {
    let authToken = this.authFetch.getStoredToken();
    if (!authToken) {
      return { error: 'You are not authorized to delete lesson' };
    }
    let response = await fetch(`${environment.apiUrl}/lesson/${lessonId}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${authToken}`,
      },
    });
    if (!response.ok) {
      let errorMessage = await response.text();
      return { error: errorMessage || 'Failed to delete lesson' };
    }
    return { message: 'Lesson deleted successfully' };
  }
}


