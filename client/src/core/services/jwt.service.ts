import { inject, Injectable } from '@angular/core';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class JwtService {
  authService = inject(AuthService);

  private decodeJwt(token: string) {
    if (!token) {
      return null;
    }

    const stringSplit = token.split('.');

    const tokenObject: any = {};
    tokenObject.raw = tokenObject;
    tokenObject.header = JSON.parse(window.atob(stringSplit[0]));
    tokenObject.payload = JSON.parse(window.atob(stringSplit[1]));
    return tokenObject;
  }

  getUserId() {
    const token = this.authService.getToken() ?? '';
    const decodedJwt = this.decodeJwt(token);
    return parseInt(decodedJwt?.payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']);
  }
}
