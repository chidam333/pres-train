import { TestBed } from '@angular/core/testing';
import { provideZonelessChangeDetection } from '@angular/core';

import { AuthFetch } from './auth-fetch';

describe('AuthFetch Service', () => {
  let service: AuthFetch;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideZonelessChangeDetection(),
        AuthFetch
      ]
    });
    service = TestBed.inject(AuthFetch);
    
    // Clear localStorage before each test
    localStorage.clear();
  });

  afterEach(() => {
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should store and retrieve token', () => {
    const testToken = 'test-jwt-token';
    
    service.storeToken(testToken);
    const retrievedToken = service.getStoredToken();
    
    expect(retrievedToken).toBe(testToken);
  });

  it('should clear stored token', () => {
    const testToken = 'test-jwt-token';
    
    service.storeToken(testToken);
    expect(service.getStoredToken()).toBe(testToken);
    
    service.clearStoredToken();
    expect(service.getStoredToken()).toBeNull();
  });

  it('should handle login success', async () => {
    const mockCredentials = { email: 'test@example.com', password: 'password123' };
    const mockResponse = { 
      message: 'Login successful',
      token: 'jwt-token',
      user: { id: 1, email: 'test@example.com' }
    };
    
    spyOn(window, 'fetch').and.returnValue(Promise.resolve(new Response(
      JSON.stringify(mockResponse),
      { status: 200 }
    )));

    const result = await service.login(mockCredentials);

    expect(window.fetch).toHaveBeenCalledWith(
      jasmine.stringContaining('/auth/login'),
      jasmine.objectContaining({
        method: 'POST',
        headers: jasmine.objectContaining({
          'Content-Type': 'application/json'
        }),
        body: JSON.stringify(mockCredentials)
      })
    );
    expect(result).toEqual(mockResponse);
  });

  it('should handle login failure', async () => {
    const mockCredentials = { email: 'test@example.com', password: 'wrongpassword' };
    
    spyOn(window, 'fetch').and.returnValue(Promise.resolve(new Response(
      'Invalid credentials',
      { status: 401 }
    )));

    const result = await service.login(mockCredentials);

    expect(result).toEqual({ error: 'Invalid credentials' });
  });

  it('should register user successfully', async () => {
    const mockUserDto = {
      name: 'John Doe',
      email: 'john@example.com',
      password: 'password123',
      role: 'student'
    };
    
    spyOn(window, 'fetch').and.returnValue(Promise.resolve(new Response(
      JSON.stringify(mockUserDto),
      { status: 201 }
    )));

    const result = await service.register(mockUserDto);

    expect(window.fetch).toHaveBeenCalledWith(
      jasmine.stringContaining('/auth/register'),
      jasmine.objectContaining({
        method: 'POST',
        body: JSON.stringify(mockUserDto)
      })
    );
    expect(result).toEqual(mockUserDto);
  });

  it('should get user role from token', () => {
    // Mock JWT token with role claim
    const mockToken = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoic3R1ZGVudCIsImVtYWlsIjoidGVzdEBleGFtcGxlLmNvbSJ9.signature';
    service.storeToken(mockToken);
    
    const role = service.getUserRole();
    expect(role).toBe('student');
  });

  it('should get user email from token', () => {
    // Mock JWT token with email claim
    const mockToken = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoic3R1ZGVudCIsImVtYWlsIjoidGVzdEBleGFtcGxlLmNvbSJ9.signature';
    service.storeToken(mockToken);
    
    const email = service.getUserEmail();
    expect(email).toBe('test@example.com');
  });
});
