import { Component, OnInit } from '@angular/core';
import { SharedModule } from '../../../shared/shared.module';
import { AuthService } from '../../../services/auth.service';
import { BaseRoleDTO } from '../../../models/auth/role/base-role.dto';
import { FormsModule } from '@angular/forms';
import { RoleService } from '../../../services/role.service';
import { RoleUserCountDTO } from '../../../models/auth/role/role-user-count.dto';

@Component({
  selector: 'app-manageroles',
  standalone: true,
  imports: [SharedModule, FormsModule],
  templateUrl: './manageroles.component.html',
  styles: ``,
})
export class ManageRolesPage implements OnInit {
  baseRoles: BaseRoleDTO[] = [];
  roles: RoleUserCountDTO[] = [];
  newRole: string = '';

  constructor(private roleService: RoleService) {}

  ngOnInit(): void {
    this.roles = [];
    this.roleService.getRoles().subscribe((roles: BaseRoleDTO[]) => {
      console.log('roles' + roles);
      roles.forEach((role: BaseRoleDTO) => {
        console.log(role.name);
        this.roleService
          .getRoleUsers(role.name)
          .subscribe((roleUsers: RoleUserCountDTO) => {
            console.log('roleUsers' + roleUsers);
            this.roles.push(roleUsers);
          });
      });
    });
  }

  createRole() {
    if (this.newRole === '') {
      alert("Role name can't be empty");
      return;
    }
    this.roleService.createRole(this.newRole).subscribe((role) => {
      next: {
        this.roles.push({ id: role.id, name: this.newRole, userIds: [] });
        this.newRole = '';
      }
      error: console.log('Error creating role');
    });
  }

  getUserCount(role: RoleUserCountDTO) {
    return role.userIds.length;
  }

  deleteRole(roleName: string) {
    this.roleService.deleteRole(roleName).subscribe(() => {
      error: console.log('Error deleting role');
    });
    this.roles = this.roles.filter((role) => role.name !== roleName);
  }
}
