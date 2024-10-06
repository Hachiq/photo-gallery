import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginRequest } from '../models/login.request';
import { environment } from '../../environments/environment';
import { CONFIGURATION } from '../configuration/config';
import { RegisterRequest } from '../models/register.request';
import { Helpers } from './helpers';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = `${environment.apiUrl}/${CONFIGURATION.auth.url}`

  constructor(private http: HttpClient) { }

  register(user: RegisterRequest) {
    return this.http.post(`${this.baseUrl}/register`, user);
  }

  login(user: LoginRequest) {
    return this.http.post(
      `${this.baseUrl}/login`,
      user,
      {
        withCredentials: true,
        responseType: 'text'
      }
    );
  }

  logout() {
    localStorage?.removeItem(CONFIGURATION.auth.tokenKey);
    window.location.reload();
  }

  getToken() {
    const isLocalStorageAvailable = typeof window !== 'undefined' && window.localStorage;
    const token = isLocalStorageAvailable ? localStorage.getItem(CONFIGURATION.auth.tokenKey) : '';
    return token;
  }

  isAuthenticated() {
    return !!this.getToken();
  }

  isAdmin() {
    if (this.getRole() === 'Admin') {
      return true;
    }
    return false;
  }

  getRole() {
    const token = this.getToken() ?? '';
    const decodedJwt = Helpers.decodeJwt(token);
    return decodedJwt?.payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
  }

  getUserId() {
    const token = this.getToken() ?? '';
    const decodedJwt = Helpers.decodeJwt(token);
    return parseInt(decodedJwt?.payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']);
  }
}
