export function roundValues<T>(values: any): T {
  var k: keyof typeof values;
  for (k in values) {
    values[k] = +values[k].toFixed(3);
  }
  return values;
}
