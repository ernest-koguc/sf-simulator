export interface SimulationOptions {
  simulationType: SimulationType;
  simulateUntil: number;
};

export enum SimulationType {
    UntilDays = 0,
    UntilLevel = 1,
    UntilBaseStats = 2
}
