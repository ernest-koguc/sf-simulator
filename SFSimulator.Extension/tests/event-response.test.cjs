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
const { EventType } = require('../src/app/sfgame/SFGameModels.ts');
const { parseEvents } = require('../src/app/sfgame/parsers/EventParser.ts');
const { parseResponseList } = require('../src/app/sfgame/parsers/ResponseParser.ts');
const { SFGameRequestHandler } = require('../src/app/services/SFGameRequestHandler.ts');
const { SessionManager } = require('../src/app/services/SessionManager.ts');

const capturedEvents = '31/66410/1795391940/1/1795391940/20/1/0/1/0/0/hv';
const capturedActiveEvents = [
  EventType.GloriousGoldGalore, EventType.AssemblyOfAwesomeAnimals,
  EventType.DaysOfDoomedSouls, EventType.WitchesDance,
  EventType.ForgeFrenzyFestival, EventType.EpicShoppingSpreeExtravaganza,
  EventType.HolidaySale, EventType.Hellevator,
];

test('the captured event response combines numeric flags with the Hellevator code at index eleven', () => {
  assert.deepEqual(parseEvents(parseResponseList(capturedEvents)), capturedActiveEvents);
});

test('event flags retain their offsets for numeric and string response fields', () => {
  const data = [123, (1 << 2) | (1 << 14), 0, 0, 0, 0, 0, 0];
  const expected = [EventType.TidyToiletTime, EventType.LuckyDay];
  assert.deepEqual(parseEvents(data), expected);
  assert.deepEqual(parseEvents(data.map(String)), expected);
  assert.deepEqual(parseEvents(new Array(8).fill(0)), []);
});

test('Hellevator does not require numeric flags and unknown event codes are ignored', () => {
  const data = new Array(12).fill(0);
  data[11] = EventType.Hellevator;
  assert.deepEqual(parseEvents(data), [EventType.Hellevator]);
  for (const code of ['unknown', 'LuckyDay', 'Hellevator', '']) {
    data[11] = code;
    assert.deepEqual(parseEvents(data), []);
  }
});

test('short event responses are ignored and invalid numeric flags still fail', () => {
  for (const data of [[], ['hv'], [0, 1 << 14], new Array(7).fill(0)]) {
    assert.equal(parseEvents(data), null);
  }
  for (const flags of ['hv', '', '1.5', '0x10', 'NaN']) {
    const data = parseResponseList(capturedEvents);
    data[1] = flags;
    assert.throws(() => parseEvents(data), TypeError);
  }
});

test('the handler accepts captured mixed event fields and preserves or replaces session events', async t => {
  const sessions = new SessionManager();
  const injector = core.createEnvironmentInjector([{ provide: SessionManager, useValue: sessions }]);
  t.after(() => injector.destroy());
  const handler = core.runInInjectionContext(injector, () => new SFGameRequestHandler());
  const character = new Array(40).fill(0);
  character[1] = 42;
  character[20] = 1;
  const request = data => ({ req: 'Update', params: null, server: 'test.sfgame.net', data });

  await handler.digestResponse({
    ...request({
      events: capturedEvents,
      ownplayersavecharacter: character.join('/'),
      toiletstate: '100/0/1',
    }),
    req: 'SFAccountCharLogin',
  });
  assert.deepEqual(sessions.current().activeEvents, capturedActiveEvents);
  assert.deepEqual(sessions.current().toiletState, { AmountOfItemsToSacrifice: 1 });

  await handler.digestResponse(request({ wheel: '0/3' }));
  await handler.digestResponse(request({ events: '0/1' }));
  await handler.digestResponse(request({ events: '' }));
  assert.deepEqual(sessions.current().activeEvents, capturedActiveEvents);

  await handler.digestResponse(request({ events: '0/16384/0/0/0/0/0/0' }));
  assert.deepEqual(sessions.current().activeEvents, [EventType.LuckyDay]);
  await handler.digestResponse(request({ events: '0/0/0/0/0/0/0/0' }));
  assert.deepEqual(sessions.current().activeEvents, []);
});
