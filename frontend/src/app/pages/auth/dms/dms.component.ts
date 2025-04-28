import { Component, OnInit } from '@angular/core';
import { SharedModule } from '../../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { FolderService } from '../../../services/filesystem/folder.service';
import { GetFolderContentDTO } from '../../../models/filesystem/folders/get-folder-contents.dto';
import { FolderContentsComponent } from '../../../components/filesystem/folder/folder-contents/folder-contents.component';

@Component({
  selector: 'app-dms',
  standalone: true,
  imports: [SharedModule, FormsModule, FolderContentsComponent],
  templateUrl: './dms.component.html',
  styles: ``,
})
export class DmsPage implements OnInit {
 
  baseFolderContents: GetFolderContentDTO[] = [];

  constructor(private folderService: FolderService) {}

  ngOnInit(): void {
    this.folderService.getFolderContents().subscribe((data: GetFolderContentDTO[]) => {
      this.baseFolderContents = data;
     });    
  }
}