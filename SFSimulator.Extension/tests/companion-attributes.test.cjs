const assert = require('node:assert/strict');
const { test } = require('node:test');
const fs = require('node:fs');
const ts = require('typescript');

require.extensions['.ts'] = (module, filename) => {
  const result = ts.transpileModule(fs.readFileSync(filename, 'utf8'), {
    compilerOptions: {
      module: ts.ModuleKind.CommonJS,
      target: ts.ScriptTarget.ES2022,
      experimentalDecorators: true,
    },
    fileName: filename,
  });
  module._compile(result.outputText, filename);
};

const core = require('@angular/core');
const { Class } = require('../src/app/sfgame/SFGameModels.ts');
const { getCompanionAttributes } = require('../src/app/sfgame/CompanionAttributes.ts');
const { ItemModel, parseEquipment } = require('../src/app/sfgame/parsers/EquipmentParser.ts');
const { parseTower } = require('../src/app/sfgame/parsers/TowerParser.ts');
const { parsePotions } = require('../src/app/sfgame/parsers/CharacterStatusParser.ts');
const { SFGameRequestHandler } = require('../src/app/services/SFGameRequestHandler.ts');
const { SessionManager } = require('../src/app/services/SessionManager.ts');

const zero = { Strength: 0, Dexterity: 0, Intelligence: 0, Constitution: 0, Luck: 0 };
const base = { Strength: 101, Dexterity: 102, Intelligence: 103, Constitution: 104, Luck: 105 };
const companion = (attributes = zero, bonus = zero) => ({ Level: 10, ...attributes, Bonus: { ...bonus } });
const words = (length, values = {}) => Object.assign(new Array(length).fill(0), values);
const item = (values = {}) => new ItemModel(words(19, { 0: 3, 13: 100, ...values }));
const attributes = fighter => Object.fromEntries(Object.keys(zero).map(name => [name, fighter[name]]));

function gear() {
  const items = parseEquipment([]);
  items.Head = item({ 1: 15, 7: 6, 10: 11, 16: 7 });
  items.Body = item({ 0: 4, 7: 21, 9: 35, 10: 13, 12: 9, 15: 20 });
  items.Wpn1 = item({ 0: 1, 1: 15, 5: 123, 6: 456, 7: 1, 10: 17, 16: 19 });
  return items;
}

test('tower retains each companion base and server bonus separately', () => {
  const data = words(474);
  for (let index = 0; index < 3; index++) {
    const start = 3 + index * 148;
    data[start] = 10 + index;
    for (let attribute = 0; attribute < 5; attribute++) {
      data[start + 4 + attribute] = 101 + attribute + index * 10;
      data[start + 9 + attribute] = 201 + attribute + index * 10;
    }
  }
  const companions = Object.values(parseTower(data).Companions);
  for (let index = 0; index < companions.length; index++) {
    assert.equal(companions[index].Level, 10 + index);
    assert.deepEqual(Object.values(attributes(companions[index])), [101, 102, 103, 104, 105].map(n => n + index * 10));
    assert.deepEqual(Object.values(companions[index].Bonus), [201, 202, 203, 204, 205].map(n => n + index * 10));
  }
});

test('equipment, single-count weapon gems, potions and pet percentages match sf-tools totals', () => {
  const potions = [{ Type: 1, Size: 25 }, { Type: 4, Size: 15 }, { Type: 6, Size: 25 }];
  const pets = { Water: 23, Light: 11, Earth: 7, Shadow: 17, Fire: 13 };
  for (const classType of [Class.Warrior, Class.Mage, Class.Scout]) {
    assert.deepEqual(getCompanionAttributes(companion(base), gear(), classType, Class.Warrior, potions, pets), {
      Strength: 259, Dexterity: 155, Intelligence: 150, Constitution: 209, Luck: 176,
    });
  }
});

test('nonzero server bonuses are authoritative per attribute and are not applied twice', () => {
  const stats = companion(base, { ...zero, Strength: 500, Constitution: 600 });
  const result = getCompanionAttributes(stats, gear(), Class.Warrior, Class.Warrior,
    [{ Type: 1, Size: 25 }, { Type: 4, Size: 25 }], { Water: 100, Shadow: 100 });
  assert.deepEqual(result, { Strength: 601, Dexterity: 139, Intelligence: 140, Constitution: 704, Luck: 155 });
  assert.deepEqual(stats, companion(base, { ...zero, Strength: 500, Constitution: 600 }));
});

test('legacy three-attribute epics normalize once, and all-stat items add to every attribute', () => {
  const items = parseEquipment([]);
  items.Head = item({ 7: 2, 8: 4, 9: 5, 10: 17, 11: 99, 12: 999 });
  items.Body = item({ 7: 6, 10: 3 });
  assert.deepEqual(getCompanionAttributes(companion(), items, Class.Scout, Class.Warrior, [], null), {
    Strength: 3, Dexterity: 20, Intelligence: 3, Constitution: 20, Luck: 20,
  });
});

test('orange gems affect the companion primary attribute and constitution; black and typed gems stay single-counted', () => {
  const items = parseEquipment([]);
  items.Wpn1 = item({ 0: 1, 1: 16, 16: 19 });
  items.Head = item({ 1: 15, 16: 7 });
  items.Body = item({ 0: 4, 1: 14, 16: 3 });
  items.Hand = item({ 0: 5, 1: 12, 16: 5 });
  for (const [classType, primary] of [[Class.Warrior, 'Strength'], [Class.Mage, 'Intelligence'], [Class.Scout, 'Dexterity']]) {
    assert.deepEqual(getCompanionAttributes(companion(), items, classType, Class.Warrior, [], null), {
      ...zero, Strength: 7, Dexterity: 7, Intelligence: 12, Constitution: 26, Luck: 10, [primary]: (primary === 'Intelligence' ? 12 : 7) + 19,
    });
  }
});

const conversions = [
  [Class.BattleMage, Class.Mage, 3, 1, 3],
  [Class.Assassin, Class.Warrior, 1, 2, 1],
  [Class.DemonHunter, Class.Warrior, 3, 2, 1],
  [Class.Druid, Class.Scout, 3, 3, 2],
  [Class.Bard, Class.Scout, 3, 3, 2],
  [Class.PlagueDoctor, Class.Warrior, 1, 2, 1],
  [Class.PlagueDoctor, Class.Mage, 3, 2, 3],
];

for (const [playerClass, classType, type, from, to] of conversions) {
  test(`class ${playerClass} converts companion ${classType} item attributes ${from} to ${to} without changing captured equipment`, () => {
    for (const epic of [false, true]) {
      const items = parseEquipment([]);
      const slot = type === 1 ? 'Wpn1' : 'Head';
      items[slot] = item({ 0: type, 3: (classType - 1) * 1000 + 50, 7: from + (epic ? 20 : 0), 10: 17 });
      const result = getCompanionAttributes(companion(), items, classType, playerClass, [], null);
      assert.deepEqual(Object.values(result), [1, 2, 3, 4, 5].map(id => id === to || epic && id > 3 ? 17 : 0));
      assert.equal(items[slot].AttributeTypes[0], from + (epic ? 20 : 0));
    }
  });
}

test('conversions respect sale value, equipment class, accessories and weapon restrictions', () => {
  const items = parseEquipment([]);
  items.Head = item({ 0: 3, 3: 1050, 7: 1, 10: 17, 13: 0 });
  items.Body = item({ 0: 4, 3: 50, 7: 1, 10: 19 });
  items.Ring = item({ 0: 9, 3: 1050, 7: 1, 10: 23 });
  items.Wpn1 = item({ 0: 1, 3: 1050, 7: 1, 10: 29 });
  assert.deepEqual(getCompanionAttributes(companion(), items, Class.Mage, Class.BattleMage, [], null), {
    ...zero, Strength: 88,
  });
});

test('only Bert receives second-slot equipment bonuses', () => {
  const items = parseEquipment([]);
  items.Wpn2 = item({ 0: 2, 7: 6, 10: 17, 1: 15, 16: 3 });
  assert.deepEqual(getCompanionAttributes(companion(), items, Class.Warrior, Class.Warrior, [], null), {
    Strength: 20, Dexterity: 20, Intelligence: 20, Constitution: 20, Luck: 20,
  });
  for (const classType of [Class.Mage, Class.Scout]) {
    assert.deepEqual(getCompanionAttributes(companion(), items, classType, Class.Warrior, [], null), zero);
  }
});

test('potion parsing uses type and size columns and selects the strongest matching potion', () => {
  const data = words(10, { 1: 1, 2: 11, 3: 16, 4: 999, 5: 999, 6: 999, 7: 10, 8: 25, 9: 25 });
  const potions = parsePotions(data);
  assert.deepEqual(potions, [{ Type: 1, Size: 25 }, { Type: 6, Size: 25 }, { Type: 1, Size: 10 }]);
  assert.equal(getCompanionAttributes(companion({ ...zero, Strength: 101 }), parseEquipment([]),
    Class.Warrior, Class.Warrior, potions, null).Strength, 127);
  assert.deepEqual(parsePotions(data.slice(0, 9)), []);
});

function character(id = 42) {
  return words(40, {
    1: id, 3: 420, 20: Class.Warrior, 28: 12,
    30: 100, 31: 200, 32: 300, 33: 400, 34: 500,
    35: 11, 36: 22, 37: 33, 38: 44, 39: 55,
  });
}

function request(data) {
  return {
    req: 'Update', params: null, server: 'test.sfgame.net',
    data: Object.fromEntries(Object.entries(data).map(([key, value]) => [key, value.join('/')])),
  };
}

function equipmentWords() {
  const data = words(570);
  for (let index = 0; index < 3; index++) {
    const start = index * 190;
    data.splice(start, 19, ...words(19, { 0: 3, 1: 15, 7: 6, 10: 11, 16: 7 }));
    data.splice(start + 19, 19, ...words(19, { 0: 4, 7: 21, 9: 35, 10: 13, 12: 9 }));
    data.splice(start + 8 * 19, 19, ...words(19, { 0: 1, 1: 15, 7: 1, 10: 17, 16: 19 }));
  }
  return data;
}

test('captured responses build corrected companions and health, retain partial updates, and isolate accounts', async t => {
  const sessions = new SessionManager();
  const injector = core.createEnvironmentInjector([{ provide: SessionManager, useValue: sessions }]);
  t.after(() => injector.destroy());
  const handler = core.runInInjectionContext(injector, () => new SFGameRequestHandler());
  const tower = words(474);
  for (let index = 0; index < 3; index++) {
    const start = 3 + index * 148;
    tower[start] = 10;
    tower.splice(start + 4, 5, ...Object.values(base));
  }
  await handler.digestResponse(request({
    ownplayersavecharacter: character(), ownplayersaveequipment: words(190),
    owntower: tower, companionequipment: equipmentWords(), dungeonprogresslight: [3],
    ownplayersavepotions: words(10, { 1: 11, 2: 14, 3: 16, 7: 25, 8: 15, 9: 25 }),
    ownpets: words(228, { 104: 17, 105: 11, 106: 7, 107: 13, 108: 23, 223: 1 }),
  }));
  const fighter = sessions.fightable();
  for (const result of fighter.Companions) {
    assert.deepEqual(attributes(result), { Strength: 259, Dexterity: 155, Intelligence: 150, Constitution: 209, Luck: 176 });
  }
  assert.deepEqual(fighter.Companions.map(companion => companion.Health), [21384, 7015, 14032]);
  assert.deepEqual(attributes(fighter), { Strength: 111, Dexterity: 222, Intelligence: 333, Constitution: 444, Luck: 555 });
  assert.equal(sessions.current().pets.ShadowArenaFought, true);

  await handler.digestResponse(request({ ownplayersavepotions: words(10) }));
  assert.equal(sessions.fightable().Companions[0].Strength, 207);
  assert.equal(sessions.current().hasEternalPotion, false);

  const updatedEquipment = equipmentWords();
  updatedEquipment[10] = 21;
  await handler.digestResponse(request({ companionequipment: updatedEquipment }));
  assert.equal(sessions.fightable().Companions[0].Strength, 219);
  assert.equal(sessions.fightable().Companions[1].Strength, 207);

  await handler.digestResponse(request({
    ownplayersavecharacter: character(43), ownplayersaveequipment: words(190),
    owntower: tower, companionequipment: equipmentWords(), dungeonprogresslight: [3],
  }));
  assert.equal(sessions.fightable().Companions[0].Strength, 168);
  assert.deepEqual(sessions.current().potions, []);
  assert.equal(sessions.current().pets, null);
  assert.equal(sessions.sessions()['test.sfgame.net-42'].pets.Water, 23);
});
