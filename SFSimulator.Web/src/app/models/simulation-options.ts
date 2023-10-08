export interface SimulationOptions {
  simulationType: SimulationType;
  simulateUntil: number;
};

export type SimulationType = 'Days' | 'Level';
