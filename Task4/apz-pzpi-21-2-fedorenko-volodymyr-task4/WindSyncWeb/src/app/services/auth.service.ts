import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { LoginViewModel } from '../../assets/models/login.viewmodel';
import { Constants } from '../../assets/models/constants';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7213/api/auth';
  
  private http = inject(HttpClient);

  private authState = new BehaviorSubject<boolean>(this.isTokenPresent());

  login(credentials: LoginViewModel) {
    return this.http.post(`${this.apiUrl}/login`, credentials, { responseType: 'text' });
  }

  isAuthenticatedListener() {
    return this.authState.asObservable();
  }

  isAuthenticated(){
    return this.isTokenPresent();
  }

  setToken(token: string, email: string) {
    localStorage.setItem(Constants.authTokenProperty, token);
    this.authState.next(true);
  }

  getToken() {
    return localStorage.getItem(Constants.authTokenProperty);
  }

  removeToken() {
    localStorage.removeItem(Constants.authTokenProperty);
    this.authState.next(false);
  }

  private isTokenPresent(): boolean {
    return !!this.getToken();
  }
}