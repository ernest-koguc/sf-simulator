import Dexie, { Table } from 'dexie';
import { ExpeditionEncounter } from './sfgame/SFGameModels';

export class Database extends Dexie {
  ExpeditionHistory!: Table<ExpeditionHistoryItem, number>;
  EquipmentGathering!: Table<EquipmentGatheringItem, number>;

  constructor() {
    super('XTOOL');
    this.version(2).stores({
      ExpeditionHistory: '++Id',
      EquipmentGathering: '++Id',
    }).upgrade(() => {});
  }
}

export const db = new Database();

export interface ExpeditionHistoryItem {
  Id?: number;
  UniquePlayerId: string;
  MainTask: ExpeditionEncounter;
  Heroism: number;
  Thirst: number;
  CreatedAt: Date;
  ChestCount?: number;
  ItemCount?: number;
  // Existing history stores arrays. Only their lengths are read.
  ChestRewards?: unknown[];
  HalfTimeItems?: unknown[];
  FinalItems?: unknown[];
}

export function expeditionChestCount(record: ExpeditionHistoryItem): number {
  return record.ChestCount ?? record.ChestRewards?.length ?? 0;
}

export function expeditionItemCount(record: ExpeditionHistoryItem): number {
  return record.ItemCount ?? (record.HalfTimeItems?.length ?? 0) + (record.FinalItems?.length ?? 0);
}

export interface EquipmentGatheringItem {
  Id?: number;
  UniquePlayerId: string;
  Class: number;
  Items: {
    ItemQuality: number;
    Type: number;
    Armor: number;
    MinDmg: number;
    MaxDmg: number;
  }[];
}
