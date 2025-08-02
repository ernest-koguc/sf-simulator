export const WARRIOR = 1;
export const MAGE = 2;
export const SCOUT = 3;
export const ASSASSIN = 4;
export const BATTLEMAGE = 5;
export const BERSERKER = 6;
export const DEMONHUNTER = 7;
export const DRUID = 8;
export const BARD = 9;
export const NECROMANCER = 10;
export const PALADIN = 11;
export const PLAGUEDOCTOR = 12;

export const CONFIG = Object.defineProperties({
  General: {
    CritBase: 2,
    CritGladiatorBonus: 0.11,
    CritEnchantmentBonus: 0.05
  },
  Warrior: {
    ID: WARRIOR,

    Attribute: 'Strength',

    HealthMultiplier: 5,
    WeaponMultiplier: 2,
    DamageMultiplier: 1,
    MaximumDamageReduction: 50,
    MaximumDamageReductionMultiplier: 1,

    SkipChance: 0.25,
    SkipLimit: 999,
    //SkipType: SKIP_TYPE_DEFAULT,
    //SkipVariant: DEFENSE_TYPE_BLOCK,

    UseBlockChance: true
  },
  Mage: {
    ID: MAGE,

    Attribute: 'Intelligence',

    HealthMultiplier: 2,
    WeaponMultiplier: 4.5,
    DamageMultiplier: 1,
    MaximumDamageReduction: 10,
    MaximumDamageReductionMultiplier: 1,

    BypassDamageReduction: true,
    BypassSkipChance: true,
    BypassSpecial: true,

    SkipChance: 0,
    SkipLimit: 999,
    //SkipType: SKIP_TYPE_DEFAULT,
    //SkipVariant: DEFENSE_TYPE_NONE,

    PaladinDamageMultiplier: 1.5
  },
  Scout: {
    ID: SCOUT,

    Attribute: 'Dexterity',

    HealthMultiplier: 4,
    WeaponMultiplier: 2.5,
    DamageMultiplier: 1,
    MaximumDamageReduction: 25,
    MaximumDamageReductionMultiplier: 1,

    SkipChance: 0.50,
    SkipLimit: 999,
    //SkipType: SKIP_TYPE_DEFAULT,
    //SkipVariant: DEFENSE_TYPE_EVADE
  },
  Assassin: {
    ID: ASSASSIN,

    Attribute: 'Dexterity',

    HealthMultiplier: 4,
    WeaponMultiplier: 2,
    DamageMultiplier: 0.625,
    MaximumDamageReduction: 25,
    MaximumDamageReductionMultiplier: 1,

    SkipChance: 0.50,
    SkipLimit: 999,
    //SkipType: SKIP_TYPE_DEFAULT,
    //SkipVariant: DEFENSE_TYPE_EVADE
  },
  Battlemage: {
    ID: BATTLEMAGE,

    Attribute: 'Strength',

    HealthMultiplier: 5,
    WeaponMultiplier: 2,
    DamageMultiplier: 1,
    MaximumDamageReduction: 10,
    MaximumDamageReductionMultiplier: 5,

    SkipChance: 0,
    SkipLimit: 999,
    //SkipType: SKIP_TYPE_DEFAULT,
    //SkipVariant: DEFENSE_TYPE_NONE
  },
  Berserker: {
    ID: BERSERKER,

    Attribute: 'Strength',

    HealthMultiplier: 4,
    WeaponMultiplier: 2,
    DamageMultiplier: 1.25,
    MaximumDamageReduction: 50,
    MaximumDamageReductionMultiplier: 0.5,

    SkipChance: 0.5,
    SkipLimit: 14,
    //SkipType: SKIP_TYPE_CONTROL,
    //SkipVariant: DEFENSE_TYPE_NONE
  },
  DemonHunter: {
    ID: DEMONHUNTER,

    Attribute: 'Dexterity',

    HealthMultiplier: 4,
    WeaponMultiplier: 2.5,
    DamageMultiplier: 1,
    MaximumDamageReduction: 50,
    MaximumDamageReductionMultiplier: 1,

    SkipChance: 0,
    SkipLimit: 999,
    //SkipType: SKIP_TYPE_DEFAULT,
    //SkipVariant: DEFENSE_TYPE_NONE,

    ReviveChance: 0.44,
    ReviveChanceDecay: 0.11,
    ReviveHealth: 0.9,
    ReviveHealthMin: 0.1,
    ReviveHealthDecay: 0.1,
    ReviveDamage: 1,
    ReviveDamageMin: 0,
    ReviveDamageDecay: 0,
    ReviveMax: 999
  },
  Druid: {
    ID: DRUID,

    Attribute: 'Intelligence',

    HealthMultiplier: 5,
    WeaponMultiplier: 4.5,
    DamageMultiplier: 1 / 3,
    MaximumDamageReduction: 25,
    MaximumDamageReductionMultiplier: 1,

    SkipChance: 0.35,
    SkipLimit: 999,
    //SkipType: SKIP_TYPE_DEFAULT,
    //SkipVariant: DEFENSE_TYPE_EVADE,

    DemonHunterDamageMultiplier: 1.15,
    MageDamageMultiplier: 4 / 3,

    SwoopChance: 0.15,
    SwoopChanceMin: 0,
    SwoopChanceMax: 0.50,
    SwoopChanceDecay: -0.05,
    SwoopBonus: 0.8,

    Rage: {
      SkipChance: 0,
      CriticalChance: 0.75,
      CriticalChanceBonus: 0.1,
      CriticalBonus: 4
    }
  },
  Bard: {
    ID: BARD,

    Attribute: 'Intelligence',

    HealthMultiplier: 2,
    WeaponMultiplier: 4.5,
    DamageMultiplier: 1.125,
    MaximumDamageReduction: 25,
    MaximumDamageReductionMultiplier: 2,

    SkipChance: 0,
    SkipLimit: 999,
    //SkipType: SKIP_TYPE_DEFAULT,
    //SkipVariant: DEFENSE_TYPE_NONE,

    ConIntRatioDependentRoundBonus: true,//Playa wants to remove this in some future update, with that toggle it could be tested now

    EffectRounds: 4,
    EffectBaseDuration: [1, 1, 2],
    EffectBaseChance: [25, 50, 25],
    EffectValues: [0.2, 0.4, 0.6]
  },
  Necromancer: {
    ID: NECROMANCER,

    Attribute: 'Intelligence',

    HealthMultiplier: 4,
    WeaponMultiplier: 4.5,
    DamageMultiplier: 5 / 9,
    MaximumDamageReduction: 10,
    MaximumDamageReductionMultiplier: 2,

    SkipChance: 0,
    SkipLimit: 999,
    //SkipType: SKIP_TYPE_DEFAULT,
    //SkipVariant: DEFENSE_TYPE_NONE,

    DemonHunterDamageBonus: 0.1,

    SummonChance: 0.5,
    SummonImmediateAttack: true,
    Summons: [
      {
        Duration: 3,
        DamageBonus: 0.25,
        SkipChance: 0,
        CriticalBonus: 0,
        CriticalChance: 0.5,
        CriticalChanceBonus: 0,
        ReviveCount: 1,//should be 2; sfgame is bugged, for some reason the skeleton only revives once
        ReviveDuration: 1,
        ReviveChance: 0.5
      },
      {
        Duration: 2,
        DamageBonus: 1,
        SkipChance: 0,
        CriticalBonus: 0.5,
        CriticalChance: 0.6,
        CriticalChanceBonus: 0.1
      },
      {
        Duration: 4,
        DamageBonus: 0,
        SkipChance: 0.25,
        CriticalBonus: 0,
        CriticalChance: 0.5,
        CriticalChanceBonus: 0,
        //SkipVariant: DEFENSE_TYPE_BLOCK
      }
    ]
  },
  Paladin: {
    ID: PALADIN,

    Attribute: 'Strength',

    HealthMultiplier: 6,
    WeaponMultiplier: 2,
    DamageMultiplier: 0.833,

    MaximumDamageReduction: 45,
    MaximumDamageReductionMultiplier: 1,

    SkipChance: 0,
    SkipLimit: 999,
    //SkipType: SKIP_TYPE_DEFAULT,
    //SkipVariant: DEFENSE_TYPE_NONE,

    MageDamageMultiplier: 1.5,
    AssassinDamageMultiplier: 1,
    DruidDamageMultiplier: 1,

    StanceInitial: 0,
    Stances: [
      {
        Name: 'NEUTRAL',
        /* Nothing, he just sad */
        DamageBonus: 0,
        DamageReductionBonus: 0,
        MaximumDamageReductionBonus: 0,
        SkipChance: 0.3,
        CriticalBonus: 0,
        CriticalChance: 0.5,
        CriticalChanceBonus: 0,
        StanceChangeChance: 0.5
      },
      {
        Name: 'DEFENSIVE',
        DamageBonus: -0.265,
        DamageReductionBonus: 0,
        MaximumDamageReductionBonus: 0,
        SkipChance: 0.5,
        CriticalBonus: 0,
        CriticalChance: 0.5,
        CriticalChanceBonus: 0,
        StanceChangeChance: 0.5,
        HealMultiplier: 0.3,
        //SkipVariant: DEFENSE_TYPE_BLOCK_HEAL
      },
      {
        Name: 'OFFENSIVE',
        DamageBonus: 0.42,
        DamageReductionBonus: 0,
        MaximumDamageReductionBonus: -25,
        SkipChance: 0.25,
        CriticalBonus: 0,
        CriticalChance: 0.5,
        CriticalChanceBonus: 0,
        StanceChangeChance: 0.5
      }
    ]
  },
  PlagueDoctor: {
    ID: PLAGUEDOCTOR,

    Attribute: 'Dexterity',

    HealthMultiplier: 4,
    WeaponMultiplier: 2,
    DamageMultiplier: 5 / 4,
    MaximumDamageReduction: 25,
    MaximumDamageReductionMultiplier: 1,

    SkipChance: 0,
    SkipLimit: 999,
    //SkipType: SKIP_TYPE_DEFAULT,
    //SkipVariant: DEFENSE_TYPE_NONE,

    AssassinDamageBonus: 0,
    BattlemageDamageBonus: 0,
    BattlemageDamageMultiplier: 1,
    DemonHunterDamageBonus: 0,
    DemonHunterDamageMultiplier: 1,

    TinctureChance: 0.55,
    DelayAfterTincture: true,
    TinctureEndsAfterEnemyAttack: false,
    Tinctures: [
      {   // Red
        Duration: 1,
        DamageBonus: 0.33,
        SkipChance: 0.4,
        CriticalBonus: 0,
        CriticalChance: 0.5,
        CriticalChanceBonus: 0,
        //SkipVariant: DEFENSE_TYPE_EVADE
      },
      {   // Yellow
        Duration: 2,
        DamageBonus: 0,
        SkipChance: 0.5,
        CriticalBonus: 0,
        CriticalChance: 0.5,
        CriticalChanceBonus: 0,
        //SkipVariant: DEFENSE_TYPE_EVADE
      },
      {   // Green
        Duration: 3,
        DamageBonus: -0.1,
        SkipChance: 0.6,
        CriticalBonus: 0,
        CriticalChance: 0.5,
        CriticalChanceBonus: 0,
        //SkipVariant: DEFENSE_TYPE_EVADE
      }
    ]
  }
},
  {
    fromID: {
      value: function(index: number) {
        return Object.values(this)[index];
      }
    }
  }
);
