import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { TokenApiModel } from '../models/token-api.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl: string = 'https://localhost:7002/api/User';
  private userPayload: any;

  constructor(private http: HttpClient, private router: Router) {
    this.userPayload = this.decodedToken();
  }

  signUp(userObj: any) {
    return this.http.post<any>(`${this.baseUrl}/register`, userObj);
  }
  login(loginObj: any) {
    return this.http.post<any>(`${this.baseUrl}/authenticate`, loginObj);
  }
  storeToken(tokenValue: string) {
    localStorage.setItem('token', tokenValue);
  }
  getToken() {
    return localStorage.getItem('token');
  }
  isLoggedIn(): boolean {
    return !!localStorage.getItem('token'); // Convert string value to boolean
  }

  signOut() {
    localStorage.clear();
    this.router.navigate(['/login']);
    //localStorage.removeItem('token');
  }

  decodedToken() {
    const jwtHelper = new JwtHelperService();
    const token = this.getToken()!;
    return jwtHelper.decodeToken(token);
  }

  getFullNameFromToken() {
    if (this.userPayload) {
      return this.userPayload.unique_name;
    }
  }
  getRoleFromToken() {
    if (this.userPayload) {
      return this.userPayload.role;
    }
  }
  // REFRESH TOKEN
  reNewToken(tokenApi: TokenApiModel) {
    return this.http.post<any>(`${this.baseUrl}/refresh`, tokenApi);
  }
  storeRefreshToken(tokenValue: string) {
    localStorage.setItem('refreshToken', tokenValue);
  }
  getRefreshToken() {
    return localStorage.getItem('refreshToken');
  }
}
