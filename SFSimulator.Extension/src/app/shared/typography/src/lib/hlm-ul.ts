import { Directive } from '@angular/core';
import { classes } from '@spartan-ng/helm/utils';

export const hlmUl = 'ml-6 list-disc [&>li]:mt-0.5';

@Directive({
  selector: '[hlmUl]',
  exportAs: 'hlmUl',
})
export class HlmUl {
  constructor() {
    classes(() => hlmUl);
  }
}
