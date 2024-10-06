import { inject } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivateFn, Router } from "@angular/router";
import { AuthService } from "../services/auth.service";
import { CONFIGURATION } from "../configuration/config";

export const UserGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const userId = +route.params['id'];

  const authService = inject(AuthService);
  const router = inject(Router);

  const currentUser = authService.getUser();

  if (currentUser.id === userId || currentUser.role === CONFIGURATION.roles.admin) {
    return true;
  } else {
    router.navigate(['albums']);
    return false;
  }
};