import { booleanAttribute, ChangeDetectionStrategy, Component, input } from '@angular/core';
import { HlmScrollArea } from '@spartan-ng/helm/scroll-area';
import { NgScrollbar, type ScrollbarAppearance } from 'ngx-scrollbar';

@Component({
  selector: 'tab-content',
  imports: [HlmScrollArea, NgScrollbar],
  template: `
    <ng-scrollbar hlm [appearance]="appearance()" class="h-115">
      <div class="pr-3" [class.h-full]="fullHeight()">
        <ng-content />
      </div>
    </ng-scrollbar>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TabContentComponent {
  readonly appearance = input<ScrollbarAppearance>('native');
  readonly fullHeight = input(false, { transform: booleanAttribute });
}
