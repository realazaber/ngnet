import { CanActivateFn, Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { inject } from '@angular/core';
import { UserDTO } from '../models/auth/user/user.dto';
import { map, catchError, of } from 'rxjs';

export const ManageUsersGuard: CanActivateFn = (route, state) => {
  const userService = inject(UserService);
  const router = inject(Router);

  return userService.getCurrentUser().pipe(
    map((user: UserDTO) => {
      console.log(user);
      const authorized =
        user.roles.includes('Admin') || user.roles.includes('ManageUsers');
      if (!authorized) {
        router.navigate(['login']);
      }
      console.log(authorized);
      return authorized;
    }),
    catchError(() => {
      router.navigate(['login']);
      console.log('fail');
      return of(false);
    })
  );
};
