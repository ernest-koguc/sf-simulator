import { ASSASSIN, BARD, BATTLEMAGE, CONFIG, DEMONHUNTER, DRUID, MAGE, SCOUT, WARRIOR } from "../ClassConfig";
import { ItemModel, PlayerModel, RUNE_VALUE } from "../sfgame-parser";
import { Attribute, Equipment } from "../SFGameModels";
import { ByteParser } from "./ByteParser";

export function parseTower(towerData: number[], player: PlayerModel) {
  let dataType = new ByteParser(towerData.slice(448));
  const heart = dataType.long();
  const gate = dataType.long();
  const goldPit = dataType.long();
  const extractor = dataType.long();
  const goblinPit = dataType.long();
  const torture = dataType.long();
  const gladiator = dataType.long();
  const trollBlock = dataType.long();
  const timeMachine = dataType.long();
  const keeper = dataType.long();
  dataType.skip(1);
  const extractorSouls = dataType.long();
  const extractorMax = dataType.long();
  const maxSouls = dataType.long();
  dataType.skip(1);
  const extractorHourly = dataType.long();
  const goldPitGold = dataType.long() / 100;
  const goldPitMax = dataType.long() / 100;
  const goldPitHourly = dataType.long() / 100;
  dataType.skip(6);
  const timeMachineThirst = dataType.long();
  const timeMachineMax = dataType.long();
  const timeMachineDaily = dataType.long();
  const underworld = {
    Heart: heart,
    Gate: gate,
    GoldPit: goldPit,
    Extractor: extractor,
    GoblinPit: goblinPit,
    Torture: torture,
    Gladiator: gladiator,
    TrollBlock: trollBlock,
    TimeMachine: timeMachine,
    Keeper: keeper,
    ExtractorSouls: extractorSouls,
    ExtractorMax: extractorMax,
    MaxSouls: maxSouls,
    ExtractorHourly: extractorHourly,
    GoldPitGold: goldPitGold,
    GoldPitMax: goldPitMax,
    GoldPitHourly: goldPitHourly,
    TimeMachineThirst: timeMachineThirst,
    TimeMachineMax: timeMachineMax,
    TimeMachineDaily: timeMachineDaily
  }

  dataType = new ByteParser(towerData);

  dataType.skip(3);
  const bert = CompanionModel.fromTower(dataType);
  const bertInventory = PlayerModel.loadEquipment(dataType, 2, WARRIOR);

  dataType.skip(6);
  const mark = CompanionModel.fromTower(dataType);
  const markInventory = PlayerModel.loadEquipment(dataType, 3, MAGE);

  dataType.skip(6);
  const kuni = CompanionModel.fromTower(dataType);
  const kunigundeInventory = PlayerModel.loadEquipment(dataType, 4, SCOUT);

  const companions = {
    Bert: new CompanionModel(player, bert, bertInventory, WARRIOR),
    Mark: new CompanionModel(player, mark, markInventory, MAGE),
    Kunigunde: new CompanionModel(player, kuni, kunigundeInventory, SCOUT)
  };

  return {
    Underworld: underworld,
    Companions: companions
  }
}

export class CompanionModel extends PlayerModel {
  constructor(player: PlayerModel, comp: any, items: Equipment, pclass: number) {
    super(null, null);

    this.ID = -390 - pclass;
    this.Name = pclass == 1 ? "Bert" : pclass == 2 ? "Mark" : "Kunigunde";
    this.Level = comp.Level;
    this.Class = pclass;
    this.Armor = comp.Armor;
    this.Damage = comp.Damage;
    this.Potions = player.Potions;
    this.PotionsLife = player.PotionsLife;
    this.Pets = player.Pets;
    this.Fortress = player.Fortress;

    this.Items = items;
    for (const [key, item] of Object.entries(this.Items).filter(v => v[1] != null)) {
      const k = key as keyof Equipment;
      if (player.Class == BATTLEMAGE && this.Class == MAGE && item!.Class == MAGE && item!.Type > 1) {
        // When player is BattleMage and it's Mage equipment -> Strength into Intelligence
        this.Items[k] = item!.morph(1, 3);
      } else if (player.Class == ASSASSIN && this.Class == WARRIOR && item!.Class == WARRIOR && item!.Type == 1) {
        // When player is Assassin and it's Warrior weapon -> Dexterity into Strength
        this.Items[k] = item!.morph(2, 1);
      } else if (player.Class == DEMONHUNTER && this.Class == WARRIOR && item!.Class == WARRIOR && item!.Type > 1) {
        // When player is DemonHunter and it's Warrior equipment -> Dexterity into Strength
        this.Items[k] = item!.morph(2, 1);
      } else if (player.Class == DRUID && this.Class == SCOUT && item!.Class == SCOUT && item!.Type > 1) {
        // When player is Druid and it's Scout equipment -> Intelligence into Dexterity
        this.Items[k] = item!.morph(3, 2);
      } else if (player.Class == BARD && this.Class == SCOUT && item!.Class == SCOUT && item!.Type > 1) {
        // When player is Bard and it's Scout equipment -> Intelligence into Dexterity
        this.Items[k] = item!.morph(3, 2);
      }
    }

    this.Strength = comp.Strength;
    this.Dexterity = comp.Dexterity;
    this.Intelligence = comp.Intelligence;
    this.Constitution = comp.Constitution;
    this.Luck = comp.Luck;

    this.evaluateCommonCompanion(player);
  }

  evaluateCommonCompanion(player: PlayerModel) {
    this.Config = (CONFIG as any).fromID(this.Class);
    this.ItemsArray = Object.values(this.Items).filter(v => v !== null) as ItemModel[];

    this.Primary = this.getPrimaryAttribute();

    this.addCalculatedAttributes(this.Strength, player.Pets.Water);
    this.addCalculatedAttributes(this.Dexterity, player.Pets.Light);
    this.addCalculatedAttributes(this.Intelligence, player.Pets.Earth);
    this.addCalculatedAttributes(this.Constitution, player.Pets.Shadow);
    this.addCalculatedAttributes(this.Luck, player.Pets.Fire);

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

    for (const item of this.ItemsArray) {
      if (item.HasRune) {
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
          this.Runes.Resistance += Math.trunc(value / 3);
          if (RUNE_VALUE.SINGLE_RESISTANCE(value) > this.Runes.Runes) {
            this.Runes.Runes = RUNE_VALUE.SINGLE_RESISTANCE(value);
          }
        } else if (rune == 37) {
          this.Runes.ResistanceCold += value;
          this.Runes.Resistance += Math.trunc(value / 3);
          if (RUNE_VALUE.SINGLE_RESISTANCE(value) > this.Runes.Runes) {
            this.Runes.Runes = RUNE_VALUE.SINGLE_RESISTANCE(value);
          }
        } else if (rune == 38) {
          this.Runes.ResistanceLightning += value;
          this.Runes.Resistance += Math.trunc(value / 3);
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
        } else if (rune == 40) {
          this.Runes.DamageFire += value;
          this.Runes.Damage += value;
          if (RUNE_VALUE.ELEMENTAL_DAMAGE(value) > this.Runes.Runes) {
            this.Runes.Runes = RUNE_VALUE.ELEMENTAL_DAMAGE(value);
          }
        } else if (rune == 41) {
          this.Runes.DamageCold += value;
          this.Runes.Damage += value;
          if (RUNE_VALUE.ELEMENTAL_DAMAGE(value) > this.Runes.Runes) {
            this.Runes.Runes = RUNE_VALUE.ELEMENTAL_DAMAGE(value);
          }
        } else if (rune == 42) {
          this.Runes.DamageLightning += value;
          this.Runes.Damage += value;
          if (RUNE_VALUE.ELEMENTAL_DAMAGE(value) > this.Runes.Runes) {
            this.Runes.Runes = RUNE_VALUE.ELEMENTAL_DAMAGE(value);
          }
        }
      }
    }

    this.Runes.Gold = Math.min(50, this.Runes.Gold);
    this.Runes.Chance = Math.min(50, this.Runes.Chance);
    this.Runes.Quality = Math.min(5, this.Runes.Quality);
    this.Runes.XP = Math.min(10, this.Runes.XP);
    this.Runes.Health = Math.min(15, this.Runes.Health);
    this.Runes.Resistance = Math.min(75, this.Runes.Resistance);
    this.Runes.ResistanceFire = Math.min(75, this.Runes.ResistanceFire);
    this.Runes.ResistanceCold = Math.min(75, this.Runes.ResistanceCold);
    this.Runes.ResistanceLightning = Math.min(75, this.Runes.ResistanceLightning);
    this.Runes.Damage = Math.min(60, this.Runes.Damage);
    this.Runes.DamageFire = Math.min(60, this.Runes.DamageFire);
    this.Runes.DamageCold = Math.min(60, this.Runes.DamageCold);
    this.Runes.DamageLightning = Math.min(60, this.Runes.DamageLightning);
  }

  // Override to disable gem doubling
  override getEquipmentGemBonus(attribute: Attribute) {
    let bonus = 0;
    for (const item of this.ItemsArray) {
      if (item.HasGem && (item.GemType == attribute.Type || item.GemType == 6 || (item.GemType == 7 && (attribute.Type == this.Primary.Type || attribute.Type == 4)))) {
        bonus += item.GemValue;
      }
    }

    return bonus;
  }

  static fromTower(dataType: ByteParser) {
    const data = {
      Level: dataType.long()
    } as PlayerModel;

    dataType.skip(3);
    PlayerModel.loadAttributes(data, dataType);

    data.Armor = dataType.long();
    data.Damage = {
      Min: dataType.long(),
      Max: dataType.long()
    };

    return data;
  }
}

