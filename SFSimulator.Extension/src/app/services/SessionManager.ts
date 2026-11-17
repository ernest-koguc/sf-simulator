import { computed, Injectable, signal, WritableSignal } from '@angular/core';
import {
  CharacterStatus, DailyTask, DailyTaskReward, Dungeons, Equipment, EventType,
  Expedition, Guild, OwnPlayerSave, Pets, PortalProgress, Potion, ToiletState, Tower, Witch,
} from '../sfgame/SFGameModels';
import { Fightable } from '../sfgame/Fightable';

@Injectable({ providedIn: 'root' })
export class SessionManager {
  public sessions: WritableSignal<Record<string, SessionData>> = signal({});
  public currentId: WritableSignal<string | null> = signal(null);

  public fightable = computed(() => {
    const current = this.current();
    if (!current?.ownPlayerSave || !current.ownItems || !current.dungeons) {
      return null;
    }
    return new Fightable(current.ownPlayerSave, current.ownItems, current.companionItems,
      current.tower, current.hasEternalPotion, current.potions, current.pets);
  });

  public current = computed(() => {
    const id = this.currentId();
    return id === null ? null : this.sessions()[id] ?? null;
  });

  public updateCurrent(update: (data: SessionData) => void) {
    const currentId = this.currentId();
    if (currentId === null) {
      return;
    }
    const currentData: SessionData = { ...(this.current() ?? createSessionData()) };
    update(currentData);
    this.sessions.update(sessions => ({ ...sessions, [currentId]: currentData }));
  }
}

export function createSessionData(): SessionData {
  return {
    playerName: null,
    server: null,
    ownPlayerSave: null,
    ownItems: null,
    companionItems: null,
    tower: null,
    dungeons: null,
    witch: null,
    pets: null,
    guild: null,
    portalProgress: null,
    activeEvents: null,
    dailyTasksRewards: null,
    dailyTasks: null,
    toiletState: null,
    characterStatus: null,
    hasEternalPotion: false,
    potions: [],
    wheelSpinsToday: null,
    diceGamesRemaining: null,
    arenaFightsToday: null,
    canAttackHydra: null,
    expedition: null,
  };
}

export type SessionData = {
  playerName: string | null;
  server: string | null;
  ownPlayerSave: OwnPlayerSave | null;
  ownItems: Equipment | null;
  companionItems: { Bert: Equipment; Mark: Equipment; Kunigunde: Equipment } | null;
  tower: Tower | null;
  dungeons: Dungeons | null;
  witch: Witch | null;
  pets: Pets | null;
  guild: Guild | null;
  portalProgress: PortalProgress | null;
  activeEvents: EventType[] | null;
  dailyTasksRewards: DailyTaskReward[] | null;
  dailyTasks: DailyTask[] | null;
  toiletState: ToiletState | null;
  characterStatus: CharacterStatus | null;
  hasEternalPotion: boolean;
  potions: Potion[];
  wheelSpinsToday: number | null;
  diceGamesRemaining: number | null;
  arenaFightsToday: number | null;
  canAttackHydra: boolean | null;
  expedition: Expedition[] | null;
};
