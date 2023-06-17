import { ConfigurationCharacter, ConfigurationPlaystyle } from "../models/configuration";
import { SimulationOptionsForm } from "../models/simulation-options";
import { SimulationResult } from "../models/simulation-result";
import { SimulationSnapshot, SimulationSnapshotTableRecord } from "../models/simulation-snapshot";
import { addDays } from "./date-helper";
import { roundValues } from "./round-values";

export function mapToSimulationSnapshot(data: SimulationResult): SimulationSnapshot {
  var timestamp = Date.now();
  var startDate = new Date(timestamp);
  var simulationSnapshot: SimulationSnapshot = {
    timestamp: timestamp,
    characterName: data.characterName ? data.characterName : '',
    startDate: startDate.toLocaleDateString(),
    endDate: addDays(startDate, data.days).toLocaleDateString(),
    daysPassed: data.days,
    levelDifference: data.characterPreviously.level + ' > ' + data.characterAfter.level,
    baseStatDifference: data.characterPreviously.baseStat + ' > ' + data.characterAfter.baseStat,
    description: 'Essa',
    characterBeforeSimulation: data.characterPreviously,
    characterAfterSimulation: data.characterAfter,
    avgBaseStatGain: roundValues(data.averageGains.baseStatGain),
    avgXPGain: roundValues(data.averageGains.experienceGain),
    totalBaseStatGain: roundValues(data.totalGains.baseStatGain),
    totalXPGain: roundValues(data.totalGains.experienceGain)
  }
  return simulationSnapshot;
}

export function mapToSimulationSnapshotTableRecord(data: SimulationSnapshot): SimulationSnapshotTableRecord {
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
