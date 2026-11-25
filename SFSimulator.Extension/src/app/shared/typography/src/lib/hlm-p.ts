import { Directive } from '@angular/core';
import { classes } from '@spartan-ng/helm/utils';

export const hlmP = 'leading-5';

@Directive({
  selector: '[hlmP]',
  exportAs: 'hlmP',
})
export class HlmP {
  constructor() {
    classes(() => hlmP);
  }
}
