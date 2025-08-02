import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ExpeditionService } from '../services/expedition.service';
import {
  HlmTabsComponent,
  HlmTabsListComponent,
  HlmTabsContentDirective,
  HlmTabsTriggerDirective
} from '@spartan-ng/helm/tabs';
import { ExpeditionEncounter } from '../sfgame/SFGameModels';

@Component({
  selector: 'expedition-preview',
  imports: [HlmTabsComponent, HlmTabsListComponent, HlmTabsContentDirective, HlmTabsTriggerDirective],
  templateUrl: './expedition-preview.component.html',
  styleUrl: './expedition-preview.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ExpeditionPreviewComponent {
  public expeditionService = inject(ExpeditionService);

  public getAssetUrl(encounterId: ExpeditionEncounter) {
    return `https://sfsimulator.xyz/external/expedition/encounter${encounterId.toString()}.png`;

  }
}
