import { EventType } from "../SFGameModels";

export function parseEvents(events: number): { Id: EventType, Name: string }[] {
  const eventList = [];
  const eventMap = Object.values(EventType);
  for (let i of eventMap.filter(v => typeof v === 'number') as number[]) {
    if ((events & (1 << i)) > 0) {
      eventList.push({ Id: i, Name: eventMap[i] as string });
    }
  }

  return eventList;
}
