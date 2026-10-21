import { ScrollType, Witch } from '../SFGameModels';
import { ByteParser } from './ByteParser';

const SCROLL_TYPES = new Set<number>(Object.values(ScrollType).filter(v => typeof v === 'number'));

// Witch::update uses a count at [4] and enchantment IDs at [6 + 3*i].
// Presence in this list unlocks a scroll; dates are not an unlock condition.
export function parseWitch(data: number[]): Witch {
  const parser = new ByteParser(data);
  const count = parser.unsignedAt(4, 0xff);
  const scrolls: ScrollType[] = [];
  for (let i = 0; i < count; i++) {
    const type = parser.at(6 + 3 * i);
    if (SCROLL_TYPES.has(type)) {
      scrolls.push(type);
    }
  }
  return { Scrolls: scrolls };
}
