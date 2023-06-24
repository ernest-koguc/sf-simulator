import { ConfigurationCharacter, ConfigurationPlaystyle } from "../models/configuration";
import { SimulationOptionsForm } from "../models/simulation-options";
import { SavedSimulationSnapshot, SimulationSnapshotTableRecord } from "../models/simulation-snapshot";

export function mapToSimulationSnapshotTableRecord(data: SavedSimulationSnapshot): SimulationSnapshotTableRecord {
  return { ...data, chartsEnabled: false, avgBaseStatChart: null, totalBaseStatChart: null, avgXPChart: null, totalXPChart: null }
}

export function mapToConfigurationPlaystyle(data: SimulationOptionsForm) {
  var playstyle: ConfigurationPlaystyle = {
    questPriority: data.questPriority,
    hybridRatio: data.hybridRatio,
    switchPriority: data.switchPriority,
    switchLevel: data.switchLevel,
    priorityAfterSwitch: data.priorityAfterSwitch,
    drinkBeerOneByOne: data.drinkBeerOneByOne,
    dailyThirst: data.dailyThirst,
    skipCalendar: data.skipCalendar,
    spinAmount: data.spinAmount,
    dailyGuard: data.dailyGuard
  }
  return playstyle;
}

export function mapToConfigurationCharacter(data: SimulationOptionsForm): ConfigurationCharacter {
  var character: ConfigurationCharacter = {
  characterName: data.characterName,
  level: data.level,
  baseStat: data.baseStat,
  experience: data.experience,
  goldPitLevel: data.goldPitLevel,
  academyLevel: data.academyLevel,
  hydraHeads: data.hydraHeads,
  gemMineLevel: data.gemMineLevel,
  treasuryLevel: data.treasuryLevel,
  mountType: data.mountType,
  scrapbookFillness: data.scrapbookFillness,
  xpGuildBonus: data.xpGuildBonus,
  xpRuneBonus: data.xpRuneBonus,
  hasExperienceScroll: data.hasExperienceScroll,
  tower: data.tower,
  goldGuildBonus: data.goldGuildBonus,
  goldRuneBonus: data.goldRuneBonus,
  hasGoldScroll: data.hasGoldScroll
  }
  return character;
}
