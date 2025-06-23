import { CanActivateFn, RedirectCommand, Router } from '@angular/router';
import { AuthFetch } from '../auth/auth-service/auth-fetch';
import { inject } from '@angular/core';

export const studentGuard: CanActivateFn = (route, state) => {
  const authFetch = inject(AuthFetch);
  const router = inject(Router);
  const token = authFetch.getStoredToken();
  if (!token) {
    return new RedirectCommand(router.parseUrl('/auth?error=not authorized as a student'));
  }
  if(!authFetch.isAuthenticated()){
    return new RedirectCommand(router.parseUrl('/auth?error=not authorized as a student'));
  }
  return authFetch.getUserRole() === 'student';
};
