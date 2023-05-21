export type SimulationResult = {
  characterName: string;
  days: number;
  characterAfter: Character;
  characterPreviously: Character;
  simulatedDays: SimulatedGains[];
  totalGains: SimulatedGains;
  averageGains: SimulatedGains;
}

export type SimulatedGains = {
  dayId: number;
  experienceGain: ExperienceGain;
  baseStatGain: BaseStatGain;
}

export type Character = {
  level: number;
  experience: number;
  baseStat: number;
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

export const BaseStatKeys: (keyof BaseStatGain)[] = ['QUEST', 'CALENDAR', 'TIME_MACHINE', 'WHEEL', 'GOLD_PIT', 'GUARD', 'GEM', 'ITEM', 'DICE_GAME'];
export const ExperienceKeys: (keyof ExperienceGain)[] = ['QUEST', 'CALENDAR', 'TIME_MACHINE', 'WHEEL', 'ACADEMY', 'DAILY_MISSION', 'ARENA', 'GUILD_FIGHT'];
