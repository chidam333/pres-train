import { inject } from '@angular/core';
import { CanActivateFn, RedirectCommand, Router } from '@angular/router';
import { AuthFetch } from '../auth/auth-service/auth-fetch';

export const instructorGuard: CanActivateFn = (route, state) => {
  const authFetch = inject(AuthFetch);
  const router = inject(Router);
  const token = authFetch.getStoredToken();
  if (!token) {
    return new RedirectCommand(router.parseUrl('/auth?error=not authorized as an instructor'));
  }
  if (!authFetch.isAuthenticated()) {
    return new RedirectCommand(router.parseUrl('/auth?error=not authorized as an instructor'));
  }
  return authFetch.getUserRole() === 'instructor';
};
