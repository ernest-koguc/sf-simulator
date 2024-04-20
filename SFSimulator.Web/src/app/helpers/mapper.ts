import { ConfigurationCharacter, ConfigurationPlaystyle } from "../models/configuration";
import { SimulationConfigForm } from "../models/simulation-configuration";
import { SavedSimulationSnapshot, SimulationSnapshotTableRecord } from "../models/simulation-snapshot";

export function mapToSimulationSnapshotTableRecord(data: SavedSimulationSnapshot): SimulationSnapshotTableRecord {
  return { ...data, chartsEnabled: false, avgBaseStatChart: null, totalBaseStatChart: null, avgXPChart: null, totalXPChart: null }
}

export function mapToConfigurationPlaystyle(data: SimulationConfigForm) {
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
    dailyGuard: data.dailyGuard,
    simulateDungeon: data.simulateDungeon,
    fightsForGold: data.fightsForGold,
    doWeeklyTasks: data.doWeeklyTasks,
    drinkExtraWeeklyBeer: data.drinkExtraWeeklyBeer,
    expeditionOptions: data.expeditionOptions,
    expeditionOptionsAfterSwitch: data.expeditionOptionsAfterSwitch,
    expeditionsInsteadOfQuests: data.expeditionsInsteadOfQuests
  }
  return playstyle;
}

export function mapToConfigurationCharacter(data: SimulationConfigForm): ConfigurationCharacter {
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

export function mapToLowerCase(data: any) {
    var key, keys = Object.keys(data);
    var n = keys.length;
    var mappedData: any = {};
    while (n--) {
      key = keys[n][0].toLowerCase();
      var lowerCaseName = key + keys[n].substring(1);
      let property = data[keys[n]];
      if (property !== null && typeof property !== 'string' && Object.keys(property).length) {
        mappedData[lowerCaseName] = mapToLowerCase(property);
      }
      else {
        mappedData[lowerCaseName] = property;
      }
    }

    return mappedData;
}
