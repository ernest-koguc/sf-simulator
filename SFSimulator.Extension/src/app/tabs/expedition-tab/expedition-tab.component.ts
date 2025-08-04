import { ChangeDetectionStrategy, Component, computed, inject } from '@angular/core';
import {
  HlmTabsComponent,
  HlmTabsListComponent,
  HlmTabsContentDirective,
  HlmTabsTriggerDirective
} from '@spartan-ng/helm/tabs';
import { ExpeditionEncounter } from '../../sfgame/SFGameModels';
import { SessionManager } from '../../services/SessionManager';

@Component({
  selector: 'expedition-tab',
  imports: [HlmTabsComponent, HlmTabsListComponent, HlmTabsContentDirective, HlmTabsTriggerDirective],
  templateUrl: './expedition-tab.component.html',
  styleUrl: './expedition-tab.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ExpeditionTabComponent {
  public sessionManager = inject(SessionManager);
  public expeditionProgress = computed(() => this.sessionManager.current()?.expeditionProgress);
  public expeditions = computed(() => this.sessionManager.current()?.expedition);

  public getAssetUrl(encounterId: ExpeditionEncounter) {
    return `https://sfsimulator.xyz/external/expedition/encounter${encounterId.toString()}.png`;

  }
}
