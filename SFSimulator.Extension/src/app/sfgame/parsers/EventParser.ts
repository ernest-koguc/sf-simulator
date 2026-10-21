import { EventType } from '../SFGameModels';
import { ByteParser } from './ByteParser';

const EVENTS = Object.values(EventType).filter(v => typeof v === 'number');

// gamestate/mod.rs: events. Short responses leave the existing state untouched.
export function parseEvents(data: number[]): EventType[] | null {
  if (data.length < 8) {
    return null;
  }
  const flags = new ByteParser(data).at(1);
  return EVENTS.filter(event => (flags & (1 << event)) !== 0);
}
