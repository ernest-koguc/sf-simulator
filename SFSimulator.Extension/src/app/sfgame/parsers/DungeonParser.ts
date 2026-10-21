import { Dungeons } from '../SFGameModels';
import { unsigned } from './ByteParser';

const LOCKED = -1;
const FINISHED = -2;

// Protocol indices follow LightDungeon / ShadowDungeon in Rust. Values are
// the simulation API's dungeon positions. null denotes a special/unsupported
// slot (tower, portal gap, training camp, sandstorm), never a shifted index.
const POSITIONS: readonly (number | null)[] = [
  1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 15, 19, null, 17, 18, null,
  24, 27, 21, 13, 14, 16, 20, 28, 22, 23, 25, 26, null, null, 29, 30, 31, 32, 33,
];

function progress(value: number, index: number, shadow: boolean): number {
  if (value === -1) {
    return LOCKED;
  }
  const finished = unsigned(value, 0xffff);
  // Dungeons::update_progress, including its generic ten-floor completion rule.
  if (finished === 10 || (index === 14 && finished === 100)
    || (shadow && index === 14 && finished >= 1000)
    || (shadow && index === 17 && finished >= 21)) {
    return FINISHED;
  }
  return finished;
}

export function parseDungeons(data: { light?: number[]; shadow?: number[] }, previous?: Dungeons | null): Dungeons {
  const update = (values: number[] | undefined, shadow: boolean) => {
    const current = shadow ? previous?.Shadow : previous?.Light;
    const byPosition = new Map(current?.map(item => [item.Position, item.Current]));
    POSITIONS.forEach((position, index) => {
      if (position === null) {
        return;
      }
      const id = position + (shadow ? 100 : 0);
      if (values?.[index] !== undefined) {
        byPosition.set(id, progress(values[index], index, shadow));
      } else if (!byPosition.has(id)) {
        byPosition.set(id, LOCKED);
      }
    });
    return Array.from(byPosition, ([Position, Current]) => ({ Position, Current }));
  };
  const special = (values: number[] | undefined, index: number, shadow: boolean, previousValue?: number) =>
    values?.[index] === undefined ? previousValue ?? LOCKED : progress(values[index], index, shadow);

  return {
    Light: update(data.light, false),
    Shadow: update(data.shadow, true),
    Tower: special(data.light, 14, false, previous?.Tower),
    Twister: special(data.shadow, 14, true, previous?.Twister),
    LoopOfIdols: special(data.shadow, 17, true, previous?.LoopOfIdols),
    Sandstorm: special(data.light, 31, false, previous?.Sandstorm),
  };
}
