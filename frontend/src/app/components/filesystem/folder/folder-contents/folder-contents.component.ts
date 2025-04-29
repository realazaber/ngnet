import { Component, Input } from '@angular/core';
import { GetFolderContentDTO } from '../../../../models/filesystem/folders/get-folder-contents.dto';
import { CreateFolderComponent } from "../create-folder/create-folder.component";
import { UploadFileComponent } from "../../file/upload-file/upload-file.component";

@Component({
  selector: 'app-folder-contents',
  standalone: true,
  imports: [CreateFolderComponent, UploadFileComponent],
  templateUrl: './folder-contents.component.html',
  styles: ``
})
export class FolderContentsComponent {
  @Input() folderContents: GetFolderContentDTO[] = [];  
  displayFolderCreator: boolean = false;
  displayFileCreator: boolean = false;

  toggleFolderCreator() {
    this.displayFolderCreator = !this.displayFolderCreator; 
    this.displayFileCreator = false;
  }

  toggleFileUploader() {
    this.displayFileCreator = !this.displayFileCreator;
    this.displayFolderCreator = false;
  }
}
