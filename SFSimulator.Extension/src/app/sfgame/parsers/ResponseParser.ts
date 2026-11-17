export function parseResponseList(value: string): string[] {
  const trimmed = value.replace(/^[\s/]+|[\s/]+$/g, '');
  return trimmed === '' ? [] : trimmed.split('/');
}

export function parseResponseInteger(value: string, index: number): number {
  const token = value.trim();
  if (!/^[+-]?\d+$/.test(token)) {
    throw new TypeError(`Invalid response integer at ${index}: ${JSON.stringify(value)}`);
  }
  const result = Number(token);
  if (!Number.isFinite(result)) {
    throw new RangeError(`Response integer is out of range at ${index}`);
  }
  return result;
}

export function parseNumberList(value: string): number[] {
  return parseResponseList(value).map(parseResponseInteger);
}

/** Server timestamps encode the server's calendar; use UTC calendar accessors. */
export function serverDayOfYear(serverTime: number): number {
  const date = new Date(serverTime);
  const start = Date.UTC(date.getUTCFullYear(), 0, 0);
  const today = Date.UTC(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate());
  return (today - start) / 86_400_000;
}
