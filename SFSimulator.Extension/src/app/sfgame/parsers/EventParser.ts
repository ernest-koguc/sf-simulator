import { EventType } from '../SFGameModels';
import { ByteParser } from './ByteParser';

const FLAG_EVENTS = Object.values(EventType).filter(v => typeof v === 'number');

export function parseEvents(data: (number | string)[]): EventType[] | null {
  if (data.length < 8) {
    return null;
  }
  const flags = new ByteParser(data).at(1);
  const events: EventType[] = FLAG_EVENTS.filter(event => (flags & (1 << event)) !== 0);
  if (data[11] === EventType.Hellevator) {
    events.push(EventType.Hellevator);
  }
  return events;
}
