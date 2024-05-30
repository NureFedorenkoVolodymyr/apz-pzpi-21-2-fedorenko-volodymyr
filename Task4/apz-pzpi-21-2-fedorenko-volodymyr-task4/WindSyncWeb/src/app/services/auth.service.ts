import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { LoginViewModel } from '../../assets/models/login.viewmodel';
import { Constants } from '../../assets/models/constants';
import { BehaviorSubject } from 'rxjs';
import { RegisterViewModel } from '../../assets/models/register.viewmodel';
import { UserInfo } from '../../assets/models/user.info';
import { UserViewModel } from '../../assets/models/user.viewmodel';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7213/api/auth';
  
  private http = inject(HttpClient);

  private authState = new BehaviorSubject<boolean>(this.isTokenPresent());
  private username = new BehaviorSubject<string>(this.getUsername());
  private isAdmin = new BehaviorSubject<boolean>(this.getIsAdmin());

  login(credentials: LoginViewModel) {
    return this.http.post(`${this.apiUrl}/login`, credentials, { responseType: 'text' });
  }

  register(credentials: RegisterViewModel) {
    return this.http.post(`${this.apiUrl}/register`, credentials);
  }

  registerAdmin(credentials: RegisterViewModel) {
    return this.http.post(`${this.apiUrl}/register-admin`, credentials);
  }

  isAuthenticatedListener() {
    return this.authState.asObservable();
  }

  usernameListener() {
    return this.username.asObservable();
  }

  isAdminListener() {
    return this.isAdmin.asObservable();
  }

  isAuthenticated(){
    return this.isTokenPresent();
  }

  setToken(token: string) {
    localStorage.setItem(Constants.authTokenProperty, token);
    this.authState.next(true);

    this.getInfo()
      .subscribe(info => {
        localStorage.setItem(Constants.usernameProperty, info.username);
        this.username.next(this.getUsername());
        localStorage.setItem(Constants.isAdminProperty, info.roles.includes('Admin') ? 'true' : 'false');
        this.isAdmin.next(this.getIsAdmin());
      });
  }

  getToken() {
    return localStorage.getItem(Constants.authTokenProperty);
  }

  getUsername() {
    return localStorage.getItem(Constants.usernameProperty) || '';
  }

  getIsAdmin() {
    return localStorage.getItem(Constants.isAdminProperty) === 'true' ? true : false;
  }

  removeToken() {
    localStorage.removeItem(Constants.authTokenProperty);
    localStorage.removeItem(Constants.usernameProperty);
    localStorage.removeItem(Constants.isAdminProperty);
    this.authState.next(false);
    this.username.next('');
    this.isAdmin.next(false);
  }

  getAdmins() {
    return this.http.get<UserViewModel[]>(`${this.apiUrl}/admins`);
  }

  getUsers() {
    return this.http.get<UserViewModel[]>(`${this.apiUrl}/users`);
  }

  deleteUser(userId: string) {
    return this.http.delete(`${this.apiUrl}/users/${userId}`);
  }

  private getInfo() {
    return this.http.get<UserInfo>(this.apiUrl);
  }

  private isTokenPresent(): boolean {
    return !!this.getToken();
  }
}
