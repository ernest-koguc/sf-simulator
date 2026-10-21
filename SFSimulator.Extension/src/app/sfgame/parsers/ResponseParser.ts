/** Mirrors ResponseVal::into_list: trim padding, never remove interior slots. */
export function parseNumberList(value: string): number[] {
  const trimmed = value.replace(/^[\s/]+|[\s/]+$/g, '');
  if (trimmed === '') {
    return [];
  }
  return trimmed.split('/').map((part, index) => {
    const token = part.trim();
    if (!/^[+-]?\d+$/.test(token)) {
      throw new TypeError(`Invalid response integer at ${index}: ${JSON.stringify(part)}`);
    }
    const result = Number(token);
    if (!Number.isFinite(result)) {
      throw new RangeError(`Response integer is out of range at ${index}`);
    }
    return result;
  });
}

/** Server timestamps encode the server's calendar; use UTC calendar accessors. */
export function serverDayOfYear(serverTime: number): number {
  const date = new Date(serverTime);
  const start = Date.UTC(date.getUTCFullYear(), 0, 0);
  const today = Date.UTC(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate());
  return (today - start) / 86_400_000;
}
