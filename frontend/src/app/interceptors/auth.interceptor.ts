import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { Observable, catchError, switchMap, throwError } from 'rxjs';

export const AuthInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const accessToken = authService.getAccessToken();

  // Clone request and add Authorization header if token exists
  const authReq = accessToken
    ? req.clone({
        setHeaders: { Authorization: `Bearer ${accessToken}` },
      })
    : req;

  return next(authReq).pipe(
    catchError((error) => {
      if (error.status === 401) {
        // If token is expired, attempt refresh
        return authService.refreshTokenRequest().pipe(
          switchMap((newToken) => {
            authService.saveToken(newToken);

            // Retry the failed request with the new token
            const clonedReq = req.clone({
              setHeaders: { Authorization: `Bearer ${newToken.accessToken}` },
            });
            return next(clonedReq);
          }),
          catchError(() => {
            authService.logout();
            router.navigate(['/login']);
            return throwError(() => error);
          })
        );
      }
      return throwError(() => error);
    })
  );
};
