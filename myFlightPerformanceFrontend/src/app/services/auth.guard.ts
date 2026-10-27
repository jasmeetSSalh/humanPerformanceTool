import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { map } from 'rxjs';
import { AuthService } from './auth.service';

// Sends signed-out users to the login page, then back to where they were going.
export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.checkSession().pipe(
    map(signedIn =>
      signedIn ? true : router.createUrlTree(['/login'], { queryParams: { returnUrl: state.url } })
    )
  );
};
