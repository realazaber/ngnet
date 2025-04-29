import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { LoginDTO } from '../models/auth/user/login.dto';
import { environment } from '../../environments/environment';
import { TokenDTO } from '../models/auth/token.dto';
import { BehaviorSubject, Observable } from 'rxjs';
import { RegisterDTO } from '../models/auth/user/register.dto';
import { isPlatformBrowser } from '@angular/common';
import { BaseRoleDTO } from '../models/auth/role/base-role.dto';
import { httpHeaders, setupHeader } from '../utils/headers-setup';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  header: HttpHeaders = {} as HttpHeaders;
  private isBrowser: boolean;
  private isLoggedInSubject = new BehaviorSubject<boolean>(this.hasToken());

  isLoggedIn$: Observable<boolean> = this.isLoggedInSubject.asObservable();

  constructor(
    private http: HttpClient,
    @Inject(PLATFORM_ID) platformId: Object
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
    if (this.isBrowser) {
      this.header = setupHeader(this.getAccessToken());
    }
  }

  private hasToken(): boolean {
    if (this.isBrowser) {
      return !!localStorage.getItem('accessToken');
    }
    return false;
  }

  register(registerDto: RegisterDTO): Observable<any> {
    console.log('registerDto', registerDto);
    return this.http.post<RegisterDTO>(
      environment.baseUrl + '/identity/register',
      registerDto
    );
  }

  login(loginDto: LoginDTO): Observable<TokenDTO> {
    return this.http.post<TokenDTO>(
      environment.baseUrl + '/identity/login',
      loginDto
    );
  }

  logout() {
    if (this.isBrowser) {
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
      localStorage.removeItem('expiresIn');
      this.isLoggedInSubject.next(false);
    }
  }

  saveToken(token: TokenDTO) {
    if (this.isBrowser) {
      localStorage.setItem('accessToken', token.accessToken);
      localStorage.setItem('refreshToken', token.refreshToken);
      localStorage.setItem('expiresIn', token.expiresIn.toString());

      this.isLoggedInSubject.next(true);
    }
  }

  getAccessToken(): string {
    if (this.isBrowser) {
      return localStorage.getItem('accessToken') ?? '';
    }
    return '';
  }

  getRefreshToken(): string {
    if (this.isBrowser) {
      return localStorage.getItem('refreshToken') ?? '';
    }
    return '';
  }

  refreshTokenRequest(): Observable<TokenDTO> {
    const refreshToken = this.getRefreshToken();
    return this.http.post<TokenDTO>(environment.baseUrl + '/identity/refresh', {
      refreshToken,
    });
  }
}
