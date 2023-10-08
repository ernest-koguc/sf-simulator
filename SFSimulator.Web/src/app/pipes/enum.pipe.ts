import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'enum'
})
export class EnumPipe implements PipeTransform {

  transform(data: Object) {
    const allKeys = Object.keys(data);
    const keys = allKeys.slice(allKeys.length / 2); 
    const values = allKeys.slice(0, allKeys.length / 2);
    let keyValuePairs: { key: string, value: number }[] = [];
    for (let i = 0; i < allKeys.length / 2; i++) {
      keyValuePairs[i] = { key: keys[i], value: parseInt(values[i]) } 
    }

    return keyValuePairs;
  }
}
