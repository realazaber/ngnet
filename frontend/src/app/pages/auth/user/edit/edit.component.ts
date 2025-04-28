import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SharedModule } from '../../../../shared/shared.module';
import { UserService } from '../../../../services/user.service';
import { UserDTO } from '../../../../models/auth/user/user.dto';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../../services/auth.service';
import { BaseRoleDTO } from '../../../../models/auth/role/base-role.dto';
import { RoleService } from '../../../../services/role.service';
import { EditUserComponent } from '../../../../components/auth/user/edit/edit.component';

@Component({
  selector: 'app-edit',
  standalone: true,
  imports: [SharedModule, FormsModule, EditUserComponent],
  templateUrl: './edit.component.html',
  styles: ``,
})
export class EditUserPage implements OnInit {
  selectedUserId: string = '';
  selectedUser: UserDTO = {} as UserDTO;
  currentUser: UserDTO = {} as UserDTO;
  isAdmin: boolean = false;
  extraRoles: BaseRoleDTO[] = [];

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.selectedUserId = this.route.snapshot.paramMap.get('id') ?? '';
  }
}
