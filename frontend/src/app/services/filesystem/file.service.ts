import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { AuthService } from '../auth.service';
import { isPlatformBrowser } from '@angular/common';
import { setupHeader } from '../../utils/headers-setup';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class FileService {
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

  uploadFile(file: File, description: string): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('description', description);
    return this.http.post(environment.baseUrl + '/files/upload', formData, {
      headers: this.header,
    });
  }

  editFileMetaData(fileId: string, description: string): void {}

  getFile(filePath: string): void {}

  deleteFile(fileId: string): void {}
}
