export type Character = {
  level: number,
  class: ClassType,
  strength: number,
  dexterity: number,
  intelligence: number,
  constitution: number,
  luck: number,
  armor: number,
  firstWeapon: Weapon | undefined,
  secondWeapon: Weapon | undefined
  runeBonuses: ResistanceRuneBonuses,
  hasGlovesScroll: boolean,
  hasWeaponScroll: boolean,
  hasEternityPotion: boolean,
  gladiatorLevel: number,
  soloPortal: number, 
  guildPortal: number,
} 

export type Weapon = {
  minDmg: number,
  maxDmg: number,
  damageRuneType: DamageRuneType,
  runeBonus: number 
}

export type ResistanceRuneBonuses = {
  lightningResistance: number,
  fireResistance: number,
  coldResistance: number,
  healthRune: number
}

export enum ClassType {
  Warrior = 1,
  Scout = 2,
  Mage = 3,
  Assassin = 4,
  Berserker = 5,
  BattleMage = 6,
  Druid = 7,
  DemonHunter = 8,
  Bard = 9
}

export enum DamageRuneType {
    None = 0,
    Lightning = 1,
    Fire = 2,
    Cold = 3
}
