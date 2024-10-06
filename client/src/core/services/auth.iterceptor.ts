import { HttpHandlerFn, HttpRequest } from "@angular/common/http";
import { inject } from "@angular/core";
import { AuthService } from "./auth.service";

export function authInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn) {
  const authService = inject(AuthService);

  // let token = '';

  // if (authService.isAuthenticated()) {
  //   token = authService.getToken() ?? '';
  // }

  const token = authService.getToken() ?? '';

  const authReq = req.clone({
    headers: req.headers.set('Authorization', token)
  });

  return next(authReq);
}