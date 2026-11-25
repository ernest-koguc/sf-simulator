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
const { SFGameRequestHandler } = require('../src/app/services/SFGameRequestHandler.ts');
const { SessionManager } = require('../src/app/services/SessionManager.ts');

test('dungeon fight responses update only the progress entries they contain', async t => {
  const sessions = new SessionManager();
  const injector = core.createEnvironmentInjector([{ provide: SessionManager, useValue: sessions }]);
  t.after(() => injector.destroy());
  const handler = core.runInInjectionContext(injector, () => new SFGameRequestHandler());
  const digest = (req, data) => handler.digestResponse({ req, params: null, server: 'test.sfgame.net', data });
  const character = new Array(40).fill(0);
  character[1] = 42;
  character[20] = 1;

  await digest('SFAccountCharLogin', {
    ownplayersavecharacter: character.join('/'),
    dungeonprogresslight: '10/0/7',
    dungeonprogressshadow: '4/3/8',
    dungeonprogressclass: '2/-1/-1/-1/-1',
  });
  const initial = sessions.current().dungeons;

  await digest('FightDungeon', {
    ownplayersavecharacter: character.join('/'),
    winnerid: '42',
    dungeonprogressclass: '3/-1/-1/-1/-1',
  });
  assert.deepEqual(sessions.current().dungeons, {
    Light: [10, 0, 7], Shadow: [4, 3, 8], Class: [3, -1, -1, -1, -1],
  });

  await digest('Update', { dungeonprogresslight: '', dungeonprogressshadow: '0/-1' });
  assert.deepEqual(sessions.current().dungeons, {
    Light: [10, 0, 7], Shadow: [0, -1, 8], Class: [3, -1, -1, -1, -1],
  });
  assert.deepEqual(initial, {
    Light: [10, 0, 7], Shadow: [4, 3, 8], Class: [2, -1, -1, -1, -1],
  });
});
