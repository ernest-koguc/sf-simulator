import { SimulationSnapshot } from "../layout/simulation-result/simulation-result.component";
import { SimulationResult } from "../models/simulation-result";
import { addDays } from "./date-helper";
import { roundValues } from "./round-values";

export function mapToSimulationSnaphsot(data: SimulationResult) {
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
