import { Injectable, PLATFORM_ID } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

export const requireAuthGuard: CanActivateFn = () => {
  const platformId = inject(PLATFORM_ID);
  const cookieService = inject(CookieService);
  const router = inject(Router);

  if (isPlatformBrowser(platformId)) {
    const hasToken = cookieService.check('jwt_token');
    if (!hasToken) {
      router.navigate(['/auth']);
      return false;
    }
    return true;
  }

  return false;
};

export const RedirectIfLoggedInGuard: CanActivateFn = () => {
  const cookieService = inject(CookieService);
  const router = inject(Router);
  const platformId = inject(PLATFORM_ID);
  if (isPlatformBrowser(platformId)) {
    const hasToken = cookieService.check('jwt_token');

    if (hasToken) {
      router.navigate(['/product']);
      return false;
    }

    return true;
  }
  return false;
};