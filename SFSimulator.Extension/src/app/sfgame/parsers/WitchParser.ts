import { ScrollType, Witch } from '../SFGameModels';
import { ByteParser } from './ByteParser';

const SCROLL_TYPES = new Set<number>(Object.values(ScrollType).filter(v => typeof v === 'number'));

export function parseWitch(data: number[], previous?: Witch | null): Witch {
  const parser = new ByteParser(data);
  const count = parser.at(4);
  if (count < 0 || count > 0xff) {
    throw new RangeError(`Invalid witch enchantment count: ${count}`);
  }
  const scrolls = new Set<ScrollType>(previous?.Scrolls);
  for (let i = 0; i < count; i++) {
    const type = parser.at(6 + 3 * i);
    if (SCROLL_TYPES.has(type)) {
      scrolls.add(type);
    }
  }
  return { Scrolls: [...scrolls] };
}
