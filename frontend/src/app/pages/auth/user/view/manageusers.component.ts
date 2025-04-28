import { Component, OnInit } from '@angular/core';
import { ContainerComponent } from '../../../../components/container/container.component';
import { SharedModule } from '../../../../shared/shared.module';
import { UserService } from '../../../../services/user.service';
import { UserDTO } from '../../../../models/auth/user/user.dto';

@Component({
  selector: 'app-manageusers',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './manageusers.component.html',
  styles: ``,
})
export class ViewUsersPage implements OnInit {
  currentUser: UserDTO = {} as UserDTO;
  users: UserDTO[] = [];
  admin: boolean = false;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.userService.getCurrentUser().subscribe((data: UserDTO) => {
      this.currentUser = data;
      if (this.currentUser.roles.includes('Admin')) {
        this.admin = true;
      }
    });

    this.userService.getUsers().subscribe((data: UserDTO[]) => {
      this.users = data;
    });
  }
}
