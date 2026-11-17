import { existsSync } from 'node:fs';
import { spawnSync } from 'node:child_process';
import { homedir } from 'node:os';
import { join } from 'node:path';

if (existsSync('.env.local')) {
  process.loadEnvFile('.env.local');
}

const localBinary = join(homedir(), '.local', 'bin', 'sentry');
const sentry = process.env.SENTRY_CLI_PATH ?? (existsSync(localBinary) ? localBinary : 'sentry');
const result = spawnSync(sentry, ['sourcemaps', 'upload', process.argv[2] ?? 'build/browser'], {
  stdio: 'inherit',
  env: {
    ...process.env,
    SENTRY_ORG: 'ernest-koguc',
    SENTRY_PROJECT: 'sf-lab-extension',
  },
});

if (result.error) {
  throw result.error;
}
process.exit(result.status ?? 1);
