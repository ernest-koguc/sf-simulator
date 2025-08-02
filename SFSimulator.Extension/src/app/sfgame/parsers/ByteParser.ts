export class ByteParser {
  values: number[];
  ptr: number;
  bytes: any[];
  constructor(values: number[] | null) {
    this.values = values || [];
    this.ptr = 0;
    this.bytes = [];
  }

  empty() {
    return this.values.length <= this.ptr;
  }

  atLeast(size: number) {
    return (this.ptr + size) <= this.values.length;
  }

  long() {
    return this.values[this.ptr++] || 0;
  }

  peek() {
    return this.values[this.ptr] || 0;
  }

  string() {
    return this.values[this.ptr++] || '';
  }

  split() {
    var word = this.long();
    this.bytes = [word % 0x100, (word >> 8) % 0x100, (word >> 16) % 0x100, (word >> 24) % 0x100];
  }

  short() {
    if (!this.bytes.length) {
      this.split();
    }

    return this.bytes.shift() + (this.bytes.shift() << 8);
  }

  byte() {
    if (!this.bytes.length) {
      this.split();
    }

    return this.bytes.shift();
  }

  byteArray(len: number) {
    const array = [];
    for (let i = 0; i < len; i++) {
      array.push(this.byte());
    }

    return array;
  }

  assert(size: number) {
    if (this.values.length < size) {
      throw `ComplexDataType Exception: Expected ${size} values but ${this.values.length} were supplied!`;
    }
  }

  sub(size: number) {
    var b = this.values.slice(this.ptr, this.ptr + size);
    this.ptr += size;
    return b;
  }

  clear() {
    this.bytes = [];
  }

  skip(size?: number) {
    this.ptr += size!;
    return this;
  }

  back(size: number) {
    this.ptr -= size;
    return this;
  }
}
