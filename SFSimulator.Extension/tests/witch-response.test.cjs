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
const { ScrollType } = require('../src/app/sfgame/SFGameModels.ts');
const { parseWitch } = require('../src/app/sfgame/parsers/WitchParser.ts');
const { SFGameRequestHandler } = require('../src/app/services/SFGameRequestHandler.ts');
const { SessionManager } = require('../src/app/services/SessionManager.ts');
const { WarningService } = require('../src/app/services/WarningService.ts');

function witchshop(scrolls) {
  const data = new Array(Math.max(36, 5 + scrolls.length * 3)).fill(0);
  data[4] = scrolls.length;
  scrolls.forEach((scroll, index) => {
    data[5 + index * 3] = 4_000_000_000;
    data[6 + index * 3] = scroll;
    data[7 + index * 3] = 4_000_000_001;
  });
  return data;
}

function character(id = 42, level = 66) {
  const data = new Array(40).fill(0);
  data[1] = id;
  data[3] = level;
  data[20] = 1;
  return data;
}

function equipment(enchanted = false) {
  const data = new Array(190).fill(0);
  data[2 * 19] = 5;
  data[2 * 19 + 2] = enchanted ? ScrollType.Reaction : 0;
  data[8 * 19] = 1;
  data[8 * 19 + 2] = enchanted ? ScrollType.Crit : 0;
  return data;
}

function request(data, server = 'test.sfgame.net') {
  return {
    req: 'Update', params: null, server,
    data: Object.fromEntries(Object.entries(data).map(([key, value]) => [
      key, Array.isArray(value) ? value.join('/') : value,
    ])),
  };
}

function context(t) {
  const sessions = new SessionManager();
  const injector = core.createEnvironmentInjector([{ provide: SessionManager, useValue: sessions }]);
  t.after(() => injector.destroy());
  return {
    sessions,
    handler: core.runInInjectionContext(injector, () => new SFGameRequestHandler()),
    warnings: core.runInInjectionContext(injector, () => new WarningService()),
  };
}

test('witchshop recognizes all nine Rust enchantment IDs independently of surrounding dates', () => {
  const scrolls = [
    ScrollType.Crit, ScrollType.QuestMushroom, ScrollType.QuestSpeed,
    ScrollType.Reaction, ScrollType.QuestExperience, ScrollType.Beer,
    ScrollType.QuestItems, ScrollType.QuestGold, ScrollType.ArenaGold,
  ];
  assert.deepEqual(parseWitch(witchshop(scrolls)), { Scrolls: scrolls });
});

test('witchshop ignores empty and unknown entries, deduplicates scrolls, and respects the entry count', () => {
  assert.deepEqual(parseWitch(witchshop([0, 1, 999, ScrollType.Crit, ScrollType.Crit])), {
    Scrolls: [ScrollType.Crit],
  });
  const data = witchshop([ScrollType.Reaction, ScrollType.Crit]);
  data[4] = 1;
  assert.deepEqual(parseWitch(data), { Scrolls: [ScrollType.Reaction] });
  assert.deepEqual(parseWitch(witchshop([])), { Scrolls: [] });
});

test('witchshop retains previous unlocks without mutating prior state', () => {
  const previous = parseWitch(witchshop([ScrollType.Crit]));
  assert.deepEqual(parseWitch(witchshop([ScrollType.Reaction, ScrollType.Crit]), previous), {
    Scrolls: [ScrollType.Crit, ScrollType.Reaction],
  });
  assert.deepEqual(parseWitch(witchshop([]), previous), previous);
  assert.deepEqual(previous, { Scrolls: [ScrollType.Crit] });
});

test('witchshop rejects truncated records and invalid unsigned-byte counts', () => {
  assert.throws(() => parseWitch([]), RangeError);
  assert.throws(() => parseWitch(witchshop([ScrollType.Crit]).slice(0, 6)), RangeError);
  for (const count of [-1, 256]) {
    const data = witchshop([]);
    data[4] = count;
    assert.throws(() => parseWitch(data), /Invalid witch enchantment count/);
  }
});

test('witchshop responses activate missing player and companion scroll warnings', async t => {
  const { sessions, handler, warnings } = context(t);
  await handler.digestResponse(request({
    witchshop: witchshop([ScrollType.Crit, ScrollType.Reaction]),
    ownplayersavecharacter: character(), ownplayersaveequipment: equipment(),
    companionequipment: [...equipment(), ...equipment(), ...equipment()],
  }));

  assert.deepEqual(sessions.current().witch, { Scrolls: [ScrollType.Crit, ScrollType.Reaction] });
  assert.deepEqual(warnings.dungeonWarnings(), [
    'Weapon scroll missing!', 'Gloves scroll missing!',
    'Bert weapon scroll missing!', 'Mark weapon scroll missing!', 'Kunigunde weapon scroll missing!',
    'Bert gloves scroll missing!', 'Mark gloves scroll missing!', 'Kunigunde gloves scroll missing!',
  ]);

  await handler.digestResponse(request({
    ownplayersaveequipment: equipment(true),
    companionequipment: [...equipment(true), ...equipment(true), ...equipment(true)],
  }));
  assert.deepEqual(warnings.dungeonWarnings(), []);
  assert.deepEqual(sessions.current().witch.Scrolls, [ScrollType.Crit, ScrollType.Reaction]);
});

test('the obsolete witch response is ignored and cannot overwrite witchshop unlocks', async t => {
  const { sessions, handler, warnings } = context(t);
  await handler.digestResponse(request({
    ownplayersavecharacter: character(), ownplayersaveequipment: equipment(),
    witch: witchshop([ScrollType.Crit, ScrollType.Reaction]),
  }));
  assert.equal(sessions.current().witch, null);
  assert.deepEqual(warnings.dungeonWarnings(), []);

  await handler.digestResponse(request({
    witchshop: witchshop([ScrollType.Crit]), witch: 'obsolete non-numeric data',
  }));
  assert.deepEqual(sessions.current().witch.Scrolls, [ScrollType.Crit]);
  assert.deepEqual(warnings.dungeonWarnings(), ['Weapon scroll missing!']);
});

test('witch unlocks survive partial updates and remain isolated by character and server', async t => {
  const { sessions, handler, warnings } = context(t);
  await handler.digestResponse(request({
    ownplayersavecharacter: character(), ownplayersaveequipment: equipment(),
    witchshop: witchshop([ScrollType.Crit]),
  }));
  await handler.digestResponse(request({ witchshop: witchshop([ScrollType.Reaction]) }));
  await handler.digestResponse(request({ witchshop: witchshop([]) }));
  await handler.digestResponse(request({ wheel: [0, 3] }));
  assert.deepEqual(sessions.current().witch.Scrolls, [ScrollType.Crit, ScrollType.Reaction]);

  await handler.digestResponse(request({
    ownplayersavecharacter: character(43), ownplayersaveequipment: equipment(),
  }));
  assert.equal(sessions.current().witch, null);
  assert.deepEqual(warnings.dungeonWarnings(), []);
  await handler.digestResponse(request({ witchshop: witchshop([ScrollType.Crit]) }, 'other.sfgame.net'));
  assert.equal(sessions.current().witch, null);

  await handler.digestResponse(request({ ownplayersavecharacter: character() }));
  assert.deepEqual(sessions.current().witch.Scrolls, [ScrollType.Crit, ScrollType.Reaction]);
  assert.deepEqual(warnings.dungeonWarnings(), ['Weapon scroll missing!', 'Gloves scroll missing!']);
});

test('scroll warnings require level 66 and an unlocked relevant scroll', async t => {
  const { handler, warnings } = context(t);
  await handler.digestResponse(request({
    ownplayersavecharacter: character(42, 65), ownplayersaveequipment: equipment(),
    witchshop: witchshop([ScrollType.Crit]),
  }));
  assert.deepEqual(warnings.dungeonWarnings(), []);

  await handler.digestResponse(request({ ownplayersavecharacter: character() }));
  assert.deepEqual(warnings.dungeonWarnings(), ['Weapon scroll missing!']);
});
