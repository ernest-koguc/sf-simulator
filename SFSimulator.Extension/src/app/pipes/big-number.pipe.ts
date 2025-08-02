import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'bigNumber'
})
export class BigNumberPipe implements PipeTransform {

  transform(value: unknown, ...args: unknown[]): unknown {
    if (typeof value !== 'number') {
      return value;
    }

    if (value >= 1e12) {
      return `${(value / 1e12).toFixed(0)}T`; // Trillions
    } else if (value >= 1e9) {
      return `${(value / 1e9).toFixed(0)}B`; // Billions
    } else if (value >= 1e6) {
      return `${(value / 1e6).toFixed(0)}M`; // Millions
    } else if (value >= 1e3) {
      return `${(value / 1e3).toFixed(0)}K`; // Thousands
    }

    return value;
  }
}
