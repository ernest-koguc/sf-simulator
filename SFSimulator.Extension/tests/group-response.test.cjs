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
const { parseNumberList, parseResponseList } = require('../src/app/sfgame/parsers/ResponseParser.ts');
const { ByteParser } = require('../src/app/sfgame/parsers/ByteParser.ts');
const { parseOwnGroupSave } = require('../src/app/sfgame/parsers/OwnGroupSaveParser.ts');
const { SFGameRequestHandler } = require('../src/app/services/SFGameRequestHandler.ts');
const { SessionManager } = require('../src/app/services/SessionManager.ts');

function groupSave(petMaxLevel = 123) {
  const fields = new Array(379).fill('0');
  fields[374] = '6,302,306,6,299,2,1,5,0,0';
  fields[378] = String(petMaxLevel);
  return fields;
}

test('guild responses preserve comma-separated fields without shifting the pet level', () => {
  const fields = groupSave();
  fields[375] = '';
  fields[376] = 'unused text';
  const response = parseResponseList(` / ${fields.join('/')} /\n`);

  assert.equal(response.length, 379);
  assert.equal(response[374], fields[374]);
  assert.equal(response[375], '');
  assert.deepEqual(parseOwnGroupSave(response), { PetMaxLevel: 123 });
});

test('guild responses still reject invalid or missing pet levels', () => {
  for (const value of ['1,2', '', '1.5', '0x10', '2e3', 'NaN']) {
    assert.throws(() => parseOwnGroupSave(groupSave(value)), {
      name: 'TypeError',
      message: `Invalid response integer at 378: ${JSON.stringify(value)}`,
    });
  }
  assert.throws(() => parseOwnGroupSave(groupSave().slice(0, 378)), RangeError);
  assert.deepEqual(parseOwnGroupSave(groupSave('0')), { PetMaxLevel: 0 });
});

test('integer parsing remains strict when a field is read', () => {
  const reader = new ByteParser([' +12 ', '6,302,306', '-4']);
  assert.equal(reader.read(), 12);
  reader.skip(1);
  assert.equal(reader.read(), -4);
  assert.throws(() => reader.at(1), TypeError);
  assert.throws(() => reader.at(3), RangeError);
  assert.deepEqual(parseNumberList('/ 1 / 0 / -4 /\n'), [1, 0, -4]);
  for (const value of ['1//2', '1/ /2', '1.5', '0x10', '2e3', 'NaN', '1/6,302,306/2']) {
    assert.throws(() => parseNumberList(value), TypeError);
  }
});

test('the response handler accepts nested guild fields and continues processing the response', async t => {
  const sessions = new SessionManager();
  const injector = core.createEnvironmentInjector([{ provide: SessionManager, useValue: sessions }]);
  t.after(() => injector.destroy());
  const handler = core.runInInjectionContext(injector, () => new SFGameRequestHandler());
  const character = new Array(40).fill(0);
  character[1] = 42;
  character[20] = 1;

  const login = await handler.digestResponse({
    req: 'SFAccountCharLogin',
    params: null,
    server: 'test.sfgame.net',
    data: {
      ownplayersavecharacter: character.join('/'),
      owngroupsave: groupSave().join('/'),
      dungeonprogresslight: '3',
      toiletstate: '0/0/2',
    },
  });

  assert.equal(login, true);
  assert.deepEqual(sessions.current().guild, { PetMaxLevel: 123 });
  assert.equal(sessions.current().dungeons.Light[0].Current, 3);
  assert.deepEqual(sessions.current().toiletState, { AmountOfItemsToSacrifice: 2 });
});
