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
      esModuleInterop: true,
    },
    fileName: filename,
  });
  module._compile(result.outputText, filename);
};

require('@angular/compiler');
const Sentry = require('@sentry/browser');
const { HttpRequest, HttpResponse, HttpErrorResponse } = require('@angular/common/http');
const { firstValueFrom, of, throwError } = require('rxjs');

test('extension monitoring isolates the host SDK and instruments only its API', async t => {
  const originalFetch = global.fetch;
  const hostScope = Sentry.getCurrentScope();
  const originalClient = hostScope.getClient();
  const hostEnvelopes = [];
  const hostClient = new Sentry.BrowserClient({
    dsn: 'https://host@example.com/123',
    transport: () => ({
      send: async envelope => { hostEnvelopes.push(envelope); return {}; },
      flush: async () => true,
    }),
    stackParser: Sentry.defaultStackParser,
    integrations: [],
  });
  hostScope.setClient(hostClient);
  const envelopes = [];
  global.fetch = async (url, options) => {
    envelopes.push({ url: String(url), body: String(options.body) });
    return new Response('{}', { status: 200 });
  };
  t.after(() => {
    global.fetch = originalFetch;
    hostScope.setClient(originalClient);
    delete global.SENTRY_DSN;
  });

  const monitoring = require('../src/app/monitoring/sentry.ts');
  const request = new HttpRequest('GET', 'https://sfsimulator.xyz/api/dungeon/test');

  monitoring.initializeSentry();
  await firstValueFrom(monitoring.sentryHttpInterceptor(request, outgoing => {
    assert.equal(outgoing, request);
    return of(new HttpResponse({ status: 200 }));
  }));
  assert.equal(envelopes.length, 0);

  global.SENTRY_DSN = 'https://extension@example.com/456';
  monitoring.initializeSentry();
  assert.equal(Sentry.getClient(), hostClient);

  await firstValueFrom(monitoring.sentryHttpInterceptor(request, outgoing => {
    assert.match(outgoing.headers.get('sentry-trace'), /^[a-f0-9]{32}-[a-f0-9]{16}-[01]$/);
    assert.match(outgoing.headers.get('baggage'), /sentry-public_key=extension/);
    assert.equal(Sentry.getClient(), hostClient);
    return of(new HttpResponse({ status: 200 }));
  }));

  const traceHeaders = [];
  const repeatedRequest = monitoring.sentryHttpInterceptor(request, outgoing => {
    traceHeaders.push(outgoing.headers.get('sentry-trace'));
    return of(new HttpResponse({ status: 200 }));
  });
  await firstValueFrom(repeatedRequest);
  await firstValueFrom(repeatedRequest);
  assert.notEqual(traceHeaders[0].split('-')[0], traceHeaders[1].split('-')[0]);

  const originalRandom = Math.random;
  try {
    for (const [random, sampled] of [[0.01, '1'], [0.99, '0']]) {
      Math.random = () => random;
      await firstValueFrom(monitoring.sentryHttpInterceptor(request, outgoing => {
        assert.equal(outgoing.headers.get('sentry-trace').split('-')[2], sampled);
        return of(new HttpResponse({ status: 200 }));
      }));
    }
  } finally {
    Math.random = originalRandom;
  }

  const gameRequest = new HttpRequest('GET', 'https://sfgame.net/req.php?params=private-game-data');
  await firstValueFrom(monitoring.sentryHttpInterceptor(gameRequest, outgoing => {
    assert.equal(outgoing, gameRequest);
    assert.equal(outgoing.headers.has('sentry-trace'), false);
    return of(new HttpResponse({ status: 200 }));
  }));

  const failure = new HttpErrorResponse({
    status: 500,
    url: request.url,
    error: { token: 'private-game-data' },
  });
  await assert.rejects(firstValueFrom(monitoring.sentryHttpInterceptor(request, () => throwError(() => failure))));
  monitoring.captureExtensionError(new Error('Extension test failure'), 'game-response');

  for (let attempt = 0; attempt < 20 && !envelopes.some(item => item.body.includes('Extension test failure')); attempt++) {
    await new Promise(resolve => setTimeout(resolve, 10));
  }

  assert.ok(envelopes.some(item => item.body.includes('Extension test failure')));
  assert.ok(envelopes.some(item => item.body.includes('API request failed')));
  assert.ok(envelopes.every(item => item.url.includes('/456/envelope/')));
  assert.ok(envelopes.every(item => !item.body.includes('private-game-data')));
  assert.equal(Sentry.getClient(), hostClient);
  assert.equal(hostEnvelopes.length, 0);
});
