import { ChangeDetectionStrategy, Component } from '@angular/core';
import { BrnAccordionContent } from '@spartan-ng/brain/accordion';
import { classes } from '@spartan-ng/helm/utils';

@Component({
	selector: 'hlm-accordion-content',
	exportAs: 'hlmAccordionContent',
	changeDetection: ChangeDetectionStrategy.OnPush,
	hostDirectives: [{ directive: BrnAccordionContent, inputs: ['style'] }],
	host: {
		'data-slot': 'accordion-content',
	},
	template: `
		<div
			class="flex flex-col gap-4 text-balance p-3 pr-1 overflow-y-auto"
			style="max-height: max(80vh, 800px); scrollbar-width: thin; scrollbar-gutter: stable; scrollbar-color: var(--color-purple-500) transparent;"
		>
			<ng-content />
		</div>
	`,
})
export class HlmAccordionContent {
	constructor() {
		classes(
			() =>
				'text-sm transition-all data-[state=closed]:h-0 data-[state=open]:h-(--brn-accordion-content-height)',
		);
	}
}
