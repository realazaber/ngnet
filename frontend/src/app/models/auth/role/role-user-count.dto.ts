import { BaseRoleDTO } from './base-role.dto';

export interface RoleUserCountDTO extends BaseRoleDTO {
  userIds: string[];
}
