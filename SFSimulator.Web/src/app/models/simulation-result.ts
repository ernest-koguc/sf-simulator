export type SimulationResult = {
  days: number;
  level: number;
  experience: number;
  baseStat: number;
  simulatedDays: SimulatedGains[];
  totalGains: SimulatedGains;
  averageGains: SimulatedGains;
}

export type SimulatedGains = {
  dayId: number;
  experienceGain: ExperienceGain;
  baseStatGain: BaseStatGain;
}

export type ExperienceGain = {
  QUEST: number;
  CALENDAR: number;
  TIME_MACHINE: number;
  WHEEL: number;
  ACADEMY: number;
  DAILY_MISSION: number;
  ARENA: number;
  GUILD_FIGHT: number;
  TOTAL: number;
}

export type BaseStatGain = {
  QUEST: number;
  CALENDAR: number;
  TIME_MACHINE: number;
  WHEEL: number;
  GOLD_PIT: number;
  GUARD: number;
  GEM: number;
  ITEM: number;
  DICE_GAME: number;
  TOTAL: number;
}

export type DungeonResult = {
  iterations: number,
  wonFights: number,
  experience: number,
  succeeded: boolean
}

export const BaseStatKeys: (keyof BaseStatGain)[] = ['QUEST', 'CALENDAR', 'TIME_MACHINE', 'WHEEL', 'GOLD_PIT', 'GUARD', 'GEM', 'ITEM', 'DICE_GAME'];
export const ExperienceKeys: (keyof ExperienceGain)[] = ['QUEST', 'CALENDAR', 'TIME_MACHINE', 'WHEEL', 'ACADEMY', 'DAILY_MISSION', 'ARENA', 'GUILD_FIGHT'];
