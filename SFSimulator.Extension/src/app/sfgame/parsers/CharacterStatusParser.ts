import { CharacterStatus } from '../SFGameModels';
import { ByteParser } from './ByteParser';

// gamestate/mod.rs: characterstatus, ownplayersavepotions, wheel, dice, arena, charactergroup.
export function parseCharacterStatus(data: number[]): CharacterStatus {
  const parser = new ByteParser(data);
  return {
    MaxBeers: parser.unsignedAt(5, 0xff),
    ThirstLeft: parser.unsignedAt(6),
    UsedBeers: parser.unsignedAt(7, 0xff),
  };
}

export function parseHasEternalPotion(data: number[]): boolean {
  // parse_active_potions ignores lists shorter than ten words. Type 16 is EternalLife.
  return data.length >= 10 && data.slice(1, 4).includes(16);
}

export function parseWheelSpins(data: number[]): number {
  return new ByteParser(data).unsignedAt(1, 0xff);
}

export function parseDiceGamesRemaining(data: number[]): number {
  return new ByteParser(data).unsignedAt(1, 0xff);
}

export function parseArenaFights(data: number[]): number {
  return new ByteParser(data).unsignedAt(1, 0xff);
}

export function parseCanAttackHydra(data: number[]): boolean {
  return new ByteParser(data).unsignedAt(3, 0xffff) > 0;
}
