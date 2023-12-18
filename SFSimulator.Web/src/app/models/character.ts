export type Character = {
  level: number,
  class: ClassType,
  strength: number,
  dexterity: number,
  intelligence: number,
  constitution: number,
  luck: number,
  armor: number,
  firstWeapon: Weapon | null,
  secondWeapon: Weapon | null,
  lightningResistance: number,
  fireResistance: number,
  coldResistance: number,
  healthRune: number
  reaction: boolean | number,
  hasWeaponScroll: boolean,
  hasEternityPotion: boolean,
  gladiatorLevel: number,
  soloPortal: number,
  guildPortal: number,
}

export type Companion = {
  class: ClassType,
  strength: number,
  dexterity: number,
  intelligence: number,
  constitution: number,
  luck: number,
  armor: number,
  firstWeapon: Weapon | null,
  lightningResistance: number,
  fireResistance: number,
  coldResistance: number,
  healthRune: number
  reaction: boolean | number,
  hasWeaponScroll: boolean,
}

export type Weapon = {
  minDmg: number | null,
  maxDmg: number | null,
  runeType: RuneType | null,
  runeValue: number | null
}

export enum ClassType {
  Bert = 0,
  Warrior = 1,
  Mage = 2,
  Scout = 3,
  Assassin = 4,
  BattleMage = 5,
  Berserker = 6,
  DemonHunter = 7,
  Druid = 8,
  Bard = 9
}

export enum RuneType {
    None = 0,
    Fire = 10,
    Cold = 11,
    Lightning = 12,
}
