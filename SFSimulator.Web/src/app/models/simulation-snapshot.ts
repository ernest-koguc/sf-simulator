import { addDays } from "../helpers/date-helper";
import { roundValues } from "../helpers/round-values";
import { ChartConfig } from "./chart";
import { BaseStatGain, Character, ExperienceGain, SimulationResult } from "./simulation-result";
 
export class SavedSimulationSnapshot {
  constructor(simulationResult: SimulationResult) {
    var timestamp = Date.now();
    var startDate = new Date(timestamp);
    this.timestamp = timestamp;
    this.characterName = simulationResult.characterName ? simulationResult.characterName : '';
    this.startDate = startDate.toLocaleDateString();
    this.endDate = addDays(startDate, simulationResult.days).toLocaleDateString();
    this.daysPassed = simulationResult.days;
    this.levelDifference = simulationResult.characterPreviously.level + ' > ' + simulationResult.characterAfter.level;
    this.baseStatDifference = simulationResult.characterPreviously.baseStat + ' > ' + simulationResult.characterAfter.baseStat;
    this.description = '';
    this.characterBeforeSimulation = simulationResult.characterPreviously;
    this.characterAfterSimulation = simulationResult.characterAfter;
    this.avgBaseStatGain = roundValues(simulationResult.averageGains.baseStatGain)
    this.avgXPGain = roundValues(simulationResult.averageGains.experienceGain);
    this.totalBaseStatGain = roundValues(simulationResult.totalGains.baseStatGain);
    this.totalXPGain = roundValues(simulationResult.totalGains.experienceGain);
  }
  timestamp: number;
  characterName: string;
  startDate: string;
  endDate: string;
  daysPassed: number;
  levelDifference: string;
  baseStatDifference: string;
  description: string;
  characterBeforeSimulation: Character;
  characterAfterSimulation: Character;
  avgBaseStatGain: BaseStatGain;
  avgXPGain: ExperienceGain;
  totalBaseStatGain: BaseStatGain;
  totalXPGain: ExperienceGain;
}
export interface SimulationSnapshotTableRecord extends SavedSimulationSnapshot {
  timestamp: number;
  characterName: string;
  startDate: string;
  endDate: string;
  daysPassed: number;
  levelDifference: string;
  baseStatDifference: string;
  description: string;
  characterBeforeSimulation: Character;
  characterAfterSimulation: Character;
  avgBaseStatGain: BaseStatGain;
  avgXPGain: ExperienceGain;
  totalBaseStatGain: BaseStatGain;
  totalXPGain: ExperienceGain;
  chartsEnabled: boolean;
  avgBaseStatChart: ChartConfig | null;
  avgXPChart: ChartConfig | null;
  totalBaseStatChart: ChartConfig | null;
  totalXPChart: ChartConfig | null;
}
