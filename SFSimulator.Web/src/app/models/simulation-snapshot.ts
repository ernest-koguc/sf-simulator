import { addDays } from "../helpers/date-helper";
import { roundValues } from "../helpers/round-values";
import { ChartConfig } from "./chart";
import { BaseStatGain, ExperienceGain, SimulationResult } from "./simulation-result";
 
export class SavedSimulationSnapshot {
  constructor(simulationResult: SimulationResult, beforeSimulation: CharacterSnapshot, characterName?: string | null) {
    var timestamp = Date.now();
    var startDate = new Date(timestamp);
    this.timestamp = timestamp;
    this.characterName = characterName ?? '';
    this.startDate = startDate.toLocaleDateString();
    this.endDate = addDays(startDate, simulationResult.days).toLocaleDateString();
    this.daysPassed = simulationResult.days;
    this.levelDifference = beforeSimulation.level + ' > ' + simulationResult.level;
    this.baseStatDifference = beforeSimulation.baseStat + ' > ' + simulationResult.baseStat;
    this.description = '';
    this.characterBeforeSimulation = beforeSimulation;
    this.characterAfterSimulation = { level: simulationResult.level, experience: simulationResult.experience, baseStat: simulationResult.baseStat };
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
  characterBeforeSimulation: CharacterSnapshot;
  characterAfterSimulation: CharacterSnapshot;
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
  characterBeforeSimulation: CharacterSnapshot;
  characterAfterSimulation: CharacterSnapshot;
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

export type CharacterSnapshot = {
  level: number,
  experience: number,
  baseStat: number
}
