import { inject } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivateFn, Router } from "@angular/router";
import { AuthService } from "../services/auth.service";

export const UserGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const userId = +route.params['id'];

  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.getUserId() === userId || authService.isAdmin()) {
    return true;
  } else {
    router.navigate(['albums']);
    return false;
  }
};