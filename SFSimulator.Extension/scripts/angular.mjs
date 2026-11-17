import { existsSync } from 'node:fs';
import { spawnSync } from 'node:child_process';

if (existsSync('.env.local')) {
  process.loadEnvFile('.env.local');
}

const args = process.argv.slice(2);
const defines = ['SENTRY_DSN', 'SENTRY_RELEASE'].map(name =>
  `--define=${name}=${JSON.stringify(process.env[name] ?? '')}`);
const result = spawnSync(process.execPath, ['node_modules/@angular/cli/bin/ng.js', ...args, ...defines], {
  stdio: 'inherit',
});

if (result.error) {
  throw result.error;
}
process.exit(result.status ?? 1);
