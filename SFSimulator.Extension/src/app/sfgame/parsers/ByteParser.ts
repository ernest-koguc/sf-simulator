/** Reads the server's integer words. Offsets always refer to whole words. */
export class ByteParser {
  private position = 0;

  constructor(private readonly values: readonly number[]) {}

  get remaining(): number {
    return this.values.length - this.position;
  }

  at(index: number): number {
    const value = this.values[index];
    if (!Number.isInteger(index) || index < 0 || value === undefined) {
      throw new RangeError(`Missing response value at ${index} (length ${this.values.length})`);
    }
    if (!Number.isFinite(value) || !Number.isInteger(value)) {
      throw new TypeError(`Invalid response integer at ${index}: ${value}`);
    }
    return value;
  }

  read(): number {
    const value = this.at(this.position);
    this.position++;
    return value;
  }

  unsignedAt(index: number, max = 0xffffffff, fallback = 0): number {
    return unsigned(this.at(index), max, fallback);
  }

  skip(count: number): this {
    if (!Number.isInteger(count) || count < 0 || count > this.remaining) {
      throw new RangeError(`Cannot skip ${count} values (${this.remaining} remaining)`);
    }
    this.position += count;
    return this;
  }
}

/** Rust's soft_into for the unsigned integers used by these parsers. */
export function unsigned(value: number, max = 0xffffffff, fallback = 0): number {
  return Number.isInteger(value) && value >= 0 && value <= max ? value : fallback;
}

/** Like Rust's chunks_exact: ignore an incomplete record at the end. */
export function* chunks(values: readonly number[], size: number): Generator<number[]> {
  if (!Number.isInteger(size) || size <= 0) {
    throw new RangeError('Record size must be a positive integer');
  }
  for (let offset = 0; offset + size <= values.length; offset += size) {
    yield values.slice(offset, offset + size);
  }
}
