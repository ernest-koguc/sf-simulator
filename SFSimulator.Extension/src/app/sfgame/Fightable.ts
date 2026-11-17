import { Class, Companion, Equipment, OwnPlayerSave, Pets, Potion, RuneType, Tower } from './SFGameModels';
import { getCompanionAttributes } from './CompanionAttributes';

type Weapon = { MinDmg: number; MaxDmg: number; RuneType: number; RuneValue: number };
type CompanionFighter = {
  Class: Class;
  Strength: number;
  Dexterity: number;
  Intelligence: number;
  Constitution: number;
  Luck: number;
  Health: number;
  Armor: number;
  FirstWeapon: Weapon;
  Reaction: number;
  CritMultiplier: number;
  LightningResistance: number;
  FireResistance: number;
  ColdResistance: number;
};

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
  public FirstWeapon: Weapon;
  public SecondWeapon: Weapon | null;
  public Reaction: number;
  public CritMultiplier: number;
  public LightningResistance: number;
  public FireResistance: number;
  public ColdResistance: number;
  public GuildPortal: number;
  public Companions: CompanionFighter[];

  constructor(player: OwnPlayerSave, equipment: Equipment,
    companionEquipment: { Bert: Equipment; Mark: Equipment; Kunigunde: Equipment } | null,
    tower: Tower | null, hasEternalPotion: boolean, potions: readonly Potion[], pets: Pets | null) {
    this.Level = player.Level;
    this.Class = player.Class;
    this.Strength = player.Strength;
    this.Dexterity = player.Dexterity;
    this.Intelligence = player.Intelligence;
    this.Constitution = player.Constitution;
    this.Luck = player.Luck;
    this.Health = getHealth(player.Class, player.Level, player.Constitution, hasEternalPotion,
      player.PortalHealthBonus, getRuneValue(RuneType.HealthBonus, equipment));
    this.Armor = player.Armor;
    this.FirstWeapon = weapon(equipment.Wpn1);
    this.SecondWeapon = equipment.Wpn2?.Type === 1 ? weapon(equipment.Wpn2) : null;
    this.Reaction = equipment.Hand.HasEnchantment ? 1 : 0;
    const gladiator = tower?.Underworld.Gladiator ?? 0;
    this.CritMultiplier = 2 + (equipment.Wpn1.HasEnchantment || equipment.Wpn2?.HasEnchantment ? 0.05 : 0)
      + 0.11 * gladiator;
    this.GuildPortal = player.GuildPortal;
    this.LightningResistance = getRuneValue(RuneType.LightningResistance, equipment);
    this.FireResistance = getRuneValue(RuneType.FireResistance, equipment);
    this.ColdResistance = getRuneValue(RuneType.ColdResistance, equipment);

    const companion = (stats: Companion, items: Equipment, classType: Class): CompanionFighter => {
      const attributes = getCompanionAttributes(stats, items, classType, player.Class, potions, pets);
      return {
        Class: classType,
        ...attributes,
        Health: getHealth(classType, Math.max(1, stats.Level), attributes.Constitution, hasEternalPotion,
          player.PortalHealthBonus, getRuneValue(RuneType.HealthBonus, items), true),
        Armor: Object.values(items).reduce((sum, item) => sum + (item?.Armor ?? 0), 0),
        FirstWeapon: weapon(items.Wpn1),
        Reaction: items.Hand.HasEnchantment ? 1 : 0,
        CritMultiplier: 2 + (items.Wpn1.HasEnchantment ? 0.05 : 0) + 0.11 * gladiator,
        LightningResistance: getRuneValue(RuneType.LightningResistance, items),
        FireResistance: getRuneValue(RuneType.FireResistance, items),
        ColdResistance: getRuneValue(RuneType.ColdResistance, items),
      };
    };
    this.Companions = companionEquipment && tower ? [
      companion(tower.Companions.Bert, companionEquipment.Bert, Class.Warrior),
      companion(tower.Companions.Mark, companionEquipment.Mark, Class.Mage),
      companion(tower.Companions.Kunigunde, companionEquipment.Kunigunde, Class.Scout),
    ] : [];
  }
}

function weapon(item: Equipment['Wpn1']): Weapon {
  return { MinDmg: item.DamageMin, MaxDmg: item.DamageMax, RuneType: item.RuneType, RuneValue: item.RuneValue };
}

function getRuneValue(runeType: RuneType, equipment: Equipment): number {
  const resistance = runeType === RuneType.FireResistance || runeType === RuneType.ColdResistance
    || runeType === RuneType.LightningResistance;
  const total = Object.values(equipment).reduce((sum, item) =>
    sum + (item && (item.RuneType === runeType || resistance && item.RuneType === RuneType.TotalResistance)
      ? item.RuneValue : 0), 0);
  return Math.min(total, resistance ? 75 : 15);
}

const HEALTH_MULTIPLIER: Record<Class, number> = {
  [Class.Warrior]: 5,
  [Class.Mage]: 2,
  [Class.Scout]: 4,
  [Class.Assassin]: 4,
  [Class.BattleMage]: 5,
  [Class.Berserker]: 4,
  [Class.DemonHunter]: 4,
  [Class.Druid]: 5,
  [Class.Bard]: 2,
  [Class.Necromancer]: 4,
  [Class.Paladin]: 6,
  [Class.PlagueDoctor]: 4,
  [Class.BloodWeaver]: 4,
};

function getHealth(classType: Class, level: number, constitution: number, eternal: boolean,
  portalBonus: number, healthRune: number, companion = false): number {
  const multiplier = companion && classType === Class.Warrior ? 6.1 : HEALTH_MULTIPLIER[classType];
  let health = Math.trunc(constitution * multiplier) * (level + 1);
  if (eternal) {
    health = Math.trunc(health * 1.25);
  }
  health += Math.trunc(health * portalBonus / 100);
  health += Math.trunc(health * healthRune / 100);
  return health;
}
