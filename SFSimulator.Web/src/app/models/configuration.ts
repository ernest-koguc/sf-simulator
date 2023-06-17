import { mapToConfigurationCharacter, mapToConfigurationPlaystyle } from "../helpers/mapper";
import { MountType } from "./mount-type";
import { QuestPriority } from "./quest-priority";
import { SimulationOptionsForm } from "./simulation-options";
import { SpinTactic } from "./spin-tactics";

export class SavedConfiguration {
  constructor(name: string, simulationOptions: SimulationOptionsForm, includeCharacter: boolean = false) {
    this.name = name;
    this.timestamp = Date.now();

    this.updateConfiguration(simulationOptions, includeCharacter);
  }

  public timestamp: number;
  public name: string;
  public scheduleId!: number | 'Default';
  public playstyle!: ConfigurationPlaystyle;
  public character: ConfigurationCharacter | undefined;

  public updateConfiguration(simulationOptions: SimulationOptionsForm, includeCharacter: boolean) {
    this.playstyle = mapToConfigurationPlaystyle(simulationOptions);

    if (simulationOptions.schedule)
      this.scheduleId = simulationOptions.schedule?.timestamp;
    else
      this.scheduleId = 'Default';

    if (includeCharacter)
      this.character = mapToConfigurationCharacter(simulationOptions);
  }
}

export type ConfigurationPlaystyle = {
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
}

export type ConfigurationCharacter = {
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
}
