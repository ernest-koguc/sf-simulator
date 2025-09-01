import { CONFIG } from "./ClassConfig";
import { Class, Companion, Dungeons, Equipment, OwnPlayerSave, RuneType, Tower } from "./SFGameModels";

export class Fightable {
  public Level: number;
  public Class: Class;
  public Strength: number;
  public Dexterity: number;
  public Intelligence: number;
  public Constitution: number;
  public Luck: number;
  public Health: number;
  public Armor: number;
  public FirstWeapon: {
    MinDmg: number,
    MaxDmg: number,
    RuneType: number,
    RuneValue: number,
  };
  public SecondWeapon: {
    MinDmg: number,
    MaxDmg: number,
    RuneType: number,
    RuneValue: number,
  } | null
  public Reaction: number;
  public CritMultiplier: number;
  public LightningResistance: number;
  public FireResistance: number;
  public ColdResistance: number;
  public GuildPortal: number;
  public Companions:
    {
      Class: Class,
      Strength: number,
      Dexterity: number,
      Intelligence: number,
      Constitution: number,
      Luck: number,
      Health: number,
      Armor: number,
      FirstWeapon: {
        MinDmg: number,
        MaxDmg: number,
        RuneType: number,
        RuneValue: number,
      },
      Reaction: number,
      CritMultiplier: number,
      LightningResistance: number,
      FireResistance: number,
      ColdResistance: number,
    }[];

  constructor(playerSave: OwnPlayerSave, playerEquipment: Equipment, companionsEquipment: CompanionsEquipment | null,
    tower: Tower | null, dungeons: Dungeons) {
    this.Level = playerSave.Level;
    this.Class = playerSave.Class;
    this.Strength = playerSave.Strength.Total;
    this.Dexterity = playerSave.Dexterity.Total;
    this.Intelligence = playerSave.Intelligence.Total;
    this.Constitution = playerSave.Constitution.Total;
    this.Luck = playerSave.Luck.Total;
    this.Health = getHealth(playerSave.Class, playerSave.Level, playerSave.Constitution.Total, playerSave.HasEternalPotion, dungeons.SoloPortal, getRuneValue(RuneType.HealthBonus, playerEquipment));
    this.Armor = playerSave.Armor;
    this.FirstWeapon = {
      MinDmg: playerEquipment.Wpn1.DamageMin,
      MaxDmg: playerEquipment.Wpn1.DamageMax,
      RuneType: playerEquipment.Wpn1.RuneType,
      RuneValue: playerEquipment.Wpn1.RuneValue,
    }
    this.SecondWeapon = playerEquipment.Wpn2 ? {
      MinDmg: playerEquipment.Wpn2.DamageMin,
      MaxDmg: playerEquipment.Wpn2.DamageMax,
      RuneType: playerEquipment.Wpn2.RuneType,
      RuneValue: playerEquipment.Wpn2.RuneValue,
    } : null;
    this.Reaction = playerEquipment.Hand.HasEnchantment ? 1 : 0;
    this.CritMultiplier = 2 + (playerEquipment.Wpn1.HasEnchantment || playerEquipment.Wpn2?.HasEnchantment ? 0.05 : 0) + 0.11 * (tower?.Underworld.Gladiator ?? 0);
    this.GuildPortal = playerSave.GuildPortal;
    this.LightningResistance = getRuneValue(RuneType.LightningResistance, playerEquipment);
    this.FireResistance = getRuneValue(RuneType.FireResistance, playerEquipment);
    this.ColdResistance = getRuneValue(RuneType.ColdResistance, playerEquipment);


    const comps = [];
    if (companionsEquipment && tower) {
      comps.push({ companion: tower.Companions.Bert, equipment: companionsEquipment.Bert });
      comps.push({ companion: tower.Companions.Mark, equipment: companionsEquipment.Mark });
      comps.push({ companion: tower.Companions.Kunigunde, equipment: companionsEquipment.Kunigunde })
      this.Companions = comps.map((c, i) => {
        let hp;
        if (i === 0) {
          hp = getHealth(i + 1, playerSave.Level, c.companion.Constitution.Total, playerSave.HasEternalPotion, dungeons.SoloPortal, getRuneValue(RuneType.HealthBonus, c.equipment), 6.1);
        } else {
          hp = getHealth(i + 1, playerSave.Level, c.companion.Constitution.Total, playerSave.HasEternalPotion, dungeons.SoloPortal, getRuneValue(RuneType.HealthBonus, c.equipment));
        }

        return {
          Class: i + 1,
          Strength: c.companion.Strength.Total,
          Dexterity: c.companion.Dexterity.Total,
          Intelligence: c.companion.Intelligence.Total,
          Constitution: c.companion.Constitution.Total,
          Luck: c.companion.Luck.Total,
          Health: hp,
          Armor: c.companion.Armor,
          FirstWeapon: {
            MinDmg: c.equipment.Wpn1.DamageMin,
            MaxDmg: c.equipment.Wpn1.DamageMax,
            RuneType: c.equipment.Wpn1.RuneType,
            RuneValue: c.equipment.Wpn1.RuneValue,
          },
          Reaction: c.equipment.Hand.HasEnchantment ? 1 : 0,
          CritMultiplier: 2 + (c.equipment.Wpn1.HasEnchantment ? 0.05 : 0) + 0.11 * tower.Underworld.Gladiator,
          LightningResistance: getRuneValue(RuneType.LightningResistance, c.equipment),
          FireResistance: getRuneValue(RuneType.FireResistance, c.equipment),
          ColdResistance: getRuneValue(RuneType.ColdResistance, c.equipment),
        }
      });
    } else {
      this.Companions = [];
    }

  }
}

function getRuneValue(runeType: RuneType, equipment: Equipment) {
  const items = [
    equipment.Head,
    equipment.Body,
    equipment.Hand,
    equipment.Feet,
    equipment.Neck,
    equipment.Belt,
    equipment.Ring,
    equipment.Misc,
    equipment.Wpn1,
    equipment.Wpn2
  ].filter(i => i !== null) as (typeof equipment.Wpn1)[];

  const runeTypes = [runeType];
  if (runeType === RuneType.FireResistance || runeType === RuneType.ColdResistance || runeType === RuneType.LightningResistance) {
    runeTypes.push(RuneType.TotalResistance);
  }

  const runeValue = items.filter(v => runeTypes.indexOf(v.RuneType) >= 0).reduce((a, b) => a + b.RuneValue, 0);

  return Math.min(runeValue, RUNE_CAP[runeType]);
}

const RUNE_CAP: Record<RuneType, number> = {
  [RuneType.None]: 0,
  [RuneType.GoldBonus]: 50,
  [RuneType.EpicChance]: 50,
  [RuneType.ItemQuality]: 5,
  [RuneType.ExperienceBonus]: 10,
  [RuneType.HealthBonus]: 15,
  [RuneType.FireResistance]: 75,
  [RuneType.ColdResistance]: 75,
  [RuneType.LightningResistance]: 75,
  [RuneType.TotalResistance]: 75,
  [RuneType.FireDamage]: 60,
  [RuneType.ColdDamage]: 60,
  [RuneType.LightningDamage]: 60,
}

function getHealth(classType: Class, level: number, constitution: number, hasEternalPotion: boolean, soloPortal: number, hpRune: number, hpMultiplier?: number) {
  const config = (CONFIG as any).fromID(classType);

  let ma = hpMultiplier || config.HealthMultiplier;
  let mb = (100 + soloPortal) / 100;
  let mc = hasEternalPotion ? 1.25 : 1;
  let md = (100 + hpRune) / 100;

  return Math.trunc(Math.floor(Math.floor(constitution * ma * (level + 1) * mb) * mc) * md);
}


type Companions = {
  Bert: Companion,
  Mark: Companion,
  Kunigunde: Companion,
}

type CompanionsEquipment = {
  Bert: Equipment,
  Mark: Equipment,
  Kunigunde: Equipment,
}
