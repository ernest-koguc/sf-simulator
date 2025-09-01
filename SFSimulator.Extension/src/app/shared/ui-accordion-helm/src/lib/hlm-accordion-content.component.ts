import { ChangeDetectionStrategy, Component, ViewEncapsulation, computed, input } from '@angular/core';
import { BrnAccordionContentComponent } from '@spartan-ng/brain/accordion';
import { hlm } from '@spartan-ng/brain/core';
import type { ClassValue } from 'clsx';

@Component({
  selector: 'hlm-accordion-content',
  template: `
		<div [attr.inert]="_addInert()" style="overflow: hidden">
			<p class="flex flex-col gap-4 text-balance p-3 pr-1 overflow-y-auto"
        style="max-height: max(80vh, 800px); scrollbar-width: thin; scrollbar-gutter: stable; scrollbar-color: var(--color-purple-500) transparent;">
				<ng-content />
			</p>
		</div>
	`,
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None,
  host: {
    '[class]': '_computedClass()',
  },
})
export class HlmAccordionContentComponent extends BrnAccordionContentComponent {
  public readonly userClass = input<ClassValue>('', { alias: 'class' });
  protected readonly _computedClass = computed(() => {
    const gridRows = this.state() === 'open' ? 'grid-rows-[1fr]' : 'grid-rows-[0fr]';
    return hlm('text-sm transition-all grid', gridRows, this.userClass());
  });
}
