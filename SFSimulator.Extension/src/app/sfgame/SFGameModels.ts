import type { ItemModel } from "./parsers/EquipmentParser";

export type SFGameRequest = {
  req: string;
  params: string | null;
  data: Record<string, string>;
  server: string;
}

export type Attributes = {
  Strength: number;
  Dexterity: number;
  Intelligence: number;
  Constitution: number;
  Luck: number;
};

export type Equipment = {
  Head: ItemModel,
  Body: ItemModel,
  Hand: ItemModel,
  Feet: ItemModel,
  Neck: ItemModel,
  Belt: ItemModel,
  Ring: ItemModel,
  Misc: ItemModel,
  Wpn1: ItemModel,
  Wpn2: ItemModel | null
};

export type Tower = {
  Underworld: { Gladiator: number; TimeMachineThirst: number };
  Companions: { Bert: Companion; Mark: Companion; Kunigunde: Companion };
};

export type Dungeons = {
  Light: { Position: number, Current: number }[],
  Shadow: { Position: number, Current: number }[],
  Tower: number,
  Twister: number,
  LoopOfIdols: number,
  Sandstorm: number
}

export type Expedition = {
  MainTask: ExpeditionEncounter,
  SideTasks: ExpeditionEncounter[],
  Time: number,
}

export type ExpeditionState = {
  Stage: number;
  CurrentStatus: ExpeditionStatus;
  CurrentHeroism: number;
};

export enum ExpeditionStatus {
  EncounterChoosing = 1,
  BossFight = 2,
  RewardChoosing = 3,
  Waiting = 4,
}

export type ExpeditionCrossroad = {
  Left: ExpeditionEncounter,
  Mid: ExpeditionEncounter,
  Right: ExpeditionEncounter,
}

export enum ExpeditionEncounter {
  None = 0,
  Skeleton_1 = 1,
  Skeleton_2 = 2,
  Skeleton_3 = 3,
  ToiletPaper = 11,
  DragonBait = 21,
  Dragon = 22,
  Campfire_1 = 31,
  Campfire_2 = 32,
  Campfire = 33,
  Unicorn_1 = 41,
  Unicorn_2 = 42,
  Unicorn_3 = 43,
  Unicorn = 44,
  SmallPig = 51,
  BigPig = 61,
  Trophy_1 = 71,
  Trophy_2 = 72,
  Trophy = 73,
  Couple_1 = 81,
  Couple_2 = 82,
  Couple = 83,
  BrokenSword_1 = 91,
  BrokenSword_2 = 92,
  BrokenSword = 93,
  Witch_1 = 101,
  Witch_2 = 102,
  Witch = 103,
  CursedWell_1 = 111,
  CursedWell = 112,
  Klaus_1 = 121,
  Klaus_2 = 122,
  Klaus_3 = 123,
  Klaus = 124,
  Key = 131,
  Chest = 132,
  FishingRod = 141,
  FishingBait = 142,
  Merman = 143,
  Mugs = 151,
  DraftBeer = 152,
  Barkeeper = 153,
  Chicken = 161,
  Tiger = 162,
  RidingStan = 163,
  Cupid = 171,
  LovestruckShakes = 172,
  LoveBirds = 173,
  Bounty_Skeleton = 1000,
  Bounty_ToiletPaper = 1001,
  Bounty_Dragon = 1002,
  Bounty_Campfire = 1003,
  Bounty_Unicorn = 1004,
  Bounty_Trophy = 1007,
  Bounty_Couple = 1008,
  Bounty_BrokenSword = 1009,
  Bounty_Witch = 1010,
  Bounty_CursedWell = 1011,
  Bounty_Klaus = 1012,
  Bounty_Merman = 1014,
  Bounty_Barkeeper = 1015,
  Bounty_Stan = 1016,
  Bounty_LoveBirds = 1017,
}

export type Witch = { Scrolls: ScrollType[] };

export enum ScrollType {
  Crit = 11,
  QuestSpeed = 41,
  QuestMushroom = 31,
  Reaction = 51,
  QuestExperience = 61,
  Beer = 71,
  QuestItems = 81,
  QuestGold = 91,
  ArenaGold = 101,
}

export type Pets = {
  ShadowArenaFought: boolean;
  LightArenaFought: boolean;
  EarthArenaFought: boolean;
  FireArenaFought: boolean;
  WaterArenaFought: boolean;
};

export type OwnPlayerSave = Attributes & {
  ID: number;
  Level: number;
  Class: Class;
  Armor: number;
  GuildPortal: number;
  PortalHealthBonus: number;
};

export type CharacterStatus = {
  MaxBeers: number;
  ThirstLeft: number;
  UsedBeers: number;
};

export type Companion = Attributes & { Level: number };

export enum Class {
  Warrior = 1,
  Mage = 2,
  Scout = 3,
  Assassin = 4,
  BattleMage = 5,
  Berserker = 6,
  DemonHunter = 7,
  Druid = 8,
  Bard = 9,
  Necromancer = 10,
  Paladin = 11,
  PlagueDoctor = 12,
  BloodWeaver = 13,
}

export enum RuneType {
  None = 0,
  GoldBonus = 1,
  EpicChance = 2,
  ItemQuality = 3,
  ExperienceBonus = 4,
  HealthBonus = 5,
  FireResistance = 6,
  ColdResistance = 7,
  LightningResistance = 8,
  TotalResistance = 9,
  FireDamage = 10,
  ColdDamage = 11,
  LightningDamage = 12
}

export enum EventType {
  ExceptionalXPEvent = 0,
  GloriousGoldGalore = 1,
  TidyToiletTime = 2,
  AssemblyOfAwesomeAnimals = 3,
  FantasticFortressFestivity = 4,
  DaysOfDoomedSouls = 5,
  WitchesDance = 6,
  SandsOfTimeSpecial = 7,
  ForgeFrenzyFestival = 8,
  EpicShoppingSpreeExtravaganza = 9,
  EpicQuestExtravaganza = 10,
  EpicGoodLuckExtravaganza = 11,
  OneBeerTwoBeerFreeBeer = 12,
  PieceworkParty = 13,
  LuckyDay = 14,
  CrazyMushroomHarvest = 15,
  HolidaySale = 16,
  ValentinesBlessing = 17,
  BlackGemRush = 18,
  RumbleForRiches = 19,
}

export type DailyTaskReward = {
  Claimed: boolean;
  PointsRequired: number;
};

export type DailyTask = {
  Points: number;
  Completed: boolean;
};

export type ToiletState = { AmountOfItemsToSacrifice: number };

export type Guild = { PetMaxLevel: number };

export type PortalProgress = { Finished: number; CanAttack: boolean };
