import { OwnPlayerSave } from "../SFGameModels";
import { ByteParser } from "./ByteParser";

export function parseOwnPlayerSave(ownSaveData: number[]): OwnPlayerSave {
  const dataType = new ByteParser(ownSaveData);
  dataType.assert(652);

  dataType.skip(1);
  const id = dataType.long();
  // TODO: check if it is valid without offset
  dataType.skip(1);
  const registered = dataType.long() * 1000;// + data.offset;
  dataType.skip(3);
  const level = dataType.short();
  const arenaFights = dataType.short();
  dataType.clear();
  const xp = dataType.long();
  const xpNext = dataType.long();
  const honor = dataType.long();
  const rank = dataType.long();
  dataType.short();
  const devilPercent = dataType.short();
  const maxBeers = dataType.long()
  dataType.skip(13);
  dataType.short();
  dataType.clear();
  dataType.short();
  dataType.short();
  const classType = dataType.short();
  dataType.clear();
  const attributes = getAttributes(dataType, false);
  const status = dataType.short()
  dataType.short()
  const index = dataType.short()
  dataType.short();
  const finish = dataType.long() * 1000;// + data.offset;
  const action = {
    Status: status,
    Index: index,
    Finish: finish
  };
  dataType.skip(120);
  dataType.skip(60);
  dataType.skip(58);
  const mount = dataType.short();
  const mountValue = MOUNT_MAP[mount];

  dataType.short();

  dataType.skip(1);
  dataType.skip(72);
  dataType.skip(1);
  dataType.skip(72);
  const guildRaid = dataType.short();

  dataType.short();
  dataType.skip(1);
  const groupId = dataType.long();
  dataType.skip(2);

  const book = Math.max(0, dataType.long() - 10000);

  dataType.skip(4);

  const groupJoined = dataType.long() * 1000;// + data.offset;
  dataType.skip(1);
  dataType.short();

  const guildPortal = dataType.byte();
  dataType.byte()

  dataType.skip(1);
  const armor = dataType.long();
  const damage = {
    Min: dataType.long(),
    Max: dataType.long()
  };
  dataType.skip(1);
  const mountExpire = dataType.long() * 1000;//data.offset,
  dataType.skip(3);
  const thirstReroll = dataType.long() * 1000;// + data.offset;
  const thirstLeft = dataType.long();
  const usedBeer = dataType.long();
  dataType.skip(33);

  let aura = dataType.long();
  let mana = dataType.long();
  const potions = [{
    Type: getPotionType(dataType.long()),
    Expire: dataType.skip(2).long() * 1000,// + data.offset,
    Size: dataType.skip(2).long()
  }, {
    Type: getPotionType(dataType.back(6).long()),
    Expire: dataType.skip(2).long() * 1000,// + data.offset,
    Size: dataType.skip(2).long()
  }, {
    Type: getPotionType(dataType.back(6).long()),
    Expire: dataType.skip(2).long() * 1000,// + data.offset,
    Size: dataType.skip(2).long()
  }];
  const potionsLife = dataType.long() > 0;
  dataType.skip(12);
  const toilet = {
    Aura: aura,
    Mana: mana,
    Capacity: dataType.long()
  };
  dataType.skip(8);
  const fort = dataType.long();
  const labQuarte = dataType.long();
  const woodcutterGuild = dataType.long();
  const quarry = dataType.long();
  const gemMine = dataType.long();
  const academy = dataType.long();
  const archeryGuild = dataType.long();
  const barracks = dataType.long();
  const mageTower = dataType.long();
  const treasury = dataType.long();
  const smithy = dataType.long();
  const fortifications = dataType.long();

  dataType.skip(29);

  const woodcutterMax = dataType.long();
  const quarryMax = dataType.long();
  const academyMax = dataType.long();
  const maxWood = dataType.long();
  const maxStone = dataType.long();
  dataType.skip(1);
  const fortUpgrade = {
    Building: dataType.long() - 1,
    Finish: dataType.long() * 1000,// + data.offset,
    Start: dataType.long() * 1000// + data.offset
  }

  dataType.skip(7);

  const fortUpgrades = dataType.long();
  const fortHonor = dataType.long();
  dataType.skip(15);
  const knights = dataType.long();
  dataType.skip(5);

  //TODO: improve so we can just skip through easier
  dataType.byteArray(14);

  dataType.clear();
  dataType.skip(15);
  const groupTreasure = dataType.long();
  const groupInstructor = dataType.long();
  dataType.skip(4);
  const groupPet = dataType.long();
  dataType.skip(18);

  dataType.short();

  const calendarDay = dataType.short();
  dataType.skip(8);

  // Normalize calendar type in order to align it with S&F Tavern's calendar indexing
  const calendarType = 1 + (dataType.long() + 10) % 12;
  dataType.skip(4);
  const legendaryDungeonTries = dataType.long();
  dataType.skip(2);
  const usedAdventureTime = dataType.long();
  dataType.skip(5);
  const clientVersion = dataType.long();
  const adventureSkips = dataType.long();
  const summer = {
    Missions: [
      {
        Type: dataType.long(),
        Current: dataType.skip(2).long(),
        Target: dataType.skip(2).long(),
        Points: dataType.skip(2).long()
      },
      {
        Type: dataType.back(9).long(),
        Current: dataType.skip(2).long(),
        Target: dataType.skip(2).long(),
        Points: dataType.skip(2).long()
      },
      {
        Type: dataType.back(9).long(),
        Current: dataType.skip(2).long(),
        Target: dataType.skip(2).long(),
        Points: dataType.skip(2).long()
      }
    ],
    TotalPoints: dataType.long(),
  }

  dataType.skip(12);
  const secretWoodLimit = dataType.long();
  dataType.skip(1);
  const secretStoneLimit = dataType.long();
  const fortress = {
    Fortress: fort,
    LaborerQuarters: labQuarte,
    WoodcutterGuild: woodcutterGuild,
    Quarry: quarry,
    GemMine: gemMine,
    Academy: academy,
    ArcheryGuild: archeryGuild,
    Barracks: barracks,
    MageTower: mageTower,
    Treasury: treasury,
    Smithy: smithy,
    Fortifications: fortifications,
    Knights: knights,
    Upgrade: fortUpgrade,
    Upgrades: fortUpgrades,
    Honor: fortHonor,
    WoodcutterMax: woodcutterMax,
    QuarryMax: quarryMax,
    AcademyMax: academyMax,
    MaxWood: maxWood,
    MaxStone: maxStone,
    SecretWoodLimit: secretWoodLimit,
    SecretStoneLimit: secretStoneLimit
  }


  return {
    ID: id,
    Registered: registered,
    Level: level,
    XP: xp,
    XPNext: xpNext,
    Honor: honor,
    Rank: rank,
    DevilPercent: devilPercent,
    Class: classType,
    Action: action,
    Mount: mount,
    MountValue: mountValue,
    MountExpire: mountExpire,
    GuildRaid: guildRaid,
    GuildPortal: guildPortal,
    Book: book,
    ClientVersion: clientVersion,
    Group: {
      ID: groupId,
      Joined: groupJoined,
      Treasure: groupTreasure,
      Instructor: groupInstructor,
      Pet: groupPet
    },
    Fortress: fortress,
    Summer: summer,
    AdventureSkips: adventureSkips,
    CalendarDay: calendarDay,
    CalendarType: calendarType,
    Toilet: toilet,
    UsedBeers: usedBeer,
    MaxBeers: maxBeers,
    UsedAdventureTime: usedAdventureTime,
    ThirstLeft: thirstLeft,
    ThirstReroll: thirstReroll,
    LegendaryDungeonTries: legendaryDungeonTries,
    Armor: armor,
    Damage: damage,
    Potions: potions,
    HasEternalPotion: potionsLife,
    Strength: attributes.Strength,
    Dexterity: attributes.Dexterity,
    Intelligence: attributes.Intelligence,
    Constitution: attributes.Constitution,
    Luck: attributes.Luck,
    WheelSpinsToday: dataType.atLong(579),
    DiceGamesRemaining: dataType.atLong(651),
    ArenaFightsToday: arenaFights,
  }
}

export function getAttributes(data: ByteParser, skipPurchased = true) {
  const Strength = {
    Type: 1,
    Base: data.long(),
    Bonus: data.skip(4).long(),
    Purchased: 0,
    Items: 0,
    Gems: 0,
    Upgrades: 0,
    Potion: 0,
    Class: 0,
    Pet: 0,
    Total: 0,
  };

  Strength.Total = Strength.Base + Strength.Bonus;

  const Dexterity = {
    Type: 2,
    Base: data.back(5).long(),
    Bonus: data.skip(4).long(),
    Purchased: 0,
    Items: 0,
    Gems: 0,
    Upgrades: 0,
    Potion: 0,
    Class: 0,
    Pet: 0,
    Total: 0,
  };

  Dexterity.Total = Dexterity.Base + Dexterity.Bonus;

  const Intelligence = {
    Type: 3,
    Base: data.back(5).long(),
    Bonus: data.skip(4).long(),
    Purchased: 0,
    Items: 0,
    Gems: 0,
    Upgrades: 0,
    Potion: 0,
    Class: 0,
    Pet: 0,
    Total: 0,
  };

  Intelligence.Total = Intelligence.Base + Intelligence.Bonus;

  const Constitution = {
    Type: 4,
    Base: data.back(5).long(),
    Bonus: data.skip(4).long(),
    Purchased: 0,
    Items: 0,
    Gems: 0,
    Upgrades: 0,
    Potion: 0,
    Class: 0,
    Pet: 0,
    Total: 0,
  };

  Constitution.Total = Constitution.Base + Constitution.Bonus;

  const Luck = {
    Type: 5,
    Base: data.back(5).long(),
    Bonus: data.skip(4).long(),
    Purchased: 0,
    Items: 0,
    Gems: 0,
    Upgrades: 0,
    Potion: 0,
    Class: 0,
    Pet: 0,
    Total: 0,
  };

  if (skipPurchased) {
    data.skip(5);
  } else {
    Strength.Purchased = data.long();
    Dexterity.Purchased = data.long();
    Intelligence.Purchased = data.long();
    Constitution.Purchased = data.long();
    Luck.Purchased = data.long();
  }

  Luck.Total = Luck.Base + Luck.Bonus;

  return {
    Strength: Strength,
    Dexterity: Dexterity,
    Intelligence: Intelligence,
    Constitution: Constitution,
    Luck: Luck
  }
}

const MOUNT_MAP = [
  0, 10, 20, 30, 50, 50
];

function getPotionType(type: number) {
  return type == 16 ? 6 : (type == 0 ? 0 : 1 + (type - 1) % 5);
}
