import { Equipment, RuneType } from "../SFGameModels";
import { ByteParser } from "./ByteParser";

export function parseEquipment(equipmentData: number[]): Equipment {
  const dataType = new ByteParser(equipmentData);
  const items = {
    Head: new ItemModel(dataType.sub(19)),
    Body: new ItemModel(dataType.sub(19)),
    Hand: new ItemModel(dataType.sub(19)),
    Feet: new ItemModel(dataType.sub(19)),
    Neck: new ItemModel(dataType.sub(19)),
    Belt: new ItemModel(dataType.sub(19)),
    Ring: new ItemModel(dataType.sub(19)),
    Misc: new ItemModel(dataType.sub(19)),
    Wpn1: new ItemModel(dataType.sub(19)),
    Wpn2: null as ItemModel | null
  }

  const wpn2 = dataType.sub(19);
  const item = new ItemModel(wpn2);
  if (item.Type > 0) {
    items.Wpn2 = new ItemModel(wpn2);
  }

  return items;
}

export function parseCompanionEquipment(equipmentData: number[]) {
  const dataType = new ByteParser(equipmentData);

  const bertInventory = parseEquipment(dataType.sub(190));
  const markInventory = parseEquipment(dataType.sub(190));
  const kunigundeInventory = parseEquipment(dataType.sub(190));

  return {
    Bert: bertInventory,
    Mark: markInventory,
    Kunigunde: kunigundeInventory
  }
}

export function parseBackpack(backpackData: number[]): ItemModel[] {
  const dataType = new ByteParser(backpackData);
  const items = [];

  for (let i = 0; i < 45 && dataType.atLeast(19); i++) {
    const item = new ItemModel(dataType.sub(19));
    if (item.Type > 0) {
      items.push(item);
    }
  }

  return items;
}


export class ItemModel {
  public Data: number[];
  public GemType: number;
  public HasSocket: boolean;
  public GemValue: number;
  public HasGem: boolean;
  public HasRune: boolean;
  public Class: number;
  public PicIndex: number;
  public Index: number;
  public IsEpic: boolean;
  public Type: number;
  public IsFlushed: boolean;
  public HasValue: boolean;
  public Enchantment: number;
  public Armor: number;
  public DamageMin: number;
  public DamageMax: number;
  public Upgrades: number;
  public UpgradeMultiplier: number;
  public AttributeTypes: number[];
  public Attributes: number[];
  public HasEnchantment: boolean;
  public Color: number;
  public ColorClass: number;
  public RuneType: RuneType;
  public RuneValue: number;
  public Strength: number;
  public Dexterity: number;
  public Intelligence: number;
  public Constitution: number;
  public Luck: number;
  public SellPrice: {
    Gold: number,
    Mushrooms: number
  };
  public ItemQuality: number;
  public BaseStrength: number;
  public BaseDexterity: number;
  public BaseIntelligence: number;
  public BaseConstitution: number;
  public BaseLuck: number;

  constructor(data: number[]) {
    let dataType = new ByteParser(data);

    dataType.assert(19);

    // Item type
    const type = dataType.long();
    // Socket - 0 for no socket, 1 for socket and 2+ for slotted gems
    const socket = dataType.long();
    // Enchantment type
    const enchantmentType = dataType.long();
    // Picture index
    const picIndex = dataType.long();
    // Enchantment power
    dataType.long();
    // Damage Min / Armor
    const damageMin = dataType.long();
    // Damage Max
    const damageMax = dataType.long();
    // Attribute Types
    let attributeType = [
      dataType.long(),
      dataType.long(),
      dataType.long()
    ];
    // Attribute Values
    let attributeValue = [
      dataType.long(),
      dataType.long(),
      dataType.long()
    ];
    // Gold valuee
    const gold = dataType.long();
    // Mushroom value
    const coins = dataType.long();
    // Upgrade level
    const upgradeLevel = dataType.long();
    // Socketted gem power
    const socketPower = dataType.long();
    // Item level
    const itemLevel = dataType.long();
    // Secret
    dataType.long();

    if (attributeType[1] === 4 && attributeType[2] === 5) {
      // Legacy attribute
      attributeType = [20 + attributeType[0], 0, 0];
      attributeValue = [attributeValue[0], 0, 0];
    }

    this.Data = data;
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
    this.HasValue = (coins > 0 || gold > 0 || (upgradeLevel > 0 && gold != 0 && type != 1));
    this.Enchantment = enchantmentType;
    this.Armor = damageMin;
    this.ItemQuality = itemLevel;
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
    this.Strength = this.getAttribute(1);
    this.Dexterity = this.getAttribute(2);
    this.Intelligence = this.getAttribute(3);
    this.Constitution = this.getAttribute(4);
    this.Luck = this.getAttribute(5);
    this.BaseStrength = this.deupgrade(upgradeLevel, this.Strength);
    this.BaseDexterity = this.deupgrade(upgradeLevel, this.Dexterity);
    this.BaseIntelligence = this.deupgrade(upgradeLevel, this.Intelligence);
    this.BaseConstitution = this.deupgrade(upgradeLevel, this.Constitution);
    this.BaseLuck = this.deupgrade(upgradeLevel, this.Luck);
  }

  private deupgrade(upgradeLevel: number, attrValue: number) {
    while (upgradeLevel > 0) {
      attrValue = Math.round((1 / 1.03) * attrValue);
      upgradeLevel--;
    }
    return attrValue;
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

      return new ItemModel(data);
    } else {
      return new ItemModel(this.Data);
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
    return new ItemModel(new Array(19).fill(0));
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
