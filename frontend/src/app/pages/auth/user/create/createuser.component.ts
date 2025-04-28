import { Component } from '@angular/core';
import { SharedModule } from '../../../../shared/shared.module';

@Component({
  selector: 'app-createuser',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './createuser.component.html',
  styles: ``,
})
export class CreateUserPage {}
