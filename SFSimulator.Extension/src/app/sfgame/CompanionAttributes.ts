import { Attributes, AttributeType, Class, Companion, Equipment, Pets, Potion } from './SFGameModels';
import { ItemModel } from './parsers/EquipmentParser';

enum EquipmentCategory {
  Weapon,
  NonWeapon,
}

const ATTRIBUTE_CONVERSIONS = [
  [Class.BattleMage, Class.Mage, EquipmentCategory.NonWeapon, AttributeType.Strength, AttributeType.Intelligence],
  [Class.Assassin, Class.Warrior, EquipmentCategory.Weapon, AttributeType.Dexterity, AttributeType.Strength],
  [Class.DemonHunter, Class.Warrior, EquipmentCategory.NonWeapon, AttributeType.Dexterity, AttributeType.Strength],
  [Class.Druid, Class.Scout, EquipmentCategory.NonWeapon, AttributeType.Intelligence, AttributeType.Dexterity],
  [Class.Bard, Class.Scout, EquipmentCategory.NonWeapon, AttributeType.Intelligence, AttributeType.Dexterity],
  [Class.PlagueDoctor, Class.Warrior, EquipmentCategory.Weapon, AttributeType.Dexterity, AttributeType.Strength],
  [Class.PlagueDoctor, Class.Mage, EquipmentCategory.NonWeapon, AttributeType.Dexterity, AttributeType.Intelligence],
] as const;

export function getCompanionAttributes(stats: Companion, items: Equipment, classType: Class,
  playerClass: Class, potions: readonly Potion[], pets: Pets | null): Attributes {
  const primary = classType === Class.Warrior ? AttributeType.Strength
    : classType === Class.Mage ? AttributeType.Intelligence : AttributeType.Dexterity;
  const equipment = Object.entries(items).flatMap(([slot, item]) => {
    if (!item || item.Type === 0 || slot === 'Wpn2' && classType !== Class.Warrior) {
      return [];
    }
    return [{ item, types: getAttributeTypes(item, classType, playerClass) }];
  });

  const total = (name: keyof Attributes, type: AttributeType, petBonus: number): number => {
    if (stats.Bonus[name]) {
      return stats[name] + stats.Bonus[name];
    }
    let value = stats[name];
    for (const { item, types } of equipment) {
      for (let index = 0; index < types.length; index++) {
        const attributeType = types[index];
        if (attributeType === type || attributeType === AttributeType.All || attributeType === type + 20
          || type > AttributeType.Intelligence && attributeType >= 21 && attributeType <= 23) {
          value += item.Attributes[index];
        }
      }
      if (item.HasGem && (item.GemType === type || item.GemType === 6
        || item.GemType === 7 && (type === primary || type === AttributeType.Constitution))) {
        value += item.GemValue;
      }
    }
    const potion = potions.find(potion => potion.Type === type);
    value += Math.ceil(value * (potion?.Size ?? 0) / 100);
    return value + Math.ceil(value * petBonus / 100);
  };

  return {
    Strength: total('Strength', AttributeType.Strength, pets?.Water ?? 0),
    Dexterity: total('Dexterity', AttributeType.Dexterity, pets?.Light ?? 0),
    Intelligence: total('Intelligence', AttributeType.Intelligence, pets?.Earth ?? 0),
    Constitution: total('Constitution', AttributeType.Constitution, pets?.Shadow ?? 0),
    Luck: total('Luck', AttributeType.Luck, pets?.Fire ?? 0),
  };
}

function getAttributeTypes(item: ItemModel, classType: Class, playerClass: Class): readonly number[] {
  if (item.Class !== classType || item.Type > 7 || item.Gold <= 0) {
    return item.AttributeTypes;
  }
  const conversion = ATTRIBUTE_CONVERSIONS.find(([player, companion, equipmentCategory]) =>
    player === playerClass && companion === classType
    && (equipmentCategory === EquipmentCategory.Weapon ? item.Type === 1 : item.Type > 1));
  if (!conversion) {
    return item.AttributeTypes;
  }
  const [, , , from, to] = conversion;
  return item.AttributeTypes.map(type => type === from ? to : type === from + 20 ? to + 20 : type);
}
