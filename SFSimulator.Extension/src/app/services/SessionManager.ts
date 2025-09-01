import { computed, Injectable, signal, WritableSignal } from '@angular/core';
import { DailyTask, DailyTaskReward, Dungeons, Equipment, EventType, Expedition, OwnPlayerSave, Pets, Resources, ToiletState, Tower, Witch } from '../sfgame/SFGameModels';
import { ExpeditionProgress } from './ExpeditionService';
import { ItemModel } from '../sfgame/parsers/EquipmentParser';
import { Fightable } from '../sfgame/Fightable';

@Injectable({
  providedIn: 'root'
})
export class SessionManager {
  public sessions: WritableSignal<Record<string, SessionData>> = signal({});
  public currentId: WritableSignal<string | null> = signal(null);

  public fightable = computed(() => {
    const current = this.current();
    if (!current || !current.ownPlayerSave || !current.ownItems || !current.dungeons) {
      return null;
    }

    return new Fightable(current.ownPlayerSave, current.ownItems, current.companionItems, current.tower, current.dungeons);
  });

  public current = computed(() => {
    const current = this.currentId();
    if (!current) {
      return null;
    }

    return this.sessions()[current] || null;
  });

  public updateCurrent(update: (data: SessionData) => void) {
    const currentId = this.currentId();
    let currentData = this.current();
    if (currentId === null) {
      return;
    }

    if (currentData === null) {
      currentData = {
        playerName: null,
        server: null,
        guildName: null,
        dungeons: null,
        resources: null,
        tower: null,
        witch: null,
        expedition: null,
        expeditionProgress: null,
        pets: null,
        ownItems: null,
        backpack: null,
        dummy: null,
        ownPlayerSave: null,
        companionItems: null,
        activeEvents: null,
        dailyTasksRewards: null,
        dailyTasks: null,
        toiletState: null,
      }
    }

    update(currentData);

    this.sessions.update(sessions => {
      return { ...sessions, [currentId]: { ...currentData } };
    });
  }
}

type SessionData = {
  playerName: string | null;
  server: string | null;
  guildName: string | null;
  dungeons: Dungeons | null;
  resources: Resources | null;
  tower: Tower | null;
  witch: Witch | null;
  expedition: Expedition[] | null;
  expeditionProgress: ExpeditionProgress | null;
  pets: Pets | null;
  ownItems: Equipment | null;
  companionItems: {
    Bert: Equipment;
    Mark: Equipment;
    Kunigunde: Equipment;
  } | null;
  backpack: ItemModel[] | null;
  dummy: Equipment | null;
  ownPlayerSave: OwnPlayerSave | null;
  activeEvents: { Id: EventType, Name: string }[] | null;
  dailyTasksRewards: DailyTaskReward[] | null;
  dailyTasks: DailyTask[] | null;
  toiletState: ToiletState | null;
}
