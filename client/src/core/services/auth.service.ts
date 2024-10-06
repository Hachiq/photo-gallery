import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginRequest } from '../models/login.request';
import { environment } from '../../environments/environment';
import { CONFIGURATION } from '../configuration/config';
import { RegisterRequest } from '../models/register.request';
import { Helpers } from './helpers';
import { User } from '../../models/user';

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
    localStorage?.removeItem(CONFIGURATION.auth.token.key);
    window.location.reload();
  }

  getToken() {
    const isLocalStorageAvailable = typeof window !== 'undefined' && window.localStorage;
    const token = isLocalStorageAvailable ? localStorage.getItem(CONFIGURATION.auth.token.key) : '';
    return token;
  }

  isAuthenticated() {
    return !!this.getToken();
  }

  getUserId() {
    const token = this.getToken() ?? '';
    const decodedJwt = Helpers.decodeJwt(token);
    return parseInt(decodedJwt?.payload[CONFIGURATION.auth.token.id]);
  }

  getUser(): User {
    const token = this.getToken() ?? '';
    const decodedJwt = Helpers.decodeJwt(token);
    return {
      id: parseInt(decodedJwt?.payload[CONFIGURATION.auth.token.id]),
      username: decodedJwt?.payload[CONFIGURATION.auth.token.name],
      role: decodedJwt?.payload[CONFIGURATION.auth.token.role]
    }
  }
}
