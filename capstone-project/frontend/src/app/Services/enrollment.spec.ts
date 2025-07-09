import { TestBed } from '@angular/core/testing';
import { provideZonelessChangeDetection } from '@angular/core';

import { Enrollment } from './enrollment';
import { AuthFetch } from '../auth/auth-service/auth-fetch';

describe('Enrollment Service', () => {
  let service: Enrollment;
  let mockAuthFetch: jasmine.SpyObj<AuthFetch>;

  beforeEach(() => {
    mockAuthFetch = jasmine.createSpyObj('AuthFetch', ['getStoredToken']);
    mockAuthFetch.getStoredToken.and.returnValue('fake-token');

    TestBed.configureTestingModule({
      providers: [
        provideZonelessChangeDetection(),
        Enrollment,
        { provide: AuthFetch, useValue: mockAuthFetch }
      ]
    });
    service = TestBed.inject(Enrollment);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should handle enrollment failure', async () => {
    spyOn(window, 'fetch').and.returnValue(Promise.resolve(new Response(
      'Already enrolled',
      { status: 400 }
    )));

    const result = await service.enrollCourse(1);

    expect(result.error).toContain('Already enrolled');
  });

  it('should handle service initialization', () => {
    expect(service).toBeTruthy();
    expect(service['apiUrl']).toBeDefined();
  });
});
