import { MountType } from "./mount-type";
import { QuestPriority } from "./quest-priority";
import { SavedSchedule } from "./schedule";
import { SpinTactic } from "./spin-tactics";

export interface SimulationConfigForm {
  account: {
    characterName: string | null;
    level: number | null;
    baseStat: number | null;
    experience: number | null;
    goldPitLevel: number | null;
    academyLevel: number | null;
    hydraHeads: number | null;
    gemMineLevel: number | null;
    treasuryLevel: number | null;
    mountType: MountType | null;
  },
  bonuses: {
    scrapbookFillness: number | null;
    xpGuildBonus: number | null;
    xpRuneBonus: number | null
    hasExperienceScroll: boolean;
    tower: number | null
    goldGuildBonus: number | null;
    goldRuneBonus: number | null;
    hasGoldScroll: boolean;
    hasArenaGoldScroll: boolean;
  },
  playstyle: {
    switchPriority: boolean;
    switchLevel: number | null;
    drinkBeerOneByOne: boolean;
    dailyThirst: number | null;
    skipCalendar: boolean;
    spinAmount: SpinTactic | null;
    dailyGuard: number | null;
    schedule: SavedSchedule | null;
    simulateDungeon: boolean;
    fightsForGold: number | null;
    doWeeklyTasks: boolean;
    drinkExtraWeeklyBeer: boolean;
    expeditionOptions: ExpeditionOptions;
    expeditionOptionsAfterSwitch: ExpeditionOptions;
    expeditionsInsteadOfQuests: boolean;
    questOptions: QuestOptions;
    questOptionsAfterSwitch: QuestOptions;
  },
};

export type ExpeditionOptions = {
  averageAmountOfChests: number | null;
  averageStarExperienceBonus: number | null;
};

export type QuestOptions = {
  priority: QuestPriority | null;
  hybridRatio: number | null;
}

export type FlatSimulationConfig = {
   characterName: string;
   level: number;
   experience: number;
   baseStat: number;
   goldPitLevel: number;
   academyLevel: number;
   gemMineLevel: number;
   treasuryLevel: number;
   hydraHeads: number;
   scrapbookFillness: number;
   xpGuildBonus: number;
   xpRuneBonus: number;
   hasExperienceScroll: boolean;
   tower: number;
   goldGuildBonus: number;
   goldRuneBonus: number;
   hasGoldScroll: boolean;
   hasArenaGoldScroll: boolean;
   mountType: MountType;
   calendar: number;
   calendarDay: number;
}
