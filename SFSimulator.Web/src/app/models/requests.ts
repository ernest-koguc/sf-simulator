import { Character, ClassType, ResistanceRuneBonuses, Weapon } from "./character";
import { MountType } from "./mount-type";
import { QuestPriority } from "./quest-priority";
import { SavedSchedule } from "./schedule";
import { SimulationConfigForm } from "./simulation-configuration";
import { SpinTactic } from "./spin-tactics";

export type SimulateDayRequest = {
  daysCount: number | null,
  level: number | null,
  experience: number | null,
  baseStat: number | null,
  class: ClassType | null,
  strength: number | null,
  dexterity: number | null,
  intelligence: number | null,
  constitution: number | null,
  luck: number | null,
  armor: number | null,
  firstWeapon: Weapon | undefined,
  secondWeapon: Weapon | undefined
  runeBonuses: ResistanceRuneBonuses,
  hasGlovesScroll: boolean,
  hasWeaponScroll: boolean,
  hasEternityPotion: boolean,
  gladiatorLevel: number,
  soloPortal: number, 
  guildPortal: number,
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
  simulateDungeon: boolean | null;
}

export type SimulateUntilLevelRequest = {
  simulationOptions: SimulationConfigForm,
  untilLevel: number,
  level: number,
  experience: number,
  baseStat: number,
  class: ClassType,
  strength: number,
  dexterity: number,
  intelligence: number,
  constitution: number,
  luck: number,
  armor: number,
  firstWeapon: Weapon | undefined,
  secondWeapon: Weapon | undefined
  runeBonuses: ResistanceRuneBonuses,
  hasGlovesScroll: boolean,
  hasWeaponScroll: boolean,
  hasEternityPotion: boolean,
  gladiatorLevel: number,
  soloPortal: number, 
  guildPortal: number,
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
  simulateDungeon: boolean | null;
}
