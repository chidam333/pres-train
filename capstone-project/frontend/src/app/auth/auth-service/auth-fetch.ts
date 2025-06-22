import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { CredentialsDTO, LoginResponse } from '../../model/auth.model';
import { UserDto, UserProfile } from '../../model/user.model';

export interface ApiErrorResponse {
  message: string;
  statusCode?: number;
}

@Injectable({
  providedIn: 'root'
})
export class AuthFetch {
  async login(credentials: CredentialsDTO): Promise<LoginResponse> {
    const response = await fetch(`${environment.apiUrl}/auth/login`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(credentials),
    });

    if (!response.ok) {
      const errorData = await response.text();
      throw new Error(errorData || 'Login failed');
    }

    return await response.json();
  }

  async register(userDto: UserDto): Promise<UserDto | {error:string}> {
    await new Promise(resolve => setTimeout(() => {
      resolve(null);
    }, 1000));
    const response = await fetch(`${environment.apiUrl}/auth/register`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(userDto),
    });

    if (!response.ok) {
      const errorData = await response.text();
      if (response.status === 409) {
        return {error: 'User with this email already exists.'};
      }
      return {error: errorData || 'Registration failed'};
    }

    return await response.json();
  }

  async aboutMe(): Promise<UserProfile> {
    const token = this.getStoredToken();
    if (!token) {
      throw new Error('No authentication token found');
    }

    const response = await fetch(`${environment.apiUrl}/auth/aboutme`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`,
      },
    });

    if (!response.ok) {
      if (response.status === 401) {
        this.clearStoredToken();
        throw new Error('Authentication token is invalid or expired');
      }
      const errorData = await response.text();
      throw new Error(errorData || 'Failed to fetch user profile');
    }

    return await response.json();
  }

  storeToken(token: string): void {
    localStorage.setItem('auth_token', token);
  }


  getStoredToken(): string | null {
    return localStorage.getItem('auth_token');
  }


  clearStoredToken(): void {
    localStorage.removeItem('auth_token');
  }

  isAuthenticated(): boolean {
    const token = this.getStoredToken();
    if (!token) return false;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const currentTime = Math.floor(Date.now() / 1000);
      return payload.exp > currentTime;
    } catch (error) {
      this.clearStoredToken();
      return false;
    }
  }

  logout(): void {
    this.clearStoredToken();
  }

  getUserRole(): string | null {
    const token = this.getStoredToken();
    if (!token) return null;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.role || payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || null;
    } catch (error) {
      return null;
    }
  }

  getUserEmail(): string | null {
    const token = this.getStoredToken();
    if (!token) return null;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.email || payload.sub || payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] || null;
    } catch (error) {
      return null;
    }
  }
}
