import { ChartConfig } from "./chart";
import { BaseStatGain, Character, ExperienceGain } from "./simulation-result";

export interface SimulationSnapshot {
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
export interface SimulationSnapshotTableRecord extends SimulationSnapshot {
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
