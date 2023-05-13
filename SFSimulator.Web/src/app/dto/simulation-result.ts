export type SimulationResult = {
  days: number;
  characterAfter: Character;
  characterPreviously: Character;
  simulatedDays: SimulatedDay[];
}

export type SimulatedDay = {
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
}
