import { isPlatformBrowser } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { AuthService } from '../auth.service';
import { setupHeader } from '../../utils/headers-setup';
import { CreateFolderDTO } from '../../models/filesystem/folders/create-folder.dto';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { GetFolderContentDTO } from '../../models/filesystem/folders/get-folder-contents.dto';

@Injectable({
  providedIn: 'root',
})
export class FolderService {
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

  createFolder(folder: CreateFolderDTO): Observable<any> {
    return this.http.post(
      environment.baseUrl + '/folders/create',
      folder,
      {
        headers: this.header,
      }
    );
  }

  editFolderMetaData(folderId: string, description: string): void {}

  getFolderContents(folderId?: string): Observable<GetFolderContentDTO[]> {

    if (folderId === undefined) { 
      folderId = '';
    }

    return this.http.get<GetFolderContentDTO[]>(environment.baseUrl + '/folder?folderId=' + folderId, 
      {
        headers: this.header }
    );
  }

  deleteFolder(folderId: string): void {}
}
