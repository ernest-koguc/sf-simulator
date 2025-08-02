import { ASSASSIN, BARD, BATTLEMAGE, BERSERKER, CONFIG, DEMONHUNTER, DRUID, MAGE, NECROMANCER, PALADIN, PLAGUEDOCTOR, SCOUT, WARRIOR } from "./ClassConfig";
import { ByteParser } from "./parsers/ByteParser";
import { Attribute, Equipment, Potion } from "./SFGameModels";

export type DataModel = {
  ownsave: number[];
  units: number[] | null;
  pets: number[] | null;
  chest: number[] | null;
  dummy: number[] | null;
  witch: number[] | null;
  idle: number[] | null;
  calendar: number[] | null;
  dailyTasks: number[] | null;
  dailyTasksRewards: number[] | null;
  eventTasks: number[] | null;
  eventTasksRewards: number[] | null;
}

export class PlayerModel {
  public Toilet!: { Aura: number, Mana: number, Capacity: number };
  public Witch!: {
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
  };
  public ID!: number;
  public Registered!: number;
  public Level!: number;
  public XP!: number;
  public XPNext!: number;
  public Honor!: number;
  public Rank!: number;
  public DevilPercent!: number;
  public Class!: number;
  public Action!: { Status: number, Index: number, Finish: number };
  public Strength!: Attribute;
  public Dexterity!: Attribute;
  public Intelligence!: Attribute;
  public Constitution!: Attribute;
  public Luck!: Attribute;
  public Items!: Equipment;
  public Inventory!: {
    Backpack: ItemModel[],
    Chest: ItemModel[],
    Shop: ItemModel[],
    Dummy: Equipment | null,
  };
  public Mount!: number;
  public MountValue!: number;
  public Group!: { ID: number, Name: string | null, Joined?: number, Identifier?: string, Treasure?: number, Instructor?: number, Pet?: number };
  public Book!: number;
  public Armor!: number;
  public Damage!: { Min: number, Max: number };
  public Damage2?: { Min: number, Max: number };
  public MountExpire!: number;
  public ThirstReroll!: number;
  public ThirstLeft!: number;
  public UsedBeers!: number;
  public Potions!: Potion[];
  public PotionsLife!: number;
  public Fortress!: {
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
  public CalendarDay!: number;
  public CalendarType!: number;
  public LegendaryDungeonTries!: number;
  public UsedAdventureTime!: number;
  public ClientVersion!: number;
  public AdventureSkips!: number;
  public Summer!: {
    Missions: { Type: number, Current: number, Target: number, Points: number }[],
    TotalPoints: number
  };
  public Pets!: {
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
  };
  public Name!: string;
  public GuildRaid!: number;
  public GuildPortal!: number;
  public DailyTasks!: {
    Rewards: {
      Collected: boolean,
      Points: number,
      ResourceType: number,
      ResourceAmount: number
    }[]
  };
  public EventTasks!: {
    Rewards: {
      Collected: boolean,
      Points: number,
      ResourceType: number,
      ResourceAmount: number
    }[]
  };
  public ItemsArray!: ItemModel[];
  public ClassBonus!: boolean;
  public Runes!: {
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
  };
  public Config?: any;
  public BlockChance!: number;
  public Primary!: Attribute;

  constructor(data: DataModel | null, groupName: string | null) {
    if (!data) {
      return;
    }

    let dataType = new ByteParser(data.ownsave);
    dataType.assert(650);

    dataType.skip(1); // skip
    this.ID = dataType.long();
    // TODO: check if it is valid without offset
    dataType.skip(1); // LastActive
    this.Registered = dataType.long() * 1000;// + data.offset;
    dataType.skip(3); // skip
    this.Level = dataType.short();
    dataType.clear();
    this.XP = dataType.long();
    this.XPNext = dataType.long();
    this.Honor = dataType.long();
    this.Rank = dataType.long();
    dataType.short();
    this.DevilPercent = dataType.short();
    dataType.skip(14); // skip
    //this.Face = {
    //  Mouth: dataType.long(),
    //  Hair: {
    //    Type: dataType.long() % 100,
    //    Color: Math.trunc(dataType.back(1).long() / 100)
    //  },
    //  Brows: {
    //    Type: dataType.long() % 100,
    //    Color: Math.trunc(dataType.back(1).long() / 100)
    //  },
    //  Eyes: dataType.long(),
    //  Beard: {
    //    Type: dataType.long() % 100,
    //    Color: Math.trunc(dataType.back(1).long() / 100)
    //  },
    //  Nose: dataType.long(),
    //  Ears: dataType.long(),
    //  Special: dataType.long(),
    //  Special2: dataType.long(),
    //  Portrait: dataType.long()
    //};
    dataType.short();
    //this.Race = dataType.short();
    dataType.clear(); // skip
    dataType.short();
    // skip mirror
    //this.Mirror = dataType.byte();
    // skip mirror pieces
    //this.MirrorPieces = PlayerModel.getMirrorPieces(this.ServerId = dataType.short());
    dataType.short();
    this.Class = dataType.short();
    dataType.clear(); // skip
    PlayerModel.loadAttributes(this, dataType, false);
    const status = dataType.short()
    dataType.short()
    const index = dataType.short()
    dataType.short();
    // TODO: check if it is valid without offset
    const finish = dataType.long() * 1000;// + data.offset;
    this.Action = {
      Status: status,
      Index: index,
      Finish: finish
    };
    this.Items = PlayerModel.loadEquipment(dataType, 1, this.Class);
    this.Inventory = {
      Backpack: [],
      Chest: [],
      Shop: [],
      Dummy: null,
    };
    for (let i = 0; i < 5; i++) {
      const item = new ItemModel(dataType.sub(12), 6, i + 1);
      if (item.Type > 0) {
        this.Inventory.Backpack.push(item);
      }
    }
    dataType.skip(58); // skip
    this.Mount = dataType.short();
    this.MountValue = PlayerModel.getMount(this.Mount);

    //skip
    dataType.short();//legacyDungeons.Tower = dataType.short();

    dataType.skip(1);
    for (let i = 0; i < 6; i++) {
      const item = new ItemModel(dataType.sub(12), 7, i + 1);
      if (item.Type > 0) {
        this.Inventory.Shop.push(item);
      }
    }
    dataType.skip(1);
    for (let i = 0; i < 6; i++) {
      const item = new ItemModel(dataType.sub(12), 8, i + 1);
      if (item.Type > 0) {
        this.Inventory.Shop.push(item);
      }
    }

    this.GuildRaid = dataType.short();

    dataType.short();
    dataType.skip(1); // skip
    this.Group = {
      ID: dataType.long(),
      Name: groupName
    };
    dataType.skip(2); // skip

    this.Book = Math.max(0, dataType.long() - 10000);

    dataType.skip(4); // skip

    this.Group.Joined = dataType.long() * 1000;// + data.offset;
    dataType.skip(1); // skip
    dataType.short(); // skip

    this.GuildPortal = dataType.byte();
    // skip
    dataType.byte()//this.SoloPortal = dataType.byte();

    dataType.skip(1); // skip
    this.Armor = dataType.long();
    this.Damage = {
      Min: dataType.long(),
      Max: dataType.long()
    };
    dataType.skip(1); // skip
    this.MountExpire = dataType.long() * 1000;//data.offset,
    dataType.skip(3);
    this.ThirstReroll = dataType.long() * 1000;// + data.offset;
    this.ThirstLeft = dataType.long();
    this.UsedBeers = dataType.long();
    dataType.skip(33); // skip

    let aura = dataType.long();
    let mana = dataType.long();
    this.Potions = [{
      Type: PlayerModel.getPotionType(dataType.long()),
      Expire: dataType.skip(2).long() * 1000,// + data.offset,
      Size: dataType.skip(2).long()
    }, {
      Type: PlayerModel.getPotionType(dataType.back(6).long()),
      Expire: dataType.skip(2).long() * 1000,// + data.offset,
      Size: dataType.skip(2).long()
    }, {
      Type: PlayerModel.getPotionType(dataType.back(6).long()),
      Expire: dataType.skip(2).long() * 1000,// + data.offset,
      Size: dataType.skip(2).long()
    }];
    this.PotionsLife = dataType.long();
    dataType.skip(12); // skip
    this.Toilet = {
      Aura: aura,
      Mana: mana,
      Capacity: dataType.long()
    };
    dataType.skip(8); // skip
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

    dataType.skip(29); // skip

    const woodcutterMax = dataType.long();
    const quarryMax = dataType.long();
    const academyMax = dataType.long();
    const maxWood = dataType.long();
    const maxStone = dataType.long();
    dataType.skip(1); // skip
    const fortUpgrade = {
      Building: dataType.long() - 1,
      Finish: dataType.long() * 1000,// + data.offset,
      Start: dataType.long() * 1000// + data.offset
    }

    dataType.skip(7);

    const fortUpgrades = dataType.long();
    const fortHonor = dataType.long();
    dataType.skip(1); // fort rank
    dataType.skip(9); // skip
    //if (dataType.long() * 1000/* + data.offset*/ < data.timestamp) {
    //  raidWood += Math.trunc(this.Fortress.Wood / 10);
    //  raidStone += Math.trunc(this.Fortress.Stone / 10);
    //}
    dataType.skip(5); // skip
    const knights = dataType.long();
    dataType.skip(5); // skip

    //Todo improve so we can just skip through easier
    dataType.byteArray(14);
    //legacyDungeons.Shadow = dataType.byteArray(14);

    dataType.clear(); // skip
    dataType.skip(15); // skip
    this.Group.Treasure = dataType.long();
    this.Group.Instructor = dataType.long();
    dataType.skip(4); // skip
    this.Group.Pet = dataType.long();
    dataType.skip(18);

    // skip
    dataType.short();

    this.CalendarDay = dataType.short();
    dataType.skip(5);

    dataType.skip(3);

    // Normalize calendar type in order to align it with S&F Tavern's calendar indexing
    this.CalendarType = 1 + (dataType.long() + 10) % 12;
    const timeMachineMushrooms = dataType.long();
    dataType.skip(3);
    this.LegendaryDungeonTries = dataType.long();
    dataType.skip(2);
    this.UsedAdventureTime = dataType.long();
    dataType.skip(5);
    this.ClientVersion = dataType.long();
    this.AdventureSkips = dataType.long();
    this.Summer = {
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
      TotalPoints: dataType.long()
    }

    dataType.skip(12);
    const secretWoodLimit = dataType.long();
    dataType.skip(1);
    const secretStoneLimit = dataType.long();
    this.Fortress = {
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

    dataType.skip(1);

    dataType = new ByteParser(data.pets);
    dataType.skip(2);

    const petLevels = dataType.sub(100);

    let shadowCount = 0;
    let lightCount = 0;
    let earthCount = 0;
    let fireCount = 0;
    let waterCount = 0;
    let shadowLevel = 0;
    let lightLevel = 0;
    let earthLevel = 0;
    let fireLevel = 0;
    let waterLevel = 0;

    if (petLevels.length) {
      for (let i = 0; i < 20; i++) {
        shadowCount += petLevels[i] > 0 ? 1 : 0;
        shadowLevel += petLevels[i];
      }

      for (let i = 0; i < 20; i++) {
        lightCount += petLevels[i + 20] > 0 ? 1 : 0;
        lightLevel += petLevels[i + 20];
      }

      for (let i = 0; i < 20; i++) {
        earthCount += petLevels[i + 40] > 0 ? 1 : 0;
        earthLevel += petLevels[i + 40];
      }

      for (let i = 0; i < 20; i++) {
        fireCount += petLevels[i + 60] > 0 ? 1 : 0;
        fireLevel += petLevels[i + 60];
      }

      for (let i = 0; i < 20; i++) {
        waterCount += petLevels[i + 80] > 0 ? 1 : 0;
        waterLevel += petLevels[i + 80];
      }
    }

    dataType.skip(1);
    const totalCount = dataType.long();
    const shadow = dataType.long();
    const light = dataType.long();
    const earth = dataType.long();
    const fire = dataType.long();
    const water = dataType.long();


    dataType.skip(101);
    const petDungeons = dataType.sub(5);
    dataType.skip(18);
    const petRank = dataType.long();
    const petHonor = dataType.long();
    dataType.skip(29);
    const petTotalLevel = shadowLevel + lightLevel + fireLevel + earthLevel + waterLevel;
    this.Pets = {
      Levels: petLevels,
      ShadowLevels: petLevels.slice(0, 20),
      LightLevels: petLevels.slice(20, 40),
      EarthLevels: petLevels.slice(40, 60),
      FireLevels: petLevels.slice(60, 80),
      WaterLevels: petLevels.slice(80, 100),
      ShadowCount: shadowCount,
      LightCount: lightCount,
      EarthCount: earthCount,
      FireCount: fireCount,
      WaterCount: waterCount,
      ShadowLevel: shadowLevel,
      LightLevel: lightLevel,
      EarthLevel: earthLevel,
      FireLevel: fireLevel,
      WaterLevel: waterLevel,
      TotalCount: totalCount,
      Shadow: shadow,
      Light: light,
      Earth: earth,
      Fire: fire,
      Water: water,
      Rank: petRank,
      Honor: petHonor,
      TotalLevel: petTotalLevel,
      Dungeons: petDungeons
    };

    this.evaluateCommon();

    if (data.chest) {
      dataType = new ByteParser(data.chest);
      for (let i = 0; i < 45 && dataType.atLeast(12); i++) {
        const item = new ItemModel(dataType.sub(12), 6, i + 6);
        if (item.Type > 0) {
          if (i >= 15) {
            this.Inventory.Chest.push(item);
          } else {
            this.Inventory.Backpack.push(item);
          }
        }
      }
    }

    if (data.dummy) {
      this.Inventory.Dummy = PlayerModel.loadEquipment(new ByteParser(data.dummy), 5, this.Class);
    }

    dataType = new ByteParser(data.witch);
    let witchStage = dataType.long();

    let witchItems = dataType.long();
    const witchItemsNext = Math.max(0, dataType.long());
    const witchItem = dataType.long();
    witchItems = Math.min(witchItems, witchItemsNext);

    dataType.skip(2);

    const witchFinish = dataType.long() * 1000; //+ data.offset;
    //if (this.Witch.Finish < this.Timestamp) {
    //  this.Witch.Finish = 0;
    //}

    dataType.skip(1);

    const witchScrolls = [];
    for (let i = 0; i < 9; i++) {
      // TODO: check if this is correct
      dataType.skip();

      const picIndex = dataType.long();
      const date = dataType.long() * 1000;// + data.offset;
      const type = picIndex % 1000;

      witchScrolls[PlayerModel.getScroll(type)] = {
        Date: date,
        Type: type,
        Owned: _between(date, 0, (new Date()).getTime())
      };
    }

    witchStage = _lenWhere(witchScrolls, (scroll: any) => scroll.Owned);
    this.Witch = {
      Stage: witchStage,
      Items: witchItems,
      ItemsNext: witchItemsNext,
      Item: witchItem,
      Finish: witchFinish,
      Scrolls: witchScrolls
    }

    if (data.dailyTasksRewards && data.dailyTasksRewards.length > 0) {
      const rewards = []

      for (const [collected, required, , resourceType, resourceAmount] of _eachBlock(data.dailyTasksRewards, 5)) {
        rewards.push({
          Collected: !!collected,
          Points: required,
          ResourceType: resourceType,
          ResourceAmount: resourceAmount
        })
      }

      this.DailyTasks = {
        Rewards: rewards
      }
    }

    if (data.eventTasksRewards && data.eventTasksRewards.length > 0) {
      const rewards = []

      for (const [collected, required, , resourceType, resourceAmount] of _eachBlock(data.eventTasksRewards, 5)) {
        rewards.push({
          Collected: !!collected,
          Points: required,
          ResourceType: resourceType,
          ResourceAmount: resourceAmount
        })
      }

      this.EventTasks = {
        Rewards: rewards
      }
    }
  }

  getPrimaryAttribute() {
    return (this as any)[(this.Config as any).Attribute];
  }

  evaluateCommon() {
    this.Config = (CONFIG as any).fromID(this.Class);
    this.ItemsArray = Object.values(this.Items).filter(i => i !== null) as ItemModel[];
    this.Primary = this.getPrimaryAttribute();

    this.ClassBonus = this.Class == BATTLEMAGE || this.Class == BERSERKER;

    this.addCalculatedAttributes(this.Strength, this.Pets.Water);
    this.addCalculatedAttributes(this.Dexterity, this.Pets.Light);
    this.addCalculatedAttributes(this.Intelligence, this.Pets.Earth);
    this.addCalculatedAttributes(this.Constitution, this.Pets.Shadow);
    this.addCalculatedAttributes(this.Luck, this.Pets.Fire);

    this.Runes = {
      Gold: 0,
      Chance: 0,
      Quality: 0,
      XP: 0,
      Health: 0,
      ResistanceFire: 0,
      ResistanceCold: 0,
      ResistanceLightning: 0,
      Damage: 0,
      DamageFire: 0,
      DamageCold: 0,
      DamageLightning: 0,
      Damage2: 0,
      Damage2Fire: 0,
      Damage2Cold: 0,
      Damage2Lightning: 0,
      Resistance: 0,
      Runes: 0,
      Achievements: 0
    };

    let partCold = 0;
    let partFire = 0;
    let partLightning = 0;

    for (const item of this.ItemsArray) {
      if (item.HasRune && item.Type != 1) {
        const rune = item.AttributeTypes[2];
        const value = item.Attributes[2];

        if (rune == 31) {
          this.Runes.Gold += value;
          if (RUNE_VALUE.GOLD(value) > this.Runes.Runes) {
            this.Runes.Runes = RUNE_VALUE.GOLD(value);
          }
        } else if (rune == 32) {
          this.Runes.Chance += value;
          if (RUNE_VALUE.EPIC_FIND(value) > this.Runes.Runes) {
            this.Runes.Runes = RUNE_VALUE.EPIC_FIND(value);
          }
        } else if (rune == 33) {
          this.Runes.Quality += value;
          if (RUNE_VALUE.ITEM_QUALITY(value) > this.Runes.Runes) {
            this.Runes.Runes = RUNE_VALUE.ITEM_QUALITY(value);
          }
        } else if (rune == 34) {
          this.Runes.XP += value;
          if (RUNE_VALUE.XP(value) > this.Runes.Runes) {
            this.Runes.Runes = RUNE_VALUE.XP(value);
          }
        } else if (rune == 35) {
          this.Runes.Health += value;
          if (RUNE_VALUE.HEALTH(value) > this.Runes.Runes) {
            this.Runes.Runes = RUNE_VALUE.HEALTH(value);
          }
        } else if (rune == 36) {
          this.Runes.ResistanceFire += value;
          partFire += value;
          if (RUNE_VALUE.SINGLE_RESISTANCE(value) > this.Runes.Runes) {
            this.Runes.Runes = RUNE_VALUE.SINGLE_RESISTANCE(value);
          }
        } else if (rune == 37) {
          this.Runes.ResistanceCold += value;
          partCold += value;
          if (RUNE_VALUE.SINGLE_RESISTANCE(value) > this.Runes.Runes) {
            this.Runes.Runes = RUNE_VALUE.SINGLE_RESISTANCE(value);
          }
        } else if (rune == 38) {
          this.Runes.ResistanceLightning += value;
          partLightning += value;
          if (RUNE_VALUE.SINGLE_RESISTANCE(value) > this.Runes.Runes) {
            this.Runes.Runes = RUNE_VALUE.SINGLE_RESISTANCE(value);
          }
        } else if (rune == 39) {
          this.Runes.ResistanceFire += value;
          this.Runes.ResistanceCold += value;
          this.Runes.ResistanceLightning += value;
          this.Runes.Resistance += value;
          if (RUNE_VALUE.TOTAL_RESISTANCE(value) > this.Runes.Runes) {
            this.Runes.Runes = RUNE_VALUE.TOTAL_RESISTANCE(value);
          }
        }
      }
    }

    if (this.Items.Wpn1.AttributeTypes[2] >= 40) {
      const rune = this.Items.Wpn1.AttributeTypes[2];
      const value = this.Items.Wpn1.Attributes[2];

      this.Runes.Damage += value;
      if (RUNE_VALUE.ELEMENTAL_DAMAGE(value) > this.Runes.Runes) {
        this.Runes.Runes = RUNE_VALUE.ELEMENTAL_DAMAGE(value);
      }

      if (rune == 40) {
        this.Runes.DamageFire = value;
      } else if (rune == 41) {
        this.Runes.DamageCold = value;
      } else if (rune == 42) {
        this.Runes.DamageLightning = value;
      }
    }

    if (this.Class == ASSASSIN && this.Items.Wpn2!.AttributeTypes[2] >= 40) {
      const rune = this.Items.Wpn2!.AttributeTypes[2];
      const value = this.Items.Wpn2!.Attributes[2];

      this.Runes.Damage2 += value;
      if (RUNE_VALUE.ELEMENTAL_DAMAGE(value) > this.Runes.Runes) {
        this.Runes.Runes = RUNE_VALUE.ELEMENTAL_DAMAGE(value);
      }

      if (rune == 40) {
        this.Runes.Damage2Fire = value;
      } else if (rune == 41) {
        this.Runes.Damage2Cold = value;
      } else if (rune == 42) {
        this.Runes.Damage2Lightning = value;
      }
    }

    if (this.Class === ASSASSIN) {
      this.Damage2 = {
        Min: this.Items.Wpn2!.DamageMin,
        Max: this.Items.Wpn2!.DamageMax,
      };
    }

    this.Runes.Gold = Math.min(50, this.Runes.Gold);
    this.Runes.Chance = Math.min(50, this.Runes.Chance);
    this.Runes.Quality = Math.min(5, this.Runes.Quality);
    this.Runes.XP = Math.min(10, this.Runes.XP);
    this.Runes.Health = Math.min(15, this.Runes.Health);

    this.Runes.Resistance += Math.min(25, Math.trunc(partFire / 3));
    this.Runes.Resistance += Math.min(25, Math.trunc(partCold / 3));
    this.Runes.Resistance += Math.min(25, Math.trunc(partLightning / 3));
    this.Runes.Resistance = Math.min(75, this.Runes.Resistance);

    this.Runes.ResistanceFire = Math.min(75, this.Runes.ResistanceFire);
    this.Runes.ResistanceCold = Math.min(75, this.Runes.ResistanceCold);
    this.Runes.ResistanceLightning = Math.min(75, this.Runes.ResistanceLightning);

    this.Runes.Damage = Math.min(60, this.Runes.Damage);
    this.Runes.DamageFire = Math.min(60, this.Runes.DamageFire);
    this.Runes.DamageCold = Math.min(60, this.Runes.DamageCold);
    this.Runes.DamageLightning = Math.min(60, this.Runes.DamageLightning);

    this.Runes.Damage2 = Math.min(60, this.Runes.Damage2);
    this.Runes.Damage2Fire = Math.min(60, this.Runes.Damage2Fire);
    this.Runes.Damage2Cold = Math.min(60, this.Runes.Damage2Cold);
    this.Runes.Damage2Lightning = Math.min(60, this.Runes.Damage2Lightning);

    if (this.Action.Status < 0) {
      this.Action.Status += 256;
    }

    if (this.Action.Status == 0) {
      this.Action.Index = 0;
      this.Action.Finish = 0;
    }

    if (this.Class === WARRIOR) {
      this.BlockChance = this.Items.Wpn2!.DamageMin;
    }
  }

  static getHealth(player: PlayerModel, soloPortal: number, hpMultiplier?: number) {
    const config = player.Config || (CONFIG as any).fromID(player.Class);

    let ma = hpMultiplier || config.HealthMultiplier;
    let mb = (100 + soloPortal) / 100;
    let mc = player.PotionsLife ? 1.25 : 1;
    let md = (100 + player.Runes.Health) / 100;

    return Math.trunc(Math.floor(Math.floor(player.Constitution.Total * ma * (player.Level + 1) * mb) * mc) * md);
  }

  getEquipmentItemBonus(attribute: Attribute) {
    let bonus = 0;
    for (const item of this.ItemsArray) {
      for (let i = 0; i < 3; i++) {
        if (item.AttributeTypes[i] == attribute.Type || item.AttributeTypes[i] == 6 || item.AttributeTypes[i] == attribute.Type + 20 || (attribute.Type > 3 && item.AttributeTypes[i] >= 21 && item.AttributeTypes[i] <= 23)) {
          bonus += item.Attributes[i];
        }
      }
    }

    return bonus;
  }

  getEquipmentUpgradeBonus(attribute: Attribute) {
    let bonus = 0;
    for (const item of this.ItemsArray) {
      if (item.Upgrades > 0) {
        for (let i = 0; i < 3; i++) {
          if (item.AttributeTypes[i] == attribute.Type || item.AttributeTypes[i] == 6 || item.AttributeTypes[i] == attribute.Type + 20 || (attribute.Type > 3 && item.AttributeTypes[i] >= 21 && item.AttributeTypes[i] <= 23)) {
            bonus += item.Attributes[i] - Math.floor(item.Attributes[i] / item.UpgradeMultiplier);
          }
        }
      }
    }

    return bonus;
  }

  getEquipmentGemBonus(attribute: Attribute) {
    let bonus = 0;
    for (const item of this.ItemsArray) {
      if (item.HasGem && (item.GemType == attribute.Type || item.GemType == 6 || (item.GemType == 7 && (attribute.Type == this.Primary.Type || attribute.Type == 4)))) {
        bonus += item.GemValue * (item.Type == 1 && this.Class != 4 ? 2 : 1);
      }
    }

    return bonus;
  }

  getClassBonus(attribute: Attribute) {
    if (this.Class == BATTLEMAGE || this.Class == BERSERKER) {
      return Math.ceil((this.Class == BATTLEMAGE ? attribute.Items + attribute.Gems : attribute.Items) * 11 / 100);
    } else {
      return 0;
    }
  }

  getPotionBonus(attribute: Attribute) {
    for (const potion of this.Potions) {
      if (potion.Type == attribute.Type) {
        return Math.ceil((attribute.Base + attribute.Class + attribute.Items + attribute.Gems) * potion.Size / 100);
      }
    }

    return 0;
  }

  getPetBonus(attribute: Attribute, pet: number) {
    return Math.ceil((attribute.Base + attribute.Items + attribute.Gems + attribute.Class + attribute.Potion) * pet / 100);
  }

  addCalculatedAttributes(attribute: Attribute, pet: number) {
    attribute.Items = this.getEquipmentItemBonus(attribute);
    attribute.Gems = this.getEquipmentGemBonus(attribute);
    attribute.Upgrades = this.getEquipmentUpgradeBonus(attribute);
    attribute.Class = this.getClassBonus(attribute);
    attribute.Potion = this.getPotionBonus(attribute);
    attribute.Pet = this.getPetBonus(attribute, pet);

    if (!attribute.Bonus) {
      attribute.Total = attribute.Base + attribute.Pet + attribute.Potion + attribute.Class + attribute.Items + attribute.Gems;
    } else {
      attribute.Total = attribute.Base + attribute.Bonus;
    }
  }

  static loadAttributes(player: PlayerModel, dataType: ByteParser, skipPurchased = true) {
    player.Strength = {
      Type: 1,
      Base: dataType.long(),
      Bonus: dataType.skip(4).long(),
      Purchased: 0,
      Items: 0,
      Gems: 0,
      Upgrades: 0,
      Potion: 0,
      Class: 0,
      Pet: 0,
      Total: 0,
    };

    player.Dexterity = {
      Type: 2,
      Base: dataType.back(5).long(),
      Bonus: dataType.skip(4).long(),
      Purchased: 0,
      Items: 0,
      Gems: 0,
      Upgrades: 0,
      Potion: 0,
      Class: 0,
      Pet: 0,
      Total: 0,
    };

    player.Intelligence = {
      Type: 3,
      Base: dataType.back(5).long(),
      Bonus: dataType.skip(4).long(),
      Purchased: 0,
      Items: 0,
      Gems: 0,
      Upgrades: 0,
      Potion: 0,
      Class: 0,
      Pet: 0,
      Total: 0,
    };

    player.Constitution = {
      Type: 4,
      Base: dataType.back(5).long(),
      Bonus: dataType.skip(4).long(),
      Purchased: 0,
      Items: 0,
      Gems: 0,
      Upgrades: 0,
      Potion: 0,
      Class: 0,
      Pet: 0,
      Total: 0,
    };

    player.Luck = {
      Type: 5,
      Base: dataType.back(5).long(),
      Bonus: dataType.skip(4).long(),
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
      dataType.skip(5);
    } else {
      player.Strength.Purchased = dataType.long();
      player.Dexterity.Purchased = dataType.long();
      player.Intelligence.Purchased = dataType.long();
      player.Constitution.Purchased = dataType.long();
      player.Luck.Purchased = dataType.long();
    }
  }

  static loadEquipment(dataType: ByteParser, inventoryType: number, characterClass: number) {
    const items = {
      Head: new ItemModel(dataType.sub(12), inventoryType, 1),
      Body: new ItemModel(dataType.sub(12), inventoryType, 2),
      Hand: new ItemModel(dataType.sub(12), inventoryType, 3),
      Feet: new ItemModel(dataType.sub(12), inventoryType, 4),
      Neck: new ItemModel(dataType.sub(12), inventoryType, 5),
      Belt: new ItemModel(dataType.sub(12), inventoryType, 6),
      Ring: new ItemModel(dataType.sub(12), inventoryType, 7),
      Misc: new ItemModel(dataType.sub(12), inventoryType, 8),
      Wpn1: new ItemModel(dataType.sub(12), inventoryType, 9),
      Wpn2: null as (ItemModel | null)
    };

    if (characterClass === WARRIOR || characterClass === ASSASSIN) {
      items.Wpn2 = new ItemModel(dataType.sub(12), inventoryType, 10)
    } else {
      dataType.sub(12);
    }

    return items;
  }

  static getMount(value: number) {
    return this.MOUNT_MAP[value];
  }

  static MOUNT_MAP = [
    0, 10, 20, 30, 50, 50
  ];

  static getFlags(value: number) {
    let background = 0;
    for (let i = 2; i >= 0; i--) {
      if ((value & (1 << (6 + i))) != 0) {
        background = i + 1;
      }
    }

    return {
      GroupTournamentBackground: background,
      GoldFrame: (value & (1 << 5)) != 0,
      GoldFrameDisabled: false,
      OfficialCreator: (value & (1 << 9)) != 0,
      OfficialDiscord: (value & (1 << 10)) != 0,
      TwitchFrame: (value & (1 << 11)) != 0,
      FriendlyFireFrame: (value & (1 << 12)) != 0,
      InvitesDisabled: false
    }
  }

  static getPotionType(type: number) {
    return type == 16 ? 6 : (type == 0 ? 0 : 1 + (type - 1) % 5);
  }

  static getScroll(value: number) {
    return this.SCROLL_MAP[value];
  }

  static SCROLL_MAP: { [key: number]: number } = {
    11: 0,
    31: 1,
    41: 2,
    51: 3,
    61: 4,
    71: 5,
    81: 6,
    91: 7,
    101: 8
  };
}

export class ItemModel {
  public Data!: number[];
  public SlotType!: number;
  public SlotIndex!: number;
  public GemType!: number;
  public HasSocket!: boolean;
  public GemValue!: number;
  public HasGem!: boolean;
  public HasRune!: boolean;
  public Class!: number;
  public PicIndex!: number;
  public Index!: number;
  public IsEpic!: boolean;
  public Type!: number;
  public IsFlushed!: boolean;
  public HasValue!: boolean;
  public Enchantment!: number;
  public Armor!: number;
  public DamageMin!: number;
  public DamageMax!: number;
  public Upgrades!: number;
  public UpgradeMultiplier!: number;
  public AttributeTypes!: number[];
  public Attributes!: number[];
  public HasEnchantment!: boolean;
  public Color!: number;
  public ColorClass!: number;
  public RuneType!: number;
  public RuneValue!: number;
  public SellPrice!: {
    Gold: number,
    Mushrooms: number
  }
  private _Strength: number = 0;
  private _StrengthSet: boolean = false;
  private _Dexterity: number = 0;
  private _DexteritySet: boolean = false;
  private _Intelligence: number = 0;
  private _IntelligenceSet: boolean = false;
  private _Constitution: number = 0;
  private _ConstitutionSet: boolean = false;
  private _Luck: number = 0;
  private _LuckSet: boolean = false;
  constructor(data: number[], slotType: number, slotIndex: number) {
    let dataType = new ByteParser(data);
    dataType.assert(12);

    let type = dataType.short();
    let socket = dataType.byte();
    let enchantmentType = dataType.byte();
    let picIndex = dataType.short();
    dataType.short();
    let damageMin = dataType.long();
    let damageMax = dataType.long();
    let attributeType = [dataType.long(), dataType.long(), dataType.long()];
    let attributeValue = [dataType.long(), dataType.long(), dataType.long()];
    let gold = dataType.long();
    let coins = dataType.byte();
    let upgradeLevel = dataType.byte();
    let socketPower = dataType.short();

    if (attributeType[1] === 4 && attributeType[2] === 5) {
      // Legacy attribute
      attributeType = [20 + attributeType[0], 0, 0];
      attributeValue = [attributeValue[0], 0, 0];
    }

    this.Data = data;
    this.SlotType = slotType;
    this.SlotIndex = slotIndex;
    this.GemType = socket >= 10 ? (1 + (socket % 10)) : 0;
    this.HasSocket = socket > 0;
    this.GemValue = socketPower;
    this.HasGem = socket > 1;
    this.HasRune = attributeType[2] > 30;
    this.Class = Math.trunc(picIndex / 1000) + 1;
    this.PicIndex = picIndex;
    this.Index = picIndex % 1000;
    this.IsEpic = type < 11 && this.Index >= 50;
    this.Type = type;
    this.IsFlushed = coins == 0 && gold == 0 && type > 0 && type < 11;
    this.HasValue = coins > 0 || gold > 0;
    this.Enchantment = enchantmentType;
    this.Armor = damageMin;
    this.DamageMin = damageMin;
    this.DamageMax = damageMax;
    this.Upgrades = upgradeLevel;
    this.UpgradeMultiplier = Math.pow(1.03, upgradeLevel);
    this.AttributeTypes = attributeType;
    this.Attributes = attributeValue;
    this.HasEnchantment = enchantmentType > 0;
    this.Color = (this.Index >= 50 || this.Type == 10) ? 0 : ((damageMax + damageMin + _sum(attributeType) + _sum(attributeValue)) % 5);
    this.ColorClass = (this.Type >= 8) ? 0 : this.Class;
    this.RuneType = Math.max(0, this.AttributeTypes[2] - 30);
    this.RuneValue = this.AttributeTypes[2] > 30 ? this.Attributes[2] : 0;
    this.SellPrice = {
      Gold: gold / 100,
      Mushrooms: coins
    };
  }

  get Strength() {
    if (!this._StrengthSet) {
      this._StrengthSet = true;
      this._Strength = this.getAttribute(1);
    }

    return this._Strength;
  }

  get Dexterity() {
    if (!this._DexteritySet) {
      this._DexteritySet = true;
      this._Dexterity = this.getAttribute(2);
    }

    return this._Dexterity;
  }

  get Intelligence() {
    if (!this._IntelligenceSet) {
      this._IntelligenceSet = true;
      this._Intelligence = this.getAttribute(3);
    }

    return this._Intelligence;
  }

  get Constitution() {
    if (!this._ConstitutionSet) {
      this._ConstitutionSet = true;
      this._Constitution = this.getAttribute(4);
    }

    return this._Constitution;
  }

  get Luck() {
    if (!this._LuckSet) {
      this._LuckSet = true;
      this._Luck = this.getAttribute(5);
    }

    return this._Luck;
  }

  morph(from: number, to: number, force = false) {
    if ((this.Type <= 7 || force) && this.SellPrice.Gold > 0) {
      var data = [... this.Data];
      for (var i = 0; i < 3; i++) {
        if (data[i + 4] == from) {
          data[i + 4] = to;
        } else if (data[i + 4] == from + 20) {
          data[i + 4] = to + 20;
        }
      }

      return new ItemModel(data, this.SlotType, this.SlotIndex);
    } else {
      return new ItemModel(this.Data, this.SlotType, this.SlotIndex);
    }
  }

  getAttribute(id: number) {
    for (var i = 0; i < 3; i++) {
      if (this.AttributeTypes[i] == id || this.AttributeTypes[i] == 6 || this.AttributeTypes[i] == 20 + id || (id > 3 && this.AttributeTypes[i] >= 21 && this.AttributeTypes[i] <= 23)) {
        return this.Attributes[i];
      }
    }
    return 0;
  }

  getRune(rune: number) {
    if (this.AttributeTypes[2] - 30 == rune) {
      return this.Attributes[2];
    } else {
      return 0;
    }
  }
  upgradeTo(upgrades: number) {
    upgrades = Math.max(0, Math.min(20, upgrades));
    if (upgrades > this.Upgrades) {
      while (this.Upgrades != upgrades) {
        this.Upgrades++;
        for (var j = 0; j < 3; j++) {
          if (this.AttributeTypes[j] < 30) {
            this.Attributes[j] = Math.trunc(1.03 * this.Attributes[j]);
          }
        }
      }
    } else if (upgrades < this.Upgrades) {
      while (this.Upgrades != upgrades) {
        this.Upgrades--;
        for (var j = 0; j < 3; j++) {
          if (this.AttributeTypes[j] < 30) {
            this.Attributes[j] = Math.trunc((1 / 1.03) * this.Attributes[j]);
          }
        }
      }
    }
  }

  static empty() {
    return new ItemModel(new Array(12).fill(0), 0, 0);
  }


  static SCRAPBOOK_BOUNDARIES = [
    {
      '7': [800, 1010],
      '8': [1050, 1210],
      '9': [1250, 1324]
    },
    {
      '0': [1364, 1664],
      '1': [1704, 1804],
      '2': [1844, 1944],
      '3': [1984, 2084],
      '4': [2124, 2224],
      '5': [2264, 2364],
      '6': [2404, 2504]
    },
    {
      '0': [2544, 2644],
      '2': [2684, 2784],
      '3': [2824, 2924],
      '4': [2964, 3064],
      '5': [3104, 3204],
      '6': [3244, 3344]
    },
    {
      '0': [3384, 3484],
      '2': [3524, 3624],
      '3': [3664, 3764],
      '4': [3804, 3904],
      '5': [3944, 4044],
      '6': [4084, 4184]
    }
  ];
}

export function _between(val: any, min: any, max: any) {
  return val > min && val < max;
}

function _lenWhere(array: any[], filter: any) {
  let count = 0;
  for (const obj of array) {
    if (filter(obj)) count++;
  }
  return count;
}
export function* _eachBlock(arr: any[], size: number) {
  for (let i = 0; i < arr.length / size; i++) {
    if (arr.length >= i * size + size) {
      yield arr.slice(i * size, i * size + size);
    }
  }
}

export function _dig(obj: any, ...path: (string | number)[]) {
  for (let i = 0; obj && i < path.length; i++) obj = obj[path[i]];
  return obj;
}

export function _sum(array: any[], base = 0) {
  return array.reduce((m, v) => m + v, base);
}

export const RUNE_VALUE = {
  GOLD: function(rune: number) {
    return rune < 2 ? 0 : (3 + 2 * (rune - 2));
  },
  EPIC_FIND: function(rune: number) {
    return rune < 2 ? 0 : (3 + 2 * (rune - 2));
  },
  ITEM_QUALITY: function(rune: number) {
    switch (rune) {
      case 1: return 3;
      case 2: return 19;
      case 3: return 50;
      case 4: return 75;
      case 5: return 99;
      default: return 0;
    }
  },
  XP: function(rune: number) {
    switch (rune) {
      case 1: return 3;
      case 2: return 9;
      case 3: return 25;
      case 4: return 35;
      case 5: return 45;
      case 6: return 55;
      case 7: return 65;
      case 8: return 75;
      case 9: return 85;
      case 10: return 95;
      default: return 0;
    }
  },
  HEALTH: function(rune: number) {
    switch (rune) {
      case 1: return 3;
      case 2: return 6;
      case 3: return 17;
      case 4: return 23;
      case 5: return 30;
      case 6: return 36;
      case 7: return 43;
      case 8: return 50;
      case 9: return 56;
      case 10: return 64;
      case 11: return 72;
      case 12: return 80;
      case 13: return 88;
      case 14: return 94;
      case 15: return 99;
      default: return 0;
    }
  },
  SINGLE_RESISTANCE: function(rune: number) {
    if (rune < 2) {
      return 0;
    } else {
      return Math.floor((rune - 0.4) / 0.75);
    }
  },
  TOTAL_RESISTANCE: function(rune: number) {
    return RUNE_VALUE.SINGLE_RESISTANCE(rune * 3);
  },
  ELEMENTAL_DAMAGE: function(rune: number) {
    if (rune < 2) {
      return 0;
    } else {
      return Math.floor((rune - 0.3) / 0.6);
    }
  }
}
