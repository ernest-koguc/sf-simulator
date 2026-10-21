import { PortalProgress } from '../SFGameModels';
import { ByteParser } from './ByteParser';
import { serverDayOfYear } from './ResponseParser';

// Portal::update compares ordinals for inequality, including across new years.
export function parsePortalProgress(data: number[], serverTime: number): PortalProgress {
  const parser = new ByteParser(data);
  return {
    Finished: parser.unsignedAt(0, 0xffff, 10_000),
    CanAttack: parser.unsignedAt(2) !== serverDayOfYear(serverTime),
  };
}
