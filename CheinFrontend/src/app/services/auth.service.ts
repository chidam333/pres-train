import { Injectable, signal, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Auth } from '../models/auth.model';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private baseUrl = 'http://localhost:5127/api/Auth';
  token = signal<string | null>(null);
  currentUserId = signal<number | null>(null);

  login(credentials: any) {
    return this.http.post<Auth>(`${this.baseUrl}/login`, credentials).pipe(
      tap(response => {
        this.token.set(response.token);
        this.currentUserId.set(response.userId);
        localStorage.setItem('authToken', response.token);
        localStorage.setItem('currentUserId', response.userId.toString());
      })
    );
  }

  logout() {
    this.token.set(null);
    this.currentUserId.set(null);
    localStorage.removeItem('authToken');
    localStorage.removeItem('currentUserId');
  }

  isLoggedIn() {
    return this.token() !== null;
  }

  constructor() {
    // Initialize from local storage
    const storedToken = localStorage.getItem('authToken');
    const storedUserId = localStorage.getItem('currentUserId');
    if (storedToken && storedUserId) {
      this.token.set(storedToken);
      this.currentUserId.set(parseInt(storedUserId, 10));
    }
  }
}