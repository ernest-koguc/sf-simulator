import { Equipment, RuneType, ScrollType } from '../SFGameModels';
import { ByteParser, chunks } from './ByteParser';

export const ITEM_LENGTH = 19;
const EQUIPMENT_SLOTS = ['Head', 'Body', 'Hand', 'Feet', 'Neck', 'Belt', 'Ring', 'Misc', 'Wpn1', 'Wpn2'] as const;
const ENCHANTMENTS = new Set<number>(Object.values(ScrollType).filter(v => typeof v === 'number'));

export function parseEquipment(data: number[]): Equipment {
  if (data.length % ITEM_LENGTH !== 0) {
    throw new RangeError('Equipment must contain complete 19-value items');
  }
  // Rust represents empty slots as None. The first nine TS slots use zero
  // items so existing combat consumers can read them without optional chains.
  const items = EQUIPMENT_SLOTS.map((_, index) => {
    const start = index * ITEM_LENGTH;
    return start < data.length ? new ItemModel(data.slice(start, start + ITEM_LENGTH)) : ItemModel.empty();
  });
  return {
    Head: items[0], Body: items[1], Hand: items[2], Feet: items[3],
    Neck: items[4], Belt: items[5], Ring: items[6], Misc: items[7],
    Wpn1: items[8], Wpn2: items[9].Type === 0 ? null : items[9],
  };
}

export function parseCompanionEquipment(data: number[]) {
  const length = ITEM_LENGTH * EQUIPMENT_SLOTS.length;
  if (data.length < length * 3) {
    throw new RangeError('Companion equipment must contain three sets of ten items');
  }
  return {
    Bert: parseEquipment(data.slice(0, length)),
    Mark: parseEquipment(data.slice(length, length * 2)),
    Kunigunde: parseEquipment(data.slice(length * 2, length * 3)),
  };
}

export class ItemModel {
  public readonly Type: number;
  public readonly Armor: number;
  public readonly DamageMin: number;
  public readonly DamageMax: number;
  public readonly HasEnchantment: boolean;
  public readonly RuneType: RuneType;
  public readonly RuneValue: number;
  public readonly ItemQuality: number;

  // Item::parse / ItemType::parse / Item::armor in gamestate/items.rs.
  constructor(data: number[]) {
    const parser = new ByteParser(data);
    this.Type = parser.at(0) & 0xff;
    const hasItem = this.Type !== 0;
    this.Armor = this.Type >= 3 && this.Type <= 10 ? parser.unsignedAt(5) : 0;
    this.DamageMin = this.Type === 1 ? parser.unsignedAt(5) : 0;
    this.DamageMax = this.Type === 1 ? parser.unsignedAt(6) : 0;
    this.HasEnchantment = hasItem && ENCHANTMENTS.has(parser.at(2));
    this.ItemQuality = hasItem ? parser.unsignedAt(17) : 0;
    let runeType = RuneType.None;
    let runeValue = 0;
    if (this.Type >= 1 && this.Type <= 10) {
      // A rune can occupy any attribute slot, not just slot three.
      for (let i = 0; i < 3; i++) {
        const type = parser.at(7 + i);
        const value = parser.at(10 + i);
        if (type >= 31 && type <= 42 && value >= 0 && value <= 0xff) {
          runeType = type - 30;
          runeValue = value;
        }
      }
    }
    this.RuneType = runeType;
    this.RuneValue = runeValue;
  }

  static empty(): ItemModel {
    return new ItemModel(new Array<number>(ITEM_LENGTH).fill(0));
  }
}

/** Item rewards only feed item-count statistics; their prices are never read. */
export function countItemRewards(data: number[]): number {
  return Array.from(chunks(data, ITEM_LENGTH)).filter(item => (item[0] & 0xff) !== 0).length;
}
