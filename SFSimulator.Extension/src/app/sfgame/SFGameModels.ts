import { ItemModel } from "./sfgame-parser";
import { CompanionModel } from "./parsers/TowerParser";

export type SFGameRequest = {
  req: string;
  params: string | null;
  data: Record<string, string>;
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
    Bert: CompanionModel,
    Mark: CompanionModel,
    Kunigunde: CompanionModel
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

export type OwnPlayerSave = {
  Toilet: { Aura: number, Mana: number, Capacity: number },
  Witch: {
    Stage: number,
    Items: number,
    ItemsNext: number,
    Item: number,
    Finish: number,
    Scrolls: {
      Date: number,
      Type: number,
      Owned: boolean
    }[]
  },
  ID: number,
  Registered: number,
  Level: number,
  XP: number,
  XPNext: number,
  Honor: number,
  Rank: number,
  DevilPercent: number,
  Class: number,
  Action: { Status: number, Index: number, Finish: number },
  Strength: Attribute,
  Dexterity: Attribute,
  Intelligence: Attribute,
  Constitution: Attribute,
  Luck: Attribute,
  Items: Equipment,
  Inventory: {
    Backpack: ItemModel[],
    Chest: ItemModel[],
    Shop: ItemModel[],
    Dummy: Equipment | null,
  },
  Mount: number,
  MountValue: number,
  Group: { ID: number, Name: string | null, Joined?: number, Identifier?: string, Treasure?: number, Instructor?: number, Pet?: number },
  Book: number,
  Armor: number,
  Damage: { Min: number, Max: number },
  Damage2?: { Min: number, Max: number },
  MountExpire: number,
  ThirstReroll: number,
  ThirstLeft: number,
  UsedBeers: number,
  Potions: Potion[],
  PotionsLife: number,
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
    SecretWoodLimit: number,
    SecretStoneLimit: number,
  },
  CalendarDay: number,
  CalendarType: number,
  LegendaryDungeonTries: number,
  UsedAdventureTime: number,
  ClientVersion: number,
  AdventureSkips: number,
  Summer: {
    Missions: { Type: number, Current: number, Target: number, Points: number }[],
    TotalPoints: number
  },
  Pets: {
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
    TotalCount: number,
    Shadow: number,
    Light: number,
    Earth: number,
    Fire: number,
    Water: number,
    Dungeons: number[],
    Rank: number,
    Honor: number,
    TotalLevel: number,
  },
  Name: string,
  GuildRaid: number,
  GuildPortal: number,
  DailyTasks: {
    Rewards: {
      Collected: boolean,
      Points: number,
      ResourceType: number,
      ResourceAmount: number
    }[]
  },
  EventTasks: {
    Rewards: {
      Collected: boolean,
      Points: number,
      ResourceType: number,
      ResourceAmount: number
    }[]
  },
  ItemsArray: ItemModel[],
  ClassBonus: boolean,
  Runes: {
    Gold: number,
    Chance: number,
    Quality: number,
    XP: number,
    Health: number,
    ResistanceFire: number,
    ResistanceCold: number,
    ResistanceLightning: number,
    Damage: number,
    DamageFire: number,
    DamageCold: number,
    DamageLightning: number,
    Damage2: number,
    Damage2Fire: number,
    Damage2Cold: number,
    Damage2Lightning: number,
    Resistance: number,
    Runes: number,
    Achievements: number,
  },
  Config?: any,
  BlockChance: number,
  Primary: Attribute,
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


