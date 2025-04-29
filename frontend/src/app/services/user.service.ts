import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable, OnInit, PLATFORM_ID } from '@angular/core';
import { UserDTO } from '../models/auth/user/user.dto';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { AuthService } from './auth.service';
import { isPlatformBrowser } from '@angular/common';
import { setupHeader } from '../utils/headers-setup';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  header: HttpHeaders = {} as HttpHeaders;
  private isBrowser: boolean;

  constructor(
    private http: HttpClient,
    private authService: AuthService,
    @Inject(PLATFORM_ID) platformId: Object
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
    if (this.isBrowser) {
      this.header = setupHeader(this.authService.getAccessToken());
    }
  }

  getCurrentUser(): Observable<UserDTO> {
    return this.http.get<UserDTO>(environment.baseUrl + '/user', {
      headers: this.header,
    });
  }

  getUserById(userId: string): Observable<UserDTO> {
    return this.http.get<UserDTO>(
      environment.baseUrl + '/user/single?userId=' + userId,
      {
        headers: this.header,
      }
    );
  }

  getUsers(): Observable<UserDTO[]> {
    return this.http.get<UserDTO[]>(environment.baseUrl + '/user/view', {
      headers: this.header,
    });
  }

  updateUser(user: UserDTO): Observable<UserDTO> {
    return this.http.put<UserDTO>(environment.baseUrl + '/user', user, {
      headers: this.header,
    });
  }
}
