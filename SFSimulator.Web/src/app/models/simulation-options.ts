import { MountType } from "./mount-type";
import { QuestPriority } from "./quest-priority";
import { SavedSchedule } from "./schedule";
import { SpinTactic } from "./spin-tactics";

export interface SimulationOptionsForm {
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
  scrapbookFillness: number | null;
  xpGuildBonus: number | null;
  xpRuneBonus: number | null
  hasExperienceScroll: boolean | null;
  tower: number | null
  goldGuildBonus: number | null;
  goldRuneBonus: number | null;
  hasGoldScroll: boolean | null;
  questPriority: QuestPriority | null;
  hybridRatio: number | null;
  switchPriority: boolean | null;
  switchLevel: number | null;
  priorityAfterSwitch: QuestPriority | null;
  drinkBeerOneByOne: boolean | null;
  dailyThirst: number | null;
  skipCalendar: boolean | null;
  spinAmount: SpinTactic | null;
  dailyGuard: number | null;
  schedule: SavedSchedule | null;
};
