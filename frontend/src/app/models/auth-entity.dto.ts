import { Entity } from './entity.dto';

export type AuthEntity = Entity & {
  creatorId: string;
};
