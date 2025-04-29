import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { environment } from '../../environments/environment';
import { BaseRoleDTO } from '../models/auth/role/base-role.dto';
import { Observable } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';
import { setupHeader } from '../utils/headers-setup';
import { AuthService } from './auth.service';
import { RoleUserCountDTO } from '../models/auth/role/role-user-count.dto';

@Injectable({
  providedIn: 'root',
})
export class RoleService {
  header: HttpHeaders = {} as HttpHeaders;
  private isBrowser: boolean;

  constructor(
    private http: HttpClient,
    private authService: AuthService,
    @Inject(PLATFORM_ID) platformId: Object
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
    if (this.isBrowser) {
      this.header = setupHeader(authService.getAccessToken());
    }
  }

  getRoles(): Observable<BaseRoleDTO[]> {
    return this.http.get<BaseRoleDTO[]>(
      environment.baseUrl + '/role/getroles',
      {
        headers: this.header,
      }
    );
  }

  getRoleUsers(roleName: string): Observable<RoleUserCountDTO> {
    return this.http.get<RoleUserCountDTO>(
      environment.baseUrl + '/role/getroleusers?roleName=' + roleName,
      {
        headers: this.header,
      }
    );
  }

  createRole(roleName: string): Observable<BaseRoleDTO> {
    return this.http.post<BaseRoleDTO>(
      environment.baseUrl + '/role/createrole?role=' + roleName,
      {
        headers: this.header,
      }
    );
  }

  deleteRole(roleName: string) {
    return this.http.delete(
      environment.baseUrl + '/role/deleterole?roleName=' + roleName,
      { headers: this.header }
    );
  }
}
