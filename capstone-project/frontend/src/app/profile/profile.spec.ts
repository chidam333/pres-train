import { ComponentFixture, TestBed } from '@angular/core/testing';
import { signal } from '@angular/core';
import { provideZonelessChangeDetection } from '@angular/core';

import { Profile } from './profile';
import { AuthFetch } from '../auth/auth-service/auth-fetch';
import { Router } from '@angular/router';

describe('Profile Component', () => {
  let component: Profile;
  let fixture: ComponentFixture<Profile>;
  let mockAuthFetch: jasmine.SpyObj<AuthFetch>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    mockAuthFetch = jasmine.createSpyObj('AuthFetch', [
      'getUniqueName',
      'getUserEmail', 
      'getUserRole',
      'logout'
    ]);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);

    mockAuthFetch.getUniqueName.and.returnValue('John Doe');
    mockAuthFetch.getUserEmail.and.returnValue('john@example.com');
    mockAuthFetch.getUserRole.and.returnValue('student');

    await TestBed.configureTestingModule({
      imports: [Profile],
      providers: [
        provideZonelessChangeDetection(),
        { provide: AuthFetch, useValue: mockAuthFetch },
        { provide: Router, useValue: mockRouter }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Profile);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create and initialize user data', () => {
    expect(component).toBeTruthy();
    expect(component.user).toBe('John Doe');
    expect(component.email).toBe('john@example.com');
    expect(component.role).toBe('student');
    expect(component.firstchar).toBe('J');
  });

  it('should set first character correctly', () => {
    expect(component.firstchar).toBe('J');
  });

  it('should handle logout', () => {
    component.logout();
    
    expect(mockAuthFetch.logout).toHaveBeenCalled();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/auth']);
  });

  it('should handle case when user is not string', () => {
    mockAuthFetch.getUniqueName.and.returnValue({ error: 'Not found' } as any);
    
    TestBed.resetTestingModule();
    TestBed.configureTestingModule({
      imports: [Profile],
      providers: [
        provideZonelessChangeDetection(),
        { provide: AuthFetch, useValue: mockAuthFetch },
        { provide: Router, useValue: mockRouter }
      ]
    });

    fixture = TestBed.createComponent(Profile);
    component = fixture.componentInstance;
    
    expect(component.firstchar).toBe('');
  });
});
