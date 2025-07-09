import { TestBed } from '@angular/core/testing';
import { provideZonelessChangeDetection } from '@angular/core';

import { Lesson } from './lesson';
import { AuthFetch } from '../auth/auth-service/auth-fetch';

describe('Lesson Service', () => {
  let service: Lesson;
  let mockAuthFetch: jasmine.SpyObj<AuthFetch>;

  beforeEach(() => {
    mockAuthFetch = jasmine.createSpyObj('AuthFetch', ['getStoredToken']);
    mockAuthFetch.getStoredToken.and.returnValue('fake-token');

    TestBed.configureTestingModule({
      providers: [
        provideZonelessChangeDetection(),
        Lesson,
        { provide: AuthFetch, useValue: mockAuthFetch }
      ]
    });
    service = TestBed.inject(Lesson);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should handle get lessons failure', async () => {
    spyOn(window, 'fetch').and.returnValue(Promise.resolve(new Response(
      'Failed to fetch lessons',
      { status: 404 }
    )));

    const result = await service.getLessonsByCourseId(1);

    expect(result).toEqual({ error: 'Failed to fetch lessons' });
  });

  it('should add lesson successfully', async () => {
    const mockLesson = { id: 1, title: 'New Lesson', description: 'Description' };
    spyOn(window, 'fetch').and.returnValue(Promise.resolve(new Response(
      JSON.stringify(mockLesson),
      { status: 201 }
    )));

    const result = await service.addLesson('1', 'New Lesson', 'Description', 1);

    expect(window.fetch).toHaveBeenCalledWith(
      jasmine.stringContaining('/lesson'),
      jasmine.objectContaining({
        method: 'POST',
        headers: jasmine.objectContaining({
          'Content-Type': 'application/json',
          'Authorization': 'Bearer fake-token'
        }),
        body: JSON.stringify({
          title: 'New Lesson',
          description: 'Description',
          sequenceNo: 1,
          courseId: '1'
        })
      })
    );
    expect(result).toEqual(mockLesson);
  });

  it('should handle add lesson failure', async () => {
    spyOn(window, 'fetch').and.returnValue(Promise.resolve(new Response(
      'Lesson creation failed',
      { status: 400 }
    )));

    const result = await service.addLesson('1', 'New Lesson', 'Description', 1);

    expect(result).toEqual({ error: 'Lesson creation failed' });
  });


  it('should handle delete lesson failure', async () => {
    spyOn(window, 'fetch').and.returnValue(Promise.resolve(new Response(
      'Delete failed',
      { status: 404 }
    )));

    const result = await service.deleteLesson(1);

    expect(result).toEqual({ error: 'Delete failed' });
  });
});
