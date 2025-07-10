import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { signal } from '@angular/core';
import { provideZonelessChangeDetection } from '@angular/core';

import { LoginForm } from './login-form';
import { AuthFetch } from '../auth/auth-service/auth-fetch';
import { CustomValidator } from '../Services/custom-validator';

describe('LoginForm Component', () => {
  let component: LoginForm;
  let fixture: ComponentFixture<LoginForm>;
  let mockAuthFetch: jasmine.SpyObj<AuthFetch>;
  let mockRouter: jasmine.SpyObj<Router>;
  let mockCustomValidator: jasmine.SpyObj<CustomValidator>;

  beforeEach(async () => {
    mockAuthFetch = jasmine.createSpyObj('AuthFetch', ['login']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate', 'parseUrl']);
    mockCustomValidator = jasmine.createSpyObj('CustomValidator', ['checkForSymbolAndNumber']);

    mockRouter.parseUrl.and.returnValue({ queryParams: {} } as any);
    mockCustomValidator.checkForSymbolAndNumber.and.returnValue(() => null);

    await TestBed.configureTestingModule({
      imports: [LoginForm, ReactiveFormsModule],
      providers: [
        provideZonelessChangeDetection(),
        FormBuilder,
        { provide: AuthFetch, useValue: mockAuthFetch },
        { provide: Router, useValue: mockRouter },
        { provide: CustomValidator, useValue: mockCustomValidator }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(LoginForm);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('forRole', 'student');
    fixture.detectChanges();
  });

  it('should initialize with empty form', () => {
    expect(component.loginForm.get('email')?.value).toBe('');
    expect(component.loginForm.get('password')?.value).toBe('');
    expect(component.loginForm.invalid).toBeTruthy();
  });

  it('should validate email field correctly', () => {
    const emailControl = component.loginForm.get('email');
    
    emailControl?.setValue('');
    expect(emailControl?.hasError('required')).toBeTruthy();
    
    emailControl?.setValue('invalid-email');
    expect(emailControl?.hasError('email')).toBeTruthy();
    
    emailControl?.setValue('test@example.com');
    expect(emailControl?.valid).toBeTruthy();
  });

  it('should validate password field correctly', () => {
    const passwordControl = component.loginForm.get('password');
    
    passwordControl?.setValue('');
    expect(passwordControl?.hasError('required')).toBeTruthy();
    
    passwordControl?.setValue('123');
    expect(passwordControl?.hasError('minlength')).toBeTruthy();
    
    passwordControl?.setValue('password123@');
    expect(passwordControl?.valid).toBeTruthy();
  });

  it('should handle login failure', async () => {
    mockAuthFetch.login.and.returnValue(Promise.resolve({ error: 'Invalid credentials' }));
    
    component.loginForm.patchValue({
      email: 'test@example.com',
      password: 'wrongpassword'
    });

    await component.onSubmit();

    expect(component.error()).toBe('Invalid credentials');
  });
});
