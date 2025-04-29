import { Component, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CreateFolderDTO } from '../../../../models/filesystem/folders/create-folder.dto';
import { FolderService } from '../../../../services/filesystem/folder.service';

@Component({
  selector: 'app-create-folder',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './create-folder.component.html',
  styles: ``
})
export class CreateFolderComponent {
  @Input() currentFolderId: string | undefined = undefined;
  newFolder: CreateFolderDTO = {} as CreateFolderDTO;

  constructor(private folderService: FolderService) {}

  createFolder() {
    this.folderService.createFolder(this.newFolder).subscribe((res: any) => {
      console.log(res);
    });
  }

}
