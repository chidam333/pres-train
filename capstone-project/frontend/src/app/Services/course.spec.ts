import { TestBed } from '@angular/core/testing';
import { provideZonelessChangeDetection } from '@angular/core';

import { Course } from './course';
import { AuthFetch } from '../auth/auth-service/auth-fetch';

describe('Course Service', () => {
  let service: Course;
  let mockAuthFetch: jasmine.SpyObj<AuthFetch>;

  beforeEach(() => {
    mockAuthFetch = jasmine.createSpyObj('AuthFetch', ['getStoredToken']);
    mockAuthFetch.getStoredToken.and.returnValue('fake-token');

    TestBed.configureTestingModule({
      providers: [
        provideZonelessChangeDetection(),
        Course,
        { provide: AuthFetch, useValue: mockAuthFetch }
      ]
    });
    service = TestBed.inject(Course);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should create course with valid data', async () => {
    spyOn(window, 'fetch').and.returnValue(Promise.resolve(new Response(
      JSON.stringify({ id: 1, title: 'Test Course' }),
      { status: 200 }
    )));

    const result = await service.createCourse('Test Course', 'Description', 'thumbnail.jpg');

    expect(window.fetch).toHaveBeenCalledWith(
      jasmine.stringContaining('/course'),
      jasmine.objectContaining({
        method: 'POST',
        headers: jasmine.objectContaining({
          'Content-Type': 'application/json',
          'Authorization': 'Bearer fake-token'
        }),
        body: JSON.stringify({
          title: 'Test Course',
          description: 'Description',
          thumbnail: 'thumbnail.jpg'
        })
      })
    );
    expect(result.id).toBe(1);
  });

  it('should handle course creation failure', async () => {
    spyOn(window, 'fetch').and.returnValue(Promise.resolve(new Response(
      'Course creation failed',
      { status: 400 }
    )));

    const result = await service.createCourse('Test Course', 'Description', 'thumbnail.jpg');

    expect(result).toEqual({ error: 'Course creation failed' });
  });

  it('should get courses successfully', async () => {
    const mockCourses = { '$values': [{ id: 1, title: 'Course 1' }, { id: 2, title: 'Course 2' }] };
    spyOn(window, 'fetch').and.returnValue(Promise.resolve(new Response(
      JSON.stringify(mockCourses),
      { status: 200 }
    )));

    const result = await service.getCourses();

    expect(window.fetch).toHaveBeenCalledWith(
      jasmine.stringContaining('/course'),
      jasmine.objectContaining({
        method: 'GET',
        headers: jasmine.objectContaining({
          'Authorization': 'Bearer fake-token'
        })
      })
    );
    expect(result).toEqual(mockCourses['$values']);
  });

  it('should handle get courses when no values found', async () => {
    spyOn(window, 'fetch').and.returnValue(Promise.resolve(new Response(
      JSON.stringify({}),
      { status: 200 }
    )));

    const result = await service.getCourses();

    expect(result).toEqual({ error: 'no value found' });
  });

  it('should get course by ID successfully', async () => {
    const mockCourse = { id: 1, title: 'Test Course', description: 'Test Description' };
    spyOn(window, 'fetch').and.returnValue(Promise.resolve(new Response(
      JSON.stringify(mockCourse),
      { status: 200 }
    )));

    const result = await service.getCourseById(1);

    expect(window.fetch).toHaveBeenCalledWith(
      jasmine.stringContaining('/course/1'),
      jasmine.objectContaining({
        method: 'GET'
      })
    );
    expect(result).toEqual(mockCourse);
  });
});
