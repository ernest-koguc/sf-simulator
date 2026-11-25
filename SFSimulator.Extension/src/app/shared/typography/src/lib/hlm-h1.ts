import { Directive } from '@angular/core';
import { classes } from '@spartan-ng/helm/utils';

export const hlmH1 = 'scroll-m-20 text-lg font-extrabold tracking-tight';

@Directive({
  selector: '[hlmH1]',
  exportAs: 'hlmH1',
})
export class HlmH1 {
  constructor() {
    classes(() => hlmH1);
  }
}
