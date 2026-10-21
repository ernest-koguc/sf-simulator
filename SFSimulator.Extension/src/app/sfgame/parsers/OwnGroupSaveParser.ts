import { Guild } from '../SFGameModels';
import { ByteParser } from './ByteParser';

// Guild::update_group_save.
export function parseOwnGroupSave(data: number[]): Guild {
  return { PetMaxLevel: new ByteParser(data).unsignedAt(378, 0xffff) };
}
