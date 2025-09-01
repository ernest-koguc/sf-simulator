import Dexie, { Table } from 'dexie';
import { ExpeditionEncounter, ExpeditionHalfTime, ExpeditionItemReward, ExpeditionRewardResources } from './sfgame/SFGameModels';
import { ItemModel } from './sfgame/parsers/EquipmentParser';

export class Database extends Dexie {
  ExpeditionHistory!: Table<ExpeditionHistoryItem, number>;
  EquipmentGathering!: Table<EquipmentGatheringItem, number>;

  constructor() {
    super('XTOOL');
    this.version(2).stores({
      ExpeditionHistory: '++Id',
      EquipmentGathering: '++Id',
    }).upgrade(() => { });
  }
}

export const db = new Database();

export interface ExpeditionHistoryItem {
  Id?: number;
  UniquePlayerId: string;
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
  Thirst: number;
  CreatedAt: Date;
}

export interface EquipmentGatheringItem {
  Id?: number;
  UniquePlayerId: string,
  Class: number,
  Items: {
    Strength: number,
    Dexterity: number,
    Intelligence: number,
    Constitution: number,
    Luck: number,
    ItemQuality: number,
    PicIndex: number,
    Type: number,
    Armor: number,
    MinDmg: number,
    MaxDmg: number,
  }[]
}
