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

const parser = name => require('../src/app/sfgame/parsers/' + name + '.ts');
const { ByteParser, chunks } = parser('ByteParser');
const { parseNumberList, serverDayOfYear } = parser('ResponseParser');
const { parseOwnPlayerSave, parseCharacterClass } = parser('OwnPlayerSaveParser');
const status = parser('CharacterStatusParser');
const { parseTower } = parser('TowerParser');
const { parsePets } = parser('PetParser');
const { parseWitch } = parser('WitchParser');
const { parseOwnGroupSave } = parser('OwnGroupSaveParser');
const { parsePortalProgress } = parser('PortalProgressParser');
const { parseDailyTaskList, parseDailyTaskRewards } = parser('DailyTasksParser');
const { parseEvents } = parser('EventParser');
const { parseToiletState } = parser('ToiletParser');
const { parseDungeons } = parser('DungeonParser');
const { ItemModel, parseEquipment, parseCompanionEquipment, countItemRewards } = parser('EquipmentParser');
const { parseExpeditions, parseExpeditionState, parseExpeditionCrossroad } = parser('ExpeditionParser');
const { Class, EventType, ScrollType, RuneType } = require('../src/app/sfgame/SFGameModels.ts');
const { Fightable } = require('../src/app/sfgame/Fightable.ts');

function words(length, values = {}) {
  return Object.assign(new Array(length).fill(0), values);
}

function character(id = 42) {
  return words(40, {
    1: id, 3: 0x12340000 + 420, 20: 3, 23: 21_000, 26: 7, 28: 12,
    30: 100, 31: 200, 32: 300, 33: 400, 34: 500,
    35: 11, 36: 22, 37: 33, 38: 44, 39: 55,
  });
}

function item(values = {}) {
  return words(19, { 0: 1, 5: 123, 6: 456, 17: 600, ...values });
}

test('integer lists preserve zero slots and reject interior gaps and non-integers', () => {
  assert.deepEqual(parseNumberList('/ 1 / 0 / -4 /\n'), [1, 0, -4]);
  assert.deepEqual(parseNumberList(''), []);
  assert.deepEqual(parseNumberList(' //\n'), []);
  for (const bad of ['1//2', '1/ /2', '1.5', '0x10', '2e3', 'NaN']) {
    assert.throws(() => parseNumberList(bad), TypeError);
  }
});

test('word reads fail on truncation instead of silently fabricating zeros', () => {
  const reader = new ByteParser([0, -3, 4_000_000_000]);
  assert.equal(reader.read(), 0);
  assert.equal(reader.unsignedAt(1), 0);
  assert.equal(reader.at(2), 4_000_000_000);
  reader.skip(2);
  assert.throws(() => reader.read(), RangeError);
  assert.throws(() => reader.at(-1), RangeError);
  assert.throws(() => reader.skip(-1), RangeError);
  assert.deepEqual([...chunks([1, 2, 3, 4, 5], 2)], [[1, 2], [3, 4]]);
  assert.throws(() => [...chunks([], 0)], RangeError);
});

test('modern character fields use fixed Rust offsets and expose only used values', () => {
  assert.deepEqual(parseOwnPlayerSave(character()), {
    ID: 42, Level: 420, Class: Class.Scout, Armor: 21_000,
    GuildPortal: 7, PortalHealthBonus: 12,
    Strength: 111, Dexterity: 222, Intelligence: 333, Constitution: 444, Luck: 555,
  });
  assert.equal(parseCharacterClass(words(21, { 20: 13 })), Class.BloodWeaver);
  assert.throws(() => parseCharacterClass(words(21)), /Unknown character class/);
  assert.throws(() => parseOwnPlayerSave(character().slice(0, 39)), RangeError);
});

test('status, potions, wheel, dice, arena and hydra use their separate response layouts', () => {
  assert.deepEqual(status.parseCharacterStatus(words(8, { 5: 11, 6: 3600, 7: 3 })), {
    MaxBeers: 11, ThirstLeft: 3600, UsedBeers: 3,
  });
  assert.equal(status.parseHasEternalPotion(words(10, { 3: 16 })), true);
  assert.equal(status.parseHasEternalPotion(words(9, { 3: 16 })), false);
  assert.equal(status.parseHasEternalPotion(words(10, { 7: 16 })), false);
  assert.equal(status.parseWheelSpins([999, 14, 0]), 14);
  assert.equal(status.parseDiceGamesRemaining([999, 6]), 6);
  assert.equal(status.parseArenaFights([999, 10]), 10);
  assert.equal(status.parseCanAttackHydra([999, 0, 888, 1]), true);
  assert.equal(status.parseCanAttackHydra([999, 0, 888, 0]), false);
  assert.equal(status.parseDiceGamesRemaining([0, 256]), 0);
});

test('tower uses three 148-word companion records, base attributes, server bonuses, and underworld offsets', () => {
  const data = words(474, { 448: 1, 454: 15, 473: 1200 });
  for (let i = 0; i < 3; i++) {
    const start = 3 + 148 * i;
    data[start] = 100 + i;
    for (let j = 0; j < 5; j++) {
      data[start + 4 + j] = 10 * (i + 1) + j;
      data[start + 9 + j] = 999;
    }
  }
  const tower = parseTower(data);
  assert.deepEqual(tower.Underworld, { Gladiator: 15, TimeMachineThirst: 1200 });
  assert.deepEqual(tower.Companions.Bert, {
    Level: 100, Strength: 10, Dexterity: 11, Intelligence: 12, Constitution: 13, Luck: 14,
    Bonus: { Strength: 999, Dexterity: 999, Intelligence: 999, Constitution: 999, Luck: 999 },
  });
  assert.equal(tower.Companions.Mark.Strength, 20);
  assert.equal(tower.Companions.Kunigunde.Luck, 34);
  data[448] = 0;
  assert.deepEqual(parseTower(data).Underworld, { Gladiator: 0, TimeMachineThirst: 0 });
});

test('pet attribute bonuses, arena flags and guild pet cap are parsed', () => {
  assert.deepEqual(parsePets(words(228, { 104: 11, 105: 22, 106: 33, 107: 44, 108: 55, 223: 1, 225: 1, 226: 2, 227: 1 })), {
    Shadow: 11, Light: 22, Earth: 33, Fire: 44, Water: 55,
    ShadowArenaFought: true, LightArenaFought: false, EarthArenaFought: true,
    FireArenaFought: false, WaterArenaFought: true,
  });
  assert.deepEqual(parseOwnGroupSave(words(379, { 378: 100 })), { PetMaxLevel: 100 });
});

test('witch scrolls are unlocked by listed IDs, without date comparisons', () => {
  const data = words(18, { 4: 4, 6: 11, 9: 51, 12: 41, 15: 999 });
  assert.deepEqual(parseWitch(data), { Scrolls: [ScrollType.Crit, ScrollType.Reaction, ScrollType.QuestSpeed] });
  assert.deepEqual(parseWitch(words(5)), { Scrolls: [] });
});

test('events use flags at index one and ignore truncated responses', () => {
  assert.deepEqual(parseEvents([123, (1 << 2) | (1 << 14), 0, 0, 0, 0, 0, 0]), [
    EventType.TidyToiletTime, EventType.LuckyDay,
  ]);
  assert.deepEqual(parseEvents(words(8, { 0: 0xffff })), []);
  assert.equal(parseEvents([1, 2]), null);
});

test('daily tasks use complete four-word records and rewards skip variable payload lengths', () => {
  assert.deepEqual(parseDailyTaskList([2, 19, 2, 3, 4, 20, 5, 5, 8, 99]), [
    { Completed: false, Points: 4 }, { Completed: true, Points: 8 },
  ]);
  assert.deepEqual(parseDailyTaskList([0]), []);
  assert.deepEqual(parseDailyTaskRewards([
    2, 10, 2, 3, 99, 4, 100,
    0, 20, 0,
    1, 30, 1, 24, 1000,
  ]), [
    { Claimed: true, PointsRequired: 10 },
    { Claimed: false, PointsRequired: 20 },
    { Claimed: true, PointsRequired: 30 },
  ]);
  assert.deepEqual(parseDailyTaskRewards([]), new Array(3).fill({ Claimed: false, PointsRequired: 0 }));
  assert.deepEqual(parseToiletState([0, 0, 2]), { AmountOfItemsToSacrifice: 2 });
  assert.equal(parseToiletState([0, 0]), null);
});

test('portal compares server day ordinals by inequality across leap days and new years', () => {
  assert.equal(serverDayOfYear(Date.UTC(2024, 1, 29, 23, 59)), 60);
  assert.equal(serverDayOfYear(Date.UTC(2024, 2, 1)), 61);
  assert.deepEqual(parsePortalProgress([49, 12, 60], Date.UTC(2024, 1, 29)), { Finished: 49, CanAttack: false });
  assert.equal(parsePortalProgress([0, 0, 366], Date.UTC(2025, 0, 1)).CanAttack, true);
  assert.equal(parsePortalProgress([0, 0, 61], Date.UTC(2024, 1, 29)).CanAttack, true);
});

test('dungeon IDs stay at their raw protocol indices after gaps and special dungeons', () => {
  const light = words(37, { 12: 1, 13: 2, 15: 3, 18: 4, 30: 5, 31: 6, 32: 7, 36: 8 });
  const data = parseDungeons({ light });
  const find = id => data.Light.find(d => d.Position === id)?.Current;
  assert.equal(find(15), 1);
  assert.equal(find(19), 2);
  assert.equal(find(17), 3);
  assert.equal(find(24), 4);
  assert.equal(find(29), 7);
  assert.equal(find(33), 8);
  assert.equal(data.Sandstorm, 6);
  assert.equal(data.Twister, -1);
  assert.equal(data.Light.length, 33);
  assert.equal('SoloPortal' in data, false);
});

test('partial dungeon responses preserve the other world and normalize Rust terminal states', () => {
  const first = parseDungeons({ light: words(16, { 0: 3, 14: 100 }), shadow: words(18, { 0: 4, 17: 21 }) });
  const second = parseDungeons({ shadow: [5, -1, -5] }, first);
  assert.equal(second.Light[0].Current, 3);
  assert.equal(second.Shadow[0].Current, 5);
  assert.equal(second.Shadow[1].Current, -1);
  assert.equal(second.Shadow[2].Current, 0);
  assert.equal(second.Tower, -2);
  assert.equal(second.LoopOfIdols, -2);
  assert.equal(first.Shadow[0].Current, 4);
  assert.equal(parseDungeons({ shadow: words(15, { 14: 1000 }) }).Twister, -2);
  assert.equal(parseDungeons({ light: [10] }).Light[0].Current, -2);
});

test('items mask type flags, restrict armor/damage to their types, and find runes in any slot', () => {
  const weapon = new ItemModel(item({ 0: 0x101, 2: 11, 7: 40, 10: 17, 9: 0 }));
  assert.deepEqual({ ...weapon }, {
    Class: 1, AttributeTypes: [40, 0, 0], Attributes: [17, 0, 0],
    HasGem: false, GemType: 0, GemValue: 0, Gold: 0,
    Type: 1, Armor: 0, DamageMin: 123, DamageMax: 456, HasEnchantment: true,
    ItemQuality: 600, RuneType: RuneType.FireDamage, RuneValue: 17,
  });
  const armor = new ItemModel(item({ 0: 6, 2: 999, 8: 35, 11: 15 }));
  assert.equal(armor.Armor, 123);
  assert.equal(armor.DamageMin, 0);
  assert.equal(armor.DamageMax, 0);
  assert.equal(armor.HasEnchantment, false);
  assert.equal(armor.RuneType, RuneType.HealthBonus);
  assert.equal(new ItemModel(item({ 7: 40, 10: 256 })).RuneType, RuneType.None);
  assert.equal(new ItemModel(item({ 0: 2 })).Armor, 0);
  assert.deepEqual(ItemModel.empty(), new ItemModel(item({ 0: 0 })));
});

test('equipment allows empty slots, rejects broken strides, and reads three companion sets', () => {
  assert.equal(parseEquipment([]).Wpn2, null);
  assert.throws(() => parseEquipment([0]), /19-value/);
  const data = words(570);
  data.splice(8 * 19, 19, ...item({ 5: 111 }));
  data.splice(18 * 19, 19, ...item({ 5: 222 }));
  data.splice(28 * 19, 19, ...item({ 5: 333 }));
  const equipment = parseCompanionEquipment(data);
  assert.equal(equipment.Bert.Wpn1.DamageMin, 111);
  assert.equal(equipment.Mark.Wpn1.DamageMin, 222);
  assert.equal(equipment.Kunigunde.Wpn1.DamageMin, 333);
  assert.equal(countItemRewards([...item(), ...words(19), ...item(), 999]), 2);
});

test('expeditions consume complete records and expose only the used state/choice fields', () => {
  assert.deepEqual(parseExpeditions([22, 1, 2, 3, 44, 55, 600, 0, 143, 4, 5, 6, 77, 88, 1200, 0, 999]), [
    { MainTask: 22, SideTasks: [1, 2, 3], Time: 600 },
    { MainTask: 143, SideTasks: [4, 5, 6], Time: 1200 },
  ]);
  assert.deepEqual(parseExpeditionState(words(14, { 0: 7, 2: 1, 13: -12 })), {
    Stage: 7, CurrentStatus: 1, CurrentHeroism: -12,
  });
  assert.deepEqual(parseExpeditionCrossroad([22, 99, 143, 55, 999, 0, 11]), {
    Left: 22, Mid: 143, Right: 0,
  });
});

test('combat uses character portal bonus, companion levels/gear armor and excludes shield offhands', () => {
  const equipment = parseEquipment(words(190));
  equipment.Wpn1 = new ItemModel(item());
  equipment.Wpn2 = new ItemModel(item({ 0: 2 }));
  equipment.Body = new ItemModel(item({ 0: 3, 5: 321 }));
  const companion = {
    Level: 10, Strength: 1, Dexterity: 2, Intelligence: 3, Constitution: 100, Luck: 4,
    Bonus: { Strength: 0, Dexterity: 0, Intelligence: 0, Constitution: 0, Luck: 0 },
  };
  const tower = {
    Underworld: { Gladiator: 10, TimeMachineThirst: 0 },
    Companions: { Bert: companion, Mark: companion, Kunigunde: companion },
  };
  const fighter = new Fightable(parseOwnPlayerSave(character()), equipment,
    { Bert: equipment, Mark: equipment, Kunigunde: equipment }, tower, true, [], null);
  assert.equal(fighter.SecondWeapon, null);
  assert.equal(fighter.GuildPortal, 7);
  assert.equal(fighter.Companions[0].Armor, 321);
  assert.equal(fighter.Companions[0].Health, 9393);
  assert.equal(fighter.CritMultiplier, 3.1);
});

const core = require('@angular/core');
const { SessionManager } = require('../src/app/services/SessionManager.ts');
const { ExpeditionService } = require('../src/app/services/ExpeditionService.ts');
const { EquipmentService } = require('../src/app/services/EquipmentService.ts');
const { SFGameRequestHandler } = require('../src/app/services/SFGameRequestHandler.ts');
const { db, expeditionChestCount, expeditionItemCount } = require('../src/app/db.ts');

test('history statistics read both previous reward arrays and new counts, including zero counts', () => {
  const legacy = { ChestRewards: [{}, {}], HalfTimeItems: [{}], FinalItems: [{}, {}, {}] };
  assert.equal(expeditionChestCount(legacy), 2);
  assert.equal(expeditionItemCount(legacy), 4);
  assert.equal(expeditionChestCount({ ...legacy, ChestCount: 0 }), 0);
  assert.equal(expeditionItemCount({ ...legacy, ItemCount: 0 }), 0);
  assert.equal(expeditionChestCount({ ChestCount: 5 }), 5);
  assert.equal(expeditionItemCount({ ItemCount: 7 }), 7);
});

function handlerContext() {
  const sessions = new SessionManager();
  const savedEquipment = [];
  const injector = core.createEnvironmentInjector([
    { provide: SessionManager, useValue: sessions },
    { provide: ExpeditionService, useFactory: () => new ExpeditionService() },
    { provide: EquipmentService, useValue: { saveEquipment: async (...args) => { savedEquipment.push(args); } } },
  ]);
  return {
    sessions, savedEquipment, injector,
    handler: core.runInInjectionContext(injector, () => new SFGameRequestHandler()),
  };
}

function request(data, req = 'Update', params = null, server = 'test.sfgame.net') {
  return {
    req, params, server,
    data: Object.fromEntries(Object.entries(data).map(([key, value]) => [key, Array.isArray(value) ? value.join('/') : value])),
  };
}

test('handler routes modern split responses, processes parameterless login, and retains partial updates', async t => {
  const context = handlerContext();
  t.after(() => context.injector.destroy());
  await context.handler.digestResponse(request({
    ownplayersavecharacter: character(), ownplayername: 'Player', timestamp: '1760000000',
    events: [99, (1 << 14), 0, 0, 0, 0, 0, 0],
    characterstatus: words(8, { 5: 11, 6: 3600, 7: 3 }),
    dice: [0, 2], wheel: [0, 7], arena: [0, 9], charactergroup: [0, 0, 0, 1],
    ownplayersavepotions: words(10, { 1: 16 }),
    expeditions: [22, 1, 2, 3, 0, 0, 600, 0],
    dungeonprogresslight: [3],
  }, 'SFAccountCharLogin'));
  const current = context.sessions.current();
  assert.equal(context.sessions.currentId(), 'test.sfgame.net-42');
  assert.equal(current.playerName, 'Player');
  assert.equal(current.ownPlayerSave.Constitution, 444);
  assert.deepEqual(current.activeEvents, [EventType.LuckyDay]);
  assert.equal(current.diceGamesRemaining, 2);
  assert.equal(current.wheelSpinsToday, 7);
  assert.equal(current.arenaFightsToday, 9);
  assert.equal(current.canAttackHydra, true);
  assert.equal(current.hasEternalPotion, true);
  assert.equal(current.expedition.length, 1);
  await context.handler.digestResponse(request({ dungeonprogressshadow: [4], dice: [0, 0], events: [1] }));
  assert.equal(context.sessions.current().dungeons.Light[0].Current, 3);
  assert.equal(context.sessions.current().dungeons.Shadow[0].Current, 4);
  assert.equal(context.sessions.current().diceGamesRemaining, 0);
  assert.deepEqual(context.sessions.current().activeEvents, [EventType.LuckyDay]);
});

test('handler keeps account state isolated and reads looked-up class from otherplayersavecharacter', async t => {
  const context = handlerContext();
  t.after(() => context.injector.destroy());
  await context.handler.digestResponse(request({ ownplayersavecharacter: character(), wheel: [0, 12] }));
  await context.handler.digestResponse(request({ ownplayersavecharacter: character(43), wheel: [0, 2] }));
  assert.equal(context.sessions.current().wheelSpinsToday, 2);
  assert.equal(context.sessions.sessions()['test.sfgame.net-42'].wheelSpinsToday, 12);
  await context.handler.digestResponse(request({ wheel: [0, 19] }, 'Update', null, 'other.sfgame.net'));
  assert.equal(context.sessions.current().wheelSpinsToday, 2);
  await context.handler.digestResponse(request({
    otherplayersaveequipment: words(190), otherplayersavecharacter: words(21, { 20: 10 }),
    otherplayername: 'Other', otherplayer: words(21, { 20: 1 }),
  }, 'PlayerLookAt', '1'));
  assert.equal(context.savedEquipment[0][1], Class.Necromancer);
  assert.equal(context.savedEquipment[0][2], 'Other');
});

test('handler advances per-server calendar time and ignores retired combined save/event keys', async t => {
  const context = handlerContext();
  t.after(() => context.injector.destroy());
  const realNow = Date.now;
  let now = Date.UTC(2025, 2, 1, 12);
  Date.now = () => now;
  t.after(() => { Date.now = realNow; });
  const nearMidnight = Date.UTC(2025, 0, 1, 23, 59) / 1000;
  await context.handler.digestResponse(request({
    ownplayersavecharacter: character(), timestamp: String(nearMidnight), portalprogress: [0, 0, 1],
    events: words(8), tavernspecialsub: String(1 << 14), ownplayersave: [999],
  }));
  assert.equal(context.sessions.current().portalProgress.CanAttack, false);
  assert.deepEqual(context.sessions.current().activeEvents, []);
  now += 120_000;
  await context.handler.digestResponse(request({ portalprogress: [0, 0, 1] }));
  assert.equal(context.sessions.current().portalProgress.CanAttack, true);
});

test('expedition tracking stores counts with modern reward key and no unused reward payload', async t => {
  const context = handlerContext();
  const saved = [];
  const originalPut = db.ExpeditionHistory.put;
  db.ExpeditionHistory.put = async record => { saved.push(record); return 1; };
  t.after(() => { db.ExpeditionHistory.put = originalPut; context.injector.destroy(); });
  await context.handler.digestResponse(request({
    ownplayersavecharacter: character(), expeditions: [22, 1, 2, 3, 0, 0, 600, 0],
  }));
  await context.handler.digestResponse(request({
    expeditionstate: words(14, { 0: 1, 2: 1 }), expeditioncrossroad: [22, 5, 41, 6, 132, 7],
  }, 'ExpeditionStart', '1'));
  await context.handler.digestResponse(request({
    expeditionstate: words(14, { 0: 1, 2: 1 }), expeditionrewardresources: [2, 3, 200],
  }, 'ExpeditionProceed', '2'));
  assert.equal(context.sessions.current().expeditionProgress.stages[0].chosen, 'mid');
  await context.handler.digestResponse(request({
    expeditionstate: words(14, { 0: 10, 2: 4, 13: 99 }),
    expeditionrewardresources: [2, 3, 200], expeditionreward: [...item(), ...item()],
  }, 'ExpeditionProceed', '1'));
  assert.equal(saved.length, 1);
  assert.equal(saved[0].ChestCount, 1);
  assert.equal(saved[0].ItemCount, 2);
  assert.equal(saved[0].Heroism, 99);
  assert.equal(saved[0].Thirst, 10);
  assert.equal('FinalRewards' in saved[0], false);
  assert.equal('Stages' in saved[0], false);
  assert.equal(context.sessions.current().expeditionProgress, null);
});
