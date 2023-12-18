export type SFToolsCharacterModel = {
      Class: number,
      Level: number,
      Armor: number,
      Runes: {
        ResistanceFire: number,
        ResistanceCold: number,
        ResistanceLightning: number,
        Health: number
      },
      Dungeons: {
        Player: number,
        Group: number
      },
      Fortress: {
        Gladiator: number
      },
      Potions: {
        Life: number
      },
      Items: {
        Hand: {
          HasEnchantment: boolean
        },
        Wpn1: {
          DamageMin: number,
          DamageMax: number,
          HasEnchantment: boolean,
          AttributeTypes: { "2": number },
          Attributes: { "2": number }
        },
        Wpn2: {
          DamageMin: number,
          DamageMax: number,
          HasEnchantment: boolean,
          AttributeTypes: { "2": number },
          Attributes: { "2": number }
        },
      },
      BlockChance: number,
      Strength: { Total: number },
      Dexterity: { Total: number },
      Intelligence: { Total: number },
      Constitution: { Total: number },
      Luck: { Total: number },
    }
