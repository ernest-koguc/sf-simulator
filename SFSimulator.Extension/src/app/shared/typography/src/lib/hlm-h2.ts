import { Directive } from '@angular/core';
import { classes } from '@spartan-ng/helm/utils';

export const hlmH2 =
  'scroll-m-20 text-lg font-semibold tracking-tight transition-colors';

@Directive({
  selector: '[hlmH2]',
  exportAs: 'hlmH2',
})
export class HlmH2 {
  constructor() {
    classes(() => hlmH2);
  }
}
