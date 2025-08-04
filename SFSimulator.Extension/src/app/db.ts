import Dexie, { Table } from 'dexie';
import { ExpeditionEncounter, ExpeditionHalfTime, ExpeditionItemReward, ExpeditionRewardResources } from './sfgame/SFGameModels';

export interface ExpeditionHistoryItem {
  Id?: number;
  Server: string;
  PlayerId: number;
  MainTask: ExpeditionEncounter;
  SideTasks: ExpeditionEncounter[];
  Heroism: number;
  HalfTime: ExpeditionHalfTime,
  HalfTimeItems: ExpeditionItemReward[],
  HalfTimeChosen: 'left' | 'mid' | 'right',
  ChestRewards: ExpeditionRewardResources[],
  FinalRewards: ExpeditionRewardResources,
  FinalItems: ExpeditionItemReward[],
  Stages: {
    Left: ExpeditionEncounter,
    Mid: ExpeditionEncounter,
    Right: ExpeditionEncounter,
    Chosen: 'left' | 'mid' | 'right',
    Backpack: ExpeditionEncounter[],
  }[],
  CreatedAt: Date;
}

export class Database extends Dexie {
  ExpeditionHistory!: Table<ExpeditionHistoryItem, number>;

  constructor() {
    super('XTOOL');
    this.version(1).stores({
      ExpeditionHistory: '++id',
    });
  }
}

export const db = new Database();

