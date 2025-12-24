import { ItemModel } from "./parsers/EquipmentParser";

export type SFGameRequest = {
  req: string;
  params: string | null;
  data: Record<string, string>;
  server: string;
}

export type Attribute = {
  Type: number,
  Base: number,
  Bonus: number,
  Purchased: number,
  Items: number,
  Gems: number,
  Upgrades: number,
  Class: number,
  Potion: number,
  Pet: number,
  Total: number,
}

export type Potion = {
  Type: number,
  Expire: number,
  Size: number
}

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
  Underworld: {
    Heart: number,
    Gate: number,
    GoldPit: number,
    Extractor: number,
    GoblinPit: number,
    Torture: number,
    Gladiator: number,
    TrollBlock: number,
    TimeMachine: number,
    Keeper: number,
    ExtractorSouls: number,
    ExtractorMax: number,
    MaxSouls: number,
    ExtractorHourly: number,
    GoldPitGold: number,
    GoldPitMax: number,
    GoldPitHourly: number,
    TimeMachineThirst: number,
    TimeMachineMax: number,
    TimeMachineDaily: number,
  };
  Companions: {
    Bert: Companion,
    Mark: Companion,
    Kunigunde: Companion,
  };
}

export type Dungeons = {
  Light: { Position: number, Current: number }[],
  Shadow: { Position: number, Current: number }[],
  SoloPortal: number,
  Tower: number,
  Twister: number,
  LoopOfIdols: number,
  Sandstorm: number
}

export type Resources = {
  Mushrooms: number,
  Gold: number,
  LuckyCoins: number,
  Hourglass: number,
  Wood: number,
  SecretWood: number,
  Stone: number,
  SecretStone: number,
  Metal: number,
  Crystals: number,
  Souls: number,
  ShadowFood: number,
  LightFood: number,
  EarthFood: number,
  FireFood: number,
  WaterFood: number,
}

export type Expedition = {
  MainTask: ExpeditionEncounter,
  SideTasks: ExpeditionEncounter[],
  Time: number,
}

export type ExpeditionState = {
  Stage: number,
  CurrentLocation: number,
  CurrentStatus: ExpeditionStatus,
  MainTask: ExpeditionEncounter,
  SideTasks: ExpeditionEncounter[],
  MainTaskFinishedCounter: number,
  MainTaskMaxCounter: number,
  Backpack: ExpeditionEncounter[],
  CurrentHeroism: number,
  CurrentStars: number,
  WaitingStarted: number,
  WaitingUntil: number,
  TaskBonus: number,
  TaskPenalty: number,
  BonusForRepeatableTask: number,
  BonusLimitForRepeatableTask: number,
}

export enum ExpeditionStatus {
  EncounterChoosing = 1,
  BossFight = 2,
  RewardChoosing = 3,
  Waiting = 4,
}

export type ExpeditionCrossroad = {
  Left: ExpeditionEncounter,
  LeftHeroism: number,
  Mid: ExpeditionEncounter,
  MidHeroism: number,
  Right: ExpeditionEncounter,
  RightHeroism: number,
}

export type ExpeditionHalfTime = {
  LeftReward: {
    Type: ResourceType,
    Amount: number,
  },
  MidReward: {
    Type: ResourceType,
    Amount: number,
  },
  RightReward: {
    Type: ResourceType,
    Amount: number,
  },
}

export type ExpeditionRewardResources = {
  Resources: {
    Type: ResourceType,
    Amount: number,
  }[]
}

export type ExpeditionItemReward = {
  GoldValue: number,
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
  Bounty_Skeleton = 1000,
  Bounty_Dragon = 1002,
  Bounty_Campfire = 1003,
  Bounty_Unicorn = 1004,
  Bounty_Trophy = 1007,
  Bounty_Couple = 1008,
  Bounty_BrokenSword = 1009,
  Bounty_Witch = 1010,
  Bounty_CursedWell = 1011,
  Bounty_Klaus = 1012
}

export enum ResourceType {
  None = 0,
  Mushroom = 3,
  Gold = 4,
  Wood = 6,
  Stone = 7,
  Splinters = 8,
  Metal = 9,
  Souls = 10,
  Fruit = 23,
  Experience = 24,
  PetEgg = 25,
  Hourglass = 26,
  Honor = 27,
}

export type Witch = {
  Stage: number,
  Items: number,
  ItemsNext: number,
  ItemForCauldron: number,
  Finish: number,
  Scrolls: {
    Date: number,
    Type: ScrollType,
    Unlocked: boolean
  }[]
}

export enum ScrollType {
  None = 0,
  Crit = 11,
  QuestSpeed = 21,
  QuestMushroom = 31,
  Reaction = 51,
  QuestExperience = 61,
  Beer = 71,
  QuestItems = 81,
  QuestGold = 91,
  ArenaGold = 101,
}

export type Pets = {
  Levels: number[],
  ShadowLevels: number[],
  LightLevels: number[],
  EarthLevels: number[],
  FireLevels: number[],
  WaterLevels: number[],
  ShadowCount: number,
  LightCount: number,
  EarthCount: number,
  FireCount: number,
  WaterCount: number,
  ShadowLevel: number,
  LightLevel: number,
  EarthLevel: number,
  FireLevel: number,
  WaterLevel: number,
  TotalCount: number;
  Shadow: number,
  Light: number,
  Earth: number,
  Fire: number,
  Water: number,
  Dungeons: number[],
  Rank: number,
  Honor: number,
  TotalLevel: number,
  ShadowArenaFought: boolean,
  LightArenaFought: boolean,
  EarthArenaFought: boolean,
  WaterArenaFought: boolean,
  FireArenaFought: boolean,
}

export type OwnPlayerSave = {
  Toilet: { Aura: number, Mana: number, Capacity: number };
  ID: number;
  Registered: number;
  Level: number;
  XP: number;
  XPNext: number;
  Honor: number;
  Rank: number;
  DevilPercent: number;
  Class: number;
  Action: { Status: number, Index: number, Finish: number };
  Strength: Attribute;
  Dexterity: Attribute;
  Intelligence: Attribute;
  Constitution: Attribute;
  Luck: Attribute;
  Mount: number;
  MountValue: number;
  Group: { ID: number, Joined: number, Treasure: number, Instructor: number, Pet: number, CanAttackHydra: boolean };
  Book: number;
  Armor: number;
  Damage: { Min: number, Max: number };
  Damage2?: { Min: number, Max: number };
  MountExpire: number;
  /**
   * Indicates when the Thirst going to be reset (e.g. midnight)
   *
   **/
  ThirstReroll: number;
  ThirstLeft: number;
  UsedBeers: number;
  MaxBeers: number;
  Potions: Potion[];
  HasEternalPotion: boolean;
  Fortress: {
    Fortress: number,
    LaborerQuarters: number,
    WoodcutterGuild: number,
    Quarry: number,
    GemMine: number,
    Academy: number,
    ArcheryGuild: number,
    Barracks: number,
    MageTower: number,
    Treasury: number,
    Smithy: number,
    Fortifications: number,
    Knights: number,
    Upgrade: { Building: number, Finish: number, Start: number },
    Upgrades: number,
    Honor: number,
    WoodcutterMax: number,
    QuarryMax: number,
    AcademyMax: number,
    MaxWood: number,
    MaxStone: number,
    SecretWoodLimit: number;
    SecretStoneLimit: number;
  };
  CalendarDay: number;
  CalendarType: number;
  LegendaryDungeonTries: number;
  UsedAdventureTime: number;
  ClientVersion: number;
  AdventureSkips: number;
  Summer: {
    Missions: { Type: number, Current: number, Target: number, Points: number }[],
    TotalPoints: number
  };
  GuildRaid: number;
  GuildPortal: number;
  WheelSpinsToday: number;
  DiceGamesRemaining: number;
  ArenaFightsToday: number;
}

export type Companion = {
  Armor: number,
  Damage: {
    Min: number,
    Max: number
  },
  Strength: Attribute,
  Dexterity: Attribute,
  Intelligence: Attribute,
  Constitution: Attribute,
  Luck: Attribute,
}

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
  Claimed: boolean,
  PointsRequired: number,
  Rewards: { ResourceType: ResourceType, Amount: number }[]
}

export type DailyTask = {
  TaskType: number,
  Current: number,
  Target: number,
  Points: number,
  Completed: boolean
}

export type ToiletState = {
  AmountOfItemsToSacrifice: number
}

export type Guild = {
  Id: number,
  TotalTreasure: number,
  PortalLife: number,
  TotalInstructor: number,
  Portal: number,
  Honor: number,
  Raids: number,
  PetId: number,
  PetMaxLevel: number,
}

export type PortalProgress = {
  Finished: number,
  HpPercentage: number,
  CanAttack: boolean,
}
