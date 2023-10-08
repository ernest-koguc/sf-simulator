import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'wordSpace'
})
export class WordSpacePipe implements PipeTransform {

  transform(data: Object) {
    if (typeof data != 'string') {
      return data;
    }
    let words = data.split(/(?=[A-Z])/);
    let wordsSpaced = words.join(' ');
    return wordsSpaced;
  }

}
