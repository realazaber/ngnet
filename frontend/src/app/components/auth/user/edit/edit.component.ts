import { Component, Input, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UserDTO } from '../../../../models/auth/user/user.dto';
import { UserService } from '../../../../services/user.service';
import { BaseRoleDTO } from '../../../../models/auth/role/base-role.dto';
import { RoleService } from '../../../../services/role.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-edituser',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './edit.component.html',
  styles: ``,
})
export class EditUserComponent implements OnInit {
  selectedUser: UserDTO = {} as UserDTO;
  currentUser: UserDTO = {} as UserDTO;
  isAdmin: boolean = false;
  extraRoles: BaseRoleDTO[] = [];
  @Input() userId: string = '';

  constructor(
    private userService: UserService,
    private roleService: RoleService,
    private router: Router
  ) {}

  ngOnInit(): void {
    console.log(this.userId, 'userId');
    this.selectedUser.id = this.userId;
    this.userService.getCurrentUser().subscribe((data: UserDTO) => {
      this.currentUser = data;
      console.log('Current User: ', this.currentUser);
      if (this.currentUser.roles.includes('Admin')) {
        this.isAdmin = true;
      }
    });

    this.userService
      .getUserById(this.selectedUser.id)
      .subscribe((data: UserDTO) => {
        this.selectedUser = data;
      });

    if (this.selectedUser.id == '') {
      this.router.navigateByUrl('/login');
    }

    this.roleService.getRoles().subscribe((data: BaseRoleDTO[]) => {
      this.extraRoles = data;
    });
  }

  hasRole(role: string): boolean {
    return this.selectedUser.roles.includes(role);
  }

  saveChanges(): void {}
}
