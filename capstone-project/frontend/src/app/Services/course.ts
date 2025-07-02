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
  async getCourses(): Promise<any> {
  let auth_token = this.authFetch.getStoredToken();
  const response = await fetch(`${environment.apiUrl}/course`, {
    method: 'GET',
    headers: {
    'Content-Type': 'application/json',
    Authorization: `Bearer ${auth_token}`,
    },
  });
  if (!response.ok) {
    const errorData = await response.text();
    return { error: errorData || 'Failed to fetch courses' };
  }
  const data = await response.json();
  if(!data["$values"]){
	return {error: "no value found"}
  }
  return data["$values"];
  }

  async getCourseByInstructorEmail(): Promise<any | { error: string }> {
    let auth_token = this.authFetch.getStoredToken();
    const response = await fetch(`${environment.apiUrl}/course/instructor/`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${auth_token}`,
      },
    });
    if (!response.ok) {
      const errorData = await response.text();
      return { error: errorData || 'Failed to fetch courses by instructor' };
    }
    const data = await response.json();
    if (!data || data.length === 0) {
      return { error: 'No courses found for this instructor' };
    }
    return data;
  }

  async getCourseById(id: number): Promise<any | { error: string }> {
    let auth_token = this.authFetch.getStoredToken();
    const response = await fetch(`${environment.apiUrl}/course/${id}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${auth_token}`,
      },
    });
    if (!response.ok) {
      const errorData = await response.text();
      return { error: errorData || 'Failed to fetch course' };
    }
    const data = await response.json();
    if (!data) {
      return { error: 'Course not found' };
    }
    return data;
  }

  async updateCourse(
    id: number,
    title: string,
    description: string,
    thumbnail: string
  ): Promise<any | { error: string }> {
    let auth_token = this.authFetch.getStoredToken();
    const response = await fetch(`${environment.apiUrl}/course/${id}`, {
      method: 'PUT',
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
      return { error: errorData || 'Course update failed' };
    }
    return await response.json();
  }

  async getOtherCourses(): Promise<any[] | { error: string }> {
    let auth_token = this.authFetch.getStoredToken();
    const response = await fetch(`${environment.apiUrl}/course/others`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${auth_token}`,
      },
    });
    if (!response.ok) {
      const errorData = await response.text();
      return { error: errorData || 'Failed to fetch other courses' };
    }
    const data = await response.json();
    if (!data['$values']) {
      return { error: 'no value found' };
    }
    return data['$values'];
  }

  async getMyCourses(): Promise<any[] | { error: string }> {
    let auth_token = this.authFetch.getStoredToken();
    const response = await fetch(`${environment.apiUrl}/enrollment/my-courses`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${auth_token}`,
      },
    });
    if (!response.ok) {
      const errorData = await response.text();
      return { error: errorData || 'Failed to fetch my courses' };
    }
    const data = await response.json();
    if (!data['$values']) {
      return { error: 'no value found' };
    }
    return data['$values'];
  }

  async unenrollCourse(courseId: number): Promise<any | { error: string }> {
    let auth_token = this.authFetch.getStoredToken();
    const response = await fetch(`${environment.apiUrl}/enrollment/`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${auth_token}`,
      },
      body: JSON.stringify({ courseId }),
    });
    if (!response.ok) {
      const errorData = await response.text();
      return { error: errorData || 'Unenrollment failed' };
    }
    const data = await response.json();
    console.log({data})
    return data;
  }

  async getCourseDetailsById(
    courseId: number
  ): Promise<any | { error: string }> {
    let auth_token = this.authFetch.getStoredToken();
    const response = await fetch(
      `${environment.apiUrl}/course/details/${courseId}`,
      {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${auth_token}`,
        },
      }
    );
    if (!response.ok) {
      const errorData = await response.text();
      return { error: errorData || 'Failed to fetch course details' };
    }
    const data = await response.json();
    console.log({data})
    return data;
  }

  async deleteCourse(courseId: number): Promise<any | { error: string }> {
    let auth_token = this.authFetch.getStoredToken();
    const response = await fetch(`${environment.apiUrl}/course/${courseId}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${auth_token}`,
      },
    });
    if (!response.ok) {
      const errorData = await response.text();
      return { error: errorData || 'Course deletion failed' };
    }
    return await response.json();
  }
}
