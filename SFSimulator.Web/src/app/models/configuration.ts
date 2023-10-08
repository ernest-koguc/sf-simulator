import { mapToConfigurationCharacter, mapToConfigurationPlaystyle } from "../helpers/mapper";
import { MountType } from "./mount-type";
import { QuestPriority } from "./quest-priority";
import { SimulationConfigForm } from "./simulation-configuration";
import { SpinTactic } from "./spin-tactics";

export class SavedConfiguration {
  constructor(name: string, simulationOptions: SimulationConfigForm, includeCharacter: boolean = false) {
    this.name = name;
    this.timestamp = Date.now();

    updateSavedConfiguration(this, simulationOptions, includeCharacter);
  }

  public timestamp: number;
  public name: string;
  public scheduleId!: number | 'Default';
  public playstyle!: ConfigurationPlaystyle;
  public character: ConfigurationCharacter | undefined;

}

export function updateSavedConfiguration(configuration: SavedConfiguration, simulationOptions: SimulationConfigForm, includeCharacter: boolean) {
    configuration.playstyle = mapToConfigurationPlaystyle(simulationOptions);

    if (simulationOptions.schedule)
      configuration.scheduleId = simulationOptions.schedule?.timestamp;
    else
      configuration.scheduleId = 'Default';

    if (includeCharacter)
      configuration.character = mapToConfigurationCharacter(simulationOptions);
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
