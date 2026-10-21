import { Attributes, Class, OwnPlayerSave } from '../SFGameModels';
import { ByteParser } from './ByteParser';

// gamestate/mod.rs: ownplayersavecharacter. This is no longer the old combined save.
export function parseOwnPlayerSave(data: number[]): OwnPlayerSave {
  const parser = new ByteParser(data);
  return {
    ID: parser.unsignedAt(1),
    Level: parser.at(3) & 0xffff,
    Class: parseCharacterClass(data),
    Armor: parser.unsignedAt(23, Number.MAX_SAFE_INTEGER),
    GuildPortal: parser.unsignedAt(26, 0xff),
    PortalHealthBonus: parser.unsignedAt(28, 0xffff),
    ...parseAttributes(parser, 30, 35),
  };
}

// OtherPlayer::update uses the same class word as the own character save.
export function parseCharacterClass(data: number[]): Class {
  const classType = new ByteParser(data).at(20);
  if (classType < Class.Warrior || classType > Class.BloodWeaver) {
    throw new RangeError(`Unknown character class: ${classType}`);
  }
  return classType;
}

export function parseAttributes(parser: ByteParser, start: number, bonusStart?: number): Attributes {
  const total = (index: number) => parser.unsignedAt(start + index)
    + (bonusStart === undefined ? 0 : parser.unsignedAt(bonusStart + index));
  return {
    Strength: total(0),
    Dexterity: total(1),
    Intelligence: total(2),
    Constitution: total(3),
    Luck: total(4),
  };
}
