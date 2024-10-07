import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { AuthService } from "../services/auth.service";

export const AuthGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isAuthenticated()){
    return true;
  } else {
    router.navigate(['albums/all'])
    return false;
  }
};

export const UnauthorizedGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (!authService.isAuthenticated()){
    return true;
  } else {
    router.navigate(['albums/all'])
    return false;
  }
};