import { ToiletState } from '../SFGameModels';
import { ByteParser } from './ByteParser';

export function parseToiletState(data: number[]): ToiletState | null {
  // gamestate/mod.rs: toiletstate ignores responses shorter than three words.
  return data.length < 3 ? null : {
    AmountOfItemsToSacrifice: new ByteParser(data).at(2) >>> 0,
  };
}
